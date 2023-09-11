using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowDescription : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public string title;
    [TextArea(1, 3)]
    public string description;

    private DescriptionPanel _descriptionObj;

    public void OnPointerDown(PointerEventData eventData)
    {
        _descriptionObj = Managers.Resource.Instantiate("UI/DescriptionPanel", Managers.Canvas.GetCanvas("UserInfoPanel").transform).GetComponent<DescriptionPanel>();
        _descriptionObj.transform.position = Camera.main.ScreenToWorldPoint(eventData.position);
        _descriptionObj.GetComponent<RectTransform>().DOAnchorPos3DZ(0, 0);
        _descriptionObj.transform.localScale = Vector3.one;
        _descriptionObj.SetUp(title, description);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Destroy(_descriptionObj.gameObject);
    }
}
