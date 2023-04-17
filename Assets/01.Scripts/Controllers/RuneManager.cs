using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneManager
{
    private AllRuneListSO _runeList;

    private List<BaseRune> _runeHandler = new List<BaseRune>();

    public void Init()
    {
        _runeList = Managers.Resource.Load<AllRuneListSO>("SO/" + typeof(AllRuneListSO).Name);
    }

    public BaseRune GetRandomRuneOfRarity(RuneRarity rarity, List<BaseRune> ignoreRuneList = null)
    {


        return ignoreRuneList[0]; // юс╫ц╥н
    }

    public BaseRune GetRandomRune(List<BaseRune> ignoreRuneList = null)
    {
        List<BaseRune> newRuneList = new List<BaseRune>(_runeList.BaseRuneList);

        if(ignoreRuneList != null)
        {
            for (int i = 0; i < ignoreRuneList.Count; i++)
            {
                newRuneList.Remove(ignoreRuneList[i]);
            }
        }

        return newRuneList[Random.Range(0, newRuneList.Count)];
    }

    public List<BaseRune> GetRandomRune(int count, List<BaseRune> ignoreRuneList = null)
    {
        List<BaseRune> runeList = new List<BaseRune>();

        List<BaseRune> newRuneList = _runeList.BaseRuneList;
        if (ignoreRuneList != null)
        {
            for (int i = 0; i < ignoreRuneList.Count; i++)
            {
                if (newRuneList.Contains(ignoreRuneList[i]))
                {
                    newRuneList.Remove(ignoreRuneList[i]);
                }
            }
        }

        List<int> numberList = new List<int>();
        for(int i = 0; i < newRuneList.Count; i++)
        {
            numberList.Add(i);
        }

        for(int i = 0; i < count; i++)
        {
            if (numberList.Count <= 0) break;
            int randomIndex = Random.Range(0, numberList.Count);
            runeList.Add(newRuneList[numberList[randomIndex]]);
            numberList.RemoveAt(randomIndex);
        }

        return runeList;
    }
}
