using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyBGUI : MonoBehaviour
{
    [SerializeField] RectTransform bgParent;
    [SerializeField]
    private RectTransform _selectPanel;
    [SerializeField]
    private float[] xPosArray;

    public Ease moveBGEase;
    public float moveBGSpeed;

    private int _index = -1;

    #region Swipe Parameta
    private Vector2 touchBeganPos;
    private Vector2 touchEndedPos;
    private Vector2 touchDif;
    [SerializeField]
    private float swipeSensitivity = 5;
    #endregion

    private void Start()
    {
        bgParent.DOAnchorPosX(0, 0).SetEase(moveBGEase);
        _selectPanel.DOAnchorPosX(xPosArray[0 + 1], 0).SetEase(moveBGEase);
    }

    public void MoveBG(int leftRightToMain)
    {
        _index = leftRightToMain;
        bgParent.DOAnchorPosX(1440 * leftRightToMain, moveBGSpeed).SetEase(moveBGEase);
        _selectPanel.DOAnchorPosX(xPosArray[leftRightToMain + 1], moveBGSpeed).SetEase(moveBGEase);
    }

    public void GameStart()
    {
        Managers.Scene.LoadScene(Define.Scene.MapScene);
    }

    private void Update()
    {
        Swipe1();
    }

    public void Swipe1()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchBeganPos = touch.position;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                
            }
            if (touch.phase == TouchPhase.Ended)
            {
                touchEndedPos = touch.position;
                touchDif = (touchEndedPos - touchBeganPos);

                if (Mathf.Abs(touchDif.y) > swipeSensitivity || Mathf.Abs(touchDif.x) > swipeSensitivity)
                {
                    if (touchDif.y > 0 && Mathf.Abs(touchDif.y) > Mathf.Abs(touchDif.x))
                    {
                        //Debug.Log("up");
                    }
                    else if (touchDif.y < 0 && Mathf.Abs(touchDif.y) > Mathf.Abs(touchDif.x))
                    {
                        //Debug.Log("down");
                    }
                    else if (touchDif.x > 0 && Mathf.Abs(touchDif.y) < Mathf.Abs(touchDif.x))
                    {
                        //Debug.Log("right");
                        _index = _index + 1;
                        if (_index > 2)
                            _index = -1;

                        MoveBG(_index);
                    }
                    else if (touchDif.x < 0 && Mathf.Abs(touchDif.y) < Mathf.Abs(touchDif.x))
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
