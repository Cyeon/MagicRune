using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager
{
    private List<Status> _statusList = new List<Status>();
    private DialScene _dialScene;
    public DialScene DialScene
    {
        get
        {
            if(_dialScene == null)
            {
                _dialScene =  Managers.Scene.CurrentScene as DialScene;
            }
            return _dialScene;
        }
    }
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
            DialScene.ReloadStatusPanel(_unit, status);
        }
        else
        {
            status = Managers.Resource.Instantiate("Status/Status_" + statusName, _unit.statusTrm).GetComponent<Status>();
            status.unit = _unit;
            status.AddValue(count);
            DialScene.AddStatus(_unit, status);
            _statusList.Add(status);
        }

        if(status.OnAddStatus.Count > 0)
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
    }

    public void DeleteStatus(Status status)
    {
        _statusList.Remove(status);
        DialScene.RemoveStatusPanel(_unit, status.statusName);

        for(int i = 0; i < _unit.statusTrm.childCount; ++i)
        {
            if (_unit.statusTrm.GetChild(i).name == "Status_" + status.statusName)
            {
                Managers.Resource.Destroy(_unit.statusTrm.GetChild(i).gameObject);
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

    public float GetStatusValue(StatusName status)
    {
        return GetStatus(status).TypeValue;
    }

    public void OnTurnStart()
    {
        if (_unit.IsDie) return;

        for(int i = 0; i < _statusList.Count; ++i)
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
                _statusList[i].OnAttack.ForEach(x => x.Invoke());
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

        for(int i = 0; i < _statusList.Count; ++i)
        {
            if (_statusList[i] != null)
            {
                if (_statusList[i].type == StatusType.Stack)
                {
                    if (_statusList[i].isTurnRemove == false) continue;
                }

                _statusList[i].RemoveValue(1);
            }
        }
    }

    public void Reset()
    {
        _statusList.Clear();
        _dialScene?.ClearStatusPanel(_unit);

        for(int i = _unit.statusTrm.childCount - 1; i >= 0; --i)
        {
            Debug.Log(_unit.statusTrm.GetChild(i).name);
            Managers.Resource.Destroy(_unit.statusTrm.GetChild(i).gameObject);
        }
    }
}
