using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using static MapDefine;

public class MapManager : MonoSingleton<MapManager>
{
    [Header("Chapter")]
    public List<Chapter> chapterList = new List<Chapter>();

    private int _chapter = 1;
    public int Chapter => _chapter;
    private Chapter _currentChapter = null;
    public Chapter CurrentChapter => _currentChapter;

    [Header("Stage")]
    public List<Stage> stageList = new List<Stage>();
 [SerializeField]   private int Stage => Floor - ((this.Chapter - 1) * 9);

    private int _floor = 0;
    public int Floor => _floor;

    [Header("Portal")]
    public Dictionary<StageType, List<Portal>> stageOfPortalDic = new Dictionary<StageType, List<Portal>>();
    public Portal selectPortal;

    [Header("Attack")]
    public EnemySO selectEnemy;
    public AttackMapListSO attackMap;

    private bool _isFirst = true;

    private MapUI _mapSceneUI;
    public MapUI MapSceneUI
    {
        get
        {
            if (_mapSceneUI == null)
            {
                _mapSceneUI = CanvasManager.Instance.GetCanvas("MapUI")?.GetComponent<MapUI>();
            }
            return _mapSceneUI;
        }
    }

    private void Awake()
    {
        stageOfPortalDic.Add(StageType.Attack, new List<Portal>());
        stageOfPortalDic.Add(StageType.Boss, new List<Portal>());
        stageOfPortalDic.Add(StageType.Event, new List<Portal>());
        //stageOfPortalDic.Add(StageType.Rest, new List<Portal>());

        Transform atkTrm = transform.Find("Attack");
        for (int i = 0; i < atkTrm.childCount; ++i)
        {
            stageOfPortalDic[StageType.Attack].Add(atkTrm.GetChild(i).GetComponent<Portal>());
        }

        stageOfPortalDic[StageType.Boss].Add(atkTrm.GetChild(1).GetComponent<Portal>());

        atkTrm = transform.Find("Event");
        for (int i = 0; i < atkTrm.childCount; ++i)
        {
            if (atkTrm.GetChild(i).gameObject.activeSelf == false) continue;
            stageOfPortalDic[StageType.Event].Add(atkTrm.GetChild(i).GetComponent<Portal>());
        }

        //atkTrm = transform.Find("Rest");
        //for (int i = 0; i < atkTrm.childCount; ++i)
        //{
        //    if (atkTrm.GetChild(i).gameObject.activeSelf == false) continue;
        //    stageOfPortalDic[StageType.Rest].Add(atkTrm.GetChild(i).GetComponent<Portal>());
        //}
    }

    private void Start()
    {
        _mapSceneUI = CanvasManager.Instance.GetCanvas("MapUI").GetComponent<MapUI>();

        ChapterInit();
        PortalInit();
        MapSceneUI?.InfoUIReload();
        //CanvasManager.instance.GetCanvas("MapUI").GetComponent<MapUI>().InfoUIReload();

        RewardManager.ImageLoad();
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

            stage.Init(random <= chance ? StageType.Event : StageType.Attack, MapSceneUI.stages[idx], idx);
            stageList.Add(stage);

            idx++;
        }

        Stage bossStage = new Stage();
        bossStage.Init(StageType.Boss, MapSceneUI.stages[9], 9);
        stageList.Add(bossStage);

        stageList[0].ChangeResource(Color.white);
    }

    private void PortalInit()
    {
        MapSceneUI.PortalEffectUp(stageList[Stage].type);

        float time = 1.2f;

        if (stageList[Stage].type == StageType.Attack)
        {
            foreach(var portal in MapSceneUI.portals)
            {
                StartCoroutine(portal.Effecting(time));
                portal.Init(SpawnPortal(stageList[Stage].type));
                portal.transform.DOScale(1f, 0.8f);
            }
        }
        //else if (stageList[Stage].type == StageType.Rest)
        //{
        //    MapSceneUI.portals[0].Init(SpawnPortal(stageList[Stage].type));
        //    MapSceneUI.portals[0].transform.DOScale(2f, 1f);

        //    MapSceneUI.portals[2].Init(SpawnPortal(stageList[Stage].type));
        //    MapSceneUI.portals[2].transform.DOScale(2f, 1f);
        //}
        else
        {
            StartCoroutine(MapSceneUI.portals[1].Effecting(1.8f));
            MapSceneUI.portals[1].Init(SpawnPortal(stageList[Stage].type));
            MapSceneUI.portals[1].transform.DOScale(2f, 1f);
        }
    }

    public void NextStage()
    {
        if (GameManager.Instance.player.IsDie == true) // 버그
        {
            ResetChapter();
            // 플레이어 죽음 리셋
        }

        MapSceneUI.InfoUIReload();

        if (_isFirst)
        {
            _isFirst = false;
            return;
        }

        #region 초기화 부분
        if (stageList[Stage].type != StageType.Event)
        {
            DOTween.KillAll();

            foreach (var portal in stageOfPortalDic)
            {
                if (portal.Value.Count > 0)
                    portal.Value.ForEach(e =>
                    {
                        if (e != null)
                            e.isUse = false;
                    });
            }
        }
        else
        {
            MapSceneUI.ResetPortal(stageList[Stage].type);
        }

        for (int i = 0; i < stageList.Count; ++i)
        {
            MapSceneUI.stages[i].sprite = stageList[i].icon;
            MapSceneUI.stages[i].color = stageList[i].color;
        }
        #endregion

        if (stageList[Stage].type == StageType.Boss)
        {
            NextChapter();
            return;
        }

        MapSceneUI.StageList.transform.DOLocalMoveX(Stage * -300f, 0);
        stageList[Stage].ChangeResource(Color.white, selectPortal.icon);
        _floor += 1;

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.5f);
        seq.Append(MapSceneUI.StageList.transform.DOLocalMoveX(Stage * -300f, 1f));
        seq.Append(MapSceneUI.stages[Stage].DOColor(Color.white, 0.5f));
        seq.AppendCallback(() =>
        {
            stageList[Stage].color = Color.white;
            PortalInit();
        });
    }

    public void NextChapter()
    {
        if(Chapter < chapterList.Count)
        {
            _chapter++;
        }

        ChapterInit();
        PortalInit();
    }

    private Portal SpawnPortal(StageType type)
    {
        List<Portal> pList = stageOfPortalDic[type].Where(e => e.isUse == false).ToList();
        if(pList.Count == 0)
        {
            Debug.LogError("Not Found Portal Object");
            return null;
        }

        Portal portal = null;

        int cnt = pList.Count;
        portal = pList[Random.Range(0, cnt)];
        switch (type)

        {
            case StageType.Attack:
            case StageType.Boss:
                AttackPortal atkPortal = portal as AttackPortal;
                atkPortal.enemy = type == StageType.Attack ? GetAttackEnemy() : _currentChapter.boss;
                atkPortal.icon = atkPortal.enemy.icon;
                atkPortal.portalName = atkPortal.enemy.enemyName;
                atkPortal.isUse = true;
                return atkPortal;

            //case StageType.Rest:
            //    return portal;
            case StageType.Event:
                return portal;
        }

        return null;
    }

    private EnemySO GetAttackEnemy()
    {
        List<EnemySO> enemyList = new List<EnemySO>();
        for (int i = 0; i < attackMap.map.Count; ++i)
        {
            if (attackMap.map[i].MinFloor <= Floor + 1 && attackMap.map[i].MaxFloor >= Floor + 1)
            {
                foreach (var enemy in attackMap.map[i].enemyList)
                {
                    if (!enemy.IsEnter) enemyList.Add(enemy);
                }
            }
        }

        int idx = Random.Range(0, enemyList.Count);
        if (enemyList.Count == 0)
        {
            return attackMap.map[0].enemyList[0];
        }
        enemyList[idx].IsEnter = true;
        return enemyList[idx];
    }

    public void ResetChapter()
    {
        _chapter = 1;
        ChapterInit();
        GameManager.Instance.player.ResetHealth();
    }
}
