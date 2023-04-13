using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MyBox;
using System;

[Serializable]
public abstract class BaseRune : MonoBehaviour
{
    #region Rune Stat Parameta
    [SerializeField]
    protected BaseRuneSO _baseRuneSO;
    public BaseRuneSO BaseRuneSO => _baseRuneSO;

    private int _coolTime;
    public int CoolTIme => _coolTime;

    public bool IsCoolTime => _coolTime > 0;
    #endregion

    #region UI Parameta
    private SpriteRenderer _runeSpriteRenderer;
    #endregion

    private void Start()
    {
        _runeSpriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();

        if(_baseRuneSO != null)
        {
            _runeSpriteRenderer.sprite = _baseRuneSO.RuneSprite;
        }

        RuneColor(new Color(0.26f, 0.26f, 0.26f, 1f));
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
