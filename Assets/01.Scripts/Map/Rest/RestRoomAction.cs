using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MyBox;

public class RestRoomAction : MonoBehaviour
{
    [SerializeField]
    private bool _isRest;
    [SerializeField]
    private AdventureSO[] _action;

    [ConditionalField(nameof(_isRest), false, false)]
    private EnhanceType _enhanceType;

    #region Swipe Parameta
    private Vector2 touchBeganPos;
    private Vector2 touchEndedPos;
    private Vector2 touchDif;
    [SerializeField]
    private float swipeSensitivity = 5;
    #endregion

    [SerializeField]
    private float _inDistance;
    [SerializeField]
    private float _outDistance;

    public void Action()
    {
        if (_isRest)
        {
            foreach(var list in _action[0].distractors)
            {
                list.function?.Invoke();
            }
        }
    }
}
