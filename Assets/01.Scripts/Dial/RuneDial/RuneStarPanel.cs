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
                        (_dial.DialElementList[2] as RuneDialElement).EffectHandler.EditAllEffectScale(1f);
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
                        (_dial.DialElementList[2 - i] as RuneDialElement).EffectHandler.EditEffectScale(3 - i, 1.5f);
                    }
                    else
                    {
                        _dial.MagicCircleGlow(2 - i, false);
                        (_dial.DialElementList[2 - i] as RuneDialElement).EffectHandler.EditEffectScale(3 - i, 1f);
                    }
                }

                if (Managers.Swipe.TouchDif.y < 0)
                {
                    _dial.AllMagicCircleGlow(false);
                    (_dial.DialElementList[2] as RuneDialElement).EffectHandler.EditAllEffectScale(1f);
                    //return;
                }
            }
        });
        Managers.Swipe.AddAction(SwipeType.UpSwipe, (touch) =>
        {
            float distance = Vector2.Distance(Define.MainCam.ScreenToWorldPoint(Managers.Swipe.TouchBeganPos), (Vector2)this.transform.position);
            if (distance >= _inDistance && distance <= _outDistance && BattleManager.Instance.Enemy.IsDie == false)
            {
                _dial.Attack();
            }
            else
            {
                _dial.AllMagicCircleGlow(false);
                (_dial.DialElementList[2] as RuneDialElement).EffectHandler.EditAllEffectScale(1f);
            }
        });
        Managers.Swipe.AddAction(SwipeType.DownSwipe, (touch) =>
        {
            _dial.AllMagicCircleGlow(false);
            (_dial.DialElementList[2] as RuneDialElement).EffectHandler.EditAllEffectScale(1f);
        });
        Managers.Swipe.AddAction(SwipeType.Touch, (touch) =>
        {
            _dial.AllMagicCircleGlow(false);
            (_dial.DialElementList[2] as RuneDialElement).EffectHandler.EditAllEffectScale(1f);
        });
        Managers.Swipe.AddAction(SwipeType.TouchEnd, (touch) =>
        {
            _dial.AllMagicCircleGlow(false);
            (_dial.DialElementList[2] as RuneDialElement).EffectHandler.EditAllEffectScale(1f);
        });
        #endregion
    }
}
