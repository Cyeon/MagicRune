using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using Unity.VisualScripting;

public class FeedbackManager : MonoSingleton<FeedbackManager>
{
    [SerializeField] MMFeedbacks _enemyAttackFeedback;

    public void Init()
    {
        BattleManager.Instance.Enemy.OnTakeDamageFeedback.AddListener(() => _enemyAttackFeedback.PlayFeedbacks());
        _enemyAttackFeedback.Feedbacks[0].GetComponent<MMFeedbackPosition>().AnimatePositionTarget = BattleManager.Instance.Enemy.gameObject;
        _enemyAttackFeedback.Feedbacks[2].GetComponent<MMFeedbackSpriteRenderer>().BoundSpriteRenderer = BattleManager.Instance.Enemy.spriteRenderer;
    }
}
