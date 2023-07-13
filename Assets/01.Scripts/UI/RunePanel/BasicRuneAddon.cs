using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicRuneAddon : MonoBehaviour
{
    private BasicRunePanel _basic;
    public BasicRunePanel Basic
    {
        get
        {
            if(_basic == null )
            {
                _basic = GetComponent<BasicRunePanel>();
            }
            return _basic;
        }
    }

    public abstract void SetUI(BaseRune rune, bool isEnhance = true);
}
