using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AdventureUI : MonoBehaviour
{
    private Image _cg;
    private TextMeshProUGUI _titleText;
        
    private GameObject _storyPanel;
    private TextMeshProUGUI _storyText;

    private GameObject _distractorPanel;
    private Button[] _distractorButtons;
    private List<TextMeshProUGUI> _distractorText = new List<TextMeshProUGUI>();

    private void Start()
    {
        UIManager.Instance.Bind<Image>("CG", CanvasManager.Instance.GetCanvas("Adventure").gameObject);
        UIManager.Instance.Bind<TextMeshProUGUI>("Title Text", CanvasManager.Instance.GetCanvas("Adventure").gameObject);
        UIManager.Instance.Bind<GameObject>("Story", CanvasManager.Instance.GetCanvas("Adventure").gameObject);
        UIManager.Instance.Bind<TextMeshProUGUI>("Story Text", CanvasManager.Instance.GetCanvas("Adventure").gameObject);
        UIManager.Instance.Bind<Button>("Story Button", CanvasManager.Instance.GetCanvas("Adventure").gameObject);

        _cg = UIManager.Instance.Get<Image>("CG");
        _titleText = UIManager.Instance.Get<TextMeshProUGUI>("Title Text");

        _storyPanel = UIManager.Instance.Get<GameObject>("Story");
        _storyText = UIManager.Instance.Get<TextMeshProUGUI>("Story Text");
        UIManager.Instance.Get<Button>("Story Button").onClick.AddListener(() => StoryClick());

        _distractorPanel = transform.Find("Distractor").gameObject;
        _distractorButtons = _distractorPanel.GetComponentsInChildren<Button>(true);
        _distractorButtons.ForEach(x => _distractorText.Add(x.GetComponentInChildren<TextMeshProUGUI>(true)));

        GetComponent<Canvas>().enabled = false;
    }

    private void StoryClick()
    {
        _storyPanel.SetActive(false);
        _distractorPanel.SetActive(true);

    }

    public void Init(AdventureSO info, AdventurePortal portal)
    {
        _storyPanel.SetActive(true);
        _distractorButtons.ForEach(x => x.gameObject.SetActive(false));
        _distractorPanel.SetActive(false);

        _titleText.text = info.adventureName;
        _cg.sprite = info.image;

        _storyText.text = info.message;

        for (int i = 0; i < info.distractors.Count; i++)
        {
            _distractorText[i].text = info.distractors[i].text;
            _distractorButtons[i].onClick.RemoveAllListeners();
            int index = i;
            _distractorButtons[i].onClick.AddListener(() => info.distractors[index].function.Invoke());
            _distractorButtons[i].gameObject.SetActive(true);
        }
    }
}
