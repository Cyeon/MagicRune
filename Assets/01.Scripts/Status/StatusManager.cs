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
        status.ForEach(x => x.StatusInvoke());
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

        switch(status.invokeTime)
        {
            case StatusInvokeTime.Start:
                AddStatus(unit.OnTurnStartStatus, newStatus);
                break;

            case StatusInvokeTime.Attack:
                AddStatus(unit.OnAttackStatus, newStatus);
                break;

            case StatusInvokeTime.End:
                AddStatus(unit.OnTurnStopStatus, newStatus);
                break;
        }
        
    }

    private void AddStatus(List<Status> list, Status status)
    {
        if(list.Contains(status))
        {
            Status currentStauts = list.Where(e => e.statusName == status.statusName).FirstOrDefault();
            currentStauts.currentTurn += status.durationTurn;
        }
        else
        {
            list.Add(status);
        }
    }

    // 상태이상 제거
    public void RemStatus(Unit unit, Status status)
    {
        switch(status.invokeTime)
        {
            case StatusInvokeTime.Start:
                RemStatus(unit.OnTurnStartStatus, status);
                break;

            case StatusInvokeTime.Attack:
                RemStatus(unit.OnAttackStatus, status);
                break;

            case StatusInvokeTime.End:
                RemStatus(unit.OnTurnStopStatus, status);
                break;
        }
    }

    private void RemStatus(List<Status> list, Status status)
    {
        if(!list.Contains(status))
        {
            Debug.LogWarning(string.Format("{0} status is not found. Can't Remove do it."));
            return;
        }

        list.Remove(status);
    }
}
