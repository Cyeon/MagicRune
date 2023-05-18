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

    private ChapterTransition _chapterTransition = null;
    public ChapterTransition ChapterTransition
    {
        get
        {
            if(_chapterTransition == null)
            {
                _chapterTransition = FindObjectOfType<ChapterTransition>();
            }
            return _chapterTransition;
        }
    }

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
        if(_mainBackground != null)
            _mainBackground.sprite = Resources.Load<Sprite>(path);
    }
}