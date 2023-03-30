using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AdventureUI : MonoBehaviour
{
    private Image _adventureImage;
    private TextMeshProUGUI _titleText;

    [SerializeField]
    private GameObject _storyPanel;
    private TextMeshProUGUI _storyText;

    private GameObject _distractorPanel;
    private Button[] _distractorButtons;
    private List<TextMeshProUGUI> _distractorText = new List<TextMeshProUGUI>();

    private void Start()
    {
        UIManager.Instance.Bind<Image>("Adventure_Image", CanvasManager.Instance.GetCanvas("Adventure").gameObject);
        UIManager.Instance.Bind<TextMeshProUGUI>("AdventureTitle_Text", CanvasManager.Instance.GetCanvas("Adventure").gameObject);
        UIManager.Instance.Bind<TextMeshProUGUI>("Story_Text", CanvasManager.Instance.GetCanvas("Adventure").gameObject);
        UIManager.Instance.Bind<Button>("Story_Button", CanvasManager.Instance.GetCanvas("Adventure").gameObject);

        _adventureImage = UIManager.Instance.Get<Image>("Adventure_Image");
        _titleText = UIManager.Instance.Get<TextMeshProUGUI>("AdventureTitle_Text");

        _storyText = UIManager.Instance.Get<TextMeshProUGUI>("Story_Text");
        UIManager.Instance.Get<Button>("Story_Button").onClick.AddListener(() => StoryClick());

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
        _adventureImage.sprite = info.image;

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
