using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

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

    [SerializeField] private GameObject _mapPrefab;
    public List<Map> mapList = new List<Map>();

    private int Stage => Floor - ((this.Chapter - 1) * 10) - 1;

    private int _floor = 1;
    public int Floor => _floor;

    public static EnemySO SelectEnemy;
    public AttackMapListSO attackMap;

    public MapUI ui;
    [SerializeField]
    private Sprite _eventIcon;
    [SerializeField]
    private Sprite _attackIcon;

    private void Awake()
    {
        ui = FindObjectOfType<MapUI>();
    }

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
        foreach(var chance in _currentChapter.stageList)
        {
            Map map = Instantiate(_mapPrefab, ui.StageList.transform).GetComponent<Map>();
            map.name = "Stage " + stage;

            int random = Random.Range(0, 100);
            if(random <= chance)
            {
                map.type = MapType.Event;
                map.icon.sprite = _eventIcon;
            }
            else
            {
                map.type = MapType.Attack;
                map.icon.sprite = _attackIcon;
            }
            map.icon.color = Color.gray;

            mapList.Add(map);
            stage++;       
        }

        mapList[0].icon.color = Color.white;
    }

    private void MapInit()
    {
        if (mapList[Stage].type == MapType.Attack)
        {
            foreach (var map in ui.maps)
            {
                map.gameObject.SetActive(true);
                map.Init(GetAttackEnemy());
                map.transform.DOScale(1.2f, 0.8f);
            }
        }
        else
        {

            ui.maps[1].gameObject.SetActive(true);
            ui.maps[1].Init(GetAttackEnemy());
            ui.maps[1].transform.DOScale(1.2f, 0.8f);
        }
    }

    private void NextStage()
    {
        ui.maps.ForEach(x => x.transform.DOScale(0, 0.5f));

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.5f);
        seq.Append(ui.StageList.transform.DOLocalMoveX(ui.StageList.transform.localPosition.x + -300f, 1f));
        seq.AppendCallback(() => _floor++);
        seq.Append(mapList[Stage].icon.DOColor(Color.white, 0.5f));
        seq.AppendCallback(() =>
        {
            MapInit();
        });
    }

    //private MapType GetMapType()
    //{
    //    float sum = 0f;
    //    foreach(var map in chapterList)
    //    {
    //        sum += map.percent;
    //    }

    //    float value = Random.Range(0, sum);
    //    float temp = 0f;

    //    for(int i = 0; i < chapterList.Count; i++) 
    //    { 
    //        if(value >= temp && value < temp + chapterList[i].percent)
    //            return chapterList[i].type;

    //        temp += chapterList[i].percent;
    //    }

    //    return MapType.Attack;
    //}

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
