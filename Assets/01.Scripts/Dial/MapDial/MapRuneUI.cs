using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRuneUI : MonoBehaviour
{
    private Action _action;

    public void SetInfo(Action action)
    {
        _action = action;
    }

    public Action ClickAction()
    {
        return _action;
    }
}
