using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    [SerializeField]
    private DeckSO _deck;

    private Button _btn;

    private List<BaseRune> _runeList = new List<BaseRune>();

    private void Start()
    {
        _btn = GetComponent<Button>();

        //Init();

        _btn.onClick.RemoveAllListeners();
        //_btn.onClick.AddListener(() => Managers.Deck.SetDefaultDeck(_runeList));
        _btn.onClick.AddListener(() => Managers.Deck.SetDefaultDeck(_deck.RuneList));
        _btn.onClick.AddListener(() => Managers.Scene.LoadScene(Define.Scene.MapScene));
    }

    protected virtual void Init()
    {

    }

    protected void AddRune(BaseRune rune)
    {
        _runeList.Add(rune);
    }
}
