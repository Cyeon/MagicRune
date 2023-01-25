using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameTurn
{
    Player,
    Monster,
    Unknown,
    PlayerWait,
    MonsterWait
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
        // StatusManager.Instance.AddStatus(enemy, "약쇄");

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

        gameTurn = GameTurn.Player;
        currentUnit = player;
    }

    public void OnMonsterTurn()
    {
        gameTurn = GameTurn.Monster;
        currentUnit = enemy;
        enemy.TurnStart();
    }

    # region Debug

    public void TurnChange()
    {
        if (gameTurn == GameTurn.Player || gameTurn == GameTurn.Monster)
            currentUnit?.InvokeStatus(StatusInvokeTime.End);

        switch(gameTurn)
        {
            case GameTurn.Player:
                UIManager.Instance.Turn("Enemy Turn");
                gameTurn = GameTurn.PlayerWait;
                break;

            case GameTurn.PlayerWait:
                OnMonsterTurn();
                break;

            case GameTurn.Monster:
            case GameTurn.Unknown:
                StatusManager.Instance.StatusTurnChange(player);
                StatusManager.Instance.StatusTurnChange(enemy);

                enemy.pattern?.End();

                enemy.pattern = PatternManager.Instance.GetPattern();
                enemy.pattern?.Start();

                UIManager.Instance.Turn("Player Turn");
                gameTurn = GameTurn.MonsterWait;
                break;

            case GameTurn.MonsterWait:
                OnPlayerTurn();
                break;
        }

        if (gameTurn == GameTurn.PlayerWait || gameTurn == GameTurn.MonsterWait)
            currentUnit?.InvokeStatus(StatusInvokeTime.Start);
    }

    #endregion

    private void OnDestroy()
    {
        EventManager.StopListening(Define.ON_START_PLAYER_TURN, OnPlayerTurn);
        EventManager.StopListening(Define.ON_START_MONSTER_TURN, OnMonsterTurn);
    }

}