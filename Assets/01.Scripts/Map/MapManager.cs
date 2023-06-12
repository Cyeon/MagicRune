using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using MyBox;

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
    private List<StageType> _firstPeriodStageList = new List<StageType>(); // ??ш끽維곻쭚??딅텑?
    private List<StageType> _secondPeriodStageList = new List<StageType>(); // ??ш끽維곻쭚??딅텑?

    private List<StageType> _currentPeriodStageList = new List<StageType>(); // ??ш끽維????影?됀?
    public List<StageType> CurrentPeriodStageList => _currentPeriodStageList;

    private int _periodProgress = 0; // ??ш끽維??癲ル슣???몄춿??
    private int _nextCondition = 4; // ???源낆쓱 ??影?됀嚥?肉???筌뚯슦苑??좊읈????釉뚰???쨨?????읐??? ??좊즵獒??

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
    public bool isTutorial = true;

    #region Adventure
    private bool _isAdventure = false;
    public bool IsAdventureWar { get => _isAdventure; set { _isAdventure = value; } }
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
        _firstPeriodStageList.Clear();
        _secondPeriodStageList.Clear();

        _currentChapter = _chapterList[Chapter - 1];

        _periodType = PeriodType.None;

        bool isAttack = false;

        if(isTutorial)
        {
            _firstPeriodStageList.Add(StageType.Tutorial);
            _firstPeriodStageList.Add(StageType.Tutorial);
            _firstPeriodStageList.Add(StageType.Tutorial);
            _firstPeriodStageList.Add(StageType.Tutorial);
            NextPeriod();
            return;
        }

        // First Period
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
        _firstPeriodStageList.Shuffle();

        // Second Period
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
        _secondPeriodStageList.Shuffle();

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
        Managers.Canvas.GetCanvas("MapDial").enabled = true;
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
        MapScene.CompousProgress((float)_periodProgress / _nextCondition);

        if(_periodProgress == _nextCondition)
        {
            NextPeriod();
            MapScene.mapDial.Clear();
            MapScene.mapDial.MapStageSpawn();
        }
    }
    #endregion

    #region Period
    public void NextPeriod()
    {
        _currentPeriodStageList.Clear();

        _periodProgress = 0;
        _periodType++;

        MapScene?.CompousProgress(0);

        switch (CurrentPeriodType)
        {
            case PeriodType.First:
                _currentPeriodStageList = _firstPeriodStageList.ToList();
                break;

            case PeriodType.Second:
                _currentPeriodStageList = _secondPeriodStageList.ToList();
                break;

            case PeriodType.Boss:
                _currentPeriodStageList.Add(StageType.Boss);
                break;
        }

    }

    /// <summary>
    /// ??????????? 癲ル슢?꾤땟洹잕덩?????읐??????壤굿??レ툗?? ??????????true 癲ル슢?꾤땟洹잕덩?????false
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
        _floor = 0;
        _isFirst = true;
        isTutorial = true;
        ChapterInit();
        Managers.GetPlayer().ResetHealth();
    }

    public void AdventureWar(string text)
    {
        _isAdventure = true;
        _adventureResultText = text;
    }


    /// <summary>
    /// 50% ?嶺뚮㉡???
    /// </summary>
    /// <returns></returns>
    private bool IsFiftyChance()
    {
        return Random.Range(0, 2) == 0;
    }
}
