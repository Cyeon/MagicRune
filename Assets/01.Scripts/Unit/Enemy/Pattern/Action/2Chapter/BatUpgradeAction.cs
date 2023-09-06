using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatUpgradeAction : PatternAction
{
    [SerializeField] private int _removeHP = 10;

    public override string Description => "본인의 체력을 <color=#369AC2> " + _removeHP + "</color> 감소하고 " + Define.BENEFIC_DESC;

    public override void TurnAction()
    {
        if(Enemy.HP + Enemy.StatusManager.GetStatusValue(StatusName.Fire) > 10 
            && Enemy.StatusManager.GetStatusValue(StatusName.BatUpgrade) <= 5)
        {
            Enemy.StatusManager.AddStatus(StatusName.BatUpgrade, 1);
            Enemy.RemTrueHP(_removeHP);
        }

        base.TurnAction();
    }
}
