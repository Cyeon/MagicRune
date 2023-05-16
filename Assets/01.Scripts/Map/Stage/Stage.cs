    using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum StageType
{
    Attack,
    Adventure,
    Rest,
    Shop,
}

public class Stage : MonoBehaviour
{
    public StageType type;

    /// <summary>
    /// 스테이지에 들어갈 때 발동되는 함수
    /// </summary>
    public virtual void InStage()
    {

    }

    public virtual void Init()
    {

    }
}

