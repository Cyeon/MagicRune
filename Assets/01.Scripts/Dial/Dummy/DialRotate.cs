using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialRotate : MonoBehaviour, IDragHandler
{
    private RectTransform targetObjTrans;

    public float speed = 0f;

    private void Awake()
    {
        targetObjTrans = GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {

        if (eventData.delta.x > 0)
        {
             
        }
        else
        {

        }

        float leftRight = eventData.delta.x > 0 ? 1 : -1;
        leftRight *= speed;

        Vector3 rot= targetObjTrans.transform.eulerAngles;
        rot.z += -1 * leftRight;

        targetObjTrans.transform.rotation = Quaternion.Euler(rot);


    }
}
