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

    private void Awake()
    {
        if (FindObjectsOfType<Player>().Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);

        relicTrm = transform.Find("Relic");
    }

    public void SliderInit()
    {
        if (Managers.UI.Get<Slider>("P HealthFeedbackBar") == null)
        {
            Managers.UI.Bind<Slider>("P HealthFeedbackBar", Managers.Canvas.GetCanvas("Main").gameObject);
            Managers.UI.Bind<Slider>("P ShieldBar", Managers.Canvas.GetCanvas("Main").gameObject);
            Managers.UI.Bind<Slider>("P HealthBar", Managers.Canvas.GetCanvas("Main").gameObject);
            Managers.UI.Bind<TextMeshProUGUI>("P HealthText", Managers.Canvas.GetCanvas("Main").gameObject);
        }

        _healthFeedbackSlider = Managers.UI.Get<Slider>("P HealthFeedbackBar");
        _shieldSlider = Managers.UI.Get<Slider>("P ShieldBar");
        _healthSlider = Managers.UI.Get<Slider>("P HealthBar");
        _healthText = Managers.UI.Get<TextMeshProUGUI>("P HealthText");
    }

    public override void Attack(float dmg, bool isTrueDamage)
    {
        base.Attack(dmg);
        BattleManager.Instance.Enemy.TakeDamage(currentDmg, isTrueDamage);
    }
}