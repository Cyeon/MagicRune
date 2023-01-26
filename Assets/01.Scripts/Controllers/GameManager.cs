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

    private void Awake()
    {
        EventManager.StartListening(Define.ON_START_PLAYER_TURN, OnPlayerTurn);
        EventManager.StartListening(Define.ON_START_MONSTER_TURN, OnMonsterTurn);
    }

    private void Start() {

        enemy = EnemyManager.Instance.SpawnEnemy();
        player = FindObjectOfType<Player>();

        UIManager.instance.PlayerHealthbarInit(player.HP);

        enemy.OnTakeDamageFeedback.AddListener(() => TurnChange());
        enemy.OnTakeDamageFeedback.AddListener(() => UIManager.instance.UpdateEnemyHealthbar());
        // StatusManager.Instance.AddStatus(enemy, "?½ì‡„");

        TurnChange();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            OnMonsterTurn();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            OnPlayerTurn();
        }
    }

    private void OnPlayerTurn()
    {
        EventManager.TriggerEvent(Define.ON_END_MONSTER_TURN);

        Debug.Log("Player Turn!");
        gameTurn = GameTurn.Player;
        currentUnit = player;
    }

    public void OnMonsterTurn()
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
        {
            OnMonsterTurn();
        }
        else
        {
            StatusManager.Instance.StatusTurnChange(player);
            StatusManager.Instance.StatusTurnChange(enemy);

            enemy.pattern?.End();

            enemy.pattern = PatternManager.Instance.GetPattern();
            enemy.pattern?.Start();
            Debug.Log(enemy.pattern.patternName);

            OnPlayerTurn();
        }
           

        currentUnit?.InvokeStatus(StatusInvokeTime.Start);
    }

    #endregion

    private void OnDestroy()
    {
        EventManager.StopListening(Define.ON_START_PLAYER_TURN, OnPlayerTurn);
        EventManager.StopListening(Define.ON_START_MONSTER_TURN, OnMonsterTurn);
    }

}