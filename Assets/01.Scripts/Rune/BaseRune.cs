using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MyBox;
using System;
using Random = UnityEngine.Random;
using Unity.VisualScripting;

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
    #endregion

    #region Item Interface
    public Sprite Icon
    {
        get
        {
            if(_baseRuneSO != null)
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

    public virtual bool AbilityCondition()
    {
        Debug.Log("BaseRune Condition");
        return true;
    }

    public virtual void AbilityAction()
    {
        if(BattleManager.Instance.Player.StatusManager.IsHaveStatus(StatusName.Heating))
        {
            BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Fire, 1);
        }
    }

    public float GetAbliltiValaue(EffectType type)
    {
        float? value = _baseRuneSO.AbilityList.Where(x => x.EffectType == type).Select(x => x.Value).FirstOrDefault();

        if (value.HasValue)
        {
            Managers.StatModifier.GetStatModifierValue(type, ref value);

            return value.Value;
        }

        return int.MinValue;
    }

    public virtual object Clone()
    {
        BaseRune rune = new BaseRune();
        return rune;
    }

    protected bool IsIncludeKeyward(KeywardType keyward)
    {
        return _baseRuneSO.KeywardList.Contains(keyward);
    }
}
