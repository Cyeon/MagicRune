using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PortalPanel : MonoBehaviour
{
    private TextMeshProUGUI _titleText;
    private Image _image;
    private EnemySO _enemy;
    private PortalType _mapType;

    private void Awake()
    {
        _titleText = transform.Find("TItle_Text").GetComponent<TextMeshProUGUI>();
        _image = transform.Find("Map_Image").GetComponent<Image>();
    }

    public void Init(EnemySO enemy)
    {
        _enemy = enemy;
        _titleText.text = enemy.enemyName;
        _image.sprite = enemy.icon;
        _mapType = PortalType.Attack;
    }

    public void Init(PortalType type)
    {
        _mapType = type;
        _titleText.text = type.ToString();
    }

    public void Choose()
    {
        if (_mapType == PortalType.Attack)
        {
            MapManager.Instance.selectEnemy = _enemy;
            SceneManager.LoadScene("MainBattleScene");
        }
    }
}
