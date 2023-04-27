using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonDeck : Deck
{
    protected override void Init()
    {
        for (int i = 0; i < 6; i++)
        {
            AddRune(new MagicShield());
            AddRune(new MagicBullet());
        }
    }
}
