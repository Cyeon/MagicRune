using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpdateRotate : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool rotating;
    public float rotateDamp = 5.0f;
    Vector3 mousePos, offset;

    private void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.04f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rotating = true;

        mousePos = Input.mousePosition; // Input.GetTouch
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        rotating = false;
    }

    void Update()
    {
        if (rotating)
        {
            offset = (Input.mousePosition - mousePos);

            Vector3 rot = transform.eulerAngles;

            float temp = Input.mousePosition.x > Screen.width / 2 ? offset.x - offset.y : offset.x + offset.y;

            if (offset.x > 0)
                temp = Mathf.Clamp(temp, 0, offset.x);
            else
                temp = Mathf.Clamp(temp, offset.x, 0);

            rot.z += -1 * temp / rotateDamp;

            transform.rotation = Quaternion.Euler(rot);
            mousePos = Input.mousePosition;
        }
    }
}
