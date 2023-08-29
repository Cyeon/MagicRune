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
    public Status Status => _status;
    public StatusName StatusName => _status.statusName;

    private Passive _passive;
    public Passive Passive => _passive;

    private Image _image;
    private TextMeshProUGUI _duration;
    private Image _effectImage;
    private Sequence _effectSeq;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _duration = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Init(Status status)
    {
        _status = status;
        transform.name = status.debugName;

        if (transform.Find("Effect").TryGetComponent(out _effectImage))
        {
            _effectImage.color = new Color(1, 1, 1, 0);

            _effectSeq = DOTween.Sequence();
            _effectSeq.Append(_effectImage?.transform.DOScale(Vector2.one * 2, 0));
            _effectSeq.Join(_effectImage?.DOFade(0.5f, 0));
            _effectSeq.AppendInterval(0.1f);
            _effectSeq.Append(_effectImage?.DOFade(0.1f, 0.5f));
            _effectSeq.Join(_effectImage?.transform.DOScale(Vector2.one, 0.5f));
            _effectSeq.Append(_effectImage?.DOFade(0, 0));
        }

        _image.sprite = _status.icon;
        _image.color = _status.color;
        _effectImage.sprite = _status.icon;
        _effectImage.color = _status.color;
        _duration.SetText(_status.TypeValue.ToString());


        Effect();
    }

    public void Init(Passive passive)
    {
        _passive = passive;
        transform.name = passive.passiveName;

        _image.sprite = passive.passiveIcon;
        _image.color = Color.white;

        _duration.SetText("");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(_passive != null)
            Define.DialScene?.DescriptionPopup(_passive.passiveName, _passive.passiveDescription, eventData.position);
        else
            Define.DialScene?.DescriptionPopup(_status.debugName, _status.information, eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Define.DialScene?.CloseDescriptionPanel();
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
