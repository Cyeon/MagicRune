using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Keyward
{
    public string KeywardName;
    public StatusName StatusName;
    public string KeywardDescription;
}

[CreateAssetMenu(menuName = "SO/Keyward/KeywardList")]
public class KeywardListSO : ScriptableObject
{
    public List<Keyward> KeywardList;
}
