using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfoPanelCanvas : MonoBehaviour
{
    private void Start()
    {
        if (FindObjectsOfType<Player>().Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }
}
