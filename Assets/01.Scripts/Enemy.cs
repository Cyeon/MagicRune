using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Image hpImage;

    public int HP;
    public int MaxHp = 100;

    public Text text;

    public void Damage(int damage)
    {
        HP -= damage;
        hpImage.fillAmount = (float)HP / MaxHp;
        text.text = $"{HP} / {MaxHp}";
    }
}
