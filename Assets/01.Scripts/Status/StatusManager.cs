using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

public class StatusManager : MonoSingleton<StatusManager>
{
    public List<Status> statusList = new List<Status>(); // 모든 ?�태?�상 목록
    private StatusFuncList _statusFuncList = null;

    private void Awake()
    {
        _statusFuncList = GetComponent<StatusFuncList>();
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
        Status status = GetStatus(statusName);
        if(status == null)
        {
            Debug.LogWarning(string.Format("{0} status not found.", statusName));
            return;
        }

        Status newStatus = new Status(status);
        newStatus.typeValue = value;
        
        List<Status> statusList = new List<Status>();
        if(unit.unitStatusDic.TryGetValue(status.invokeTime, out statusList))
        {
            if (unit == BattleManager.Instance.enemy)
                //UIManager.Instance.StatusPopup(newStatus, UIManager.Instance.enemyIcon.transform.position);
                UIManager.Instance.StatusPopup(newStatus);

            Status currentStauts = statusList.Where(e => e.statusName == status.statusName).FirstOrDefault();
            if(currentStauts != null)
            {
                currentStauts.typeValue += value;
                UIManager.Instance.ReloadStatusPanel(unit, currentStauts.statusName, currentStauts.typeValue);

                currentStauts.addFunc?.Invoke();
            }
            else
            {
                newStatus.unit = unit;
                unit.unitStatusDic[newStatus.invokeTime].Add(newStatus);
                UIManager.Instance.AddStatus(unit, newStatus);

                newStatus.addFunc?.Invoke();
            }
        }
        else
        {
            Debug.LogError("��ųʸ� ����Ʈ�� ã�� �� �����~~");
        }
    }

    // ?�태?�상 ?�거
    public void DeleteStatus(Unit unit, Status status)
    {
        List<Status> statusList = new List<Status>();
        if(unit.unitStatusDic.TryGetValue(status.invokeTime, out statusList))
        {
            Status currentStauts = statusList.Where(e => e.statusName == status.statusName).FirstOrDefault();
            if (currentStauts != null)
            {
                unit.unitStatusDic[status.invokeTime].Remove(status);
                UIManager.Instance.RemoveStatusPanel(unit, status.statusName);
            }
            else
            {
                Debug.LogWarning(string.Format("{0} status is not found. Can't Remove do it.", status.statusName));
            }
        }
    }

    public void DeleteStatus(Unit unit, StatusName statusName)
    {
        Status status = GetStatus(statusName);
        DeleteStatus(unit, status);
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

            indexes.ForEach(e => DeleteStatus(unit, x.Value[e]));
        }
    }

    public void StatusTurnChange(Unit unit)
    {
        foreach(var x in unit.unitStatusDic)
        {
            List<int> indexes = new List<int>();
            for(int i = 0; i < x.Value.Count; i++)
            {
                if (x.Value[i].type == StatusType.Turn) x.Value[i].typeValue--;

                UIManager.Instance.ReloadStatusPanel(unit, x.Value[i].statusName, x.Value[i].typeValue);
                if (x.Value[i].typeValue <= 0)
                {
                    indexes.Add(i);
                }
            }

            indexes.ForEach(e => DeleteStatus(unit, x.Value[e]));
        }
    }

    public void RemoveValue(Unit unit, Status status, int value)
    {
        Status s = unit.unitStatusDic[status.invokeTime].Where(e => e.statusName == status.statusName).FirstOrDefault();
        s.typeValue -= value;
        UIManager.Instance.ReloadStatusPanel(unit, s.statusName, s.typeValue);
    }

    public void RemoveValue(Unit unit, StatusName status, int value)
    {
        RemoveValue(unit, GetStatus(status), value);
    }
}
