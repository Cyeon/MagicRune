using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PatternDecision : MonoBehaviour
{
    /// <summary>
    /// transition을 일으킬지 여부결정 함수
    /// </summary>
    /// <returns>transition을 일으킬거면 True, 아니면 False</returns>
    public abstract bool MakeADecision();
}
