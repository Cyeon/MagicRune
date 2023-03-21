using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum EnhanceType
{
    Sacrifice, // Àç¹°
    COUNT,
}

[System.Serializable]
public class EnhanceActionPair
{
    public EnhanceType Type;
    public AdventureSO[] Action;
}

public class RestPortal : Portal
{
    #region Rest Parameta
    [SerializeField, Range(0, 100), OverrideLabel("Health %")]
    private int _healthPercent = 25;
    #endregion

    [SerializeField]
    private Canvas _restCanvas;

    [SerializeField]
    private ParticleSystem _healthParticle;

    public override void Init()
    {

    }

    public override void Execute()
    {
        StartCoroutine(PlayerHealthCoroutine());
    }

    private IEnumerator PlayerHealthCoroutine()
    {
        _restCanvas.enabled = true;
        _healthParticle.gameObject.SetActive(true);
        Debug.Log(GameManager.Instance.player.GetHP());
        GameManager.Instance.player.AddHPPercent(_healthPercent);
        Debug.Log(GameManager.Instance.player.GetHP());
        yield return new WaitForSeconds(0.5f);

        _restCanvas.enabled = false;
        MapManager.Instance.NextStage();
    }
}
