using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    private Slider _slider;
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _text = transform.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Reload()
    {
        _slider.maxValue = GameManager.Instance.player.MaxHealth;
        _slider.value = GameManager.Instance.player.HP;
        _text.text = string.Format("{0} / {1}", _slider.value, _slider.maxValue);
    }
}
