using System;
using UnityEngine;

[Serializable]
public class EffectPair
{
    public Condition Condition;
    public string Effect;
    public float Value;
}

[Serializable]
public class EffectObjectPair
{
    public Pair pair;
    public GameObject effect;

    public EffectObjectPair(Pair pair, GameObject effect)
    {
        this.pair = pair;
        this.effect = effect;
    }
}

/// <summary>
/// 상수 매니저
/// </summary>
public class Define
{
    public const int ON_START_PLAYER_TURN = 100;
    public const int ON_START_MONSTER_TURN = 101;

    public const int ON_END_PLAYER_TURN = 200;
    public const int ON_END_MONSTER_TURN = 201;

    public static int ON_ATTACK_PLAYER = 202;
    public static int ON_ATTACK_ENEMY = 203;

    public static int ON_GET_DAMAGE_PLAYTER = 204;
    public static int ON_GET_DAMAGE_ENEMY = 205;

    public const int ON_ADD_STATUS = 300;

    public const int GAME_WIN = 1000;
    public const int GAME_LOSE = 1001;
    public const int GAME_END = 1002;

    public const int CLICK_VIEW_UI = 10000;

    private static Camera _mainCam;
    public static Camera MainCam
    {
        get
        {
            if(_mainCam == null)
            {
                _mainCam = Camera.main;
            }
            return _mainCam;
        }
    }

    public enum Scene
    {
        Unknown,
        MapScene,
        DialScene,
    }
}