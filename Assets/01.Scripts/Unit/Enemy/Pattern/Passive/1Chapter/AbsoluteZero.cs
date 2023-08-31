using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsoluteZero : Passive
{
    public override void Disable()
    {
        Enemy.IsShiledReset = true;
    }

    public override void Init()
    {
        Enemy.IsShiledReset = false;
    }
}
