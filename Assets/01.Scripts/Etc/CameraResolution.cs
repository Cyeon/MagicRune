using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    private new Camera camera;

    private float scaleHeight;
    private float scaleWidth;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        Rect rect = camera.rect;

        //scaleHeight = ((float)Screen.width / Screen.height) / ((float)9f / 18.5f); // (가로 / 세로)
        scaleHeight = ((float)1440f / 2960f) / ((float)9f / 18.5f); // (가로 / 세로)
        scaleWidth = 1f / scaleHeight;
        if(scaleHeight < 1)
        {
            rect.height = scaleHeight;
            rect.y = (1f - scaleHeight) / 2f;
        }
        else
        {
            rect.width = scaleWidth;
            rect.x = (1f - scaleWidth) / 2f;
        }
        camera.rect = rect;
    }

    void OnPreCull() => GL.Clear(true, true, Color.black);
    void OnPreRender() => GL.Clear(true, true, Color.black);

    void OnPostRender() => GL.Clear(true, true, Color.black);
}
