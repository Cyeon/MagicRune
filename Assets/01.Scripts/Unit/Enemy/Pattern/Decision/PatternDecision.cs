using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PatternDecision : MonoBehaviour
{
    /// <summary>
    /// transition�� ����ų�� ���ΰ��� �Լ�
    /// </summary>
    /// <returns>transition�� ����ų�Ÿ� True, �ƴϸ� False</returns>
    public abstract bool MakeADecision();
}
