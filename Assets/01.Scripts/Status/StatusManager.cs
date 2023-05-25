using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;

public class StatusManager
{
    private List<Status> _statusList = new List<Status>();
    private Unit _unit;

    public Action<Status, int> OnAddStatus;

    public StatusManager(Unit unit)
    {
        _unit = unit;
        _unit.OnGetDamage += OnGetDamage;
    }

    public void AddStatus(StatusName statusName, int count)
    {
        if (_unit.IsDie) return;

        Status status;
        if (IsHaveStatus(statusName))
        {
            status = GetStatus(statusName);

            if (status == null)
            {
                Debug.LogWarning(status + "Not Found.");
                return;
            }

            status.AddValue(count);
            Define.DialScene?.ReloadStatusPanel(_unit, status);
        }
        else
        {
            status = Managers.Resource.Instantiate("Status/Status_" + statusName, _unit.statusTrm).GetComponent<Status>();
            status.unit = _unit;
            status.AddValue(count);
            Define.DialScene?.AddStatus(_unit, status);
            _statusList.Add(status);
        }

        switch (status.statusSoundType)
        {
            case StatusSoundType.Main:
                if (status.getSound != null)
                    Managers.Sound.PlaySound(status.getSound, SoundType.Effect);
                else
                    Debug.LogError($"{status.statusName}'s AudioClip is NULL");
                break;
            case StatusSoundType.Positive:
                Managers.Sound.PlaySound("SFX/PositiveStatus", SoundType.Effect);
                break;
            case StatusSoundType.Negative:
                Managers.Sound.PlaySound("SFX/NegativeStatus", SoundType.Effect);
                break;
            case StatusSoundType.None:
            default:
                break;
        }

        if (status.OnAddStatus.Count > 0)
            status.OnAddStatus.ForEach(x => x.Invoke());

        if (_unit is Enemy)
            Define.DialScene?.AddStatusEffect(_unit, status);
        OnAddStatus?.Invoke(status, count);
    }

    public void RemoveStatus(StatusName statusName, int count)
    {
        if (_unit.IsDie) return;

        if (!IsHaveStatus(statusName))
        {
            Debug.LogWarning(_unit + "is not have " + statusName);
            return;
        }

        Status status = GetStatus(statusName);
        status.RemoveValue(count);
    }

    public void DeleteStatus(Status status)
    {
        if (status == null) return;
        if (_unit.IsDie || IsHaveStatus(status.statusName) == false) return;

        _statusList.Remove(status);
        Define.DialScene?.RemoveStatusPanel(_unit, status.statusName);

        if (status.OnRemoveStatus.Count > 0)
            status.OnRemoveStatus.ForEach(x => x.Invoke());

        for (int i = 0; i < _unit.statusTrm.childCount; ++i)
        {
            if (_unit.statusTrm.GetChild(i).name == "Status_" + status.statusName)
            {
                Managers.Resource.Destroy(_unit.statusTrm.GetChild(i).gameObject);
            }
        }
    }

    public void DeleteStatus(StatusName statusName)
    {
        if (GetStatus(statusName) == null) return;
        DeleteStatus(GetStatus(statusName));
    }

    public bool IsHaveStatus(StatusName status)
    {
        for (int i = 0; i < _statusList.Count; ++i)
        {
            if (_statusList[i].statusName == status)
            {
                return true;
            }
        }

        return false;
    }

    public Status GetStatus(StatusName status)
    {
        for (int i = 0; i < _statusList.Count; ++i)
        {
            if (_statusList[i].statusName == status)
            {
                return _statusList[i];
            }
        }

        return null;
    }

    public float GetStatusValue(StatusName status)
    {
        if (GetStatus(status) == null) return 0;

        return GetStatus(status).TypeValue;
    }

    public void OnTurnStart()
    {
        if (_unit.IsDie) return;

        for (int i = 0; i < _statusList.Count; ++i)
        {
            if (_statusList[i] != null && _statusList[i].OnTurnStart.Count > 0)
            {
                _statusList[i].OnTurnStart.ForEach(x => x.Invoke());
            }
        }
    }

    public void OnAttack()
    {
        if (_unit.IsDie) return;

        for (int i = 0; i < _statusList.Count; ++i)
        {
            if (_statusList[i] != null && _statusList[i].OnAttack.Count > 0)
            {
                _statusList[i].OnAttack.ForEach(x => {
                    if (!_unit.IsDie) x.Invoke();
                    });
            }
        }
    }

    public void OnGetDamage()
    {
        if (_unit.IsDie) return;

        for (int i = 0; i < _statusList.Count; ++i)
        {
            if (_statusList[i] != null && _statusList[i].OnGetDamage.Count > 0)
            {
                _statusList[i].OnGetDamage.ForEach(x => x.Invoke());
            }
        }
    }

    public void OnTurnEnd()
    {
        if (_unit.IsDie) return;

        for (int i = 0; i < _statusList.Count; ++i)
        {
            if (_statusList[i] != null && _statusList[i].OnTurnEnd.Count > 0)
            {
                _statusList[i].OnTurnEnd.ForEach(x => x.Invoke());
            }
        }
    }

    public void TurnChange()
    {
        if (_unit.IsDie) return;

        List<Status> remStatusList = new List<Status>();
        for (int i = 0; i < _statusList.Count; ++i)
        {
            if (_statusList[i] != null)
            {
                if (_statusList[i].type == StatusType.Stack)
                {
                    if (_statusList[i].isTurnRemove == false) continue;
                }
                remStatusList.Add(_statusList[i]);
            }
        }

        for (int i = 0; i < remStatusList.Count; ++i)
        {
            if (remStatusList[i] != null)
            {
                remStatusList[i].RemoveValue(1);
            }
        }
    }

    public void Reset()
    {
        _statusList.Clear();
        Define.DialScene?.ClearStatusPanel(_unit);

        for (int i = _unit.statusTrm.childCount - 1; i >= 0; --i)
        {
            Managers.Resource.Destroy(_unit.statusTrm.GetChild(i).gameObject);
        }
    }
}
