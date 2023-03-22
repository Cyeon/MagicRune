using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoSingleton<CanvasManager>
{
    private Dictionary<string, Canvas> _canvasDict = new Dictionary<string, Canvas>();

    public void SetCanvas()
    {
        Canvas[] canvasArray = FindObjectsOfType<Canvas>(); // 수정해야함
    }

    public Canvas GetCanvas(string name)
    {
        if (_canvasDict.ContainsKey(name))
        {
            return _canvasDict[name];
        }

        return null;
    }

    public void Clear()
    {
        _canvasDict.Clear();
    }
}
