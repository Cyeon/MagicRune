using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class RuneManager
{
    private List<BaseRune> _runeHandler = new List<BaseRune>();

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
        _runeHandler.Add(new GroundBeat());
        _runeHandler.Add(new Bouncing());

        _runeHandler.Add(new Charge());
        _runeHandler.Add(new RailGun());
        _runeHandler.Add(new LightingRod());
        _runeHandler.Add(new Release());
        _runeHandler.Add(new ElectricBarrier());
        _runeHandler.Add(new ElectricAbsorption());
        _runeHandler.Add(new SelfGeneration());

        _runeHandler.Add(new MagicBullet());
        _runeHandler.Add(new MagicShield());

        for (int i = 0; i < _runeHandler.Count; i++)
        {
            _runeHandler[i].Init();
        }
    }

    public List<BaseRune> GetRuneList()
    {
        return _runeHandler;
    }

    public BaseRune GetRandomRuneOfRarity(RuneRarity rarity, List<BaseRune> ignoreRuneList = null)
    {
        List<BaseRune> newRuneList = new List<BaseRune>(_runeHandler);
        if (ignoreRuneList != null)
        {
            for (int i = 0; i < ignoreRuneList.Count; i++)
            {
                if (newRuneList.Contains(ignoreRuneList[i]) == true)
                {
                    newRuneList.Remove(ignoreRuneList[i]);
                }
            }
        }

        newRuneList = newRuneList.Where(x => x.BaseRuneSO.Rarity == rarity).ToList();

        int idx = Random.Range(0, newRuneList.Count);
        return newRuneList[idx].Clone() as BaseRune;
    }

    public BaseRune GetRandomRune(List<BaseRune> ignoreRuneList = null)
    {
        List<BaseRune> newRuneList = new List<BaseRune>(_runeHandler);

        if (ignoreRuneList != null)
        {
            for (int i = 0; i < ignoreRuneList.Count; i++)
            {
                if (newRuneList.Contains(ignoreRuneList[i]) == true)
                {
                    newRuneList.Remove(ignoreRuneList[i]);
                }
            }
        }
        int idx = Random.Range(0, newRuneList.Count);
        return newRuneList[idx].Clone() as BaseRune;
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
        for (int i = 0; i < newRuneList.Count; i++)
        {
            numberList.Add(i);
        }

        for (int i = 0; i < count; i++)
        {
            if (numberList.Count <= 0) break;
            int randomIndex = Random.Range(0, numberList.Count);
            runeList.Add(newRuneList[numberList[randomIndex]].Clone() as BaseRune);
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
