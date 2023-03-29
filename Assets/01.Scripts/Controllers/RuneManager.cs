using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneManager : MonoBehaviour
{
    // 뭔가 룬 전체를 갖고 있는 리스트? 뭐가가 있어야 함

    public Rune GetRandomRuneOfRarity(RuneRarity rarity, List<RuneSO> ignoreRuneList)
    {
        return new Rune(ignoreRuneList[0]); // 임시로
    }

    public Rune GetRandomRune(List<RuneSO> ignoreRuneList)
    {
        return new Rune(ignoreRuneList[0]); // 임시로
    }
}
