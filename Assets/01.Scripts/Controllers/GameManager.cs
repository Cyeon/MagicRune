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
        EventManager.StartListening(Define.ON_START_PLAYER_TRUN, OnPlayerTurn);
        EventManager.StartListening(Define.ON_START_MONSTER_TURN, OnMonsterTurn);
    }

    private void Start() {
        enemy = EnemyManager.Instance.SpawnEnemy();
        player = FindObjectOfType<Player>();

        UIManager.instance.PlayerHealthbarInit(player.HP);

        enemy.OnTakeDamageFeedback.AddListener(() => TurnChange());
        enemy.OnTakeDamageFeedback.AddListener(() => UIManager.instance.UpdateEnemyHealthbar());
        StatusManager.Instance.AddStatus(enemy, "약쇄");

        TurnChange();
    }

    private void OnPlayerTurn()
    {
        enemy.pattern?.End();

        StatusManager.Instance.StatusTurnChange(player);
        StatusManager.Instance.StatusTurnChange(enemy);

        Debug.Log("Player Turn!");
        gameTurn = GameTurn.Player;
        currentUnit = player;

        enemy.pattern = PatternManager.Instance.GetPattern();
        enemy.pattern?.Start();
        Debug.Log(enemy.pattern.patternName);
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

        if(gameTurn == GameTurn.Player)
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