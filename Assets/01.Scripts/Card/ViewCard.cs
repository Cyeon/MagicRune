using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ViewCard : MonoBehaviour, IPointerDownHandler
{
    public bool isActive = false;
    private Vector3 newScale = new Vector3(3f, 3f, 3f);
    private Vector3 newPosition = new Vector3(720f, 1483f, 0f);

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isActive)
        {
            EventManager.TriggerEvent(Define.CLICK_VIEW_UI);
            GameObject gameObject = Instantiate(this.gameObject, transform.parent.parent.parent.parent);
            gameObject.name = "Card_Temp";
            gameObject.transform.localScale = newScale;
            //gameObject.GetComponent<Card>().OutlineEffect.gameObject.SetActive(false);
            gameObject.GetComponent<ViewCard>().enabled = false;
            gameObject.GetComponent<RectTransform>().anchoredPosition = newPosition;
            gameObject.transform.Find("Keyword").GetComponent<RectTransform>().anchoredPosition = new Vector2(5f, -310f);
        }
    }
    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}