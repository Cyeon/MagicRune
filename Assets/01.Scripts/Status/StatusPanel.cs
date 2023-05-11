using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

public class StatusPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Status _status;
    private Image _image;
    private TextMeshProUGUI _duration;

    public StatusName StatusName => _status.statusName;

    private Image _effectImage;
    private Sequence _effectSeq;

    private void OnEnable()
    {
        _image = GetComponent<Image>();
        _duration = GetComponentInChildren<TextMeshProUGUI>();

        _effectImage = transform.Find("Effect").GetComponent<Image>();
        _effectImage.DOFade(0, 0);

        _effectSeq = DOTween.Sequence();
        _effectSeq.Append(_effectImage.transform.DOScale(Vector2.one * 2, 0));
        _effectSeq.Join(_effectImage.DOFade(0.5f, 0));
        _effectSeq.AppendInterval(0.1f);
        _effectSeq.Append(_effectImage.DOFade(0.1f, 0.5f));
        _effectSeq.Join(_effectImage.transform.DOScale(Vector2.one, 0.5f));
        _effectSeq.Append(_effectImage.DOFade(0, 0));
    }

    public void Init(Status status)
    {
        _status = status;

        _image.sprite = _status.icon;
        _image.color = _status.color;
        _effectImage.sprite = _status.icon;
        _effectImage.color = _status.color;
        _duration.SetText(_status.TypeValue.ToString());

        Effect();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 pos = Define.MainCam.ScreenToWorldPoint(eventData.position);
        Define.DialScene?.StatusDescPopup(_status, pos);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Define.DialScene?.StatusDescPopup(null, Vector3.zero, false);
    }

    public void UpdateDurationText()
    {
        if(_duration.text != _status.TypeValue.ToString())
        {
            Effect();
        }
        _duration.text = _status.TypeValue.ToString();
    }

    private void Effect()
    {
        _effectSeq.Restart();
    }
}
