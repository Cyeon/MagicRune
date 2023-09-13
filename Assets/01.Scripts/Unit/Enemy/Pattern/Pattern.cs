using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using MyBox;
using UnityEngine.Events;

public enum PatternType
{ 
    Attack,
    AtkDef,
    AtkStatus,
    Defence,
    DefStatus,
    Status,
    ETC,
}

public class Pattern : MonoBehaviour
{
    [Header("[ 기본정보 ]")]
    public string patternName;
    [HideInInspector]
    public string patternValue = "";
    [TextArea(1, 10)]
    public string PatternDescription = "";
    public List<PatternAction> patternActionDescList = new List<PatternAction>();
    private List<string> _patternDescList = new List<string>();
    [Tooltip("순환 패턴 목록에 포함되는가?")]
    public bool isIncluding = true;

    [Space, Header("[ 패턴타입 ]")]
    public PatternType patternType = PatternType.Attack;

    // 공격
    [Header("> 공격")]
    public int atkDamage = 0;
    public int atkCount = 1;
    public bool isTrueDmg = false;
    public List<PatternAction> atkDamageApplyList = new List<PatternAction>();

    // 방어
    [Header("> 방어")]
    public int defValue = 0;

    // 상태이상
    [Header("> 상태이상")]
    public StatusName statusName;
    public int statusValue = 0;
    public bool isSelf = false;

    [Space, Header("[ 패턴 아이콘 ]")]
    public Sprite icon;
    [SerializeField] private float _iconSize = 1f;
    public Vector2 IconSize => new Vector2(_iconSize, _iconSize);

    [Space, Header("[ 패턴 실행 행동들 ]")]
    public List<PatternAction> startPatternAction;
    public List<PatternAction> turnPatternAction;
    public List<PatternAction> endPatternAction;

    [Space, Header("[ 패턴 조건 ]")]
    public List<PatternTransition> transitions;

    private int _actionIndex = 0;
    private enum patternInvokeTime { start, turn, end, damage };
    private patternInvokeTime _patternTime = patternInvokeTime.start;

    public void Init()
    {
        Enemy enemy = Managers.Enemy.CurrentEnemy;
        if (patternType == PatternType.Attack || patternType == PatternType.AtkDef || patternType == PatternType.AtkStatus)
        {
            enemy.attackDamage += atkDamage;
            enemy.StatusManager.DamageApply();

            _actionIndex = 0;
            _patternTime = patternInvokeTime.damage;

            if (atkDamageApplyList.Count > _actionIndex)
            {
                atkDamageApplyList[_actionIndex].DamageApplyAction();
            }

            if (atkCount > 1) patternValue = enemy.attackDamage + "x" + atkCount;
            else patternValue = enemy.attackDamage.ToString();

            _patternDescList.Add(Define.DamageDesc(enemy.attackDamage, atkCount));
        }

        if (patternType == PatternType.Defence || patternType == PatternType.AtkDef || patternType == PatternType.DefStatus)
        {
            enemy.shieldValue += defValue;
            enemy.StatusManager.ShieldApply();

            if (patternType == PatternType.AtkDef)
            {
                patternValue += "&" + defValue;
            }
            else patternValue = defValue.ToString();
            _patternDescList.Add("<color=#369AC2>" + enemy.shieldValue + "</color> <color=#F9B41F>방어</color>를 획득");
        }

        if (patternType == PatternType.Status || patternType == PatternType.AtkStatus || patternType == PatternType.DefStatus)
        {
            if (isSelf) _patternDescList.Add(Define.BENEFIC_DESC);
            else _patternDescList.Add(Define.INJURIOUS_DESC);
        }

        DescriptionInit();
    }

    /// <summary>
    /// 플레이어 턴이 시작될떄 발동되는 함수
    /// </summary>
    public void StartAction()
    {
        _actionIndex = 0;
        _patternTime = patternInvokeTime.start;

        if (startPatternAction.Count > _actionIndex)
        {
            startPatternAction[_actionIndex].StartAction();
        }
    }

    /// <summary>
    /// 적 턴이 시작될떄 발동되는 함수
    /// </summary>
    public void TurnAction()
    {
        _actionIndex = 0;
        _patternTime = patternInvokeTime.turn;
        if (turnPatternAction.Count > _actionIndex)
        {
            turnPatternAction[_actionIndex].TurnAction();
        }
        else
        {
            if(BattleManager.Instance.Enemy.PatternManager.IsEffecting == false)
                BattleManager.Instance.TurnChange();
        }
    }

    /// <summary>
    /// 적 턴이 끝날때 발동되는 함수
    /// </summary>
    public void EndAction()
    {
        _actionIndex = 0;
        _patternTime = patternInvokeTime.end;
        if (endPatternAction.Count > _actionIndex)
        {
            endPatternAction[_actionIndex].EndAction();
        }
    }

    /// <summary>
    /// 다음 액션으로 넘어가는 함수
    /// </summary>
    public void NextAction()
    {
        _actionIndex++;

        List<PatternAction> actions = new List<PatternAction>();
        switch (_patternTime)
        {
            case patternInvokeTime.start:
                actions = startPatternAction;
                if (actions.Count > _actionIndex)
                {
                    actions[_actionIndex].StartAction();
                }
                break;

            case patternInvokeTime.turn:
                actions = turnPatternAction;
                if (actions.Count <= _actionIndex)
                {
                    BattleManager.Instance.Enemy.PatternManager.isPatternActioning = false;
                    BattleManager.Instance.Enemy.PatternManager.PatternEnd();
                    return;
                }
                actions[_actionIndex].TurnAction();
                break;

            case patternInvokeTime.end:
                actions = endPatternAction;
                if (actions.Count > _actionIndex)
                {
                    actions[_actionIndex].EndAction();
                }
                break;

            case patternInvokeTime.damage:
                actions = atkDamageApplyList;
                if (actions.Count > _actionIndex)
                {
                    actions[_actionIndex].DamageApplyAction();
                }
                break;
        }
    }

    /// <summary>
    /// 다음 순서에 있는 패턴으로 변경
    /// </summary>
    public void NextPattern()
    {
        foreach (PatternTransition transition in transitions)
        {
            bool result = false;
            foreach (PatternDecision decision in transition.decisions)
            {
                result = decision.MakeADecision();
                if (!result) break;
            }

            if (result)
            {
                if (transition.positivePattern != null)
                {
                    BattleManager.Instance.Enemy.PatternManager.ChangePattern(transition.positivePattern);
                    return;
                }
            }
        }

        BattleManager.Instance.Enemy.PatternManager.NextPattern();
    }

    public void ChangePatternValue(string description)
    {
        patternValue = description;
        Managers.Enemy.CurrentEnemy.PatternManager.UpdatePatternUI();
    }

    private void DescriptionInit()
    {
        

        if(patternActionDescList.Count > 0)
        {
            string desc = "";
            for(int i = 0; i < patternActionDescList.Count; i++)
            {
                desc += patternActionDescList[i].Description;
                if(i + 1 == patternActionDescList.Count)
                {
                    desc += " 하려고 합니다.";
                }
                else
                {
                    desc += " 후, ";
                }
            }

            PatternDescription = desc;
        }
    }

#if UNITY_EDITOR
    [ButtonMethod]
    private void IconApply()
    {
        transform.parent.parent.Find("UI/Pattern/PatternIcon").GetComponent<SpriteRenderer>().sprite = icon;
        transform.parent.parent.Find("UI/Pattern/PatternIcon").transform.localScale = IconSize;
    }

    [ButtonMethod]
    private void IconReset()
    {
        transform.parent.parent.Find("UI/Pattern/PatternIcon").GetComponent<SpriteRenderer>().sprite = null;
    }
#endif
}
