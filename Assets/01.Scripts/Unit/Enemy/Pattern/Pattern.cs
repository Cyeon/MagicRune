using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using MyBox;
using UnityEngine.Events;

public class Pattern : MonoBehaviour
{
    [Header("기본정보")]
    public string patternName;
    public string desc = "";
    [TextArea(1, 10)]
    public string PatternDescription = "";
    public List<PatternAction> patternActionDescList = new List<PatternAction>();
    [Tooltip("순환 패턴 목록에 포함되는가?")]
    public bool isIncluding = true;

    [Header("패턴 아이콘")]
    public Sprite icon;
    [SerializeField] private float _iconSize = 1f;
    public Vector2 IconSize => new Vector2(_iconSize, _iconSize);

    [Header("패턴 실행 행동들")]
    public List<PatternAction> startPatternAction;
    public List<PatternAction> turnPatternAction;
    public List<PatternAction> endPatternAction;

    [Header("패턴 조건")]
    public List<PatternTransition> transitions;

    private int _actionIndex = 0;
    private enum patternInvokeTime { start, turn, end};
    private patternInvokeTime _patternTime = patternInvokeTime.start;

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
        desc = description;
        Managers.Enemy.CurrentEnemy.PatternManager.UpdatePatternUI();
    }

    public void DescriptionInit()
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
