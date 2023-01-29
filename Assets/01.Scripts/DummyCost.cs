using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DummyCost : MonoSingleton<DummyCost>
{
    // �̱��� ���ϵ� �̷��ſ����� �Ⱦ��Ű�
    
    // �̰� ���߿� ���� �� �ٸ����� ��������� �� ��
    [SerializeField] private TextMeshProUGUI _manaText;

    private int _maxCost = 10;
    public int MaxCost => _maxCost;
    
    private int _nowCost = 10;
    public int NowCost => _nowCost;

    private void Start()
    {
        UpdateManaText();
    }

    // �̰Ŵ� ���⿡�� cost�� �Ű������� ������
    // �ؽ�Ʈ �ִµ����� �޾��ְ� �Ű����� �޾Ƽ� �� ������ �ٲ�� �� ��
    public void UpdateManaText()
    {
         _manaText.SetText($"{_nowCost}/{_maxCost}");
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
}