using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CanvasManager : MonoSingleton<CanvasManager>
{
    [SerializeField]
    private bool _isOnlyParentObject; // 자식 캔버스느 거리고 싶을 때

    private Dictionary<string, Canvas> _canvasDict = new Dictionary<string, Canvas>();

    public void SetCanvas()
    {
        Clear();
        
        Canvas[] canvasArray = GetCanvasArray();
        for(int i = 0; i < canvasArray.Length; i++)
        {
            _canvasDict.Add(canvasArray[i].name, canvasArray[i]);
        }

        foreach(var c in _canvasDict)
        {
            Debug.Log($"{c.Key} : {c.Value}, {c.Value.transform.parent}");
        }
    }

    public Canvas[] GetCanvasArray()
    {
        Canvas[] canvasArray = FindObjectsOfType<Canvas>(true);

        if (_isOnlyParentObject)
        {
            //Canvas[] parentCanvasArray = canvasArray.ForEach(x => x.transform.parent == null).ToArray();
            Canvas[] parentCanvasArray = canvasArray.Where(x => x.transform.parent == null).ToArray();

            return parentCanvasArray;
        }

        return canvasArray;
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
