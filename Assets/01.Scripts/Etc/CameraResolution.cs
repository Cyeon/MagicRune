using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CameraResolution : MonoBehaviour
{
    private new Camera camera;

    private float scaleHeight;
    private float scaleWidth;

    private void Start()
    {
        camera = GetComponent<Camera>();
        OnSetting();
    }

    private void OnSetting()
    {
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
    }

    void OnEnable()
    {
#if !UNITY_EDITOR
        RenderPipelineManager.beginCameraRendering += RenderPipelineManager_endCameraRendering;
#endif
    }

    void OnDisable()
    {
#if !UNITY_EDITOR
        RenderPipelineManager.beginCameraRendering -= RenderPipelineManager_endCameraRendering;
#endif
    }

    private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        GL.Clear(true, true, Color.black);
    }

    public void OnPreCull() => GL.Clear(true, true, Color.black);
}
