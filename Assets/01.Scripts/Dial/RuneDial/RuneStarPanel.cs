using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RuneStarPanel : StarPanel<BaseRuneUI, BaseRune>
{
    protected override void Start()
    {
        base.Start();

        #region Add Event
        Managers.Swipe.AddAction(SwipeType.TouchMove, (touch) =>
        {
            if (Mathf.Abs(Vector2.Distance(transform.position, Define.MainCam.ScreenToWorldPoint(Managers.Swipe.TouchBeganPos))) <= _outDistance)
            {
                for (int i = 0; i < _dial.DialElementList.Count; i++)
                {
                    if (_dial.DialElementList[i].DialState == DialState.Drag)
                    {
                        _dial.AllMagicCircleGlow(false);
                        return;
                    }
                }

                Vector2 touchDif = (touch.position - Managers.Swipe.TouchBeganPos);

                int count = (int)(Mathf.Abs(touchDif.y) / (Managers.Swipe.SwipeSensitivity / 3));
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

                if (Managers.Swipe.TouchDif.y < 0)
                {
                    _dial.AllMagicCircleGlow(false);
                    //return;
                }
            }
        });
        Managers.Swipe.AddAction(SwipeType.UpSwipe, (touch) =>
        {
            float distance = Vector2.Distance(Define.MainCam.ScreenToWorldPoint(Managers.Swipe.TouchBeganPos), (Vector2)this.transform.position);
            if (distance >= _inDistance && distance <= _outDistance)
            {
                _dial.Attack();
            }
            else
            {
                _dial.AllMagicCircleGlow(false);
            }
        });
        Managers.Swipe.AddAction(SwipeType.DownSwipe, (touch) =>
        {
            _dial.AllMagicCircleGlow(false);
        });
        Managers.Swipe.AddAction(SwipeType.Touch, (touch) =>
        {
            if (Vector2.Distance(transform.position, Define.MainCam.ScreenToWorldPoint(Managers.Swipe.TouchEndedPos)) <= _outDistance)
            {
                Define.DialScene?.AllCardDescPopup();
            }
            _dial.AllMagicCircleGlow(false);
        });
        #endregion
    }
}
