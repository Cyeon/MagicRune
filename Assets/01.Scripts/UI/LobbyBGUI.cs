using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class LobbyBGUI : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> _panelTextList = new List<TextMeshProUGUI>();
    [SerializeField]
    private Material _activeFontMaterial = null;
    [SerializeField]
    private Material _basicFontMaterial = null;
    [SerializeField]
    private HorizontalScrollSnap _scrollSnap;
    [SerializeField]
    private RectTransform _selectPanel;
    [SerializeField]
    private float[] _xPosArray;

    public Ease moveBGEase;
    public float moveBGSpeed;

    private int _index = -1;

    private void Start()
    {
        _scrollSnap.OnSelectionPageChangedEvent.AddListener(ChangeIndex);
        MoveSelectPanel(_scrollSnap.CurrentPage);
    }

    public void ChangeIndex(int index)
    {
        _index = index;

        MoveSelectPanel(_index);
        SetFontMaterial(_index);
    }

    public void MoveSelectPanel(int leftRightToMain)
    {
        _selectPanel.DOAnchorPosX(_xPosArray[leftRightToMain], moveBGSpeed).SetEase(moveBGEase);
    }

    [Obsolete]
    public void MoveBG(int leftRightToMain)
    {
        _index = leftRightToMain;
        _scrollSnap.ChangePage(leftRightToMain);
    }

    public void GameStart()
    {
        Managers.Scene.LoadScene(Define.Scene.MapScene);
    }

    private void SetFontMaterial(int idx)
    {
        for (int i = 0; i < _panelTextList.Count; i++)
        {
            _panelTextList[i].fontMaterial = _basicFontMaterial;
        }
        _panelTextList[idx].fontMaterial = _activeFontMaterial;
    }
}