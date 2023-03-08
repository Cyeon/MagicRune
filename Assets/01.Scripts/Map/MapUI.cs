using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    public GameObject StageList;

    public List<MapPanel> maps = new List<MapPanel>();

    private void Awake()
    {
        StageList = transform.Find("StageSlider/StageImage").gameObject;

        Transform mapTrm = transform.Find("Maps");
        for(int i = 0; i < mapTrm.childCount; ++i)
        {
            maps.Add(mapTrm.GetChild(i).GetComponent<MapPanel>());
        }
    }
}
