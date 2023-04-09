using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneManager
{
    private AllRuneListSO _runeList;

    public void Init()
    {
        _runeList = Managers.Resource.Load<AllRuneListSO>("SO/" + typeof(AllRuneListSO).Name);
    }

    public Rune GetRandomRuneOfRarity(RuneRarity rarity, List<RuneSO> ignoreRuneList = null)
    {
        return new Rune(ignoreRuneList[0]); // юс╫ц╥н
    }

    public Rune GetRandomRune(List<RuneSO> ignoreRuneList = null)
    {
        List<RuneSO> newRuneList = new List<RuneSO>(_runeList.RuneList);

        for(int i = 0; i < ignoreRuneList.Count; i++)
        {
            newRuneList.Remove(ignoreRuneList[i]);
        }

        return new Rune(newRuneList[Random.Range(0, newRuneList.Count)]);
    }

    public List<Rune> GetRandomRune(int count = 1, List<RuneSO> ignoreRuneList = null)
    {
        List<Rune> runeList = new List<Rune>();

        List<RuneSO> newRuneList = new List<RuneSO>(_runeList.RuneList);
        for (int i = 0; i < ignoreRuneList.Count; i++)
        {
            newRuneList.Remove(ignoreRuneList[i]);
        }

        List<int> numberList = new List<int>();
        for(int i = 0; i < newRuneList.Count; i++)
        {
            numberList.Add(i);
        }

        for(int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, numberList.Count);
            runeList.Add(new Rune(newRuneList[numberList[randomIndex]]));
            numberList.RemoveAt(randomIndex);
        }

        return runeList;
    }
}
