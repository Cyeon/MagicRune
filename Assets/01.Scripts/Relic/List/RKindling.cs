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
            player.TakeDamage(player.StatusManager.GetStatus(StatusName.Fire).TypeValue, true);
            player.StatusManager.DeleteStatus(StatusName.Fire);
        }
    }
}
