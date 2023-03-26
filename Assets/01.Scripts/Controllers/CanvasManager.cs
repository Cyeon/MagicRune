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

    private void Awake()
    {
        SetCanvas();
    }

    public void SetCanvas()
    {
        Clear();
        
        Canvas[] canvasArray = GetCanvasArray();
        for(int i = 0; i < canvasArray.Length; i++)
        {
            _canvasDict.Add(canvasArray[i].name, canvasArray[i]);
        }
    }

    public void SetCanvas(string name)
    {
        GameObject canvas = GameObject.Find(name);
        if (canvas == null)
        {
            canvas = GameObject.Find(name + "Canvas");

            if (canvas == null)
            {
                canvas = GameObject.Find(name + " Canvas");
                if (canvas == null)
                {
                    return;
                }
                else
                {
                    if (_canvasDict.ContainsKey(name) == false)
                    {
                        _canvasDict.Add(canvas.name, canvas.GetComponent<Canvas>());
                    }
                    else
                    {
                        _canvasDict[canvas.name] = canvas.GetComponent<Canvas>();
                    }
                }
            }
            else
            {
                if (_canvasDict.ContainsKey(name) == false)
                {
                    _canvasDict.Add(canvas.name, canvas.GetComponent<Canvas>());
                }
                else
                {
                    _canvasDict[canvas.name] = canvas.GetComponent<Canvas>();
                }
            }
        }
        else
        {
            if (_canvasDict.ContainsKey(name) == false)
            {
                _canvasDict.Add(canvas.name, canvas.GetComponent<Canvas>());
            }
            else
            {
                _canvasDict[canvas.name] = canvas.GetComponent<Canvas>();
            }
        }
    }

    public Canvas[] GetCanvasArray()
    {
        Canvas[] canvasArray = FindObjectsOfType<Canvas>(true);

        if (_isOnlyParentObject)
        {
            Canvas[] parentCanvasArray = canvasArray.Where(x => x.transform.parent == null).ToArray();

            return parentCanvasArray;
        }

        return canvasArray;
    }

    public Canvas GetCanvas(string name)
    {
        if(_canvasDict.Count == 0)
        {
            SetCanvas();
        }

        if (_canvasDict.ContainsKey(name))
        {
            return _canvasDict[name];
        }
        else
        {
            //SetCanvas(name);

            string canvasName = name + " Canvas";
            if (_canvasDict.ContainsKey(canvasName))
            {
                return _canvasDict[canvasName];
            }
            else
            {
                string canvasNameSecond = name + "Canvas";
                if (_canvasDict.ContainsKey(canvasNameSecond))
                {
                    return _canvasDict[canvasNameSecond];
                }
            }
        }

        return null;
    }

    public int GetCanvasCount()
    {
        return _canvasDict.Count;
    }

    public void Clear()
    {
        _canvasDict.Clear();
    }
}
