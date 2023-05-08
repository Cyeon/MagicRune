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
    private Button _storyPanelClickBtn;
    private TextMeshProUGUI _storyText;

    private GameObject _distractorPanel;
    private Button[] _distractorButtons;
    private List<TextMeshProUGUI> _distractorText = new List<TextMeshProUGUI>();


    private void Start()
    {
        Managers.UI.Bind<Image>("Adventure_Image", Managers.Canvas.GetCanvas("Adventure").gameObject);
        Managers.UI.Bind<TextMeshProUGUI>("AdventureTitle_Text", Managers.Canvas.GetCanvas("Adventure").gameObject);
        Managers.UI.Bind<TextMeshProUGUI>("Story_Text", Managers.Canvas.GetCanvas("Adventure").gameObject);
        Managers.UI.Bind<Button>("Story_Button", Managers.Canvas.GetCanvas("Adventure").gameObject);

        _adventureImage = Managers.UI.Get<Image>("Adventure_Image");
        _titleText = Managers.UI.Get<TextMeshProUGUI>("AdventureTitle_Text");

        _storyText = Managers.UI.Get<TextMeshProUGUI>("Story_Text");
        _storyPanelClickBtn = Managers.UI.Get<Button>("Story_Button");

        _storyPanelClickBtn.onClick.AddListener(() => StoryClick());

        _distractorPanel = transform.Find("Distractor").gameObject;
        _distractorButtons = _distractorPanel.GetComponentsInChildren<Button>(true);
        _distractorButtons.ForEach(x => _distractorText.Add(x.GetComponentInChildren<TextMeshProUGUI>(true)));

        GetComponent<Canvas>().enabled = false;
    }

    private void StoryClick()
    {
        //_storyPanel.SetActive(false);
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
            _distractorText[i].text = info.distractors[i].distractorText;
            _distractorButtons[i].onClick.RemoveAllListeners();

            int index = i;
            _distractorButtons[i].onClick.AddListener(() =>
            {
                info.distractors[index].function.Invoke();
                _storyText.text = info.distractors[index].resultText;
                _distractorPanel.SetActive(false);

                _storyPanelClickBtn.onClick.RemoveAllListeners();
                _storyPanelClickBtn.onClick.AddListener(() => { DistracotrFuncList.NextStage(); });
            });

            if (info.distractors[index].resultText == "")
                _distractorButtons[i].onClick.AddListener(() => DistracotrFuncList.NextStage());

            _distractorButtons[i].gameObject.SetActive(true);
        }
    }

}