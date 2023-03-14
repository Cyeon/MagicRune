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

    private int _floor = 1;
    public int Floor => _floor;

    [Header("Portal")]




    public EnemySO selectEnemy;
    public AttackMapListSO attackMap;

    public MapUI ui;

    private bool _isFirst = true;

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
            if (random <= chance)
            {
                stage.Init(StageType.Event, ui.stages[0]);
            }
            else
            {
                stage.Init(StageType.Attack, ui.stages[0]);
            }
            stageList.Add(stage);
        }

        Stage bossStage = new Stage();
        bossStage.Init(StageType.Boss, ui.stages[9]);
        stageList.Add(bossStage);

        stageList[0].color = Color.white;
        ui.StageUI();
    }

    private void PortalInit()
    {
        ui.PortalEffectUp(portalList[Stage].type);

        if (portalList[Stage].type == PortalType.Attack)
        {
            foreach (var portal in ui.portals)
            {
                portal.Init(GetAttackEnemy());
                portal.transform.DOScale(1f, 0.8f);
            }
        }
        else if (portalList[Stage].type == PortalType.Boss)
        {
            ui.portals[1].Init(_currentChapter.boss);
            ui.portals[1].transform.DOScale(2f, 1f);
        }
        else if (portalList[Stage].type == PortalType.Event)
        {
            ui.portals[1].Init(GetAttackEnemy());
            ui.portals[1].transform.DOScale(1f, 0.8f);
        }
    }

    public void NextStage()
    {
        if(_isFirst)
        {
            _isFirst = false;
            return;
        }

        DOTween.KillAll();

        ui.StageUI();
        ui.StageList.transform.DOLocalMoveX(Stage * -300f, 0);

        if (portalList[Stage].type == PortalType.Attack)
            portalList[Stage].icon = ui.stages[Stage].sprite = selectEnemy.icon;

        _floor += 1;

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.5f);
        seq.Append(ui.StageList.transform.DOLocalMoveX(Stage * -300f, 1f));
        seq.Append(ui.stages[Stage].DOColor(Color.white, 0.5f));
        seq.AppendCallback(() =>
        {
            portalList[Stage].color = Color.white;
            PortalInit();
        });
    }

    private EnemySO GetAttackEnemy()
    {
        List<EnemySO> enemyList = new List<EnemySO>();
        for (int i = 0; i < attackMap.map.Count; ++i)
        {
            if (attackMap.map[i].minFloor <= Floor && attackMap.map[i].maxFloor >= Floor)
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
}
