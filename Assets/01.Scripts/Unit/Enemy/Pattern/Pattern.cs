using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Pattern : MonoBehaviour
{
    public Sprite icon;

    [Header("Actions")]
    public List<PatternAction> startPattern;
    public List<PatternAction> turnPattern;
    public List<PatternAction> endPattern;

    [Header("Transitions")]
    public List<PatternTransition> transitions;

    /// <summary>
    /// �÷��̾� �Ͽ� ���� �ൿ���� �����ٶ� �ߵ��� ���� �׼� ����
    /// </summary>
    public void StartAction()
    {
        UIManager.Instance.ReloadPattern(icon);

        foreach(PatternAction action in startPattern)
        {
            action.TakeAction();
        }
    }

    /// <summary>
    /// ���� ���� ���۵� �� �ߵ��� ���� �׼� ����
    /// </summary>
    public void TurnAction()
    {
        foreach (PatternAction action in turnPattern)
        {
            action.TakeAction();
        }
    }

    /// <summary>
    /// ���� ���� ���� �� �ߵ��� ���� �׼� ����
    /// </summary>
    public void EndAction()
    {
        foreach (PatternAction action in endPattern)
        {
            action.TakeAction();
        }
    }

    public void NextPattern()
    {
        foreach (PatternTransition transition in transitions)
        {
            bool result = false;
            foreach(PatternDecision decision in transition.decisions)
            {
                result = decision.MakeADecision();
                if(!result)
                {
                    BattleManager.Instance.enemy.NextPattern();
                    break;
                }
            }

            if(result)
            {
                if(transition.positivePattern != null)
                {
                    BattleManager.Instance.enemy.ChangePattern(transition.positivePattern);
                }
            }
            else
            {
                BattleManager.Instance.enemy.NextPattern();
            }
        }

    }
}
