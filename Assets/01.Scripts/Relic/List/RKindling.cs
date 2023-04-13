using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RKindling : Relic, IContinuousHandler
{
    public void Execute()
    {
        Player player = Managers.GetPlayer();
        Status status = player.StatusManager.GetStatus(StatusName.Fire);
        if(status != null)
        {
            player.TakeDamage(player.StatusManager.GetStatus(StatusName.Fire).TypeValue * 2, true);
            player.StatusManager.DeleteStatus(StatusName.Fire);
        }
    }

    public override void OnAdd()
    {
        EventManager.StartListening(Define.ON_END_PLAYER_TURN, () => { Execute(); });
    }

    public override void OnRemove()
    {
        EventManager.StopListening(Define.ON_END_PLAYER_TURN, () => { Execute(); });
    }
}
