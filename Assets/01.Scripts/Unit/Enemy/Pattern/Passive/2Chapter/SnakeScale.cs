using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeScale : Passive
{
    [SerializeField] private int _increasePercent = 50;
    private int _increasedPerncent = 0;
    public int IncreasePercent => _increasedPerncent;

    public override void Disable()
    {
        Enemy.IsShiledReset = true;
        Enemy.OnTurnEnd.RemoveListener(IncreaseCheck);
        Player.OnGetDamage -= DamageIncrease;
    }

    public override void Init()
    {
        Enemy.IsShiledReset = false;
        Enemy.OnTurnEnd.AddListener(IncreaseCheck);
        Player.OnGetDamage += DamageIncrease;

        passiveDescription = "데미지를 받지 않으면 영구적으로 공격력 50%가 상승합니다.\r\n방어력은 턴이 지나도 사라지지 않습니다.\r\n현재 추가 공격력: " + _increasedPerncent + "%";
    }

    private void IncreaseCheck()
    {
        if(Enemy.attackDamage == 0)
        {
            _increasedPerncent += _increasePercent;
            passiveDescription = "데미지를 받지 않으면 영구적으로 공격력 50%가 상승합니다.\r\n방어력은 턴이 지나도 사라지지 않습니다.\r\n현재 추가 공격력: " + _increasedPerncent + "%";
        }
    }

    private void DamageIncrease()
    {
        Player.attackDamage += Player.attackDamage * (IncreasePercent / 100);
    }
}
