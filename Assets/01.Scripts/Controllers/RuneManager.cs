using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneManager : MonoBehaviour
{
    // ���� �� ��ü�� ���� �ִ� ����Ʈ? ������ �־�� ��

    public Rune GetRandomRuneOfRarity(RuneRarity rarity, List<RuneSO> ignoreRuneList)
    {
        return new Rune(ignoreRuneList[0]); // �ӽ÷�
    }

    public Rune GetRandomRune(List<RuneSO> ignoreRuneList)
    {
        return new Rune(ignoreRuneList[0]); // �ӽ÷�
    }
}
