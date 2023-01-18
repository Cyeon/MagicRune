using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private Slider _enemyHealthSlider;
    [SerializeField] private TextMeshProUGUI _enemyHealthText;
    [SerializeField] private Slider _playerHealthSlider;
    [SerializeField] private TextMeshProUGUI _playerHealthText;

    public void EnemyHealthbarInit(float health)
    {
        _enemyHealthSlider.maxValue = health;
        _enemyHealthSlider.value = health;
        _enemyHealthText.text = string.Format("{0} / {1}", health, health);
    }

    public void UpdateEnemyHealthbar()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_enemyHealthSlider.DOValue(GameManager.instance.enemy.HP, 0.2f));
        seq.AppendCallback(() => _enemyHealthText.text = string.Format("{0} / {1}", _enemyHealthSlider.value, _enemyHealthSlider.maxValue));
    }


    public void PlayerHealthbarInit(float health)
    {
        _playerHealthSlider.maxValue = health;
        _playerHealthSlider.value = health;
        _playerHealthText.text = string.Format("{0} / {1}", health, health);
    }

    public void UpdatePlayerHealthbar()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_playerHealthSlider.DOValue(GameManager.instance.player.HP, 0.2f));
        seq.AppendCallback(() => _playerHealthText.text = string.Format("{0} / {1}", _playerHealthSlider.value, _playerHealthSlider.maxValue));
    }
}
