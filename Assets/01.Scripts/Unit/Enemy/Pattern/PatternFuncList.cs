using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternFuncList : MonoBehaviour
{
    private float addDmg;

    public void AddAtkDmg(float dmg)
    {
        GameManager.Instance.enemy.atkDamage += dmg;
        addDmg = dmg;
    }

    public void RemAtkDmg()
    {
        GameManager.Instance.enemy.atkDamage -= addDmg;
    }

    public void AddShield(float shield)
    {
        GameManager.Instance.enemy.Shield += shield;
    }

    public void AddStatus(StatusName statusName)
    {
        StatusManager.Instance.AddStatus(GameManager.Instance.player, statusName);
    }

    public void Attack()
    {
        Invoke("DelayAttack", 1f);
    }

    private void DelayAttack()
    {
        GameManager.Instance.enemy.Attack();
    }

    public void DelayShake()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(Camera.main.DOShakeRotation(2));
        seq.AppendCallback(() => GameManager.Instance.TurnChange());
    }
}
