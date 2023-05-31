using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VisualPlayer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] protected Transform _healthBar;
    [SerializeField] protected Transform _shieldBar;
    [SerializeField] protected Transform _healthFeedbackBar;
    [SerializeField] protected Transform _shieldIcon;
    [SerializeField] protected TextMeshPro _healthText;
    [SerializeField] protected TextMeshPro _shieldText;

    public void UISetting()
    {
        Managers.GetPlayer().UISetting(_healthBar, _shieldBar, _healthFeedbackBar, _shieldIcon, _healthText, _shieldText);
    }
}
