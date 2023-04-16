using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Item
{
    public Sprite Icon { get; }
    public int Gold { get; }

    /// <summary>
    /// 딱히 쓰는데는 없음
    /// </summary>
    public ShopItemType ShopItemType { get; }

    public void SetRandomGold(int start, int end);

    /// <summary>
    /// 구매시 발동되는 함수
    /// </summary>
    public virtual void Execute()
    {
        Debug.Log("Parent Func");
    }
}
