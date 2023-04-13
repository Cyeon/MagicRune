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
    Enemy,
    PlayerEnd,
    EnemyEnd,
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

    private void OnDestroy()
    {
        DOTween.KillAll();
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

        EventManager.TriggerEvent(Define.ON_START_PLAYER_TURN);

        enemy.PatternManager.CurrentPattern.NextPattern();

        player.StatusManager.OnTurnStart();
        enemy.PatternManager.StartAction();
    }

    public void OnMonsterTurn()
    {
        _gameTurn = GameTurn.Enemy;
        currentUnit = enemy;
        attackUnit = player;
        
        EventManager.TriggerEvent(Define.ON_START_MONSTER_TURN);

        enemy?.StatusManager.OnTurnStart();
        enemy.PatternManager.TurnAction();
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
                EventManager.TriggerEvent(Define.ON_START_PLAYER_TURN);
                _dialScene?.Turn("Player Turn");
                _gameTurn = GameTurn.EnemyEnd;
                break;

            case GameTurn.Player:
                EventManager.TriggerEvent(Define.ON_END_PLAYER_TURN);

                Managers.Sound.PlaySound(turnChangeSound, SoundType.Effect);

                player?.StatusManager.OnTurnEnd();
                player.StatusManager.TurnChange();

                if (enemy.Shield > 0)
                {
                    enemy.ResetShield();
                    _dialScene?.UpdateHealthbar(false);
                }

                _dialScene?.Turn("Enemy Turn");
                _gameTurn = GameTurn.PlayerEnd;
                break;

            case GameTurn.PlayerEnd:
                enemy.StopIdle();
                OnMonsterTurn();
                break;

            case GameTurn.Enemy:
                enemy.PatternManager.EndAction();

                EventManager.TriggerEvent(Define.ON_END_MONSTER_TURN);

                enemy?.StatusManager.OnTurnEnd();
                enemy.StatusManager.TurnChange();

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
                _gameTurn = GameTurn.EnemyEnd;
                break;

            case GameTurn.EnemyEnd:
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