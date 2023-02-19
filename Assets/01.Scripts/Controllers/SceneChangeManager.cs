using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    public enum SceneName
    {
        MainBattleScene,
        MapScene
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadEnumScene(SceneName name)
    {
        SceneManager.LoadScene((int)name);
    }
}
