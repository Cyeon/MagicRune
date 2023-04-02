using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestUI : MonoBehaviour
{
    [SerializeField, Range(0, 100)]
    private int _healthPercent = 25;

    [SerializeField]
    private Button _restBtn;
    [SerializeField]
    private Button _enhanceBtn;

    [SerializeField]
    private GameObject _healthParticle;

    private EnhanceType _enhanceType;

    private void Start()
    {
        GetComponent<Canvas>().enabled = false;

        _restBtn.onClick.RemoveAllListeners();
        _restBtn.onClick.AddListener(() =>
        {
            _enhanceBtn.onClick.RemoveAllListeners();
            StartCoroutine(RestCoroutine());
        });

        //_enhanceBtn.onClick.RemoveAllListeners();
        _enhanceBtn.onClick.AddListener(() =>
        {
            _restBtn.onClick.RemoveAllListeners();
            Debug.Log("Enhance Complete");


            NextStage();
        });
    }

    public void SetEnhanceType(EnhanceType type)
    {
        _enhanceType = type;
    }

    public void PortalStartAnimation()
    {
        _restBtn.gameObject.SetActive(true);
        _enhanceBtn.gameObject.SetActive(true);

        _restBtn.transform.localScale = Vector3.zero;
        _enhanceBtn.transform.localScale = Vector3.zero;

        _restBtn.transform.DOScale(1.5f, 0.1f);
        _enhanceBtn.transform.DOScale(1.5f, 0.1f);
    }

    private IEnumerator RestCoroutine()
    {
        GameManager.Instance.player.AddHPPercent(_healthPercent);
        _healthParticle.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        NextStage();
    }

    private void NextStage()
    {
        _restBtn.gameObject.SetActive(false);
        _enhanceBtn.gameObject.SetActive(false);
        CanvasManager.Instance.GetCanvas(this.name).enabled = false;
        CanvasManager.Instance.GetCanvas("MapUI").enabled = true;
        MapManager.Instance.NextStage();
    }
}
