using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapPanel : MonoBehaviour
{
    private TextMeshProUGUI _titleText;
    private Image _image;
    private EnemySO _enemy;
    private MapType _mapType;

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
        _mapType = MapType.Attack;
    }

    public void Init(MapType type)
    {
        _mapType = type;
        _titleText.text = type.ToString();
    }

    public void Choose()
    {
        if(_mapType == MapType.Attack)
        {
            MapManager.Instance.selectEnemy = _enemy;
            SceneManager.LoadScene("MainBattleScene");
        }
    }
}
