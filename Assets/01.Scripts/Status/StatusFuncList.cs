using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusFuncList : MonoBehaviour
{
    public Status status;
    public Unit unit;

    private DialScene _dialScene;

    private void Start()
    {
        _dialScene = Managers.Scene.CurrentScene as DialScene;
    }

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

    public void RemoveStatusOneStack()
    {
        StatusManager.Instance.CountRemStatus(unit, status, 1);
    }

    public void StackDmg()
    {
        unit.TakeDamage(status.typeValue, true, status);
        _dialScene?.UpdateHealthbar(unit.IsPlayer);
        StatusManager.Instance.CountRemStatus(unit, status, 0);
        _dialScene?.ReloadStatusPanel(unit, status.statusName, status.typeValue);
    }

    public void WoundGetDmg()
    {
        AddGetDamage(status.typeValue);
        status.typeValue = 0;
        _dialScene?.ReloadStatusPanel(unit, status.statusName, status.typeValue);
    }

    public void RemoveStack()
    {
        StatusManager.Instance.CountRemStatus(unit, status, Mathf.FloorToInt(unit.currentDmg));
    }

    public void FreezeFiveCilliness()
    {
        if(StatusManager.Instance.GetUnitStatusValue(unit, status.statusName) >= 5)
        {
            StatusManager.Instance.AllRemStatus(unit, status);
            StatusManager.Instance.AddStatus(unit, StatusName.Ice);
        }
    }

    public void TurnChange()
    {
        BattleManager.Instance.TurnChange();
    }
}
