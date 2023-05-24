using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestDialElement : DialElement<RestRuneUI, RestRuneUI>
{
    public override RestRuneUI SelectElement
    {
        get => _selectElement;
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
        }
    }

    protected override void ChangeSelectElement(int index)
    {
        if (index == -1)
        {
            SelectElement = null;
        }
        else
        {
            SelectElement = _elementList[index];
            if (_isTouchDown == true)
            {
                if (_selectElement != null)
                {
                    (_dial as RestDial).EditText(_selectElement.Desc);
                }
            }
        }
    }

    protected override void OnSelectElementAction()
    {
        if (_selectElement != null)
            (_dial as RestDial).EditText(_selectElement.Desc);
    }
}