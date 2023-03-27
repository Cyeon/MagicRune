using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private void Awake()
    {
        Managers[] obj = GetComponents<Managers>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
            MapManager.Instance.attackMap.EnterReset();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
