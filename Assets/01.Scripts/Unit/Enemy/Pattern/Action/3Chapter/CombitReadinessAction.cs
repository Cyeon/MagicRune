using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombitReadinessAction : PatternAction
{
    public int absorbDamage = 0;

    public override void StartAction()
    {
        Enemy.OnTakeDamage.AddListener(AbsorbDamage);
        base.StartAction();
    }

    public override void EndAction()
    {
        Enemy.OnTakeDamage.RemoveListener(AbsorbDamage);
        base.EndAction();
    }

    private void AbsorbDamage(float damage)
    {
        absorbDamage += damage.RoundToInt();
    }
}
