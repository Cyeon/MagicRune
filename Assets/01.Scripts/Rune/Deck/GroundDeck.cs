using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDeck : Deck
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
            AddRune(new Attack());
            AddRune(new GroundShield());
        }

        AddRune(new ShieldAttack());
        AddRune(new ThreeAttack());
    }
}
