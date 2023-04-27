using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeywardManager
{
    private List<Keyward> _keywardList = new List<Keyward>();

    public void Init()
    {
        if(_keywardList.Count <= 0)
        {
            _keywardList = Managers.Resource.Load<KeywardListSO>("SO/" + typeof(KeywardListSO).Name).KeywardList;
        }
    }

    public Keyward GetKeyward(StatusName statusName)
    {
        return _keywardList.Find(x => x.StatusName == statusName);
    }

    public Keyward GetKeyward(string name)
    {
        return _keywardList.Find(x => x.KeywardName == name); 
    }
}
