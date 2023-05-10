using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class LobbyBGUI : MonoBehaviour
{
    [SerializeField]
    private HorizontalScrollSnap _scrollSnap;
    [SerializeField] RectTransform bgParent;
    [SerializeField]
    private RectTransform _selectPanel;
    [SerializeField]
    private float[] _xPosArray;

    public Ease moveBGEase;
    public float moveBGSpeed;

    private int _index = -1;

    #region Swipe Parameta
    private Vector2 _touchBeganPos;
    private Vector2 _touchEndedPos;
    private Vector2 _touchDif;
    [SerializeField]
    private float _swipeSensitivity = 5;
    #endregion

    private void Start()
    {
        //bgParent.DOAnchorPosX(0, 0).SetEase(moveBGEase);
        //_selectPanel.DOAnchorPosX(_xPosArray[0 + 1], 0).SetEase(moveBGEase);

        _scrollSnap.OnSelectionPageChangedEvent.AddListener(ChangeIndex);
        MoveSelectPanel(_scrollSnap.CurrentPage);
    }

    public void ChangeIndex(int index)
    {
        _index = index;

        MoveSelectPanel(_index);
    }

    public void MoveSelectPanel(int leftRightToMain)
    {
        _selectPanel.DOAnchorPosX(_xPosArray[leftRightToMain], moveBGSpeed).SetEase(moveBGEase);
    }

    [Obsolete]
    public void MoveBG(int leftRightToMain)
    {
        _index = leftRightToMain;
        bgParent.DOAnchorPosX(1440 * leftRightToMain, moveBGSpeed).SetEase(moveBGEase);
    }

    public void GameStart()
    {
        Managers.Scene.LoadScene(Define.Scene.MapScene);
    }

    private void Update()
    {
        // Swipe1();
    }

    public void Swipe1()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _touchBeganPos = touch.position;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                
            }
            if (touch.phase == TouchPhase.Ended)
            {
                _touchEndedPos = touch.position;
                _touchDif = (_touchEndedPos - _touchBeganPos);

                if (Mathf.Abs(_touchDif.y) > _swipeSensitivity || Mathf.Abs(_touchDif.x) > _swipeSensitivity)
                {
                    if (_touchDif.y > 0 && Mathf.Abs(_touchDif.y) > Mathf.Abs(_touchDif.x))
                    {
                        //Debug.Log("up");
                    }
                    else if (_touchDif.y < 0 && Mathf.Abs(_touchDif.y) > Mathf.Abs(_touchDif.x))
                    {
                        //Debug.Log("down");
                    }
                    else if (_touchDif.x > 0 && Mathf.Abs(_touchDif.y) < Mathf.Abs(_touchDif.x))
                    {
                        //Debug.Log("right");
                        _index = _index + 1;
                        if (_index > 2)
                            _index = -1;

                        MoveBG(_index);
                    }
                    else if (_touchDif.x < 0 && Mathf.Abs(_touchDif.y) < Mathf.Abs(_touchDif.x))
                    {
                        //Debug.Log("Left");
                        
                        _index = _index - 1;
                        if (_index < -1)
                            _index = 2;

                        MoveBG(_index);
                    }
                }
                else
                {
                    //Debug.Log("touch");
                }
            }
        }
    }
}
