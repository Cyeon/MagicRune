using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SETakeDamage : StatusEvent
{
    private enum DamageType
    {
        Value,
        StackDmg
    }

    [SerializeField] private DamageType _damageType = DamageType.Value;

    [SerializeField, ConditionalField(nameof(_damageType), false, DamageType.Value)]
    protected int _damage;
    [SerializeField]
    private bool _isTrueDamage = false;

    public override void Invoke()
    {
        if(_damageType == DamageType.StackDmg)
        {
            _damage = _status.TypeValue;
        }
        Managers.Sound.PlaySound(_status.activeSound, SoundType.Effect);
        _unit.TakeDamage(_damage, _isTrueDamage, _status);
    }
}
