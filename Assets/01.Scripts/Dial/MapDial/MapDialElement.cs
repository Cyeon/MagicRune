using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDialElement : DialElement<MapRuneUI, MapRuneUI>
{
    public override MapRuneUI SelectElement
    {
        get => _selectElement;
        set
        {
            if (value == null)
            {
                if (_selectElement != null)
                {
                    //_selectCard.SetActiveOutline(OutlineType.Default);
                }
                _selectElement = value;
            }
            else
            {
                if (_selectElement != null)
                {
                    //_selectCard.SetActiveOutline(OutlineType.Default);
                }
                _selectElement = value;
                //_selectCard.SetActiveOutline(OutlineType.Cyan);
            }
        }
    }

    protected override void ChangeSelectElement(int index)
    {
        if(index == -1)
        {
            _selectElement = null;
        }
        else
        {
            _selectElement = _elementList[index];
            Managers.Map.MapScene.MapDescChange(_elementList[index]);
        }
    }

    public override void Attack()
    {
        _elementList.Remove(SelectElement);
        SelectElement = null;
    }
}
