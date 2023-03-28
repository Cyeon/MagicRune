using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 안드로이드 토스트 메시지 표시 싱글톤 </summary>
public class AndroidToast : MonoSingleton<AndroidToast>
{
    public enum ToastLength
    {
        /// <summary> 약 2.5초 </summary>
        Short,
        /// <summary> 약 4초 </summary>
        Long
    };

#if UNITY_EDITOR
    private float __editorGuiTime = 0f;
    private string __editorGuiMessage;

#elif UNITY_ANDROID

    private AndroidJavaClass _unityPlayer;
    private AndroidJavaObject _unityActivity;
    private AndroidJavaClass _toastClass;

    private void Start()
    {
        _unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        _unityActivity = _unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        _toastClass = new AndroidJavaClass("android.widget.Toast");
    }
#endif

    /// <summary> 안드로이드 토스트 메시지 표시하기 </summary>
    [System.Diagnostics.Conditional("UNITY_ANDROID")]
    public void ShowToastMessage(string message, ToastLength length = ToastLength.Short)
    {
#if UNITY_EDITOR
        __editorGuiTime = length == ToastLength.Short ? 2.5f : 4f;
        __editorGuiMessage = message;

#elif UNITY_ANDROID
        if (_unityActivity != null)
        {
            _unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = _toastClass.CallStatic<AndroidJavaObject>("makeText", _unityActivity, message, (int)length);
                toastObject.Call("show");
            }));
        }
#endif
    }

#if UNITY_EDITOR
    /* 유니티 에디터 IMGUI를 통해 토스트 메시지 표시 모방하기 */

    private GUIStyle toastStyle;
    private void OnGUI()
    {
        if (__editorGuiTime <= 0f) return;

        float width = Screen.width * 0.5f;
        float height = Screen.height * 0.08f;
        Rect rect = new Rect((Screen.width - width) * 0.5f, Screen.height * 0.8f, width, height);

        if (toastStyle == null)
        {
            toastStyle = new GUIStyle(GUI.skin.box);
            toastStyle.fontSize = 36;
            toastStyle.fontStyle = FontStyle.Bold;
            toastStyle.alignment = TextAnchor.MiddleCenter;
            toastStyle.normal.textColor = Color.white;
        }

        GUI.Box(rect, __editorGuiMessage, toastStyle);
    }
    private void Update()
    {
        if (__editorGuiTime > 0f)
            __editorGuiTime -= Time.unscaledDeltaTime;
    }
#endif
}