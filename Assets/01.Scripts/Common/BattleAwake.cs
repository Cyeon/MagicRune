using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAwake : MonoBehaviour
{
    private void Awake()
    {
        gameObject.hideFlags = HideFlags.HideAndDontSave;
    }
    private void Start()
    {
        GameManager.Instance.OnPlayerTurn();
    }
}