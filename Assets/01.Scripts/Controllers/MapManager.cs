using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using MyBox;
using System;

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
    private List<StageType> _firstPeriodStageList = new List<StageType>(); // ????썹땟怨살춾???낇뀘?
    private List<StageType> _secondPeriodStageList = new List<StageType>(); // ????썹땟怨살춾???낇뀘?

    private List<StageType> _currentPeriodStageList = new List<StageType>(); // ????썹땟????壤굿????
    public List<StageType> CurrentPeriodStageList => _currentPeriodStageList;

    private int _periodProgress = 0; // ????썹땟???꿔꺂????紐꾩뗄??
    private int _nextCondition = 7; // ?꾨컲/?꾨컲遺 ?꾪솚 ?섎젮硫?紐?媛쒖쓽 ?ㅽ뀒?댁?瑜??대━???댁빞?섎뒗吏 

    private PeriodType _periodType = PeriodType.None;
    public PeriodType CurrentPeriodType => _periodType;
    #endregion

    private int _stage = 0;
    public int Stage => _stage;

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

    private bool _isFirst = true;

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
        Managers.GetPlayer()?.ResetHealth();

        _currentPeriodStageList.Clear();
        _firstPeriodStageList.Clear();
        _secondPeriodStageList.Clear();

        _currentChapter = _chapterList[Chapter - 1];

        _periodType = PeriodType.None;

        bool isAttack = false;

        if(Define.SaveData.IsTutorial)
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
        _firstPeriodStageList.Add(StageType.Attack); // ?섏쨷???섎━??諛⑹쑝濡?蹂寃?
        _firstPeriodStageList.Add(StageType.Adventure);
        _firstPeriodStageList.Add(StageType.Adventure);
        _firstPeriodStageList.Add(StageType.Adventure);
        _firstPeriodStageList.Add(StageType.Rest);

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

        // Second Period?
        _secondPeriodStageList.Add(StageType.Attack);
        _secondPeriodStageList.Add(StageType.Attack);
        _secondPeriodStageList.Add(StageType.Attack);
        _secondPeriodStageList.Add(StageType.Attack); // ?섏쨷???섎━??諛⑹쑝濡?蹂寃?
        _secondPeriodStageList.Add(StageType.Adventure);
        _secondPeriodStageList.Add(StageType.Adventure);
        _secondPeriodStageList.Add(StageType.Adventure);
        _secondPeriodStageList.Add(StageType.Rest);

        if (!_firstPeriodStageList.Contains(StageType.Rest) && !_firstPeriodStageList.Contains(StageType.Shop))
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
            _stage = 0;
            ChapterInit();
            _mapSceneUI.ChangeBackground();
            _mapSceneUI.ChapterTransition.Transition();
        }
        else
        {
            Managers.Scene.LoadScene("EndScene");
        }
    }

    public void NextStage()
    {
        Managers.Canvas.GetCanvas("MapDial").enabled = true;
        Define.MapScene.mapDial.gameObject.SetActive(true);

        Define.MapScene.mapDial.Clear();
        Define.MapScene.mapDial.MapStageSpawn();

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

        _stage++;

        _periodProgress++;
        Define.MapScene.CompousProgress((float)_periodProgress / _nextCondition);

        if(_periodProgress == _nextCondition)
        {
            NextPeriod();
            Define.MapScene.mapDial.Clear();
            Define.MapScene.mapDial.MapStageSpawn();
        }

    }
    #endregion

    #region Period
    public void NextPeriod()
    {
        _currentPeriodStageList.Clear();

        _periodProgress = 0;
        _periodType++;

        Define.MapScene?.CompousProgress(0);

        switch (CurrentPeriodType)
        {
            case PeriodType.First:
                _currentPeriodStageList = _firstPeriodStageList.ToList();
                Define.MapScene?.FirstProgress();
                break;

            case PeriodType.Second:
                _currentPeriodStageList = _secondPeriodStageList.ToList();
                Define.MapScene?.SecondProgress();
                break;

            case PeriodType.Boss:
                _currentPeriodStageList.Add(StageType.Boss);
                Define.MapScene?.BossProgress();
                break;
        }

    }

    /// <summary>
    /// ??????????? ?꿔꺂??袁ㅻ븶域뱀옎???????먃??????鶯ㅺ동????ы닓?? ??????????true ?꿔꺂??袁ㅻ븶域뱀옎??????false
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
        _stage = 0;
        _isFirst = true;
        ChapterInit();
        Managers.GetPlayer()?.ResetHealth();
    }

    public void AdventureWar(string text)
    {
        _isAdventure = true;
        _adventureResultText = text;
    }


    /// <summary>
    /// 50% ?癲ル슢????
    /// </summary>
    /// <returns></returns>
    private bool IsFiftyChance()
    {
        return UnityEngine.Random.Range(0, 2) == 0;
    }

    public void Tutorial()
    {
        Define.SaveData.IsTutorial = true;
        ChapterInit();
        _isFirst = false;
        _chapterList.ForEach(x => x.EnemyReset());
        _mapSceneUI.ChapterTransition.Transition();
    }
}
