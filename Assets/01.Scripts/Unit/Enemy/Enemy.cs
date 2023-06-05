using DG.Tweening;
using MyBox;
using TMPro;
using UnityEngine;

public class Enemy : Unit
{
    public enum EnemyType
    {
        Ice,
        Fire
    }

    [Header("Enemy Info")]
    public string enemyName;
    public EnemyType enemyType = EnemyType.Ice;

    public bool isEnter = false;

    private PatternManager _patternManager;
    public PatternManager PatternManager => _patternManager;

    #region UI

    [Header("UI")]
    public SpriteRenderer spriteRenderer;
    #endregion

    public virtual void Init()
    {
        if (_patternManager == null)
            _patternManager = GetComponentInChildren<PatternManager>();

        _patternManager.Init();
        UISetting();

        transform.localPosition = new Vector3(2.5f, 4.5f, 0);
    }

    public override void Attack(float damage, bool isTrueDamage = false)
    {
        base.Attack(damage);
        BattleManager.Instance.Player.TakeDamage(currentDmg, isTrueDamage);
    }

    public override void Die()
    {
        _isDie = true;

        Sequence seq = DOTween.Sequence();
        seq.Append(spriteRenderer.DOFade(0, 0.75f));
        seq.Join(spriteRenderer.transform.DOMoveY(spriteRenderer.transform.position.y - 1f, 0.75f));
        seq.AppendInterval(0.5f);
        seq.AppendCallback(() =>
        {
            OnDieEvent?.Invoke();
            Managers.Resource.Destroy(gameObject);
            StopAllCoroutines();
        });

    }

    private void OnDestroy()
    {
        //DOTween.KillAll();
        transform.DOKill();
    }
}
