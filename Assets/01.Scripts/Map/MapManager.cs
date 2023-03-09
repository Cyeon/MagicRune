using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public  enum MapType
{
    Attack,
    Event
}

[System.Serializable] 
public class Chapter
{
    public int chapter = 0;
    public float[] stageList = new float[9];
}

public class MapManager : MonoSingleton<MapManager>
{
    public List<Chapter> chapterList = new List<Chapter>();

    private int _chapter = 1;
    public int Chapter => _chapter;
    private Chapter _currentChapter = null;

    public List<Map> mapList = new List<Map>();

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
    public int isSceneLoad = 0;

    private void Start()
    {
        ChapterInit();
        MapInit();
    }

    private void ChapterInit()
    {
        mapList.Clear();
        _currentChapter = chapterList[Chapter - 1];

        int stage = 1;
        foreach (var chance in _currentChapter.stageList)
        {
            Map map = new Map();

            int random = Random.Range(1, 100);
            if (random <= chance)
            {
                map.type = MapType.Event;
                map.icon = _eventIcon;
            }
            else
            {
                map.type = MapType.Attack;
                map.icon = _attackIcon;
            }

            map.color = Color.gray;

            mapList.Add(map);
            stage++;
        }

        mapList[0].color = Color.white;
        ui.StageUI();
    }

    private void MapInit()
    {
        if (mapList[Stage].type == MapType.Attack)
        {
            foreach (var map in ui.maps)
            {
                map.Init(GetAttackEnemy());
                map.transform.DOScale(1.2f, 0.8f);
            }
        }
        else
        {
            ui.maps[1].Init(GetAttackEnemy());
            ui.maps[1].transform.DOScale(1.2f, 0.8f);
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

        mapList[Stage].icon = ui.stages[Stage].sprite = selectEnemy.icon;
        _floor += 1;

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.5f);
        seq.Append(ui.StageList.transform.DOLocalMoveX(Stage * -300f, 1f));
        seq.Append(ui.stages[Stage].DOColor(Color.white, 0.5f));
        seq.AppendCallback(() =>
        {
            mapList[Stage].color = Color.white;
            MapInit();
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
