using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/AllRuneList")]
public class AllRuneListSO : ScriptableObject
{
    public List<RuneSO> RuneList;

    public List<BaseRune> BaseRuneList;
}
