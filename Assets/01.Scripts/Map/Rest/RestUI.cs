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
    [SerializeField]
    private GameObject _greenPortal;
    [SerializeField]
    private GameObject _orangePortal;

    private void Start()
    {
        _restBtn.onClick.RemoveAllListeners();
        _restBtn.onClick.AddListener(() => StartCoroutine(RestCoroutine()));

        _enhanceBtn.onClick.RemoveAllListeners();
        _enhanceBtn.onClick.AddListener(() =>
        {
            Debug.Log("Attack Complete");
            MapManager.Instance.NextStage();
        });
    }

    public void PortalStartAnimation()
    {
        _orangePortal.SetActive(true);
        _greenPortal.SetActive(true);

        _orangePortal.transform.localScale = Vector3.zero;
        _greenPortal.transform.localScale = Vector3.zero;

        _orangePortal.transform.DOScale(1.5f, 0.1f);
        _greenPortal.transform.DOScale(1.5f, 0.1f);
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
        _orangePortal.SetActive(false);
        _greenPortal.SetActive(false);
        CanvasManager.Instance.GetCanvas(this.name).enabled = false;
        CanvasManager.Instance.GetCanvas("MapUI").enabled = true;
        MapManager.Instance.NextStage();
    }
}
