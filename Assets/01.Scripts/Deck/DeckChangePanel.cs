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
            //panel.GetComponentInChildren<Image>().sprite = _deckInfoList[index].DeckImage;
            panel.transform.GetChild(0).GetComponent<Image>().sprite = _deckInfoList[index].DeckImage;
            panel.GetComponent<Button>().onClick.RemoveAllListeners();
            panel.GetComponent<Button>().onClick.AddListener(() => _deckInfoPanel.SetInfo(_deckInfoList[index]));
            panel.GetComponent<Button>().onClick.AddListener(() => Managers.Deck.SetDefaultDeck(_deckInfoList[index].DeckSO.RuneList));
            panel.GetComponent<Button>().onClick.AddListener(() => SelectButton(index));
        }

        Managers.Deck.SetDefaultDeck(_deckInfoList[0].DeckSO.RuneList);
        _deckInfoPanel.SetInfo(_deckInfoList[0]);
        SelectButton(0);
    }

    private void SelectButton(int index)
    {
        for (int i = 0; i < _deckInfoList.Length; i++)
        {
            if(i == index)
            {
                transform.GetChild(i).GetComponent<Image>().color = Color.white;
            }
            else
            {
                transform.GetChild(i).GetComponent<Image>().color = Color.clear;
            }
        }
    }
}
