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
    //[Header("Buttons")]
    //public Button backButton = null;
    //public Button statusButton = null;
    //public Button interactionButton = null;

    [Header("GameObjects")]
    public GameObject mainPanel = null;
    public List<GameObject> childPanels = null;
    public GameObject statusTemplate = null;
    public GameObject interactionTemplate = null;

    [Header("Contents")]
    public Transform statusContent = null;
    public Transform interactionContent = null;

    private void Update()
    {
        if (mainPanel.activeSelf == true && Input.GetKeyDown(KeyCode.Escape))
        {
            ButtonClick((int)HelpUIButtons.back);
        }
    }

    public void ButtonClick(int idx)
    {
        HelpUIButtons kind = (HelpUIButtons)idx;
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
                mainPanel.SetActive(false);
                break;
            case HelpUIButtons.on:
                mainPanel.SetActive(true);
                childPanels[(int)HelpUIButtons.status].SetActive(true);
                break;
            default:
                break;
        }
    }
}