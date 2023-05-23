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
                }
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
                
                //_selectCard.SetActiveOutline(OutlineType.Cyan);
                _selectElement.RuneColor(Color.white);

            }
            (_dial as RuneDial).CheckResonance();
        }
    }

    protected override bool _isAttackCondition => BattleManager.Instance.Enemy.IsDie == false && _selectElement != null;
    protected override bool _isRotateAdditionalCondition => BattleManager.Instance.IsPlayerTurn();

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
                        OnSelectElementAction();
                        //Define.DialScene?.CardDescPopup(_selectElement.Rune);
                    }
                }
            }
        }
    }

    protected override void OnSelectElementAction()
    {
        Define.DialScene?.CardDescPopup(SelectElement == null ? null : SelectElement.Rune);
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