using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugNextStageBtn : MonoBehaviour
{
    private void Start()
    {
        transform.GetComponent<Button>().onClick.AddListener(() =>
        {
            Managers.Map.StageSpawner.ResetPortal();
            Managers.Map.NextStage();
        });
    }
}
