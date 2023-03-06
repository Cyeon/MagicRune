using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void FuncInvoke(string funcName)
    {
        Invoke(funcName, 0f);
    }

    public void AddAtkDmg()
    {
        GameManager.Instance.enemy.atkDamage += value;
        _addDmg = value;
    }

    public void RemAtkDmg()
    {
        GameManager.Instance.enemy.atkDamage -= _addDmg;
    }

    public void AddShield()
    {
        GameManager.Instance.enemy.Shield += value;
    }

    public void AddStatus(string statusName)
    {
        StatusName status;

        float v = value > 0 ? value : 1;

        if (System.Enum.TryParse(statusName, out status))
            StatusManager.Instance.AddStatus(GameManager.Instance.player, status, (int)v);
        else
            Debug.LogError(string.Format("{0} status is not found.", statusName));
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
        seq.AppendCallback(() => GameManager.Instance.TurnChange());
    }

    private void DelayAttack()
    {
        GameManager.Instance.enemy.Attack();
    }

    public void ShieldUse()
    {
        _shieldEffect.gameObject.SetActive(true);
        _shieldEffect.Play();
        Invoke("TurnChange", 2f);
    }

    public void TurnChange()
    {
        GameManager.Instance.TurnChange();
    }

    public void DelayShake()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(Camera.main.DOShakeRotation(2));
        seq.AppendCallback(() => GameManager.Instance.TurnChange());
    }

    public void Beeeeem()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(UIManager.Instance.enemyIcon.DOShakeRotation(2, 90, 5)).SetEase(Ease.Linear);
        SoundManager.Instance.PlaySound(beamSound, SoundType.Effect);
        seq.AppendCallback(() => GameManager.Instance.enemy.isSkip = true);
        seq.AppendCallback(() => GameManager.Instance.player.TakeDamage(value));
        seq.AppendInterval(0.2f);
        seq.AppendCallback(() => GameManager.Instance.TurnChange());
    }

    public void TurnSkip()
    {
        GameManager.Instance.enemy.isSkip = false;
        GameManager.Instance.TurnChange();
    }
}
