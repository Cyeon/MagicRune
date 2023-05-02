using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyBGUI : MonoBehaviour
{
    [SerializeField] RectTransform bgParent;

    public Ease moveBGEase;
    public float moveBGSpeed;

    public void MoveBG(int leftRightToMain)
    {
        bgParent.transform.DOMoveX(1440 * leftRightToMain + 720, moveBGSpeed).SetEase(moveBGEase);

    }
}
