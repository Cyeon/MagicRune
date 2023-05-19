using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeywardType
{
    None,
    Charge,
    Fire,
    Impact,
    Chilliness,
    Ice,
    consume,
    penetration
}

[Serializable]
public class Keyward
{
    public string KeywardName;
    public KeywardType TypeName;
    public string KeywardDescription;

    public Keyward()
    {
        KeywardName = "";
        TypeName = KeywardType.None;
        KeywardDescription = "";
    }
}

[CreateAssetMenu(menuName = "SO/Keyward/KeywardList")]
public class KeywardListSO : ScriptableObject
{
    public List<Keyward> KeywardList;
}
