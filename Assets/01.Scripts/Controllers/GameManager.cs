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
    public GameTurn gameTurn = GameTurn.Unknown;

    private void Awake()
    {
        EventManager.StartListening(Define.ON_START_PLAYER_TRUN, OnPlayerTurn);
        EventManager.StartListening(Define.ON_START_MONSTER_TURN, OnMonsterTurn);
    }

    private void OnPlayerTurn()
    {
        gameTurn = GameTurn.Player;
    }

    private void OnMonsterTurn()
    {
        gameTurn = GameTurn.Monster;
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Define.ON_START_PLAYER_TRUN, OnPlayerTurn);
        EventManager.StopListening(Define.ON_START_MONSTER_TURN, OnMonsterTurn);
    }
}