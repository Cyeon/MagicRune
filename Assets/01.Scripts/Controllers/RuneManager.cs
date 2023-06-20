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
        _runeHandler.Add(new MagicSpree());
        _runeHandler.Add(new VariableRune());

        for (int i = 0; i < _runeHandler.Count; i++)
        {
            _runeHandler[i].Init();
        }
    }

    public List<BaseRune> GetRuneList()
    {
        return _runeHandler;
    }

    [Obsolete]
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

    [Obsolete]
    public List<BaseRune> GetRandomRuneOfRarity(int count, RuneRarity rarity, List<BaseRune> ignoreRuneList = null)
    {
        return null;
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

        #region Set Attribute
        AttributeType attributeType = AttributeType.None;
        int attributeMaxValue = 0;
        for(int i = 1; i < (int)AttributeType.MAX_COUNT; i++)
        {
            // 만약 선택 룬 속성이라면 10이 아니라 더 큰 수를 더하면 됨
            attributeMaxValue += 10;
        }
        int attributeValue = Random.Range(0, attributeMaxValue + 1);
        for(int i = 1; i < (int)AttributeType.MAX_COUNT; i++)
        {
            if((i - 1) * 10 <= attributeValue && i * 10 >= attributeValue)
            {
                attributeType = (AttributeType)i;
                break;
            }
        }
        #endregion

        #region Set Rarity
        RuneRarity rarity = RuneRarity.Normal;
        RuneRarity[] rarityArray = Enum.GetValues(typeof(RuneRarity)).Cast<RuneRarity>().ToArray();

        int rarityMaxValue = 0;
        for(int i = 0; i < rarityArray.Length; i++)
        {
            rarityMaxValue += (int)rarityArray[i];
        }

        int rarityValue = Random.Range(1, rarityMaxValue + 1);
        int rarityMinValue = 0;
        for(int i = 0; i < rarityArray.Length; i++)
        {
            if(rarityMinValue <= rarityValue && rarityValue >= (int)rarityArray[i + 1])
            {
                rarity = rarityArray[i];
                break;
            }
            else
            {
                rarityMinValue += (int)rarityArray[i + 1];
            }
        }
        #endregion

        newRuneList = newRuneList.Where(x => x.BaseRuneSO.AttributeType == attributeType && x.BaseRuneSO.Rarity == rarity).ToList();

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

        #region Set Attribute
        AttributeType attributeType = AttributeType.None;
        int attributeMaxValue = 0;
        for (int i = 1; i < (int)AttributeType.MAX_COUNT; i++)
        {
            // 만약 선택 룬 속성이라면 10이 아니라 더 큰 수를 더하면 됨
            attributeMaxValue += 10;
        }
        int attributeValue = Random.Range(0, attributeMaxValue + 1);
        for (int i = 1; i < (int)AttributeType.MAX_COUNT; i++)
        {
            if ((i - 1) * 10 <= attributeValue && i * 10 >= attributeValue)
            {
                attributeType = (AttributeType)i;
                break;
            }
        }
        #endregion

        #region Set Rarity
        RuneRarity rarity = RuneRarity.Normal;
        RuneRarity[] rarityArray = Enum.GetValues(typeof(RuneRarity)).Cast<RuneRarity>().ToArray();

        int rarityMaxValue = 0;
        for (int i = rarityArray.Length - 1; i >= 1 ; i--)
        {
            rarityMaxValue += (int)rarityArray[i];
        }

        int rarityValue = Random.Range(1, rarityMaxValue + 1);
        int rarityMinValue = 0;
        for (int i = rarityArray.Length - 1; i >= 1; i--)
        {
            if (rarityMinValue <= rarityValue && rarityValue >= (int)rarityArray[i + 1])
            {
                rarity = rarityArray[i];
                break;
            }
            else
            {
                rarityMinValue += (int)rarityArray[i + 1];
            }
        }
        #endregion

        newRuneList = newRuneList.Where(x => x.BaseRuneSO.AttributeType == attributeType && x.BaseRuneSO.Rarity == rarity).ToList();

        List<int> numberList = new List<int>();
        for (int i = 0; i < newRuneList.Count; i++)
        {
            numberList.Add(i);
        }

        for (int i = 0; i < count; i++)
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
