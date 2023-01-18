using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCard : Card
{
    public override void UseAssistEffect()
    {
        GameManager.Instance.player.Attack(1);
    }

    public override void UseMainEffect()
    {
        GameManager.Instance.player.Attack(5);
    }
}
