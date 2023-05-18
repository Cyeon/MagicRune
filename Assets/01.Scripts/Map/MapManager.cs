using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public enum HalfType
{
    First,
    Second,
    Boss
}

public class MapManager
{
    #region Chapter
    private List<Chapter> _chapterList = new List<Chapter>();

    private int _chapter = 1;
    public int Chapter => _chapter;
    private Chapter _currentChapter = null;
    public Chapter CurrentChapter => _currentChapter;
    #endregion

    #region Half
    private List<StageType> _firstHalfStageList = new List<StageType>(); // 전반부
    private List<StageType> _secondHalfStageList = new List<StageType>(); // 후반부

    private int _halfProgress = 0; // 현재 진행도
    private int _nextCondition = 4; // 다음 단계로 넘어가는 조건 스테이지 개수

    private HalfType _halfType = HalfType.First;
    public HalfType CurrentHalfType => _halfType;
    #endregion

    #region Stage
    private List<Stage> _stageList = new List<Stage>();
    #endregion 

    private int _floor = 0;
    public int Floor => _floor;

    private StageSpawner _stageSpawner;
    public StageSpawner StageSpawner => _stageSpawner;

    private MapUI _mapSceneUI;
    public MapUI MapSceneUI
    {
        get
        {
            if (_mapSceneUI == null)
            {
                _mapSceneUI = Managers.Canvas.GetCanvas("MapUI")?.GetComponent<MapUI>();
            }
            return _mapSceneUI;
        }
    }

    private MapScene _mapScene;

    private bool _isFirst = true;

    #region Adventure
    private bool _isAdventure = false;
    public bool IsAdventureWar {get => _isAdventure; set { _isAdventure = value;} }
    private string _adventureResultText;
    public string AdventureResultText => _adventureResultText;
    #endregion

    #region Init
    public void MapInit()
    {
        _mapSceneUI = Managers.Canvas.GetCanvas("MapUI").GetComponent<MapUI>();
        _chapterList = new List<Chapter>(Managers.Resource.Load<ChapterListSO>("SO/" + typeof(ChapterListSO).Name).chapterList);

        if (_stageSpawner == null)
        {
            StageSpawner portalSpawner = Managers.Resource.Instantiate(typeof(StageSpawner).Name, Managers.Scene.CurrentScene.transform).GetComponent<StageSpawner>();
            portalSpawner.transform.SetParent(null);
            _stageSpawner = portalSpawner;
        }

        Managers.Reward.ImageLoad();
        _mapSceneUI.ChapterTransition.Init();

        if (_isFirst)
        {
            ChapterInit();
            _isFirst = false;
            _chapterList.ForEach(x => x.EnemyReset());
            _mapSceneUI.ChapterTransition.Transition();
        }
        else if (_isAdventure)
        {
            Managers.Canvas.GetCanvas("Adventure").enabled = true;
        }
        else
        {
            NextStage();
        }
    }

    private void ChapterInit()
    {
        _stageList.Clear();
        _currentChapter = _chapterList[Chapter - 1];

        bool isAttack = false;

        // 전반부 세팅
        _firstHalfStageList.Add(StageType.Attack);
        _firstHalfStageList.Add(StageType.Attack);
        _firstHalfStageList.Add(StageType.Attack);
        _firstHalfStageList.Add(StageType.Adventure);
        _firstHalfStageList.Add(StageType.Adventure);

        if (IsFiftyChance())
        {
            _firstHalfStageList.Add(IsFiftyChance() ? StageType.Shop : StageType.Rest);
            isAttack = AttackOrAdventure(_firstHalfStageList);
        }
        else
        {
            isAttack = AttackOrAdventure(_firstHalfStageList);
            if (IsFiftyChance())
            {
                _firstHalfStageList.Add(IsFiftyChance() ? StageType.Shop : StageType.Rest);
            }
            else
            {
                isAttack = AttackOrAdventure(_firstHalfStageList);
            }
        }

        // 후반부 세팅

        _secondHalfStageList.Add(StageType.Attack);
        _secondHalfStageList.Add(StageType.Attack);
        _secondHalfStageList.Add(StageType.Attack);
        _secondHalfStageList.Add(StageType.Adventure);
        _secondHalfStageList.Add(StageType.Adventure);
        
        if(!_firstHalfStageList.Contains(StageType.Rest) && !_firstHalfStageList.Contains(StageType.Shop))
        {
            _secondHalfStageList.Add(StageType.Rest);
            _secondHalfStageList.Add(StageType.Shop);
        }
        else
        {
            if(_firstHalfStageList.Contains(StageType.Rest))
            {
                _secondHalfStageList.Add(StageType.Shop);
            }
            else
            {
                _secondHalfStageList.Add(StageType.Rest);
            }

            _secondHalfStageList.Add(isAttack ? StageType.Adventure : StageType.Attack);
        }
        
        FirstHalf();
    
    }
    #endregion

    #region Next
    public void NextChapter()
    {
        if (Chapter < _chapterList.Count)
        {
            _chapter++;
        }

        ChapterInit();
        _mapSceneUI.ChangeBackground();
        _mapSceneUI.ChapterTransition.Transition();
    }

    public void NextStage()
    {
        if (Managers.GetPlayer() != null && Managers.GetPlayer().IsDie == true)
        {
            ResetChapter();
        }

        Managers.Enemy.ResetEnemy();

        if (CurrentHalfType == HalfType.Boss)
        {
            NextChapter();
            return;
        }

        if(_mapScene == null)
        {
            _mapScene = Managers.Scene.CurrentScene as MapScene;
        }

        _floor++;

        _halfProgress++;
        if(_halfProgress == _nextCondition)
        {
            if (CurrentHalfType == HalfType.First) SecondHalf();
            else if (CurrentHalfType == HalfType.Second) BossHalf();
        }
    }
    #endregion

    #region Half
    private void HalfReset()
    {
        _stageList.ForEach(x => Managers.Resource.Destroy(x.gameObject));
        _stageList.Clear();

        _halfProgress = 0;
    }

    public void FirstHalf()
    {
        HalfReset();

        for(int i = 0; i < _firstHalfStageList.Count; i++)
        {
            Stage stage = StageSpawner.SpawnStage(_firstHalfStageList[i]);
            _stageList.Add(stage);
        }
    }

    public void SecondHalf()
    {
        HalfReset();

        for (int i = 0; i < _secondHalfStageList.Count; i++)
        {
            Stage stage = StageSpawner.SpawnStage(_secondHalfStageList[i]);
            _stageList.Add(stage);
        }
    }

    private void BossHalf()
    {
        HalfReset();

        Stage stage = StageSpawner.SpawnStage(StageType.Boss);
        _stageList.Add(stage);
    }

    /// <summary>
    /// 공격 혹은 모험 스테이지를 넣는다. 공격이면 true 모험이면 false
    /// </summary>
    /// <param name="half"></param>
    /// <returns></returns>
    private bool AttackOrAdventure(List<StageType> half)
    {
        if (IsFiftyChance())
        {
            half.Add(StageType.Attack);
            return true;
        }
        else
        {
            half.Add(StageType.Adventure);
            return false;
        }
    }
    #endregion

    public void ResetChapter()
    {
        _chapter = 1;
        ChapterInit();
        Managers.GetPlayer().ResetHealth();
    }

    public void AdventureWar(string text)
    {
        _isAdventure = true;
        _adventureResultText = text;
    }


    /// <summary>
    /// 50% 확률
    /// </summary>
    /// <returns></returns>
    private bool IsFiftyChance()
    {
        return Random.Range(0, 2) == 0;
    }

    public List<Stage> GetStageList()
    {
        return _stageList;
    }
}
