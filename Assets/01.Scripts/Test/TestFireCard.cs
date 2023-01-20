using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFireCard : TestCard
{
    public override void UseAssistEffect()
    {
        Debug.Log(1);
    }

    public override void UseMainEffect()
    {
        Debug.Log(5);
    }
}
