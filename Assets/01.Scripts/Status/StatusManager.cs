using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StatusManager : MonoSingleton<StatusManager>
{
    public List<Status> statusList = new List<Status>(); // 모든 상태이상 목록

    // 상태이상 효과 발동
    public void StatusFuncInvoke(List<Status> status)
    {
        status.ForEach(x => x.statusFunc.Invoke());
    }

    // 상태이상 목록에서 가져오기
    private Status GetStatus(string name)
    {
        return statusList.Where(e => e.statusName == name).FirstOrDefault();
    }

    // 상태이상 추가
    public void AddStatus(Unit unit, string statusNmae)
    {
        Status status = GetStatus(statusNmae);
        if(status == null)
        {
            Debug.LogWarning(string.Format("{0} status not found."));
            return;
        }

        Status newStatus = new Status(status);
        
        List<Status> statusList = new List<Status>();
        if(unit.unitStatusDic.TryGetValue(status.invokeTime, out statusList))
        {
            if(statusList.Contains(status))
            {
                Status currentStauts = statusList.Where(e => e.statusName == status.statusName).FirstOrDefault();
                currentStauts.durationTurn += status.durationTurn;
            }
            else
            {
                unit.unitStatusDic[status.invokeTime].Add(status);
            }
        }
    }

    // 상태이상 제거
    public void RemStatus(Unit unit, Status status)
    {
        List<Status> statusList = new List<Status>();
        if(unit.unitStatusDic.TryGetValue(status.invokeTime, out statusList))
        {
            if(!statusList.Contains(status))
            {
                Debug.LogWarning(string.Format("{0} status is not found. Can't Remove do it.", status.statusName));
                return;
            }

            unit.unitStatusDic[status.invokeTime].Remove(status);
        }
    }

    public void StatusTurnChange(Unit unit)
    {
        foreach(var x in unit.unitStatusDic)
        {
            if(x.Value.Count > 0)
                x.Value.ForEach(x => x.RemDuration(unit));
        }
    }
}
