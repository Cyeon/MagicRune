using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MapDefine;

public enum DistractorFunc
{
    NextStage,
    Healing
}

public class DistracotrFuncList : MonoBehaviour
{
    public void NextStage()
    {
        MapManager.Instance.NextStage();
        MapSceneUI.adventureUI.gameObject.SetActive(false);
    }

    public void Healing(int amount)
    {
        GameManager.Instance.player.HP += 10;
        MapSceneUI.InfoUIReload();
        NextStage();
    }
}
