using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Item
{
    public Sprite Icon { get; }
    public int Gold { get; }

    /// <summary>
    /// ���� ���µ��� ����
    /// </summary>
    public ShopItemType ShopItemType { get; }

    public void SetRandomGold(int start, int end);

    /// <summary>
    /// ���Ž� �ߵ��Ǵ� �Լ�
    /// </summary>
    public virtual void Execute()
    {
        Debug.Log("Parent Func");
    }
}
