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
    private AttributeType _selectAttributeType = AttributeType.None;

    public void Init()
    {
        _runeHandler.Add(new Fire());
        _runeHandler.Add(new FirePunch());
        _runeHandler.Add(new FireRegeneration());
        _runeHandler.Add(new FireBreath());
        _runeHandler.Add(new RapidFire());
        _runeHandler.Add(new Reload());
        _runeHandler.Add(new TheLastShot());

        _runeHandler.Add(new Ice());
        _runeHandler.Add(new SnowBall());
        _runeHandler.Add(new IceShield());
        _runeHandler.Add(new IceSmash());
        _runeHandler.Add(new IceHeart());
        _runeHandler.Add(new AbsorptionChilliness());
        _runeHandler.Add(new BreathOfIceDragon());

        _runeHandler.Add(new ShieldAttack());
        _runeHandler.Add(new Attack());
        _runeHandler.Add(new ThreeAttack());
        _runeHandler.Add(new GroundShield());
        _runeHandler.Add(new GroundBeat());
        _runeHandler.Add(new Bouncing());
        _runeHandler.Add(new DiamondBody());

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

    public void SetSelectAttribute(AttributeType type)
    {
        _selectAttributeType = type;
    }

    public AttributeType GetSelectAttribute()
    {
        return _selectAttributeType;
    }

    public List<BaseRune> GetRuneList()
    {
        return _runeHandler;
    }

    [Obsolete]
    public BaseRune GetRandomRuneOfRarity(RuneRarity rarity, List<BaseRuneSO> ignoreRuneList = null)
    {
        List<BaseRune> runeList = new List<BaseRune>();
        List<BaseRune> newRuneList = new List<BaseRune>(_runeHandler);

        if (ignoreRuneList != null)
        {
            foreach (BaseRune item in newRuneList)
            {
                if (ignoreRuneList.Contains(item.BaseRuneSO))
                {
                    runeList.Add(item);
                }
            }
        }

        for (int i = 0; i < runeList.Count; i++)
        {
            newRuneList.Remove(runeList[i]);
        }

        runeList.Clear();

        newRuneList = newRuneList.Where(x => x.BaseRuneSO.Rarity == rarity).ToList();

        int idx = Random.Range(0, newRuneList.Count);
        return newRuneList[idx].Clone() as BaseRune;
    }

    [Obsolete]
    public List<BaseRune> GetRandomRuneOfRarity(int count, RuneRarity rarity, List<BaseRune> ignoreRuneList = null)
    {
        return null;
    }

    public BaseRune GetRandomRune(List<BaseRuneSO> ignoreRuneList = null)
    {
        List<BaseRune> newRuneList = new List<BaseRune>(_runeHandler);
        List<BaseRune> runeList = new List<BaseRune>();

        if (ignoreRuneList != null)
        {
            foreach (BaseRune item in newRuneList)
            {
                if (ignoreRuneList.Contains(item.BaseRuneSO))
                {
                    runeList.Add(item);
                }
            }
        }

        for (int i = 0; i < runeList.Count; i++)
        {
            newRuneList.Remove(runeList[i]);
        }

        runeList.Clear();

        #region Set Attribute
        AttributeType attributeType = AttributeType.None;
        int attributeMaxValue = 0;
        for (int i = 2; i < (int)AttributeType.MAX_COUNT; i++)
        {
            if (_selectAttributeType == (AttributeType)i)
            {
                attributeMaxValue += 30;
            }
            else
            {
                attributeMaxValue += 10;
            }
        }
        int attributeValue = Random.Range(0, attributeMaxValue + 1);
        int attributeMinValue = 0;
        for (int i = 2; i < (int)AttributeType.MAX_COUNT; i++)
        {
            if (attributeMinValue <= attributeValue && ((AttributeType)i == _selectAttributeType ? 30 : 10) + attributeMinValue >= attributeValue)
            {
                attributeType = (AttributeType)i;
                break;
            }
            else
            {
                attributeMinValue += (AttributeType)i == _selectAttributeType ? 30 : 10;
            }
        }
        #endregion

        #region Set Rarity
        RuneRarity rarity = RuneRarity.Normal;
        RuneRarity[] rarityArray = Enum.GetValues(typeof(RuneRarity)).Cast<RuneRarity>().ToArray();

        int rarityMaxValue = 0;
        for (int i = 0; i < rarityArray.Length; i++)
        {
            rarityMaxValue += (int)rarityArray[i];
        }

        int rarityValue = Random.Range(1, rarityMaxValue + 1);
        int rarityMinValue = 0;
        for (int i = 0; i < rarityArray.Length; i++)
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

        int idx = Random.Range(0, newRuneList.Count);
        return newRuneList[idx].Clone() as BaseRune;
    }

    public List<BaseRune> GetRandomRune(int count, List<BaseRuneSO> ignoreRuneList = null)
    {
        List<BaseRune> runeList = new List<BaseRune>();

        List<BaseRune> newRuneList = new List<BaseRune>(_runeHandler);

        if (ignoreRuneList != null)
        {
            foreach (BaseRune item in newRuneList)
            {
                if (ignoreRuneList.Contains(item.BaseRuneSO))
                {
                    runeList.Add(item);
                }
            }
        }

        for (int i = 0; i < runeList.Count; i++)
        {
            newRuneList.Remove(runeList[i]);
        }

        runeList.Clear();

        Debug.ClearDeveloperConsole();

        while (runeList.Count < count)
        {
            #region Set Attribute
            AttributeType attributeType = AttributeType.None;
            int attributeMaxValue = 0;
            for (int i = 2; i < (int)AttributeType.MAX_COUNT; i++)
            {
                if (_selectAttributeType == (AttributeType)i)
                {
                    attributeMaxValue += 30;
                }
                else
                {
                    attributeMaxValue += 10;
                }
            }
            int attributeValue = Random.Range(0, attributeMaxValue + 1);
            int attributeMinValue = 0;
            for (int i = 2; i < (int)AttributeType.MAX_COUNT; i++)
            {
                if (attributeMinValue <= attributeValue && ((AttributeType)i == _selectAttributeType ? 30 : 10) + attributeMinValue >= attributeValue)
                {
                    attributeType = (AttributeType)i;
                    break;
                }
                else
                {
                    attributeMinValue += (AttributeType)i == _selectAttributeType ? 30 : 10;
                }
            }
            #endregion

            #region Set Rarity
            RuneRarity rarity = RuneRarity.Normal;
            RuneRarity[] rarityArray = Enum.GetValues(typeof(RuneRarity)).Cast<RuneRarity>().ToArray();
            Array.Reverse(rarityArray);

            int rarityMaxValue = 0;
            for (int i = 0; i < rarityArray.Length - 1; i++)
            {
                rarityMaxValue += (int)rarityArray[i];
            }

            int rarityValue = Random.Range(1, rarityMaxValue + 1);
            int rarityMinValue = 0;
            for (int i = 0; i < rarityArray.Length - 1; i++)
            {
                if (rarityMinValue <= rarityValue && rarityValue <= (int)rarityArray[i + 1] + rarityMinValue)
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

            List<BaseRune> list = new List<BaseRune>(newRuneList.Where(x => x.BaseRuneSO.AttributeType == attributeType && x.BaseRuneSO.Rarity == rarity));
            BaseRune rune = list[Random.Range(0, list.Count)];

            bool isIn = false;
            for (int i = 0; i < runeList.Count; i++)
            {
                if (runeList[i].BaseRuneSO == rune.BaseRuneSO)
                {
                    isIn = true;
                    break;
                }
            }

            if (isIn == false)
            {
                runeList.Add(rune.Clone() as BaseRune);
            }
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
