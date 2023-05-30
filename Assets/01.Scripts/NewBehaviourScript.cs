using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public void Die()
    {
        BattleManager.Instance.Enemy.Die();
    }
}
