using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : PatternAction
{
    public int damage;

    private DialScene _dialScene;

    private void Start()
    {
        _dialScene = SceneManagerEX.Instance.CurrentScene as DialScene;
    }

    public override void TakeAction()
    {
        Sequence seq = DOTween.Sequence();
        //seq.AppendCallback(() => StartCoroutine(_dialScene?.PatternIconAnimationCoroutine()));
        //seq.Append(_dialScene?.EnemyIcon.transform.DOShakePosition(0.6f, 0.5f, 1)).SetEase(Ease.Linear);
        //seq.Append(_dialScene?.EnemyIcon.transform.DOMoveY(-10f, 0.2f)).SetEase(Ease.Linear);
        seq.AppendCallback(() => BattleManager.Instance.enemy.Attack(damage));
        //seq.Append(_dialScene?.EnemyIcon.transform.DOMoveY(5.82f, 0.2f)).SetEase(Ease.Linear);
        seq.AppendInterval(0.1f);
        seq.AppendCallback(() => BattleManager.Instance.TurnChange());
    }
}
