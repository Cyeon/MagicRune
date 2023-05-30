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

    public override void Attack(float dmg, bool isTrueDamage)
    {
        base.Attack(dmg);
        BattleManager.Instance.Enemy.TakeDamage(currentDmg, isTrueDamage);
    }

    public void VisualInit(VisualPlayer vp)
    {
        _visual = vp;
    }

    public void UISetting(Transform health, Transform shield, Transform healthFeedback, Transform shieldIcon, TextMeshPro healthText, TextMeshPro shieldText)
    {
        _healthBar = health;
        _shieldBar = shield;
        _healthFeedbackBar = healthFeedback;
        _shieldIcon = shieldIcon;
        _healthText = healthText;
        _shieldText = shieldText;
    }
}