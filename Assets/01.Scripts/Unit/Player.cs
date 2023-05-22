using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : Unit
{
    [HideInInspector]
    public Transform relicTrm;

    [SerializeField] private Transform _uiTrm;

    private void Awake()
    {
        if (FindObjectsOfType<Player>().Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);

        relicTrm = transform.Find("Relic");
    }

    public override void Attack(float dmg, bool isTrueDamage)
    {
        base.Attack(dmg);
        BattleManager.Instance.Enemy.TakeDamage(currentDmg, isTrueDamage);
    }

    public void SetUIActive(bool active)
    {
        _uiTrm.gameObject.SetActive(active);
    }
}