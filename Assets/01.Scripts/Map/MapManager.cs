using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

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
    #endregion

    #region Stage
    private List<Stage> _stageList = new List<Stage>();
    public int Stage => Floor - ((this.Chapter - 1) * 9);
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

        _firstHalfStageList.Add(StageType.Attack);
        _firstHalfStageList.Add(StageType.Attack);
        _firstHalfStageList.Add(StageType.Adventure);

        if(IsFiftyChance())
        {
            _firstHalfStageList.Add(IsFiftyChance() ? StageType.Shop : StageType.Rest);
            _firstHalfStageList.Add(IsFiftyChance() ? StageType.Attack : StageType.Adventure);
        }
        else
        {
            _firstHalfStageList.Add(IsFiftyChance() ? StageType.Attack : StageType.Adventure);
            if (IsFiftyChance())
            {
                _firstHalfStageList.Add(IsFiftyChance() ? StageType.Shop : StageType.Rest);
            }
            else
            {
                _firstHalfStageList.Add(IsFiftyChance() ? StageType.Attack : StageType.Adventure);
            }
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

        if (_stageList[Stage].type == StageType.Boss)
        {
            NextChapter();
            return;
        }

        if(_mapScene == null)
        {
            _mapScene = Managers.Scene.CurrentScene as MapScene;
        }

        _floor += 1;
    }
    #endregion

    #region Half
    public void FirstHalf()
    {
        _stageList.ForEach(x => Managers.Resource.Destroy(x.gameObject));
        _stageList.Clear();

        for(int i = 0; i < _firstHalfStageList.Count; i++)
        {
            Stage stage = StageSpawner.SpawnStage(_firstHalfStageList[i]);
            _stageList.Add(stage);
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
