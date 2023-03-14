using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PortalPanel : MonoBehaviour
{
    private TextMeshProUGUI _titleText;
    private Image _image;
    private Portal _portal;

    private void Awake()
    {
        _titleText = transform.Find("TItle_Text").GetComponent<TextMeshProUGUI>();
        _image = transform.Find("Map_Image").GetComponent<Image>();
    }

    public void Init(Portal portal)
    {
        _portal = portal;
        _titleText.text = portal.portalName;
        _image.sprite = portal.icon;
    }

    public void Select()
    {
        MapManager.Instance.selectPortal = _portal;
        _portal.Execute();
    }
}
