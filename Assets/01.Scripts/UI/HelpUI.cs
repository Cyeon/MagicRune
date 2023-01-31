using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HelpUIButtons
{
    status,
    interaction,
    gameScene,
    manual,
    back,
    on
}

[System.Serializable]
public struct BasicHelpUI
{
    public string title;
    public Sprite icon;
    [TextArea]
    public string description;
}

[System.Serializable]
public struct InteractionHelpUI
{
    public string title;
    public Sprite icon1;
    public Sprite icon2;
    [TextArea]
    public string description;
}

public class HelpUI : MonoBehaviour
{
    [Header("Buttons")]
    public Button backButton = null;
    public Button onButton = null;
    public Button statusButton = null;
    public Button interactionButton = null;
    public Button gameSceneButton = null;
    public Button manualButton = null;

    [Header("GameObjects")]
    public GameObject mainPanel = null;
    public GameObject basicTemplate = null;
    public GameObject interactionTemplate = null;
    public GameObject deckViewUI = null;
    public GameObject restViewUI = null;
    public List<GameObject> childPanels = null;

    [Header("Contents")]
    public Transform statusContent = null;
    public Transform gameSceneContent = null;
    public Transform interactionContent = null;

    [Header("Lists")]
    public List<BasicHelpUI> statusList = new List<BasicHelpUI>();
    public List<BasicHelpUI> gameSceneList = new List<BasicHelpUI>();
    public List<InteractionHelpUI> interactionList = new List<InteractionHelpUI>();

    private void Awake()
    {
        backButton.onClick.AddListener(() => ButtonClick(HelpUIButtons.back));
        onButton.onClick.AddListener(() => ButtonClick(HelpUIButtons.on));
        statusButton.onClick.AddListener(() => ButtonClick(HelpUIButtons.status));
        interactionButton.onClick.AddListener(() => ButtonClick(HelpUIButtons.interaction));
        gameSceneButton.onClick.AddListener(() => ButtonClick(HelpUIButtons.gameScene));
        manualButton.onClick.AddListener(() => ButtonClick(HelpUIButtons.manual));

        TemplateInstantiate();
    }

    private void Update()
    {
        if (mainPanel.activeSelf == true && Input.GetKeyDown(KeyCode.Escape))
        {
            ButtonClick(HelpUIButtons.back);
        }
    }

    public void ButtonClick(HelpUIButtons kind)
    {
        for (int i = 0; i < childPanels.Count; i++)
        {
            childPanels[i].SetActive(false);
        }

        switch (kind)
        {
            case HelpUIButtons.status:
                childPanels[(int)HelpUIButtons.status].SetActive(true);
                break;
            case HelpUIButtons.interaction:
                childPanels[(int)HelpUIButtons.interaction].SetActive(true);
                break;
            case HelpUIButtons.back:
                deckViewUI.SetActive(true);
                restViewUI.SetActive(true);
                mainPanel.SetActive(false);
                break;
            case HelpUIButtons.on:
                mainPanel.SetActive(true);
                deckViewUI.SetActive(false);
                restViewUI.SetActive(false);
                childPanels[(int)HelpUIButtons.status].SetActive(true);
                break;
            case HelpUIButtons.gameScene:
                childPanels[(int)HelpUIButtons.gameScene].SetActive(true);
                break;
            case HelpUIButtons.manual:
                childPanels[(int)HelpUIButtons.manual].SetActive(true);
                break;
            default:
                break;
        }
    }

    private void TemplateInstantiate()
    {
        for (int i = 0; i < statusList.Count; i++)
        {
            GameObject gameObject = Instantiate(basicTemplate, statusContent);
            gameObject.SetActive(true);
            BasicHelpUI basic = statusList[i];
            gameObject.name = basic.title;
            gameObject.transform.GetChild(0).GetComponent<Image>().sprite = basic.icon;
            Text text = gameObject.transform.GetChild(1).GetComponent<Text>();
            text.color = Color.white;
            text.text = $"<size=100><b>{basic.title}</b></size>\n{basic.description}";
        }

        for (int i = 0; i < gameSceneList.Count; i++)
        {
            GameObject gameObject = Instantiate(basicTemplate, gameSceneContent);
            gameObject.SetActive(true);
            BasicHelpUI basic = gameSceneList[i];
            gameObject.name = basic.title;
            gameObject.transform.GetChild(0).GetComponent<Image>().sprite = basic.icon;
            Text text = gameObject.transform.GetChild(1).GetComponent<Text>();
            text.color = Color.white;
            text.text = $"<size=100><b>{basic.title}</b></size>\n{basic.description}";
        }

        for (int i = 0; i < interactionList.Count; i++)
        {
            GameObject gameObject = Instantiate(interactionTemplate, interactionContent);
            gameObject.SetActive(true);
            InteractionHelpUI basic = interactionList[i];
            gameObject.name = basic.title;
            gameObject.transform.GetChild(1).GetComponent<Image>().sprite = basic.icon1;
            gameObject.transform.GetChild(2).GetComponent<Image>().sprite = basic.icon2;
            gameObject.transform.GetChild(3).GetComponent<Text>().text = $"<b>{basic.title}</b>";
            gameObject.transform.GetChild(4).GetComponent<Text>().text = basic.description;
        }
    }
}