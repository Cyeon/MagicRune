using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyBGUI : MonoBehaviour
{
    [SerializeField] RectTransform bgParent;

    public Ease moveBGEase;
    public float moveBGSpeed;

    public void MoveMainBG()
    {
        bgParent.transform.DOMoveX(0 + 720, moveBGSpeed).SetEase(moveBGEase);

    }

    public void MoveShopBG()
    {
        bgParent.transform.DOMoveX(2160 + 720, moveBGSpeed).SetEase(moveBGEase);

    }

    public void MoveSkillTreeBG()
    {
        bgParent.transform.DOMoveX(-2160 + 720, moveBGSpeed).SetEase(moveBGEase);

    }


}
