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
    public Player player = null;
    public Enemy enemy = null;
    public Unit currentUnit = null;

    private void Update()
    {
        EventManager.StartListening(Define.ON_START_PLAYER_TURN, OnPlayerTurn);
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
        EventManager.TriggerEvent(Define.ON_START_PLAYER_TURN);
        EventManager<bool>.TriggerEvent(Define.ON_START_PLAYER_TURN, true);
        currentUnit = player;

        StatusManager.Instance.StatusTurnChange(player);
        StatusManager.Instance.StatusTurnChange(enemy);
    }

    public void OnMonsterTurn()
    {
        EventManager.TriggerEvent(Define.ON_END_PLAYER_TURN);
        gameTurn = GameTurn.Monster;
        EventManager<bool>.TriggerEvent(Define.ON_END_PLAYER_TURN, false);

        EventManager.TriggerEvent(Define.ON_START_MONSTER_TURN);
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
        EventManager.StopListening(Define.ON_START_PLAYER_TURN, OnPlayerTurn);
        EventManager.StopListening(Define.ON_START_MONSTER_TURN, OnMonsterTurn);
    }

}