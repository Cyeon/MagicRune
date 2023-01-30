using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum HelpChilds
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

    public void ButtonClick(int idx)
    {
        HelpChilds kind = (HelpChilds)idx;
        for (int i = 0; i < childPanels.Count; i++)
        {
            childPanels[i].SetActive(false);
        }

        switch (kind)
        {
            case HelpChilds.status:
                childPanels[(int)HelpChilds.status].SetActive(true);
                break;
            case HelpChilds.interaction:
                childPanels[(int)HelpChilds.interaction].SetActive(true);
                break;
            case HelpChilds.back:
                mainPanel.SetActive(false);
                break;
            case HelpChilds.on:
                mainPanel.SetActive(true);
                break;
            default:
                break;
        }
    }
}