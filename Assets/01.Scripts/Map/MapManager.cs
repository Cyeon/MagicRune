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

    #region Stage
    private List<Stage> _stageList = new List<Stage>();

    public int Stage => Floor - ((this.Chapter - 1) * 9);

    private int _floor = 0;
    public int Floor => _floor;
    #endregion

    private PortalSpawner _portalSpawner;
    public PortalSpawner PortalSpawner => _portalSpawner;

    public Sprite selectPortalSprite;

    private bool _isFirst = true;

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

    public void MapInit()
    {
        _mapSceneUI = Managers.Canvas.GetCanvas("MapUI").GetComponent<MapUI>();
        _chapterList = new List<Chapter>(Managers.Resource.Load<ChapterListSO>("SO/" + typeof(ChapterListSO).Name).chapterList);

        if (_portalSpawner == null)
        {
            PortalSpawner portalSpawner = Managers.Resource.Instantiate(typeof(PortalSpawner).Name, Managers.Scene.CurrentScene.transform).GetComponent<PortalSpawner>();
            portalSpawner.transform.SetParent(null);
            _portalSpawner = portalSpawner;
        }

        Managers.Reward.ImageLoad();

        if (_isFirst)
        {
            ChapterInit();
            _isFirst = false;
            _chapterList.ForEach(x => x.EnemyReset());
            _portalSpawner.SpawnPortal(_stageList[Stage].type);
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

        int idx = 0;
        foreach (var chance in _currentChapter.eventStagesChance)
        {
            Stage stage = new Stage();
            int random = Random.Range(1, 100);

            stage.Init(random <= chance ? StageType.Event : StageType.Attack, MapSceneUI.Stages[idx], idx);
            _stageList.Add(stage);

            idx++;
        }

        Stage bossStage = new Stage();
        bossStage.Init(StageType.Boss, MapSceneUI.Stages[9], 9);
        _stageList.Add(bossStage);

        _stageList[0].ChangeResource(Color.white);
    }

    public void NextStage()
    {
        if (Managers.GetPlayer() != null && Managers.GetPlayer().IsDie == true)
        {
            ResetChapter();
        }

        #region 초기화
        for (int i = 0; i < _stageList.Count; ++i)
        {
            MapSceneUI.Stages[i].sprite = _stageList[i].icon;
            MapSceneUI.Stages[i].color = _stageList[i].color;
        }
        Managers.Enemy.ResetEnemy();
        #endregion

        if (_stageList[Stage].type == StageType.Boss)
        {
            NextChapter();
            return;
        }

        MapSceneUI.StageList.transform.DOLocalMoveX(Stage * -300f, 0);
        if(_mapScene == null)
        {
            _mapScene = Managers.Scene.CurrentScene as MapScene;
        }

        if (_mapScene != null)
        {
            _mapScene?.ArrowImage.transform.SetParent(MapSceneUI.StageList.GetChild(Stage + 1));
            _mapScene.ArrowImage.transform.localPosition = new Vector3(0, _mapScene.ArrowImage.transform.localPosition.y, 0);
        }
        _stageList[Stage].ChangeResource(Color.white, selectPortalSprite);
        _floor += 1;

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.5f);
        if (Stage < MapSceneUI.StageList.childCount - 2)
            seq.Append(MapSceneUI.StageList.transform.DOLocalMoveX(Stage * -300f, 0.5f));
        seq.Append(MapSceneUI.Stages[Stage].DOColor(Color.white, 0.25f));
        seq.AppendCallback(() =>
        {
            _stageList[Stage].color = Color.white;
            _portalSpawner.SpawnPortal(_stageList[Stage].type);
        });
    }

    public void NextChapter()
    {
        if(Chapter < _chapterList.Count)
        {
            _chapter++;
        }

        ChapterInit();
        _portalSpawner.SpawnPortal(_stageList[Stage].type);
        _mapSceneUI.ChangeBackground();
    }

    public void ResetChapter()
    {
        _chapter = 1;
        ChapterInit();
        Managers.GetPlayer().ResetHealth();
    }
}
