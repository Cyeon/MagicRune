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

    public void UpdateUI(RuneProperty rune)
    {
        if (rune == null) return;

        _runeNameText.text = rune.Name;
        _runeDescText.text = rune.CardDescription;
        _manaText.text = rune.Cost.ToString();
        _coolTImeText.text = rune.DelayTurn.ToString();
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
