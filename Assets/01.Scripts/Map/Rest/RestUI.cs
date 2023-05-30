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
    public RestDial Dial => _dial;

    [SerializeField]
    private ExplainPanel _explainPanel;
    [SerializeField]
    private EnhancePanel _enhancePanel;

    private void Start()
    {
        GetComponent<Canvas>().enabled = false;
    }

    public void SetActiveExplainPanel(bool active)
    {
        _explainPanel.gameObject.SetActive(active);
    }

    public void SetActiveEnhancePanel(bool active)
    {
        _enhancePanel.gameObject.SetActive(active);
        if(active == true)
            _enhancePanel.CreateRune();
    }

    public void NextStage()
    {
        Managers.Canvas.GetCanvas(this.name).enabled = false;
        Managers.Canvas.GetCanvas("MapUI").enabled = true;
        _dial.gameObject.SetActive(false);
        Managers.Map.NextStage();
    }
}
