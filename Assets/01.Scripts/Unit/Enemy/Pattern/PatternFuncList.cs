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
    TurnSkip,
    DrainAttack,
    ShieldAttack,
    ShieldKeep,
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
        BattleManager.Instance.enemy.atkDamage += value;
        _addDmg = value;
    }

    public void RemAtkDmg()
    {
        BattleManager.Instance.enemy.atkDamage -= _addDmg;
    }

    public void AddShield()
    {
        BattleManager.Instance.enemy.Shield += value;
    }

    public void AddIceStatus()
    {
        AddStatus(StatusName.Ice);
    }

    private void AddStatus(StatusName statusName)
    {
        float v = value > 0 ? value : 1;
        StatusManager.Instance.AddStatus(BattleManager.Instance.player, statusName, (int)v);
    }

    public void Attack()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => StartCoroutine(UIManager.Instance.PatternIconAnimationCoroutine()));
        seq.Append(UIManager.Instance.enemyIcon.DOShakePosition(0.6f, 0.5f, 15)).SetEase(Ease.Linear);
        seq.Append(UIManager.Instance.enemyIcon.DOMoveY(-10f, 0.2f)).SetEase(Ease.Linear);
        seq.AppendCallback(() => DelayAttack());
        seq.Append(UIManager.Instance.enemyIcon.DOMoveY(5.82f, 0.2f)).SetEase(Ease.Linear);
        seq.AppendInterval(0.1f);
        seq.AppendCallback(() => BattleManager.Instance.TurnChange());
    }

    private void DelayAttack()
    {
        BattleManager.Instance.enemy.Attack();
    }

    public void ShieldUse()
    {
        _shieldEffect.gameObject.SetActive(true);
        //_shieldEffect.Play();
        Invoke("TurnChange", 2f);
    }

    public void TurnChange()
    {
        BattleManager.Instance.TurnChange();
    }

    public void DelayShake()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(Camera.main.DOShakeRotation(2));
        seq.AppendCallback(() => BattleManager.Instance.TurnChange());
    }

    public void Beeeeem()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(UIManager.Instance.enemyIcon.DOShakeRotation(2, 90, 5)).SetEase(Ease.Linear);
        SoundManager.Instance.PlaySound(beamSound, SoundType.Effect);
        seq.AppendCallback(() => BattleManager.Instance.enemy.isSkip = true);
        seq.AppendCallback(() => BattleManager.Instance.player.TakeDamage(value));
        seq.AppendInterval(0.2f);
        seq.AppendCallback(() => BattleManager.Instance.TurnChange());
    }

    public void TurnSkip()
    {
        BattleManager.Instance.enemy.isSkip = false;
        BattleManager.Instance.TurnChange();
    }

    public void DrainAttack()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => StartCoroutine(UIManager.Instance.PatternIconAnimationCoroutine()));
        seq.Append(UIManager.Instance.enemyIcon.DOShakePosition(0.6f, 50, 5)).SetEase(Ease.Linear);
        seq.Append(UIManager.Instance.enemyIcon.DOLocalMoveY(-1700f, 0.2f)).SetEase(Ease.Linear);
        seq.AppendCallback(() => DelayAttack());
        seq.Append(UIManager.Instance.enemyIcon.DOLocalMoveY(130, 0.2f)).SetEase(Ease.Linear);
        seq.AppendInterval(0.1f);
        seq.AppendCallback(() => BattleManager.Instance.enemy.AddHP(value));
        seq.AppendCallback(() => UIManager.Instance.UpdateHealthbar(false));
        seq.AppendCallback(() => BattleManager.Instance.TurnChange());
    }

    public void ShieldAttack()
    {
        value = BattleManager.Instance.enemy.Shield;
        Attack();
    }

    public void ShieldKeep()
    {
        BattleManager.Instance.enemy.Shield += BattleManager.Instance.enemy.Shield;
    }
}
