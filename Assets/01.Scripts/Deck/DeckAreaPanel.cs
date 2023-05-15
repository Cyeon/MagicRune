//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;

//public class DeckAreaPanel : MonoBehaviour, IDropHandler
//{
//    public DeckType DeckType = DeckType.Unknown;
//    private DeckSettingUI _deckSettingUI = null;

//    private void Awake()
//    {
//        _deckSettingUI = FindObjectOfType<DeckSettingUI>();
//    }

//    public void OnDrop(PointerEventData eventData)
//    {
//        if (_deckSettingUI.SelectRune != null)
//        {
//            _deckSettingUI.Equip(DeckType);
//        }
//    }
//}