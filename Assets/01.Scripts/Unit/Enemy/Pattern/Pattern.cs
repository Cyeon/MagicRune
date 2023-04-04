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
    public string desc ="";
    public bool isIncluding = true;

    [Header("[ Actions ]")]
    public List<PatternAction> startPattern;
    public List<PatternAction> turnPattern;
    public List<PatternAction> endPattern;

    [Header("[ Transition ]")]
    public List<PatternTransition> transitions;

    private int _patternIndex = 0;
    private enum patternInvokeTime { start, turn, end};
    private patternInvokeTime _patternTime = patternInvokeTime.start;

    /// <summary>
    /// 플레이어 턴에 무슨 행동할지 보여줄때 발동될 패턴 액션 실행
    /// </summary>
    public void StartAction()
    {
        _patternIndex = 0;
        _patternTime = patternInvokeTime.start;
        if(startPattern.Count > _patternIndex)
        {
            startPattern[_patternIndex].TakeAction();
        }
    }

    /// <summary>
    /// 몬스터 턴이 시작될 때 발동될 패턴 액션 실행
    /// </summary>
    public void TurnAction()
    {
        _patternIndex = 0;
        _patternTime = patternInvokeTime.turn;
        if (turnPattern.Count > _patternIndex)
        {
            turnPattern[_patternIndex].TakeAction();
        }
    }

    /// <summary>
    /// 몬스터 턴이 끝날 때 발동될 패턴 액션 실행
    /// </summary>
    public void EndAction()
    {
        _patternIndex = 0;
        _patternTime = patternInvokeTime.end;
        if (endPattern.Count > _patternIndex)
        {
            endPattern[_patternIndex].TakeAction();
        }
    }

    public void NextAction()
    {
        _patternIndex++;

        List<PatternAction> actions = new List<PatternAction>();
        switch (_patternTime)
        {
            case patternInvokeTime.start:
                actions = startPattern;
                break;

            case patternInvokeTime.turn:
                actions = turnPattern;
                break;

            case patternInvokeTime.end:
                actions = endPattern;
                break;
        }

        if(actions.Count <= _patternIndex)
        {
            BattleManager.Instance.TurnChange();
            return;
        }

        actions[_patternIndex].TakeAction();
    }

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

            if(result)
            {
                if(transition.positivePattern != null)
                {
                    BattleManager.Instance.enemy.patternM.ChangePattern(transition.positivePattern);

                }
            }
            else
            {
                BattleManager.Instance.enemy.patternM.NextPattern();
            }
        }
    }
}
