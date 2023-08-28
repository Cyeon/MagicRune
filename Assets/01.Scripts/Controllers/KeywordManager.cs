using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class KeywordManager
{
    private List<Keyword> _keywardList = new List<Keyword>();

    public void Init()
    {
        if(_keywardList.Count <= 0)
        {
            _keywardList = Managers.Resource.Load<KeywardListSO>("SO/" + typeof(KeywardListSO).Name).KeywardList;
        }
    }

    public Keyword GetKeyword(KeywordName typeName)
    {
        Keyword keyward = _keywardList.Find(x => x.TypeName == typeName);
        if(keyward == null)
        {
            keyward = new Keyword();
        }

        return keyward;
    }

    public Keyword GetKeyward(string name)
    {
        Keyword keyward = _keywardList.Find(x => x.KeywardName == name);
        if (keyward == null)
        {
            keyward = new Keyword();
        }

        return keyward;
    }
}
