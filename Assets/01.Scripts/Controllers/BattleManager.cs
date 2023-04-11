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

    public int missileCount = 0;

    private void Start()
    {
        _dialScene = Managers.Scene.CurrentScene as DialScene;

        GameStart();
    }

    public void GameStart()
    {
        enemy = Managers.Resource.Instantiate("Enemy/" + Managers.Map.SelectEnemy.name).GetComponent<Enemy>();
        enemy.Init();
        enemy.PatternManager.ChangePattern(enemy.PatternManager.patternList[0]);

        enemy.OnDieEvent.RemoveAllListeners();
        enemy.OnDieEvent.AddListener(() =>
        {
            REGold reward = new REGold();
            reward.gold = Managers.Map.CurrentChapter.Gold;
            reward.AddRewardList();

            _dialScene?.RewardUI.VictoryPanelPopup();
        });

        player = Managers.GetPlayer();
        player.StatusManager.Reset();
        player.SliderInit();

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

        enemy.PatternManager.StartAction();
        player.StatusManager.OnTurnStart();
    }

    public void OnMonsterTurn()
    {
        EventManager.TriggerEvent(Define.ON_START_MONSTER_TURN);
        _gameTurn = GameTurn.Monster;
        currentUnit = enemy;
        attackUnit = player;
        enemy?.StatusManager.OnTurnStart();

        if(_gameTurn == GameTurn.Monster)
        {
            enemy.PatternManager.TurnAction();
        }
    }

    public bool IsPlayerTurn()
    {
        return _gameTurn == GameTurn.Player;
    }

    # region Debug

    public void TurnChange()
    {
        if (enemy.HP <= 0 || player.HP <= 0) return;

        switch (_gameTurn)
        {
            case GameTurn.Unknown:
                enemy.PatternManager.StartAction();
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

                Managers.Sound.PlaySound(turnChangeSound, SoundType.Effect);

                player?.StatusManager.OnTurnEnd();

                if (enemy.Shield > 0)
                {
                    enemy.ResetShield();
                    _dialScene?.UpdateHealthbar(false);
                }

                _dialScene?.Turn("Enemy Turn");
                _gameTurn = GameTurn.PlayerWait;
                break;

            case GameTurn.PlayerWait:
                enemy.StopIdle();
                OnMonsterTurn();
                break;

            case GameTurn.Monster:
                enemy?.StatusManager.OnTurnEnd();
                enemy.PatternManager.EndAction();

                player.StatusManager.TurnChange();
                enemy.StatusManager.TurnChange();

                enemy.PatternManager.CurrentPattern.NextPattern();

                EventManager<int>.TriggerEvent(Define.ON_START_PLAYER_TURN, 5);
                EventManager.TriggerEvent(Define.ON_START_PLAYER_TURN);
                EventManager<bool>.TriggerEvent(Define.ON_START_PLAYER_TURN, true);

                Managers.Sound.PlaySound(turnChangeSound, SoundType.Effect);

                if (player.Shield > 0)
                {
                    player.ResetShield();
                    _dialScene?.UpdateHealthbar(true);
                }

                _dialScene?.Turn("Player Turn");
                _dialScene?.Dial?.ResetDial();
                _dialScene?.Dial?.AllMagicCircleGlow(false);
                _dialScene?.Dial?.AllMagicSetCoolTime();
                _dialScene?.Dial?.SettingDialRune(false);
                _dialScene?.AllCardDescPopup();
                _gameTurn = GameTurn.MonsterWait;
                break;

            case GameTurn.MonsterWait:
                enemy.Idle();
                OnPlayerTurn();
                break;
        }
    }

    

    public void MissileAttackEnd()
    {
        missileCount--;
        if (_gameTurn == GameTurn.Player && missileCount <= 0)
        {
            missileCount = 0;
            TurnChange();
        }
    }

    public void PlayerTurnEnd()
    {
        if(_gameTurn == GameTurn.Player)
        {
            TurnChange();
        }
    }

    #endregion

    public void NextStage()
    {
        Managers.Reward.ResetRewardList();
        Managers.Scene.LoadScene(Define.Scene.MapScene);
    }

}