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
    }

    public void Choose()
    {
        MapManager.SelectEnemy = _enemy;
        SceneManager.LoadScene("MainBattleScene");
    }
}
