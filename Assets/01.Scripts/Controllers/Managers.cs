using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
