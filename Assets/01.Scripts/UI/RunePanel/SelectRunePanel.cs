using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public enum RuneSelectMode
{
    None, // Default
    Delete, // 선택 룬 삭제
    Copy, // 선택 룬 복제 
    Enhance // 선택 룬 강화 
}

public class SelectRunePanel : BasicRuneAddon
{
    private RuneSelectMode _selectMode = RuneSelectMode.None;

    public override void SetRune(BaseRune rune)
    {
        Basic.SetRune(rune);
        Basic.ClickAction -= RuneClick;
        Basic.ClickAction += RuneClick;
    }

    public void SetMode(RuneSelectMode mode)
    {
        _selectMode = mode;
    }

    private void RuneClick()
    {
        switch (_selectMode) // 모드에 따라 다른 기능 해줌 
        {
            case RuneSelectMode.Delete:
                Managers.Deck.RemoveDeck(Basic.Rune);
                break;
            case RuneSelectMode.Copy:
                Managers.Deck.AddRune(Managers.Rune.GetRune(Basic.Rune));
                break;
            case RuneSelectMode.Enhance:
                Basic.Rune.Enhance();
                break;
            case RuneSelectMode.None:
            default:
                break;
        }

        EventManager<BaseRune>.TriggerEvent(Define.SELECT_RUNE_EVENT, Basic.Rune); // RuneEventUI 쪽에서 UI 처리 해줌 
    }
}
