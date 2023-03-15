using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Highlighters
{

    [CustomEditor(typeof(HighlighterTrigger))]
    [CanEditMultipleObjects]
    public class HighlighterTriggerEditor : Editor
    {
        SerializedProperty TriggeringMode;
        SerializedProperty volumeLayerMask;
        SerializedProperty myCamera;
        SerializedProperty maxDistanceFromCamera;
        SerializedProperty drawDebugLine;
        SerializedProperty isCurrentlyTriggeredDebug;

        void OnEnable()
        {
            TriggeringMode = serializedObject.FindProperty("TriggeringMode");
            volumeLayerMask = serializedObject.FindProperty("volumeLayerMask");
            myCamera = serializedObject.FindProperty("myCamera");
            maxDistanceFromCamera = serializedObject.FindProperty("maxDistanceFromCamera");
            drawDebugLine = serializedObject.FindProperty("drawDebugLine");
            isCurrentlyTriggeredDebug = serializedObject.FindProperty("isCurrentlyTriggeredDebug");
        }

        public override void OnInspectorGUI()
        {
            HighlighterTrigger myScript = (HighlighterTrigger)target;
            serializedObject.Update();

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);

                EditorGUILayout.PropertyField(TriggeringMode);

                switch (TriggeringMode.enumValueIndex)
                {
                    case 0: // ObjectEnterVolume
                        EditorGUILayout.PropertyField(volumeLayerMask);

                        break;
                    case 1: // CameraRaycast
                        EditorGUILayout.PropertyField(myCamera);
                        EditorGUILayout.PropertyField(maxDistanceFromCamera);
                        EditorGUILayout.PropertyField(drawDebugLine);


                        break;
                    case 2: // CustomEvents


                        break;
                }

            }

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("Testing", EditorStyles.boldLabel);
                EditorGUILayout.HelpBox("This will work properly only in play mode.", MessageType.Info);

                if (GUILayout.Button("Call Triggering Started"))
                {
                    myScript.TestTriggeringStarted();
                }

                if (GUILayout.Button("Call Triggering Ended"))
                {
                    myScript.TestTriggeringEnded();
                }

                if (GUILayout.Button("Call Trigger Hit"))
                {
                    myScript.TriggerHit();
                }

                EditorGUILayout.PropertyField(isCurrentlyTriggeredDebug);


            }


            serializedObject.ApplyModifiedProperties();

        }
    }
}