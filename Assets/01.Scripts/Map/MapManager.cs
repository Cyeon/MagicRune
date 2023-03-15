using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MapManager : MonoSingleton<MapManager>
{
    [Header("Chapter")]
    public List<Chapter> chapterList = new List<Chapter>();

    private int _chapter = 1;
    public int Chapter => _chapter;
    private Chapter _currentChapter = null;

    [Header("Stage")]
    public List<Stage> stageList = new List<Stage>();
    private int Stage => Floor - ((this.Chapter - 1) * 10);

    private int _floor = 0;
    public int Floor => _floor;

    [Header("Portal")]
    public Dictionary<StageType, List<Portal>> stageOfPortalDic = new Dictionary<StageType, List<Portal>>();
    public Portal selectPortal;

    [Header("Attack")]
    public EnemySO selectEnemy;
    public AttackMapListSO attackMap;

    [HideInInspector]
    public MapUI ui;

    private bool _isFirst = true;

    private void Awake()
    {
        stageOfPortalDic.Add(StageType.Attack, new List<Portal>());
        stageOfPortalDic.Add(StageType.Boss, new List<Portal>());
        stageOfPortalDic.Add(StageType.Event, new List<Portal>());

        Transform atkTrm = transform.Find("Attack");
        for (int i = 0; i < atkTrm.childCount; ++i)
        {
            stageOfPortalDic[StageType.Attack].Add(atkTrm.GetChild(i).GetComponent<AttackPortal>());
        }

        stageOfPortalDic[StageType.Boss].Add(atkTrm.GetComponent<AttackPortal>());
        stageOfPortalDic[StageType.Event].Add(atkTrm.GetComponent<AttackPortal>());
    }

    private void Start()
    {
        ChapterInit();
        PortalInit();
        ui.InfoUIReload();
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

            stage.Init(random <= chance ? StageType.Event : StageType.Attack, ui.stages[idx], idx);
            stageList.Add(stage);

            idx++;
        }

        Stage bossStage = new Stage();
        bossStage.Init(StageType.Boss, ui.stages[9], 9);
        stageList.Add(bossStage);

        stageList[0].ChangeResource(Color.white);
    }

    private void PortalInit()
    {
        ui.PortalEffectUp(stageList[Stage].type);

        if (stageList[Stage].type == StageType.Attack)
        {
            foreach(var portal in ui.portals)
            {
                portal.Init(SpawnPortal(stageList[Stage].type));
                portal.transform.DOScale(1f, 0.8f);
            }
        }
        else
        {
            ui.portals[1].Init(SpawnPortal(stageList[Stage].type));
            ui.portals[1].transform.DOScale(2f, 1f);
        }
    }

    public void NextStage()
    {
        if(_isFirst)
        {
            _isFirst = false;
            return;
        }

        #region 초기화 부분
        DOTween.KillAll();

        foreach(var portal in stageOfPortalDic)
        {
            if (portal.Value.Count > 0)
                portal.Value.ForEach(e =>
                {
                    if(e != null)
                        e.isUse = false;
                });
        }

        for(int i = 0; i < stageList.Count; ++i)
        {
            ui.stages[i].sprite = stageList[i].icon;
            ui.stages[i].color = stageList[i].color;
        }
        #endregion

        ui.StageList.transform.DOLocalMoveX(Stage * -300f, 0);
        stageList[Stage].ChangeResource(Color.white, selectPortal.icon);
        _floor += 1;

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.5f);
        seq.Append(ui.StageList.transform.DOLocalMoveX(Stage * -300f, 1f));
        seq.Append(ui.stages[Stage].DOColor(Color.white, 0.5f));
        seq.AppendCallback(() =>
        {
            stageList[Stage].color = Color.white;
            PortalInit();
        });
    }

    private EnemySO GetAttackEnemy()
    {
        List<EnemySO> enemyList = new List<EnemySO>();
        for (int i = 0; i < attackMap.map.Count; ++i)
        {
            if (attackMap.map[i].minFloor <= Floor + 1 && attackMap.map[i].maxFloor >= Floor + 1)
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

    private Portal SpawnPortal(StageType type)
    {
        List<Portal> pList = stageOfPortalDic[type].Where(e => e.isUse == false).ToList();
        if(pList.Count == 0)
        {
            Debug.LogError("Not Found Portal Object");
            return null;
        }

        int cnt = pList.Count;
        Portal portal = pList[Random.Range(0, cnt)];

        switch(type)
        {
            case StageType.Attack:
                AttackPortal atkPortal = portal as AttackPortal;
                atkPortal.enemy = GetAttackEnemy();
                atkPortal.icon = atkPortal.enemy.icon;
                atkPortal.portalName = atkPortal.enemy.enemyName;
                atkPortal.isUse = true;
                return atkPortal;

            case StageType.Boss:
                AttackPortal bossPortal = portal as AttackPortal;
                bossPortal.enemy = _currentChapter.boss;
                bossPortal.icon = bossPortal.enemy.icon;
                bossPortal.portalName = bossPortal.enemy.enemyName;
                bossPortal.isUse = true;
                return bossPortal;
        }

        return null;
    }
}
