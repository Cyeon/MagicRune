using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private Transform _canvas;

    [Header("Enemy UI")]
    [SerializeField] private Slider _enemyHealthSlider;
    [SerializeField] private TextMeshProUGUI _enemyHealthText;
    [SerializeField] private Image _enemyPatternIcon;
    [SerializeField] private TextMeshProUGUI _enemyPatternValueText;
    [SerializeField] private TextMeshProUGUI _enemyShieldText;
    public Transform enemyIcon;

    [Header("Player UI")]
    [SerializeField] private Slider _playerHealthSlider;
    [SerializeField] private TextMeshProUGUI _playerHealthText;
    [SerializeField] private TextMeshProUGUI _playerShieldText;

    [Header("상태이상 UI")]
    [SerializeField] private GameObject _statusPrefab;
    [SerializeField] private GameObject _statusPopup;
    [SerializeField] private Transform _statusPlayerPanel;
    [SerializeField] private Transform _statusEnemyPanel;

    [Header("Panel")]
    [SerializeField] private GameObject _turnPanel;
    private Image _turnBackground;
    private TextMeshProUGUI _turnText;

    [Header("ETC")]
    [SerializeField] private GameObject _damagePopup;
    [SerializeField] private GameObject _infoMessage;

    private void Awake()
    {
        _turnBackground = _turnPanel?.GetComponent<Image>();
        _turnText = _turnPanel?.GetComponentInChildren<TextMeshProUGUI>();
    }

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

    public void ReloadPattern(Sprite sprite, string value = "")
    {
        _enemyPatternIcon.sprite = sprite;
        _enemyPatternValueText.text = value;
    }


    #region Status

    public StatusPanel GetStatusPanel(Status status, Transform parent, bool isPopup = false)
    {
        StatusPanel statusPanel = Instantiate(isPopup ? _statusPopup : _statusPrefab).GetComponent<StatusPanel>();
        
        statusPanel.image.sprite = status.icon;
        statusPanel.image.color = status.color;
        statusPanel.duration.text = status.typeValue.ToString();
        statusPanel.statusName = status.statusName;
        statusPanel.transform.SetParent(parent);
        statusPanel.transform.localScale = Vector3.one;

        return statusPanel;
    }

    public void AddStatus(Unit unit, Status status)
    {
        Transform trm = unit == GameManager.Instance.player ? _statusPlayerPanel : _statusEnemyPanel;
        GetStatusPanel(status, trm);
    }

    public GameObject GetStatusPanelStatusObj(Unit unit, StatusName name)
    {
        Transform trm = unit == GameManager.Instance.player ? _statusPlayerPanel : _statusEnemyPanel;

        GameObject obj = null;
        for (int i = 0; i < trm.childCount; i++)
        {
            if (trm.GetChild(i).GetComponent<StatusPanel>().statusName == name)
            {
                obj = trm.GetChild(i).gameObject;
            }
        }

        return obj;
    }

    public void ReloadStatusPanel(Unit unit, StatusName name, int duration)
    {
        GameObject obj = GetStatusPanelStatusObj(unit, name);

        if (obj == null)
            return;

        if(duration <= 0)
        {
            RemoveStatusPanel(unit, name);
            return;
        }

        obj.GetComponent<StatusPanel>().duration.text = duration.ToString();
    }

    public void RemoveStatusPanel(Unit unit, StatusName name)
    {
        GameObject obj = GetStatusPanelStatusObj(unit, name);

        if (obj == null)
        {
            return;
        }

        Destroy(obj);
    }

    #endregion

    public void UpdateShieldText(bool isPlayer, float shield)
    {
        if (isPlayer)
            _playerShieldText.text = shield.ToString();
        else
            _enemyShieldText.text = shield.ToString();
    }

    public void Turn(string text)
    {
        _turnPanel.SetActive(true);
        _turnText.text = text;

        Sequence seq = DOTween.Sequence();
        seq.Append(_turnBackground.DOFade(0.5f, 0.5f));
        seq.Join(_turnText.DOFade(1f, 0.5f));
        seq.AppendInterval(0.5f);
        seq.Append(_turnBackground.DOFade(0, 0.5f));
        seq.Join(_turnText.DOFade(0, 0.5f));
        seq.AppendCallback(() => _turnPanel.SetActive(false));
        seq.AppendCallback(() =>GameManager.Instance.TurnChange());
    }

    public void OnDestroy()
    {
        transform.DOKill();
    }

    #region Popup
    public void DamageUIPopup(float amount, Vector3 pos)
    {
        DamagePopup popup = Instantiate(_damagePopup, _canvas).GetComponent<DamagePopup>();
        popup.Setup(amount, pos);
    }

    public void StatusPopup(Status status, Vector3 pos)
    {
        StatusPanel panel = GetStatusPanel(status, _canvas, true);
        panel.transform.position = pos;
        panel.transform.localScale = Vector3.one * 2f;

        CanvasGroup group = panel.GetComponent<CanvasGroup>();

        Sequence seq = DOTween.Sequence();
        seq.Append(panel.transform.DOJump(new Vector3(pos.x + Random.Range(-2f, 2f), pos.y, pos.x), 0.8f, 1, 1f));
        seq.Join(group.DOFade(0, 1f).SetEase(Ease.InQuart));
        seq.AppendCallback(() =>
        {
            Destroy(group.gameObject);
        });
    }

    public void InfoMessagePopup(string message, Vector3 pos)
    {
        InfoMessage popup = Instantiate(_infoMessage, _canvas).GetComponent<InfoMessage>();
        pos.z = 0;
        pos.y += 1;
        popup.Setup(message, pos);
    }
    #endregion
}
