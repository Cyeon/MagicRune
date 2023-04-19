using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resonance : MonoBehaviour
{
    public void Invocation(AttributeType resonanceType)
    {
        Invoke(resonanceType + "Resonance", 0);
    }

    public void FireResonance()
    {
        BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Fire, 5);
    }

    public void IceResonance()
    {
        BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Chilliness, 3);
    }

    public void GroundResonance()
    {
        BattleManager.Instance.Enemy.StatusManager.AddStatus(StatusName.Impact, 5);
    }

    public void ElectricResonance()
    {
        BattleManager.Instance.Player.StatusManager.AddStatus(StatusName.Recharging, 5);
    }
}
