using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class EnemyInfo
{
    public string Name;
    public Sprite Sprite;
    public Color BackgroundColor;
}

public class BossAppearanceTimeline : MonoBehaviour
{
    private PlayableDirector _director;
    public PlayableDirector Director => _director;

    private SpriteRenderer _colorBar = null;
    private SpriteRenderer _enemy = null;
    private TextMeshPro _name;

    [SerializeField]
    private EnemyInfo[] _enemyInfoArray = new EnemyInfo[3];

    private void OnEnable()
    {
        _director = GetComponent<PlayableDirector>();
        _colorBar = transform.Find("ColorBar").GetComponent<SpriteRenderer>();
        _enemy = transform.Find("Enemy").GetComponent<SpriteRenderer>();
        _name = transform.Find("Name").GetComponent<TextMeshPro>();
    }

    public void SetEnemyInfo(int index)
    {
        string name = _enemyInfoArray[index].Name.Replace("\\n", "\n");
        _name.SetText(name);
        _enemy.sprite = _enemyInfoArray[index].Sprite;
        _colorBar.color = _enemyInfoArray[index].BackgroundColor;
    }

    public void BossAppearanceStart()
    {
        Managers.Canvas.GetCanvas("Popup").gameObject.SetActive(false);
        Managers.Canvas.GetCanvas("Main").gameObject.SetActive(false);
        Managers.Canvas.GetCanvas("BG").gameObject.SetActive(false);
        Managers.Canvas.GetCanvas("UserInfoPanel").gameObject.SetActive(false);
        (Managers.Scene.CurrentScene as DialScene).Dial.gameObject.SetActive(false);
    }

    public void BossAppearanceEnd()
    {
        Managers.Canvas.GetCanvas("Popup").gameObject.SetActive(true);
        Managers.Canvas.GetCanvas("Main").gameObject.SetActive(true);
        Managers.Canvas.GetCanvas("BG").gameObject.SetActive(true);
        Managers.Canvas.GetCanvas("UserInfoPanel").gameObject.SetActive(true);
        (Managers.Scene.CurrentScene as DialScene).Dial.gameObject.SetActive(true);

        Managers.Resource.Destroy(this.gameObject);
    }
}
