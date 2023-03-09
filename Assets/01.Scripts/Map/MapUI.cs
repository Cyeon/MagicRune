using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    public Transform StageList;

    public List<MapPanel> maps = new List<MapPanel>();
    public List<Image> stages = new List<Image>();

    private void Awake()
    {
        StageList = transform.Find("StageSlider/StageImage");

        Transform mapTrm = transform.Find("Maps");
        for(int i = 0; i < mapTrm.childCount; ++i)
        {
            maps.Add(mapTrm.GetChild(i).GetComponent<MapPanel>());
        }

        for (int i = 0; i < StageList.childCount; ++i)
        {
            stages.Add(StageList.GetChild(i).GetComponent<Image>());
        }
    }

    private void OnEnable()
    {
        MapManager.Instance.ui = this;
        MapManager.Instance.NextStage();
    }

    public void StageUI()
    {
        for(int i = 0; i < 9; ++i)
        {
            stages[i].sprite = MapManager.Instance.mapList[i].icon;
            stages[i].color = MapManager.Instance.mapList[i].color;
        }
    }
}
