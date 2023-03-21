using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDefine : MonoBehaviour
{
    private static MapUI _ui = null;
    public static MapUI MapSceneUI
    {
        get
        {
            if(_ui == null)
                _ui = FindObjectOfType<MapUI>();

            return _ui;
        }
    }
}
