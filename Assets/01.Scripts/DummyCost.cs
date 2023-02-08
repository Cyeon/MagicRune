using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DummyCost : MonoSingleton<DummyCost>
{
    private int _maxCost = 10;
    public int MaxCost => _maxCost;
    [SerializeField]
    private int _nowCost = 10;
    public int NowCost => _nowCost;

    private int _uiCost = 0;

    [SerializeField]
    private List<GameObject> _emptyManaObjs = new List<GameObject>();

    private void Awake()
    {
        EventManager.StartListening(Define.ON_START_PLAYER_TURN, ManaFill);

        for(int i = 0; i < transform.childCount; i++)
        {
            _emptyManaObjs.Add(transform.GetChild(i).GetChild(0).gameObject);
        }

        UpdateManaUI();
    }

    // 이거는 여기에서 cost를 매개변수로 보내서
    // 텍스트 있는데에서 받아주고 매개변수 받아서 그 값으로 바꿔야 할 듯
    public void UpdateManaUI()
    {
        if (_uiCost == _nowCost) return;

        if(_uiCost < _nowCost)
        {
            for(int i = _uiCost; i < _nowCost; i++)
            {
                _emptyManaObjs[i].SetActive(false);
            }
        }
        else
        {
            for(int i = _uiCost; i > _nowCost - 1; i--)
            {
                _emptyManaObjs[i].SetActive(true);
            }
        }

        _uiCost = _nowCost - 1;
    }

    public void ManaFill()
    {
        _nowCost = _maxCost;
        UpdateManaUI();
    }

    public bool CanUseMainRune(int cost)
    {
        if (_nowCost >= cost)
        {
            _nowCost -= cost;
            UpdateManaUI();
            return true;
        }
        else
            return false;
    }

    public bool CanMainRune(int cost)
    {
        return _nowCost >= cost;
    }

    public bool CanUseSubRune(int cost)
    {
        if (_nowCost >= cost)
        {
            _nowCost -= cost;
            UpdateManaUI();
            return true;
        }
        else
            return false;
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Define.ON_START_PLAYER_TURN, ManaFill);
    }
}