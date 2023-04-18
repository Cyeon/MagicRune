using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager
{
    private int _gold = 100;
    public int Gold { get => _gold; private set => _gold = value; }

    private UserInfoUI _userInfoUI;

    public void Init()
    {
        _gold = 12038571;

        Managers.UI.Bind<UserInfoUI>("Upper_Frame", GameObject.FindObjectOfType<DontDestroyOnLoad>().gameObject);
        _userInfoUI = Managers.UI.Get<UserInfoUI>("Upper_Frame");
        _userInfoUI.UpdateGoldText();
    }

    public void AddGold(int amount)
    {
        _gold += amount;
        _userInfoUI.UpdateGoldText();
    }
}
