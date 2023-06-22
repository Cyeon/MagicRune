using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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

    public Keyward GetKeyward(KeywordName typeName)
    {
        Keyward keyward = _keywardList.Find(x => x.TypeName == typeName);
        if(keyward == null)
        {
            keyward = new Keyward();
        }

        return keyward;
    }

    public Keyward GetKeyward(string name)
    {
        Keyward keyward = _keywardList.Find(x => x.KeywardName == name);
        if (keyward == null)
        {
            keyward = new Keyward();
        }

        return keyward;
    }
}
