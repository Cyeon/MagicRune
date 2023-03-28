using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public Player player;

    private int _gold = 100;
    public int Gold { get => _gold; set => _gold = value; }

    private bool _preparedToQuit = false;

    private void Awake()
    {
        Application.targetFrameRate = 30;
        DOTween.Init(false, false, LogBehaviour.Default).SetCapacity(500, 50);
    }

    public void AddGold(int amount)
    {
        _gold += amount;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(_preparedToQuit == false)
            {
                AndroidToast.Instance.ShowToastMessage("뒤로가기 버튼을 한 번 더 누르시면 종료합니다.");
                PreparedToQuit();
            }
            else
            {
                GameQuit();
            }
        }
    }

    private void PreparedToQuit()
    {
        StartCoroutine(PreparedToQuitCoroutine());
    }

    private IEnumerator PreparedToQuitCoroutine()
    {
        _preparedToQuit = true;
        yield return new WaitForSecondsRealtime(2.5f);
        _preparedToQuit = false;
    }

    public void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
