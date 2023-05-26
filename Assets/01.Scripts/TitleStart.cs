using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleStart : MonoBehaviour
{
    public void GameStart()
    {
        Managers.Scene.LoadScene("LobbyScene");
    }
}
