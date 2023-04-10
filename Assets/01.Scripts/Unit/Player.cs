using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : Unit
{
    public int cost = 10; // 마나
    public AudioClip attackSound = null;

    private void Awake()
    {
        _isPlayer = true;

        var obj = FindObjectsOfType<Managers>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SliderInit()
    {
        UIManager.Instance.Bind<Slider>("P HealthFeedbackBar", CanvasManager.Instance.GetCanvas("Main").gameObject);
        UIManager.Instance.Bind<Slider>("P ShieldBar", CanvasManager.Instance.GetCanvas("Main").gameObject);
        UIManager.Instance.Bind<Slider>("P HealthBar", CanvasManager.Instance.GetCanvas("Main").gameObject);
        UIManager.Instance.Bind<TextMeshProUGUI>("P HealthText", CanvasManager.Instance.GetCanvas("Main").gameObject);

        _healthFeedbackSlider = UIManager.Instance.Get<Slider>("P HealthFeedbackBar");
        _shieldSlider = UIManager.Instance.Get<Slider>("P ShieldBar");
        _healthSlider = UIManager.Instance.Get<Slider>("P HealthBar");
        _healthText = UIManager.Instance.Get<TextMeshProUGUI>("P HealthText");
    }

    public void Attack(float dmg)
    {
        currentDmg = dmg;

        StatusManager.OnAttack();

        BattleManager.Instance.enemy.TakeDamage(currentDmg);
        SoundManager.Instance.PlaySound(attackSound, SoundType.Effect);

    }
}
