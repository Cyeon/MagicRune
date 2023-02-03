using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternFuncList : MonoBehaviour
{
    private float _addDmg;
    [HideInInspector] public float value;

    [SerializeField] private ParticleSystem _shieldEffect;

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

        if (System.Enum.TryParse(statusName, out status))
            StatusManager.Instance.AddStatus(GameManager.Instance.player, status);
        else
            Debug.LogError(string.Format("{0} status is not found.", statusName));
    }

    public void Attack()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(UIManager.Instance.enemyIcon.DOShakePosition(1, 50, 5)).SetEase(Ease.Linear);
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
        seq.AppendCallback(() => GameManager.Instance.enemy.isSkip = true);
        seq.AppendCallback(() => GameManager.Instance.player.TakeDamage(20));
        seq.AppendInterval(0.2f);
        seq.AppendCallback(() => GameManager.Instance.TurnChange());
    }

    public void OnDestroy()
    {
        transform.DOKill();
    }
}
