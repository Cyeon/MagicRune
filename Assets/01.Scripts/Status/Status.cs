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
    Fire,                    // 불
    Ice,                     // 얼음
    Recharging,         // 충전
    ChillinessZip,       // 한기응축
    Chilliness,           // 한기
    BladeOfKnife,      // 칼날
    Impact,              // 충격
    IceShield,           // 얼음막
    Web,                  // 거미줄
    FoxOrb,              // 여우구슬
    FlameArmor,         // 화염갑옷
    Boom,                 // 폭발
    Absorptioning,       // 흡수중
    Absorption,           // 흡수
    GroundBeat,         // 땅울림
    Bouncing,            // 척력
    DiamondBody,      // 금강불괴
    SelfGeneration,     // 자가발전
    OverHeat,            // 발열
    Heating,              // 과열
    Penetration,        // 관통
    Ghost,                // 유체화
    Strength,            // 힘
    Annoy,               // 짜증
    PoisonousLiquid, // 독액
    Faint,                 // 기절
    BatUpgrade,        // 방망이 강화
    FlameBody,         // 화염신체
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
    [ConditionalField(nameof(type), false, StatusType.Turn)]
    public bool isFirst = true;

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
        if(isFirst ==  true && type == StatusType.Turn)
        {
            isFirst = false;
        }
        else
        {
            _typeValue = Mathf.Clamp(_typeValue - count, 0, _typeValue);
            if (_typeValue <= 0)
            {
                _unit.StatusManager.DeleteStatus(this);
                return;
            }
        }

        Define.DialScene?.ReloadStatusPanel(_unit, this);
    }
}