using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RuneManager
{
    private List<BaseRune> _runeHandler = new List<BaseRune>();
    private Dictionary<string, BaseRune> _runeNameDict = new Dictionary<string, BaseRune>();

    public void Init()
    {
        _runeHandler.Add(new Fire());
        _runeHandler.Add(new FirePunch());
        _runeHandler.Add(new FireRegeneration());
        _runeHandler.Add(new FireBreath());

        _runeHandler.Add(new Ice());
        _runeHandler.Add(new SnowBall());
        _runeHandler.Add(new IceShield());
        _runeHandler.Add(new IceSmash());

        _runeHandler.Add(new ShieldAttack());
        _runeHandler.Add(new Attack());
        _runeHandler.Add(new ThreeAttack());
        _runeHandler.Add(new GroundShield());

        _runeHandler.Add(new Charge());
        _runeHandler.Add(new RailGun());
        _runeHandler.Add(new LightingRod());
        _runeHandler.Add(new Release());

        _runeHandler.Add(new MagicBullet());
        _runeHandler.Add(new MagicShield());

        for(int i = 0; i < _runeHandler.Count; i++)
        {
            _runeHandler[i].Init();
            //_runeNameDict.Add(_runeHandler[i].GetType().Name, _runeHandler[i]);
        }

        foreach(var list in _runeNameDict)
        {
            Debug.Log($"{list.Key}, {list.Value}");
        }
    }

    public BaseRune GetRandomRuneOfRarity(RuneRarity rarity, List<BaseRune> ignoreRuneList = null)
    {


        return ignoreRuneList[0]; // юс╫ц╥н
    }

    public BaseRune GetRandomRune(List<BaseRune> ignoreRuneList = null)
    {
        List<BaseRune> newRuneList = new List<BaseRune>(_runeHandler);

        if (ignoreRuneList != null)
        {
            for (int i = 0; i < ignoreRuneList.Count; i++)
            {
                newRuneList.Remove(ignoreRuneList[i]);
            }
        }
        int idx = Random.Range(0, newRuneList.Count);
        return newRuneList[idx];
    }

    public List<BaseRune> GetRandomRune(int count, List<BaseRune> ignoreRuneList = null)
    {
        List<BaseRune> runeList = new List<BaseRune>();

        List<BaseRune> newRuneList = new List<BaseRune>(_runeHandler);
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

    public BaseRune GetRune(BaseRuneSO runeSO)
    {
        return _runeHandler.Find(x => x.BaseRuneSO == runeSO).Clone() as BaseRune;
    }

    public BaseRune GetRune(BaseRune rune)
    {
        return _runeHandler.Find(x => x.BaseRuneSO == rune.BaseRuneSO).Clone() as BaseRune;
    }
}
