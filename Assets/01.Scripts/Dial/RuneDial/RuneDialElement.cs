using DG.Tweening;
using MyBox;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RuneDialElement : DialElement<BaseRuneUI, BaseRune>
{
    public override BaseRuneUI SelectElement
    {
        get
        {
            //if (_selectCard == null)
            //    _selectCard = _runeList[0];
            return _selectElement;
        }
        set
        {
            if (value == null)
            {
                if (_selectElement != null)
                {
                    //_selectCard.SetActiveOutline(OutlineType.Default);
                    _selectElement.RuneColor(new Color(0.26f, 0.26f, 0.26f, 1f));
                    _selectElement = value;
                }
                else
                {
                    if (_selectElement != null)
                    {
                        //_selectCard.SetActiveOutline(OutlineType.Default);
                        _selectElement.RuneColor(new Color(0.26f, 0.26f, 0.26f, 1f));
                    }

                    _selectElement = value;

                    //_selectElement.SetActiveOutline(OutlineType.Cyan);
                    _selectElement.RuneColor(Color.white);

                }
                _selectIndex = _elementList.FindIndex(x => x == _selectElement);
                (_dial as RuneDial).CheckResonance();
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        _isAttackCondition = BattleManager.Instance.Enemy.IsDie == false && _selectElement != null;
        _isRotateAdditionalCondition = BattleManager.Instance.IsPlayerTurn();
        OnSelectElementAction -= () => Define.DialScene?.CardDescPopup(SelectElement == null ? null : SelectElement.Rune);
        OnSelectElementAction += () => Define.DialScene?.CardDescPopup(SelectElement == null ? null : SelectElement.Rune);
    }

    protected override void ChangeSelectElement(int index)
    {
        if (index == -1)
        {
            SelectElement = null;
        }
        else
        {
            if (_elementList[index].Rune.IsCoolTime == false)
            {
                SelectElement = _elementList[index];

                if (_isTouchDown == true)
                {
                    if (_selectElement != null)
                    {
                        OnSelectElementAction?.Invoke();
                        //Define.DialScene?.CardDescPopup(_selectElement.Rune);
                    }
                }
            }
        }
    }

    public override void Attack()
    {
        if (_isAttackCondition)
        {
            if (SelectElement.Rune.AbilityCondition())
            {
                SelectElement.Rune.AbilityAction();

                SelectElement = null;
            }
        }
    }
}