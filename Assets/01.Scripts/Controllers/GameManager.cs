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

    public GameTurn gameTurn = GameTurn.Player;

    private void Awake()
    {
        EventManager.StartListening(Define.ON_START_PLAYER_TRUN, OnPlayerTurn);
        EventManager.StartListening(Define.ON_START_MONSTER_TURN, OnMonsterTurn);
    }

    private void Start() {
        enemy = EnemyManager.Instance.SpawnEnemy();
        player = FindObjectOfType<Player>();
        currentUnit = player;

        UIManager.instance.PlayerHealthbarInit(player.HP);

        enemy.OnTakeDamageFeedback.AddListener(() => TurnChange());
        enemy.OnTakeDamageFeedback.AddListener(() => UIManager.instance.UpdateEnemyHealthbar());
        StatusManager.Instance.AddStatus(enemy, "약쇄");
    }

    private void OnPlayerTurn()
    {
        Debug.Log("Player Turn!");
        gameTurn = GameTurn.Player;
        currentUnit = player;

        StatusManager.Instance.StatusTurnChange(player);
        StatusManager.Instance.StatusTurnChange(enemy);
    }

    private void OnMonsterTurn()
    {
        Debug.Log("Enemy Turn!");
        gameTurn = GameTurn.Monster;
        currentUnit = enemy;
        enemy.TurnStart();
    }

    # region Debug

    public void TurnChange()
    {
        currentUnit?.InvokeStatus(StatusInvokeTime.End);

        if(gameTurn == GameTurn.Player || gameTurn == GameTurn.Unknown)
            OnMonsterTurn();
        else
            OnPlayerTurn();

        currentUnit?.InvokeStatus(StatusInvokeTime.Start);
    }

    #endregion

    private void OnDestroy()
    {
        EventManager.StopListening(Define.ON_START_PLAYER_TRUN, OnPlayerTurn);
        EventManager.StopListening(Define.ON_START_MONSTER_TURN, OnMonsterTurn);
    }
}