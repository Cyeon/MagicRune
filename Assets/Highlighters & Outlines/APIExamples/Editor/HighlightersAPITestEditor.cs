using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

namespace Highlighters
{
    [CustomEditor(typeof(HighlightersAPITest))]
    public class HighlightersAPITestEditor : Editor
    {
        //SerializedProperty id;

        void OnEnable()
        {
            //id = serializedObject.FindProperty("id");
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            HighlightersAPITest myScript = (HighlightersAPITest)target;
            if (GUILayout.Button("Add highlighter script"))
            {
                myScript.TestAddingScripts();
            }
            if (GUILayout.Button("Enable | Disable"))
            {
                myScript.TestEnablingDisabling();
            }
            if (GUILayout.Button("Test changing highlighter settings"))
            {
                myScript.TestSettings();
            }
        }
    }
}