using UnityEngine;
using UnityEditor;

namespace Highlighters
{
    [CanEditMultipleObjects]
    [CustomPropertyDrawer(typeof(HighlighterSettings))]
    public class HighlighterSettingsDrawerUIE : PropertyDrawer
    {
        // General
        SerializedProperty depthMask;
        SerializedProperty depthLayerMask;
        SerializedProperty infoRenderScale;

        // Outer Glow
        SerializedProperty useOuterGlow;
        SerializedProperty useSingleOuterGlow;
        SerializedProperty outlineVisibleBeforeObject;
        SerializedProperty outerGlowColorFront;
        SerializedProperty outerGlowColorBack;
        SerializedProperty blurIterations;
        SerializedProperty blurRenderScale;
        SerializedProperty blurIntensity;
        SerializedProperty blurIntensityMultiplier;
        SerializedProperty blurAdaptiveThickness;
        SerializedProperty cameraDistanceMultiplier;

        // Blurs
        SerializedProperty blurType;
        SerializedProperty blendType;
        SerializedProperty boxBlurSize;


        // Mesh outlines

        SerializedProperty useMeshOutline;
        SerializedProperty useSingleMeshOutline;
        SerializedProperty meshOutlineFront;
        SerializedProperty meshOutlineBack;
        SerializedProperty meshOutlineThickness;
        SerializedProperty meshOutlineAdaptiveThickness;

        //Inner Glow
        SerializedProperty useInnerGlow;
        SerializedProperty useSingleInnerGlow;
        SerializedProperty innerGlowFront;
        SerializedProperty innerGlowBack;

        //Overlays
        SerializedProperty useOverlay;
        SerializedProperty useSingleOverlay;

        SerializedProperty overlayFront;
        SerializedProperty overlayBack;

        GUIStyle centeredBoldLabel = new GUIStyle()
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            fontSize = 12,
        };

        public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label)
        {
            centeredBoldLabel.normal.textColor = Color.white;

            // Main
            depthMask = property.FindPropertyRelative("depthMask");
            depthLayerMask = property.FindPropertyRelative("depthLayerMask");
            infoRenderScale = property.FindPropertyRelative("infoRenderScale");

            // Outer Glow
            useOuterGlow = property.FindPropertyRelative("useOuterGlow");
            useSingleOuterGlow = property.FindPropertyRelative("useSingleOuterGlow");
            outlineVisibleBeforeObject = property.FindPropertyRelative("outlineVisibleBeforeObject");
            outerGlowColorFront = property.FindPropertyRelative("outerGlowColorFront");
            outerGlowColorBack = property.FindPropertyRelative("outerGlowColorBack");
            blurIterations = property.FindPropertyRelative("blurIterations");
            blurRenderScale = property.FindPropertyRelative("blurRenderScale");
            blurIntensity = property.FindPropertyRelative("blurIntensity");
            blurIntensityMultiplier = property.FindPropertyRelative("blurIntensityMultiplier");
            blurAdaptiveThickness = property.FindPropertyRelative("blurAdaptiveThickness");
            cameraDistanceMultiplier = property.FindPropertyRelative("cameraDistanceMultiplier");

            // Blurs
            blurType = property.FindPropertyRelative("blurType");
            blendType = property.FindPropertyRelative("blendType");
            boxBlurSize = property.FindPropertyRelative("boxBlurSize");

            // Mesh Outline
            useMeshOutline = property.FindPropertyRelative("useMeshOutline");
            useSingleMeshOutline = property.FindPropertyRelative("useSingleMeshOutline");
            meshOutlineFront = property.FindPropertyRelative("meshOutlineFront");
            meshOutlineBack = property.FindPropertyRelative("meshOutlineBack");
            meshOutlineThickness = property.FindPropertyRelative("meshOutlineThickness");
            meshOutlineAdaptiveThickness = property.FindPropertyRelative("meshOutlineAdaptiveThickness");


            //Inner Glow
            useInnerGlow = property.FindPropertyRelative("useInnerGlow");
            useSingleInnerGlow = property.FindPropertyRelative("useSingleInnerGlow");
            innerGlowFront = property.FindPropertyRelative("innerGlowFront");
            innerGlowBack = property.FindPropertyRelative("innerGlowBack");

            //Overlays
            useOverlay = property.FindPropertyRelative("useOverlay");
            useSingleOverlay = property.FindPropertyRelative("useSingleOverlay");
            overlayFront = property.FindPropertyRelative("overlayFront");
            overlayBack = property.FindPropertyRelative("overlayBack");


            // General Settings

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField("General Settings", EditorStyles.boldLabel);

                //EditorGUI.indentLevel = 1;
                EditorGUILayout.PropertyField(depthMask, new GUIContent("Masking"));

                if (depthMask.enumValueIndex != 3)
                {
                    //EditorGUILayout.PropertyField(depthLayerMask, new GUIContent("Depth Layer Mask"));
                }
                EditorGUILayout.PropertyField(infoRenderScale);

                //EditorGUI.indentLevel = 0;
            }


            // Outer Glow

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Outer Glow", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(useOuterGlow, new GUIContent(""));
                }

                if (useOuterGlow.boolValue)
                {
                    if (depthMask.enumValueIndex == 2) // Both
                    {
                        EditorGUILayout.PropertyField(useSingleOuterGlow);
                        if (useSingleOuterGlow.boolValue)
                        {
                            EditorGUILayout.PropertyField(outerGlowColorFront);
                        }
                        else
                        {

                            EditorGUILayout.PropertyField(outerGlowColorFront);
                            EditorGUILayout.PropertyField(outerGlowColorBack);
                        }
                    }
                    else if(depthMask.enumValueIndex == 0 || depthMask.enumValueIndex == 1) // Front or Back
                    {
                        EditorGUILayout.PropertyField(outerGlowColorFront);
                        EditorGUILayout.PropertyField(outlineVisibleBeforeObject);
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(outerGlowColorFront);
                    }

                    EditorGUILayout.PropertyField(blendType);
                    EditorGUILayout.PropertyField(blurType);
                    switch (blurType.enumValueIndex)
                    {
                        case 0: // Gaussian
                            //EditorGUILayout.PropertyField(blurIterations);

                            break;
                        case 1: // Box 
                            EditorGUILayout.PropertyField(boxBlurSize);

                            break;
                    }

                    EditorGUILayout.PropertyField(blurRenderScale);
                    EditorGUILayout.PropertyField(blurIterations);
                    EditorGUILayout.PropertyField(blurIntensity);
                    EditorGUILayout.PropertyField(blurIntensityMultiplier);
                    EditorGUILayout.PropertyField(blurAdaptiveThickness);
                    EditorGUILayout.PropertyField(cameraDistanceMultiplier);
                }
            }

            // Mesh Outline

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Mesh Outline", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(useMeshOutline, new GUIContent(""));
                }
                if (useMeshOutline.boolValue)
                {
                    EditorGUILayout.PropertyField(meshOutlineThickness);
                    EditorGUILayout.PropertyField(meshOutlineAdaptiveThickness);

                    if (depthMask.enumValueIndex == 2)
                    {
                        EditorGUILayout.PropertyField(useSingleMeshOutline);
                        if (useSingleMeshOutline.boolValue)
                        {
                            EditorGUILayout.PropertyField(meshOutlineFront);
                        }
                        else
                        {
                            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                            {
                                EditorGUILayout.LabelField("Front", centeredBoldLabel);
                                EditorGUILayout.PropertyField(meshOutlineFront);
                                EditorGUILayout.Space();
                            }
                            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                            {
                                EditorGUILayout.LabelField("Back", centeredBoldLabel);
                                EditorGUILayout.PropertyField(meshOutlineBack);
                                EditorGUILayout.Space();
                            }
                        }
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(meshOutlineFront);
                    }

                }
            }
            
            switch (depthMask.enumValueIndex)
            {
                // None
                case 3:
                    InnerGlowDrawer(innerGlowFront);
                    OverlayDrawer(overlayFront);
                    break;

                // Back
                case 0:
                    InnerGlowDrawer(innerGlowBack);
                    OverlayDrawer(overlayBack);
                    break;

                // Front
                case 1:
                    InnerGlowDrawer(innerGlowFront);
                    OverlayDrawer(overlayFront);
                    break;

                // Both
                case 2:
                    InnerGlowDrawerBoth();
                    OverlayDrawerBoth();
                    break;
            }

        }

        // Inner Glow drawers
        void InnerGlowHeader(SerializedProperty useRim)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Inner Glow", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(useRim, new GUIContent(""));
            }

        }

        void InnerGlowDrawer(SerializedProperty innerGlowSettings)
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                InnerGlowHeader(useInnerGlow);
                if (useInnerGlow.boolValue)
                {
                    EditorGUILayout.PropertyField(innerGlowSettings);
                }
            }
        }

        void InnerGlowDrawerBoth()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                InnerGlowHeader(useInnerGlow);
                if (useInnerGlow.boolValue)
                {
                    EditorGUILayout.PropertyField(useSingleInnerGlow);
                    if (useSingleInnerGlow.boolValue)
                    {
                        EditorGUILayout.PropertyField(innerGlowFront);
                    }
                    else
                    {
                        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                        {
                            EditorGUILayout.LabelField("Front", centeredBoldLabel);
                            EditorGUILayout.PropertyField(innerGlowFront);
                            EditorGUILayout.Space();
                        }
                        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                        {
                            EditorGUILayout.LabelField("Back", centeredBoldLabel);
                            EditorGUILayout.PropertyField(innerGlowBack);
                            EditorGUILayout.Space();
                        }
                    }

                }
            }
        }


        // Overlay drawers
        void OverlayHeader(SerializedProperty useOverlay)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Overlay", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(useOverlay, new GUIContent(""));
            }

        }

        void OverlayDrawer(SerializedProperty overlaySettings)
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                OverlayHeader(useOverlay);
                if (useOverlay.boolValue)
                {
                    EditorGUILayout.PropertyField(overlaySettings);
                }
            }
        }

        void OverlayDrawerBoth()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                OverlayHeader(useOverlay);
                if (useOverlay.boolValue)
                {
                    EditorGUILayout.PropertyField(useSingleOverlay);
                    if (useSingleOverlay.boolValue)
                    {
                        EditorGUILayout.PropertyField(overlayFront);
                    }
                    else
                    {
                        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                        {
                            EditorGUILayout.LabelField("Front", centeredBoldLabel);
                            EditorGUILayout.PropertyField(overlayFront);
                            EditorGUILayout.Space();
                        }
                        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                        {
                            EditorGUILayout.LabelField("Back", centeredBoldLabel);
                            EditorGUILayout.PropertyField(overlayBack);
                            EditorGUILayout.Space();
                        }
                    }

                }
            }
        }

    }


    [CanEditMultipleObjects]
    [CustomPropertyDrawer(typeof(HighlighterRenderer))]
    public class HighlighterRendererDrawerUIE : PropertyDrawer
    {
        const int StandardPropertyHeight = 20;

        const int NumberOfClipProperties = 3;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int height = 0;
            SerializedProperty useCutout = property.FindPropertyRelative("useCutout");
            SerializedProperty clippingSource = property.FindPropertyRelative("clippingSource");
            SerializedProperty submeshIndexes = property.FindPropertyRelative("submeshIndexes");

            // renderer and useCutout properties
            height += 3 * StandardPropertyHeight;

            // submesh indexex property
            // if(submeshIndexes.arraySize==0 ) height += 20;
            //height += Mathf.Max(submeshIndexes.arraySize,1) * StandardPropertyHeight;
            //Debug.Log(submeshIndexes.CountInProperty());

            int subMeshIndexesPropCount = submeshIndexes.CountInProperty();
            if (subMeshIndexesPropCount == 1)
            {
                height += 20;
            }
            else
            {
                height += Mathf.Max(subMeshIndexesPropCount, 3) * 20;
                height += 10;
            }

            height += 10;

            if (useCutout.boolValue)
            {
                if (clippingSource.enumValueIndex == 1)
                {
                    height += NumberOfClipProperties * StandardPropertyHeight;
                }
                else height += (NumberOfClipProperties - 1) * StandardPropertyHeight;
            }

            return height;
        }

        public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label)
        {
            SerializedProperty renderer = property.FindPropertyRelative("renderer");
            SerializedProperty useCutout = property.FindPropertyRelative("useCutout");
            SerializedProperty clippingThreshold = property.FindPropertyRelative("clippingThreshold");
            SerializedProperty clippingSource = property.FindPropertyRelative("clippingSource");
            SerializedProperty clipTexture = property.FindPropertyRelative("clipTexture");
            SerializedProperty submeshIndexes = property.FindPropertyRelative("submeshIndexes");
            SerializedProperty cullMode = property.FindPropertyRelative("cullMode");

            // Objects on top
            EditorGUI.PropertyField(new Rect(pos.x, pos.y, pos.width, StandardPropertyHeight), renderer);
            EditorGUI.PropertyField(new Rect(pos.x, pos.y + StandardPropertyHeight, pos.width, StandardPropertyHeight), useCutout);

            int index = 2;

            EditorGUI.PropertyField(new Rect(pos.x, pos.y + index * StandardPropertyHeight, pos.width, StandardPropertyHeight), cullMode, false);
            index++;

            if (useCutout.boolValue)
            {

                EditorGUI.PropertyField(new Rect(pos.x, pos.y + index * StandardPropertyHeight, pos.width, StandardPropertyHeight), clippingThreshold);
                index++;
                EditorGUI.PropertyField(new Rect(pos.x, pos.y + index * StandardPropertyHeight, pos.width, StandardPropertyHeight), clippingSource);
                index++;
                if (clippingSource.enumValueIndex == 1)
                {
                    EditorGUI.PropertyField(new Rect(pos.x, pos.y + index * StandardPropertyHeight, pos.width, StandardPropertyHeight), clipTexture);
                    index++;
                }

            }

            EditorGUI.PropertyField(new Rect(pos.x, pos.y + index * StandardPropertyHeight, pos.width, StandardPropertyHeight), submeshIndexes, true);

        }
    }


    [CanEditMultipleObjects]
    [CustomPropertyDrawer(typeof(HighlighterSettings.InnerGlowSettings))]
    public class InnerGlowSettingsDrawerUIE : PropertyDrawer
    {
        const int StandardPropertyHeight = 20;

        const int XSpacing = 10;
        const int YSpacing = 10;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int height = 0;

            height += 2 * StandardPropertyHeight;
            //height += YSpacing;

            return height;
        }

        public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label)
        {
            SerializedProperty Color = property.FindPropertyRelative("color");
            SerializedProperty Power = property.FindPropertyRelative("power");

            // Objects on top
            EditorGUI.PropertyField(new Rect(pos.x, pos.y, pos.width, StandardPropertyHeight), Color);
            EditorGUI.PropertyField(new Rect(pos.x, pos.y + StandardPropertyHeight, pos.width, StandardPropertyHeight), Power);
        }
    }


    [CanEditMultipleObjects]
    [CustomPropertyDrawer(typeof(HighlighterSettings.MeshOutlineSettings))]
    public class MeshOutlineSettingsDrawerUIE : PropertyDrawer
    {
        const int StandardPropertyHeight = 20;

        const int XSpacing = 10;
        const int YSpacing = 10;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int height = 0;

            height += 1 * StandardPropertyHeight;
            //height += YSpacing;

            return height;
        }

        public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label)
        {
            SerializedProperty Color = property.FindPropertyRelative("color");

            // Objects on top
            EditorGUI.PropertyField(new Rect(pos.x, pos.y, pos.width, StandardPropertyHeight), Color);
        }
    }


    [CanEditMultipleObjects]
    [CustomPropertyDrawer(typeof(HighlighterSettings.OverlaySettings))]
    public class OverlaySettingsDrawerUIE : PropertyDrawer
    {
        const int StandardPropertyHeight = 20;

        const int XSpacing = 10;
        const int YSpacing = 10;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int height = 0;

            SerializedProperty useTexture = property.FindPropertyRelative("useTexture");

            height += 2 * StandardPropertyHeight;
            if (useTexture.boolValue)
            {
                height += 4 * StandardPropertyHeight;
            }

            return height;
        }

        public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label)
        {
            SerializedProperty Color = property.FindPropertyRelative("color");
            SerializedProperty useTexture = property.FindPropertyRelative("useTexture");
            SerializedProperty Texture = property.FindPropertyRelative("texture");
            SerializedProperty Background = property.FindPropertyRelative("background");
            SerializedProperty Tilling = property.FindPropertyRelative("tilling");
            SerializedProperty Rotation = property.FindPropertyRelative("rotation");

            // Objects on top
            EditorGUI.PropertyField(new Rect(pos.x, pos.y, pos.width, StandardPropertyHeight), Color);
            EditorGUI.PropertyField(new Rect(pos.x, pos.y + StandardPropertyHeight, pos.width, StandardPropertyHeight), useTexture);

            if (useTexture.boolValue)
            {
                int index = 2;
                EditorGUI.PropertyField(new Rect(pos.x, pos.y + index * StandardPropertyHeight, pos.width, StandardPropertyHeight), Texture);
                index++;
                EditorGUI.PropertyField(new Rect(pos.x, pos.y + index * StandardPropertyHeight, pos.width, StandardPropertyHeight), Background);
                index++;
                EditorGUI.PropertyField(new Rect(pos.x, pos.y + index * StandardPropertyHeight, pos.width, StandardPropertyHeight), Tilling);
                index++;
                EditorGUI.PropertyField(new Rect(pos.x, pos.y + index * StandardPropertyHeight, pos.width, StandardPropertyHeight), Rotation);
            }
        }
    }



    [CustomEditor(typeof(Highlighter))]
    [CanEditMultipleObjects]
    public class HighlighterEditor : Editor
    {
        SerializedProperty highlighterSettings;
        SerializedProperty findRenderers;
        SerializedProperty renderers;
        SerializedProperty id;
        SerializedProperty showRenderBounds;
        SerializedProperty showScreenAdjustedRenderBounds;
        SerializedProperty RenderingBoundsDistanceFix;
        SerializedProperty RenderingBoundsSizeIncrease;
        SerializedProperty RenderingBoundsMaxDistanceFix;
        SerializedProperty RenderingBoundsMinDistanceFix;

        void OnEnable()
        {
            highlighterSettings = serializedObject.FindProperty("highlighterSettings");
            findRenderers = serializedObject.FindProperty("findRenderers");
            renderers = serializedObject.FindProperty("renderers");
            id = serializedObject.FindProperty("id");
            showRenderBounds = serializedObject.FindProperty("showRenderBounds");
            showScreenAdjustedRenderBounds = serializedObject.FindProperty("showScreenAdjustedRenderBounds");
            RenderingBoundsSizeIncrease = highlighterSettings.FindPropertyRelative("RenderingBoundsSizeIncrease");
            RenderingBoundsDistanceFix = highlighterSettings.FindPropertyRelative("RenderingBoundsDistanceFix");
            RenderingBoundsMaxDistanceFix = highlighterSettings.FindPropertyRelative("RenderingBoundsMaxDistanceFix");
            RenderingBoundsMinDistanceFix = highlighterSettings.FindPropertyRelative("RenderingBoundsMinDistanceFix");
        }

        public override void OnInspectorGUI()
        {
            //DrawDefaultInspector();
            Highlighter myScript = (Highlighter)target;
            serializedObject.Update();

            GUIStyle centeredBoldLabel = new GUIStyle()
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = 12,
            };
            centeredBoldLabel.normal.textColor = Color.white;


            //EditorGUILayout.Separator();
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                //EditorGUILayout.LabelField("Refresh ", EditorStyles.boldLabel);

                if (GUILayout.Button(new GUIContent("Refresh highlighters", "Use when not all highlighters are visible in the editor scene.")))
                {
                    myScript.Setup();
                    Highlighter.HighlightersNeedReset();
                }

                GUI.enabled = false;
                EditorGUILayout.PropertyField(id, new GUIContent("Highlighter ID"));
                GUI.enabled = true;
            }

            EditorGUILayout.PropertyField(highlighterSettings);

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                var centeredLabel = new GUIStyle(GUI.skin.label) { 
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 15,
                    fontStyle = FontStyle.Bold
                };


                using (var h = new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Setup", EditorStyles.boldLabel,GUILayout.MaxWidth(55));
                    if (GUILayout.Button("Find Renderers"))
                    {
                        myScript.GetRenderers();
                    }

                    EditorGUILayout.PropertyField(findRenderers,new GUIContent(""));
                    
                }
                EditorGUILayout.Space();

                if(renderers.arraySize <= 0)
                {
                    EditorGUILayout.HelpBox("There is no renderers to be highlighted.", MessageType.Warning);
                    EditorGUILayout.Space();
                }

                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(renderers);
                EditorGUI.indentLevel--;
            }


            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {

                EditorGUILayout.LabelField("Rendering Bounds", centeredBoldLabel);

                EditorGUILayout.PropertyField(showRenderBounds);
                EditorGUILayout.PropertyField(showScreenAdjustedRenderBounds);
                EditorGUILayout.PropertyField(RenderingBoundsSizeIncrease);
                EditorGUILayout.PropertyField(RenderingBoundsDistanceFix);
                EditorGUILayout.PropertyField(RenderingBoundsMaxDistanceFix);
                EditorGUILayout.PropertyField(RenderingBoundsMinDistanceFix);
            }


            serializedObject.ApplyModifiedProperties();
        }
    }
}
