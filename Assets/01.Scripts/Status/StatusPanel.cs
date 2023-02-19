using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class StatusPanel : MonoBehaviour, IPointerClickHandler
{
    public Status status;
    public Image image;
    public TextMeshProUGUI duration;
    public StatusName statusName;

    public void OnPointerClick(PointerEventData eventData)
    {
        Touch touch = Input.GetTouch(0);
        Vector3 pos = Camera.main.ScreenToWorldPoint(touch.position);
        UIManager.Instance.StatusDescPopup(status, pos);
    }

    private void OnEnable()
    {
        image = GetComponent<Image>();
        duration = GetComponentInChildren<TextMeshProUGUI>();
    }
}
