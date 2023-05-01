using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REGold : Reward
{
    private int _gold;

    public override void AddRewardList()
    {
        rewardIcon = Managers.Reward.GetRewardIcon(RewardType.Gold);
        desc = string.Format("{0} 골드", _gold);
        isAuto = true;
        Managers.Reward.AddRewardList(this);
    }
     
    public override void GiveReward()
    {
        UserInfoUI ui = Managers.UI.Get<UserInfoUI>("Upper_Frame");

        Vector3 pos1 = new Vector3(0, 0, 100);
        Vector3 pos2 = new Vector3(ui.transform.Find("SettingButton").position.x, 0, 100);
        Vector3 pos3 = ui.transform.Find("SettingButton").position;
        pos3.z = 100;
        Vector3 pos4 = ui.transform.Find("Coin_Image").position;
        pos4.z = 100;

        BezierMissile missle = Managers.Resource.Instantiate("UI/GoldBezier", ui.transform).GetComponent<BezierMissile>();

        missle.Init(pos1, pos2, pos3, pos4, 1.8f, null, () => Managers.Gold.AddGold(_gold));
    }

    public void SetGold(int gold)
    {
        _gold = gold;
    }
}
