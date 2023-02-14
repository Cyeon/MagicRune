using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusFuncList : MonoBehaviour
{
    public Status status;

    public void AddGetDamage(float dmg)
    {
        status.unit.currentDmg += dmg;
    }

    public void AddAtkDamage(float dmg)
    {
        status.unit.currentDmg += dmg;
    }

    public void RemAtkDamagePercent(float percent)
    {
        status.unit.currentDmg -= status.unit.currentDmg * (percent * 0.01f);
    }

    public void StackDmg()
    {
        status.unit.TakeDamage(status.typeValue, true);
        UIManager.Instance.UpdateHealthbar(false);
        status.typeValue = 0;
        UIManager.Instance.RemoveStatusPanel(status.unit, status.statusName);
    }

    public void AddFire()
    {
        Status status = StatusManager.Instance.GetUnitHaveStauts(GameManager.Instance.attackUnit, StatusName.Ice);
        if (status != null)
        {
            GameManager.Instance.attackUnit.TakeDamage(status.typeValue * 2);
            status.typeValue = 0;
            UIManager.Instance.RemoveStatusPanel(status.unit, status.statusName);
        }
    }

    public void WoundGetDmg()
    {
        AddGetDamage(status.typeValue);
        status.typeValue = 0;
        UIManager.Instance.RemoveStatusPanel(status.unit, status.statusName);
    }
}
