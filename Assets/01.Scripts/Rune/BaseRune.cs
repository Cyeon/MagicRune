using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MyBox;
using System;
using Random = UnityEngine.Random;
using static UnityEngine.Rendering.DebugUI;

[Serializable]
public class BaseRune : Item, ICloneable
{
    #region Constructor
    public BaseRune()
    {

    }

    public BaseRune(BaseRune rune)
    {
        _baseRuneSO = rune.BaseRuneSO;
        _coolTime = rune.CoolTime;
        _isUsing = rune.IsUsing;
    }
    #endregion

    #region Rune Stat Parameta
    [SerializeField]
    protected BaseRuneSO _baseRuneSO;
    public BaseRuneSO BaseRuneSO => _baseRuneSO;

    private int _coolTime;
    public int CoolTime => _coolTime;

    public bool IsCoolTime => _coolTime > 0;

    private bool _isUsing = false;
    public bool IsUsing => _isUsing;

    private bool _isEnhanced = false;
    public bool IsEnhanced => _isEnhanced;

    public KeywordName[] KeywordList => (_isEnhanced ? _baseRuneSO.EnhancedKeywardList : _baseRuneSO.KeywardList);
    #endregion

    #region Item Interface
    public Sprite Icon
    {
        get
        {
            if (_baseRuneSO != null)
            {
                return _baseRuneSO.RuneSprite;
            }
            return null;
        }
    }

    public int Gold { get; private set; }

    public ShopItemType ShopItemType { get => ShopItemType.Rune; }
    public BaseRune Rune => this;
    #endregion

    public virtual void Init()
    {

    }


    public void SetRandomGold(int start, int end)
    {
        Gold = Random.Range(start, end + 1);
    }

    public virtual void Execute()
    {
        Managers.Deck.AddRune(Managers.Rune.GetRune(this));
    }

    public void SetCoolTime()
    {
        _coolTime = _baseRuneSO.CoolTime;
    }

    public void SetCoolTime(int value)
    {
        _coolTime = value;
    }

    public void AddCoolTime(int value)
    {
        _coolTime += value;
    }

    public void SetIsUsing(bool value)
    {
        _isUsing = value;
    }

    public virtual void Enhance()
    {
        _isEnhanced = true;
    }

    public virtual void UnEnhance()
    {
        _isEnhanced = false;
    }

    public virtual bool AbilityCondition()
    {
        return true;
    }

    public virtual void AbilityAction()
    {
    }

    public float GetAbliltiValue(EffectType type, StatusName status = StatusName.Null, float ? value = null)
    {
        float? currentValue = 0;
        if (value != null)
        {
            currentValue = value.Value;
        }
        else
        {
            if (status == StatusName.Null)
            {
                currentValue = (_isEnhanced ? _baseRuneSO.EnhancedAbilityList : _baseRuneSO.AbilityList).Where(x => x.EffectType == type).Select(x => x.Value).FirstOrDefault();
            }
            else
            {
                currentValue = (_isEnhanced ? _baseRuneSO.EnhancedAbilityList : _baseRuneSO.AbilityList).Where(x => x.EffectType == type && x.StatusName == status).Select(x => x.Value).FirstOrDefault();
            }
        }

        if (currentValue.HasValue)
        {
            Managers.StatModifier.GetStatModifierValue(type, ref currentValue);

            return currentValue.Value;
        }

        return 0;
    }

    public float GetValue(EffectType type, StatusName status = StatusName.Null)
    {
        float value = 0;
        if (status == StatusName.Null)
        {
            value = (_isEnhanced ? _baseRuneSO.EnhancedAbilityList : _baseRuneSO.AbilityList).Where(x => x.EffectType == type).Select(x => x.Value).FirstOrDefault();
        }
        else
        {
            value = (_isEnhanced ? _baseRuneSO.EnhancedAbilityList : _baseRuneSO.AbilityList).Where(x => x.EffectType == type && x.StatusName == status).Select(x => x.Value).FirstOrDefault();
        }

        return value;
    }

    public virtual object Clone()
    {
        BaseRune rune = new BaseRune();
        rune.Init();
        rune._isEnhanced = false;
        return rune;
    }

    public bool IsIncludeKeyword(KeywordName keyward)
    {
        return KeywordList.Contains(keyward);
    }
}
