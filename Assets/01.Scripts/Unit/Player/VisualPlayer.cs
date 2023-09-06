using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class VisualPlayer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] protected SpriteRenderer _healthBar;
    [SerializeField] protected SpriteRenderer _shieldBar;
    [SerializeField] protected SpriteRenderer _healthFeedbackBar;
    [SerializeField] protected Transform _shieldIcon;
    [SerializeField] protected TextMeshPro _healthText;
    [SerializeField] protected TextMeshPro _shieldText;

    [Header("Feedback")]
    public UnityEvent OnTakeDamageEvent;

    public void UISetting()
    {
        Managers.GetPlayer().UISetting(_healthBar, _shieldBar, _healthFeedbackBar, _shieldIcon, _healthText, _shieldText);
        Managers.GetPlayer().Animator = transform.Find("UI/PlayerSprite").GetComponent<Animator>();
    }
}
