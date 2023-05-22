using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LobbyScene : BaseScene
{


    protected override void Init()
    {
        Managers.Sound.PlaySound("BGM/LOOP_Battle for Peace", SoundType.Bgm, true);
    }

    public override void Clear()
    {

    }
}
