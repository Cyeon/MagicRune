using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEX : MonoSingleton<SceneManagerEX>
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(true); } }

    public void LoadScene(Define.Scene type)
    {
        SceneManager.LoadScene(GetSceneName(type));
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    string GetSceneName(Define.Scene type)
    {
        string name = Enum.GetName(typeof(Define.Scene), type);
        return name;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}