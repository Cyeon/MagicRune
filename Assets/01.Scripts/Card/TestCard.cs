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
        // ���� ��
        // 1. ���µ���
        //      - ���� ���õǱ�
        // 2. ���´� �ƴ�
        //      - ���´�� ������(���� ����)
        //if(_d)


        if (_dialElement.IsHaveCard(this) == false)
        {
            _dialElement.AddSelectCard(this);
        }
        else
        {
            _dialElement.RemoveSelectCard(this);
        }
    }

    public void SetActiveOutline(bool value)
    {
        _outline.SetActive(value);
    }
}