using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Highlighters
{
    public enum DepthMask
    {
        BehindOnly, FrontOnly, Both, Disable
    }

    /// <summary>
    /// Class containing all the settings for each highlighter class.
    /// </summary>
    [System.Serializable]
    public class HighlighterSettings
    {
        // TODO: test performance | what to do with the material properties updating functions
        // probably you should replace public fields with setter functions

        #region structers

        [System.Serializable]
        public class OverlaySettings
        {
            [NonSerialized]HighlighterSettings highlighterSettings;
            public OverlaySettings(HighlighterSettings highlighterSettings)
            {
                this.highlighterSettings = highlighterSettings;
            }

            public void SetMaterialProperties(string type)
            {
                highlighterSettings.overlayMaterial.SetColor("_OverlayColor" + type, Color);
                highlighterSettings.overlayMaterial.SetFloat("_OverlayBackground" + type, Background);
                highlighterSettings.overlayMaterial.SetInt("_OverlayUseTex" + type, UseTexture ? 1 : 0);
                highlighterSettings.overlayMaterial.SetTexture("_OverlayTex" + type, Texture);
                highlighterSettings.overlayMaterial.SetFloat("_OverlayTilling" + type, Tilling);
                highlighterSettings.overlayMaterial.SetFloat("_OverlayRotation" + type, Rotation);
            }

            [Tooltip("Color of the overlay.")]
            [SerializeField]private Color color = Color.cyan;

            /// <summary>
            /// Color of the overlay.
            /// </summary>
            public Color Color
            {
                get => color;
                set
                {
                    color = value;
                    highlighterSettings.SetOverlayMaterialProperties();
                }
            }

            [Tooltip("Whether to use texture for overlay patterns.")]
            [SerializeField] private bool useTexture = false;

            /// <summary>
            /// Whether to use texture for overlay patterns.
            /// </summary>
            public bool UseTexture
            {
                get => useTexture;
                set
                {
                    useTexture = value;
                    highlighterSettings.SetOverlayMaterialProperties();
                }
            }

            [Tooltip("Overlay pattern texture.")]
            [SerializeField] private Texture2D texture;

            /// <summary>
            /// Overlay pattern texture.
            /// </summary>
            public Texture2D Texture
            {
                get => texture;
                set
                {
                    texture = value;
                    highlighterSettings.SetOverlayMaterialProperties();
                }
            }

            [Tooltip("Overlay background opacity.")]
            [Range(0, 1),SerializeField] private float background = 0f;

            /// <summary>
            /// Overlay background opacity.
            /// </summary>
            public float Background
            {
                get => background;
                set
                {
                    background = value;
                    highlighterSettings.SetOverlayMaterialProperties();
                }
            }

            [Tooltip("Tilling of the overlay pattern.")]
            [SerializeField] private float tilling = 60;

            /// <summary>
            /// Tilling of the overlay pattern.
            /// </summary>
            public float Tilling
            {
                get => tilling;
                set
                {
                    tilling = value;
                    highlighterSettings.SetOverlayMaterialProperties();
                }
            }

            [Tooltip("Rotation of the overlay pattern.")]
            [Range(0, 180),SerializeField] private float rotation = 0;

            /// <summary>
            /// Rotation of the overlay pattern.
            /// </summary>
            public float Rotation
            {
                get => rotation;
                set
                {
                    rotation = value;
                    highlighterSettings.SetOverlayMaterialProperties();
                }
            }
        }

        [System.Serializable]
        public class InnerGlowSettings
        {
            [NonSerialized] HighlighterSettings highlighterSettings;
            public InnerGlowSettings(HighlighterSettings highlighterSettings)
            {
                this.highlighterSettings = highlighterSettings;
            }

            public void SetMaterialProperties(string type)
            {
                highlighterSettings.overlayMaterial.SetColor("_RimColor" + type, Color);
                highlighterSettings.overlayMaterial.SetFloat("_RimPower" + type, Power);
            }

            [Tooltip("Color of the inner glow.")]
            [SerializeField,ColorUsage(true,true)] private Color color = Color.cyan;

            /// <summary>
            /// Color of the inner glow.
            /// </summary>
            public Color Color
            {
                get => color;
                set
                {
                    color = value;
                    highlighterSettings.SetOverlayMaterialProperties();
                }
            }

            [Tooltip("Power of the rim used for inner glow.")]
            [Range(0, 7),SerializeField] private float power = 4f;

            /// <summary>
            /// Power of the rim used for inner glow.
            /// </summary>
            public float Power
            {
                get => power;
                set
                {
                    power = value;
                    highlighterSettings.SetOverlayMaterialProperties();
                }
            }
        }

        [System.Serializable]
        public class MeshOutlineSettings
        {
            [NonSerialized] HighlighterSettings highlighterSettings;
            public MeshOutlineSettings(HighlighterSettings highlighterSettings)
            {
                this.highlighterSettings = highlighterSettings;
            }

            public void SetMaterialProperties(string type)
            {
                highlighterSettings.meshOutlineMaterial.SetColor("_Color" + type, Color);
            }


            [Tooltip("Color of the mesh outline.")]
            [ColorUsage(true,true),SerializeField] private Color color = Color.white;

            /// <summary>
            /// Color of the mesh outline.
            /// </summary>
            public Color Color
            {
                get => color;
                set
                {
                    color = value;
                    highlighterSettings.SetMeshOutlineMaterialProperties();
                }
            }
            
        }

        public enum BlurType
        {
            Gaussian, Box
        }

        public enum BlendType
        {
            Alpha, Additive
        }

        #endregion

        #region general

        
        [SerializeField,Tooltip("A type of masking for highlighter.")] private DepthMask depthMask = DepthMask.Disable;

        /// <summary>
        /// A type of masking for highlighter.
        /// </summary>
        public DepthMask DepthMask
        {
            get => depthMask;
            set
            {
                depthMask = value;
                Highlighter.HighlightersNeedReset();
            }
        }

        /*

        [SerializeField, Tooltip("Layer mask used for depth testing.")] private LayerMask depthLayerMask = 0;

        /// <summary>
        /// Layer mask used for depth testing.
        /// </summary>
        public LayerMask DepthLayerMask
        {
            get => depthLayerMask;
            set
            {
                depthLayerMask = value;
                Highlighter.HighlightersNeedReset();
            }
        }
        */

        [Range(0.05f, 1), Tooltip("Render scale of the info buffer. If the render scale is set to e.g. 0.5, the highlighter will be rendered with half the resolution of the camera. Use as low number as possible to improve performance.")]
        [SerializeField] private float infoRenderScale = 1;

        /// <summary>
        /// Render scale of the info buffer.
        /// </summary>
        public float InfoRenderScale
        {
            get => infoRenderScale;
            set
            {
                infoRenderScale = value;
                Highlighter.HighlightersNeedReset();
            }
        }

        #endregion

        #region outerGlow
        [Tooltip("Enable or disable outer glow.")]
        [SerializeField] private bool useOuterGlow = false;

        /// <summary>
        /// Enable or disable outer glow.
        /// </summary>
        public bool UseOuterGlow
        {
            get => useOuterGlow;
            set
            {
                useOuterGlow = value;
            }
        }

        [Tooltip("Whether to use one color for the front and back of the object.")]
        [SerializeField] private bool useSingleOuterGlow = true;

        /// <summary>
        /// Whether to use one color for the front and back of the object.
        /// </summary>
        public bool UseSingleOuterGlow
        {
            get => useSingleOuterGlow;
            set
            {
                useSingleOuterGlow = value;
                SetBlurMaterialProperties();
            }
        }

        [Tooltip("Front color of the outer glow.")]
        [ColorUsage(true, true), SerializeField] private Color outerGlowColorFront = Color.black;

        /// <summary>
        /// Front color of the outer glow.
        /// </summary>
        public Color OuterGlowColorFront
        {
            get => outerGlowColorFront;
            set
            {
                outerGlowColorFront = value;
                SetBlurMaterialProperties();
                SetAlphaBlitMaterialProperties();
            }
        }

        [Tooltip("Back color of the outer glow.")]
        [ColorUsage(true, true),SerializeField] private Color outerGlowColorBack = Color.black;

        /// <summary>
        /// Back color of the outer glow.
        /// </summary>
        public Color OuterGlowColorBack
        {
            get => outerGlowColorBack;
            set
            {
                outerGlowColorBack = value;
                SetBlurMaterialProperties();
                SetAlphaBlitMaterialProperties();
            }
        }

        [Tooltip("Should the outline be visible in front of the object.")]
        [SerializeField] private bool outlineVisibleBeforeObject = true;

        /// <summary>
        /// Should the outline be visible in front of the object.
        /// </summary>
        public bool OutlineVisibleBeforeObject
        {
            get => outlineVisibleBeforeObject;
            set
            {
                outlineVisibleBeforeObject = value;
                SetBlurMaterialProperties();
            }
        }

        [Tooltip("The type of algorithm used to blur outer glow.")]
        [SerializeField] private BlurType blurType = BlurType.Box;

        /// <summary>
        /// The type of algorithm used to blur outer glow.
        /// </summary>
        public BlurType BlurRenderingType
        {
            get => blurType;
        }

        [Tooltip("A type of blending used to blend the outer glow with the camera texture.")]
        [SerializeField] private BlendType blendType;

        /// <summary>
        /// A type of blending used to blend the outer glow with the camera texture.
        /// </summary>
        public BlendType BlendingType
        {
            get => blendType;
        }

        [Tooltip("The larger the box blur sample, the thicker the outer glow")]
        [Range(0, 0.15f), SerializeField] private float boxBlurSize = 0.04f;

        /// <summary>
        /// The size of the box blur sample. The larger it is, the thicker the outer glow.
        /// </summary>
        public float BoxBlurSize
        {
            get => boxBlurSize;
            set
            {
                boxBlurSize = value;
                SetBlurMaterialProperties();
            }
        }

        [Tooltip("The number of iterations of the blur algorithm. Use the smallest number possible to improve performance.")]
        [Range(0, 50), SerializeField] private float blurIterations = 40;

        /// <summary>
        /// The number of iterations of the blur algorithm. Use the smallest number possible to improve performance.
        /// </summary>
        public float BlurIterations
        {
            get => blurIterations;
            set
            {
                blurIterations = value;
                SetBlurMaterialProperties();
            }
        }

        [Tooltip("Render scale of the blur buffer. If the render scale is set to e.g. 0.5, the outer glow will be rendered with half the resolution of the camera. Use as low value as possible to improve performance.")]
        [Range(0.05f, 1), SerializeField] private float blurRenderScale = 1;

        /// <summary>
        /// Render scale of the blur buffer.
        /// </summary>
        public float BlurRenderScale
        {
            get => blurRenderScale;
            set
            {
                blurRenderScale = value;
                Highlighter.HighlightersNeedReset();
            }
        }

        [Tooltip("Visibility level of outer glow. Increases the amount of outer glow.")]
        [Range(0, 6),SerializeField] private float blurIntensity = 1;

        /// <summary>
        /// Visibility level of outer glow. Increases the amount of outer glow.
        /// </summary>
        public float BlurIntensity
        {
            get => blurIntensity;
            set
            {
                blurIntensity = value;
                SetBlurMaterialProperties();
            }
        }

        [Tooltip("Multiplier for the Blur Intensity property.")]
        [SerializeField] private float blurIntensityMultiplier = 1;

        /// <summary>
        /// Multiplier for the Blur Intensity property.
        /// </summary>
        public float BlurIntensityMultiplier
        {
            get => blurIntensityMultiplier;
            set
            {
                blurIntensityMultiplier = value;
                SetBlurMaterialProperties();
            }
        }

        [Tooltip("How much distance from the camera will impact the thickness. Makes the outer glow with gaussian blur fade out, and the box blur retains its width when the camera is moving away.")]
        [SerializeField, Range(0, 1)]private float blurAdaptiveThickness = 0;

        /// <summary>
        /// How much distance from the camera will impact the thickness.
        /// </summary>
        public float BlurAdaptiveThickness
        {
            get => blurAdaptiveThickness;
            set
            {
                blurAdaptiveThickness = value;
                UpdateBlurAdaptiveProperties();
            }
        }

        [Tooltip("Control how fast the thickness should change based on the distance of the object from the camera.")]
        [SerializeField, Range(0.1f, 20)] private float cameraDistanceMultiplier = 1;

        /// <summary>
        /// Control how fast the thickness should change based on distance of the object from the camera.
        /// </summary>
        public float CameraDistanceMultiplier
        {
            get => cameraDistanceMultiplier;
            set
            {
                cameraDistanceMultiplier = value;
                UpdateBlurAdaptiveProperties();
            }
        }

        private float cameraObjectDistace = 0;

        /// <summary>
        /// Internal Property | Used only in ObjectsPass.cs to write correct distance from the current camera to the center of rendering bounds.
        /// </summary>
        public float CameraObjectDistace
        {
            get => cameraObjectDistace;
            set
            {
                cameraObjectDistace = value;
                UpdateBlurAdaptiveProperties();
            }
        }

        private void UpdateBlurAdaptiveProperties()
        {
            if (blurMaterial == null) return;

            switch (blurType)
            {
                case BlurType.Gaussian:
                    var iter = Mathf.Lerp(blurIterations, blurIterations / cameraObjectDistace * cameraDistanceMultiplier, blurAdaptiveThickness);
                    blurMaterial.SetFloat("_BlurIterations", iter);
                    break;
                case BlurType.Box:
                    var blurSize = Mathf.Lerp(boxBlurSize, boxBlurSize / cameraObjectDistace * cameraDistanceMultiplier, blurAdaptiveThickness);
                    blurMaterial.SetFloat("_BoxBlurSize", blurSize);
                    break;
            }
        }

        #endregion

        #region meshOutline
        [Tooltip("Enable or disable mesh outline.")]
        [SerializeField] private bool useMeshOutline = false;

        /// <summary>
        /// Enable or disable mesh outline.
        /// </summary>
        public bool UseMeshOutline
        {
            get => useMeshOutline;
            set
            {
                useMeshOutline = value;
                overlayMaterial.SetInt("_UseMeshOutline", useMeshOutline ? 1 : 0);
            }
        }

        [Tooltip("Whether to use a single color for the object's front and back.")]
        [SerializeField] private bool useSingleMeshOutline = true;

        /// <summary>
        /// Whether to use one color for the front and back of the object.
        /// </summary>
        public bool UseSingleMeshOutline
        {
            get => useSingleMeshOutline;
            set
            {
                useSingleMeshOutline = value;
                SetMeshOutlineMaterialProperties();
            }
        }

        [SerializeField] private MeshOutlineSettings meshOutlineFront;

        /// <summary>
        /// Holds settings for the front mesh outline.
        /// </summary>
        public MeshOutlineSettings MeshOutlineFront
        {
            get => meshOutlineFront;
            set
            {
                meshOutlineFront = value;
                SetMeshOutlineMaterialProperties();
            }
        }

        
        [SerializeField] private MeshOutlineSettings meshOutlineBack;

        /// <summary>
        /// Holds settings for the back mesh outline.
        /// </summary>
        public MeshOutlineSettings MeshOutlineBack
        {
            get => meshOutlineBack;
            set
            {
                meshOutlineBack = value;
                SetMeshOutlineMaterialProperties();
            }
        }


        [Tooltip("Thickness of the outline.")]
        [Range(0, 0.1f),SerializeField] private float meshOutlineThickness = 0.1f;

        /// <summary>
        /// Thickness of the outline.
        /// </summary>
        public float MeshOutlineThickness
        {
            get => meshOutlineThickness;
            set
            {
                meshOutlineThickness = value;
                SetMeshOutlineMaterialProperties();
            }
        }

        [Tooltip("As the camera moves away, the outline becomes thicker.")]
        [Range(0, 1), SerializeField] private float meshOutlineAdaptiveThickness = 0;

        /// <summary>
        /// How much distance from the camera will impact the thickness.
        /// </summary>
        public float MeshOutlineAdaptiveThickness
        {
            get => meshOutlineAdaptiveThickness;
            set
            {
                meshOutlineAdaptiveThickness = value;
                SetMeshOutlineMaterialProperties();
            }
        }
        

        #endregion

        #region innerGlow
        [Tooltip("Enable or disable inner glow.")]
        [SerializeField] private bool useInnerGlow = false;

        /// <summary>
        /// Enable or disable inner glow.
        /// </summary>
        public bool UseInnerGlow
        {
            get => useInnerGlow;
            set
            {
                useInnerGlow = value;
                SetOverlayMaterialProperties();
            }
        }

        [Tooltip("Whether to use a single color for the object's front and back.")]
        [SerializeField] private bool useSingleInnerGlow = true;

        /// <summary>
        /// Whether to use one color for the front and back of the object.
        /// </summary>
        public bool UseSingleInnerGlow
        {
            get => useSingleInnerGlow;
            set
            {
                useSingleInnerGlow = value;
                SetOverlayMaterialProperties();
            }
        }

        [SerializeField] private InnerGlowSettings innerGlowFront;

        /// <summary>
        /// Holds settings for the front inner glow.
        /// </summary>
        public InnerGlowSettings InnerGlowFront
        {
            get => innerGlowFront;
            set
            {
                innerGlowFront = value;
                SetOverlayMaterialProperties();
            }
        }

        [SerializeField]private InnerGlowSettings innerGlowBack;

        /// <summary>
        /// Holds settings for the back inner glow.
        /// </summary>
        public InnerGlowSettings InnerGlowBack
        {
            get => innerGlowBack;
            set
            {
                innerGlowBack = value;
                SetOverlayMaterialProperties();
            }
        }

        #endregion

        #region overlay
        [Tooltip("Enable or disable overlay.")]
        [SerializeField] private bool useOverlay = false;

        /// <summary>
        /// Enable or disable overlay.
        /// </summary>
        public bool UseOverlay
        {
            get => useOverlay;
            set
            {
                useOverlay = value;
                SetOverlayMaterialProperties();
            }
        }

        [Tooltip("Whether to use a single color for the object's front and back.")]
        [SerializeField] private bool useSingleOverlay = true;

        /// <summary>
        /// Whether to use one color for the front and back of the object.
        /// </summary>
        public bool UseSingleOverlay
        {
            get => useSingleOverlay;
            set
            {
                useSingleOverlay = value;
                SetOverlayMaterialProperties();
            }
        }

        [SerializeField] private OverlaySettings overlayFront;

        /// <summary>
        /// Holds settings for the front overlay.
        /// </summary>
        public OverlaySettings OverlayFront
        {
            get => overlayFront;
            set
            {
                overlayFront = value;
                SetOverlayMaterialProperties();
            }
        }

        [SerializeField] private OverlaySettings overlayBack;

        /// <summary>
        /// Holds settings for the back overlay.
        /// </summary>
        public OverlaySettings OverlayBack
        {
            get => overlayBack;
            set
            {
                overlayBack = value;
                SetOverlayMaterialProperties();
            }
        }

        #endregion

        #region other

        public HighlighterSettings() 
        {
            overlayBack = new OverlaySettings(this);
            overlayFront = new OverlaySettings(this);

            innerGlowBack = new InnerGlowSettings(this);
            innerGlowFront = new InnerGlowSettings(this);

            meshOutlineBack = new MeshOutlineSettings(this);
            meshOutlineFront = new MeshOutlineSettings(this);
        }

        /// <summary>
        /// Checks whether any overlay is turned on.
        /// </summary>
        /// <returns>True if useOverlay or useInnerGlow or useMeshOutline is true.</returns>
        public bool IsAnyOverlayUsed()
        {
            if (useOverlay) return true;
            if (useInnerGlow) return true;
            if (useMeshOutline) return true;
            return false;
        }

        
        private Vector4 renderingBounds;

        /// <summary>
        /// Rendering bounds values | Vector4(MinX, MinY, MaxX, MaxY)
        /// </summary>
        public Vector4 RenderingBounds
        {
            get => renderingBounds;
        }

        [Tooltip("Value that the rendering bounds of the renderers should be increased to avoid outer glow clipping.")]
        public float RenderingBoundsSizeIncrease = 0.2f;

        [Tooltip("Fix rendering bounds when camera is near the center of rendering bounds.")]
        public bool RenderingBoundsDistanceFix = false;
        [Tooltip("Upper limit of distance of the camera and the center of rendering bounds when the distance fix will be applied.")]
        [Range(-2,2)]public float RenderingBoundsMaxDistanceFix = 1;
        [Tooltip("Lower limit of distance of the camera and the center of rendering bounds when the distance fix will be applied.")]
        [Range(-2, 2)] public float RenderingBoundsMinDistanceFix = 0;


        /// <summary>
        /// Set rendering bounds of the highlighter to speed up shader passes.
        /// </summary>
        /// <param name="renderingBounds"> Vector4(MinX, MinY, MaxX, MaxY) in viewport space</param>
        /// <param name="boundsBehind"> Are the bounds rendered behind camera.</param>
        public void SetRenderBoundsValues(Vector4 renderingBounds)
        {
            this.renderingBounds = renderingBounds;

            if (blurMaterial)
            {
                blurMaterial.SetVector("_RenderingBounds", renderingBounds);
            }

            if (alphaBlitMaterial)
            {
                alphaBlitMaterial.SetVector("_RenderingBounds", renderingBounds);
            }

            if (overlayMaterial)
            {
                overlayMaterial.SetVector("_RenderingBounds", renderingBounds);
            }

        }

        #endregion

        #region materialPropertiesSetters

        // Materials for different passes, updated when new pass is created
        // =========================================================
        private Material alphaBlitMaterial;
        private Material blurMaterial;
        private Material meshOutlineMaterial;
        private Material overlayMaterial;
        // =========================================================


        // Materials properties setters used in the rendering passes
        // =========================================================

        public void SetAlphaBlitMaterialProperties(Material material = null)
        {
            if (material)
            {
                alphaBlitMaterial = material;
            }
            if(alphaBlitMaterial)
            {
                alphaBlitMaterial.SetColor("_ColorFront", outerGlowColorFront);
                alphaBlitMaterial.SetColor("_ColorBack", outerGlowColorBack);
            }
        }

        public void SetBlurMaterialProperties(Material material = null)
        {
            if (material)
            {
                blurMaterial = material;
            }

            if (blurMaterial == null) return;

            // General
            blurMaterial.SetInt("_DepthMask", (int)depthMask);

            blurMaterial.SetFloat("_BlurIterations", blurIterations);

            // Box Blur
            blurMaterial.SetFloat("_BoxBlurSize", boxBlurSize);

            switch(blurType)
            {
                case BlurType.Box:
                    blurMaterial.EnableKeyword("_BOXBLUR");
                    blurMaterial.DisableKeyword("_GAUSSIANBLUR");
                blurMaterial.SetFloat("_BlurIntensity", (blurIntensity/6) * blurIntensityMultiplier);
                    break;
                case BlurType.Gaussian:
                    blurMaterial.DisableKeyword("_BOXBLUR");
                blurMaterial.SetFloat("_BlurIntensity", Mathf.Pow(blurIntensity, 2) * blurIntensityMultiplier);
                    blurMaterial.EnableKeyword("_GAUSSIANBLUR");
                    break;

            }

            if (depthMask == DepthMask.Both)
            {
                blurMaterial.SetColor("_ColorFront", outerGlowColorFront);
                blurMaterial.SetColor("_ColorBack", outerGlowColorBack);
                blurMaterial.SetInt("_UseSingleOuterGlow", useSingleOuterGlow ? 1 : 0);

            }
            else
            {
                blurMaterial.SetColor("_ColorFront", outerGlowColorFront);
                blurMaterial.SetColor("_ColorBack", outerGlowColorFront); 
                blurMaterial.SetInt("_OutlineVisibleBeforeObject", outlineVisibleBeforeObject ? 1: 0);
            }


        }

        public void SetMeshOutlineMaterialProperties(Material material = null)
        {
            if (material)
            {
                meshOutlineMaterial = material;
            }

            if (meshOutlineMaterial == null) return;

            //Mesh Outline
            if (depthMask == DepthMask.Both)
            {
                if (useSingleMeshOutline)
                {
                    // Set same color for back and front
                    meshOutlineFront.SetMaterialProperties("Front");
                    meshOutlineFront.SetMaterialProperties("Back");
                }
                else
                {
                    meshOutlineFront.SetMaterialProperties("Front");
                    meshOutlineBack.SetMaterialProperties("Back");
                }
            }
            else
            {
                meshOutlineFront.SetMaterialProperties("Front");
                meshOutlineFront.SetMaterialProperties("Back");
            }
            

            meshOutlineMaterial.SetFloat("_AdaptiveThickness", meshOutlineAdaptiveThickness);
            meshOutlineMaterial.SetFloat("_Thickness", meshOutlineThickness);
        }

        public void SetOverlayMaterialProperties(Material material = null)
        {
            if (material)
            {
                overlayMaterial = material;
            }

            if (overlayMaterial == null) return;

            // General
            overlayMaterial.SetInt("_DepthMask", (int)depthMask); 

            //Mesh Outline
            overlayMaterial.SetInt("_UseMeshOutline", useMeshOutline ? 1 : 0);

            // Inner Glow
            if (useInnerGlow)
            {
                overlayMaterial.EnableKeyword("_INNERGLOW");
            }
            else
            {
                overlayMaterial.DisableKeyword("_INNERGLOW");
            }

            overlayMaterial.SetInt("_UseSingleInnerGlow", useSingleInnerGlow ? 1 : 0);
            innerGlowFront.SetMaterialProperties("Front");
            innerGlowBack.SetMaterialProperties("Back");

            // Overlay
            if (useOverlay)
            {
                overlayMaterial.EnableKeyword("_OVERLAY");
            }
            else
            {
                overlayMaterial.DisableKeyword("_OVERLAY");
            }

            overlayMaterial.SetInt("_UseSingleOverlay", useSingleOverlay ? 1 : 0);
            overlayFront.SetMaterialProperties("Front");
            overlayBack.SetMaterialProperties("Back");

        }

        // =========================================================

        #endregion
    }


    /// <summary>
    /// Holds data for each renderer that will be highlighted.
    /// </summary>
    [System.Serializable]
    public class HighlighterRenderer
    {
        public enum ClippingSource
        {
            Albedo, Texture
        }

        /// <summary>
        /// The renderer that should be used to draw the highlight.
        /// </summary>
        [Tooltip("The renderer that should be used to draw the highlight")]
        public Renderer renderer;

        /// <summary>
        /// A flag indicating whether the renderer should be drawn with cutoff.
        /// </summary>
        [Tooltip("A flag indicating whether the renderer should be drawn with cutoff.")]
        public bool useCutout = false;

        /// <summary>
        /// The alpha cutoff threshold.
        /// </summary>
        [Tooltip("The alpha cutoff threshold.")]
        [Range(0, 1)]
        public float clippingThreshold = 0f;

        /// <summary>
        /// The source to use for the clipping. This can be either the albedo or a texture.
        /// </summary>
        [Tooltip("The source to use for the clipping. This can be either the albedo or the texture. | Note: When drawing multiple submeshes, the albedo texture will be taken from the first submesh's material.")]
        public ClippingSource clippingSource = ClippingSource.Albedo;

        /// <summary>
        /// The texture to use for clipping. This is only used if the clipping source is set to Texture.
        /// </summary>
        [Tooltip("The texture to use for clipping. This is only used if the clipping source is set to texture.")]
        public Texture clipTexture;

        /// <summary>
        /// A list of the submesh indexes that should be used to draw the highlight.
        /// </summary>
        [Tooltip("A list of the submesh indexes that should be used to draw the highlight.")]
        public List<int> submeshIndexes = new List<int>();

        /// <summary>
        /// The culling mode to use when drawing the highlight.
        /// </summary>
        [Tooltip("The culling mode to use when drawing the highlight.")]
        public CullMode cullMode = CullMode.Back;

        /// <summary>
        /// Constructor for the HighlighterRenderer class. Initializes the submesh index list with the
        /// specified number of submeshes.
        /// </summary>
        /// <param name="renderer">The renderer to use for drawing the highlight.</param>
        /// <param name="numberOfSubmeshes">The number of submeshes in the renderer.</param>
        public HighlighterRenderer(Renderer renderer, int numberOfSubmeshes)
        {
            this.renderer = renderer;
            for (int i = 0; i < numberOfSubmeshes; i++)
            {
                submeshIndexes.Add(i);
            }
        }


        /// <summary>
        /// Returns the clip texture to use based on the specified
        /// </summary>
        public Texture GetClipTexture()
        {
            switch(clippingSource)
            {
                case ClippingSource.Albedo:
                    if (renderer == null)
                    {
                        // TODO handle better exception error when building
                        var tex = new Texture2D(1, 1);
                        return tex;
                    }
                    return renderer.sharedMaterial.mainTexture;

                case ClippingSource.Texture:
                    return clipTexture;

                default:
                    return clipTexture;
            } 


        }
    }

    /// <summary>
    /// Add to the object you want to highlight.
    /// HighlightManager automatically detects all instances of this script  in the scene.
    /// </summary>
    ////[ExecuteInEditMode]
    public class Highlighter : MonoBehaviour
    {
        #region structures
        public enum FindRenderers
        {
            FindRenderersInChildren, OnlyComponents
        }
        #endregion

        #region Properties
        [SerializeField] private HighlighterSettings highlighterSettings = new HighlighterSettings();

        /// <summary>
        /// Use to tweak settings of the highlighter.
        /// </summary>
        public HighlighterSettings Settings
        {
            get
            {
                return highlighterSettings;
            }
        }

        [Tooltip("Choose where the script should look for renderers.")]
        [SerializeField] public FindRenderers findRenderers;

        [SerializeField] private List<HighlighterRenderer> renderers = new List<HighlighterRenderer>();

        /// <summary>
        /// List containing all HighlighterRenderer objects that will be used to draw highlights.
        /// </summary>
        public List<HighlighterRenderer> Renderers
        {
            get => renderers;
            set => renderers = value;
        }

        [Tooltip("The ID of the highlighter.")]
        [SerializeField, HideInInspector] private int id = -1;

        /// <summary>
        /// The ID of the highlighter.
        /// </summary>
        public int ID
        {
            get
            {
                return id;
            }
        }

        [Tooltip("Draws helper gizmos of adjusted bounds of each renderer.")]
        [SerializeField] private bool showRenderBounds;
        [Tooltip("Draws helper gizmos of rendering bounds in screen space.")]
        [SerializeField] private bool showScreenAdjustedRenderBounds;

        #endregion

        #region Events

        public delegate void HighlighterChange(int ID);
        public static event HighlighterChange OnHighlighterValidate;

        public delegate void HighlightersReset();
        public static event HighlightersReset OnHighlighterReset;

        /// <summary>
        /// Updates specific highlighter in the manager
        /// </summary>
        /// <param name="ID"></param>
        public static void HighlighterValidate(int ID)
        {
            if (OnHighlighterValidate != null)
            {
                OnHighlighterValidate(ID);
            }
        }

        /// <summary>
        /// The manager Finds and updates all highlighters in the scene
        /// </summary>
        public static void HighlightersNeedReset()
        {
            if (OnHighlighterReset != null)
            {
                OnHighlighterReset();

            }
        }

        /// <summary>
        /// Needs to be called after you add new renderers or enable a new feature in highlighter. 
        /// </summary>
        public void HighlighterValidate()
        {
            if (OnHighlighterValidate != null)
            {
                OnHighlighterValidate(ID);
            }
        }

        #endregion

        public void OnDrawGizmos()
        {
#if UNITY_EDITOR

            if (!enabled) return;

            if (showRenderBounds)
            {
                for (int i = 0; i < renderers.Count; i++)
                {
                    var item = renderers[i];
                    if (item.renderer == null) continue;

                    var bounds = item.renderer.bounds;
                    
                    Gizmos.matrix = Matrix4x4.identity;
                    Gizmos.color = Color.blue;
                    Gizmos.DrawWireCube(bounds.center, bounds.extents * 2 + Vector3.one * Settings.RenderingBoundsSizeIncrease * 2);
                }
            }

            if (showScreenAdjustedRenderBounds)
            {
                Camera cam = null;

                foreach (var item in SceneView.GetAllSceneCameras())
                {
                    if (item.cameraType == CameraType.SceneView)
                    {
                        cam = item;
                    }
                }

                if (cam == null)
                {
                    Debug.Log("There is no scene camera.");
                    return;
                }

                var bounds = Settings.RenderingBounds;
                var boundsVec = new Vector4(bounds.x * cam.pixelWidth, cam.pixelHeight - bounds.y * cam.pixelHeight, bounds.z * cam.pixelWidth, cam.pixelHeight - bounds.w * cam.pixelHeight);

                UnityEditor.Handles.BeginGUI();

                float minX = boundsVec.x;
                float maxX = boundsVec.z;

                float minY = boundsVec.y;
                float maxY = boundsVec.w;

                UnityEditor.Handles.color = Color.red;

                UnityEditor.Handles.DrawLine(new Vector3(minX, minY, 0), new Vector3(minX, maxY, 0), 2);
                UnityEditor.Handles.DrawLine(new Vector3(minX, minY, 0), new Vector3(maxX, minY, 0), 2);
                UnityEditor.Handles.DrawLine(new Vector3(maxX, maxY, 0), new Vector3(minX, maxY, 0), 2);
                UnityEditor.Handles.DrawLine(new Vector3(maxX, maxY, 0), new Vector3(maxX, minY, 0), 2);

                UnityEditor.Handles.EndGUI();
            }
#endif
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            //Debug.Log("Validating highlighter: enabled: " + enabled.ToString() +" "+ name);


            if (enabled)
            {
                HighlighterValidate();
            }
            else
            {
                HighlightersNeedReset();
            }
        }

#endif

        public void OnEnable()
        {
            //Debug.Log("Enabling highlighter");

            // Ensures id is always setup correctly
            Setup();

            HighlightersNeedReset(); // this is too expensive

            //HighlighterValidate();
            //HighlighterValidate(id); // this breaks builds
        }

        public void OnDisable()
        {
            //Debug.Log("Disabling highlighter");

            //HighlighterValidate();
            HighlightersNeedReset(); // this is too expensive

            // TODO: when parent of a highlighter is deactivated in editor, the highligh remains.
            // But we cannot turn that option to be wokring in the editor because it breaks builds

            //if(Application.isPlaying || Application.isEditor)
            {
                //HighlightersNeedReset();
            }
        }

        public void OnDestroy()
        {
            enabled = false;
            Setup();
            HighlightersNeedReset();

            //Debug.Log("Destroying highlighter");

        }

        /// <summary>
        /// Finds working Id for the highlighter.
        /// </summary>
        public void Setup()
        {
            var highlighters = new List<Highlighter>(FindObjectsOfType<Highlighter>());
            var IDs = new List<int>();

            foreach (var item in highlighters)
            {
                if(item.enabled)
                {
                    IDs.Add(item.ID);
                }
            }

            for (int i = 1; i <= IDs.Count; i++)
            {
                if(!IDs.Contains(i))
                {
                    id = i;
                    break;
                }
            }

        }

        /// <summary>
        /// Finds renderers of the object based on the findRenderers value.
        /// </summary>
        public void GetRenderers()
        {
            switch (findRenderers)
            {
                case FindRenderers.FindRenderersInChildren:
                    renderers.Clear();

                    foreach (var item in GetComponentsInChildren<Renderer>())
                    {
                        renderers.Add(new HighlighterRenderer(item, item.sharedMaterials.Length));
                    }

                    break;
                case FindRenderers.OnlyComponents:
                    renderers.Clear();
                    var renderer = GetComponent<Renderer>();
                    renderers.Add(new HighlighterRenderer(renderer, renderer.sharedMaterials.Length));

                    break;
            }
        }

        /// <summary>
        /// Finds renderers in the object and in it's children.
        /// </summary>
        public void GetRenderersInChildren()
        {
            renderers.Clear();

            foreach (var item in GetComponentsInChildren<Renderer>())
            {
                renderers.Add(new HighlighterRenderer(item, item.sharedMaterials.Length));
            }
        }

        /// <summary>
        /// Finds renderers in the object's components.
        /// </summary>
        public void GetRenderersInComponents()
        {
            renderers.Clear();
            var renderer = GetComponent<Renderer>();
            renderers.Add(new HighlighterRenderer(renderer, renderer.sharedMaterials.Length));
        }
    }
}