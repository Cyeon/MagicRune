using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPanel : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private RuneDial _dial;

    #region Swipe Parameta
    private Vector2 _touchBeganPos;
    private Vector2 _touchEndedPos;
    private Vector2 _touchDif;
    [SerializeField]
    private float _swipeSensitivity = 450;
    #endregion

    [SerializeField]
    private float _inDistance;
    [SerializeField]
    private float _outDistance;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        //_image.alphaHitTestMinimumThreshold = 0.1f;
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
                _touchBeganPos = touch.position;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                if (Mathf.Abs(Vector2.Distance(transform.position, Define.MainCam.ScreenToWorldPoint(_touchBeganPos))) <= _outDistance)
                {
                    for (int i = 0; i < _dial.DialElementList.Count; i++)
                    {
                        if (_dial.DialElementList[i].DialState == DialState.Drag)
                        {
                            _dial.AllMagicCircleGlow(false);
                            return;
                        }
                    }

                    _touchDif = (touch.position - _touchBeganPos);

                    int count = (int)(Mathf.Abs(_touchDif.y) / (_swipeSensitivity / 3));
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

                    if (_touchDif.y < 0)
                    {
                        _dial.AllMagicCircleGlow(false);
                        //return;
                    }
                }
            }
            if (touch.phase == TouchPhase.Ended)
            {
                _touchEndedPos = touch.position;
                _touchDif = (_touchEndedPos - _touchBeganPos);

                //?좎룞?쇿뜝?숈삕?좎룞?쇿뜝?숈삕. ?좎룞?숈튂?좎룞??x?좎떛?몄삕?좎떊紐뚯삕?좎룞??y?좎떛?몄삕?좎떊紐뚯삕?좎룞???좎떥怨ㅼ삕?좎룞?쇿뜝?숈삕?좎룞???у뜝?숈삕
                if (Mathf.Abs(_touchDif.y) > _swipeSensitivity || Mathf.Abs(_touchDif.x) > _swipeSensitivity)
                {
                    if (_touchDif.y > 0 && Mathf.Abs(_touchDif.y) > Mathf.Abs(_touchDif.x))
                    {
                        //Debug.Log("up");

                        float distance = Vector2.Distance(Define.MainCam.ScreenToWorldPoint(_touchBeganPos), (Vector2)transform.position);
                        if (distance >= _inDistance && distance <= _outDistance)
                        {
                            _dial.Attack();
                        }
                        else
                        {
                            _dial.AllMagicCircleGlow(false);
                        }
                    }
                    else if (_touchDif.y < 0 && Mathf.Abs(_touchDif.y) > Mathf.Abs(_touchDif.x))
                    {
                        //Debug.Log("down");
                        _dial.AllMagicCircleGlow(false);
                    }
                    else if (_touchDif.x > 0 && Mathf.Abs(_touchDif.y) < Mathf.Abs(_touchDif.x))
                    {
                        //Debug.Log("right");
                    }
                    else if (_touchDif.x < 0 && Mathf.Abs(_touchDif.y) < Mathf.Abs(_touchDif.x))
                    {
                        //Debug.Log("Left");
                    }
                }
                //?좎룞?숈튂.
                else
                {
                    //Debug.Log("touch");
                    if (Vector2.Distance(transform.position, Define.MainCam.ScreenToWorldPoint(_touchEndedPos)) <= _outDistance)
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
