using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        Managers.Relic.AddRelic("HealingBox");
        Managers.Relic.AddRelic("Kindling");
    }
}
