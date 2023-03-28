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

    private DialScene _dialScene;

    private void Start()
    {
        _dialScene = SceneManagerEX.Instance.CurrentScene as DialScene;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 pos = Define.MainCam.ScreenToWorldPoint(eventData.position);
        _dialScene?.StatusDescPopup(status, pos);
    }

    private void OnEnable()
    {
        image = GetComponent<Image>();
        duration = GetComponentInChildren<TextMeshProUGUI>();
    }
}
