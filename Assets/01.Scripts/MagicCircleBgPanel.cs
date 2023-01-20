using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class MagicCircleBgPanel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private MagicCircle _magicCircle;

    [SerializeField]
    private TestMagicCircle _testMagicCircle;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_magicCircle != null)
        {
            _magicCircle.IsBig = false;
        }

        if (_testMagicCircle != null)
        {
            _testMagicCircle.IsBig = false;
        }
    }
}
