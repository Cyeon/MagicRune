using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

public enum GameTurn
{
    Player,
    Monster,
    PlayerWait,
    MonsterWait,
    Unknown
}

public class BattleManager : MonoSingleton<BattleManager>
{
    [SerializeField]
    private GameTurn _gameTurn = GameTurn.Unknown;
    public GameTurn GameTurn => _gameTurn;
    public Player player = null;
    public Enemy enemy = null;
    public Unit currentUnit = null;
    public Unit attackUnit = null;

    public AudioClip turnChangeSound = null;

    //public UnityEvent OnEnemyDie;

    private DialScene _dialScene;

    private void Start()
    {
        _dialScene = SceneManagerEX.Instance.CurrentScene as DialScene;

        GameStart();
    }

    public void GameStart()
    {
        enemy = ResourceManager.Instance.Instantiate("Enemy/"+MapManager.Instance.selectEnemy.name).GetComponent<Enemy>();
        enemy.Init();
        enemy.OnDieEvent.RemoveAllListeners();
        enemy.OnDieEvent.AddListener(() =>
        {
            REGold reward = new REGold();
            reward.gold = MapManager.Instance.CurrentChapter.Gold;
            reward.AddRewardList();

            _dialScene?.RewardUI.VictoryPanelPopup();
        });
        PatternManager.Instance.PatternInit(enemy.enemyInfo.patternList);

        player = GameManager.Instance.player; // �÷��̾� ���� �������� �����ؾ���
        player.OnDieEvent.RemoveAllListeners();
        player.OnDieEvent.AddListener(() => { _dialScene?.RewardUI.DefeatPanelPopup(); });

        _dialScene?.HealthbarInit(true, player.HP, player.MaxHealth);

        FeedbackManager.Instance.Init();

        TurnChange();
    }

    private void OnPlayerTurn()
    {
        //EventManager.TriggerEvent(Define.ON_END_MONSTER_TURN);

        _gameTurn = GameTurn.Player;
        currentUnit = player;
        attackUnit = enemy;
    }

    public void OnMonsterTurn()
    {
        EventManager.TriggerEvent(Define.ON_START_MONSTER_TURN);
        _gameTurn = GameTurn.Monster;
        currentUnit = enemy;
        attackUnit = player;
        enemy.patternM.TurnAction();
    }

    public bool IsPlayerTurn()
    {
        return _gameTurn == GameTurn.Player;
    }

    # region Debug

    public void TurnChange()
    {
        if (enemy.HP <= 0 || player.HP <= 0) return;

        if (_gameTurn == GameTurn.Player || _gameTurn == GameTurn.Monster)
        {
            player?.InvokeStatus(StatusInvokeTime.End);
            enemy?.InvokeStatus(StatusInvokeTime.End);

            StatusManager.Instance.StatusUpdate(player);
            StatusManager.Instance.StatusUpdate(enemy);
        }

        switch (_gameTurn)
        {
            case GameTurn.Unknown:
                enemy.patternM.StartAction();
                EventManager<int>.TriggerEvent(Define.ON_START_PLAYER_TURN, 5);
                EventManager.TriggerEvent(Define.ON_START_PLAYER_TURN);
                EventManager<bool>.TriggerEvent(Define.ON_START_PLAYER_TURN, true);

                //SoundManager.instance.PlaySound(turnChangeSound, SoundType.Effect);

                //UIManager.Instance.Turn("Player Turn");
                _dialScene?.Turn("Player Turn");
                _gameTurn = GameTurn.MonsterWait;
                break;

            case GameTurn.Player:

                EventManager<bool>.TriggerEvent(Define.ON_START_MONSTER_TURN, false);

                SoundManager.Instance.PlaySound(turnChangeSound, SoundType.Effect);

                if (enemy.Shield > 0)
                {
                    enemy.ResetShield();
                    _dialScene?.UpdateHealthbar(false);
                }

                //UIManager.Instance.Turn("Enemy Turn");
                _dialScene?.Turn("Enemy Turn");
                _gameTurn = GameTurn.PlayerWait;
                break;

            case GameTurn.PlayerWait:
                enemy.StopIdle();
                OnMonsterTurn();
                break;

            case GameTurn.Monster:
                StatusManager.Instance.StatusTurnChange(player);
                StatusManager.Instance.StatusTurnChange(enemy);

                enemy.patternM.EndAction();
                enemy.patternM.CurrentPattern.NextPattern();
                enemy.patternM.StartAction();

                EventManager<int>.TriggerEvent(Define.ON_START_PLAYER_TURN, 5);
                EventManager.TriggerEvent(Define.ON_START_PLAYER_TURN);
                EventManager<bool>.TriggerEvent(Define.ON_START_PLAYER_TURN, true);

                SoundManager.Instance.PlaySound(turnChangeSound, SoundType.Effect);

                if (player.Shield > 0)
                {
                    player.ResetShield();
                    _dialScene?.UpdateHealthbar(true);
                }

                //UIManager.Instance.Turn("Player Turn");
                _dialScene?.Turn("Player Turn");
                _dialScene?.Dial?.ResetDial();
                _dialScene?.Dial?.AllMagicSetCoolTime();
                _dialScene?.Dial?.SettingDialRune(false);
                _gameTurn = GameTurn.MonsterWait;
                break;

            case GameTurn.MonsterWait:
                enemy.Idle();
                OnPlayerTurn();
                break;
        }

        if (_gameTurn == GameTurn.PlayerWait || _gameTurn == GameTurn.MonsterWait)
            currentUnit?.InvokeStatus(StatusInvokeTime.Start);
    }

    

    public void PlayerTurnEnd()
    {
        if (_gameTurn == GameTurn.Player)
        {
            TurnChange();
        }
    }

    #endregion

    public void NextStage()
    {
        RewardManager.ResetRewardList();
        SceneManagerEX.Instance.LoadScene(Define.Scene.MapScene);
    }

}