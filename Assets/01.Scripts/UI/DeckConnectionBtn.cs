using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckConnectionBtn : MonoBehaviour
{
    [SerializeField]
    private DeckSO _deckSO;

    private Button _btn;

    private void Start()
    {
        _btn = GetComponent<Button>();

        _btn.onClick.RemoveAllListeners();
        _btn.onClick.AddListener(() => Managers.Deck.SetDefaultDeck(_deckSO.RuneList));
        _btn.onClick.AddListener(() => Managers.Scene.LoadScene(Define.Scene.MapScene));
    }
}
