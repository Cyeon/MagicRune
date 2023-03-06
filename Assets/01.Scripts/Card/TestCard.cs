using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestCard : MonoBehaviour, IPointerClickHandler
{
    public Dial Dial;
    private DialElement _dialElement;

    [SerializeField]
    private GameObject _outline;

    private void Start()
    {
        _dialElement = GetComponentInParent<DialElement>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 선택 시
        // 1. 가온데임
        //      - 마법 선택되기
        // 2. 가온대 아님
        //      - 가온대로 보내기(각도 조절)

        if (_dialElement.IsHaveCard(this) == false)
        {
            _dialElement.AddSelectCard(this);
        }
    }

    public void SetActiveOutline(bool value)
    {
        _outline.SetActive(value);
    }
}