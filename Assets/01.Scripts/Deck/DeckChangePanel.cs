using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckChangePanel : MonoBehaviour
{
    [SerializeField]
    private DeckInfoSO[] _deckInfoList;

    [SerializeField]
    private DeckInfoPanel _deckInfoPanel;

    void Start()
    {
        for(int i = 0; i < _deckInfoList.Length; i++)
        {
            int index = i;
            GameObject panel = Managers.Resource.Instantiate("UI/DeckChangeButton", this.transform);
            panel.GetComponentInChildren<Image>().sprite = _deckInfoList[index].DeckImage;
            panel.GetComponent<Button>().onClick.RemoveAllListeners();
            panel.GetComponent<Button>().onClick.AddListener(() => _deckInfoPanel.SetInfo(_deckInfoList[index]));
        }

        _deckInfoPanel.SetInfo(_deckInfoList[0]);
    }
}
