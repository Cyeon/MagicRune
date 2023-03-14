using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StarPanel : MonoBehaviour
{
    private Image _image;

    [SerializeField]
    private Dial _dial;

    #region Swipe Parameta
    private Vector2 touchBeganPos;
    private Vector2 touchEndedPos;
    private Vector2 touchDif;
    [SerializeField]
    private float swipeSensitivity = 5;
    #endregion

    private void Start()
    {
        _image = GetComponent<Image>();
        _image.alphaHitTestMinimumThreshold = 0.1f;
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

                //��������. ��ġ�� x�̵��Ÿ��� y�̵��Ÿ��� �ΰ������� ũ��
                if (Mathf.Abs(touchDif.y) > swipeSensitivity || Mathf.Abs(touchDif.x) > swipeSensitivity)
                {
                    if (touchDif.y > 0 && Mathf.Abs(touchDif.y) > Mathf.Abs(touchDif.x))
                    {
                        //Debug.Log("up");
                        var ped = new PointerEventData(null);
                        ped.position = touchBeganPos;
                        List<RaycastResult> results = new List<RaycastResult>();
                        UIManager.Instance.GR.Raycast(ped, results);

                        if (results.Count > 0)
                        {
                            if (results[0].gameObject.CompareTag("Star"))
                            {
                                _dial.Attack();
                            }
                        }
                    }
                    else if (touchDif.y < 0 && Mathf.Abs(touchDif.y) > Mathf.Abs(touchDif.x))
                    {
                        //Debug.Log("down");
                    }
                    else if (touchDif.x > 0 && Mathf.Abs(touchDif.y) < Mathf.Abs(touchDif.x))
                    {
                        //Debug.Log("right");
                    }
                    else if (touchDif.x < 0 && Mathf.Abs(touchDif.y) < Mathf.Abs(touchDif.x))
                    {
                        //Debug.Log("Left");
                    }
                }
                //��ġ.
                else
                {
                    //Debug.Log("touch");
                }
            }
        }
    }
}
