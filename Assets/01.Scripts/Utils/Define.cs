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
/// ?怨몃땾 筌띲끇???
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

    public const int SELECT_RUNE_EVENT = 5000;
    public const int RUNE_EVENT_SETTING = 5001;

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

    private static DialScene _dialScene;
    public static DialScene DialScene
    {
        get
        {
            if(_dialScene == null)
            {
                _dialScene = Managers.Scene.CurrentScene as DialScene;
            }

            return _dialScene;
        }
    }

    private static MapScene _mapScene;
    public static MapScene MapScene
    {
        get
        {
            if (_mapScene == null)
            {
                _mapScene = Managers.Scene.CurrentScene as MapScene;
            }

            return _mapScene;
        }
    }


    public enum Scene
    {
        Unknown,
        LobbyScene,
        MapScene,
        DialScene,
    }
}