using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Pattern : MonoBehaviour
{
    public string patternName;
    public Sprite icon;
    public Vector3 iconSize = Vector3.one;
    public string desc ="";
    public bool isIncluding = true; // 순화되는 패턴 목록에 포함할건가?

    [Header("[ Actions ]")]
    public List<PatternAction> startPattern;
    public List<PatternAction> turnPattern;
    public List<PatternAction> endPattern;

    [Header("[ Transition ]")]
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
        if(startPattern.Count > _actionIndex)
        {
            startPattern[_actionIndex].StartAction();
        }
    }

    /// <summary>
    /// 적 턴이 시작될떄 발동되는 함수
    /// </summary>
    public void TurnAction()
    {
        _actionIndex = 0;
        _patternTime = patternInvokeTime.turn;
        if (turnPattern.Count > _actionIndex)
        {
            turnPattern[_actionIndex].TurnAction();
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
        if (endPattern.Count > _actionIndex)
        {
            endPattern[_actionIndex].EndAction();
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
                actions = startPattern;
                if (actions.Count > _actionIndex)
                {
                    actions[_actionIndex].StartAction();
                }
                break;

            case patternInvokeTime.turn:
                actions = turnPattern;
                if (actions.Count <= _actionIndex)
                {
                    BattleManager.Instance.Enemy.PatternManager.isPatternActioning = false;
                    BattleManager.Instance.Enemy.PatternManager.PatternEnd();
                    return;
                }
                actions[_actionIndex].TurnAction();
                break;

            case patternInvokeTime.end:
                actions = endPattern;
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
            foreach(PatternDecision decision in transition.decisions)
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
}
