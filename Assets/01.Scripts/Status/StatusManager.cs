using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mono.Cecil;

public class StatusManager
{
    private List<Status> _statusList = new List<Status>();
    private DialScene _dialScene;
    private Unit _unit;


    public StatusManager(Unit unit, DialScene dialScene)
    {
        _unit = unit;
        _dialScene = dialScene;
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
            _dialScene.ReloadStatusPanel(_unit, status);
        }
        else
        {
            status = ResourceManager.Instance.Instantiate("Status/Status_" + statusName, _unit.statusTrm).GetComponent<Status>();
            status.AddValue(count);
            _dialScene.AddStatus(_unit, status);
        }

        status.OnAddStatus.ForEach(x => x.Invoke());
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
        if(status.TypeValue <= 0)
        {
            DeleteStatus(status);
            return;
        }

        _dialScene.ReloadStatusPanel(_unit, status);
    }

    public void DeleteStatus(Status status)
    {
        _statusList.Remove(status);
        _dialScene.RemoveStatusPanel(_unit, status.statusName);

        for(int i = 0; i < _unit.statusTrm.childCount; ++i)
        {
            if (_unit.statusTrm.GetChild(i).name == "Status_" + status.statusName)
            {
                ResourceManager.Instance.Destroy(_unit.statusTrm.GetChild(i).gameObject);
            }
        }
    }

    public void DeleteStatus(StatusName statusName)
    {
        DeleteStatus(GetStatus(statusName));
    }

    private bool IsHaveStatus(StatusName status)
    {
        for(int i = 0; i < _statusList.Count; ++i)
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
        for(int i = 0; i <_statusList.Count; ++i)
        {
            if (_statusList[i].statusName == status)
            {
                return _statusList[i];
            }
        }

        return null;
    }

    public void OnTurnStart()
    {
        if (_unit.IsDie) return;

        for(int i = 0; i < _statusList.Count; ++i)
        {
            if (_statusList[i] != null)
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
            if (_statusList[i] != null)
            {
                _statusList[i].OnAttack.ForEach(x => x.Invoke());
            }
        }
    }

    public void OnGetDamage()
    {
        if (_unit.IsDie) return;

        for (int i = 0; i < _statusList.Count; ++i)
        {
            if (_statusList[i] != null)
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
            if (_statusList[i] != null)
            {
                _statusList[i].OnTurnEnd.ForEach(x => x.Invoke());
            }
        }
    }

    public void TurnChange()
    {
        if (_unit.IsDie) return;

        for(int i = 0; i < _statusList.Count; ++i)
        {
            if (_statusList[i] != null)
            {
                if (_statusList[i].type == StatusType.Stack)
                {
                    if (_statusList[i].isTurnRemove == false) continue;
                }

                _statusList[i].RemoveValue(1);
                _dialScene.ReloadStatusPanel(_unit, _statusList[i]);
            }
        }
    }

    public void Reset()
    {
        _statusList.Clear();
        _dialScene?.ClearStatusPanel(_unit);
    }
}
