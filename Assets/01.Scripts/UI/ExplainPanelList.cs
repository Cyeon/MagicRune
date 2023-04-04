using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplainPanelList : MonoBehaviour
{
    private ExplainPanel[] _explaingPanelArray;

    public bool IsAllClose
    {
        get
        {
            if (_explaingPanelArray == null || _explaingPanelArray.Length <= 0)
                return false;

            for (int i = 0; i < _explaingPanelArray.Length; i++)
            {
                if (_explaingPanelArray[i].gameObject.activeSelf == true)
                {
                    return false;
                }
            }

            return true;
        }
    }

    private void OnEnable()
    {
        PanelPositionSort();
    }

    void Awake()
    {
        _explaingPanelArray = GetComponentsInChildren<ExplainPanel>(true);
    }

    public void OpenPanel(int index, Rune rune)
    {
        if (_explaingPanelArray == null || _explaingPanelArray.Length <= 0)
        {
            _explaingPanelArray = GetComponentsInChildren<ExplainPanel>(true);
        }

        _explaingPanelArray[index].gameObject.SetActive(true);
        _explaingPanelArray[index].SetUI(rune);

        PanelPositionSort();
    }

    public void ClosePanel(int index)
    {
        _explaingPanelArray[index].gameObject.SetActive(false);

        PanelPositionSort();
    }

    public void AllOpenPanel()
    {
        for (int i = 0; i < _explaingPanelArray.Length; i++)
        {
            _explaingPanelArray[i].gameObject.SetActive(true);
            
        }
    }

    public void AllClosePanel()
    {
        for (int i = 0; i < _explaingPanelArray.Length; i++)
        {
            _explaingPanelArray[i].gameObject.SetActive(false);
        }
    }

    public void PanelPositionSort()
    {
        if (IsAllClose == true) return;

        List<int> numberList = new List<int>();
        int count = 0;
        for(int i = 0; i < _explaingPanelArray.Length; i++)
        {
            if (_explaingPanelArray[i].gameObject.activeSelf == true)
            {
                count++;
                numberList.Add(i);
            }
        }
        if (count <= 0)
            return;

        float distance = Screen.width / (count + 1);

        for (int i = 0; i < numberList.Count; i++)
        {
            if(_explaingPanelArray[numberList[i]].gameObject.activeSelf == true)
            {
                _explaingPanelArray[numberList[i]].transform.localPosition = new Vector3(distance * (i + 1), _explaingPanelArray[numberList[i]].transform.localPosition.y, 0);
            }
        }
    }
}
