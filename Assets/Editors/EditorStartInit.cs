#if UNITY_EDITOR
using UnityEditor;

[InitializeOnLoad]
public class EditorStartInit
{
    [MenuItem("SceneStart/시작 씬부터 시작")]
    public static void SetupFromStartScene()
    {
        var pathOfFirstScene = EditorBuildSettings.scenes[0].path;
        var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);
        UnityEditor.SceneManagement.EditorSceneManager.playModeStartScene = sceneAsset;
        EditorApplication.isPlaying = true;
    }

    [MenuItem("SceneStart/현재 씬부터 시작")]
    public static void StartFromThisScene()
    {
        UnityEditor.SceneManagement.EditorSceneManager.playModeStartScene = null;
        EditorApplication.isPlaying = true;
    }
}
#endif