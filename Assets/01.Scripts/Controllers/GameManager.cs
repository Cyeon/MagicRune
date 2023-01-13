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
    public Player player = null;
    public Enemy enemy = null;
    public Unit currentUnit = null;

    public GameTurn gameTurn = GameTurn.Unknown;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();
        TurnChange();

        EventManager.StartListening(Define.ON_START_PLAYER_TRUN, OnPlayerTurn);
        EventManager.StartListening(Define.ON_START_MONSTER_TURN, OnMonsterTurn);
    }

    private void Start() {
        //enemy = EnemyManager.Instance.SpawnEnemy();
        StatusManager.Instance.AddStatus(enemy, "약쇄");
        StatusManager.Instance.AddStatus(enemy, "턴넘기기");
    }

    private void OnPlayerTurn()
    {
        gameTurn = GameTurn.Player;
        currentUnit = player;

        StatusManager.Instance.StatusTurnChange(player);
        StatusManager.Instance.StatusTurnChange(enemy);
    }

    private void OnMonsterTurn()
    {
        gameTurn = GameTurn.Monster;
        currentUnit = enemy;
    }

    # region Debug

    public void TurnChange()
    {
        currentUnit?.InvokeStatus(StatusInvokeTime.End);

        if(gameTurn == GameTurn.Player)
            OnMonsterTurn();
        else
            OnPlayerTurn();

        currentUnit?.InvokeStatus(StatusInvokeTime.Start);
        Debug.Log("Turn Change");
    }

    #endregion

    private void OnDestroy()
    {
        EventManager.StopListening(Define.ON_START_PLAYER_TRUN, OnPlayerTurn);
        EventManager.StopListening(Define.ON_START_MONSTER_TURN, OnMonsterTurn);
    }
}