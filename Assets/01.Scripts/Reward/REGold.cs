using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

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

        Vector2 pos1 = ui.transform.Find("SettingButton").position / 2;
        Vector2 pos2 = new Vector2(ui.transform.Find("SettingButton").position.x, pos1.y);
        Vector2 pos3 = ui.transform.Find("SettingButton").position;
        Vector2 pos4 = ui.transform.Find("Coin_Image").position;

        BezierMissile missle = Managers.Resource.Instantiate("UI/GoldBezier", ui.transform.parent).GetComponent<BezierMissile>();
        missle.transform.localScale = Vector2.one * 1.5f;
        Vector3 rectTrm = missle.GetComponent<RectTransform>().anchoredPosition3D;
        missle.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(rectTrm.x, rectTrm.y, 0);

        Managers.Sound.PlaySound("SFX/GoldMoveSound", SoundType.Effect);
        missle.Init(Vector2.zero, pos2, pos3, pos4, 2f, null, () => Managers.Gold.AddGold(_gold));
    }

    public void SetGold(int gold)
    {
        _gold = gold;
    }
}
