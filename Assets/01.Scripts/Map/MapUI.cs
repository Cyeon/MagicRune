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

    private Image _mainBackground = null;

    private void Start()
    {
        _mainBackground = transform.Find("Main Background").GetComponent<Image>();

        if (stages.Count <= 0)
        {
            for (int i = 0; i < StageList.childCount; ++i)
            {
                stages.Add(StageList.GetChild(i).GetComponent<Image>());
            }
        }
        ChangeBackground();
    }

    public void ChangeBackground()
    {
        string path = "Sprite/MapBg_" + Managers.Map.Chapter.ToString();
        _mainBackground.sprite = Managers.Resource.Load<Sprite>(path);
    }
}