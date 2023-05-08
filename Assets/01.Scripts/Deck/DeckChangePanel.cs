using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckChangePanel : MonoBehaviour
{
    [SerializeField]
    private DeckInfoSO[] _deckInfoList;

    void Start()
    {
        for(int i = 0; i < _deckInfoList.Length; i++)
        {
            GameObject panel = Managers.Resource.Instantiate("UI/DeckChangeButton");
            panel.GetComponentInChildren<Image>().sprite = _deckInfoList[i].DeckImage;
            panel.GetComponent<Button>().onClick.RemoveAllListeners();
            //panel.GetComponent<Button>().onClick.AddListener(() => )
        }
        
    }
}
