using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestUI : MonoBehaviour
{
    [SerializeField, Range(0, 100)]
    private int _healthPercent = 25;
    public int HealthPercent => _healthPercent;

    [SerializeField]
    private RestDial _dial;

    private void Start()
    {
        GetComponent<Canvas>().enabled = false;

    }

    public void NextStage()
    {
        Managers.Canvas.GetCanvas(this.name).enabled = false;
        Managers.Canvas.GetCanvas("MapUI").enabled = true;
        // Dial 안에 내용 플링
        _dial.gameObject.SetActive(false);
        Managers.Map.NextStage();
    }
}
