using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
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

    private void Awake()
    {
        _mainBackground = transform.Find("MainBackground").GetComponent<Image>();
    }

    private void Start()
    {
        ChangeBackground();
    }

    public void ChangeBackground()
    {
        string path = "Sprite/MapBg_" + Managers.Map.Chapter.ToString();
        if(_mainBackground != null)
            _mainBackground.sprite = Resources.Load<Sprite>(path);
    }

    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}