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
    HealingBox,
    Kinding
}

public class Relic : MonoBehaviour
{
    public string debugName;
    public RelicName relicName;
    [TextArea(1, 10)] public string desc;
    [ShowAssetPreview(32, 32)] public Sprite icon;
    public RelicType relicType;
}
