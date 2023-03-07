using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PatternFuncEnum
{
    AddAtkDmg,
    RemAtkDmg,
    AddShield,
    AddIceStatus,
    Attack,
    ShieldUse,
    TurnChange,
    DelayShake,
    Beeeeem,
    TurnSkip
}

public class PatternFuncList : MonoBehaviour
{
    private float _addDmg;
    [HideInInspector] public float value;

    [SerializeField] private ParticleSystem _shieldEffect;

    [Header("Sound")]
    public AudioClip attackSound = null;
    public AudioClip shieldSound = null;
    public AudioClip beamSound = null;

    public void OnDestroy()
    {
        transform.DOKill();
    }

    public void FuncInvoke(PatternFuncEnum funcName)
    {
        Invoke(funcName.ToString() , 0f);
    }

    public void AddAtkDmg()
    {
        AttackManager.Instance.enemy.atkDamage += value;
        _addDmg = value;
    }

    public void RemAtkDmg()
    {
        AttackManager.Instance.enemy.atkDamage -= _addDmg;
    }

    public void AddShield()
    {
        AttackManager.Instance.enemy.Shield += value;
    }

    public void AddIceStatus()
    {
        AddStatus(StatusName.Ice);
    }

    private void AddStatus(StatusName statusName)
    {
        float v = value > 0 ? value : 1;
        StatusManager.Instance.AddStatus(AttackManager.Instance.player, statusName, (int)v);
    }

    public void Attack()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => StartCoroutine(UIManager.Instance.PatternIconAnimationCoroutine()));
        seq.Append(UIManager.Instance.enemyIcon.DOShakePosition(0.6f, 50, 5)).SetEase(Ease.Linear);
        seq.Append(UIManager.Instance.enemyIcon.DOLocalMoveY(-1700f, 0.2f)).SetEase(Ease.Linear);
        seq.AppendCallback(() => DelayAttack());
        seq.Append(UIManager.Instance.enemyIcon.DOLocalMoveY(130, 0.2f)).SetEase(Ease.Linear);
        seq.AppendInterval(0.1f);
        seq.AppendCallback(() => AttackManager.Instance.TurnChange());
    }

    private void DelayAttack()
    {
        AttackManager.Instance.enemy.Attack();
    }

    public void ShieldUse()
    {
        _shieldEffect.gameObject.SetActive(true);
        _shieldEffect.Play();
        Invoke("TurnChange", 2f);
    }

    public void TurnChange()
    {
        AttackManager.Instance.TurnChange();
    }

    public void DelayShake()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(Camera.main.DOShakeRotation(2));
        seq.AppendCallback(() => AttackManager.Instance.TurnChange());
    }

    public void Beeeeem()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(UIManager.Instance.enemyIcon.DOShakeRotation(2, 90, 5)).SetEase(Ease.Linear);
        SoundManager.Instance.PlaySound(beamSound, SoundType.Effect);
        seq.AppendCallback(() => AttackManager.Instance.enemy.isSkip = true);
        seq.AppendCallback(() => AttackManager.Instance.player.TakeDamage(value));
        seq.AppendInterval(0.2f);
        seq.AppendCallback(() => AttackManager.Instance.TurnChange());
    }

    public void TurnSkip()
    {
        AttackManager.Instance.enemy.isSkip = false;
        AttackManager.Instance.TurnChange();
    }
}
