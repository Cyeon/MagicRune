using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicManager
{
    private List<Relic> _useRelicList = new List<Relic>();
    private List<Relic> _continuousRelicList = new List<Relic>();

    public void AddRelic(string relicName)
    {
        Relic relic = Managers.Resource.Instantiate("Relic/Relic_" + relicName, Managers.GetPlayer().relicTrm).GetComponent<Relic>();
        if (relic == null) return;

        if(relic.relicType == RelicType.Use)
        {
            _useRelicList.Add(relic);
            (relic as IUseHandler).Use();
        }
        else if(relic.relicType == RelicType.Continuous)
        {
            _continuousRelicList.Add(relic);
        }

        Debug.Log(relic.debugName);
    }

    public void ContinuousExecute(string relicName)
    {
        for(int i = 0; i < _continuousRelicList.Count; ++i)
        {
            if (_continuousRelicList[i].relicName == relicName)
            {
                (_continuousRelicList[i] as IContinuousHandler).Execute();
            }
        }
    }
}
