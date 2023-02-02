using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusFuncList : MonoBehaviour
{
    public Status status;

    public void AddGetDamage(float dmg)
    {
        if(GameManager.Instance.GameTurn == GameTurn.Player)
            GameManager.Instance.enemy.currentDmg += dmg;
        else if(GameManager.Instance.GameTurn == GameTurn.Monster)
            GameManager.Instance.player.currentDmg += dmg;
    }

    public void AddAtkDamage(float dmg)
    {
        GameManager.Instance.currentUnit.currentDmg += dmg;
    }

    public void RemAtkDamagePercent(float percent)
    {
        GameManager.Instance.currentUnit.currentDmg -= GameManager.Instance.currentUnit.currentDmg * (percent * 0.01f);
    }

    public void StackDmg()
    {
        status.unit.TakeDamage(status.typeValue, true);
        status.typeValue = 0;
        UIManager.Instance.UpdateEnemyHealthbar();
    }

    public void AddFire()
    {
        Status status = StatusManager.Instance.GetUnitHaveStauts(GameManager.Instance.attackUnit, StatusName.Ice);
        if (status != null)
        {
            GameManager.Instance.attackUnit.TakeDamage(status.typeValue * 2);
            StatusManager.Instance.RemStatus(GameManager.Instance.attackUnit, status);
            UIManager.Instance.ReloadStatusPanel(GameManager.Instance.attackUnit, status.statusName, 0);
        }
    }
}
