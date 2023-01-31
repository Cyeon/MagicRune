using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HelpUIButtons
{
    status,
    interaction,
    back,
    on
}

public class HelpUI : MonoBehaviour
{
    [Header("Buttons")]
    public Button backButton = null;
    public Button onButton = null;
    public Button statusButton = null;
    public Button interactionButton = null;

    [Header("GameObjects")]
    public GameObject mainPanel = null;
    public GameObject statusTemplate = null;
    public GameObject interactionTemplate = null;
    public GameObject deckViewUI = null;
    public GameObject restViewUI = null;
    public List<GameObject> childPanels = null;

    [Header("Contents")]
    public Transform statusContent = null;
    public Transform interactionContent = null;

    private void Awake()
    {
        backButton.onClick.AddListener(() => ButtonClick(HelpUIButtons.back));
        onButton.onClick.AddListener(() => ButtonClick(HelpUIButtons.on));
        statusButton.onClick.AddListener(() => ButtonClick(HelpUIButtons.status));
        interactionButton.onClick.AddListener(() => ButtonClick(HelpUIButtons.interaction));
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
            default:
                break;
        }
    }
}