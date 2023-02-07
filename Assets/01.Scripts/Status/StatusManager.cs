using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StatusManager : MonoSingleton<StatusManager>
{
    public List<Status> statusList = new List<Status>(); // ëª¨ë“  ?íƒœ?´ìƒ ëª©ë¡
    private StatusFuncList _statusFuncList = null;

    private void Awake()
    {
        _statusFuncList = GetComponent<StatusFuncList>();
    }

    // ?íƒœ?´ìƒ ?¨ê³¼ ë°œë™
    public void StatusFuncInvoke(List<Status> status)
    {
        foreach(var funStatus in status)
        {
            _statusFuncList.status = funStatus;
            funStatus.statusFunc?.Invoke();
        }
    }

    // ?íƒœ?´ìƒ ëª©ë¡?ì„œ ê°€?¸ì˜¤ê¸?
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

    // ?íƒœ?´ìƒ ì¶”ê?
    public void AddStatus(Unit unit, StatusName statusName, int value = 1)
    {
        Status status = GetStatus(statusName);
        if(status == null)
        {
            Debug.LogWarning(string.Format("{0} status not found."));
            return;
        }

        Status newStatus = new Status(status);
        newStatus.typeValue = value;
        
        List<Status> statusList = new List<Status>();
        if(unit.unitStatusDic.TryGetValue(status.invokeTime, out statusList))
        {
            if (unit == GameManager.Instance.enemy)
                //UIManager.Instance.StatusPopup(newStatus, UIManager.Instance.enemyIcon.transform.position);
                UIManager.Instance.StatusPopup(newStatus);

            Status currentStauts = statusList.Where(e => e.statusName == status.statusName).FirstOrDefault();
            if(currentStauts != null)
            {
                currentStauts.typeValue += status.typeValue > 0 ? status.typeValue : value;
                UIManager.Instance.ReloadStatusPanel(unit, currentStauts.statusName, currentStauts.typeValue);
            }
            else
            {
                newStatus.unit = unit;
                unit.unitStatusDic[newStatus.invokeTime].Add(newStatus);
                UIManager.Instance.AddStatus(unit, newStatus);

                newStatus.addFunc?.Invoke();
            }
        }
    }

    // ?íƒœ?´ìƒ ?œê±°
    public void RemStatus(Unit unit, Status status)
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

    public void RemStatus(Unit unit, StatusName statusName)
    {
        Status status = GetStatus(statusName);
        RemStatus(unit, status);
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

            indexes.ForEach(e => RemStatus(unit, x.Value[e]));
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

            indexes.ForEach(e => RemStatus(unit, x.Value[e]));
        }
    }
}
