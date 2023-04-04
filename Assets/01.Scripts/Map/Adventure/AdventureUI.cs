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

    private void Awake()
    {
        _cg = transform.Find("CG").GetComponent<Image>();
        _titleText = transform.Find("Title/Text").GetComponent<TextMeshProUGUI>();

        _storyPanel = transform.Find("Story").gameObject;
        _storyText = _storyPanel.transform.Find("StoryText").GetComponent<TextMeshProUGUI>();
        _storyPanel.transform.Find("Button").GetComponent<Button>().onClick.AddListener(() => StoryClick());

        _distractorPanel = transform.Find("Distractor").gameObject;
        _distractorButtons = _distractorPanel.GetComponentsInChildren<Button>(true);
        _distractorButtons.ForEach(x => _distractorText.Add(x.GetComponentInChildren<TextMeshProUGUI>(true)));
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
