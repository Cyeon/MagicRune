using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Runtime.CompilerServices;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private Slider _enemyHealthSlider;
    [SerializeField] private TextMeshProUGUI _enemyHealthText;
    [SerializeField] private Slider _playerHealthSlider;
    [SerializeField] private TextMeshProUGUI _playerHealthText;
    [SerializeField] private Image _enemyPatternIcon;

    [Header("상태이상 UI")]
    [SerializeField] private GameObject _statusPrefab;
    private Image _prefabImage;
    private TextMeshProUGUI _prefabText;
    [SerializeField] private Transform _statusPlayerPanel;
    [SerializeField] private Transform _statusEnemyPanel;

    public void EnemyHealthbarInit(float health)
    {
        _enemyHealthSlider.maxValue = health;
        _enemyHealthSlider.value = health;
        _enemyHealthText.text = string.Format("{0} / {1}", health, health);
    }

    public void UpdateEnemyHealthbar()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_enemyHealthSlider.DOValue(GameManager.Instance.enemy.HP, 0.2f));
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
        seq.Append(_playerHealthSlider.DOValue(GameManager.Instance.player.HP, 0.2f));
        seq.AppendCallback(() => _playerHealthText.text = string.Format("{0} / {1}", _playerHealthSlider.value, _playerHealthSlider.maxValue));
    }

    public void ReloadPatternIcon(Sprite sprite)
    {
        _enemyPatternIcon.sprite = sprite;
    }

    public void AddStatus(Unit unit, Status status)
    {
        Transform trm = unit == GameManager.Instance.player ? _statusPlayerPanel : _statusEnemyPanel;

        StatusPanel statusPanel = Instantiate(_statusPrefab).GetComponent<StatusPanel>();
        statusPanel.image.sprite = status.icon;
        statusPanel.duration.text = status.durationTurn.ToString();
        statusPanel.statusName = status.statusName;
        statusPanel.transform.SetParent(trm);
    }

    public void ReloadStatusPanel(Unit unit, string name, int duration)
    {
        Transform trm = unit == GameManager.Instance.player ? _statusPlayerPanel : _statusEnemyPanel;

        GameObject obj = null;
        for(int i = 0; i < trm.childCount; i++)
        {
            if(trm.GetChild(i).GetComponent<StatusPanel>().statusName == name)
            {
                obj = trm.GetChild(i).gameObject;
            }
        }

        if (obj == null)
            return;

        if(duration <= 0)
        {
            Destroy(obj);
            return;
        }

        obj.GetComponent<StatusPanel>().duration.text = duration.ToString();
    }
}
