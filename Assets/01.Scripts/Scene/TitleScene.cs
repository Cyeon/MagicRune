using UnityEngine;

public class TitleScene : BaseScene
{
    [SerializeField]
    private LoadScene _loadScene;

    public override void Clear()
    {

    }

    public void GameStart()
    {
        Managers.Clear();
        StartCoroutine(_loadScene.LoadSceneCoroutine("LobbyScene"));
    }
}
