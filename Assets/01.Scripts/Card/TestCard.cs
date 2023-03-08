using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestCard : MonoBehaviour
{
    public Dial Dial;
    private DialElement _dialElement;

    [SerializeField]
    private CardSO _magic;
    [SerializeField]
    private GameObject _outline;

    private void Start()
    {
        _dialElement = GetComponentInParent<DialElement>();
    }

    public void SetActiveOutline(bool value)
    {
        _outline.SetActive(value);
    }
}