using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PortalPanel : MonoBehaviour
{
    private TextMeshProUGUI _titleText;
    private Image _image;
    private PortalType _mapType;

    private EnemySO _enemy;

    private void Awake()
    {
        _titleText = transform.Find("TItle_Text").GetComponent<TextMeshProUGUI>();
        _image = transform.Find("Map_Image").GetComponent<Image>();
        transform.GetComponent<Button>().onClick.AddListener(null);
    }

    public void Init(EnemySO enemy)
    {
        _enemy = enemy;
        _titleText.text = enemy.enemyName;
        _image.sprite = enemy.icon;
        _mapType = PortalType.Attack;
    }
}
