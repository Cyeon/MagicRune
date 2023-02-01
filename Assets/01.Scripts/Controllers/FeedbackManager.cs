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
        GameManager.Instance.enemy.OnTakeDamageFeedback.AddListener(() => _enemyAttackFeedback.PlayFeedbacks());
    }
}
