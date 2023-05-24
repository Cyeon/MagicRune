using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStartPanel : StarPanel<MapRuneUI, MapRuneUI>
{
    protected override void Start()
    {
        base.Start();

        #region Add Event
        Managers.Swipe.AddAction(SwipeType.TouchMove, (touch) =>
        {
            if (Mathf.Abs(Vector2.Distance(transform.position, Define.MainCam.ScreenToWorldPoint(Managers.Swipe.TouchBeganPos))) <= _outDistance)
            {
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

                if (touchDif.y < 0)
                {
                    _dial.AllMagicCircleGlow(false);
                    //return;
                }
            }
        });

        Managers.Swipe.AddAction(SwipeType.UpSwipe, (touch) =>
        {
            float distance = Vector2.Distance(Define.MainCam.ScreenToWorldPoint(Managers.Swipe.TouchBeganPos), (Vector2)transform.position);
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
            _dial.AllMagicCircleGlow(false);
        });
        #endregion
    }
}
