using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public enum PeriodType
{
    None,
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

    #region Period
    private List<StageType> _firstPeriodStageList = new List<StageType>(); // 전반부
    private List<StageType> _secondPeriodStageList = new List<StageType>(); // 후반부

    private List<StageType> _currentPeriodStageList = new List<StageType>(); // 현재 단계
    public List<StageType> CurrentPeriodStageList => _currentPeriodStageList;

    private int _periodProgress = 0; // 현재 진행도
    private int _nextCondition = 4; // 다음 단계로 넘어가는 조건 스테이지 개수

    private PeriodType _periodType = PeriodType.None;
    public PeriodType CurrentPeriodType => _periodType;
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
    public MapScene MapScene
    {
        get
        {
            if (_mapScene == null)
            {
                _mapScene = Managers.Scene.CurrentScene as MapScene;
            }
            return _mapScene;
        }
    }

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
        _currentPeriodStageList.Clear();
        _currentChapter = _chapterList[Chapter - 1];

        _periodType = PeriodType.None;

        bool isAttack = false;

        // 전반부 세팅
        _firstPeriodStageList.Add(StageType.Attack);
        _firstPeriodStageList.Add(StageType.Attack);
        _firstPeriodStageList.Add(StageType.Attack);
        _firstPeriodStageList.Add(StageType.Adventure);
        _firstPeriodStageList.Add(StageType.Adventure);

        if (IsFiftyChance())
        {
            _firstPeriodStageList.Add(IsFiftyChance() ? StageType.Shop : StageType.Rest);
            isAttack = AttackOrAdventure(_firstPeriodStageList);
        }
        else
        {
            isAttack = AttackOrAdventure(_firstPeriodStageList);
            if (IsFiftyChance())
            {
                _firstPeriodStageList.Add(IsFiftyChance() ? StageType.Shop : StageType.Rest);
            }
            else
            {
                isAttack = AttackOrAdventure(_firstPeriodStageList);
            }
        }

        // 후반부 세팅

        _secondPeriodStageList.Add(StageType.Attack);
        _secondPeriodStageList.Add(StageType.Attack);
        _secondPeriodStageList.Add(StageType.Attack);
        _secondPeriodStageList.Add(StageType.Adventure);
        _secondPeriodStageList.Add(StageType.Adventure);
        
        if(!_firstPeriodStageList.Contains(StageType.Rest) && !_firstPeriodStageList.Contains(StageType.Shop))
        {
            _secondPeriodStageList.Add(StageType.Rest);
            _secondPeriodStageList.Add(StageType.Shop);
        }
        else
        {
            if(_firstPeriodStageList.Contains(StageType.Rest))
            {
                _secondPeriodStageList.Add(StageType.Shop);
            }
            else
            {
                _secondPeriodStageList.Add(StageType.Rest);
            }

            _secondPeriodStageList.Add(isAttack ? StageType.Adventure : StageType.Attack);
        }
        NextPeriod();
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
        MapScene.mapDial.gameObject.SetActive(true);

        MapScene.mapDial.Clear();
        MapScene.mapDial.MapStageSpawn();

        if (Managers.GetPlayer() != null && Managers.GetPlayer().IsDie == true)
        {
            ResetChapter();
        }

        Managers.Enemy.ResetEnemy();

        if (CurrentPeriodType == PeriodType.Boss)
        {
            NextChapter();
            return;
        }

        _floor++;

        _periodProgress++;
        if(_periodProgress == _nextCondition)
        {
            NextPeriod();
        }
    }
    #endregion

    #region Period
    public void NextPeriod()
    {
        _currentPeriodStageList.Clear();

        _periodProgress = 0;
        _periodType++;

        switch (CurrentPeriodType)
        {
            case PeriodType.First:
                _currentPeriodStageList = _firstPeriodStageList;
                break;

            case PeriodType.Second:
                _currentPeriodStageList = _secondPeriodStageList;
                break;

            case PeriodType.Boss:
                _currentPeriodStageList.Add(StageType.Boss);
                break;
        }
    }

    /// <summary>
    /// 공격 혹은 모험 스테이지를 넣는다. 공격이면 true 모험이면 false
    /// </summary>
    /// <param name="period"></param>
    /// <returns></returns>
    private bool AttackOrAdventure(List<StageType> period)
    {
        if (IsFiftyChance())
        {
            period.Add(StageType.Attack);
            return true;
        }
        else
        {
            period.Add(StageType.Adventure);
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
}
