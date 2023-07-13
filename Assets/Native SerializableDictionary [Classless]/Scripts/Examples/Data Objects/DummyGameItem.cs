using System;
using UnityEngine;
#if UNITY_2018_4_OR_NEWER
using Newtonsoft.Json;
#endif

namespace NativeSerializableDictionary.Examples
{
    /// <summary>
    /// This is a dummy class used as a container for data which can be
    /// viewed and modified inside of the Unity Inspector Window (Editor).
    /// </summary>
    [Serializable]
    public class DummyGameItem
    {
        // This allows us to multi-select
        // enum values in the inspector
        // They must be power of 2
        [Flags]
        public enum ItemTypes
        {
            Blunt = 1,
            Piercing = 4,
            Slashing = 8,
        }

        [Header("Item UI")]
        [SerializeField]
        private string itemName;
        public string ItemName { get => itemName; }
        [SerializeField]
        #if UNITY_2018_4_OR_NEWER
        [JsonIgnore]
        #endif
        private Sprite itemImage;
        #if UNITY_2018_4_OR_NEWER
        [JsonIgnore]
        #endif
        public Sprite ItemImage { get => itemImage; }
        [SerializeField]
        [TextArea(1, 10)]
        private string itemDescription;
        public string ItemDescription { get => itemDescription; }

        [Space(6)]
        [Header("Item Stats")]
        [SerializeField]
        private int cost;
        public int Cost { get => cost; }
        [SerializeField]
        private int durability;
        public int Durability { get => durability; }
        [SerializeField]
        private int baseDamage;
        public int BaseDamage { get => baseDamage; }
        [SerializeField]
        [Range(0, 100)]
        private int dropChance;
        public int DropChance { get => dropChance; }
        [SerializeField]
        private AnimationCurve statsRollRange;
        public AnimationCurve StatsRollRange { get => statsRollRange; }

        [Space(6)]
        [Header("Item Configuration")]
        [SerializeField]
        private ItemTypes itemType;
        public ItemTypes ItemType { get => itemType; }
        [SerializeField]
        private AnimationCurve critMultiplier;
        public AnimationCurve CritMultiplier { get => critMultiplier; }
        [SerializeField]
        private AnimationCurve swingPattern;
        public AnimationCurve SwingPattern { get => swingPattern; }
    }
}
