using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RelicType
{
    Use,
    Continuous
}

public enum RelicName
{
    None,
    HealingBox,
    Kindling,
}

public abstract class Relic : MonoBehaviour
{
    public string debugName;
    public RelicName relicName = RelicName.None;
    [TextArea(1, 10)] public string desc;
    [ShowAssetPreview(32, 32)] public Sprite icon;
    public RelicType relicType;

    public abstract void OnAdd();
    public abstract void OnRemove();
}
