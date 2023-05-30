using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public struct LoadScene
{
    public GameObject LoadPanel;
    public Slider ProgressSlider;

    public IEnumerator LoadSceneCoroutine(string sceneName)
    {
        Managers.Clear();

        LoadPanel.SetActive(true);
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;

            if (op.progress < 0.9f)
            {
                ProgressSlider.value = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                ProgressSlider.value = Mathf.Lerp(0.9f, 1f, timer);
                if (ProgressSlider.value == 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}