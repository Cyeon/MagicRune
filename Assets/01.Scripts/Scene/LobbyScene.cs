using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LobbyScene : BaseScene
{
    [SerializeField] private LoadScene _loadScene;

    protected override void Init()
    {
        SceneType = Define.Scene.LobbyScene;
        Managers.Sound.PlaySound("BGM/LOOP_Battle for Peace", SoundType.Bgm, true);
    }

    public override void Clear()
    {

    }

    public void GameStart()
    {
        Define.SaveData.TotalGold = 0;
        Define.SaveData.KillEnemyAmount = 0;
        Managers.Json.SaveJson<SaveData>("SaveData", Define.SaveData);

        StartCoroutine(_loadScene.LoadSceneCoroutine("MapScene"));
    }
}
