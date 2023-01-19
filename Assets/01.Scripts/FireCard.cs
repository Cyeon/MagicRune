using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCard : Card
{
    public override void UseAssistEffect()
    {
        Debug.Log("1 damage");
    }

    public override void UseMainEffect()
    {
        Debug.Log("5 damage");
    }
}
