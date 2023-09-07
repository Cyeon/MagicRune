using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LiteShader
{
    [CanEditMultipleObjects]
    public class LiteShaderEditor : ShaderGUI
    {


        private static class Styles
        {
            public static string blendingOptionsText = "Blending Options";
            public static string mainOptionsText = "Main Options";
            public static string mapsOptionsText = "Maps";
            public static string requiredVertexStreamsText = "Required Vertex Streams";

            public static string streamPositionText = "Position (POSITION.xyz)";
            public static string streamColorText = "Color (COLOR.xyzw)";
            public static string streamUVText = "UV (TEXCOORD0.xy)";
            public static string streamUV2Text = "UV2 (TEXCOORD0.zw)";

            public static GUIContent streamApplyToAllSystemsText = EditorGUIUtility.TrTextContent("Apply to Systems", "Apply the vertex stream layout to all Particle Systems using this material");
        }

        static bool bEnableCutTex = false;

        static bool bEnableUVRotation = false;
        static bool bEnableEmissionGain = false;

        static bool bEnableCutUVRotation = false;


        protected MaterialEditor m_Editor;
        protected Material m_Material;
        protected MaterialProperty[] m_Properties;

        static int Space01 = 5;

        float m_DragAndDropMinY;
        bool m_FirstTimeApply = true;

        MaterialProperty m_property;

        List<ParticleSystemRenderer> m_RenderersUsingThisMaterial = new List<ParticleSystemRenderer>();


        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {



            EditorGUI.BeginChangeCheck();

            this.m_Editor = materialEditor;
            this.m_Material = this.m_Editor.target as Material;
            this.m_Properties = properties;

            if (m_FirstTimeApply)
            {
                CacheRenderersUsingThisMaterial(m_Material);
                m_FirstTimeApply = false;
            }

            if (this.m_Material.HasProperty("_isCut"))
                bEnableCutTex = (this.m_Material.GetFloat("_isCut") > 0.0f);

            if (this.m_Material.HasProperty("_isRotation"))
                bEnableUVRotation = (this.m_Material.GetFloat("_isRotation") > 0.0f);

            if (this.m_Material.HasProperty("_isCutRotation"))
                bEnableCutUVRotation = (this.m_Material.GetFloat("_isCutRotation") > 0.0f);

            if (this.m_Material.HasProperty("_isEmissionGain"))
                bEnableEmissionGain = (this.m_Material.GetFloat("_isEmissionGain") > 0.0f);

            this.DoGUI();
            this.m_Editor.RenderQueueField();
            this.m_Editor.EnableInstancingField();
            this.streamApplyToAllSystems();

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(materialEditor.target);
            }

        }

        private void DoGUI()
        {

            GUILayout.BeginVertical("box");

            GUIStyle TitleStyle = new GUIStyle(GUI.skin.button);
            TitleStyle.alignment = TextAnchor.MiddleLeft;
            TitleStyle.fontSize = 13;
            TitleStyle.fontStyle = FontStyle.Bold;

            GUIStyle TitleStyle2 = new GUIStyle(GUI.skin.button);
            TitleStyle2.alignment = TextAnchor.MiddleLeft;
            TitleStyle2.fontSize = 9;
            TitleStyle2.stretchWidth = true;

            GUILayout.Toggle(true, "Main", TitleStyle);
            this.SetProperty(LiteShaderDefine.MainTexStr, this.m_Properties);
            EditorGUI.indentLevel += 1;
            //this.SetProperty(LiteShaderDefine.CutParticleSoftValue, this.m_Properties);
            this.SetProperty("_MainTexXSpeed", this.m_Properties);
            this.SetProperty("_MainTexYSpeed", this.m_Properties);
            this.SetProperty("_light", this.m_Properties);
            this.SetProperty("_lightGain", this.m_Properties);

            EditorGUI.indentLevel -= 1;
            GUILayout.Space(Space01);

            GUILayout.BeginHorizontal();
            GUILayout.Space(Space01);
            GUILayout.BeginVertical("box");
            bEnableUVRotation = GUILayout.Toggle(bEnableUVRotation, "Rotation", TitleStyle2);
            if (bEnableUVRotation)
            {
                EditorGUI.indentLevel += 1;
                this.SetProperty(LiteShaderDefine.MainRotationStr, this.m_Properties);
                EditorGUI.indentLevel -= 1;
            }
            GUILayout.EndVertical();
            GUILayout.Space(Space01);
            GUILayout.EndHorizontal();





            GUILayout.BeginHorizontal();
            GUILayout.Space(Space01);
            GUILayout.BeginVertical("box");
            bEnableEmissionGain = GUILayout.Toggle(bEnableEmissionGain, "Emission Gain", TitleStyle2);
            if (bEnableEmissionGain)
            {
                EditorGUI.indentLevel += 1;
                this.SetProperty(LiteShaderDefine.EmissionGain, this.m_Properties);
                EditorGUI.indentLevel -= 1;
            }
            GUILayout.EndVertical();
            GUILayout.Space(Space01);
            GUILayout.EndHorizontal();

            GUILayout.Space(Space01);
            GUILayout.EndVertical();


            GUILayout.Space(Space01);
            GUILayout.BeginVertical("box");


            bEnableCutTex = GUILayout.Toggle(bEnableCutTex, "Mash", TitleStyle);


            if (bEnableCutTex)
            {


                this.SetProperty(LiteShaderDefine.CutTexStr, this.m_Properties);
                EditorGUI.indentLevel += 1;
                //this.SetProperty(LiteShaderDefine.CutOffStr, this.m_Properties);
                //this.SetProperty("_CutSheetX", this.m_Properties);
                //this.SetProperty("_CutSheetY", this.m_Properties);
                EditorGUI.indentLevel -= 1;
                GUILayout.Space(Space01);

                GUILayout.BeginHorizontal();
                GUILayout.Space(Space01);
                GUILayout.BeginVertical("box");
                bEnableCutUVRotation = GUILayout.Toggle(bEnableCutUVRotation, "Rotation", TitleStyle2);

                if (bEnableCutUVRotation)
                {

                    EditorGUI.indentLevel += 1;
                    this.SetProperty(LiteShaderDefine.CutRotationStr, this.m_Properties);
                    EditorGUI.indentLevel -= 1;
                }
                GUILayout.EndVertical();
                GUILayout.Space(Space01);
                GUILayout.EndHorizontal();


                GUILayout.Space(Space01);
            }
            GUILayout.EndVertical();

            SetSoftAndFading();
            this.SetKeyword();
        }

        private void streamApplyToAllSystems()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.BeginHorizontal();
            GUILayout.Space(Space01);
            GUILayout.BeginVertical();

            GUILayout.Space(Space01);
            GUILayout.Label(Styles.streamPositionText, EditorStyles.label);
            GUILayout.Label(Styles.streamColorText, EditorStyles.label);
            GUILayout.Label(Styles.streamUVText, EditorStyles.label);

            GUILayout.Label("Custom1.xyzw (TEXCOORD1.xyzy)", EditorStyles.label);
            GUILayout.Label("         Custom1.x = Main Texture Scroll X", EditorStyles.miniLabel);
            GUILayout.Label("         Custom1.y = Main Texture Scroll Y", EditorStyles.miniLabel);
            GUILayout.Label("         Custom1.z = Main Texture Size", EditorStyles.miniLabel);
            GUILayout.Label("         Custom1.w = Mash Texture rotation", EditorStyles.miniLabel);
            GUILayout.Space(Space01);

            // Build the list of expected vertex streams
            List<ParticleSystemVertexStream> streams = new List<ParticleSystemVertexStream>();
            streams.Add(ParticleSystemVertexStream.Position);
            streams.Add(ParticleSystemVertexStream.Color);
            streams.Add(ParticleSystemVertexStream.UV);
            streams.Add(ParticleSystemVertexStream.UV2);
            streams.Add(ParticleSystemVertexStream.Custom1XYZW);



            if (GUILayout.Button(Styles.streamApplyToAllSystemsText, EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
            {
                foreach (ParticleSystemRenderer renderer in m_RenderersUsingThisMaterial)
                {
                    if (renderer != null)
                    {
                        renderer.SetActiveVertexStreams(streams);
                    }
                }
            }


            // Display a warning if any renderers have incorrect vertex streams
            string Warnings = "";
            List<ParticleSystemVertexStream> rendererStreams = new List<ParticleSystemVertexStream>();
            foreach (ParticleSystemRenderer renderer in m_RenderersUsingThisMaterial)
            {
                if (renderer != null)
                {
                    renderer.GetActiveVertexStreams(rendererStreams);

                    bool streamsValid;
                    streamsValid = rendererStreams.SequenceEqual(streams);

                    if (!streamsValid)
                        Warnings += "  " + renderer.name + "\n";
                }
            }
            if (Warnings != "")
            {
                EditorGUILayout.HelpBox("The following Particle System Renderers are using this material with incorrect Vertex Streams:\n" + Warnings + "Use the Apply to Systems button to fix this", MessageType.Warning, true);
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.Space(Space01);
            GUILayout.EndVertical();
            //EditorGUILayout.Space();
        }

        private void SetSoftAndFading()
        {
            bool useFading = false;
            bool useSoftParticles = false;

            if (this.m_Material.HasProperty("_CameraFadingEnabled"))
                useFading = (m_Material.GetFloat("_CameraFadingEnabled") > 0.0f);
            if (this.m_Material.HasProperty("_SoftParticlesEnabled"))
                useSoftParticles = (m_Material.GetFloat("_SoftParticlesEnabled") > 0.0f);

            useSoftParticles = GUILayout.Toggle(useSoftParticles, "Soft Particles Enabled");
            if (useSoftParticles)
            {
                this.SetProperty("_SoftParticlesNearFadeDistance", this.m_Properties);
                this.SetProperty("_SoftParticlesFarFadeDistance", this.m_Properties);
            }


            useFading = GUILayout.Toggle(useFading, "Camera Fading Enabled");
            if (useFading)
            {
                this.SetProperty("_CameraNearFadeDistance", this.m_Properties);
                this.SetProperty("_CameraFarFadeDistance", this.m_Properties);
            }

            this.m_Material.SetFloat("_SoftParticlesEnabled", useSoftParticles ? 1.0f : 0.0f);
            this.m_Material.SetFloat("_CameraFadingEnabled", useFading ? 1.0f : 0.0f);
            SetKeyword(this.m_Material, LiteShaderDefine.EnableSoftParticlesStr, useSoftParticles);
            SetKeyword(this.m_Material, LiteShaderDefine.EnableFadingStr, useFading);

        }

        private void SetKeyword()
        {
            this.m_Material.SetFloat("_isCut", bEnableCutTex ? 1.0f : 0.0f);
            this.m_Material.SetFloat("_isRotation", bEnableUVRotation ? 1.0f : 0.0f);
            this.m_Material.SetFloat("_isCutRotation", bEnableCutUVRotation ? 1.0f : 0.0f);
            this.m_Material.SetFloat("_isEmissionGain", bEnableEmissionGain ? 1.0f : 0.0f);
            //SetKeyword(this.m_Material, LiteShaderDefine.EnableAlphaMaskStr, bEnableCutTex);
            //SetKeyword(this.m_Material, LiteShaderDefine.EnableUVRotationStr, bEnableUVRotation);
            //SetKeyword(this.m_Material, LiteShaderDefine.EnableCutUVRotationStr, bEnableCutUVRotation);
            //SetKeyword(this.m_Material, LiteShaderDefine.EnableEmissionGain, bEnableEmissionGain);

        }

        private void SetProperty(string propertyName, MaterialProperty[] properties)
        {
            if (m_Material.HasProperty(propertyName))
            {
                this.m_property = FindProperty(propertyName, properties);
                this.m_Editor.ShaderProperty(this.m_property, this.m_property.displayName);
            }
        }

        static void SetKeyword(Material m, string keyword, bool state)
        {
            if (state)
                m.EnableKeyword(keyword);
            else
                m.DisableKeyword(keyword);
        }


        void CacheRenderersUsingThisMaterial(Material material)
        {
            m_RenderersUsingThisMaterial.Clear();

            ParticleSystemRenderer[] renderers = UnityEngine.Object.FindObjectsOfType(typeof(ParticleSystemRenderer)) as ParticleSystemRenderer[];
            foreach (ParticleSystemRenderer renderer in renderers)
            {
                if (renderer.sharedMaterial == material)
                    m_RenderersUsingThisMaterial.Add(renderer);
            }
        }

    }

}
