using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public  enum PortalType
{
    Attack,
    Event,
    Boss
}

[System.Serializable] 
public class Chapter
{
    public int chapter = 0;
    public EnemySO boss;
    public float[] stageList = new float[9];
}

public class MapManager : MonoSingleton<MapManager>
{
    public List<Chapter> chapterList = new List<Chapter>();

    private int _chapter = 1;
    public int Chapter => _chapter;
    private Chapter _currentChapter = null;

    public List<Portal> portalList = new List<Portal>();

    private int Stage => Floor - ((this.Chapter - 1) * 10) - 1;

    private int _floor = 1;
    public int Floor => _floor;

    public EnemySO selectEnemy;
    public AttackMapListSO attackMap;

    public MapUI ui;

    [SerializeField]
    private Sprite _eventIcon;
    [SerializeField]
    private Sprite _attackIcon;

    private bool _isFirst = true;

    private void Start()
    {
        ChapterInit();
        PortalInit();
        ui.InfoUIReload();
    }

    private void ChapterInit()
    {
        portalList.Clear();
        _currentChapter = chapterList[Chapter - 1];

        foreach (var chance in _currentChapter.stageList)
        {
            Portal map = new Portal();

            int random = Random.Range(1, 100);
            if (random <= chance)
            {
                map.type = PortalType.Event;
                map.icon = _eventIcon;
            }
            else
            {
                map.type = PortalType.Attack;
                map.icon = _attackIcon;
            }

            map.color = Color.gray;

            portalList.Add(map);
        }

        Portal portal = new Portal();
        portal.type = PortalType.Boss;
        portal.color = Color.gray;
        portal.icon = _currentChapter.boss.icon;
        portalList.Add(portal);

        portalList[0].color = Color.white;
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
        else
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
