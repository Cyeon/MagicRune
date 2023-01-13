using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusFuncList : MonoBehaviour
{
    public void AddGetDamage(float dmg)
    {
        if(GameManager.Instance.gameTurn == GameTurn.Player)
            GameManager.Instance.enemy.currentDmg += dmg;
        else if(GameManager.Instance.gameTurn == GameTurn.Monster)
            GameManager.Instance.player.currentDmg += dmg;
    }

    public void AddAtkDamage(float dmg)
    {
        GameManager.Instance.currentUnit.currentDmg += dmg;
    }

    public void TurnChange()
    {
        GameManager.Instance.TurnChange();
    }
}
