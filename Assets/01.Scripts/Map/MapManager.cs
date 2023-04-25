using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class MapManager
{
    #region Chapter
    private List<Chapter> chapterList = new List<Chapter>();

    private int _chapter = 1;
    public int Chapter => _chapter;
    private Chapter _currentChapter = null;
    public Chapter CurrentChapter => _currentChapter;
    #endregion

    #region Stage
    private List<Stage> stageList = new List<Stage>();

    [SerializeField]   private int Stage => Floor - ((this.Chapter - 1) * 9);

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
        _mapSceneUI = Managers.Canvas.GetCanvas("MapUI")?.GetComponent<MapUI>();
        chapterList = new List<Chapter>(Managers.Resource.Load<ChapterListSO>("SO/" + typeof(ChapterListSO).Name).chapterList);

        if (_portalSpawner == null)
        {
            PortalSpawner portalSpawner = Managers.Resource.Instantiate(typeof(PortalSpawner).Name, Managers.Scene.CurrentScene.transform).GetComponent<PortalSpawner>();
            portalSpawner.transform.SetParent(null);
            _portalSpawner = portalSpawner;
        }

        ChapterInit();
        _portalSpawner.ResetEnemyEnter();
        _portalSpawner.SpawnPortal(stageList[Stage].type);

        Managers.Reward.ImageLoad();
    }

    private void ChapterInit()
    {
        stageList.Clear();
        _currentChapter = chapterList[Chapter - 1];

        int idx = 0;
        foreach (var chance in _currentChapter.eventStagesChance)
        {
            Stage stage = new Stage();
            int random = Random.Range(1, 100);

            stage.Init(random <= chance ? StageType.Event : StageType.Attack, MapSceneUI.Stages[idx], idx);
            stageList.Add(stage);

            idx++;
        }

        Stage bossStage = new Stage();
        bossStage.Init(StageType.Boss, MapSceneUI.Stages[9], 9);
        stageList.Add(bossStage);

        stageList[0].ChangeResource(Color.white);
    }

    public void NextStage()
    {
        if (Managers.GetPlayer() != null && Managers.GetPlayer().IsDie == true)
        {
            ResetChapter();
            // 플레이어 죽음 리셋
        }

        if (_isFirst)
        {
            _isFirst = false;
            _portalSpawner.ResetEnemyEnter();
            return;
        }

        #region 초기화 부분
        for (int i = 0; i < stageList.Count; ++i)
        {
            MapSceneUI.Stages[i].sprite = stageList[i].icon;
            MapSceneUI.Stages[i].color = stageList[i].color;
        }
        Managers.Enemy.ResetEnemy();
        #endregion

        if (stageList[Stage].type == StageType.Boss)
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
        stageList[Stage].ChangeResource(Color.white, selectPortalSprite);
        _floor += 1;

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.5f);
        if (Stage < MapSceneUI.StageList.childCount - 2)
            seq.Append(MapSceneUI.StageList.transform.DOLocalMoveX(Stage * -300f, 1f));
        seq.Append(MapSceneUI.Stages[Stage].DOColor(Color.white, 0.5f));
        seq.AppendCallback(() =>
        {
            stageList[Stage].color = Color.white;
            _portalSpawner.SpawnPortal(stageList[Stage].type);
        });
    }

    public void NextChapter()
    {
        if(Chapter < chapterList.Count)
        {
            _chapter++;
        }

        ChapterInit();
        _portalSpawner.SpawnPortal(stageList[Stage].type);
    }

    public void ResetChapter()
    {
        _chapter = 1;
        ChapterInit();
        Managers.GetPlayer().ResetHealth();
    }
}
