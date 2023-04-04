using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

public class StatusManager : MonoSingleton<StatusManager>
{
    public List<Status> statusList = new List<Status>(); // 모든 ?�태?�상 목록
    private StatusFuncList _statusFuncList = null;

    private DialScene _dialScene;

    private void Awake()
    {
        _statusFuncList = GetComponent<StatusFuncList>();
    }

    private void Start()
    {
        _dialScene = SceneManagerEX.Instance.CurrentScene as DialScene;
    }

    // ?�태?�상 ?�과 발동
    public void StatusFuncInvoke(List<Status> status, Unit unit)
    {
        foreach(var funStatus in status)
        {
            _statusFuncList.status = funStatus;
            _statusFuncList.unit = unit;
            if (funStatus.typeValue > 0)
            {
                funStatus.statusFunc?.Invoke();
            }
        }

        StatusUpdate(unit);
    }

    // ?�태?�상 목록?�서 가?�오�?
    private Status GetStatus(StatusName name)
    {
        return statusList.Where(e => e.statusName == name).FirstOrDefault();
    }

    public bool IsHaveStatus(Unit unit, StatusName name)
    {
        Status status = GetUnitHaveStauts(unit, name);
        return status != null;
    }

    public Status GetUnitHaveStauts(Unit unit, StatusName name)
    {
        Status status = GetStatus(name);

        List<Status> statusList = new List<Status>();
        if (unit.unitStatusDic.TryGetValue(status.invokeTime, out statusList))
        {
            return statusList.Where(e => e.statusName == status.statusName).FirstOrDefault();
        }

        Debug.LogError("NotFound");
        return null;
    }

    public float GetUnitStatusValue(Unit unit, StatusName name)
    {
        Status status = GetUnitHaveStauts(unit, name);
        if (status != null)
        {
            return status.typeValue;
        }
        else
        {
            //Debug.LogError(string.Format("{0} is not have {1} status.", unit, name));
            return 0;
        }
    }

    public List<StatusName> GetUnitStateList(Unit unit)
    {
        List<StatusName> statusList = null;
        for(int i = 0; i < (int)StatusName.COUNT; i++)
        {
            if (IsHaveStatus(unit, (StatusName)i)) {
                statusList.Add((StatusName)i);
            }
        }
        return statusList;
    }

    // ?�태?�상 추�?
    public void AddStatus(Unit unit, StatusName statusName, int value = 1)
    {
        if (unit.IsDie == false)
        {
            Status status = GetStatus(statusName);
            if (status == null)
            {
                Debug.LogWarning(string.Format("{0} status not found.", statusName));
                return;
            }

            Status newStatus = new Status(status);
            newStatus.typeValue = value;

            List<Status> statusList = new List<Status>();
            if (unit.unitStatusDic.TryGetValue(status.invokeTime, out statusList))
            {
                if (unit == BattleManager.Instance.enemy)
                    //UIManager.Instance.StatusPopup(newStatus, UIManager.Instance.enemyIcon.transform.position);
                    _dialScene?.StatusPopup(newStatus);

                _statusFuncList.unit = unit;

                Status currentStauts = statusList.Where(e => e.statusName == status.statusName).FirstOrDefault();
                if (currentStauts != null)
                {
                    currentStauts.typeValue += status.typeValue > 0 ? status.typeValue : value;
                    _dialScene?.ReloadStatusPanel(unit, currentStauts.statusName, currentStauts.typeValue);

                    _statusFuncList.status = currentStauts;
                    currentStauts.addFunc?.Invoke();
                }
                else
                {
                    newStatus.unit = unit;
                    unit.unitStatusDic[newStatus.invokeTime].Add(newStatus);
                    _dialScene?.AddStatus(unit, newStatus);

                    _statusFuncList.status = newStatus;
                    newStatus.addFunc?.Invoke();
                }
            }
        }
    }

    // �����̻� ����
    public void AllRemStatus(Unit unit, Status status)
    {
        if (unit.IsDie == false)
        {
            Status currentStauts = GetUnitHaveStauts(unit, status.statusName);

            if (currentStauts != null)
            {
                unit.unitStatusDic[status.invokeTime].Remove(status);
                _dialScene?.RemoveStatusPanel(unit, status.statusName);
            }
            else
            {
                Debug.LogWarning(string.Format("{0} status is not found. Can't Remove do it.", status.statusName));
            }
        }
    }

    public void CountRemStatus(Unit unit, Status status, int count)
    {
        if (unit.IsDie == false)
        {
            Status currentStauts = GetUnitHaveStauts(unit, status.statusName);

            if (currentStauts != null)
            {
                currentStauts.typeValue -= count;
                _dialScene?.ReloadStatusPanel(unit, currentStauts.statusName, currentStauts.typeValue);
            }
            else
            {
                Debug.LogWarning(string.Format("{0} status is not found. Can't Remove do it.", status.statusName));
            }
        }
    }

    public void CountRemStatus(Unit unit, StatusName statusName, int count)
    {
        if (unit.IsDie == false)
        {
            Status status = GetStatus(statusName);
            CountRemStatus(unit, status, count);
        }
    }

    public void AllRemStatus(Unit unit, StatusName statusName)
    {
        if (unit.IsDie == false)
        {
            Status status = GetStatus(statusName);
            AllRemStatus(unit, status);
        }
    }

    public void StatusUpdate(Unit unit)
    {
        foreach (var x in unit.unitStatusDic)
        {
            List<int> indexes = new List<int>();
            for (int i = 0; i < x.Value.Count; i++)
            {
                if (x.Value[i].typeValue <= 0)
                {
                    indexes.Add(i);
                }
            }

            indexes.ForEach(e => AllRemStatus(unit, x.Value[e]));
        }
    }

    public void StatusTurnChange(Unit unit)
    {
        foreach(var x in unit.unitStatusDic)
        {
            List<int> indexes = new List<int>();
            for(int i = 0; i < x.Value.Count; i++)
            {
                if (x.Value[i].type == StatusType.Turn)
                {
                    if (x.Value[i].isTurnRemove)
                        x.Value[i].typeValue--;
                }

                _dialScene?.ReloadStatusPanel(unit, x.Value[i].statusName, x.Value[i].typeValue);
                if (x.Value[i].typeValue <= 0)
                {
                    indexes.Add(i);
                }
            }

            indexes.ForEach(e => AllRemStatus(unit, x.Value[e]));
        }
    }

    public void RemoveValue(Unit unit, StatusName status, int value)
    {
        CountRemStatus(unit, GetStatus(status), value);
    }
}
