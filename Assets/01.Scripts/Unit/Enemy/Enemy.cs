using DG.Tweening;
using MyBox;
using TMPro;
using UnityEngine;

public class Enemy : Unit
{
    public enum EnemyTag
    {
        Ice,
        Fire
    }

    [Header("Enemy Info")]
    public string enemyName;
    public EnemyTag enemyTag = EnemyTag.Ice;
    private Sequence idleSequence = null;
    public bool isEnter = false;

    public Vector3 enemyScaleVec = Vector3.one;


    private PatternManager _patternManager;
    public PatternManager PatternManager => _patternManager;

    #region UI

    [Header("UI")]
    public SpriteRenderer spriteRenderer;
    [SerializeField] private Transform _healthBar;
    [SerializeField] private Transform _shieldBar;
    [SerializeField] private Transform _healthFeedbackBar;
    [SerializeField] private Transform _shieldIcon;
    [SerializeField] private TextMeshPro _enemyHealthText;
    [SerializeField] private TextMeshPro _shieldText;

    #endregion

    public virtual void Init()
    {
        if (_patternManager == null)
            _patternManager = GetComponentInChildren<PatternManager>();

        _patternManager.Init();
        HealthUIInit();

        enemyScaleVec = spriteRenderer.transform.localScale;
        transform.localPosition = new Vector3(2, 4.5f, 0);
    }

    public override void Attack(float damage, bool isTrueDamage = false)
    {
        base.Attack(damage);
        BattleManager.Instance.Player.TakeDamage(currentDmg, isTrueDamage);
    }

    public void Idle()
    {
        idleSequence = DOTween.Sequence();
        idleSequence.Append(spriteRenderer.transform.DOScaleY(enemyScaleVec.y + 0.1f, 0.5f));
        idleSequence.Append(spriteRenderer.transform.DOScaleY(enemyScaleVec.y, 0.5f));
        idleSequence.AppendInterval(0.3f);
        idleSequence.SetLoops(-1);
    }

    public void StopIdle()
    {
        idleSequence.Kill();
        spriteRenderer.DORewind();
    }

    public override void Die()
    {
        _isDie = true;

        Sequence seq = DOTween.Sequence();
        seq.Append(spriteRenderer.DOFade(0, 0.75f));
        seq.Join(transform.DOMoveY(transform.position.y - 1f, 0.75f));
        seq.AppendInterval(0.5f);
        seq.AppendCallback(() =>
        {
            OnDieEvent?.Invoke();
            Managers.Resource.Destroy(gameObject);
            StopAllCoroutines();
        });

    }

    private void HealthUIInit()
    {
        _healthBar.DOScaleX(HP / MaxHP, 0);
        _enemyHealthText.text = string.Format("{0}/{1}", HP.ToString(), MaxHP.ToString());

        _shieldBar.DOScaleX(0, 0);
        _healthFeedbackBar.DOScaleX(0, 0);
    }

    public override void UpdateHealthUI()
    {
        _healthFeedbackBar.DOScaleX(_healthBar.localScale.x, 0);
        _healthBar.DOScaleX((float)HP / MaxHP, 0);

        _enemyHealthText.text = string.Format("{0}/{1}", HP.ToString(), MaxHP.ToString());

        if (Shield > 0)
        {
            if (HP + Shield > MaxHP)
            {
                _shieldBar.DOScaleX(1, 0);
                _healthBar.DOScaleX((float)HP / (MaxHP + Shield), 0);
            }
            else
            {
                _shieldBar.DOScaleX((float)(HP + Shield) / MaxHP, 0);
            }
        }
        else
        {
            _shieldBar.DOScaleX(0, 0);
        }

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(0.5f);
        seq.Append(_healthFeedbackBar.DOScaleX((float)HP / MaxHP, 0.2f));

        Sequence vibrateSeq = DOTween.Sequence();
        vibrateSeq.Append(_healthFeedbackBar.parent.DOShakeScale(0.1f));
        vibrateSeq.Append(_healthFeedbackBar.parent.DOScale(1f, 0));
    }

    public override void UpdateShieldUI()
    {
        _shieldText.SetText(Shield.ToString());
        UpdateHealthUI();

        Sequence seq = DOTween.Sequence();
        seq.Append(_shieldText.transform.parent.DOScale(1.2f, 0.1f));
        seq.Append(_shieldText.transform.parent.DOScale(1f, 0.1f));
    }

    private void OnDestroy()
    {
        //DOTween.KillAll();
        transform.DOKill();
    }
}
