using System.Collections.Generic;
using UnityEngine;

namespace NativeSerializableDictionary.Examples
{
    /// <summary>
    /// This example showcases the generic usage of the 
    /// <seealso cref="NativeSerializableDictionary.SerializableDictionary{K, V}"/>
    /// on a GameObject/MonoBehaviour.
    /// </summary>
    public class ExampleMonoBehaviour : MonoBehaviour
    {
        [SerializeField]
        private ExampleScriptableObject exampleSO;

        [SerializeField]
        private SerializableDictionary<string, List<DummyGameItem>> gameItemsExample;
        public SerializableDictionary<string, List<DummyGameItem>> GameItemsExample { get => gameItemsExample; }

        // It is safe to modify the values of the dictionary
        // inside of play mode.
        [SerializeField]
        private SerializableDictionaryBoxed<Vector3, Color32> boxedValuesExample;
        private SerializableDictionaryBoxed<Vector3, Color32> BoxedValuesExample { get => boxedValuesExample; }

        // This is showcasing a simple data-binding whereby we set the Dictionary
        // to the one we have setup in the ScriptableObject. 
        private void Awake()
        {
            if (exampleSO == null)
            {
                Debug.Log($"{GetType().Name}: Please assign the ScriptableObject before running this.");
                return;
            }
            if (exampleSO.GameItemsExample.Count < 1)
            {
                Debug.Log($"{GetType().Name}: The GameItemsExample is empty. Please add at least one entry.");
                return;
            }
            if (exampleSO.BoxedValuesExample.Count < 1)
            {
                Debug.Log($"{GetType().Name}: The BoxedValuesExample is empty. Please add at least one entry.");
                return;
            }
            gameItemsExample = exampleSO.GameItemsExample;
            boxedValuesExample = exampleSO.BoxedValuesExample;
        }
    }
}
