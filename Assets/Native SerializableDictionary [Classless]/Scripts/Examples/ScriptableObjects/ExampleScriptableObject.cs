using System.Linq;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_2018_4_OR_NEWER
using Newtonsoft.Json;
#endif

namespace NativeSerializableDictionary.Examples
{
    /// <summary>
    /// This example showcases how you can combine <seealso cref="UnityEngine.ScriptableObject"/>
    /// with the <seealso cref="NativeSerializableDictionary.SerializableDictionary{K, V}"/> to create a basic database.
    /// </summary>
    [CreateAssetMenu(fileName = "ExampleObject", menuName = "Examples/New ExampleObject")]
    public class ExampleScriptableObject : ScriptableObject
    {

        // the way that this boxing technique works is that it essentially limits
        // the scope of the Vector3 to the inherited List inside the container class.
        // In the instance of Vector3's, it stops JSON from accessing the normalize
        // property which is a recursive callback to a Vector3 itself. If you change
        // the type here to SerializableDictionary (not Boxed), you will see the exact
        // error which this is preventing. I added this so we can have an easy way to
        // serialize problematic structures without needing to create a custom container
        // class yourself. If you want to use only SerializableDictionary and not also
        // the auto-boxing SerializableDictionaryBoxed for more fine-control over how
        // it is done, feel free to modify it, remove it, or implement your own alternative solution.
        [SerializeField]
        private SerializableDictionaryBoxed<Vector3, Color32> boxedValuesExample;
        public SerializableDictionaryBoxed<Vector3, Color32> BoxedValuesExample { get => boxedValuesExample; }

        [SerializeField]
        private SerializableDictionary<string, List<DummyGameItem>> gameItemsExample;
        public SerializableDictionary<string, List<DummyGameItem>> GameItemsExample { get => gameItemsExample; }

#if UNITY_2018_4_OR_NEWER
        // This allows us to click the ... in the inspector window
        // and run the code for the demo provided here.
        [ContextMenu("Test JSON Serialization for Example")]
        private void SerializeExampleDictionary()
        {
            Debug.Log($"<b>Example fully converted to JSON</b>: \n{JsonConvert.SerializeObject(boxedValuesExample)}");
            Debug.Log($"<b>Serializing from Dictionary Key</b>: \n{JsonConvert.SerializeObject(boxedValuesExample[new Vector3(1, 0, 9)])}");
        }
        [ContextMenu("Test JSON Serialization for Game Items Example")]
        private void SerializeGameItemsDictionary()
        {
            Debug.Log($"<b>Game Item Example fully converted to JSON</b>: \n{JsonConvert.SerializeObject(gameItemsExample)}");
            Debug.Log($"<b>Serializing from Dictionary Key</b>: \n{JsonConvert.SerializeObject(gameItemsExample["Melee Weapons"])}");
            Debug.Log($"<b>Retrieving <color=red>Dagger</color> Item Description</b>: \n{gameItemsExample["Melee Weapons"].First(v => v.ItemName == "Dagger").ItemDescription}");
        }
#endif
    }
}
