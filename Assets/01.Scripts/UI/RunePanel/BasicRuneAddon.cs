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
            if (_basic == null)
            {
                _basic = GetComponent<BasicRunePanel>();
            }
            return _basic;
        }
    }

    public virtual void SetUI(BaseRuneSO runeSO, bool isEnhance = true)
    {
        Basic.SetUI(runeSO, isEnhance);
    }

    public virtual void SetRune(BaseRune rune)
    {
        Basic.SetRune(rune);
    }
}
