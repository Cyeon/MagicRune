using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class StatusPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Status status;
    public Image image;
    public TextMeshProUGUI duration;
    public StatusName statusName;

    private DialScene _dialScene;
    private bool _isPopuped = false;

    private void Start()
    {
        _dialScene = Managers.Scene.CurrentScene as DialScene;
    }

    private void OnEnable()
    {
        image = GetComponent<Image>();
        duration = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 pos = Define.MainCam.ScreenToWorldPoint(eventData.position);
        _dialScene?.StatusDescPopup(status, pos);
        _isPopuped = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _dialScene?.StatusDescPopup(null, Vector3.zero, false);
    }
}
