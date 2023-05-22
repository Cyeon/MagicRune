using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum GameTurn
{
    Player,
    Enemy,
    PlayerEnd,
    EnemyEnd,
    Ready
}

public class BattleManager : MonoSingleton<BattleManager>
{
    [SerializeField]
    private GameTurn _gameTurn = GameTurn.Ready;
    public GameTurn GameTurn => _gameTurn;

    public Player Player => Managers.GetPlayer();
    public Enemy Enemy => Managers.Enemy.CurrentEnemy;

    public int missileCount = 0;

    [SerializeField]
    private AudioClip _turnChangeSound = null;

    private void Start()
    {
        Managers.UI.Bind<Image>("Background", Managers.Canvas.GetCanvas("BG").gameObject);
        BattleStart();
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }

    public void BattleStart()
    {
        Managers.UI.Get<Image>("Background").sprite = Managers.Map.CurrentChapter.background;

        Managers.Enemy.BattleSetting();

        Player.StatusManager.Reset();
        Player.SetUIActive(true);
        Player.UISetting();

        Player.OnDieEvent.RemoveAllListeners();
        Player.OnDieEvent.AddListener(() => { Define.DialScene?.RewardUI.DefeatPanelPopup(); });

        TurnChange();

        //Define.DialScene?.CardDescPopup(Define.DialScene?.Dial?.DialElementList[2].SelectCard.Rune);
    }

    private void OnPlayerTurn()
    {
        _gameTurn = GameTurn.Player;

        EventManager.TriggerEvent(Define.ON_START_PLAYER_TURN);

        if (Enemy.isTurnSkip)
        {
            Enemy.isTurnSkip = false;
        }
        else
        {
            if (Enemy.PatternManager.CurrentPattern == null) Enemy.PatternManager.NextPattern();
            else Enemy.PatternManager.CurrentPattern.NextPattern();
        }

        Player.StatusManager.OnTurnStart();
        Enemy.PatternManager.StartAction();

        if (Player.isTurnSkip == true)
        {
            TurnChange();
            Player.isTurnSkip = false;
        }
    }

    public void OnMonsterTurn()
    {
        _gameTurn = GameTurn.Enemy;

        EventManager.TriggerEvent(Define.ON_START_MONSTER_TURN);

        Enemy.StatusManager.OnTurnStart();
        StartCoroutine(EnemyStatusWaitCoroutine());
    }

    IEnumerator EnemyStatusWaitCoroutine()
    {
        while(Enemy.IsHitAnimationPlaying())
        {
            yield return new WaitForEndOfFrame();
        }

        Enemy.PatternManager.TurnAction();
    }

    public bool IsPlayerTurn()
    {
        return _gameTurn == GameTurn.Player;
    }

    # region Debug

    public void TurnChange()
    {
        if (Enemy.HP <= 0 || Player.HP <= 0) return;

        switch (_gameTurn)
        {
            case GameTurn.Ready:
                EventManager.TriggerEvent(Define.ON_START_PLAYER_TURN);
                Define.DialScene?.Turn("Player Turn");
                _gameTurn = GameTurn.EnemyEnd;
                break;

            case GameTurn.Player:
                EventManager.TriggerEvent(Define.ON_END_PLAYER_TURN);

                Managers.Sound.PlaySound(_turnChangeSound, SoundType.Effect);

                Player?.StatusManager.OnTurnEnd();

                if (Enemy.Shield > 0)
                {
                    Enemy.ResetShield();
                    Enemy.UpdateHealthUI();
                }

                Define.DialScene?.Turn("Enemy Turn");
                _gameTurn = GameTurn.PlayerEnd;
                break;

            case GameTurn.PlayerEnd:
                //Enemy.StopIdle();
                Define.DialScene?.CardDescDown();
                OnMonsterTurn();
                break;

            case GameTurn.Enemy:
                Enemy.PatternManager.EndAction();

                EventManager.TriggerEvent(Define.ON_END_MONSTER_TURN);

                Enemy?.StatusManager.OnTurnEnd();

                Enemy.StatusManager.TurnChange();
                Player.StatusManager.TurnChange();

                Managers.Sound.PlaySound(_turnChangeSound, SoundType.Effect);

                if (Player.Shield > 0) Player.ResetShield();

                Define.DialScene?.Turn("Player Turn");
                Define.DialScene?.Dial?.ResetDial();
                Define.DialScene?.Dial?.AllMagicCircleGlow(false);
                Define.DialScene?.Dial?.AllMagicSetCoolTime();
                Define.DialScene?.Dial?.SettingDialRune(false);
                //Define.DialScene?.CardDescPopup(Define.DialScene?.Dial?.DialElementList[2].SelectCard.Rune);
                _gameTurn = GameTurn.EnemyEnd;
                break;

            case GameTurn.EnemyEnd:
                //Enemy.Idle();
                OnPlayerTurn();
                break;
        }
    }

    /// <summary>
    /// 미사일 공격이 끝날떄 발동되는 함수 (모든 공격이 다 들어간 후에 턴이 체인지 되도록 만듬)
    /// </summary>
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
        if (_gameTurn == GameTurn.Player)
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