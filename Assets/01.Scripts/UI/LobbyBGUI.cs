using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyBGUI : MonoBehaviour
{
    [SerializeField] RectTransform bgParent;
    [SerializeField]
    private RectTransform _selectPanel;
    [SerializeField]
    private float[] xPosArray;

    public Ease moveBGEase;
    public float moveBGSpeed;

    private void Start()
    {
        bgParent.DOAnchorPosX(1440 * 0, 0).SetEase(moveBGEase);
        _selectPanel.DOAnchorPosX(xPosArray[0 + 1], 0).SetEase(moveBGEase);
    }

    public void MoveBG(int leftRightToMain)
    {
        bgParent.DOAnchorPosX(1440 * leftRightToMain, moveBGSpeed).SetEase(moveBGEase);
        _selectPanel.DOAnchorPosX(xPosArray[leftRightToMain + 1], moveBGSpeed).SetEase(moveBGEase);
    }

    public void GameStart()
    {
        Managers.Scene.LoadScene(Define.Scene.MapScene);
    }
}
