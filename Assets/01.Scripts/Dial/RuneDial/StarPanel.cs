using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StarPanel : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private Dial _dial;

    private DialScene _dialScene;

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

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        //_image.alphaHitTestMinimumThreshold = 0.1f;

        _dialScene = Managers.Scene.CurrentScene as DialScene;
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
                for(int i = 0; i < _dial.DialElementList.Count; i++)
                {
                    if (_dial.DialElementList[i].DialState == DialState.Drag)
                    {
                        _dial.AllMagicCircleGlow(false);
                        return;
                    }
                }

                touchDif = (touch.position - touchBeganPos);
                
                int count = (int)(Mathf.Abs(touchDif.y) / (swipeSensitivity / 3));
                count = Mathf.Min(count, 3);

                for (int i = 0; i < 3; i++)
                {
                    if (i < count)
                    {
                        _dial.MagicCircleGlow(2 - i, true);
                    }
                    else
                    {
                        _dial.MagicCircleGlow(2 - i, false);
                    }
                }

                if (touchDif.y < 0)
                {
                    _dial.AllMagicCircleGlow(false);
                    //return;
                }
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

                        float distance = Vector2.Distance(Define.MainCam.ScreenToWorldPoint(touchBeganPos), (Vector2)transform.position);
                        if(distance >= _inDistance && distance <= _outDistance)
                        {
                            _dial.Attack();
                        }
                        else
                        {
                            _dial.AllMagicCircleGlow(false);
                        }
                    }
                    else if (touchDif.y < 0 && Mathf.Abs(touchDif.y) > Mathf.Abs(touchDif.x))
                    {
                        //Debug.Log("down");
                        _dial.AllMagicCircleGlow(false);
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
                    if (Vector2.Distance(transform.position, Define.MainCam.ScreenToWorldPoint(touchEndedPos)) <= _outDistance)
                    {
                        Define.DialScene?.AllCardDescPopup();
                    }
                    _dial.AllMagicCircleGlow(false);
                }
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, _inDistance);
        Gizmos.DrawWireSphere(this.transform.position, _outDistance);
        Gizmos.color = Color.white;
    }
#endif
}
