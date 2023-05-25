using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwipeType
{
    TouchBegan,
    TouchMove,
    TouchEnd,
    Touch,
    LeftSwipe,
    RightSwipe,
    UpSwipe,
    DownSwipe,
    MAX_COUNT,
}

public class SwipeManager
{
    #region Swipe Parameta
    private Vector2 _touchBeganPos;
    public Vector2 TouchBeganPos => _touchBeganPos;
    private Vector2 _touchEndedPos;
    public Vector2 TouchEndedPos => _touchEndedPos;
    private Vector2 _touchDif;
    public Vector2 TouchDif => _touchDif;
    private float _swipeSensitivity = 450;
    public float SwipeSensitivity => _swipeSensitivity;
    #endregion

    #region Actions
    private Dictionary<SwipeType, Action<Touch>> _swipeDict = new Dictionary<SwipeType, Action<Touch>>();
    #endregion

    public void Init()
    {
        if(_swipeDict == null || _swipeDict.Count == 0)
        {
            for(int i = 0; i < (int)SwipeType.MAX_COUNT; i++)
            {
                _swipeDict.Add((SwipeType)i, null);
            }
        }
    }

    public void EditSwipeSensitivity(float value = 450f)
    {
        _swipeSensitivity = value;
    }

    public void AddAction(SwipeType swipeType, Action<Touch> action)
    {
        if (_swipeDict.ContainsKey(swipeType) == true)
        {
            _swipeDict[swipeType] -= action;
            _swipeDict[swipeType] += action;
        }
        else
        {
            _swipeDict.Add(swipeType, action);
        }
    }

    public void Update(float delta)
    {
        Swipe();
    }

    public void Swipe()
    {
        if (Input.touchCount > 0 && _swipeDict != null)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _touchBeganPos = touch.position;

                if(_swipeDict.ContainsKey(SwipeType.TouchBegan) == true)
                {
                    _swipeDict[SwipeType.TouchBegan]?.Invoke(touch);
                }
            }
            if (touch.phase == TouchPhase.Moved)
            {
                if (_swipeDict.ContainsKey(SwipeType.TouchMove) == true)
                {
                    _swipeDict[SwipeType.TouchMove]?.Invoke(touch);
                }
            }
            if (touch.phase == TouchPhase.Ended)
            {
                _touchEndedPos = touch.position;
                _touchDif = (_touchEndedPos - _touchBeganPos);

                if (_swipeDict.ContainsKey(SwipeType.TouchEnd) == true)
                {
                    _swipeDict[SwipeType.TouchEnd]?.Invoke(touch);
                }

                if (Mathf.Abs(_touchDif.y) > _swipeSensitivity || Mathf.Abs(_touchDif.x) > _swipeSensitivity)
                {
                    if (_touchDif.y > 0 && Mathf.Abs(_touchDif.y) > Mathf.Abs(_touchDif.x))
                    {
                        //Debug.Log("up");
                        if (_swipeDict.ContainsKey(SwipeType.UpSwipe) == true)
                        {
                            _swipeDict[SwipeType.UpSwipe]?.Invoke(touch);
                        }
                    }
                    else if (_touchDif.y < 0 && Mathf.Abs(_touchDif.y) > Mathf.Abs(_touchDif.x))
                    {
                        //Debug.Log("down");
                        if (_swipeDict.ContainsKey(SwipeType.DownSwipe) == true)
                        {
                            _swipeDict[SwipeType.DownSwipe]?.Invoke(touch);
                        }
                    }
                    else if (_touchDif.x > 0 && Mathf.Abs(_touchDif.y) < Mathf.Abs(_touchDif.x))
                    {
                        //Debug.Log("right");
                        if (_swipeDict.ContainsKey(SwipeType.RightSwipe) == true)
                        {
                            _swipeDict[SwipeType.RightSwipe]?.Invoke(touch);
                        }
                    }
                    else if (_touchDif.x < 0 && Mathf.Abs(_touchDif.y) < Mathf.Abs(_touchDif.x))
                    {
                        //Debug.Log("Left");
                        if (_swipeDict.ContainsKey(SwipeType.LeftSwipe) == true)
                        {
                            _swipeDict[SwipeType.LeftSwipe]?.Invoke(touch);
                        }
                    }
                }
                else
                {
                    //Debug.Log("touch");
                    if (_swipeDict.ContainsKey(SwipeType.Touch) == true)
                    {
                        _swipeDict[SwipeType.Touch]?.Invoke(touch);
                    }
                }
            }
        }
    }

    public void Clear()
    {
        _swipeDict.Clear();

        Init();
    }
}
