using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceDeck : Deck
{
    protected override void Init()
    {
        for (int i = 0; i < 3; i++)
        {
            AddRune(new MagicShield());
            AddRune(new MagicBullet());
        }

        for (int i = 0; i < 2; i++)
        {
            AddRune(new Ice());
            AddRune(new SnowBall());
        }

        AddRune(new IceShield());
        AddRune(new IceSmash());
    }
}
