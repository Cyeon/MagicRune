using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOKill : MonoBehaviour
{
    private void OnDestroy()
    {
        transform.DOKill();
    }
}
