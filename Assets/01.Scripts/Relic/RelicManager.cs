using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicManager
{
    private List<Relic> _useRelicList = new List<Relic>();
    private List<Relic> _continuousRelicList = new List<Relic>();

    public void AddRelic(RelicName relicName)
    {
        Relic relic = Managers.Resource.Instantiate("Relic/Relic_" + relicName, Managers.GetPlayer().relicTrm).GetComponent<Relic>();
        if(relic.relicType == RelicType.Use)
        {
            _useRelicList.Add(relic);
            (relic as IUseHandler).Use();
        }

        Debug.Log(relic.debugName);
    }
}
