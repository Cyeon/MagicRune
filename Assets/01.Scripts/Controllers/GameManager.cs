using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameTurn
{
    Player,
    Monster,
    Unknown
}

public class GameManager : MonoSingleton<GameManager>
{
    private GameTurn gameTurn = GameTurn.Unknown;
    public GameTurn GameTurn => gameTurn;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            OnPlayerTurn();
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            OnMonsterTurn();
        }
    }

    public void OnPlayerTurn()
    {
        gameTurn = GameTurn.Player;
        EventManager.TriggerEvent(Define.ON_START_PLAYER_TURN);
        EventManager<bool>.TriggerEvent(Define.ON_START_PLAYER_TURN, true);
    }

    public void OnMonsterTurn()
    {
        EventManager.TriggerEvent(Define.ON_END_PLAYER_TURN);
        gameTurn = GameTurn.Monster;
        EventManager<bool>.TriggerEvent(Define.ON_END_PLAYER_TURN, false);

        EventManager.TriggerEvent(Define.ON_START_MONSTER_TURN);
    }

}