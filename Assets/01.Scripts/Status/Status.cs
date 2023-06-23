using MoreMountains.FeedbacksForThirdParty;
using MyBox;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum StatusSoundType
{
    None,
    Main,
    Positive,
    Negative
}

public enum StatusType
{
    Stack,
    Turn
}

public enum StatusName
{
    Null,
    Fire,               // ?붿긽
    Ice,                // 鍮숆껐
    Recharging,         // 異⑹쟾
    ChillinessZip,      // ?쒓린?묒텞
    Chilliness,         // ?쒓린
    BladeOfKnife,       // 移쇰궇
    Impact,             // 異⑷꺽
    IceShield,          // ?쇱쓬 蹂댄샇留?
    PoisonousLiquid,    // ?낆븸
    FoxOrb,             // ?ъ슦援ъ뒳
    FlameArmor,         // ?붿뿼媛묒샆
    Boom,               // ??컻
    Absorptioning,      // ?≪닔 以?
    Absorption,         // ?≪닔
    GroundBeat,         // ?낆슱由?
    Bouncing,           // 泥숇젰
    DiamondBody,     // 
    SelfGeneration,   // 자가발전
    OverHeat,          // 
    Heating,             // 
    Penetration,       // 
    Ghost,               // 유체화
    Strength,           // 힘
    COUNT
}

public class Status : MonoBehaviour
{
    public string debugName = "";
    public StatusName statusName = StatusName.Null;
    [TextArea(1, 5)]
    public string information = "";
    public Color textColor = Color.white;

    [Header("Type")]
    public StatusType type = StatusType.Stack;
    private int _typeValue = 0; 
    public int TypeValue => _typeValue;
    [ConditionalField(nameof(type), false, StatusType.Stack)]
    public bool isTurnRemove = false;

    [Header("Function")]
    public List<StatusEvent> OnAddStatus = new List<StatusEvent>();
    public List<StatusEvent> OnTurnStart = new List<StatusEvent>();
    public List<StatusEvent> OnAttack = new List<StatusEvent>();
    public List<StatusEvent> OnGetDamage = new List<StatusEvent>();
    public List<StatusEvent> OnTurnEnd = new List<StatusEvent>();
    public List<StatusEvent> OnRemoveStatus = new List<StatusEvent>();

    [Header("Resource")]
    [ShowAssetPreview(32, 32)] public Sprite icon;
    public Color color = Color.white;
    public StatusSoundType statusSoundType = StatusSoundType.None;
    [ConditionalField(nameof(statusSoundType), false, StatusSoundType.Main)]
    public AudioClip getSound = null;
    [ConditionalField(nameof(statusSoundType), false, StatusSoundType.Main)]
    public AudioClip activeSound = null;
    
    private Unit _unit;

    public void Init(Unit unit)
    {
        _unit = unit;
        _typeValue = 0;
        Define.DialScene?.AddStatus(_unit, this);
    }

    public void AddValue(int count)
    {
        _typeValue += count;
        Define.DialScene?.ReloadStatusPanel(_unit, this);
    }

    public void RemoveValue(int count)
    {
        _typeValue = Mathf.Clamp(_typeValue - count, 0, _typeValue);
        if (_typeValue <= 0)
        {
            _unit.StatusManager.DeleteStatus(this);
            return;
        }

        Define.DialScene?.ReloadStatusPanel(_unit, this);
    }
}