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
            Status currentStauts = statusList.Where(e => e.statusName == status.statusName).FirstOrDefault();
            if(currentStauts != null)
            {
                currentStauts.durationTurn += status.durationTurn;
                UIManager.Instance.ReloadStatusPanel(unit, currentStauts.statusName, currentStauts.durationTurn);
            }
            else
            {
                unit.unitStatusDic[newStatus.invokeTime].Add(newStatus);
                UIManager.Instance.AddStatus(unit, newStatus);
            }
        }
    }

    // 상태이상 제거
    public void RemStatus(Unit unit, Status status)
    {
        List<Status> statusList = new List<Status>();
        if(unit.unitStatusDic.TryGetValue(status.invokeTime, out statusList))
        {
            Status currentStauts = statusList.Where(e => e.statusName == status.statusName).FirstOrDefault();
            if (currentStauts != null)
            {
                unit.unitStatusDic[status.invokeTime].Remove(status);
            }
            else
            {
                Debug.LogWarning(string.Format("{0} status is not found. Can't Remove do it.", status.statusName));
            }
        }
    }

    public void StatusTurnChange(Unit unit)
    {
        foreach(var x in unit.unitStatusDic)
        {
            List<int> indexes = new List<int>();
            for(int i = 0; i < x.Value.Count; i++)
            {
                x.Value[i].durationTurn--;
                UIManager.Instance.ReloadStatusPanel(unit, x.Value[i].statusName, x.Value[i].durationTurn);
                if (x.Value[i].durationTurn <= 0)
                {
                    indexes.Add(i);
                }
            }

            indexes.ForEach(e => RemStatus(unit, x.Value[e]));
        }
    }
}
