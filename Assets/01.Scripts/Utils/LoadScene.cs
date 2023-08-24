using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

[System.Serializable]
public struct LoadScene
{
    public GameObject LoadPanel;
    public Slider ProgressSlider;
    public TextMeshProUGUI LoadingText;

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
                LoadingText.SetText(string.Format("룬을 모으는 중...{0}%", Mathf.FloorToInt(op.progress * 100)));
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                
                float timeValue = Mathf.Lerp(0.9f, 1f, timer);
                ProgressSlider.value = timeValue;
                LoadingText.SetText(string.Format("룬을 모으는 중...{0}%", Mathf.FloorToInt(timeValue * 100)));
                
                if (ProgressSlider.value == 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}