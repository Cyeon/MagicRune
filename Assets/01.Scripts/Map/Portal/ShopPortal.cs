using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPortal : Portal
{
    public override void Execute()
    {
        MapManager.Instance.NextStage();
    }

    public override void Init()
    {

    }

    
}
