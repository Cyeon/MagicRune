using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    public Transform StageList;

    private List<Image> stages = new List<Image>();
    public List<Image> Stages
    {
        get
        {
            if (stages.Count <= 0)
            {
                for (int i = 0; i < StageList.childCount; ++i)
                {
                    stages.Add(StageList.GetChild(i).GetComponent<Image>());
                }
            }

            return stages;
        }
    }

    public Sprite stageAtkIcon;
    public Sprite stageBossIcon;
    public Sprite stageEventIcon;
    
    private void Start()
    {
        if(stages.Count <= 0)
        {
            for (int i = 0; i < StageList.childCount; ++i)
            {
                stages.Add(StageList.GetChild(i).GetComponent<Image>());
            }
        }

        //Managers.Map.NextStage();
    }
}
