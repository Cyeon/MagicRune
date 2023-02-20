using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraResolution : MonoBehaviour
{
    private new Camera camera;

    private float scaleHeight;
    private float scaleWidth;

    private void Awake()
    {
        camera = GetComponent<Camera>();

        Rect rect = camera.rect;

        scaleHeight = ((float)Screen.width / Screen.height) / ((float)9f / 18.5f); // (가로 / 세로)
        scaleWidth = 1f / scaleHeight;
        if (scaleHeight < 1)
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

        //int setWidth = 1440;
        //int setHeight = 2960;

        //int deviceWidth = Screen.width;
        //int deviceHeight = Screen.height;

        //Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true);

        //if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight)
        //{
        //    float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight);
        //    camera.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);
        //}
        //else
        //{
        //    float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight);
        //    camera.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight);
        //}
    }

    void OnPreCull() => GL.Clear(true, true, Color.black);
    void OnPreRender() => GL.Clear(true, true, Color.black);

    void OnPostRender() => GL.Clear(true, true, Color.black);
}
