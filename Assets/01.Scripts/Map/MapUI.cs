using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    private Image _mainBackground = null;

    [Header("End Game")]
    private GameObject _endGamePanel;

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
        _endGamePanel = transform.Find("EndGamePanel").gameObject;
    }

    private void Start()
    {
        ChangeBackground();

        _endGamePanel.SetActive(false);
    }

    public void ChangeBackground()
    {
        string path = "Sprite/MapBg_" + Managers.Map.Chapter.ToString();
        if(_mainBackground != null)
            _mainBackground.sprite = Resources.Load<Sprite>(path);
    }

    public void EndGame()
    {
        transform.GetComponent<Canvas>().sortingLayerName = "Top";
        _endGamePanel.SetActive(true);

        _endGamePanel.transform.localScale = Vector3.zero;
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => _endGamePanel.SetActive(true));
        seq.Append(_endGamePanel.transform.DOScale(Vector3.one * 1.2f, 0.2f));
        seq.Append(_endGamePanel.transform.DOScale(Vector3.one, 0.05f));
        seq.AppendCallback(() => _endGamePanel.SetActive(true));
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