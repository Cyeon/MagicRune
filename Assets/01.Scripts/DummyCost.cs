using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DummyCost : MonoSingleton<DummyCost>
{
    // 싱글턴 패턴도 이런거에서는 안쓸거고
    
    // 이건 나중에 만들 땐 다른데서 관리해줘야 할 듯
    [SerializeField] private TextMeshProUGUI _manaText;

    private int _maxCost = 10;
    public int MaxCost => _maxCost;
    [SerializeField]
    private int _nowCost = 10;
    public int NowCost => _nowCost;

    private void Awake()
    {
        EventManager.StartListening(Define.ON_START_PLAYER_TURN, ManaFill);
        UpdateManaText();
    }

    // 이거는 여기에서 cost를 매개변수로 보내서
    // 텍스트 있는데에서 받아주고 매개변수 받아서 그 값으로 바꿔야 할 듯
    public void UpdateManaText()
    {
         _manaText.SetText($"{_nowCost}/{_maxCost}");
    }

    public void ManaFill()
    {
        _nowCost = _maxCost;
        UpdateManaText();
    }

    public bool CanUseMainRune(int cost)
    {
        if (_nowCost >= cost)
        {
            _nowCost -= cost;
            UpdateManaText();
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
            UpdateManaText();
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