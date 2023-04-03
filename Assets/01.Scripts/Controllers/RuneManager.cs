using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneManager : MonoBehaviour
{
    // ���� �� ��ü�� ���� �ִ� ����Ʈ? ������ �־�� ��

    private AllRuneListSO _runeList;

    private void Awake()
    {
        _runeList = ResourceManager.Instance.Load<AllRuneListSO>("SO" + typeof(AllRuneListSO).Name);
    }

    public Rune GetRandomRuneOfRarity(RuneRarity rarity, List<RuneSO> ignoreRuneList)
    {
        return new Rune(ignoreRuneList[0]); // �ӽ÷�
    }

    public Rune GetRandomRune(List<RuneSO> ignoreRuneList)
    {
        return new Rune(ignoreRuneList[0]); // �ӽ÷�
    }
}
