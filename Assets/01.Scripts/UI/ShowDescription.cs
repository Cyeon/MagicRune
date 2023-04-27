using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowDescription : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public string title;
    [TextArea(1, 3)]
    public string description;

    [SerializeField]  private GameObject _descriptionObj;

    public void OnPointerDown(PointerEventData eventData)
    {
        _descriptionObj.SetActive(true);
        _descriptionObj.GetComponent<DescriptionPanel>().SetUp(title, description);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _descriptionObj.SetActive(false);
    }
}
