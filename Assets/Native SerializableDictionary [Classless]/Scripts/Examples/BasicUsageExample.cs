using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeSerializableDictionary;

/// <summary>
/// This class is intended purely to showcase basic usage examples outside of JSON and other custom data integrations.
/// </summary>
public class BasicUsageExample : MonoBehaviour
{
    [Header("Use the Context Menu to execute each usage test")]
    public bool ThisExistsForTheHeader;

    [ContextMenu("Test Creating Entries")]
    private void CreateItem()
    {
        SerializableDictionary<string, int> test1 = new SerializableDictionary<string, int>();
        //using AddDirect instead of Add allows us to skip needing to create a SerializableKVP container
        test1.Add("test", 0);
        foreach (var kvp in test1)
            Debug.Log(kvp.Key);
        Debug.Log($"<b>SerializableDictionary:</b> <color=orange>[</color> <color=lime>\"{"test"}\"</color> : <color=red>{test1["test"]}</color> <color=orange>]</color>");
        SerializableDictionaryBoxed<string, Vector3> test2 = new SerializableDictionaryBoxed<string, Vector3>();
        //the container is made automatically
        test2.Add("boxed1", new Vector3(4, 2, 0));
        test2.Add("boxed2", new Vector3(16, 0, 9));
        test2.Add("boxed3", new Vector3(0, 8, 3));
        foreach (var kvp in test2)
            Debug.Log($"<b>SerializableDictionaryBoxed:</b> <color=orange>[</color> <color=lime>\"{kvp.Key}\"</color> : <color=red>{kvp.Value}</color> <color=orange>]</color>");
    }

    [ContextMenu("Test TryGetValue")]
    private void TestTryGetValue()
    {
        SerializableDictionary<string, int> test1 = new SerializableDictionary<string, int>();
        test1.Add("test", 99);
        int value = 0;
        bool result = test1.TryGetValue("test", out value);
        Debug.Log($"unboxed correct key result: {result} -> Value: {value}");
        value = 0;
        result = test1.TryGetValue("wrongkey", out value);
        Debug.Log($"unboxed incorrect key result: {result} -> Value: {value}");

        SerializableDictionaryBoxed<string, GameObject> test2 = new SerializableDictionaryBoxed<string, GameObject>();
        test2.Add("test", new GameObject("dummy go"));
	    GameObject go = default;
        result = test2.TryGetValue("test", out go);
        Debug.Log($"boxed correct key result: {result} -> Value: {go.name}");
        DestroyImmediate(go);
        go = default;
        result = test2.TryGetValue("wrongkey", out go);
        Debug.Log($"boxed incorrect key result: {result} -> Value: {go}");
    }

    [ContextMenu("Test GetValue")]
    private void TestGetValue()
    {
        SerializableDictionary<string, int> test1 = new SerializableDictionary<string, int>();
        test1.Add("test", 99);
        int value = 0;
        value = test1["test"];
        Debug.Log($"unboxed correct key result: {value}");
        value = 0;
        //I'm wrapping it in a try/catch so we can allow the test to fully complete
        //without the code stopping on the exception
        try
        {
            value = test1["wrongkey"];
            Debug.Log($"unboxed incorrect key result: {value}");
        }
        catch
        {
	        Debug.Log("A Sequence not found exception is thrown due to the value not existing");
        }

        SerializableDictionaryBoxed<string, GameObject> test2 = new SerializableDictionaryBoxed<string, GameObject>();
        test2.Add("test", new GameObject("dummy go"));
        GameObject go = null;
        go = test2["test"];
        Debug.Log($"boxed correct key result: {go.name}");
        DestroyImmediate(go);
        go = null;
        //Again, I'm wrapping it in a try/catch so we can allow the test to fully complete
        //without the code stopping on the exception
        try
        {
            go = test2["wrongkey"];
            Debug.Log($"boxed incorrect key result: {go}");
        }
        catch
        {
	        Debug.Log("A KeyNotFoundException is thrown due to the value not existing");
        }
    }
}
