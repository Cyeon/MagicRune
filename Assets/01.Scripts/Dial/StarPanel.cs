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

                //ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½. ï¿½ï¿½Ä¡ï¿½ï¿½ xï¿½Ìµï¿½ï¿½Å¸ï¿½ï¿½ï¿½ yï¿½Ìµï¿½ï¿½Å¸ï¿½ï¿½ï¿½ ï¿½Î°ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ Å©ï¿½ï¿½
                if (Mathf.Abs(touchDif.y) > swipeSensitivity || Mathf.Abs(touchDif.x) > swipeSensitivity)
                {
                    if (touchDif.y > 0 && Mathf.Abs(touchDif.y) > Mathf.Abs(touchDif.x))
                    {
                        //Debug.Log("up");
                        // ½ÃÀÛ À§Ä¡°¡ ½ºÅ¸ ¾ÈÀÌ¸é
                        var ped = new PointerEventData(null);
                        ped.position = touchBeganPos;
                        List<RaycastResult> results = new List<RaycastResult>();
                        UIManager.Instance.GR.Raycast(ped, results);

                        if (results.Count > 0)
                        {
                            if (results[0].gameObject.CompareTag("Star"))
                            {
                                Debug.Log("°ø°Ý!");
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
                //ï¿½ï¿½Ä¡.
                else
                {
                    //Debug.Log("touch");
                }
            }
        }
    }
}
