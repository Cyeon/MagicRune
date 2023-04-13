using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MyBox;
using System;
using Random = UnityEngine.Random;

[Serializable]
public abstract class BaseRune : MonoBehaviour, Item
{
    #region Rune Stat Parameta
    [SerializeField]
    protected BaseRuneSO _baseRuneSO;
    public BaseRuneSO BaseRuneSO => _baseRuneSO;

    private int _coolTime;
    public int CoolTIme => _coolTime;

    public bool IsCoolTime => _coolTime > 0;

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
    #endregion

    #region UI Parameta
    private SpriteRenderer _runeSpriteRenderer;
    #endregion

    private void Start()
    {
        _runeSpriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();

        if (_baseRuneSO != null)
        {
            _runeSpriteRenderer.sprite = _baseRuneSO.RuneSprite;
        }

        RuneColor(new Color(0.26f, 0.26f, 0.26f, 1f));
    }

    public void SetRandomGold(int start, int end)
    {
        Gold = Random.Range(start, end + 1);
    }

    public virtual void Execute()
    {
        Debug.Log(_baseRuneSO);
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

    public virtual bool AbilityCondition()
    {
        return true;
    }

    public abstract void AbilityAction();

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

    public void RuneColor(Color color)
    {
        _runeSpriteRenderer.color = color;
    }
}
