using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicManager
{
    private List<Relic> _relicList = new List<Relic>();

    public void AddRelic(RelicName relicName)
    {
        Relic relic = Managers.Resource.Instantiate("Relic/Relic_" + relicName, Managers.GetPlayer().relicTrm).GetComponent<Relic>();
        if (relic == null) return;

        _relicList.Add(relic);
        relic.OnAdd();
        Debug.Log(relic.debugName);
    }

    public void Reset()
    {
        for(int i = _relicList.Count - 1; i >= 0; i++)
        {
            _relicList[i].OnRemove();
            Managers.Resource.Destroy(_relicList[i].gameObject) ;
        }

        _relicList.Clear();
    }
}
