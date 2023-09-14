using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : Unit
{
    [HideInInspector]
    public Transform relicTrm;

    private VisualPlayer _visual;
    public VisualPlayer Visual => _visual;

    private void Awake()
    {
        if (FindObjectsOfType<Player>().Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);

        relicTrm = transform.Find("Relic");
    }

    public void Attack(float dmg, bool isTrueDamage = false)
    {
        attackDamage = dmg.RoundToInt();
        StatusManager.DamageApply();
        base.Attack(ref isTrueDamage);
        BattleManager.Instance.Enemy.TakeDamage(attackDamage, isTrueDamage);
    }

    public void VisualInit(VisualPlayer vp)
    {
        _visual = vp;
        OnTakeDamageFeedback = vp.OnTakeDamageEvent;
    }

    public void UISetting(SpriteRenderer health, SpriteRenderer shield, SpriteRenderer healthFeedback, Transform shieldIcon, TextMeshPro healthText, TextMeshPro shieldText)
    {
        _healthBar = health;
        _shieldBar = shield;
        _healthFeedbackBar = healthFeedback;
        _shieldIcon = shieldIcon;
        _healthText = healthText;
        _shieldText = shieldText;
    }
}