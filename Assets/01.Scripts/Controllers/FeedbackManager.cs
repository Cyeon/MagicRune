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
        BattleManager.Instance.enemy.OnTakeDamageFeedback.AddListener(() => _enemyAttackFeedback.PlayFeedbacks());
        _enemyAttackFeedback.Feedbacks[0].GetComponent<MMFeedbackPosition>().AnimatePositionTarget = BattleManager.Instance.enemy.gameObject;
        _enemyAttackFeedback.Feedbacks[2].GetComponent<MMFeedbackSpriteRenderer>().BoundSpriteRenderer = BattleManager.Instance.enemy.SpriteRenderer;
    }
}
