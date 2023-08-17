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
        if (_runeHandler.Count != 0) { return; }

        HandlerAdd(new Fire());
        HandlerAdd(new FirePunch());
        HandlerAdd(new FireRegeneration());
        HandlerAdd(new FireBreath());
        HandlerAdd(new RapidFire());
        HandlerAdd(new Reload());
        HandlerAdd(new TheLastShot());

        HandlerAdd(new Ice());
        HandlerAdd(new SnowBall());
        HandlerAdd(new IceShield());
        HandlerAdd(new IceSmash());
        HandlerAdd(new IceHeart());
        HandlerAdd(new AbsorptionChilliness());
        HandlerAdd(new BreathOfIceDragon());

        HandlerAdd(new ShieldAttack());
        HandlerAdd(new Attack());
        HandlerAdd(new ThreeAttack());
        HandlerAdd(new GroundShield());
        HandlerAdd(new GroundBeat());
        HandlerAdd(new Bouncing());
        HandlerAdd(new DiamondBody());

        HandlerAdd(new Charge());
        HandlerAdd(new RailGun());
        HandlerAdd(new LightingRod());
        HandlerAdd(new Release());
        HandlerAdd(new ElectricBarrier());
        HandlerAdd(new ElectricAbsorption());
        HandlerAdd(new SelfGeneration());

        HandlerAdd(new MagicBullet());
        HandlerAdd(new MagicShield());
        HandlerAdd(new MagicSpree());
        HandlerAdd(new VariableRune());

        for (int i = 0; i < _runeHandler.Count; i++)
        {
            _runeHandler[i].Init();
        }
    }

    private void HandlerAdd(BaseRune rune)
    {
        if (!_runeHandler.Contains(rune))
        {
            _runeHandler.Add(rune);
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

            Debug.Log($"{attributeType}, {rarity}");

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
        return _runeHandler.Find(x => x.BaseRuneSO.name == runeSO.name).Clone() as BaseRune;
    }

    public BaseRune GetRune(BaseRune rune)
    {
        return _runeHandler.Find(x => x.BaseRuneSO == rune.BaseRuneSO).Clone() as BaseRune;
    }
}
