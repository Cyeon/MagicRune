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

        DontDestroyOnLoad(gameObject);
    }

    public void SliderInit()
    {
        Managers.UI.Bind<Slider>("P HealthFeedbackBar", Managers.Canvas.GetCanvas("Main").gameObject);
        Managers.UI.Bind<Slider>("P ShieldBar", Managers.Canvas.GetCanvas("Main").gameObject);
        Managers.UI.Bind<Slider>("P HealthBar", Managers.Canvas.GetCanvas("Main").gameObject);
        Managers.UI.Bind<TextMeshProUGUI>("P HealthText", Managers.Canvas.GetCanvas("Main").gameObject);

        _healthFeedbackSlider = Managers.UI.Get<Slider>("P HealthFeedbackBar");
        _shieldSlider = Managers.UI.Get<Slider>("P ShieldBar");
        _healthSlider = Managers.UI.Get<Slider>("P HealthBar");
        _healthText = Managers.UI.Get<TextMeshProUGUI>("P HealthText");
    }

    public void Attack(float dmg)
    {
        currentDmg = dmg;

        StatusManager.OnAttack();

        BattleManager.Instance.enemy.TakeDamage(currentDmg);
        Managers.Sound.PlaySound(attackSound, SoundType.Effect);

    }
}
