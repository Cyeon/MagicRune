using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnUI : MonoBehaviour
{
    [SerializeField]
    private GameObject magicCirclePanel = null;
    [SerializeField]
    private GameObject turnEndButton = null;

    private void Awake()
    {
        EventManager<bool>.StartListening(Define.ON_START_PLAYER_TURN, UIOnOff);
        EventManager<bool>.StartListening(Define.ON_END_PLAYER_TURN, UIOnOff);
    }

    private void UIOnOff(bool active)
    {
        magicCirclePanel.SetActive(active);
        turnEndButton.SetActive(active);
    }

    public void PlayerTurnEnd()
    {

    }

    private void OnDestroy()
    {
        EventManager<bool>.StopListening(Define.ON_START_PLAYER_TURN, UIOnOff);
        EventManager<bool>.StopListening(Define.ON_END_PLAYER_TURN, UIOnOff);
    }
}