using UnityEngine;

namespace LiteShader
{
    public class LiteShaderDefine
    {
        public const string ColorStr = "_Color";
        public const string MainTexStr = "_MainTex";
        public const string CutTexStr = "_CutTex";
        public const string DisTexStr = "_DisTex";
        public const string CutOffStr = "_Cutoff";
        public const string MainRotationStr = "_MainRotation";
        public const string CutRotationStr = "_CutRotation";
        public const string UVScrollX = "_UVScrollX";
        public const string UVScrollY = "_UVScrollY";
        public const string UVCutScrollX = "_UVCutScrollX";
        public const string UVCutScrollY = "_UVCutScrollY";
        public const string UVDisScrollX = "_UVDisScrollX";
        public const string UVDisScrollY = "_UVDisScrollY";
        public const string CutParticleSoftValue = "_InvFade";
        public const string UVMirrorX = "_UVMirrorX";
        public const string UVMirrorY = "_UVMirrorY";
        public const string DissolveSrc = "_DissolveSrc";
        public const string SpecColor = "_SpecColor";
        public const string Shininess = "_Shininess";
        public const string Amount = "_Amount";
        public const string StartAmount = "_StartAmount";
        public const string DissColor = "_DissColor";
        public const string Illuminate = "_Illuminate";
        public const string EmissionGain = "_EmissionGain";
        public const string ShadowColor = "_ShadowColor";
        public const string SpecularPower = "_SpecularPower";
        public const string EdgeThickness = "_EdgeThickness";
        public const string EdgeSaturtion = "_EdgeSaturtion";
        public const string EdgeBrightness = "_EdgeBrightness";
        public const string FalloffSampler = "_FalloffSampler";
        public const string RimLightSampler = "_RimLightSampler";
        public const string ColorFactor = "_ColorFactor";
        public const string DisPowerX = "_DisPowerX";
        public const string DisPowerY = "_DisPowerY";
        public const string DisMode = "_DisMode";
        

        public const string EnableAlphaMaskStr = "Enable_AlphaMask";
        public const string EnableUVRotationStr = "Enable_UVRotation";
        public const string EnableUVScrollStr = "Enable_UVScroll";
        public const string EnableCutUVRotationStr = "Enable_CutUVRotation";
        public const string EnableCutUVScrollStr = "Enable_CutUVScroll";
        public const string EnableEmissionGain = "Enable_EmissionGain";

        public const string EnableDistortionStr = "Enable_Distortion";
        public const string EnableDisUVScrollStr = "Enable_DisUVScroll";
        public const string EnableSoftParticlesStr = "SOFTPARTICLES_ON";
        public const string EnableFadingStr = "_FADING_ON";

        public static readonly int Material_Color = Shader.PropertyToID(LiteShaderDefine.ColorStr);
        public static readonly int Material_Color_Factor = Shader.PropertyToID(LiteShaderDefine.ColorFactor);
    }

}
