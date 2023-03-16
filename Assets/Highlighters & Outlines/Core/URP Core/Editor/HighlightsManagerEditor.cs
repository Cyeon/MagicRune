using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

namespace Highlighters
{
    [CustomEditor(typeof(HighlightsManager))]
    public class HighlightsManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }
    }
}