using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RuneDesc : MonoBehaviour
{
    private Text _runeNameText;
    private Text _runeDescText;
    private Text _manaText;
    private Text _coolTImeText;

    private void Awake()
    {
        _runeNameText = transform.Find("RuneName").GetComponent<Text>();
        _runeDescText = transform.Find("RuneDesc").GetComponent<Text>();
        _manaText = transform.Find("Mana/Value").GetComponent<Text>();
        _coolTImeText = transform.Find("CoolTime/Value").GetComponent<Text>();
    }

    public void UpdateUI(RuneSO rune)
    {
        if (rune == null) return;

        _runeNameText.text = rune.Name;
        _runeDescText.text = rune.MainRune.CardDescription;
        _coolTImeText.text = rune.CoolTime.ToString();
    }

    private void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
