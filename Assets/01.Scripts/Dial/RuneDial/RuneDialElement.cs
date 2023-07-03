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
            if (_selectElement != null)
            {
                //_selectCard.SetActiveOutline(OutlineType.Default);
                _selectElement.RuneColor(new Color(0.26f, 0.26f, 0.26f, 1f));
            }
            _selectElement = value;
            if (_selectElement != null)
            {
                //_selectCard.SetActiveOutline(OutlineType.Cyan);
                _selectElement.RuneColor(Color.white);
            }
            (_dial as RuneDial).CheckResonance();
        }
    }

    protected override bool _isAttackCondition => /*BattleManager.Instance.Enemy.IsDie == false && */_selectElement != null;
    protected override bool _isRotateAdditionalCondition => BattleManager.Instance.IsPlayerTurn() && _dial.IsAttack == false;

    private RuneEffectHandler _effectHandler;
    public RuneEffectHandler EffectHandler => _effectHandler;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void ChangeSelectElement(int index)
    {
        if (_dial.IsAttack == true) return;

        if(_effectHandler == null)
        {
            _effectHandler = Managers.GetPlayer().Visual.GetComponentInChildren<RuneEffectHandler>();
        }

        if (index == -1)
        {
            if (SelectElement == null) return;
            SelectElement = null;
            _effectHandler.EditEffect(null, _lineID);
        }
        else
        {
            if(SelectElement == _elementList[index]) return;
            if (_elementList[index].Rune.IsCoolTime == false)
            {
                SelectElement = _elementList[index];
                _effectHandler.EditEffect(SelectElement.Rune.BaseRuneSO.RuneEffect, _lineID);

                if (_isTouchDown == true)
                {
                    if (SelectElement != null)
                    {
                        OnSelectElementAction();
                        //Define.DialScene?.CardDescPopup(SelectElement.Rune);
                    }
                }
            }
        }

        //_effectHandler.EditEffect(SelectElement == null ? null : SelectElement.Rune.BaseRuneSO.RuneEffect, _lineID);
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