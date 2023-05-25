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
    Boss
}

public class Stage : MonoBehaviour
{
    public StageType type;

    /// <summary>
    /// 스테이지에 들어갈 때 발동되는 함수
    /// </summary>
    public virtual void InStage()
    {
        Managers.Map.CurrentPeriodStageList.Remove(type);
        Managers.Canvas.GetCanvas("CompousProgress").enabled = false;
    }

    public virtual void Init()
    {

    }
}

