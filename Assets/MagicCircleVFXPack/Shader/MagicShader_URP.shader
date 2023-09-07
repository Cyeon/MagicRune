Shader "LiteMagic/MagicShader_URP"
{
    Properties
    {
        [NoScaleOffset]_MainTex("Texture", 2D) = "white" {}
        _MainTex_ST("MainTex ST", Vector) = (0, 0, 0, 0)
        [NoScaleOffset]_DissolveTex("Dissolve Texture", 2D) = "white" {}
        _DissolveTex_ST("DissolveTex ST", Vector) = (1, 1, 0, 0)
        _GroundResidue("Ground Residue", Range(0, 1)) = 1
        [HDR]_RColor("R Color", Color) = (1, 1, 1, 1)
        _RSize("R Size", Float) = 1
        _RRotate("R Rotate", Float) = 0
        _RLight("R Light", Range(0, 1)) = 0.2
        _RLightGain("R Light Gain", Range(1, 20)) = 1
        [HDR]_RDissolveColor("R Dissolve Color", Color) = (1, 1, 1, 1)
        _RDissolveIn("R Dissolve Inside", Float) = 0
        _RDissolveOut("R Dissolve Outside", Float) = 1
        [HDR]_GColor("G Color", Color) = (1, 1, 1, 1)
        _GSize("G Size", Float) = 1
        _GRotate("G Rotate", Float) = 0
        _GLight("G Light", Range(0, 1)) = 0.2
        _GLightGain("G Light Gain", Range(1, 20)) = 1
        [HDR]_GDissolveColor("G Dissolve Color", Color) = (1, 1, 1, 1)
        _GDissolveIn("G Dissolve Inside", Float) = 0
        _GDissolveOut("G Dissolve Outside", Float) = 1
        [HDR]_BColor("B Color", Color) = (1, 1, 1, 1)
        _BSize("B Size", Float) = 1
        _BRotate("B Rotate", Float) = 0
        _BLight("B Light", Range(0, 1)) = 0.2
        _BLightGain("B Light Gain", Range(1, 20)) = 1
        [HDR]_BDissolveColor("B Dissolve Color", Color) = (1, 1, 1, 1)
        _BDissolveIn("B Dissolve Inside", Float) = 0
        _BDissolveOut("B Dissolve Outside", Float) = 1
        [HideInInspector]_DrawOrder("Draw Order", Range(-50, 50)) = 0
        [HideInInspector][Enum(Depth Bias, 0, View Bias, 1)]_DecalMeshBiasType("DecalMesh BiasType", Float) = 0
        [HideInInspector]_DecalMeshDepthBias("DecalMesh DepthBias", Float) = 0
        [HideInInspector]_DecalMeshViewBias("DecalMesh ViewBias", Float) = 0
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            // RenderType: <None>
            "PreviewType"="Plane"
            // Queue: <None>
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"=""
        }
        Pass
        { 
            Name "DBufferProjector"
            Tags 
            { 
                "LightMode" = "DBufferProjector"
            }
        
            // Render State
            Cull Front
        Blend 0 SrcAlpha OneMinusSrcAlpha, Zero OneMinusSrcAlpha
        Blend 1 SrcAlpha OneMinusSrcAlpha, Zero OneMinusSrcAlpha
        Blend 2 SrcAlpha OneMinusSrcAlpha, Zero OneMinusSrcAlpha
        ZTest Greater
        ZWrite Off
        ColorMask RGBA
        ColorMask RGBA 1
        ColorMask 0 2
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma target 3.5
        #pragma vertex Vert
        #pragma fragment Frag
        #pragma multi_compile_instancing
        
            // Keywords
            #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
            // GraphKeywords: <None>
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        
            // Defines
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD0
            
            #define HAVE_MESH_MODIFICATION
        
        
            #define SHADERPASS SHADERPASS_DBUFFER_PROJECTOR
        #define _MATERIAL_AFFECTS_ALBEDO 1
        #define _MATERIAL_AFFECTS_NORMAL 1
        #define _MATERIAL_AFFECTS_NORMAL_BLEND 1
        
            // HybridV1InjectedBuiltinProperties: <None>
        
            // -- Properties used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DecalInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderVariablesDecal.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
        
            // --------------------------------------------------
            // Structs and Packing
        
            struct Attributes
        {
             float3 positionOS : POSITION;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 TangentSpaceNormal;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 interp0 : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyzw =  input.texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.interp0.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float4 _DissolveTex_TexelSize;
        float _GroundResidue;
        float4 _RColor;
        float _RSize;
        float _RRotate;
        float _RLight;
        float4 _RDissolveColor;
        float _RLightGain;
        float _RDissolveIn;
        float _RDissolveOut;
        float4 _DissolveTex_ST;
        float4 _MainTex_ST;
        float4 _GColor;
        float _GSize;
        float _GRotate;
        float _GLight;
        float _GLightGain;
        float4 _GDissolveColor;
        float _GDissolveIn;
        float _GDissolveOut;
        float4 _BColor;
        float _BSize;
        float _BRotate;
        float _BLight;
        float _BLightGain;
        float4 _BDissolveColor;
        float _BDissolveIn;
        float _BDissolveOut;
        float _DrawOrder;
        float _DecalMeshBiasType;
        float _DecalMeshDepthBias;
        float _DecalMeshViewBias;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_DissolveTex);
        SAMPLER(sampler_DissolveTex);
        
            // Graph Functions
            
        void RotateSize_float(float4 _uv, float _size, float _rotate, out float4 Out){
                _rotate = _rotate*(3.14159265359/180);
            
                float2 ruv = float2(((_uv.x - 0.5) * 1/_size) + 0.5 ,((_uv.y - 0.5) * 1/_size) + 0.5);
            
                ruv = ruv - float2(0.5, 0.5);  
            
                ruv = float2(ruv.x * cos(_rotate) - ruv.y * sin(_rotate),ruv.x * sin(_rotate) + ruv.y * cos(_rotate));  
            
                ruv += float2(0.5, 0.5);
            
            
            
                Out = float4(ruv.x,ruv.y,_uv.z,_uv.w);
            
        }
        
        void DTexSet_float(float4 _uvD, float4 _DissolveTex_ST, out float4 Out){
             Out = float4((_uvD.x- 0.5) * _DissolveTex_ST.x+ _DissolveTex_ST.z,(_uvD.y- 0.5) * _DissolveTex_ST.y+ _DissolveTex_ST.w,_uvD.z,_uvD.w);
        }
        
        void TextureLight_float(float4 _tex, float _light, float4 _color, float _lightGain, out float4 Out){
                float4 RtexB = smoothstep( 1-_light,1,_tex);
            
                RtexB = RtexB * 0.2 + RtexB *  _color * _lightGain;
            
                _tex.rgb = _tex.rgb * _color.rgb + RtexB.rgb;
            
                _tex = _tex * _color.a *_tex.a ;
            
                Out = _tex;
        }
        
        void Dissolve_float(float2 _uv, float4 _Dtex, float _DissolveIn, float _DissolveOut, float4 _Color, float4 _tex, out float4 Out){
                float4 spheric = saturate( distance( _uv ,float2(0.5,0.5)) );
            
                spheric =saturate(spheric + spheric * _Dtex);
            
                           
            
                float4 spheric01A =saturate( saturate( (spheric + lerp(-1.01,0.01,1-_DissolveIn ))*100));
            
                float4 spheric01B = saturate( (spheric + lerp(-1.01,0.01,1-_DissolveIn- 0.01))*100);
            
                spheric01B = spheric01A * (1-spheric01B) * _Color * _Color.a;
            
            
            
                _tex = _tex * spheric01A + spheric01B * _tex.a;
            
            
            
                float4 spheric02A = saturate( ((1-spheric) + lerp(-1.01,0.01,_DissolveOut+ 0.01))*100);
            
                float4 spheric02B = saturate( ((1-spheric) + lerp(-1.01,0.01,_DissolveOut))*100);
            
                spheric02B =spheric02A * (1-spheric02B) * _Color * _Color.a;
            
                
            
                _tex = _tex * spheric02A + spheric02B * _tex.a;
            
                Out = _tex;
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
        {
            RGBA = float4(R, G, B, A);
            RGB = float3(R, G, B);
            RG = float2(R, G);
        }
        
            // Graph Vertex
            struct VertexDescription
        {
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            return description;
        }
            
            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float3 NormalTS;
            float NormalAlpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _UV_8cea45496b8c45eab3597a074d15d476_Out_0 = IN.uv0;
            float _Property_7b769f55c7864589aebfac4ddd12b2d6_Out_0 = _RSize;
            float _Property_4acdc5976b734662994ffee635158980_Out_0 = _RRotate;
            float4 _RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3;
            RotateSize_float(_UV_8cea45496b8c45eab3597a074d15d476_Out_0, _Property_7b769f55c7864589aebfac4ddd12b2d6_Out_0, _Property_4acdc5976b734662994ffee635158980_Out_0, _RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3);
            UnityTexture2D _Property_eafddcee5f7f4f66817085ef749c6b35_Out_0 = UnityBuildTexture2DStructNoScale(_DissolveTex);
            float4 _Property_1b4f5806053747519e92207b6d7afbee_Out_0 = _DissolveTex_ST;
            float4 _DTexSetCustomFunction_b0fd3e7232bf4e08ab5d88555c1b5756_Out_3;
            DTexSet_float(_RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3, _Property_1b4f5806053747519e92207b6d7afbee_Out_0, _DTexSetCustomFunction_b0fd3e7232bf4e08ab5d88555c1b5756_Out_3);
            float4 _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0 = SAMPLE_TEXTURE2D(_Property_eafddcee5f7f4f66817085ef749c6b35_Out_0.tex, _Property_eafddcee5f7f4f66817085ef749c6b35_Out_0.samplerstate, _Property_eafddcee5f7f4f66817085ef749c6b35_Out_0.GetTransformedUV((_DTexSetCustomFunction_b0fd3e7232bf4e08ab5d88555c1b5756_Out_3.xy)));
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_R_4 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.r;
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_G_5 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.g;
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_B_6 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.b;
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_A_7 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.a;
            float _Property_a077d5d9cbf747f5875c6c24513b2800_Out_0 = _RDissolveIn;
            float _Property_0ff262777af74848aa81b3dba71541ad_Out_0 = _RDissolveOut;
            float4 _Property_7368f6d6acbe464fb71b082ec611715e_Out_0 = IsGammaSpace() ? LinearToSRGB(_RDissolveColor) : _RDissolveColor;
            UnityTexture2D _Property_c21f55442e394cf284a0766460ec9cac_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0 = SAMPLE_TEXTURE2D(_Property_c21f55442e394cf284a0766460ec9cac_Out_0.tex, _Property_c21f55442e394cf284a0766460ec9cac_Out_0.samplerstate, _Property_c21f55442e394cf284a0766460ec9cac_Out_0.GetTransformedUV((_RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3.xy)));
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_R_4 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.r;
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_G_5 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.g;
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_B_6 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.b;
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_A_7 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.a;
            float4 _Swizzle_400506fb40164c489617df53e573832a_Out_1 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_R_4.xxxx;
            float _Property_a5385ead93534fa7aa7ee63377c08787_Out_0 = _RLight;
            float4 _Property_d4de088caa5d43b99fa6421c07169156_Out_0 = IsGammaSpace() ? LinearToSRGB(_RColor) : _RColor;
            float _Property_d8081504f4364890a7d4a0f3971be926_Out_0 = _RLightGain;
            float4 _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3;
            TextureLight_float(_Swizzle_400506fb40164c489617df53e573832a_Out_1, _Property_a5385ead93534fa7aa7ee63377c08787_Out_0, _Property_d4de088caa5d43b99fa6421c07169156_Out_0, _Property_d8081504f4364890a7d4a0f3971be926_Out_0, _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3);
            float4 _DissolveCustomFunction_c1278429f28948e08cf996a8b62479a9_Out_3;
            Dissolve_float((_RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3.xy), _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0, _Property_a077d5d9cbf747f5875c6c24513b2800_Out_0, _Property_0ff262777af74848aa81b3dba71541ad_Out_0, _Property_7368f6d6acbe464fb71b082ec611715e_Out_0, _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3, _DissolveCustomFunction_c1278429f28948e08cf996a8b62479a9_Out_3);
            float _Property_3808652b3bd84f0dae18b3e1973a9e4b_Out_0 = _GSize;
            float _Property_4618d5267616447a8c6b7427eb1a57dd_Out_0 = _GRotate;
            float4 _RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3;
            RotateSize_float(_UV_8cea45496b8c45eab3597a074d15d476_Out_0, _Property_3808652b3bd84f0dae18b3e1973a9e4b_Out_0, _Property_4618d5267616447a8c6b7427eb1a57dd_Out_0, _RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3);
            UnityTexture2D _Property_d018d28ee214437a82076451743a8130_Out_0 = UnityBuildTexture2DStructNoScale(_DissolveTex);
            float4 _Property_b4cc761c375e4bf5b21dd8dcf9704990_Out_0 = _DissolveTex_ST;
            float4 _DTexSetCustomFunction_65a258090b234db4ab663709ae4ac4d4_Out_3;
            DTexSet_float(_RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3, _Property_b4cc761c375e4bf5b21dd8dcf9704990_Out_0, _DTexSetCustomFunction_65a258090b234db4ab663709ae4ac4d4_Out_3);
            float4 _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0 = SAMPLE_TEXTURE2D(_Property_d018d28ee214437a82076451743a8130_Out_0.tex, _Property_d018d28ee214437a82076451743a8130_Out_0.samplerstate, _Property_d018d28ee214437a82076451743a8130_Out_0.GetTransformedUV((_DTexSetCustomFunction_65a258090b234db4ab663709ae4ac4d4_Out_3.xy)));
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_R_4 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.r;
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_G_5 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.g;
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_B_6 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.b;
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_A_7 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.a;
            float _Property_6e040bc940b740d3954fb5ef6f7965de_Out_0 = _GDissolveIn;
            float _Property_6dce9fc004da445ab2b9bebb19d43137_Out_0 = _GDissolveOut;
            float4 _Property_5863bb3472fe4bf1800650465375edcd_Out_0 = IsGammaSpace() ? LinearToSRGB(_GDissolveColor) : _GDissolveColor;
            UnityTexture2D _Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0.tex, _Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0.samplerstate, _Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0.GetTransformedUV((_RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3.xy)));
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_R_4 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.r;
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_G_5 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.g;
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_B_6 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.b;
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_A_7 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.a;
            float4 _Swizzle_c89196372af64bba9bf916bb81898cc9_Out_1 = _SampleTexture2D_cddf3472524741888ba91b342532777d_G_5.xxxx;
            float _Property_07a37b638f724de594b4970af7e3d22f_Out_0 = _GLight;
            float4 _Property_d7f27dbc94dd4dbca6685d8c913bff10_Out_0 = IsGammaSpace() ? LinearToSRGB(_GColor) : _GColor;
            float _Property_a2e2d040f3874da48daa383fde697713_Out_0 = _GLightGain;
            float4 _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3;
            TextureLight_float(_Swizzle_c89196372af64bba9bf916bb81898cc9_Out_1, _Property_07a37b638f724de594b4970af7e3d22f_Out_0, _Property_d7f27dbc94dd4dbca6685d8c913bff10_Out_0, _Property_a2e2d040f3874da48daa383fde697713_Out_0, _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3);
            float4 _DissolveCustomFunction_c794717a85c44b778d53446bc5599b11_Out_3;
            Dissolve_float((_RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3.xy), _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0, _Property_6e040bc940b740d3954fb5ef6f7965de_Out_0, _Property_6dce9fc004da445ab2b9bebb19d43137_Out_0, _Property_5863bb3472fe4bf1800650465375edcd_Out_0, _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3, _DissolveCustomFunction_c794717a85c44b778d53446bc5599b11_Out_3);
            float _Property_8fb0e68fd07a405b921fa2706f8ae7f4_Out_0 = _BSize;
            float _Property_6040d22195bf4819ae19ecc7e8841069_Out_0 = _BRotate;
            float4 _RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3;
            RotateSize_float(_UV_8cea45496b8c45eab3597a074d15d476_Out_0, _Property_8fb0e68fd07a405b921fa2706f8ae7f4_Out_0, _Property_6040d22195bf4819ae19ecc7e8841069_Out_0, _RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3);
            UnityTexture2D _Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0 = UnityBuildTexture2DStructNoScale(_DissolveTex);
            float4 _Property_53bc3e203cdf45779cf450bcccc43ff8_Out_0 = _DissolveTex_ST;
            float4 _DTexSetCustomFunction_e8155feacdd943ddb8f4ff867df48696_Out_3;
            DTexSet_float(_RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3, _Property_53bc3e203cdf45779cf450bcccc43ff8_Out_0, _DTexSetCustomFunction_e8155feacdd943ddb8f4ff867df48696_Out_3);
            float4 _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0 = SAMPLE_TEXTURE2D(_Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0.tex, _Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0.samplerstate, _Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0.GetTransformedUV((_DTexSetCustomFunction_e8155feacdd943ddb8f4ff867df48696_Out_3.xy)));
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_R_4 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.r;
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_G_5 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.g;
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_B_6 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.b;
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_A_7 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.a;
            float _Property_df06dff0c2484fb595c137b1754753b9_Out_0 = _BDissolveIn;
            float _Property_d214b0d6c4864bde9efaa3db6db55a86_Out_0 = _BDissolveOut;
            float4 _Property_f2164c96cf7945fd975240ed7e800b18_Out_0 = IsGammaSpace() ? LinearToSRGB(_BDissolveColor) : _BDissolveColor;
            UnityTexture2D _Property_d2937aa927074faf8a7e3502b5f57de2_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0 = SAMPLE_TEXTURE2D(_Property_d2937aa927074faf8a7e3502b5f57de2_Out_0.tex, _Property_d2937aa927074faf8a7e3502b5f57de2_Out_0.samplerstate, _Property_d2937aa927074faf8a7e3502b5f57de2_Out_0.GetTransformedUV((_RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3.xy)));
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_R_4 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.r;
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_G_5 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.g;
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_B_6 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.b;
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_A_7 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.a;
            float4 _Swizzle_f28776abd40e4239a0500f742ca08953_Out_1 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_B_6.xxxx;
            float _Property_ff648668c0b14c5bb553df8ea7ab4c99_Out_0 = _BLight;
            float4 _Property_f2d51e21017b4d62854d0dd0d69a4eeb_Out_0 = IsGammaSpace() ? LinearToSRGB(_BColor) : _BColor;
            float _Property_2358423daf2b46bd88fc2c6f83681dab_Out_0 = _BLightGain;
            float4 _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3;
            TextureLight_float(_Swizzle_f28776abd40e4239a0500f742ca08953_Out_1, _Property_ff648668c0b14c5bb553df8ea7ab4c99_Out_0, _Property_f2d51e21017b4d62854d0dd0d69a4eeb_Out_0, _Property_2358423daf2b46bd88fc2c6f83681dab_Out_0, _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3);
            float4 _DissolveCustomFunction_18ec96fae3434badafbd66e66f5f7fc7_Out_3;
            Dissolve_float((_RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3.xy), _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0, _Property_df06dff0c2484fb595c137b1754753b9_Out_0, _Property_d214b0d6c4864bde9efaa3db6db55a86_Out_0, _Property_f2164c96cf7945fd975240ed7e800b18_Out_0, _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3, _DissolveCustomFunction_18ec96fae3434badafbd66e66f5f7fc7_Out_3);
            float4 _Add_b5aafd4ee22e4670a9b2a0eeae0b991b_Out_2;
            Unity_Add_float4(_DissolveCustomFunction_c794717a85c44b778d53446bc5599b11_Out_3, _DissolveCustomFunction_18ec96fae3434badafbd66e66f5f7fc7_Out_3, _Add_b5aafd4ee22e4670a9b2a0eeae0b991b_Out_2);
            float4 _Add_311c3433751947a9b670b04484a11b3b_Out_2;
            Unity_Add_float4(_DissolveCustomFunction_c1278429f28948e08cf996a8b62479a9_Out_3, _Add_b5aafd4ee22e4670a9b2a0eeae0b991b_Out_2, _Add_311c3433751947a9b670b04484a11b3b_Out_2);
            float _Swizzle_19e7c60d34834501ad98d57eecf88a20_Out_1 = _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3.w;
            float _Swizzle_2b2f7ab3dfd748fbb705d617d01b1690_Out_1 = _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3.w;
            float _Swizzle_d4f48dddbcae41cfbf129451e203ecf4_Out_1 = _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3.w;
            float _Add_d05af43285cb4cce8638140598473902_Out_2;
            Unity_Add_float(_Swizzle_2b2f7ab3dfd748fbb705d617d01b1690_Out_1, _Swizzle_d4f48dddbcae41cfbf129451e203ecf4_Out_1, _Add_d05af43285cb4cce8638140598473902_Out_2);
            float _Add_747b1754f01448a9bb57f2993c19791e_Out_2;
            Unity_Add_float(_Swizzle_19e7c60d34834501ad98d57eecf88a20_Out_1, _Add_d05af43285cb4cce8638140598473902_Out_2, _Add_747b1754f01448a9bb57f2993c19791e_Out_2);
            float4 _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGBA_4;
            float3 _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGB_5;
            float2 _Combine_94b4ca9170e44d93bdf05a483d08c44b_RG_6;
            Unity_Combine_float(0, 0, 0, _Add_747b1754f01448a9bb57f2993c19791e_Out_2, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGBA_4, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGB_5, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RG_6);
            float4 _Add_2670f6bd6b3f4beb94a86259f697e3cf_Out_2;
            Unity_Add_float4(_Add_311c3433751947a9b670b04484a11b3b_Out_2, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGBA_4, _Add_2670f6bd6b3f4beb94a86259f697e3cf_Out_2);
            float _Swizzle_6cead8bc7a22455b93e7f26073d331da_Out_1 = _Add_2670f6bd6b3f4beb94a86259f697e3cf_Out_2.w;
            surface.BaseColor = IsGammaSpace() ? float3(0, 0, 0) : SRGBToLinear(float3(0, 0, 0));
            surface.Alpha = _Swizzle_6cead8bc7a22455b93e7f26073d331da_Out_1;
            surface.NormalTS = IN.TangentSpaceNormal;
            surface.NormalAlpha = 1;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            
        //     $features.graphVertex:  $include("VertexAnimation.template.hlsl")
        //                                       ^ ERROR: $include cannot find file : VertexAnimation.template.hlsl. Looked into:
        // Packages/com.unity.shadergraph/Editor/Generation/Templates
        
            
        //     $features.graphPixel:   $include("SharedCode.template.hlsl")
        //                                       ^ ERROR: $include cannot find file : SharedCode.template.hlsl. Looked into:
        // Packages/com.unity.shadergraph/Editor/Generation/Templates
        
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorCopyToSDI' */
        
        
        
            output.TangentSpaceNormal =                         float3(0.0f, 0.0f, 1.0f);
        
        
            output.uv0 =                                        input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
            return output;
        }
        
            // --------------------------------------------------
            // Build Surface Data
        
            uint2 ComputeFadeMaskSeed(uint2 positionSS)
            {
                uint2 fadeMaskSeed;
        
                // Can't use the view direction, it is the same across the entire screen.
                fadeMaskSeed = positionSS;
        
                return fadeMaskSeed;
            }
        
            void GetSurfaceData(Varyings input, half3 viewDirectioWS, uint2 positionSS, float angleFadeFactor, out DecalSurfaceData surfaceData)
            {
                #if (SHADERPASS == SHADERPASS_DBUFFER_PROJECTOR) || (SHADERPASS == SHADERPASS_FORWARD_EMISSIVE_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_SCREEN_SPACE_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_GBUFFER_PROJECTOR)
                    half4x4 normalToWorld = UNITY_ACCESS_INSTANCED_PROP(Decal, _NormalToWorld);
                    half fadeFactor = clamp(normalToWorld[0][3], 0.0f, 1.0f) * angleFadeFactor;
                    float2 scale = float2(normalToWorld[3][0], normalToWorld[3][1]);
                    float2 offset = float2(normalToWorld[3][2], normalToWorld[3][3]);
                    input.texCoord0.xy = input.texCoord0.xy * scale + offset;
                #else
                    #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                        LODDitheringTransition(ComputeFadeMaskSeed(positionSS), unity_LODFade.x);
                    #endif
        
                    half fadeFactor = half(1.0);
                #endif
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = BuildSurfaceDescriptionInputs(input);
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
        
                // setup defaults -- these are used if the graph doesn't output a value
                ZERO_INITIALIZE(DecalSurfaceData, surfaceData);
                surfaceData.occlusion = half(1.0);
                surfaceData.smoothness = half(0);
        
                #ifdef _MATERIAL_AFFECTS_NORMAL
                    surfaceData.normalWS.w = half(1.0);
                #else
                    surfaceData.normalWS.w = half(0.0);
                #endif
        
        
                // copy across graph values, if defined
                surfaceData.baseColor.xyz = half3(surfaceDescription.BaseColor);
                surfaceData.baseColor.w = half(surfaceDescription.Alpha * fadeFactor);
        
                #if (SHADERPASS == SHADERPASS_DBUFFER_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_SCREEN_SPACE_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_GBUFFER_PROJECTOR)
                    #if defined(_MATERIAL_AFFECTS_NORMAL)
                        surfaceData.normalWS.xyz = mul((half3x3)normalToWorld, surfaceDescription.NormalTS.xyz);
                    #else
                        surfaceData.normalWS.xyz = normalToWorld[2].xyz;
                    #endif
                #elif (SHADERPASS == SHADERPASS_DBUFFER_MESH) || (SHADERPASS == SHADERPASS_DECAL_SCREEN_SPACE_MESH) || (SHADERPASS == SHADERPASS_DECAL_GBUFFER_MESH)
                    #if defined(_MATERIAL_AFFECTS_NORMAL)
                        float sgn = input.tangentWS.w;      // should be either +1 or -1
                        float3 bitangent = sgn * cross(input.normalWS.xyz, input.tangentWS.xyz);
                        half3x3 tangentToWorld = half3x3(input.tangentWS.xyz, bitangent.xyz, input.normalWS.xyz);
        
                        // We need to normalize as we use mikkt tangent space and this is expected (tangent space is not normalize)
                        surfaceData.normalWS.xyz = normalize(TransformTangentToWorld(surfaceDescription.NormalTS, tangentToWorld));
                    #else
                        surfaceData.normalWS.xyz = half3(input.normalWS); // Default to vertex normal
                    #endif
                #endif
        
                surfaceData.normalWS.w = surfaceDescription.NormalAlpha * fadeFactor;
        
                // In case of Smoothness / AO / Metal, all the three are always computed but color mask can change
            }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPassDecal.hlsl"
        
            ENDHLSL
        }
        Pass
        { 
            Name "DecalProjectorForwardEmissive"
            Tags 
            { 
                "LightMode" = "DecalProjectorForwardEmissive"
            }
        
            // Render State
            Cull Front
        Blend 0 SrcAlpha One
        ZTest Greater
        ZWrite Off
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma target 3.5
        #pragma vertex Vert
        #pragma fragment Frag
        #pragma multi_compile_instancing
        
            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        
            // Defines
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD0
            
            #define HAVE_MESH_MODIFICATION
        
        
            #define SHADERPASS SHADERPASS_FORWARD_EMISSIVE_PROJECTOR
        #define _MATERIAL_AFFECTS_ALBEDO 1
        #define _MATERIAL_AFFECTS_NORMAL 1
        #define _MATERIAL_AFFECTS_NORMAL_BLEND 1
        #define _MATERIAL_AFFECTS_EMISSION 1
        
            // HybridV1InjectedBuiltinProperties: <None>
        
            // -- Properties used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DecalInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderVariablesDecal.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
        
            // --------------------------------------------------
            // Structs and Packing
        
            struct Attributes
        {
             float3 positionOS : POSITION;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 interp0 : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyzw =  input.texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.interp0.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float4 _DissolveTex_TexelSize;
        float _GroundResidue;
        float4 _RColor;
        float _RSize;
        float _RRotate;
        float _RLight;
        float4 _RDissolveColor;
        float _RLightGain;
        float _RDissolveIn;
        float _RDissolveOut;
        float4 _DissolveTex_ST;
        float4 _MainTex_ST;
        float4 _GColor;
        float _GSize;
        float _GRotate;
        float _GLight;
        float _GLightGain;
        float4 _GDissolveColor;
        float _GDissolveIn;
        float _GDissolveOut;
        float4 _BColor;
        float _BSize;
        float _BRotate;
        float _BLight;
        float _BLightGain;
        float4 _BDissolveColor;
        float _BDissolveIn;
        float _BDissolveOut;
        float _DrawOrder;
        float _DecalMeshBiasType;
        float _DecalMeshDepthBias;
        float _DecalMeshViewBias;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_DissolveTex);
        SAMPLER(sampler_DissolveTex);
        
            // Graph Functions
            
        void RotateSize_float(float4 _uv, float _size, float _rotate, out float4 Out){
                _rotate = _rotate*(3.14159265359/180);
            
                float2 ruv = float2(((_uv.x - 0.5) * 1/_size) + 0.5 ,((_uv.y - 0.5) * 1/_size) + 0.5);
            
                ruv = ruv - float2(0.5, 0.5);  
            
                ruv = float2(ruv.x * cos(_rotate) - ruv.y * sin(_rotate),ruv.x * sin(_rotate) + ruv.y * cos(_rotate));  
            
                ruv += float2(0.5, 0.5);
            
            
            
                Out = float4(ruv.x,ruv.y,_uv.z,_uv.w);
            
        }
        
        void DTexSet_float(float4 _uvD, float4 _DissolveTex_ST, out float4 Out){
             Out = float4((_uvD.x- 0.5) * _DissolveTex_ST.x+ _DissolveTex_ST.z,(_uvD.y- 0.5) * _DissolveTex_ST.y+ _DissolveTex_ST.w,_uvD.z,_uvD.w);
        }
        
        void TextureLight_float(float4 _tex, float _light, float4 _color, float _lightGain, out float4 Out){
                float4 RtexB = smoothstep( 1-_light,1,_tex);
            
                RtexB = RtexB * 0.2 + RtexB *  _color * _lightGain;
            
                _tex.rgb = _tex.rgb * _color.rgb + RtexB.rgb;
            
                _tex = _tex * _color.a *_tex.a ;
            
                Out = _tex;
        }
        
        void Dissolve_float(float2 _uv, float4 _Dtex, float _DissolveIn, float _DissolveOut, float4 _Color, float4 _tex, out float4 Out){
                float4 spheric = saturate( distance( _uv ,float2(0.5,0.5)) );
            
                spheric =saturate(spheric + spheric * _Dtex);
            
                           
            
                float4 spheric01A =saturate( saturate( (spheric + lerp(-1.01,0.01,1-_DissolveIn ))*100));
            
                float4 spheric01B = saturate( (spheric + lerp(-1.01,0.01,1-_DissolveIn- 0.01))*100);
            
                spheric01B = spheric01A * (1-spheric01B) * _Color * _Color.a;
            
            
            
                _tex = _tex * spheric01A + spheric01B * _tex.a;
            
            
            
                float4 spheric02A = saturate( ((1-spheric) + lerp(-1.01,0.01,_DissolveOut+ 0.01))*100);
            
                float4 spheric02B = saturate( ((1-spheric) + lerp(-1.01,0.01,_DissolveOut))*100);
            
                spheric02B =spheric02A * (1-spheric02B) * _Color * _Color.a;
            
                
            
                _tex = _tex * spheric02A + spheric02B * _tex.a;
            
                Out = _tex;
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
        {
            RGBA = float4(R, G, B, A);
            RGB = float3(R, G, B);
            RG = float2(R, G);
        }
        
            // Graph Vertex
            struct VertexDescription
        {
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            return description;
        }
            
            // Graph Pixel
            struct SurfaceDescription
        {
            float Alpha;
            float3 Emission;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _UV_8cea45496b8c45eab3597a074d15d476_Out_0 = IN.uv0;
            float _Property_7b769f55c7864589aebfac4ddd12b2d6_Out_0 = _RSize;
            float _Property_4acdc5976b734662994ffee635158980_Out_0 = _RRotate;
            float4 _RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3;
            RotateSize_float(_UV_8cea45496b8c45eab3597a074d15d476_Out_0, _Property_7b769f55c7864589aebfac4ddd12b2d6_Out_0, _Property_4acdc5976b734662994ffee635158980_Out_0, _RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3);
            UnityTexture2D _Property_eafddcee5f7f4f66817085ef749c6b35_Out_0 = UnityBuildTexture2DStructNoScale(_DissolveTex);
            float4 _Property_1b4f5806053747519e92207b6d7afbee_Out_0 = _DissolveTex_ST;
            float4 _DTexSetCustomFunction_b0fd3e7232bf4e08ab5d88555c1b5756_Out_3;
            DTexSet_float(_RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3, _Property_1b4f5806053747519e92207b6d7afbee_Out_0, _DTexSetCustomFunction_b0fd3e7232bf4e08ab5d88555c1b5756_Out_3);
            float4 _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0 = SAMPLE_TEXTURE2D(_Property_eafddcee5f7f4f66817085ef749c6b35_Out_0.tex, _Property_eafddcee5f7f4f66817085ef749c6b35_Out_0.samplerstate, _Property_eafddcee5f7f4f66817085ef749c6b35_Out_0.GetTransformedUV((_DTexSetCustomFunction_b0fd3e7232bf4e08ab5d88555c1b5756_Out_3.xy)));
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_R_4 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.r;
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_G_5 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.g;
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_B_6 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.b;
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_A_7 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.a;
            float _Property_a077d5d9cbf747f5875c6c24513b2800_Out_0 = _RDissolveIn;
            float _Property_0ff262777af74848aa81b3dba71541ad_Out_0 = _RDissolveOut;
            float4 _Property_7368f6d6acbe464fb71b082ec611715e_Out_0 = IsGammaSpace() ? LinearToSRGB(_RDissolveColor) : _RDissolveColor;
            UnityTexture2D _Property_c21f55442e394cf284a0766460ec9cac_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0 = SAMPLE_TEXTURE2D(_Property_c21f55442e394cf284a0766460ec9cac_Out_0.tex, _Property_c21f55442e394cf284a0766460ec9cac_Out_0.samplerstate, _Property_c21f55442e394cf284a0766460ec9cac_Out_0.GetTransformedUV((_RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3.xy)));
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_R_4 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.r;
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_G_5 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.g;
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_B_6 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.b;
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_A_7 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.a;
            float4 _Swizzle_400506fb40164c489617df53e573832a_Out_1 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_R_4.xxxx;
            float _Property_a5385ead93534fa7aa7ee63377c08787_Out_0 = _RLight;
            float4 _Property_d4de088caa5d43b99fa6421c07169156_Out_0 = IsGammaSpace() ? LinearToSRGB(_RColor) : _RColor;
            float _Property_d8081504f4364890a7d4a0f3971be926_Out_0 = _RLightGain;
            float4 _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3;
            TextureLight_float(_Swizzle_400506fb40164c489617df53e573832a_Out_1, _Property_a5385ead93534fa7aa7ee63377c08787_Out_0, _Property_d4de088caa5d43b99fa6421c07169156_Out_0, _Property_d8081504f4364890a7d4a0f3971be926_Out_0, _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3);
            float4 _DissolveCustomFunction_c1278429f28948e08cf996a8b62479a9_Out_3;
            Dissolve_float((_RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3.xy), _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0, _Property_a077d5d9cbf747f5875c6c24513b2800_Out_0, _Property_0ff262777af74848aa81b3dba71541ad_Out_0, _Property_7368f6d6acbe464fb71b082ec611715e_Out_0, _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3, _DissolveCustomFunction_c1278429f28948e08cf996a8b62479a9_Out_3);
            float _Property_3808652b3bd84f0dae18b3e1973a9e4b_Out_0 = _GSize;
            float _Property_4618d5267616447a8c6b7427eb1a57dd_Out_0 = _GRotate;
            float4 _RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3;
            RotateSize_float(_UV_8cea45496b8c45eab3597a074d15d476_Out_0, _Property_3808652b3bd84f0dae18b3e1973a9e4b_Out_0, _Property_4618d5267616447a8c6b7427eb1a57dd_Out_0, _RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3);
            UnityTexture2D _Property_d018d28ee214437a82076451743a8130_Out_0 = UnityBuildTexture2DStructNoScale(_DissolveTex);
            float4 _Property_b4cc761c375e4bf5b21dd8dcf9704990_Out_0 = _DissolveTex_ST;
            float4 _DTexSetCustomFunction_65a258090b234db4ab663709ae4ac4d4_Out_3;
            DTexSet_float(_RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3, _Property_b4cc761c375e4bf5b21dd8dcf9704990_Out_0, _DTexSetCustomFunction_65a258090b234db4ab663709ae4ac4d4_Out_3);
            float4 _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0 = SAMPLE_TEXTURE2D(_Property_d018d28ee214437a82076451743a8130_Out_0.tex, _Property_d018d28ee214437a82076451743a8130_Out_0.samplerstate, _Property_d018d28ee214437a82076451743a8130_Out_0.GetTransformedUV((_DTexSetCustomFunction_65a258090b234db4ab663709ae4ac4d4_Out_3.xy)));
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_R_4 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.r;
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_G_5 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.g;
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_B_6 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.b;
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_A_7 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.a;
            float _Property_6e040bc940b740d3954fb5ef6f7965de_Out_0 = _GDissolveIn;
            float _Property_6dce9fc004da445ab2b9bebb19d43137_Out_0 = _GDissolveOut;
            float4 _Property_5863bb3472fe4bf1800650465375edcd_Out_0 = IsGammaSpace() ? LinearToSRGB(_GDissolveColor) : _GDissolveColor;
            UnityTexture2D _Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0.tex, _Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0.samplerstate, _Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0.GetTransformedUV((_RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3.xy)));
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_R_4 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.r;
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_G_5 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.g;
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_B_6 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.b;
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_A_7 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.a;
            float4 _Swizzle_c89196372af64bba9bf916bb81898cc9_Out_1 = _SampleTexture2D_cddf3472524741888ba91b342532777d_G_5.xxxx;
            float _Property_07a37b638f724de594b4970af7e3d22f_Out_0 = _GLight;
            float4 _Property_d7f27dbc94dd4dbca6685d8c913bff10_Out_0 = IsGammaSpace() ? LinearToSRGB(_GColor) : _GColor;
            float _Property_a2e2d040f3874da48daa383fde697713_Out_0 = _GLightGain;
            float4 _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3;
            TextureLight_float(_Swizzle_c89196372af64bba9bf916bb81898cc9_Out_1, _Property_07a37b638f724de594b4970af7e3d22f_Out_0, _Property_d7f27dbc94dd4dbca6685d8c913bff10_Out_0, _Property_a2e2d040f3874da48daa383fde697713_Out_0, _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3);
            float4 _DissolveCustomFunction_c794717a85c44b778d53446bc5599b11_Out_3;
            Dissolve_float((_RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3.xy), _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0, _Property_6e040bc940b740d3954fb5ef6f7965de_Out_0, _Property_6dce9fc004da445ab2b9bebb19d43137_Out_0, _Property_5863bb3472fe4bf1800650465375edcd_Out_0, _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3, _DissolveCustomFunction_c794717a85c44b778d53446bc5599b11_Out_3);
            float _Property_8fb0e68fd07a405b921fa2706f8ae7f4_Out_0 = _BSize;
            float _Property_6040d22195bf4819ae19ecc7e8841069_Out_0 = _BRotate;
            float4 _RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3;
            RotateSize_float(_UV_8cea45496b8c45eab3597a074d15d476_Out_0, _Property_8fb0e68fd07a405b921fa2706f8ae7f4_Out_0, _Property_6040d22195bf4819ae19ecc7e8841069_Out_0, _RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3);
            UnityTexture2D _Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0 = UnityBuildTexture2DStructNoScale(_DissolveTex);
            float4 _Property_53bc3e203cdf45779cf450bcccc43ff8_Out_0 = _DissolveTex_ST;
            float4 _DTexSetCustomFunction_e8155feacdd943ddb8f4ff867df48696_Out_3;
            DTexSet_float(_RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3, _Property_53bc3e203cdf45779cf450bcccc43ff8_Out_0, _DTexSetCustomFunction_e8155feacdd943ddb8f4ff867df48696_Out_3);
            float4 _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0 = SAMPLE_TEXTURE2D(_Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0.tex, _Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0.samplerstate, _Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0.GetTransformedUV((_DTexSetCustomFunction_e8155feacdd943ddb8f4ff867df48696_Out_3.xy)));
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_R_4 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.r;
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_G_5 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.g;
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_B_6 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.b;
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_A_7 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.a;
            float _Property_df06dff0c2484fb595c137b1754753b9_Out_0 = _BDissolveIn;
            float _Property_d214b0d6c4864bde9efaa3db6db55a86_Out_0 = _BDissolveOut;
            float4 _Property_f2164c96cf7945fd975240ed7e800b18_Out_0 = IsGammaSpace() ? LinearToSRGB(_BDissolveColor) : _BDissolveColor;
            UnityTexture2D _Property_d2937aa927074faf8a7e3502b5f57de2_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0 = SAMPLE_TEXTURE2D(_Property_d2937aa927074faf8a7e3502b5f57de2_Out_0.tex, _Property_d2937aa927074faf8a7e3502b5f57de2_Out_0.samplerstate, _Property_d2937aa927074faf8a7e3502b5f57de2_Out_0.GetTransformedUV((_RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3.xy)));
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_R_4 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.r;
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_G_5 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.g;
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_B_6 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.b;
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_A_7 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.a;
            float4 _Swizzle_f28776abd40e4239a0500f742ca08953_Out_1 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_B_6.xxxx;
            float _Property_ff648668c0b14c5bb553df8ea7ab4c99_Out_0 = _BLight;
            float4 _Property_f2d51e21017b4d62854d0dd0d69a4eeb_Out_0 = IsGammaSpace() ? LinearToSRGB(_BColor) : _BColor;
            float _Property_2358423daf2b46bd88fc2c6f83681dab_Out_0 = _BLightGain;
            float4 _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3;
            TextureLight_float(_Swizzle_f28776abd40e4239a0500f742ca08953_Out_1, _Property_ff648668c0b14c5bb553df8ea7ab4c99_Out_0, _Property_f2d51e21017b4d62854d0dd0d69a4eeb_Out_0, _Property_2358423daf2b46bd88fc2c6f83681dab_Out_0, _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3);
            float4 _DissolveCustomFunction_18ec96fae3434badafbd66e66f5f7fc7_Out_3;
            Dissolve_float((_RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3.xy), _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0, _Property_df06dff0c2484fb595c137b1754753b9_Out_0, _Property_d214b0d6c4864bde9efaa3db6db55a86_Out_0, _Property_f2164c96cf7945fd975240ed7e800b18_Out_0, _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3, _DissolveCustomFunction_18ec96fae3434badafbd66e66f5f7fc7_Out_3);
            float4 _Add_b5aafd4ee22e4670a9b2a0eeae0b991b_Out_2;
            Unity_Add_float4(_DissolveCustomFunction_c794717a85c44b778d53446bc5599b11_Out_3, _DissolveCustomFunction_18ec96fae3434badafbd66e66f5f7fc7_Out_3, _Add_b5aafd4ee22e4670a9b2a0eeae0b991b_Out_2);
            float4 _Add_311c3433751947a9b670b04484a11b3b_Out_2;
            Unity_Add_float4(_DissolveCustomFunction_c1278429f28948e08cf996a8b62479a9_Out_3, _Add_b5aafd4ee22e4670a9b2a0eeae0b991b_Out_2, _Add_311c3433751947a9b670b04484a11b3b_Out_2);
            float _Swizzle_19e7c60d34834501ad98d57eecf88a20_Out_1 = _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3.w;
            float _Swizzle_2b2f7ab3dfd748fbb705d617d01b1690_Out_1 = _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3.w;
            float _Swizzle_d4f48dddbcae41cfbf129451e203ecf4_Out_1 = _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3.w;
            float _Add_d05af43285cb4cce8638140598473902_Out_2;
            Unity_Add_float(_Swizzle_2b2f7ab3dfd748fbb705d617d01b1690_Out_1, _Swizzle_d4f48dddbcae41cfbf129451e203ecf4_Out_1, _Add_d05af43285cb4cce8638140598473902_Out_2);
            float _Add_747b1754f01448a9bb57f2993c19791e_Out_2;
            Unity_Add_float(_Swizzle_19e7c60d34834501ad98d57eecf88a20_Out_1, _Add_d05af43285cb4cce8638140598473902_Out_2, _Add_747b1754f01448a9bb57f2993c19791e_Out_2);
            float4 _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGBA_4;
            float3 _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGB_5;
            float2 _Combine_94b4ca9170e44d93bdf05a483d08c44b_RG_6;
            Unity_Combine_float(0, 0, 0, _Add_747b1754f01448a9bb57f2993c19791e_Out_2, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGBA_4, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGB_5, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RG_6);
            float4 _Add_2670f6bd6b3f4beb94a86259f697e3cf_Out_2;
            Unity_Add_float4(_Add_311c3433751947a9b670b04484a11b3b_Out_2, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGBA_4, _Add_2670f6bd6b3f4beb94a86259f697e3cf_Out_2);
            float _Swizzle_6cead8bc7a22455b93e7f26073d331da_Out_1 = _Add_2670f6bd6b3f4beb94a86259f697e3cf_Out_2.w;
            surface.Alpha = _Swizzle_6cead8bc7a22455b93e7f26073d331da_Out_1;
            surface.Emission = (_Add_2670f6bd6b3f4beb94a86259f697e3cf_Out_2.xyz);
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            
        //     $features.graphVertex:  $include("VertexAnimation.template.hlsl")
        //                                       ^ ERROR: $include cannot find file : VertexAnimation.template.hlsl. Looked into:
        // Packages/com.unity.shadergraph/Editor/Generation/Templates
        
            
        //     $features.graphPixel:   $include("SharedCode.template.hlsl")
        //                                       ^ ERROR: $include cannot find file : SharedCode.template.hlsl. Looked into:
        // Packages/com.unity.shadergraph/Editor/Generation/Templates
        
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorCopyToSDI' */
        
        
        
        
        
            output.uv0 =                                        input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
            return output;
        }
        
            // --------------------------------------------------
            // Build Surface Data
        
            uint2 ComputeFadeMaskSeed(uint2 positionSS)
            {
                uint2 fadeMaskSeed;
        
                // Can't use the view direction, it is the same across the entire screen.
                fadeMaskSeed = positionSS;
        
                return fadeMaskSeed;
            }
        
            void GetSurfaceData(Varyings input, half3 viewDirectioWS, uint2 positionSS, float angleFadeFactor, out DecalSurfaceData surfaceData)
            {
                #if (SHADERPASS == SHADERPASS_DBUFFER_PROJECTOR) || (SHADERPASS == SHADERPASS_FORWARD_EMISSIVE_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_SCREEN_SPACE_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_GBUFFER_PROJECTOR)
                    half4x4 normalToWorld = UNITY_ACCESS_INSTANCED_PROP(Decal, _NormalToWorld);
                    half fadeFactor = clamp(normalToWorld[0][3], 0.0f, 1.0f) * angleFadeFactor;
                    float2 scale = float2(normalToWorld[3][0], normalToWorld[3][1]);
                    float2 offset = float2(normalToWorld[3][2], normalToWorld[3][3]);
                    input.texCoord0.xy = input.texCoord0.xy * scale + offset;
                #else
                    #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                        LODDitheringTransition(ComputeFadeMaskSeed(positionSS), unity_LODFade.x);
                    #endif
        
                    half fadeFactor = half(1.0);
                #endif
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = BuildSurfaceDescriptionInputs(input);
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
        
                // setup defaults -- these are used if the graph doesn't output a value
                ZERO_INITIALIZE(DecalSurfaceData, surfaceData);
                surfaceData.occlusion = half(1.0);
                surfaceData.smoothness = half(0);
        
                #ifdef _MATERIAL_AFFECTS_NORMAL
                    surfaceData.normalWS.w = half(1.0);
                #else
                    surfaceData.normalWS.w = half(0.0);
                #endif
        
                surfaceData.emissive.rgb = half3(surfaceDescription.Emission.rgb * fadeFactor);
        
                // copy across graph values, if defined
                surfaceData.baseColor.w = half(surfaceDescription.Alpha * fadeFactor);
        
                #if (SHADERPASS == SHADERPASS_DBUFFER_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_SCREEN_SPACE_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_GBUFFER_PROJECTOR)
                    #if defined(_MATERIAL_AFFECTS_NORMAL)
                    #else
                        surfaceData.normalWS.xyz = normalToWorld[2].xyz;
                    #endif
                #elif (SHADERPASS == SHADERPASS_DBUFFER_MESH) || (SHADERPASS == SHADERPASS_DECAL_SCREEN_SPACE_MESH) || (SHADERPASS == SHADERPASS_DECAL_GBUFFER_MESH)
                    #if defined(_MATERIAL_AFFECTS_NORMAL)
                        float sgn = input.tangentWS.w;      // should be either +1 or -1
                        float3 bitangent = sgn * cross(input.normalWS.xyz, input.tangentWS.xyz);
                        half3x3 tangentToWorld = half3x3(input.tangentWS.xyz, bitangent.xyz, input.normalWS.xyz);
        
                        // We need to normalize as we use mikkt tangent space and this is expected (tangent space is not normalize)
                    #else
                        surfaceData.normalWS.xyz = half3(input.normalWS); // Default to vertex normal
                    #endif
                #endif
        
        
                // In case of Smoothness / AO / Metal, all the three are always computed but color mask can change
            }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPassDecal.hlsl"
        
            ENDHLSL
        }
        Pass
        { 
            Name "DecalScreenSpaceProjector"
            Tags 
            { 
                "LightMode" = "DecalScreenSpaceProjector"
            }
        
            // Render State
            Cull Front
        Blend SrcAlpha OneMinusSrcAlpha
        ZTest Greater
        ZWrite Off
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma target 2.5
        #pragma vertex Vert
        #pragma fragment Frag
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        
            // Keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
        #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
        #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
        #pragma multi_compile_fragment _ _SHADOWS_SOFT
        #pragma multi_compile _ _CLUSTERED_RENDERING
        #pragma multi_compile _DECAL_NORMAL_BLEND_LOW _DECAL_NORMAL_BLEND_MEDIUM _DECAL_NORMAL_BLEND_HIGH
            // GraphKeywords: <None>
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        
            // Defines
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_VIEWDIRECTION_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
            #define VARYINGS_NEED_SH
            #define VARYINGS_NEED_STATIC_LIGHTMAP_UV
            #define VARYINGS_NEED_DYNAMIC_LIGHTMAP_UV
            
            #define HAVE_MESH_MODIFICATION
        
        
            #define SHADERPASS SHADERPASS_DECAL_SCREEN_SPACE_PROJECTOR
        #define _MATERIAL_AFFECTS_ALBEDO 1
        #define _MATERIAL_AFFECTS_NORMAL 1
        #define _MATERIAL_AFFECTS_NORMAL_BLEND 1
        #define _MATERIAL_AFFECTS_EMISSION 1
        
            // HybridV1InjectedBuiltinProperties: <None>
        
            // -- Properties used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DecalInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderVariablesDecal.hlsl"
        
            // --------------------------------------------------
            // Structs and Packing
        
            struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 normalWS;
             float4 texCoord0;
             float3 viewDirectionWS;
            #if defined(LIGHTMAP_ON)
             float2 staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
             float2 dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
             float3 sh;
            #endif
             float4 fogFactorAndVertexLight;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 TangentSpaceNormal;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float4 interp1 : INTERP1;
             float3 interp2 : INTERP2;
             float2 interp3 : INTERP3;
             float2 interp4 : INTERP4;
             float3 interp5 : INTERP5;
             float4 interp6 : INTERP6;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.normalWS;
            output.interp1.xyzw =  input.texCoord0;
            output.interp2.xyz =  input.viewDirectionWS;
            #if defined(LIGHTMAP_ON)
            output.interp3.xy =  input.staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            output.interp4.xy =  input.dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.interp5.xyz =  input.sh;
            #endif
            output.interp6.xyzw =  input.fogFactorAndVertexLight;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.normalWS = input.interp0.xyz;
            output.texCoord0 = input.interp1.xyzw;
            output.viewDirectionWS = input.interp2.xyz;
            #if defined(LIGHTMAP_ON)
            output.staticLightmapUV = input.interp3.xy;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            output.dynamicLightmapUV = input.interp4.xy;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.sh = input.interp5.xyz;
            #endif
            output.fogFactorAndVertexLight = input.interp6.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float4 _DissolveTex_TexelSize;
        float _GroundResidue;
        float4 _RColor;
        float _RSize;
        float _RRotate;
        float _RLight;
        float4 _RDissolveColor;
        float _RLightGain;
        float _RDissolveIn;
        float _RDissolveOut;
        float4 _DissolveTex_ST;
        float4 _MainTex_ST;
        float4 _GColor;
        float _GSize;
        float _GRotate;
        float _GLight;
        float _GLightGain;
        float4 _GDissolveColor;
        float _GDissolveIn;
        float _GDissolveOut;
        float4 _BColor;
        float _BSize;
        float _BRotate;
        float _BLight;
        float _BLightGain;
        float4 _BDissolveColor;
        float _BDissolveIn;
        float _BDissolveOut;
        float _DrawOrder;
        float _DecalMeshBiasType;
        float _DecalMeshDepthBias;
        float _DecalMeshViewBias;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_DissolveTex);
        SAMPLER(sampler_DissolveTex);
        
            // Graph Functions
            
        void RotateSize_float(float4 _uv, float _size, float _rotate, out float4 Out){
                _rotate = _rotate*(3.14159265359/180);
            
                float2 ruv = float2(((_uv.x - 0.5) * 1/_size) + 0.5 ,((_uv.y - 0.5) * 1/_size) + 0.5);
            
                ruv = ruv - float2(0.5, 0.5);  
            
                ruv = float2(ruv.x * cos(_rotate) - ruv.y * sin(_rotate),ruv.x * sin(_rotate) + ruv.y * cos(_rotate));  
            
                ruv += float2(0.5, 0.5);
            
            
            
                Out = float4(ruv.x,ruv.y,_uv.z,_uv.w);
            
        }
        
        void DTexSet_float(float4 _uvD, float4 _DissolveTex_ST, out float4 Out){
             Out = float4((_uvD.x- 0.5) * _DissolveTex_ST.x+ _DissolveTex_ST.z,(_uvD.y- 0.5) * _DissolveTex_ST.y+ _DissolveTex_ST.w,_uvD.z,_uvD.w);
        }
        
        void TextureLight_float(float4 _tex, float _light, float4 _color, float _lightGain, out float4 Out){
                float4 RtexB = smoothstep( 1-_light,1,_tex);
            
                RtexB = RtexB * 0.2 + RtexB *  _color * _lightGain;
            
                _tex.rgb = _tex.rgb * _color.rgb + RtexB.rgb;
            
                _tex = _tex * _color.a *_tex.a ;
            
                Out = _tex;
        }
        
        void Dissolve_float(float2 _uv, float4 _Dtex, float _DissolveIn, float _DissolveOut, float4 _Color, float4 _tex, out float4 Out){
                float4 spheric = saturate( distance( _uv ,float2(0.5,0.5)) );
            
                spheric =saturate(spheric + spheric * _Dtex);
            
                           
            
                float4 spheric01A =saturate( saturate( (spheric + lerp(-1.01,0.01,1-_DissolveIn ))*100));
            
                float4 spheric01B = saturate( (spheric + lerp(-1.01,0.01,1-_DissolveIn- 0.01))*100);
            
                spheric01B = spheric01A * (1-spheric01B) * _Color * _Color.a;
            
            
            
                _tex = _tex * spheric01A + spheric01B * _tex.a;
            
            
            
                float4 spheric02A = saturate( ((1-spheric) + lerp(-1.01,0.01,_DissolveOut+ 0.01))*100);
            
                float4 spheric02B = saturate( ((1-spheric) + lerp(-1.01,0.01,_DissolveOut))*100);
            
                spheric02B =spheric02A * (1-spheric02B) * _Color * _Color.a;
            
                
            
                _tex = _tex * spheric02A + spheric02B * _tex.a;
            
                Out = _tex;
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
        {
            RGBA = float4(R, G, B, A);
            RGB = float3(R, G, B);
            RG = float2(R, G);
        }
        
            // Graph Vertex
            struct VertexDescription
        {
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            return description;
        }
            
            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float3 NormalTS;
            float NormalAlpha;
            float3 Emission;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _UV_8cea45496b8c45eab3597a074d15d476_Out_0 = IN.uv0;
            float _Property_7b769f55c7864589aebfac4ddd12b2d6_Out_0 = _RSize;
            float _Property_4acdc5976b734662994ffee635158980_Out_0 = _RRotate;
            float4 _RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3;
            RotateSize_float(_UV_8cea45496b8c45eab3597a074d15d476_Out_0, _Property_7b769f55c7864589aebfac4ddd12b2d6_Out_0, _Property_4acdc5976b734662994ffee635158980_Out_0, _RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3);
            UnityTexture2D _Property_eafddcee5f7f4f66817085ef749c6b35_Out_0 = UnityBuildTexture2DStructNoScale(_DissolveTex);
            float4 _Property_1b4f5806053747519e92207b6d7afbee_Out_0 = _DissolveTex_ST;
            float4 _DTexSetCustomFunction_b0fd3e7232bf4e08ab5d88555c1b5756_Out_3;
            DTexSet_float(_RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3, _Property_1b4f5806053747519e92207b6d7afbee_Out_0, _DTexSetCustomFunction_b0fd3e7232bf4e08ab5d88555c1b5756_Out_3);
            float4 _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0 = SAMPLE_TEXTURE2D(_Property_eafddcee5f7f4f66817085ef749c6b35_Out_0.tex, _Property_eafddcee5f7f4f66817085ef749c6b35_Out_0.samplerstate, _Property_eafddcee5f7f4f66817085ef749c6b35_Out_0.GetTransformedUV((_DTexSetCustomFunction_b0fd3e7232bf4e08ab5d88555c1b5756_Out_3.xy)));
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_R_4 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.r;
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_G_5 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.g;
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_B_6 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.b;
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_A_7 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.a;
            float _Property_a077d5d9cbf747f5875c6c24513b2800_Out_0 = _RDissolveIn;
            float _Property_0ff262777af74848aa81b3dba71541ad_Out_0 = _RDissolveOut;
            float4 _Property_7368f6d6acbe464fb71b082ec611715e_Out_0 = IsGammaSpace() ? LinearToSRGB(_RDissolveColor) : _RDissolveColor;
            UnityTexture2D _Property_c21f55442e394cf284a0766460ec9cac_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0 = SAMPLE_TEXTURE2D(_Property_c21f55442e394cf284a0766460ec9cac_Out_0.tex, _Property_c21f55442e394cf284a0766460ec9cac_Out_0.samplerstate, _Property_c21f55442e394cf284a0766460ec9cac_Out_0.GetTransformedUV((_RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3.xy)));
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_R_4 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.r;
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_G_5 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.g;
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_B_6 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.b;
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_A_7 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.a;
            float4 _Swizzle_400506fb40164c489617df53e573832a_Out_1 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_R_4.xxxx;
            float _Property_a5385ead93534fa7aa7ee63377c08787_Out_0 = _RLight;
            float4 _Property_d4de088caa5d43b99fa6421c07169156_Out_0 = IsGammaSpace() ? LinearToSRGB(_RColor) : _RColor;
            float _Property_d8081504f4364890a7d4a0f3971be926_Out_0 = _RLightGain;
            float4 _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3;
            TextureLight_float(_Swizzle_400506fb40164c489617df53e573832a_Out_1, _Property_a5385ead93534fa7aa7ee63377c08787_Out_0, _Property_d4de088caa5d43b99fa6421c07169156_Out_0, _Property_d8081504f4364890a7d4a0f3971be926_Out_0, _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3);
            float4 _DissolveCustomFunction_c1278429f28948e08cf996a8b62479a9_Out_3;
            Dissolve_float((_RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3.xy), _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0, _Property_a077d5d9cbf747f5875c6c24513b2800_Out_0, _Property_0ff262777af74848aa81b3dba71541ad_Out_0, _Property_7368f6d6acbe464fb71b082ec611715e_Out_0, _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3, _DissolveCustomFunction_c1278429f28948e08cf996a8b62479a9_Out_3);
            float _Property_3808652b3bd84f0dae18b3e1973a9e4b_Out_0 = _GSize;
            float _Property_4618d5267616447a8c6b7427eb1a57dd_Out_0 = _GRotate;
            float4 _RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3;
            RotateSize_float(_UV_8cea45496b8c45eab3597a074d15d476_Out_0, _Property_3808652b3bd84f0dae18b3e1973a9e4b_Out_0, _Property_4618d5267616447a8c6b7427eb1a57dd_Out_0, _RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3);
            UnityTexture2D _Property_d018d28ee214437a82076451743a8130_Out_0 = UnityBuildTexture2DStructNoScale(_DissolveTex);
            float4 _Property_b4cc761c375e4bf5b21dd8dcf9704990_Out_0 = _DissolveTex_ST;
            float4 _DTexSetCustomFunction_65a258090b234db4ab663709ae4ac4d4_Out_3;
            DTexSet_float(_RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3, _Property_b4cc761c375e4bf5b21dd8dcf9704990_Out_0, _DTexSetCustomFunction_65a258090b234db4ab663709ae4ac4d4_Out_3);
            float4 _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0 = SAMPLE_TEXTURE2D(_Property_d018d28ee214437a82076451743a8130_Out_0.tex, _Property_d018d28ee214437a82076451743a8130_Out_0.samplerstate, _Property_d018d28ee214437a82076451743a8130_Out_0.GetTransformedUV((_DTexSetCustomFunction_65a258090b234db4ab663709ae4ac4d4_Out_3.xy)));
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_R_4 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.r;
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_G_5 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.g;
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_B_6 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.b;
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_A_7 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.a;
            float _Property_6e040bc940b740d3954fb5ef6f7965de_Out_0 = _GDissolveIn;
            float _Property_6dce9fc004da445ab2b9bebb19d43137_Out_0 = _GDissolveOut;
            float4 _Property_5863bb3472fe4bf1800650465375edcd_Out_0 = IsGammaSpace() ? LinearToSRGB(_GDissolveColor) : _GDissolveColor;
            UnityTexture2D _Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0.tex, _Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0.samplerstate, _Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0.GetTransformedUV((_RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3.xy)));
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_R_4 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.r;
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_G_5 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.g;
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_B_6 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.b;
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_A_7 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.a;
            float4 _Swizzle_c89196372af64bba9bf916bb81898cc9_Out_1 = _SampleTexture2D_cddf3472524741888ba91b342532777d_G_5.xxxx;
            float _Property_07a37b638f724de594b4970af7e3d22f_Out_0 = _GLight;
            float4 _Property_d7f27dbc94dd4dbca6685d8c913bff10_Out_0 = IsGammaSpace() ? LinearToSRGB(_GColor) : _GColor;
            float _Property_a2e2d040f3874da48daa383fde697713_Out_0 = _GLightGain;
            float4 _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3;
            TextureLight_float(_Swizzle_c89196372af64bba9bf916bb81898cc9_Out_1, _Property_07a37b638f724de594b4970af7e3d22f_Out_0, _Property_d7f27dbc94dd4dbca6685d8c913bff10_Out_0, _Property_a2e2d040f3874da48daa383fde697713_Out_0, _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3);
            float4 _DissolveCustomFunction_c794717a85c44b778d53446bc5599b11_Out_3;
            Dissolve_float((_RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3.xy), _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0, _Property_6e040bc940b740d3954fb5ef6f7965de_Out_0, _Property_6dce9fc004da445ab2b9bebb19d43137_Out_0, _Property_5863bb3472fe4bf1800650465375edcd_Out_0, _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3, _DissolveCustomFunction_c794717a85c44b778d53446bc5599b11_Out_3);
            float _Property_8fb0e68fd07a405b921fa2706f8ae7f4_Out_0 = _BSize;
            float _Property_6040d22195bf4819ae19ecc7e8841069_Out_0 = _BRotate;
            float4 _RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3;
            RotateSize_float(_UV_8cea45496b8c45eab3597a074d15d476_Out_0, _Property_8fb0e68fd07a405b921fa2706f8ae7f4_Out_0, _Property_6040d22195bf4819ae19ecc7e8841069_Out_0, _RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3);
            UnityTexture2D _Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0 = UnityBuildTexture2DStructNoScale(_DissolveTex);
            float4 _Property_53bc3e203cdf45779cf450bcccc43ff8_Out_0 = _DissolveTex_ST;
            float4 _DTexSetCustomFunction_e8155feacdd943ddb8f4ff867df48696_Out_3;
            DTexSet_float(_RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3, _Property_53bc3e203cdf45779cf450bcccc43ff8_Out_0, _DTexSetCustomFunction_e8155feacdd943ddb8f4ff867df48696_Out_3);
            float4 _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0 = SAMPLE_TEXTURE2D(_Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0.tex, _Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0.samplerstate, _Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0.GetTransformedUV((_DTexSetCustomFunction_e8155feacdd943ddb8f4ff867df48696_Out_3.xy)));
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_R_4 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.r;
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_G_5 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.g;
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_B_6 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.b;
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_A_7 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.a;
            float _Property_df06dff0c2484fb595c137b1754753b9_Out_0 = _BDissolveIn;
            float _Property_d214b0d6c4864bde9efaa3db6db55a86_Out_0 = _BDissolveOut;
            float4 _Property_f2164c96cf7945fd975240ed7e800b18_Out_0 = IsGammaSpace() ? LinearToSRGB(_BDissolveColor) : _BDissolveColor;
            UnityTexture2D _Property_d2937aa927074faf8a7e3502b5f57de2_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0 = SAMPLE_TEXTURE2D(_Property_d2937aa927074faf8a7e3502b5f57de2_Out_0.tex, _Property_d2937aa927074faf8a7e3502b5f57de2_Out_0.samplerstate, _Property_d2937aa927074faf8a7e3502b5f57de2_Out_0.GetTransformedUV((_RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3.xy)));
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_R_4 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.r;
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_G_5 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.g;
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_B_6 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.b;
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_A_7 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.a;
            float4 _Swizzle_f28776abd40e4239a0500f742ca08953_Out_1 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_B_6.xxxx;
            float _Property_ff648668c0b14c5bb553df8ea7ab4c99_Out_0 = _BLight;
            float4 _Property_f2d51e21017b4d62854d0dd0d69a4eeb_Out_0 = IsGammaSpace() ? LinearToSRGB(_BColor) : _BColor;
            float _Property_2358423daf2b46bd88fc2c6f83681dab_Out_0 = _BLightGain;
            float4 _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3;
            TextureLight_float(_Swizzle_f28776abd40e4239a0500f742ca08953_Out_1, _Property_ff648668c0b14c5bb553df8ea7ab4c99_Out_0, _Property_f2d51e21017b4d62854d0dd0d69a4eeb_Out_0, _Property_2358423daf2b46bd88fc2c6f83681dab_Out_0, _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3);
            float4 _DissolveCustomFunction_18ec96fae3434badafbd66e66f5f7fc7_Out_3;
            Dissolve_float((_RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3.xy), _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0, _Property_df06dff0c2484fb595c137b1754753b9_Out_0, _Property_d214b0d6c4864bde9efaa3db6db55a86_Out_0, _Property_f2164c96cf7945fd975240ed7e800b18_Out_0, _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3, _DissolveCustomFunction_18ec96fae3434badafbd66e66f5f7fc7_Out_3);
            float4 _Add_b5aafd4ee22e4670a9b2a0eeae0b991b_Out_2;
            Unity_Add_float4(_DissolveCustomFunction_c794717a85c44b778d53446bc5599b11_Out_3, _DissolveCustomFunction_18ec96fae3434badafbd66e66f5f7fc7_Out_3, _Add_b5aafd4ee22e4670a9b2a0eeae0b991b_Out_2);
            float4 _Add_311c3433751947a9b670b04484a11b3b_Out_2;
            Unity_Add_float4(_DissolveCustomFunction_c1278429f28948e08cf996a8b62479a9_Out_3, _Add_b5aafd4ee22e4670a9b2a0eeae0b991b_Out_2, _Add_311c3433751947a9b670b04484a11b3b_Out_2);
            float _Swizzle_19e7c60d34834501ad98d57eecf88a20_Out_1 = _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3.w;
            float _Swizzle_2b2f7ab3dfd748fbb705d617d01b1690_Out_1 = _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3.w;
            float _Swizzle_d4f48dddbcae41cfbf129451e203ecf4_Out_1 = _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3.w;
            float _Add_d05af43285cb4cce8638140598473902_Out_2;
            Unity_Add_float(_Swizzle_2b2f7ab3dfd748fbb705d617d01b1690_Out_1, _Swizzle_d4f48dddbcae41cfbf129451e203ecf4_Out_1, _Add_d05af43285cb4cce8638140598473902_Out_2);
            float _Add_747b1754f01448a9bb57f2993c19791e_Out_2;
            Unity_Add_float(_Swizzle_19e7c60d34834501ad98d57eecf88a20_Out_1, _Add_d05af43285cb4cce8638140598473902_Out_2, _Add_747b1754f01448a9bb57f2993c19791e_Out_2);
            float4 _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGBA_4;
            float3 _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGB_5;
            float2 _Combine_94b4ca9170e44d93bdf05a483d08c44b_RG_6;
            Unity_Combine_float(0, 0, 0, _Add_747b1754f01448a9bb57f2993c19791e_Out_2, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGBA_4, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGB_5, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RG_6);
            float4 _Add_2670f6bd6b3f4beb94a86259f697e3cf_Out_2;
            Unity_Add_float4(_Add_311c3433751947a9b670b04484a11b3b_Out_2, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGBA_4, _Add_2670f6bd6b3f4beb94a86259f697e3cf_Out_2);
            float _Swizzle_6cead8bc7a22455b93e7f26073d331da_Out_1 = _Add_2670f6bd6b3f4beb94a86259f697e3cf_Out_2.w;
            surface.BaseColor = IsGammaSpace() ? float3(0, 0, 0) : SRGBToLinear(float3(0, 0, 0));
            surface.Alpha = _Swizzle_6cead8bc7a22455b93e7f26073d331da_Out_1;
            surface.NormalTS = IN.TangentSpaceNormal;
            surface.NormalAlpha = 1;
            surface.Emission = (_Add_2670f6bd6b3f4beb94a86259f697e3cf_Out_2.xyz);
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            
        //     $features.graphVertex:  $include("VertexAnimation.template.hlsl")
        //                                       ^ ERROR: $include cannot find file : VertexAnimation.template.hlsl. Looked into:
        // Packages/com.unity.shadergraph/Editor/Generation/Templates
        
            
        //     $features.graphPixel:   $include("SharedCode.template.hlsl")
        //                                       ^ ERROR: $include cannot find file : SharedCode.template.hlsl. Looked into:
        // Packages/com.unity.shadergraph/Editor/Generation/Templates
        
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorCopyToSDI' */
        
        
        
            output.TangentSpaceNormal =                         float3(0.0f, 0.0f, 1.0f);
        
        
            output.uv0 =                                        input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
            return output;
        }
        
            // --------------------------------------------------
            // Build Surface Data
        
            uint2 ComputeFadeMaskSeed(uint2 positionSS)
            {
                uint2 fadeMaskSeed;
        
                // Can't use the view direction, it is the same across the entire screen.
                fadeMaskSeed = positionSS;
        
                return fadeMaskSeed;
            }
        
            void GetSurfaceData(Varyings input, half3 viewDirectioWS, uint2 positionSS, float angleFadeFactor, out DecalSurfaceData surfaceData)
            {
                #if (SHADERPASS == SHADERPASS_DBUFFER_PROJECTOR) || (SHADERPASS == SHADERPASS_FORWARD_EMISSIVE_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_SCREEN_SPACE_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_GBUFFER_PROJECTOR)
                    half4x4 normalToWorld = UNITY_ACCESS_INSTANCED_PROP(Decal, _NormalToWorld);
                    half fadeFactor = clamp(normalToWorld[0][3], 0.0f, 1.0f) * angleFadeFactor;
                    float2 scale = float2(normalToWorld[3][0], normalToWorld[3][1]);
                    float2 offset = float2(normalToWorld[3][2], normalToWorld[3][3]);
                    input.texCoord0.xy = input.texCoord0.xy * scale + offset;
                #else
                    #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                        LODDitheringTransition(ComputeFadeMaskSeed(positionSS), unity_LODFade.x);
                    #endif
        
                    half fadeFactor = half(1.0);
                #endif
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = BuildSurfaceDescriptionInputs(input);
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
        
                // setup defaults -- these are used if the graph doesn't output a value
                ZERO_INITIALIZE(DecalSurfaceData, surfaceData);
                surfaceData.occlusion = half(1.0);
                surfaceData.smoothness = half(0);
        
                #ifdef _MATERIAL_AFFECTS_NORMAL
                    surfaceData.normalWS.w = half(1.0);
                #else
                    surfaceData.normalWS.w = half(0.0);
                #endif
        
                surfaceData.emissive.rgb = half3(surfaceDescription.Emission.rgb * fadeFactor);
        
                // copy across graph values, if defined
                surfaceData.baseColor.xyz = half3(surfaceDescription.BaseColor);
                surfaceData.baseColor.w = half(surfaceDescription.Alpha * fadeFactor);
        
                #if (SHADERPASS == SHADERPASS_DBUFFER_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_SCREEN_SPACE_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_GBUFFER_PROJECTOR)
                    #if defined(_MATERIAL_AFFECTS_NORMAL)
                        surfaceData.normalWS.xyz = mul((half3x3)normalToWorld, surfaceDescription.NormalTS.xyz);
                    #else
                        surfaceData.normalWS.xyz = normalToWorld[2].xyz;
                    #endif
                #elif (SHADERPASS == SHADERPASS_DBUFFER_MESH) || (SHADERPASS == SHADERPASS_DECAL_SCREEN_SPACE_MESH) || (SHADERPASS == SHADERPASS_DECAL_GBUFFER_MESH)
                    #if defined(_MATERIAL_AFFECTS_NORMAL)
                        float sgn = input.tangentWS.w;      // should be either +1 or -1
                        float3 bitangent = sgn * cross(input.normalWS.xyz, input.tangentWS.xyz);
                        half3x3 tangentToWorld = half3x3(input.tangentWS.xyz, bitangent.xyz, input.normalWS.xyz);
        
                        // We need to normalize as we use mikkt tangent space and this is expected (tangent space is not normalize)
                        surfaceData.normalWS.xyz = normalize(TransformTangentToWorld(surfaceDescription.NormalTS, tangentToWorld));
                    #else
                        surfaceData.normalWS.xyz = half3(input.normalWS); // Default to vertex normal
                    #endif
                #endif
        
                surfaceData.normalWS.w = surfaceDescription.NormalAlpha * fadeFactor;
        
                // In case of Smoothness / AO / Metal, all the three are always computed but color mask can change
            }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPassDecal.hlsl"
        
            ENDHLSL
        }
        Pass
        { 
            Name "DecalGBufferProjector"
            Tags 
            { 
                "LightMode" = "DecalGBufferProjector"
            }
        
            // Render State
            Cull Front
        Blend 0 SrcAlpha OneMinusSrcAlpha
        Blend 1 SrcAlpha OneMinusSrcAlpha
        Blend 2 SrcAlpha OneMinusSrcAlpha
        Blend 3 SrcAlpha OneMinusSrcAlpha
        ZTest Greater
        ZWrite Off
        ColorMask RGB
        ColorMask 0 1
        ColorMask RGB 2
        ColorMask RGB 3
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma target 3.5
        #pragma vertex Vert
        #pragma fragment Frag
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        
            // Keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
        #pragma multi_compile_fragment _ _SHADOWS_SOFT
        #pragma multi_compile _DECAL_NORMAL_BLEND_LOW _DECAL_NORMAL_BLEND_MEDIUM _DECAL_NORMAL_BLEND_HIGH
        #pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
            // GraphKeywords: <None>
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        
            // Defines
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_VIEWDIRECTION_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_SH
            #define VARYINGS_NEED_STATIC_LIGHTMAP_UV
            #define VARYINGS_NEED_DYNAMIC_LIGHTMAP_UV
            
            #define HAVE_MESH_MODIFICATION
        
        
            #define SHADERPASS SHADERPASS_DECAL_GBUFFER_PROJECTOR
        #define _MATERIAL_AFFECTS_ALBEDO 1
        #define _MATERIAL_AFFECTS_NORMAL 1
        #define _MATERIAL_AFFECTS_NORMAL_BLEND 1
        #define _MATERIAL_AFFECTS_EMISSION 1
        
            // HybridV1InjectedBuiltinProperties: <None>
        
            // -- Properties used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityGBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DecalInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderVariablesDecal.hlsl"
        
            // --------------------------------------------------
            // Structs and Packing
        
            struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 normalWS;
             float4 texCoord0;
             float3 viewDirectionWS;
            #if defined(LIGHTMAP_ON)
             float2 staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
             float2 dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
             float3 sh;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 TangentSpaceNormal;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float4 interp1 : INTERP1;
             float3 interp2 : INTERP2;
             float2 interp3 : INTERP3;
             float2 interp4 : INTERP4;
             float3 interp5 : INTERP5;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.normalWS;
            output.interp1.xyzw =  input.texCoord0;
            output.interp2.xyz =  input.viewDirectionWS;
            #if defined(LIGHTMAP_ON)
            output.interp3.xy =  input.staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            output.interp4.xy =  input.dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.interp5.xyz =  input.sh;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.normalWS = input.interp0.xyz;
            output.texCoord0 = input.interp1.xyzw;
            output.viewDirectionWS = input.interp2.xyz;
            #if defined(LIGHTMAP_ON)
            output.staticLightmapUV = input.interp3.xy;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            output.dynamicLightmapUV = input.interp4.xy;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.sh = input.interp5.xyz;
            #endif
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float4 _DissolveTex_TexelSize;
        float _GroundResidue;
        float4 _RColor;
        float _RSize;
        float _RRotate;
        float _RLight;
        float4 _RDissolveColor;
        float _RLightGain;
        float _RDissolveIn;
        float _RDissolveOut;
        float4 _DissolveTex_ST;
        float4 _MainTex_ST;
        float4 _GColor;
        float _GSize;
        float _GRotate;
        float _GLight;
        float _GLightGain;
        float4 _GDissolveColor;
        float _GDissolveIn;
        float _GDissolveOut;
        float4 _BColor;
        float _BSize;
        float _BRotate;
        float _BLight;
        float _BLightGain;
        float4 _BDissolveColor;
        float _BDissolveIn;
        float _BDissolveOut;
        float _DrawOrder;
        float _DecalMeshBiasType;
        float _DecalMeshDepthBias;
        float _DecalMeshViewBias;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_DissolveTex);
        SAMPLER(sampler_DissolveTex);
        
            // Graph Functions
            
        void RotateSize_float(float4 _uv, float _size, float _rotate, out float4 Out){
                _rotate = _rotate*(3.14159265359/180);
            
                float2 ruv = float2(((_uv.x - 0.5) * 1/_size) + 0.5 ,((_uv.y - 0.5) * 1/_size) + 0.5);
            
                ruv = ruv - float2(0.5, 0.5);  
            
                ruv = float2(ruv.x * cos(_rotate) - ruv.y * sin(_rotate),ruv.x * sin(_rotate) + ruv.y * cos(_rotate));  
            
                ruv += float2(0.5, 0.5);
            
            
            
                Out = float4(ruv.x,ruv.y,_uv.z,_uv.w);
            
        }
        
        void DTexSet_float(float4 _uvD, float4 _DissolveTex_ST, out float4 Out){
             Out = float4((_uvD.x- 0.5) * _DissolveTex_ST.x+ _DissolveTex_ST.z,(_uvD.y- 0.5) * _DissolveTex_ST.y+ _DissolveTex_ST.w,_uvD.z,_uvD.w);
        }
        
        void TextureLight_float(float4 _tex, float _light, float4 _color, float _lightGain, out float4 Out){
                float4 RtexB = smoothstep( 1-_light,1,_tex);
            
                RtexB = RtexB * 0.2 + RtexB *  _color * _lightGain;
            
                _tex.rgb = _tex.rgb * _color.rgb + RtexB.rgb;
            
                _tex = _tex * _color.a *_tex.a ;
            
                Out = _tex;
        }
        
        void Dissolve_float(float2 _uv, float4 _Dtex, float _DissolveIn, float _DissolveOut, float4 _Color, float4 _tex, out float4 Out){
                float4 spheric = saturate( distance( _uv ,float2(0.5,0.5)) );
            
                spheric =saturate(spheric + spheric * _Dtex);
            
                           
            
                float4 spheric01A =saturate( saturate( (spheric + lerp(-1.01,0.01,1-_DissolveIn ))*100));
            
                float4 spheric01B = saturate( (spheric + lerp(-1.01,0.01,1-_DissolveIn- 0.01))*100);
            
                spheric01B = spheric01A * (1-spheric01B) * _Color * _Color.a;
            
            
            
                _tex = _tex * spheric01A + spheric01B * _tex.a;
            
            
            
                float4 spheric02A = saturate( ((1-spheric) + lerp(-1.01,0.01,_DissolveOut+ 0.01))*100);
            
                float4 spheric02B = saturate( ((1-spheric) + lerp(-1.01,0.01,_DissolveOut))*100);
            
                spheric02B =spheric02A * (1-spheric02B) * _Color * _Color.a;
            
                
            
                _tex = _tex * spheric02A + spheric02B * _tex.a;
            
                Out = _tex;
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
        {
            RGBA = float4(R, G, B, A);
            RGB = float3(R, G, B);
            RG = float2(R, G);
        }
        
            // Graph Vertex
            struct VertexDescription
        {
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            return description;
        }
            
            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float3 NormalTS;
            float NormalAlpha;
            float3 Emission;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _UV_8cea45496b8c45eab3597a074d15d476_Out_0 = IN.uv0;
            float _Property_7b769f55c7864589aebfac4ddd12b2d6_Out_0 = _RSize;
            float _Property_4acdc5976b734662994ffee635158980_Out_0 = _RRotate;
            float4 _RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3;
            RotateSize_float(_UV_8cea45496b8c45eab3597a074d15d476_Out_0, _Property_7b769f55c7864589aebfac4ddd12b2d6_Out_0, _Property_4acdc5976b734662994ffee635158980_Out_0, _RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3);
            UnityTexture2D _Property_eafddcee5f7f4f66817085ef749c6b35_Out_0 = UnityBuildTexture2DStructNoScale(_DissolveTex);
            float4 _Property_1b4f5806053747519e92207b6d7afbee_Out_0 = _DissolveTex_ST;
            float4 _DTexSetCustomFunction_b0fd3e7232bf4e08ab5d88555c1b5756_Out_3;
            DTexSet_float(_RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3, _Property_1b4f5806053747519e92207b6d7afbee_Out_0, _DTexSetCustomFunction_b0fd3e7232bf4e08ab5d88555c1b5756_Out_3);
            float4 _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0 = SAMPLE_TEXTURE2D(_Property_eafddcee5f7f4f66817085ef749c6b35_Out_0.tex, _Property_eafddcee5f7f4f66817085ef749c6b35_Out_0.samplerstate, _Property_eafddcee5f7f4f66817085ef749c6b35_Out_0.GetTransformedUV((_DTexSetCustomFunction_b0fd3e7232bf4e08ab5d88555c1b5756_Out_3.xy)));
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_R_4 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.r;
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_G_5 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.g;
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_B_6 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.b;
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_A_7 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.a;
            float _Property_a077d5d9cbf747f5875c6c24513b2800_Out_0 = _RDissolveIn;
            float _Property_0ff262777af74848aa81b3dba71541ad_Out_0 = _RDissolveOut;
            float4 _Property_7368f6d6acbe464fb71b082ec611715e_Out_0 = IsGammaSpace() ? LinearToSRGB(_RDissolveColor) : _RDissolveColor;
            UnityTexture2D _Property_c21f55442e394cf284a0766460ec9cac_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0 = SAMPLE_TEXTURE2D(_Property_c21f55442e394cf284a0766460ec9cac_Out_0.tex, _Property_c21f55442e394cf284a0766460ec9cac_Out_0.samplerstate, _Property_c21f55442e394cf284a0766460ec9cac_Out_0.GetTransformedUV((_RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3.xy)));
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_R_4 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.r;
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_G_5 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.g;
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_B_6 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.b;
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_A_7 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.a;
            float4 _Swizzle_400506fb40164c489617df53e573832a_Out_1 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_R_4.xxxx;
            float _Property_a5385ead93534fa7aa7ee63377c08787_Out_0 = _RLight;
            float4 _Property_d4de088caa5d43b99fa6421c07169156_Out_0 = IsGammaSpace() ? LinearToSRGB(_RColor) : _RColor;
            float _Property_d8081504f4364890a7d4a0f3971be926_Out_0 = _RLightGain;
            float4 _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3;
            TextureLight_float(_Swizzle_400506fb40164c489617df53e573832a_Out_1, _Property_a5385ead93534fa7aa7ee63377c08787_Out_0, _Property_d4de088caa5d43b99fa6421c07169156_Out_0, _Property_d8081504f4364890a7d4a0f3971be926_Out_0, _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3);
            float4 _DissolveCustomFunction_c1278429f28948e08cf996a8b62479a9_Out_3;
            Dissolve_float((_RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3.xy), _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0, _Property_a077d5d9cbf747f5875c6c24513b2800_Out_0, _Property_0ff262777af74848aa81b3dba71541ad_Out_0, _Property_7368f6d6acbe464fb71b082ec611715e_Out_0, _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3, _DissolveCustomFunction_c1278429f28948e08cf996a8b62479a9_Out_3);
            float _Property_3808652b3bd84f0dae18b3e1973a9e4b_Out_0 = _GSize;
            float _Property_4618d5267616447a8c6b7427eb1a57dd_Out_0 = _GRotate;
            float4 _RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3;
            RotateSize_float(_UV_8cea45496b8c45eab3597a074d15d476_Out_0, _Property_3808652b3bd84f0dae18b3e1973a9e4b_Out_0, _Property_4618d5267616447a8c6b7427eb1a57dd_Out_0, _RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3);
            UnityTexture2D _Property_d018d28ee214437a82076451743a8130_Out_0 = UnityBuildTexture2DStructNoScale(_DissolveTex);
            float4 _Property_b4cc761c375e4bf5b21dd8dcf9704990_Out_0 = _DissolveTex_ST;
            float4 _DTexSetCustomFunction_65a258090b234db4ab663709ae4ac4d4_Out_3;
            DTexSet_float(_RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3, _Property_b4cc761c375e4bf5b21dd8dcf9704990_Out_0, _DTexSetCustomFunction_65a258090b234db4ab663709ae4ac4d4_Out_3);
            float4 _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0 = SAMPLE_TEXTURE2D(_Property_d018d28ee214437a82076451743a8130_Out_0.tex, _Property_d018d28ee214437a82076451743a8130_Out_0.samplerstate, _Property_d018d28ee214437a82076451743a8130_Out_0.GetTransformedUV((_DTexSetCustomFunction_65a258090b234db4ab663709ae4ac4d4_Out_3.xy)));
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_R_4 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.r;
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_G_5 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.g;
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_B_6 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.b;
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_A_7 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.a;
            float _Property_6e040bc940b740d3954fb5ef6f7965de_Out_0 = _GDissolveIn;
            float _Property_6dce9fc004da445ab2b9bebb19d43137_Out_0 = _GDissolveOut;
            float4 _Property_5863bb3472fe4bf1800650465375edcd_Out_0 = IsGammaSpace() ? LinearToSRGB(_GDissolveColor) : _GDissolveColor;
            UnityTexture2D _Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0.tex, _Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0.samplerstate, _Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0.GetTransformedUV((_RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3.xy)));
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_R_4 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.r;
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_G_5 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.g;
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_B_6 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.b;
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_A_7 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.a;
            float4 _Swizzle_c89196372af64bba9bf916bb81898cc9_Out_1 = _SampleTexture2D_cddf3472524741888ba91b342532777d_G_5.xxxx;
            float _Property_07a37b638f724de594b4970af7e3d22f_Out_0 = _GLight;
            float4 _Property_d7f27dbc94dd4dbca6685d8c913bff10_Out_0 = IsGammaSpace() ? LinearToSRGB(_GColor) : _GColor;
            float _Property_a2e2d040f3874da48daa383fde697713_Out_0 = _GLightGain;
            float4 _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3;
            TextureLight_float(_Swizzle_c89196372af64bba9bf916bb81898cc9_Out_1, _Property_07a37b638f724de594b4970af7e3d22f_Out_0, _Property_d7f27dbc94dd4dbca6685d8c913bff10_Out_0, _Property_a2e2d040f3874da48daa383fde697713_Out_0, _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3);
            float4 _DissolveCustomFunction_c794717a85c44b778d53446bc5599b11_Out_3;
            Dissolve_float((_RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3.xy), _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0, _Property_6e040bc940b740d3954fb5ef6f7965de_Out_0, _Property_6dce9fc004da445ab2b9bebb19d43137_Out_0, _Property_5863bb3472fe4bf1800650465375edcd_Out_0, _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3, _DissolveCustomFunction_c794717a85c44b778d53446bc5599b11_Out_3);
            float _Property_8fb0e68fd07a405b921fa2706f8ae7f4_Out_0 = _BSize;
            float _Property_6040d22195bf4819ae19ecc7e8841069_Out_0 = _BRotate;
            float4 _RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3;
            RotateSize_float(_UV_8cea45496b8c45eab3597a074d15d476_Out_0, _Property_8fb0e68fd07a405b921fa2706f8ae7f4_Out_0, _Property_6040d22195bf4819ae19ecc7e8841069_Out_0, _RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3);
            UnityTexture2D _Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0 = UnityBuildTexture2DStructNoScale(_DissolveTex);
            float4 _Property_53bc3e203cdf45779cf450bcccc43ff8_Out_0 = _DissolveTex_ST;
            float4 _DTexSetCustomFunction_e8155feacdd943ddb8f4ff867df48696_Out_3;
            DTexSet_float(_RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3, _Property_53bc3e203cdf45779cf450bcccc43ff8_Out_0, _DTexSetCustomFunction_e8155feacdd943ddb8f4ff867df48696_Out_3);
            float4 _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0 = SAMPLE_TEXTURE2D(_Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0.tex, _Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0.samplerstate, _Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0.GetTransformedUV((_DTexSetCustomFunction_e8155feacdd943ddb8f4ff867df48696_Out_3.xy)));
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_R_4 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.r;
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_G_5 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.g;
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_B_6 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.b;
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_A_7 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.a;
            float _Property_df06dff0c2484fb595c137b1754753b9_Out_0 = _BDissolveIn;
            float _Property_d214b0d6c4864bde9efaa3db6db55a86_Out_0 = _BDissolveOut;
            float4 _Property_f2164c96cf7945fd975240ed7e800b18_Out_0 = IsGammaSpace() ? LinearToSRGB(_BDissolveColor) : _BDissolveColor;
            UnityTexture2D _Property_d2937aa927074faf8a7e3502b5f57de2_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0 = SAMPLE_TEXTURE2D(_Property_d2937aa927074faf8a7e3502b5f57de2_Out_0.tex, _Property_d2937aa927074faf8a7e3502b5f57de2_Out_0.samplerstate, _Property_d2937aa927074faf8a7e3502b5f57de2_Out_0.GetTransformedUV((_RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3.xy)));
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_R_4 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.r;
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_G_5 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.g;
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_B_6 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.b;
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_A_7 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.a;
            float4 _Swizzle_f28776abd40e4239a0500f742ca08953_Out_1 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_B_6.xxxx;
            float _Property_ff648668c0b14c5bb553df8ea7ab4c99_Out_0 = _BLight;
            float4 _Property_f2d51e21017b4d62854d0dd0d69a4eeb_Out_0 = IsGammaSpace() ? LinearToSRGB(_BColor) : _BColor;
            float _Property_2358423daf2b46bd88fc2c6f83681dab_Out_0 = _BLightGain;
            float4 _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3;
            TextureLight_float(_Swizzle_f28776abd40e4239a0500f742ca08953_Out_1, _Property_ff648668c0b14c5bb553df8ea7ab4c99_Out_0, _Property_f2d51e21017b4d62854d0dd0d69a4eeb_Out_0, _Property_2358423daf2b46bd88fc2c6f83681dab_Out_0, _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3);
            float4 _DissolveCustomFunction_18ec96fae3434badafbd66e66f5f7fc7_Out_3;
            Dissolve_float((_RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3.xy), _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0, _Property_df06dff0c2484fb595c137b1754753b9_Out_0, _Property_d214b0d6c4864bde9efaa3db6db55a86_Out_0, _Property_f2164c96cf7945fd975240ed7e800b18_Out_0, _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3, _DissolveCustomFunction_18ec96fae3434badafbd66e66f5f7fc7_Out_3);
            float4 _Add_b5aafd4ee22e4670a9b2a0eeae0b991b_Out_2;
            Unity_Add_float4(_DissolveCustomFunction_c794717a85c44b778d53446bc5599b11_Out_3, _DissolveCustomFunction_18ec96fae3434badafbd66e66f5f7fc7_Out_3, _Add_b5aafd4ee22e4670a9b2a0eeae0b991b_Out_2);
            float4 _Add_311c3433751947a9b670b04484a11b3b_Out_2;
            Unity_Add_float4(_DissolveCustomFunction_c1278429f28948e08cf996a8b62479a9_Out_3, _Add_b5aafd4ee22e4670a9b2a0eeae0b991b_Out_2, _Add_311c3433751947a9b670b04484a11b3b_Out_2);
            float _Swizzle_19e7c60d34834501ad98d57eecf88a20_Out_1 = _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3.w;
            float _Swizzle_2b2f7ab3dfd748fbb705d617d01b1690_Out_1 = _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3.w;
            float _Swizzle_d4f48dddbcae41cfbf129451e203ecf4_Out_1 = _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3.w;
            float _Add_d05af43285cb4cce8638140598473902_Out_2;
            Unity_Add_float(_Swizzle_2b2f7ab3dfd748fbb705d617d01b1690_Out_1, _Swizzle_d4f48dddbcae41cfbf129451e203ecf4_Out_1, _Add_d05af43285cb4cce8638140598473902_Out_2);
            float _Add_747b1754f01448a9bb57f2993c19791e_Out_2;
            Unity_Add_float(_Swizzle_19e7c60d34834501ad98d57eecf88a20_Out_1, _Add_d05af43285cb4cce8638140598473902_Out_2, _Add_747b1754f01448a9bb57f2993c19791e_Out_2);
            float4 _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGBA_4;
            float3 _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGB_5;
            float2 _Combine_94b4ca9170e44d93bdf05a483d08c44b_RG_6;
            Unity_Combine_float(0, 0, 0, _Add_747b1754f01448a9bb57f2993c19791e_Out_2, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGBA_4, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGB_5, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RG_6);
            float4 _Add_2670f6bd6b3f4beb94a86259f697e3cf_Out_2;
            Unity_Add_float4(_Add_311c3433751947a9b670b04484a11b3b_Out_2, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGBA_4, _Add_2670f6bd6b3f4beb94a86259f697e3cf_Out_2);
            float _Swizzle_6cead8bc7a22455b93e7f26073d331da_Out_1 = _Add_2670f6bd6b3f4beb94a86259f697e3cf_Out_2.w;
            surface.BaseColor = IsGammaSpace() ? float3(0, 0, 0) : SRGBToLinear(float3(0, 0, 0));
            surface.Alpha = _Swizzle_6cead8bc7a22455b93e7f26073d331da_Out_1;
            surface.NormalTS = IN.TangentSpaceNormal;
            surface.NormalAlpha = 1;
            surface.Emission = (_Add_2670f6bd6b3f4beb94a86259f697e3cf_Out_2.xyz);
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            
        //     $features.graphVertex:  $include("VertexAnimation.template.hlsl")
        //                                       ^ ERROR: $include cannot find file : VertexAnimation.template.hlsl. Looked into:
        // Packages/com.unity.shadergraph/Editor/Generation/Templates
        
            
        //     $features.graphPixel:   $include("SharedCode.template.hlsl")
        //                                       ^ ERROR: $include cannot find file : SharedCode.template.hlsl. Looked into:
        // Packages/com.unity.shadergraph/Editor/Generation/Templates
        
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorCopyToSDI' */
        
        
        
            output.TangentSpaceNormal =                         float3(0.0f, 0.0f, 1.0f);
        
        
            output.uv0 =                                        input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
            return output;
        }
        
            // --------------------------------------------------
            // Build Surface Data
        
            uint2 ComputeFadeMaskSeed(uint2 positionSS)
            {
                uint2 fadeMaskSeed;
        
                // Can't use the view direction, it is the same across the entire screen.
                fadeMaskSeed = positionSS;
        
                return fadeMaskSeed;
            }
        
            void GetSurfaceData(Varyings input, half3 viewDirectioWS, uint2 positionSS, float angleFadeFactor, out DecalSurfaceData surfaceData)
            {
                #if (SHADERPASS == SHADERPASS_DBUFFER_PROJECTOR) || (SHADERPASS == SHADERPASS_FORWARD_EMISSIVE_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_SCREEN_SPACE_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_GBUFFER_PROJECTOR)
                    half4x4 normalToWorld = UNITY_ACCESS_INSTANCED_PROP(Decal, _NormalToWorld);
                    half fadeFactor = clamp(normalToWorld[0][3], 0.0f, 1.0f) * angleFadeFactor;
                    float2 scale = float2(normalToWorld[3][0], normalToWorld[3][1]);
                    float2 offset = float2(normalToWorld[3][2], normalToWorld[3][3]);
                    input.texCoord0.xy = input.texCoord0.xy * scale + offset;
                #else
                    #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                        LODDitheringTransition(ComputeFadeMaskSeed(positionSS), unity_LODFade.x);
                    #endif
        
                    half fadeFactor = half(1.0);
                #endif
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = BuildSurfaceDescriptionInputs(input);
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
        
                // setup defaults -- these are used if the graph doesn't output a value
                ZERO_INITIALIZE(DecalSurfaceData, surfaceData);
                surfaceData.occlusion = half(1.0);
                surfaceData.smoothness = half(0);
        
                #ifdef _MATERIAL_AFFECTS_NORMAL
                    surfaceData.normalWS.w = half(1.0);
                #else
                    surfaceData.normalWS.w = half(0.0);
                #endif
        
                surfaceData.emissive.rgb = half3(surfaceDescription.Emission.rgb * fadeFactor);
        
                // copy across graph values, if defined
                surfaceData.baseColor.xyz = half3(surfaceDescription.BaseColor);
                surfaceData.baseColor.w = half(surfaceDescription.Alpha * fadeFactor);
        
                #if (SHADERPASS == SHADERPASS_DBUFFER_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_SCREEN_SPACE_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_GBUFFER_PROJECTOR)
                    #if defined(_MATERIAL_AFFECTS_NORMAL)
                        surfaceData.normalWS.xyz = mul((half3x3)normalToWorld, surfaceDescription.NormalTS.xyz);
                    #else
                        surfaceData.normalWS.xyz = normalToWorld[2].xyz;
                    #endif
                #elif (SHADERPASS == SHADERPASS_DBUFFER_MESH) || (SHADERPASS == SHADERPASS_DECAL_SCREEN_SPACE_MESH) || (SHADERPASS == SHADERPASS_DECAL_GBUFFER_MESH)
                    #if defined(_MATERIAL_AFFECTS_NORMAL)
                        float sgn = input.tangentWS.w;      // should be either +1 or -1
                        float3 bitangent = sgn * cross(input.normalWS.xyz, input.tangentWS.xyz);
                        half3x3 tangentToWorld = half3x3(input.tangentWS.xyz, bitangent.xyz, input.normalWS.xyz);
        
                        // We need to normalize as we use mikkt tangent space and this is expected (tangent space is not normalize)
                        surfaceData.normalWS.xyz = normalize(TransformTangentToWorld(surfaceDescription.NormalTS, tangentToWorld));
                    #else
                        surfaceData.normalWS.xyz = half3(input.normalWS); // Default to vertex normal
                    #endif
                #endif
        
                surfaceData.normalWS.w = surfaceDescription.NormalAlpha * fadeFactor;
        
                // In case of Smoothness / AO / Metal, all the three are always computed but color mask can change
            }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPassDecal.hlsl"
        
            ENDHLSL
        }
        Pass
        { 
            Name "DBufferMesh"
            Tags 
            { 
                "LightMode" = "DBufferMesh"
            }
        
            // Render State
            Blend 0 SrcAlpha OneMinusSrcAlpha, Zero OneMinusSrcAlpha
        Blend 1 SrcAlpha OneMinusSrcAlpha, Zero OneMinusSrcAlpha
        Blend 2 SrcAlpha OneMinusSrcAlpha, Zero OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        ColorMask RGBA
        ColorMask RGBA 1
        ColorMask 0 2
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma target 3.5
        #pragma vertex Vert
        #pragma fragment Frag
        #pragma multi_compile_instancing
        
            // Keywords
            #pragma multi_compile_fragment _ _DBUFFER_MRT1 _DBUFFER_MRT2 _DBUFFER_MRT3
            // GraphKeywords: <None>
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        
            // Defines
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define ATTRIBUTES_NEED_TEXCOORD2
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_TANGENT_WS
            #define VARYINGS_NEED_TEXCOORD0
            
            #define HAVE_MESH_MODIFICATION
        
        
            #define SHADERPASS SHADERPASS_DBUFFER_MESH
        #define _MATERIAL_AFFECTS_ALBEDO 1
        #define _MATERIAL_AFFECTS_NORMAL 1
        #define _MATERIAL_AFFECTS_NORMAL_BLEND 1
        
            // HybridV1InjectedBuiltinProperties: <None>
        
            // -- Properties used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DecalInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderVariablesDecal.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
        
            // --------------------------------------------------
            // Structs and Packing
        
            struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 uv1 : TEXCOORD1;
             float4 uv2 : TEXCOORD2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 tangentWS;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 TangentSpaceNormal;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float3 interp1 : INTERP1;
             float4 interp2 : INTERP2;
             float4 interp3 : INTERP3;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyz =  input.normalWS;
            output.interp2.xyzw =  input.tangentWS;
            output.interp3.xyzw =  input.texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.tangentWS = input.interp2.xyzw;
            output.texCoord0 = input.interp3.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float4 _DissolveTex_TexelSize;
        float _GroundResidue;
        float4 _RColor;
        float _RSize;
        float _RRotate;
        float _RLight;
        float4 _RDissolveColor;
        float _RLightGain;
        float _RDissolveIn;
        float _RDissolveOut;
        float4 _DissolveTex_ST;
        float4 _MainTex_ST;
        float4 _GColor;
        float _GSize;
        float _GRotate;
        float _GLight;
        float _GLightGain;
        float4 _GDissolveColor;
        float _GDissolveIn;
        float _GDissolveOut;
        float4 _BColor;
        float _BSize;
        float _BRotate;
        float _BLight;
        float _BLightGain;
        float4 _BDissolveColor;
        float _BDissolveIn;
        float _BDissolveOut;
        float _DrawOrder;
        float _DecalMeshBiasType;
        float _DecalMeshDepthBias;
        float _DecalMeshViewBias;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_DissolveTex);
        SAMPLER(sampler_DissolveTex);
        
            // Graph Functions
            
        void RotateSize_float(float4 _uv, float _size, float _rotate, out float4 Out){
                _rotate = _rotate*(3.14159265359/180);
            
                float2 ruv = float2(((_uv.x - 0.5) * 1/_size) + 0.5 ,((_uv.y - 0.5) * 1/_size) + 0.5);
            
                ruv = ruv - float2(0.5, 0.5);  
            
                ruv = float2(ruv.x * cos(_rotate) - ruv.y * sin(_rotate),ruv.x * sin(_rotate) + ruv.y * cos(_rotate));  
            
                ruv += float2(0.5, 0.5);
            
            
            
                Out = float4(ruv.x,ruv.y,_uv.z,_uv.w);
            
        }
        
        void DTexSet_float(float4 _uvD, float4 _DissolveTex_ST, out float4 Out){
             Out = float4((_uvD.x- 0.5) * _DissolveTex_ST.x+ _DissolveTex_ST.z,(_uvD.y- 0.5) * _DissolveTex_ST.y+ _DissolveTex_ST.w,_uvD.z,_uvD.w);
        }
        
        void TextureLight_float(float4 _tex, float _light, float4 _color, float _lightGain, out float4 Out){
                float4 RtexB = smoothstep( 1-_light,1,_tex);
            
                RtexB = RtexB * 0.2 + RtexB *  _color * _lightGain;
            
                _tex.rgb = _tex.rgb * _color.rgb + RtexB.rgb;
            
                _tex = _tex * _color.a *_tex.a ;
            
                Out = _tex;
        }
        
        void Dissolve_float(float2 _uv, float4 _Dtex, float _DissolveIn, float _DissolveOut, float4 _Color, float4 _tex, out float4 Out){
                float4 spheric = saturate( distance( _uv ,float2(0.5,0.5)) );
            
                spheric =saturate(spheric + spheric * _Dtex);
            
                           
            
                float4 spheric01A =saturate( saturate( (spheric + lerp(-1.01,0.01,1-_DissolveIn ))*100));
            
                float4 spheric01B = saturate( (spheric + lerp(-1.01,0.01,1-_DissolveIn- 0.01))*100);
            
                spheric01B = spheric01A * (1-spheric01B) * _Color * _Color.a;
            
            
            
                _tex = _tex * spheric01A + spheric01B * _tex.a;
            
            
            
                float4 spheric02A = saturate( ((1-spheric) + lerp(-1.01,0.01,_DissolveOut+ 0.01))*100);
            
                float4 spheric02B = saturate( ((1-spheric) + lerp(-1.01,0.01,_DissolveOut))*100);
            
                spheric02B =spheric02A * (1-spheric02B) * _Color * _Color.a;
            
                
            
                _tex = _tex * spheric02A + spheric02B * _tex.a;
            
                Out = _tex;
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
        {
            RGBA = float4(R, G, B, A);
            RGB = float3(R, G, B);
            RG = float2(R, G);
        }
        
            // Graph Vertex
            struct VertexDescription
        {
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            return description;
        }
            
            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float3 NormalTS;
            float NormalAlpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _UV_8cea45496b8c45eab3597a074d15d476_Out_0 = IN.uv0;
            float _Property_7b769f55c7864589aebfac4ddd12b2d6_Out_0 = _RSize;
            float _Property_4acdc5976b734662994ffee635158980_Out_0 = _RRotate;
            float4 _RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3;
            RotateSize_float(_UV_8cea45496b8c45eab3597a074d15d476_Out_0, _Property_7b769f55c7864589aebfac4ddd12b2d6_Out_0, _Property_4acdc5976b734662994ffee635158980_Out_0, _RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3);
            UnityTexture2D _Property_eafddcee5f7f4f66817085ef749c6b35_Out_0 = UnityBuildTexture2DStructNoScale(_DissolveTex);
            float4 _Property_1b4f5806053747519e92207b6d7afbee_Out_0 = _DissolveTex_ST;
            float4 _DTexSetCustomFunction_b0fd3e7232bf4e08ab5d88555c1b5756_Out_3;
            DTexSet_float(_RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3, _Property_1b4f5806053747519e92207b6d7afbee_Out_0, _DTexSetCustomFunction_b0fd3e7232bf4e08ab5d88555c1b5756_Out_3);
            float4 _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0 = SAMPLE_TEXTURE2D(_Property_eafddcee5f7f4f66817085ef749c6b35_Out_0.tex, _Property_eafddcee5f7f4f66817085ef749c6b35_Out_0.samplerstate, _Property_eafddcee5f7f4f66817085ef749c6b35_Out_0.GetTransformedUV((_DTexSetCustomFunction_b0fd3e7232bf4e08ab5d88555c1b5756_Out_3.xy)));
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_R_4 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.r;
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_G_5 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.g;
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_B_6 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.b;
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_A_7 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.a;
            float _Property_a077d5d9cbf747f5875c6c24513b2800_Out_0 = _RDissolveIn;
            float _Property_0ff262777af74848aa81b3dba71541ad_Out_0 = _RDissolveOut;
            float4 _Property_7368f6d6acbe464fb71b082ec611715e_Out_0 = IsGammaSpace() ? LinearToSRGB(_RDissolveColor) : _RDissolveColor;
            UnityTexture2D _Property_c21f55442e394cf284a0766460ec9cac_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0 = SAMPLE_TEXTURE2D(_Property_c21f55442e394cf284a0766460ec9cac_Out_0.tex, _Property_c21f55442e394cf284a0766460ec9cac_Out_0.samplerstate, _Property_c21f55442e394cf284a0766460ec9cac_Out_0.GetTransformedUV((_RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3.xy)));
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_R_4 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.r;
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_G_5 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.g;
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_B_6 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.b;
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_A_7 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.a;
            float4 _Swizzle_400506fb40164c489617df53e573832a_Out_1 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_R_4.xxxx;
            float _Property_a5385ead93534fa7aa7ee63377c08787_Out_0 = _RLight;
            float4 _Property_d4de088caa5d43b99fa6421c07169156_Out_0 = IsGammaSpace() ? LinearToSRGB(_RColor) : _RColor;
            float _Property_d8081504f4364890a7d4a0f3971be926_Out_0 = _RLightGain;
            float4 _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3;
            TextureLight_float(_Swizzle_400506fb40164c489617df53e573832a_Out_1, _Property_a5385ead93534fa7aa7ee63377c08787_Out_0, _Property_d4de088caa5d43b99fa6421c07169156_Out_0, _Property_d8081504f4364890a7d4a0f3971be926_Out_0, _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3);
            float4 _DissolveCustomFunction_c1278429f28948e08cf996a8b62479a9_Out_3;
            Dissolve_float((_RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3.xy), _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0, _Property_a077d5d9cbf747f5875c6c24513b2800_Out_0, _Property_0ff262777af74848aa81b3dba71541ad_Out_0, _Property_7368f6d6acbe464fb71b082ec611715e_Out_0, _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3, _DissolveCustomFunction_c1278429f28948e08cf996a8b62479a9_Out_3);
            float _Property_3808652b3bd84f0dae18b3e1973a9e4b_Out_0 = _GSize;
            float _Property_4618d5267616447a8c6b7427eb1a57dd_Out_0 = _GRotate;
            float4 _RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3;
            RotateSize_float(_UV_8cea45496b8c45eab3597a074d15d476_Out_0, _Property_3808652b3bd84f0dae18b3e1973a9e4b_Out_0, _Property_4618d5267616447a8c6b7427eb1a57dd_Out_0, _RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3);
            UnityTexture2D _Property_d018d28ee214437a82076451743a8130_Out_0 = UnityBuildTexture2DStructNoScale(_DissolveTex);
            float4 _Property_b4cc761c375e4bf5b21dd8dcf9704990_Out_0 = _DissolveTex_ST;
            float4 _DTexSetCustomFunction_65a258090b234db4ab663709ae4ac4d4_Out_3;
            DTexSet_float(_RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3, _Property_b4cc761c375e4bf5b21dd8dcf9704990_Out_0, _DTexSetCustomFunction_65a258090b234db4ab663709ae4ac4d4_Out_3);
            float4 _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0 = SAMPLE_TEXTURE2D(_Property_d018d28ee214437a82076451743a8130_Out_0.tex, _Property_d018d28ee214437a82076451743a8130_Out_0.samplerstate, _Property_d018d28ee214437a82076451743a8130_Out_0.GetTransformedUV((_DTexSetCustomFunction_65a258090b234db4ab663709ae4ac4d4_Out_3.xy)));
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_R_4 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.r;
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_G_5 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.g;
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_B_6 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.b;
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_A_7 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.a;
            float _Property_6e040bc940b740d3954fb5ef6f7965de_Out_0 = _GDissolveIn;
            float _Property_6dce9fc004da445ab2b9bebb19d43137_Out_0 = _GDissolveOut;
            float4 _Property_5863bb3472fe4bf1800650465375edcd_Out_0 = IsGammaSpace() ? LinearToSRGB(_GDissolveColor) : _GDissolveColor;
            UnityTexture2D _Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0.tex, _Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0.samplerstate, _Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0.GetTransformedUV((_RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3.xy)));
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_R_4 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.r;
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_G_5 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.g;
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_B_6 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.b;
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_A_7 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.a;
            float4 _Swizzle_c89196372af64bba9bf916bb81898cc9_Out_1 = _SampleTexture2D_cddf3472524741888ba91b342532777d_G_5.xxxx;
            float _Property_07a37b638f724de594b4970af7e3d22f_Out_0 = _GLight;
            float4 _Property_d7f27dbc94dd4dbca6685d8c913bff10_Out_0 = IsGammaSpace() ? LinearToSRGB(_GColor) : _GColor;
            float _Property_a2e2d040f3874da48daa383fde697713_Out_0 = _GLightGain;
            float4 _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3;
            TextureLight_float(_Swizzle_c89196372af64bba9bf916bb81898cc9_Out_1, _Property_07a37b638f724de594b4970af7e3d22f_Out_0, _Property_d7f27dbc94dd4dbca6685d8c913bff10_Out_0, _Property_a2e2d040f3874da48daa383fde697713_Out_0, _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3);
            float4 _DissolveCustomFunction_c794717a85c44b778d53446bc5599b11_Out_3;
            Dissolve_float((_RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3.xy), _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0, _Property_6e040bc940b740d3954fb5ef6f7965de_Out_0, _Property_6dce9fc004da445ab2b9bebb19d43137_Out_0, _Property_5863bb3472fe4bf1800650465375edcd_Out_0, _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3, _DissolveCustomFunction_c794717a85c44b778d53446bc5599b11_Out_3);
            float _Property_8fb0e68fd07a405b921fa2706f8ae7f4_Out_0 = _BSize;
            float _Property_6040d22195bf4819ae19ecc7e8841069_Out_0 = _BRotate;
            float4 _RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3;
            RotateSize_float(_UV_8cea45496b8c45eab3597a074d15d476_Out_0, _Property_8fb0e68fd07a405b921fa2706f8ae7f4_Out_0, _Property_6040d22195bf4819ae19ecc7e8841069_Out_0, _RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3);
            UnityTexture2D _Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0 = UnityBuildTexture2DStructNoScale(_DissolveTex);
            float4 _Property_53bc3e203cdf45779cf450bcccc43ff8_Out_0 = _DissolveTex_ST;
            float4 _DTexSetCustomFunction_e8155feacdd943ddb8f4ff867df48696_Out_3;
            DTexSet_float(_RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3, _Property_53bc3e203cdf45779cf450bcccc43ff8_Out_0, _DTexSetCustomFunction_e8155feacdd943ddb8f4ff867df48696_Out_3);
            float4 _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0 = SAMPLE_TEXTURE2D(_Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0.tex, _Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0.samplerstate, _Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0.GetTransformedUV((_DTexSetCustomFunction_e8155feacdd943ddb8f4ff867df48696_Out_3.xy)));
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_R_4 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.r;
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_G_5 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.g;
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_B_6 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.b;
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_A_7 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.a;
            float _Property_df06dff0c2484fb595c137b1754753b9_Out_0 = _BDissolveIn;
            float _Property_d214b0d6c4864bde9efaa3db6db55a86_Out_0 = _BDissolveOut;
            float4 _Property_f2164c96cf7945fd975240ed7e800b18_Out_0 = IsGammaSpace() ? LinearToSRGB(_BDissolveColor) : _BDissolveColor;
            UnityTexture2D _Property_d2937aa927074faf8a7e3502b5f57de2_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0 = SAMPLE_TEXTURE2D(_Property_d2937aa927074faf8a7e3502b5f57de2_Out_0.tex, _Property_d2937aa927074faf8a7e3502b5f57de2_Out_0.samplerstate, _Property_d2937aa927074faf8a7e3502b5f57de2_Out_0.GetTransformedUV((_RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3.xy)));
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_R_4 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.r;
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_G_5 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.g;
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_B_6 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.b;
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_A_7 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.a;
            float4 _Swizzle_f28776abd40e4239a0500f742ca08953_Out_1 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_B_6.xxxx;
            float _Property_ff648668c0b14c5bb553df8ea7ab4c99_Out_0 = _BLight;
            float4 _Property_f2d51e21017b4d62854d0dd0d69a4eeb_Out_0 = IsGammaSpace() ? LinearToSRGB(_BColor) : _BColor;
            float _Property_2358423daf2b46bd88fc2c6f83681dab_Out_0 = _BLightGain;
            float4 _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3;
            TextureLight_float(_Swizzle_f28776abd40e4239a0500f742ca08953_Out_1, _Property_ff648668c0b14c5bb553df8ea7ab4c99_Out_0, _Property_f2d51e21017b4d62854d0dd0d69a4eeb_Out_0, _Property_2358423daf2b46bd88fc2c6f83681dab_Out_0, _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3);
            float4 _DissolveCustomFunction_18ec96fae3434badafbd66e66f5f7fc7_Out_3;
            Dissolve_float((_RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3.xy), _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0, _Property_df06dff0c2484fb595c137b1754753b9_Out_0, _Property_d214b0d6c4864bde9efaa3db6db55a86_Out_0, _Property_f2164c96cf7945fd975240ed7e800b18_Out_0, _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3, _DissolveCustomFunction_18ec96fae3434badafbd66e66f5f7fc7_Out_3);
            float4 _Add_b5aafd4ee22e4670a9b2a0eeae0b991b_Out_2;
            Unity_Add_float4(_DissolveCustomFunction_c794717a85c44b778d53446bc5599b11_Out_3, _DissolveCustomFunction_18ec96fae3434badafbd66e66f5f7fc7_Out_3, _Add_b5aafd4ee22e4670a9b2a0eeae0b991b_Out_2);
            float4 _Add_311c3433751947a9b670b04484a11b3b_Out_2;
            Unity_Add_float4(_DissolveCustomFunction_c1278429f28948e08cf996a8b62479a9_Out_3, _Add_b5aafd4ee22e4670a9b2a0eeae0b991b_Out_2, _Add_311c3433751947a9b670b04484a11b3b_Out_2);
            float _Swizzle_19e7c60d34834501ad98d57eecf88a20_Out_1 = _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3.w;
            float _Swizzle_2b2f7ab3dfd748fbb705d617d01b1690_Out_1 = _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3.w;
            float _Swizzle_d4f48dddbcae41cfbf129451e203ecf4_Out_1 = _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3.w;
            float _Add_d05af43285cb4cce8638140598473902_Out_2;
            Unity_Add_float(_Swizzle_2b2f7ab3dfd748fbb705d617d01b1690_Out_1, _Swizzle_d4f48dddbcae41cfbf129451e203ecf4_Out_1, _Add_d05af43285cb4cce8638140598473902_Out_2);
            float _Add_747b1754f01448a9bb57f2993c19791e_Out_2;
            Unity_Add_float(_Swizzle_19e7c60d34834501ad98d57eecf88a20_Out_1, _Add_d05af43285cb4cce8638140598473902_Out_2, _Add_747b1754f01448a9bb57f2993c19791e_Out_2);
            float4 _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGBA_4;
            float3 _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGB_5;
            float2 _Combine_94b4ca9170e44d93bdf05a483d08c44b_RG_6;
            Unity_Combine_float(0, 0, 0, _Add_747b1754f01448a9bb57f2993c19791e_Out_2, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGBA_4, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGB_5, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RG_6);
            float4 _Add_2670f6bd6b3f4beb94a86259f697e3cf_Out_2;
            Unity_Add_float4(_Add_311c3433751947a9b670b04484a11b3b_Out_2, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGBA_4, _Add_2670f6bd6b3f4beb94a86259f697e3cf_Out_2);
            float _Swizzle_6cead8bc7a22455b93e7f26073d331da_Out_1 = _Add_2670f6bd6b3f4beb94a86259f697e3cf_Out_2.w;
            surface.BaseColor = IsGammaSpace() ? float3(0, 0, 0) : SRGBToLinear(float3(0, 0, 0));
            surface.Alpha = _Swizzle_6cead8bc7a22455b93e7f26073d331da_Out_1;
            surface.NormalTS = IN.TangentSpaceNormal;
            surface.NormalAlpha = 1;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            
        //     $features.graphVertex:  $include("VertexAnimation.template.hlsl")
        //                                       ^ ERROR: $include cannot find file : VertexAnimation.template.hlsl. Looked into:
        // Packages/com.unity.shadergraph/Editor/Generation/Templates
        
            
        //     $features.graphPixel:   $include("SharedCode.template.hlsl")
        //                                       ^ ERROR: $include cannot find file : SharedCode.template.hlsl. Looked into:
        // Packages/com.unity.shadergraph/Editor/Generation/Templates
        
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorCopyToSDI' */
        
        
        
            output.TangentSpaceNormal =                         float3(0.0f, 0.0f, 1.0f);
        
        
            output.uv0 =                                        input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
            return output;
        }
        
            // --------------------------------------------------
            // Build Surface Data
        
            uint2 ComputeFadeMaskSeed(uint2 positionSS)
            {
                uint2 fadeMaskSeed;
        
                // Can't use the view direction, it is the same across the entire screen.
                fadeMaskSeed = positionSS;
        
                return fadeMaskSeed;
            }
        
            void GetSurfaceData(Varyings input, half3 viewDirectioWS, uint2 positionSS, float angleFadeFactor, out DecalSurfaceData surfaceData)
            {
                #if (SHADERPASS == SHADERPASS_DBUFFER_PROJECTOR) || (SHADERPASS == SHADERPASS_FORWARD_EMISSIVE_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_SCREEN_SPACE_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_GBUFFER_PROJECTOR)
                    half4x4 normalToWorld = UNITY_ACCESS_INSTANCED_PROP(Decal, _NormalToWorld);
                    half fadeFactor = clamp(normalToWorld[0][3], 0.0f, 1.0f) * angleFadeFactor;
                    float2 scale = float2(normalToWorld[3][0], normalToWorld[3][1]);
                    float2 offset = float2(normalToWorld[3][2], normalToWorld[3][3]);
                    input.texCoord0.xy = input.texCoord0.xy * scale + offset;
                #else
                    #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                        LODDitheringTransition(ComputeFadeMaskSeed(positionSS), unity_LODFade.x);
                    #endif
        
                    half fadeFactor = half(1.0);
                #endif
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = BuildSurfaceDescriptionInputs(input);
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
        
                // setup defaults -- these are used if the graph doesn't output a value
                ZERO_INITIALIZE(DecalSurfaceData, surfaceData);
                surfaceData.occlusion = half(1.0);
                surfaceData.smoothness = half(0);
        
                #ifdef _MATERIAL_AFFECTS_NORMAL
                    surfaceData.normalWS.w = half(1.0);
                #else
                    surfaceData.normalWS.w = half(0.0);
                #endif
        
        
                // copy across graph values, if defined
                surfaceData.baseColor.xyz = half3(surfaceDescription.BaseColor);
                surfaceData.baseColor.w = half(surfaceDescription.Alpha * fadeFactor);
        
                #if (SHADERPASS == SHADERPASS_DBUFFER_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_SCREEN_SPACE_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_GBUFFER_PROJECTOR)
                    #if defined(_MATERIAL_AFFECTS_NORMAL)
                        surfaceData.normalWS.xyz = mul((half3x3)normalToWorld, surfaceDescription.NormalTS.xyz);
                    #else
                        surfaceData.normalWS.xyz = normalToWorld[2].xyz;
                    #endif
                #elif (SHADERPASS == SHADERPASS_DBUFFER_MESH) || (SHADERPASS == SHADERPASS_DECAL_SCREEN_SPACE_MESH) || (SHADERPASS == SHADERPASS_DECAL_GBUFFER_MESH)
                    #if defined(_MATERIAL_AFFECTS_NORMAL)
                        float sgn = input.tangentWS.w;      // should be either +1 or -1
                        float3 bitangent = sgn * cross(input.normalWS.xyz, input.tangentWS.xyz);
                        half3x3 tangentToWorld = half3x3(input.tangentWS.xyz, bitangent.xyz, input.normalWS.xyz);
        
                        // We need to normalize as we use mikkt tangent space and this is expected (tangent space is not normalize)
                        surfaceData.normalWS.xyz = normalize(TransformTangentToWorld(surfaceDescription.NormalTS, tangentToWorld));
                    #else
                        surfaceData.normalWS.xyz = half3(input.normalWS); // Default to vertex normal
                    #endif
                #endif
        
                surfaceData.normalWS.w = surfaceDescription.NormalAlpha * fadeFactor;
        
                // In case of Smoothness / AO / Metal, all the three are always computed but color mask can change
            }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPassDecal.hlsl"
        
            ENDHLSL
        }
        Pass
        { 
            Name "DecalMeshForwardEmissive"
            Tags 
            { 
                "LightMode" = "DecalMeshForwardEmissive"
            }
        
            // Render State
            Blend 0 SrcAlpha One
        ZTest LEqual
        ZWrite Off
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma target 3.5
        #pragma vertex Vert
        #pragma fragment Frag
        #pragma multi_compile_instancing
        
            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        
            // Defines
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define ATTRIBUTES_NEED_TEXCOORD2
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_TANGENT_WS
            #define VARYINGS_NEED_TEXCOORD0
            
            #define HAVE_MESH_MODIFICATION
        
        
            #define SHADERPASS SHADERPASS_FORWARD_EMISSIVE_MESH
        #define _MATERIAL_AFFECTS_ALBEDO 1
        #define _MATERIAL_AFFECTS_NORMAL 1
        #define _MATERIAL_AFFECTS_NORMAL_BLEND 1
        #define _MATERIAL_AFFECTS_EMISSION 1
        
            // HybridV1InjectedBuiltinProperties: <None>
        
            // -- Properties used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DecalInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderVariablesDecal.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DBuffer.hlsl"
        
            // --------------------------------------------------
            // Structs and Packing
        
            struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 uv1 : TEXCOORD1;
             float4 uv2 : TEXCOORD2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 tangentWS;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 TangentSpaceNormal;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float3 interp1 : INTERP1;
             float4 interp2 : INTERP2;
             float4 interp3 : INTERP3;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyz =  input.normalWS;
            output.interp2.xyzw =  input.tangentWS;
            output.interp3.xyzw =  input.texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.tangentWS = input.interp2.xyzw;
            output.texCoord0 = input.interp3.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float4 _DissolveTex_TexelSize;
        float _GroundResidue;
        float4 _RColor;
        float _RSize;
        float _RRotate;
        float _RLight;
        float4 _RDissolveColor;
        float _RLightGain;
        float _RDissolveIn;
        float _RDissolveOut;
        float4 _DissolveTex_ST;
        float4 _MainTex_ST;
        float4 _GColor;
        float _GSize;
        float _GRotate;
        float _GLight;
        float _GLightGain;
        float4 _GDissolveColor;
        float _GDissolveIn;
        float _GDissolveOut;
        float4 _BColor;
        float _BSize;
        float _BRotate;
        float _BLight;
        float _BLightGain;
        float4 _BDissolveColor;
        float _BDissolveIn;
        float _BDissolveOut;
        float _DrawOrder;
        float _DecalMeshBiasType;
        float _DecalMeshDepthBias;
        float _DecalMeshViewBias;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_DissolveTex);
        SAMPLER(sampler_DissolveTex);
        
            // Graph Functions
            
        void RotateSize_float(float4 _uv, float _size, float _rotate, out float4 Out){
                _rotate = _rotate*(3.14159265359/180);
            
                float2 ruv = float2(((_uv.x - 0.5) * 1/_size) + 0.5 ,((_uv.y - 0.5) * 1/_size) + 0.5);
            
                ruv = ruv - float2(0.5, 0.5);  
            
                ruv = float2(ruv.x * cos(_rotate) - ruv.y * sin(_rotate),ruv.x * sin(_rotate) + ruv.y * cos(_rotate));  
            
                ruv += float2(0.5, 0.5);
            
            
            
                Out = float4(ruv.x,ruv.y,_uv.z,_uv.w);
            
        }
        
        void DTexSet_float(float4 _uvD, float4 _DissolveTex_ST, out float4 Out){
             Out = float4((_uvD.x- 0.5) * _DissolveTex_ST.x+ _DissolveTex_ST.z,(_uvD.y- 0.5) * _DissolveTex_ST.y+ _DissolveTex_ST.w,_uvD.z,_uvD.w);
        }
        
        void TextureLight_float(float4 _tex, float _light, float4 _color, float _lightGain, out float4 Out){
                float4 RtexB = smoothstep( 1-_light,1,_tex);
            
                RtexB = RtexB * 0.2 + RtexB *  _color * _lightGain;
            
                _tex.rgb = _tex.rgb * _color.rgb + RtexB.rgb;
            
                _tex = _tex * _color.a *_tex.a ;
            
                Out = _tex;
        }
        
        void Dissolve_float(float2 _uv, float4 _Dtex, float _DissolveIn, float _DissolveOut, float4 _Color, float4 _tex, out float4 Out){
                float4 spheric = saturate( distance( _uv ,float2(0.5,0.5)) );
            
                spheric =saturate(spheric + spheric * _Dtex);
            
                           
            
                float4 spheric01A =saturate( saturate( (spheric + lerp(-1.01,0.01,1-_DissolveIn ))*100));
            
                float4 spheric01B = saturate( (spheric + lerp(-1.01,0.01,1-_DissolveIn- 0.01))*100);
            
                spheric01B = spheric01A * (1-spheric01B) * _Color * _Color.a;
            
            
            
                _tex = _tex * spheric01A + spheric01B * _tex.a;
            
            
            
                float4 spheric02A = saturate( ((1-spheric) + lerp(-1.01,0.01,_DissolveOut+ 0.01))*100);
            
                float4 spheric02B = saturate( ((1-spheric) + lerp(-1.01,0.01,_DissolveOut))*100);
            
                spheric02B =spheric02A * (1-spheric02B) * _Color * _Color.a;
            
                
            
                _tex = _tex * spheric02A + spheric02B * _tex.a;
            
                Out = _tex;
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
        {
            RGBA = float4(R, G, B, A);
            RGB = float3(R, G, B);
            RG = float2(R, G);
        }
        
            // Graph Vertex
            struct VertexDescription
        {
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            return description;
        }
            
            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float3 NormalTS;
            float NormalAlpha;
            float3 Emission;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _UV_8cea45496b8c45eab3597a074d15d476_Out_0 = IN.uv0;
            float _Property_7b769f55c7864589aebfac4ddd12b2d6_Out_0 = _RSize;
            float _Property_4acdc5976b734662994ffee635158980_Out_0 = _RRotate;
            float4 _RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3;
            RotateSize_float(_UV_8cea45496b8c45eab3597a074d15d476_Out_0, _Property_7b769f55c7864589aebfac4ddd12b2d6_Out_0, _Property_4acdc5976b734662994ffee635158980_Out_0, _RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3);
            UnityTexture2D _Property_eafddcee5f7f4f66817085ef749c6b35_Out_0 = UnityBuildTexture2DStructNoScale(_DissolveTex);
            float4 _Property_1b4f5806053747519e92207b6d7afbee_Out_0 = _DissolveTex_ST;
            float4 _DTexSetCustomFunction_b0fd3e7232bf4e08ab5d88555c1b5756_Out_3;
            DTexSet_float(_RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3, _Property_1b4f5806053747519e92207b6d7afbee_Out_0, _DTexSetCustomFunction_b0fd3e7232bf4e08ab5d88555c1b5756_Out_3);
            float4 _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0 = SAMPLE_TEXTURE2D(_Property_eafddcee5f7f4f66817085ef749c6b35_Out_0.tex, _Property_eafddcee5f7f4f66817085ef749c6b35_Out_0.samplerstate, _Property_eafddcee5f7f4f66817085ef749c6b35_Out_0.GetTransformedUV((_DTexSetCustomFunction_b0fd3e7232bf4e08ab5d88555c1b5756_Out_3.xy)));
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_R_4 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.r;
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_G_5 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.g;
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_B_6 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.b;
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_A_7 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.a;
            float _Property_a077d5d9cbf747f5875c6c24513b2800_Out_0 = _RDissolveIn;
            float _Property_0ff262777af74848aa81b3dba71541ad_Out_0 = _RDissolveOut;
            float4 _Property_7368f6d6acbe464fb71b082ec611715e_Out_0 = IsGammaSpace() ? LinearToSRGB(_RDissolveColor) : _RDissolveColor;
            UnityTexture2D _Property_c21f55442e394cf284a0766460ec9cac_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0 = SAMPLE_TEXTURE2D(_Property_c21f55442e394cf284a0766460ec9cac_Out_0.tex, _Property_c21f55442e394cf284a0766460ec9cac_Out_0.samplerstate, _Property_c21f55442e394cf284a0766460ec9cac_Out_0.GetTransformedUV((_RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3.xy)));
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_R_4 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.r;
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_G_5 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.g;
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_B_6 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.b;
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_A_7 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.a;
            float4 _Swizzle_400506fb40164c489617df53e573832a_Out_1 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_R_4.xxxx;
            float _Property_a5385ead93534fa7aa7ee63377c08787_Out_0 = _RLight;
            float4 _Property_d4de088caa5d43b99fa6421c07169156_Out_0 = IsGammaSpace() ? LinearToSRGB(_RColor) : _RColor;
            float _Property_d8081504f4364890a7d4a0f3971be926_Out_0 = _RLightGain;
            float4 _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3;
            TextureLight_float(_Swizzle_400506fb40164c489617df53e573832a_Out_1, _Property_a5385ead93534fa7aa7ee63377c08787_Out_0, _Property_d4de088caa5d43b99fa6421c07169156_Out_0, _Property_d8081504f4364890a7d4a0f3971be926_Out_0, _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3);
            float4 _DissolveCustomFunction_c1278429f28948e08cf996a8b62479a9_Out_3;
            Dissolve_float((_RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3.xy), _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0, _Property_a077d5d9cbf747f5875c6c24513b2800_Out_0, _Property_0ff262777af74848aa81b3dba71541ad_Out_0, _Property_7368f6d6acbe464fb71b082ec611715e_Out_0, _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3, _DissolveCustomFunction_c1278429f28948e08cf996a8b62479a9_Out_3);
            float _Property_3808652b3bd84f0dae18b3e1973a9e4b_Out_0 = _GSize;
            float _Property_4618d5267616447a8c6b7427eb1a57dd_Out_0 = _GRotate;
            float4 _RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3;
            RotateSize_float(_UV_8cea45496b8c45eab3597a074d15d476_Out_0, _Property_3808652b3bd84f0dae18b3e1973a9e4b_Out_0, _Property_4618d5267616447a8c6b7427eb1a57dd_Out_0, _RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3);
            UnityTexture2D _Property_d018d28ee214437a82076451743a8130_Out_0 = UnityBuildTexture2DStructNoScale(_DissolveTex);
            float4 _Property_b4cc761c375e4bf5b21dd8dcf9704990_Out_0 = _DissolveTex_ST;
            float4 _DTexSetCustomFunction_65a258090b234db4ab663709ae4ac4d4_Out_3;
            DTexSet_float(_RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3, _Property_b4cc761c375e4bf5b21dd8dcf9704990_Out_0, _DTexSetCustomFunction_65a258090b234db4ab663709ae4ac4d4_Out_3);
            float4 _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0 = SAMPLE_TEXTURE2D(_Property_d018d28ee214437a82076451743a8130_Out_0.tex, _Property_d018d28ee214437a82076451743a8130_Out_0.samplerstate, _Property_d018d28ee214437a82076451743a8130_Out_0.GetTransformedUV((_DTexSetCustomFunction_65a258090b234db4ab663709ae4ac4d4_Out_3.xy)));
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_R_4 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.r;
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_G_5 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.g;
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_B_6 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.b;
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_A_7 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.a;
            float _Property_6e040bc940b740d3954fb5ef6f7965de_Out_0 = _GDissolveIn;
            float _Property_6dce9fc004da445ab2b9bebb19d43137_Out_0 = _GDissolveOut;
            float4 _Property_5863bb3472fe4bf1800650465375edcd_Out_0 = IsGammaSpace() ? LinearToSRGB(_GDissolveColor) : _GDissolveColor;
            UnityTexture2D _Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0.tex, _Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0.samplerstate, _Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0.GetTransformedUV((_RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3.xy)));
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_R_4 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.r;
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_G_5 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.g;
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_B_6 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.b;
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_A_7 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.a;
            float4 _Swizzle_c89196372af64bba9bf916bb81898cc9_Out_1 = _SampleTexture2D_cddf3472524741888ba91b342532777d_G_5.xxxx;
            float _Property_07a37b638f724de594b4970af7e3d22f_Out_0 = _GLight;
            float4 _Property_d7f27dbc94dd4dbca6685d8c913bff10_Out_0 = IsGammaSpace() ? LinearToSRGB(_GColor) : _GColor;
            float _Property_a2e2d040f3874da48daa383fde697713_Out_0 = _GLightGain;
            float4 _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3;
            TextureLight_float(_Swizzle_c89196372af64bba9bf916bb81898cc9_Out_1, _Property_07a37b638f724de594b4970af7e3d22f_Out_0, _Property_d7f27dbc94dd4dbca6685d8c913bff10_Out_0, _Property_a2e2d040f3874da48daa383fde697713_Out_0, _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3);
            float4 _DissolveCustomFunction_c794717a85c44b778d53446bc5599b11_Out_3;
            Dissolve_float((_RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3.xy), _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0, _Property_6e040bc940b740d3954fb5ef6f7965de_Out_0, _Property_6dce9fc004da445ab2b9bebb19d43137_Out_0, _Property_5863bb3472fe4bf1800650465375edcd_Out_0, _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3, _DissolveCustomFunction_c794717a85c44b778d53446bc5599b11_Out_3);
            float _Property_8fb0e68fd07a405b921fa2706f8ae7f4_Out_0 = _BSize;
            float _Property_6040d22195bf4819ae19ecc7e8841069_Out_0 = _BRotate;
            float4 _RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3;
            RotateSize_float(_UV_8cea45496b8c45eab3597a074d15d476_Out_0, _Property_8fb0e68fd07a405b921fa2706f8ae7f4_Out_0, _Property_6040d22195bf4819ae19ecc7e8841069_Out_0, _RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3);
            UnityTexture2D _Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0 = UnityBuildTexture2DStructNoScale(_DissolveTex);
            float4 _Property_53bc3e203cdf45779cf450bcccc43ff8_Out_0 = _DissolveTex_ST;
            float4 _DTexSetCustomFunction_e8155feacdd943ddb8f4ff867df48696_Out_3;
            DTexSet_float(_RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3, _Property_53bc3e203cdf45779cf450bcccc43ff8_Out_0, _DTexSetCustomFunction_e8155feacdd943ddb8f4ff867df48696_Out_3);
            float4 _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0 = SAMPLE_TEXTURE2D(_Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0.tex, _Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0.samplerstate, _Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0.GetTransformedUV((_DTexSetCustomFunction_e8155feacdd943ddb8f4ff867df48696_Out_3.xy)));
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_R_4 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.r;
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_G_5 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.g;
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_B_6 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.b;
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_A_7 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.a;
            float _Property_df06dff0c2484fb595c137b1754753b9_Out_0 = _BDissolveIn;
            float _Property_d214b0d6c4864bde9efaa3db6db55a86_Out_0 = _BDissolveOut;
            float4 _Property_f2164c96cf7945fd975240ed7e800b18_Out_0 = IsGammaSpace() ? LinearToSRGB(_BDissolveColor) : _BDissolveColor;
            UnityTexture2D _Property_d2937aa927074faf8a7e3502b5f57de2_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0 = SAMPLE_TEXTURE2D(_Property_d2937aa927074faf8a7e3502b5f57de2_Out_0.tex, _Property_d2937aa927074faf8a7e3502b5f57de2_Out_0.samplerstate, _Property_d2937aa927074faf8a7e3502b5f57de2_Out_0.GetTransformedUV((_RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3.xy)));
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_R_4 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.r;
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_G_5 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.g;
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_B_6 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.b;
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_A_7 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.a;
            float4 _Swizzle_f28776abd40e4239a0500f742ca08953_Out_1 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_B_6.xxxx;
            float _Property_ff648668c0b14c5bb553df8ea7ab4c99_Out_0 = _BLight;
            float4 _Property_f2d51e21017b4d62854d0dd0d69a4eeb_Out_0 = IsGammaSpace() ? LinearToSRGB(_BColor) : _BColor;
            float _Property_2358423daf2b46bd88fc2c6f83681dab_Out_0 = _BLightGain;
            float4 _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3;
            TextureLight_float(_Swizzle_f28776abd40e4239a0500f742ca08953_Out_1, _Property_ff648668c0b14c5bb553df8ea7ab4c99_Out_0, _Property_f2d51e21017b4d62854d0dd0d69a4eeb_Out_0, _Property_2358423daf2b46bd88fc2c6f83681dab_Out_0, _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3);
            float4 _DissolveCustomFunction_18ec96fae3434badafbd66e66f5f7fc7_Out_3;
            Dissolve_float((_RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3.xy), _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0, _Property_df06dff0c2484fb595c137b1754753b9_Out_0, _Property_d214b0d6c4864bde9efaa3db6db55a86_Out_0, _Property_f2164c96cf7945fd975240ed7e800b18_Out_0, _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3, _DissolveCustomFunction_18ec96fae3434badafbd66e66f5f7fc7_Out_3);
            float4 _Add_b5aafd4ee22e4670a9b2a0eeae0b991b_Out_2;
            Unity_Add_float4(_DissolveCustomFunction_c794717a85c44b778d53446bc5599b11_Out_3, _DissolveCustomFunction_18ec96fae3434badafbd66e66f5f7fc7_Out_3, _Add_b5aafd4ee22e4670a9b2a0eeae0b991b_Out_2);
            float4 _Add_311c3433751947a9b670b04484a11b3b_Out_2;
            Unity_Add_float4(_DissolveCustomFunction_c1278429f28948e08cf996a8b62479a9_Out_3, _Add_b5aafd4ee22e4670a9b2a0eeae0b991b_Out_2, _Add_311c3433751947a9b670b04484a11b3b_Out_2);
            float _Swizzle_19e7c60d34834501ad98d57eecf88a20_Out_1 = _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3.w;
            float _Swizzle_2b2f7ab3dfd748fbb705d617d01b1690_Out_1 = _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3.w;
            float _Swizzle_d4f48dddbcae41cfbf129451e203ecf4_Out_1 = _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3.w;
            float _Add_d05af43285cb4cce8638140598473902_Out_2;
            Unity_Add_float(_Swizzle_2b2f7ab3dfd748fbb705d617d01b1690_Out_1, _Swizzle_d4f48dddbcae41cfbf129451e203ecf4_Out_1, _Add_d05af43285cb4cce8638140598473902_Out_2);
            float _Add_747b1754f01448a9bb57f2993c19791e_Out_2;
            Unity_Add_float(_Swizzle_19e7c60d34834501ad98d57eecf88a20_Out_1, _Add_d05af43285cb4cce8638140598473902_Out_2, _Add_747b1754f01448a9bb57f2993c19791e_Out_2);
            float4 _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGBA_4;
            float3 _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGB_5;
            float2 _Combine_94b4ca9170e44d93bdf05a483d08c44b_RG_6;
            Unity_Combine_float(0, 0, 0, _Add_747b1754f01448a9bb57f2993c19791e_Out_2, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGBA_4, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGB_5, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RG_6);
            float4 _Add_2670f6bd6b3f4beb94a86259f697e3cf_Out_2;
            Unity_Add_float4(_Add_311c3433751947a9b670b04484a11b3b_Out_2, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGBA_4, _Add_2670f6bd6b3f4beb94a86259f697e3cf_Out_2);
            float _Swizzle_6cead8bc7a22455b93e7f26073d331da_Out_1 = _Add_2670f6bd6b3f4beb94a86259f697e3cf_Out_2.w;
            surface.BaseColor = IsGammaSpace() ? float3(0, 0, 0) : SRGBToLinear(float3(0, 0, 0));
            surface.Alpha = _Swizzle_6cead8bc7a22455b93e7f26073d331da_Out_1;
            surface.NormalTS = IN.TangentSpaceNormal;
            surface.NormalAlpha = 1;
            surface.Emission = (_Add_2670f6bd6b3f4beb94a86259f697e3cf_Out_2.xyz);
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            
        //     $features.graphVertex:  $include("VertexAnimation.template.hlsl")
        //                                       ^ ERROR: $include cannot find file : VertexAnimation.template.hlsl. Looked into:
        // Packages/com.unity.shadergraph/Editor/Generation/Templates
        
            
        //     $features.graphPixel:   $include("SharedCode.template.hlsl")
        //                                       ^ ERROR: $include cannot find file : SharedCode.template.hlsl. Looked into:
        // Packages/com.unity.shadergraph/Editor/Generation/Templates
        
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorCopyToSDI' */
        
        
        
            output.TangentSpaceNormal =                         float3(0.0f, 0.0f, 1.0f);
        
        
            output.uv0 =                                        input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
            return output;
        }
        
            // --------------------------------------------------
            // Build Surface Data
        
            uint2 ComputeFadeMaskSeed(uint2 positionSS)
            {
                uint2 fadeMaskSeed;
        
                // Can't use the view direction, it is the same across the entire screen.
                fadeMaskSeed = positionSS;
        
                return fadeMaskSeed;
            }
        
            void GetSurfaceData(Varyings input, half3 viewDirectioWS, uint2 positionSS, float angleFadeFactor, out DecalSurfaceData surfaceData)
            {
                #if (SHADERPASS == SHADERPASS_DBUFFER_PROJECTOR) || (SHADERPASS == SHADERPASS_FORWARD_EMISSIVE_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_SCREEN_SPACE_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_GBUFFER_PROJECTOR)
                    half4x4 normalToWorld = UNITY_ACCESS_INSTANCED_PROP(Decal, _NormalToWorld);
                    half fadeFactor = clamp(normalToWorld[0][3], 0.0f, 1.0f) * angleFadeFactor;
                    float2 scale = float2(normalToWorld[3][0], normalToWorld[3][1]);
                    float2 offset = float2(normalToWorld[3][2], normalToWorld[3][3]);
                    input.texCoord0.xy = input.texCoord0.xy * scale + offset;
                #else
                    #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                        LODDitheringTransition(ComputeFadeMaskSeed(positionSS), unity_LODFade.x);
                    #endif
        
                    half fadeFactor = half(1.0);
                #endif
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = BuildSurfaceDescriptionInputs(input);
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
        
                // setup defaults -- these are used if the graph doesn't output a value
                ZERO_INITIALIZE(DecalSurfaceData, surfaceData);
                surfaceData.occlusion = half(1.0);
                surfaceData.smoothness = half(0);
        
                #ifdef _MATERIAL_AFFECTS_NORMAL
                    surfaceData.normalWS.w = half(1.0);
                #else
                    surfaceData.normalWS.w = half(0.0);
                #endif
        
                surfaceData.emissive.rgb = half3(surfaceDescription.Emission.rgb * fadeFactor);
        
                // copy across graph values, if defined
                surfaceData.baseColor.xyz = half3(surfaceDescription.BaseColor);
                surfaceData.baseColor.w = half(surfaceDescription.Alpha * fadeFactor);
        
                #if (SHADERPASS == SHADERPASS_DBUFFER_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_SCREEN_SPACE_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_GBUFFER_PROJECTOR)
                    #if defined(_MATERIAL_AFFECTS_NORMAL)
                        surfaceData.normalWS.xyz = mul((half3x3)normalToWorld, surfaceDescription.NormalTS.xyz);
                    #else
                        surfaceData.normalWS.xyz = normalToWorld[2].xyz;
                    #endif
                #elif (SHADERPASS == SHADERPASS_DBUFFER_MESH) || (SHADERPASS == SHADERPASS_DECAL_SCREEN_SPACE_MESH) || (SHADERPASS == SHADERPASS_DECAL_GBUFFER_MESH)
                    #if defined(_MATERIAL_AFFECTS_NORMAL)
                        float sgn = input.tangentWS.w;      // should be either +1 or -1
                        float3 bitangent = sgn * cross(input.normalWS.xyz, input.tangentWS.xyz);
                        half3x3 tangentToWorld = half3x3(input.tangentWS.xyz, bitangent.xyz, input.normalWS.xyz);
        
                        // We need to normalize as we use mikkt tangent space and this is expected (tangent space is not normalize)
                        surfaceData.normalWS.xyz = normalize(TransformTangentToWorld(surfaceDescription.NormalTS, tangentToWorld));
                    #else
                        surfaceData.normalWS.xyz = half3(input.normalWS); // Default to vertex normal
                    #endif
                #endif
        
                surfaceData.normalWS.w = surfaceDescription.NormalAlpha * fadeFactor;
        
                // In case of Smoothness / AO / Metal, all the three are always computed but color mask can change
            }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPassDecal.hlsl"
        
            ENDHLSL
        }
        Pass
        { 
            Name "DecalScreenSpaceMesh"
            Tags 
            { 
                "LightMode" = "DecalScreenSpaceMesh"
            }
        
            // Render State
            Blend SrcAlpha OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma target 2.5
        #pragma vertex Vert
        #pragma fragment Frag
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        
            // Keywords
            #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DYNAMICLIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
        #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
        #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
        #pragma multi_compile_fragment _ _SHADOWS_SOFT
        #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
        #pragma multi_compile _ SHADOWS_SHADOWMASK
        #pragma multi_compile _ _CLUSTERED_RENDERING
        #pragma multi_compile _DECAL_NORMAL_BLEND_LOW _DECAL_NORMAL_BLEND_MEDIUM _DECAL_NORMAL_BLEND_HIGH
            // GraphKeywords: <None>
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        
            // Defines
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define ATTRIBUTES_NEED_TEXCOORD2
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_VIEWDIRECTION_WS
            #define VARYINGS_NEED_TANGENT_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
            #define VARYINGS_NEED_SH
            #define VARYINGS_NEED_STATIC_LIGHTMAP_UV
            #define VARYINGS_NEED_DYNAMIC_LIGHTMAP_UV
            
            #define HAVE_MESH_MODIFICATION
        
        
            #define SHADERPASS SHADERPASS_DECAL_SCREEN_SPACE_MESH
        #define _MATERIAL_AFFECTS_ALBEDO 1
        #define _MATERIAL_AFFECTS_NORMAL 1
        #define _MATERIAL_AFFECTS_NORMAL_BLEND 1
        #define _MATERIAL_AFFECTS_EMISSION 1
        
            // HybridV1InjectedBuiltinProperties: <None>
        
            // -- Properties used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DecalInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderVariablesDecal.hlsl"
        
            // --------------------------------------------------
            // Structs and Packing
        
            struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 uv1 : TEXCOORD1;
             float4 uv2 : TEXCOORD2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 tangentWS;
             float4 texCoord0;
             float3 viewDirectionWS;
            #if defined(LIGHTMAP_ON)
             float2 staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
             float2 dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
             float3 sh;
            #endif
             float4 fogFactorAndVertexLight;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 TangentSpaceNormal;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float3 interp1 : INTERP1;
             float4 interp2 : INTERP2;
             float4 interp3 : INTERP3;
             float3 interp4 : INTERP4;
             float2 interp5 : INTERP5;
             float2 interp6 : INTERP6;
             float3 interp7 : INTERP7;
             float4 interp8 : INTERP8;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyz =  input.normalWS;
            output.interp2.xyzw =  input.tangentWS;
            output.interp3.xyzw =  input.texCoord0;
            output.interp4.xyz =  input.viewDirectionWS;
            #if defined(LIGHTMAP_ON)
            output.interp5.xy =  input.staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            output.interp6.xy =  input.dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.interp7.xyz =  input.sh;
            #endif
            output.interp8.xyzw =  input.fogFactorAndVertexLight;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.tangentWS = input.interp2.xyzw;
            output.texCoord0 = input.interp3.xyzw;
            output.viewDirectionWS = input.interp4.xyz;
            #if defined(LIGHTMAP_ON)
            output.staticLightmapUV = input.interp5.xy;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            output.dynamicLightmapUV = input.interp6.xy;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.sh = input.interp7.xyz;
            #endif
            output.fogFactorAndVertexLight = input.interp8.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float4 _DissolveTex_TexelSize;
        float _GroundResidue;
        float4 _RColor;
        float _RSize;
        float _RRotate;
        float _RLight;
        float4 _RDissolveColor;
        float _RLightGain;
        float _RDissolveIn;
        float _RDissolveOut;
        float4 _DissolveTex_ST;
        float4 _MainTex_ST;
        float4 _GColor;
        float _GSize;
        float _GRotate;
        float _GLight;
        float _GLightGain;
        float4 _GDissolveColor;
        float _GDissolveIn;
        float _GDissolveOut;
        float4 _BColor;
        float _BSize;
        float _BRotate;
        float _BLight;
        float _BLightGain;
        float4 _BDissolveColor;
        float _BDissolveIn;
        float _BDissolveOut;
        float _DrawOrder;
        float _DecalMeshBiasType;
        float _DecalMeshDepthBias;
        float _DecalMeshViewBias;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_DissolveTex);
        SAMPLER(sampler_DissolveTex);
        
            // Graph Functions
            
        void RotateSize_float(float4 _uv, float _size, float _rotate, out float4 Out){
                _rotate = _rotate*(3.14159265359/180);
            
                float2 ruv = float2(((_uv.x - 0.5) * 1/_size) + 0.5 ,((_uv.y - 0.5) * 1/_size) + 0.5);
            
                ruv = ruv - float2(0.5, 0.5);  
            
                ruv = float2(ruv.x * cos(_rotate) - ruv.y * sin(_rotate),ruv.x * sin(_rotate) + ruv.y * cos(_rotate));  
            
                ruv += float2(0.5, 0.5);
            
            
            
                Out = float4(ruv.x,ruv.y,_uv.z,_uv.w);
            
        }
        
        void DTexSet_float(float4 _uvD, float4 _DissolveTex_ST, out float4 Out){
             Out = float4((_uvD.x- 0.5) * _DissolveTex_ST.x+ _DissolveTex_ST.z,(_uvD.y- 0.5) * _DissolveTex_ST.y+ _DissolveTex_ST.w,_uvD.z,_uvD.w);
        }
        
        void TextureLight_float(float4 _tex, float _light, float4 _color, float _lightGain, out float4 Out){
                float4 RtexB = smoothstep( 1-_light,1,_tex);
            
                RtexB = RtexB * 0.2 + RtexB *  _color * _lightGain;
            
                _tex.rgb = _tex.rgb * _color.rgb + RtexB.rgb;
            
                _tex = _tex * _color.a *_tex.a ;
            
                Out = _tex;
        }
        
        void Dissolve_float(float2 _uv, float4 _Dtex, float _DissolveIn, float _DissolveOut, float4 _Color, float4 _tex, out float4 Out){
                float4 spheric = saturate( distance( _uv ,float2(0.5,0.5)) );
            
                spheric =saturate(spheric + spheric * _Dtex);
            
                           
            
                float4 spheric01A =saturate( saturate( (spheric + lerp(-1.01,0.01,1-_DissolveIn ))*100));
            
                float4 spheric01B = saturate( (spheric + lerp(-1.01,0.01,1-_DissolveIn- 0.01))*100);
            
                spheric01B = spheric01A * (1-spheric01B) * _Color * _Color.a;
            
            
            
                _tex = _tex * spheric01A + spheric01B * _tex.a;
            
            
            
                float4 spheric02A = saturate( ((1-spheric) + lerp(-1.01,0.01,_DissolveOut+ 0.01))*100);
            
                float4 spheric02B = saturate( ((1-spheric) + lerp(-1.01,0.01,_DissolveOut))*100);
            
                spheric02B =spheric02A * (1-spheric02B) * _Color * _Color.a;
            
                
            
                _tex = _tex * spheric02A + spheric02B * _tex.a;
            
                Out = _tex;
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
        {
            RGBA = float4(R, G, B, A);
            RGB = float3(R, G, B);
            RG = float2(R, G);
        }
        
            // Graph Vertex
            struct VertexDescription
        {
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            return description;
        }
            
            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float3 NormalTS;
            float NormalAlpha;
            float3 Emission;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _UV_8cea45496b8c45eab3597a074d15d476_Out_0 = IN.uv0;
            float _Property_7b769f55c7864589aebfac4ddd12b2d6_Out_0 = _RSize;
            float _Property_4acdc5976b734662994ffee635158980_Out_0 = _RRotate;
            float4 _RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3;
            RotateSize_float(_UV_8cea45496b8c45eab3597a074d15d476_Out_0, _Property_7b769f55c7864589aebfac4ddd12b2d6_Out_0, _Property_4acdc5976b734662994ffee635158980_Out_0, _RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3);
            UnityTexture2D _Property_eafddcee5f7f4f66817085ef749c6b35_Out_0 = UnityBuildTexture2DStructNoScale(_DissolveTex);
            float4 _Property_1b4f5806053747519e92207b6d7afbee_Out_0 = _DissolveTex_ST;
            float4 _DTexSetCustomFunction_b0fd3e7232bf4e08ab5d88555c1b5756_Out_3;
            DTexSet_float(_RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3, _Property_1b4f5806053747519e92207b6d7afbee_Out_0, _DTexSetCustomFunction_b0fd3e7232bf4e08ab5d88555c1b5756_Out_3);
            float4 _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0 = SAMPLE_TEXTURE2D(_Property_eafddcee5f7f4f66817085ef749c6b35_Out_0.tex, _Property_eafddcee5f7f4f66817085ef749c6b35_Out_0.samplerstate, _Property_eafddcee5f7f4f66817085ef749c6b35_Out_0.GetTransformedUV((_DTexSetCustomFunction_b0fd3e7232bf4e08ab5d88555c1b5756_Out_3.xy)));
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_R_4 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.r;
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_G_5 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.g;
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_B_6 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.b;
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_A_7 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.a;
            float _Property_a077d5d9cbf747f5875c6c24513b2800_Out_0 = _RDissolveIn;
            float _Property_0ff262777af74848aa81b3dba71541ad_Out_0 = _RDissolveOut;
            float4 _Property_7368f6d6acbe464fb71b082ec611715e_Out_0 = IsGammaSpace() ? LinearToSRGB(_RDissolveColor) : _RDissolveColor;
            UnityTexture2D _Property_c21f55442e394cf284a0766460ec9cac_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0 = SAMPLE_TEXTURE2D(_Property_c21f55442e394cf284a0766460ec9cac_Out_0.tex, _Property_c21f55442e394cf284a0766460ec9cac_Out_0.samplerstate, _Property_c21f55442e394cf284a0766460ec9cac_Out_0.GetTransformedUV((_RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3.xy)));
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_R_4 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.r;
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_G_5 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.g;
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_B_6 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.b;
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_A_7 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.a;
            float4 _Swizzle_400506fb40164c489617df53e573832a_Out_1 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_R_4.xxxx;
            float _Property_a5385ead93534fa7aa7ee63377c08787_Out_0 = _RLight;
            float4 _Property_d4de088caa5d43b99fa6421c07169156_Out_0 = IsGammaSpace() ? LinearToSRGB(_RColor) : _RColor;
            float _Property_d8081504f4364890a7d4a0f3971be926_Out_0 = _RLightGain;
            float4 _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3;
            TextureLight_float(_Swizzle_400506fb40164c489617df53e573832a_Out_1, _Property_a5385ead93534fa7aa7ee63377c08787_Out_0, _Property_d4de088caa5d43b99fa6421c07169156_Out_0, _Property_d8081504f4364890a7d4a0f3971be926_Out_0, _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3);
            float4 _DissolveCustomFunction_c1278429f28948e08cf996a8b62479a9_Out_3;
            Dissolve_float((_RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3.xy), _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0, _Property_a077d5d9cbf747f5875c6c24513b2800_Out_0, _Property_0ff262777af74848aa81b3dba71541ad_Out_0, _Property_7368f6d6acbe464fb71b082ec611715e_Out_0, _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3, _DissolveCustomFunction_c1278429f28948e08cf996a8b62479a9_Out_3);
            float _Property_3808652b3bd84f0dae18b3e1973a9e4b_Out_0 = _GSize;
            float _Property_4618d5267616447a8c6b7427eb1a57dd_Out_0 = _GRotate;
            float4 _RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3;
            RotateSize_float(_UV_8cea45496b8c45eab3597a074d15d476_Out_0, _Property_3808652b3bd84f0dae18b3e1973a9e4b_Out_0, _Property_4618d5267616447a8c6b7427eb1a57dd_Out_0, _RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3);
            UnityTexture2D _Property_d018d28ee214437a82076451743a8130_Out_0 = UnityBuildTexture2DStructNoScale(_DissolveTex);
            float4 _Property_b4cc761c375e4bf5b21dd8dcf9704990_Out_0 = _DissolveTex_ST;
            float4 _DTexSetCustomFunction_65a258090b234db4ab663709ae4ac4d4_Out_3;
            DTexSet_float(_RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3, _Property_b4cc761c375e4bf5b21dd8dcf9704990_Out_0, _DTexSetCustomFunction_65a258090b234db4ab663709ae4ac4d4_Out_3);
            float4 _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0 = SAMPLE_TEXTURE2D(_Property_d018d28ee214437a82076451743a8130_Out_0.tex, _Property_d018d28ee214437a82076451743a8130_Out_0.samplerstate, _Property_d018d28ee214437a82076451743a8130_Out_0.GetTransformedUV((_DTexSetCustomFunction_65a258090b234db4ab663709ae4ac4d4_Out_3.xy)));
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_R_4 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.r;
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_G_5 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.g;
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_B_6 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.b;
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_A_7 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.a;
            float _Property_6e040bc940b740d3954fb5ef6f7965de_Out_0 = _GDissolveIn;
            float _Property_6dce9fc004da445ab2b9bebb19d43137_Out_0 = _GDissolveOut;
            float4 _Property_5863bb3472fe4bf1800650465375edcd_Out_0 = IsGammaSpace() ? LinearToSRGB(_GDissolveColor) : _GDissolveColor;
            UnityTexture2D _Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0.tex, _Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0.samplerstate, _Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0.GetTransformedUV((_RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3.xy)));
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_R_4 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.r;
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_G_5 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.g;
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_B_6 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.b;
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_A_7 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.a;
            float4 _Swizzle_c89196372af64bba9bf916bb81898cc9_Out_1 = _SampleTexture2D_cddf3472524741888ba91b342532777d_G_5.xxxx;
            float _Property_07a37b638f724de594b4970af7e3d22f_Out_0 = _GLight;
            float4 _Property_d7f27dbc94dd4dbca6685d8c913bff10_Out_0 = IsGammaSpace() ? LinearToSRGB(_GColor) : _GColor;
            float _Property_a2e2d040f3874da48daa383fde697713_Out_0 = _GLightGain;
            float4 _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3;
            TextureLight_float(_Swizzle_c89196372af64bba9bf916bb81898cc9_Out_1, _Property_07a37b638f724de594b4970af7e3d22f_Out_0, _Property_d7f27dbc94dd4dbca6685d8c913bff10_Out_0, _Property_a2e2d040f3874da48daa383fde697713_Out_0, _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3);
            float4 _DissolveCustomFunction_c794717a85c44b778d53446bc5599b11_Out_3;
            Dissolve_float((_RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3.xy), _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0, _Property_6e040bc940b740d3954fb5ef6f7965de_Out_0, _Property_6dce9fc004da445ab2b9bebb19d43137_Out_0, _Property_5863bb3472fe4bf1800650465375edcd_Out_0, _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3, _DissolveCustomFunction_c794717a85c44b778d53446bc5599b11_Out_3);
            float _Property_8fb0e68fd07a405b921fa2706f8ae7f4_Out_0 = _BSize;
            float _Property_6040d22195bf4819ae19ecc7e8841069_Out_0 = _BRotate;
            float4 _RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3;
            RotateSize_float(_UV_8cea45496b8c45eab3597a074d15d476_Out_0, _Property_8fb0e68fd07a405b921fa2706f8ae7f4_Out_0, _Property_6040d22195bf4819ae19ecc7e8841069_Out_0, _RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3);
            UnityTexture2D _Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0 = UnityBuildTexture2DStructNoScale(_DissolveTex);
            float4 _Property_53bc3e203cdf45779cf450bcccc43ff8_Out_0 = _DissolveTex_ST;
            float4 _DTexSetCustomFunction_e8155feacdd943ddb8f4ff867df48696_Out_3;
            DTexSet_float(_RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3, _Property_53bc3e203cdf45779cf450bcccc43ff8_Out_0, _DTexSetCustomFunction_e8155feacdd943ddb8f4ff867df48696_Out_3);
            float4 _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0 = SAMPLE_TEXTURE2D(_Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0.tex, _Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0.samplerstate, _Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0.GetTransformedUV((_DTexSetCustomFunction_e8155feacdd943ddb8f4ff867df48696_Out_3.xy)));
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_R_4 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.r;
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_G_5 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.g;
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_B_6 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.b;
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_A_7 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.a;
            float _Property_df06dff0c2484fb595c137b1754753b9_Out_0 = _BDissolveIn;
            float _Property_d214b0d6c4864bde9efaa3db6db55a86_Out_0 = _BDissolveOut;
            float4 _Property_f2164c96cf7945fd975240ed7e800b18_Out_0 = IsGammaSpace() ? LinearToSRGB(_BDissolveColor) : _BDissolveColor;
            UnityTexture2D _Property_d2937aa927074faf8a7e3502b5f57de2_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0 = SAMPLE_TEXTURE2D(_Property_d2937aa927074faf8a7e3502b5f57de2_Out_0.tex, _Property_d2937aa927074faf8a7e3502b5f57de2_Out_0.samplerstate, _Property_d2937aa927074faf8a7e3502b5f57de2_Out_0.GetTransformedUV((_RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3.xy)));
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_R_4 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.r;
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_G_5 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.g;
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_B_6 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.b;
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_A_7 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.a;
            float4 _Swizzle_f28776abd40e4239a0500f742ca08953_Out_1 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_B_6.xxxx;
            float _Property_ff648668c0b14c5bb553df8ea7ab4c99_Out_0 = _BLight;
            float4 _Property_f2d51e21017b4d62854d0dd0d69a4eeb_Out_0 = IsGammaSpace() ? LinearToSRGB(_BColor) : _BColor;
            float _Property_2358423daf2b46bd88fc2c6f83681dab_Out_0 = _BLightGain;
            float4 _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3;
            TextureLight_float(_Swizzle_f28776abd40e4239a0500f742ca08953_Out_1, _Property_ff648668c0b14c5bb553df8ea7ab4c99_Out_0, _Property_f2d51e21017b4d62854d0dd0d69a4eeb_Out_0, _Property_2358423daf2b46bd88fc2c6f83681dab_Out_0, _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3);
            float4 _DissolveCustomFunction_18ec96fae3434badafbd66e66f5f7fc7_Out_3;
            Dissolve_float((_RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3.xy), _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0, _Property_df06dff0c2484fb595c137b1754753b9_Out_0, _Property_d214b0d6c4864bde9efaa3db6db55a86_Out_0, _Property_f2164c96cf7945fd975240ed7e800b18_Out_0, _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3, _DissolveCustomFunction_18ec96fae3434badafbd66e66f5f7fc7_Out_3);
            float4 _Add_b5aafd4ee22e4670a9b2a0eeae0b991b_Out_2;
            Unity_Add_float4(_DissolveCustomFunction_c794717a85c44b778d53446bc5599b11_Out_3, _DissolveCustomFunction_18ec96fae3434badafbd66e66f5f7fc7_Out_3, _Add_b5aafd4ee22e4670a9b2a0eeae0b991b_Out_2);
            float4 _Add_311c3433751947a9b670b04484a11b3b_Out_2;
            Unity_Add_float4(_DissolveCustomFunction_c1278429f28948e08cf996a8b62479a9_Out_3, _Add_b5aafd4ee22e4670a9b2a0eeae0b991b_Out_2, _Add_311c3433751947a9b670b04484a11b3b_Out_2);
            float _Swizzle_19e7c60d34834501ad98d57eecf88a20_Out_1 = _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3.w;
            float _Swizzle_2b2f7ab3dfd748fbb705d617d01b1690_Out_1 = _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3.w;
            float _Swizzle_d4f48dddbcae41cfbf129451e203ecf4_Out_1 = _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3.w;
            float _Add_d05af43285cb4cce8638140598473902_Out_2;
            Unity_Add_float(_Swizzle_2b2f7ab3dfd748fbb705d617d01b1690_Out_1, _Swizzle_d4f48dddbcae41cfbf129451e203ecf4_Out_1, _Add_d05af43285cb4cce8638140598473902_Out_2);
            float _Add_747b1754f01448a9bb57f2993c19791e_Out_2;
            Unity_Add_float(_Swizzle_19e7c60d34834501ad98d57eecf88a20_Out_1, _Add_d05af43285cb4cce8638140598473902_Out_2, _Add_747b1754f01448a9bb57f2993c19791e_Out_2);
            float4 _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGBA_4;
            float3 _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGB_5;
            float2 _Combine_94b4ca9170e44d93bdf05a483d08c44b_RG_6;
            Unity_Combine_float(0, 0, 0, _Add_747b1754f01448a9bb57f2993c19791e_Out_2, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGBA_4, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGB_5, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RG_6);
            float4 _Add_2670f6bd6b3f4beb94a86259f697e3cf_Out_2;
            Unity_Add_float4(_Add_311c3433751947a9b670b04484a11b3b_Out_2, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGBA_4, _Add_2670f6bd6b3f4beb94a86259f697e3cf_Out_2);
            float _Swizzle_6cead8bc7a22455b93e7f26073d331da_Out_1 = _Add_2670f6bd6b3f4beb94a86259f697e3cf_Out_2.w;
            surface.BaseColor = IsGammaSpace() ? float3(0, 0, 0) : SRGBToLinear(float3(0, 0, 0));
            surface.Alpha = _Swizzle_6cead8bc7a22455b93e7f26073d331da_Out_1;
            surface.NormalTS = IN.TangentSpaceNormal;
            surface.NormalAlpha = 1;
            surface.Emission = (_Add_2670f6bd6b3f4beb94a86259f697e3cf_Out_2.xyz);
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            
        //     $features.graphVertex:  $include("VertexAnimation.template.hlsl")
        //                                       ^ ERROR: $include cannot find file : VertexAnimation.template.hlsl. Looked into:
        // Packages/com.unity.shadergraph/Editor/Generation/Templates
        
            
        //     $features.graphPixel:   $include("SharedCode.template.hlsl")
        //                                       ^ ERROR: $include cannot find file : SharedCode.template.hlsl. Looked into:
        // Packages/com.unity.shadergraph/Editor/Generation/Templates
        
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorCopyToSDI' */
        
        
        
            output.TangentSpaceNormal =                         float3(0.0f, 0.0f, 1.0f);
        
        
            output.uv0 =                                        input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
            return output;
        }
        
            // --------------------------------------------------
            // Build Surface Data
        
            uint2 ComputeFadeMaskSeed(uint2 positionSS)
            {
                uint2 fadeMaskSeed;
        
                // Can't use the view direction, it is the same across the entire screen.
                fadeMaskSeed = positionSS;
        
                return fadeMaskSeed;
            }
        
            void GetSurfaceData(Varyings input, half3 viewDirectioWS, uint2 positionSS, float angleFadeFactor, out DecalSurfaceData surfaceData)
            {
                #if (SHADERPASS == SHADERPASS_DBUFFER_PROJECTOR) || (SHADERPASS == SHADERPASS_FORWARD_EMISSIVE_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_SCREEN_SPACE_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_GBUFFER_PROJECTOR)
                    half4x4 normalToWorld = UNITY_ACCESS_INSTANCED_PROP(Decal, _NormalToWorld);
                    half fadeFactor = clamp(normalToWorld[0][3], 0.0f, 1.0f) * angleFadeFactor;
                    float2 scale = float2(normalToWorld[3][0], normalToWorld[3][1]);
                    float2 offset = float2(normalToWorld[3][2], normalToWorld[3][3]);
                    input.texCoord0.xy = input.texCoord0.xy * scale + offset;
                #else
                    #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                        LODDitheringTransition(ComputeFadeMaskSeed(positionSS), unity_LODFade.x);
                    #endif
        
                    half fadeFactor = half(1.0);
                #endif
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = BuildSurfaceDescriptionInputs(input);
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
        
                // setup defaults -- these are used if the graph doesn't output a value
                ZERO_INITIALIZE(DecalSurfaceData, surfaceData);
                surfaceData.occlusion = half(1.0);
                surfaceData.smoothness = half(0);
        
                #ifdef _MATERIAL_AFFECTS_NORMAL
                    surfaceData.normalWS.w = half(1.0);
                #else
                    surfaceData.normalWS.w = half(0.0);
                #endif
        
                surfaceData.emissive.rgb = half3(surfaceDescription.Emission.rgb * fadeFactor);
        
                // copy across graph values, if defined
                surfaceData.baseColor.xyz = half3(surfaceDescription.BaseColor);
                surfaceData.baseColor.w = half(surfaceDescription.Alpha * fadeFactor);
        
                #if (SHADERPASS == SHADERPASS_DBUFFER_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_SCREEN_SPACE_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_GBUFFER_PROJECTOR)
                    #if defined(_MATERIAL_AFFECTS_NORMAL)
                        surfaceData.normalWS.xyz = mul((half3x3)normalToWorld, surfaceDescription.NormalTS.xyz);
                    #else
                        surfaceData.normalWS.xyz = normalToWorld[2].xyz;
                    #endif
                #elif (SHADERPASS == SHADERPASS_DBUFFER_MESH) || (SHADERPASS == SHADERPASS_DECAL_SCREEN_SPACE_MESH) || (SHADERPASS == SHADERPASS_DECAL_GBUFFER_MESH)
                    #if defined(_MATERIAL_AFFECTS_NORMAL)
                        float sgn = input.tangentWS.w;      // should be either +1 or -1
                        float3 bitangent = sgn * cross(input.normalWS.xyz, input.tangentWS.xyz);
                        half3x3 tangentToWorld = half3x3(input.tangentWS.xyz, bitangent.xyz, input.normalWS.xyz);
        
                        // We need to normalize as we use mikkt tangent space and this is expected (tangent space is not normalize)
                        surfaceData.normalWS.xyz = normalize(TransformTangentToWorld(surfaceDescription.NormalTS, tangentToWorld));
                    #else
                        surfaceData.normalWS.xyz = half3(input.normalWS); // Default to vertex normal
                    #endif
                #endif
        
                surfaceData.normalWS.w = surfaceDescription.NormalAlpha * fadeFactor;
        
                // In case of Smoothness / AO / Metal, all the three are always computed but color mask can change
            }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPassDecal.hlsl"
        
            ENDHLSL
        }
        Pass
        { 
            Name "DecalGBufferMesh"
            Tags 
            { 
                "LightMode" = "DecalGBufferMesh"
            }
        
            // Render State
            Blend 0 SrcAlpha OneMinusSrcAlpha
        Blend 1 SrcAlpha OneMinusSrcAlpha
        Blend 2 SrcAlpha OneMinusSrcAlpha
        Blend 3 SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        ColorMask RGB
        ColorMask 0 1
        ColorMask RGB 2
        ColorMask RGB 3
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma target 3.5
        #pragma vertex Vert
        #pragma fragment Frag
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        
            // Keywords
            #pragma multi_compile _ LIGHTMAP_ON
        #pragma multi_compile _ DYNAMICLIGHTMAP_ON
        #pragma multi_compile _ DIRLIGHTMAP_COMBINED
        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
        #pragma multi_compile_fragment _ _SHADOWS_SOFT
        #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
        #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
        #pragma multi_compile _DECAL_NORMAL_BLEND_LOW _DECAL_NORMAL_BLEND_MEDIUM _DECAL_NORMAL_BLEND_HIGH
        #pragma multi_compile_fragment _ _GBUFFER_NORMALS_OCT
            // GraphKeywords: <None>
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        
            // Defines
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define ATTRIBUTES_NEED_TEXCOORD2
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_NORMAL_WS
            #define VARYINGS_NEED_VIEWDIRECTION_WS
            #define VARYINGS_NEED_TANGENT_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
            #define VARYINGS_NEED_SH
            #define VARYINGS_NEED_STATIC_LIGHTMAP_UV
            #define VARYINGS_NEED_DYNAMIC_LIGHTMAP_UV
            
            #define HAVE_MESH_MODIFICATION
        
        
            #define SHADERPASS SHADERPASS_DECAL_GBUFFER_MESH
        #define _MATERIAL_AFFECTS_ALBEDO 1
        #define _MATERIAL_AFFECTS_NORMAL 1
        #define _MATERIAL_AFFECTS_NORMAL_BLEND 1
        #define _MATERIAL_AFFECTS_EMISSION 1
        
            // HybridV1InjectedBuiltinProperties: <None>
        
            // -- Properties used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityGBuffer.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DecalInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderVariablesDecal.hlsl"
        
            // --------------------------------------------------
            // Structs and Packing
        
            struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 uv1 : TEXCOORD1;
             float4 uv2 : TEXCOORD2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 tangentWS;
             float4 texCoord0;
             float3 viewDirectionWS;
            #if defined(LIGHTMAP_ON)
             float2 staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
             float2 dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
             float3 sh;
            #endif
             float4 fogFactorAndVertexLight;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 TangentSpaceNormal;
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float3 interp0 : INTERP0;
             float3 interp1 : INTERP1;
             float4 interp2 : INTERP2;
             float4 interp3 : INTERP3;
             float3 interp4 : INTERP4;
             float2 interp5 : INTERP5;
             float2 interp6 : INTERP6;
             float3 interp7 : INTERP7;
             float4 interp8 : INTERP8;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.interp0.xyz =  input.positionWS;
            output.interp1.xyz =  input.normalWS;
            output.interp2.xyzw =  input.tangentWS;
            output.interp3.xyzw =  input.texCoord0;
            output.interp4.xyz =  input.viewDirectionWS;
            #if defined(LIGHTMAP_ON)
            output.interp5.xy =  input.staticLightmapUV;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            output.interp6.xy =  input.dynamicLightmapUV;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.interp7.xyz =  input.sh;
            #endif
            output.interp8.xyzw =  input.fogFactorAndVertexLight;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.positionWS = input.interp0.xyz;
            output.normalWS = input.interp1.xyz;
            output.tangentWS = input.interp2.xyzw;
            output.texCoord0 = input.interp3.xyzw;
            output.viewDirectionWS = input.interp4.xyz;
            #if defined(LIGHTMAP_ON)
            output.staticLightmapUV = input.interp5.xy;
            #endif
            #if defined(DYNAMICLIGHTMAP_ON)
            output.dynamicLightmapUV = input.interp6.xy;
            #endif
            #if !defined(LIGHTMAP_ON)
            output.sh = input.interp7.xyz;
            #endif
            output.fogFactorAndVertexLight = input.interp8.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float4 _DissolveTex_TexelSize;
        float _GroundResidue;
        float4 _RColor;
        float _RSize;
        float _RRotate;
        float _RLight;
        float4 _RDissolveColor;
        float _RLightGain;
        float _RDissolveIn;
        float _RDissolveOut;
        float4 _DissolveTex_ST;
        float4 _MainTex_ST;
        float4 _GColor;
        float _GSize;
        float _GRotate;
        float _GLight;
        float _GLightGain;
        float4 _GDissolveColor;
        float _GDissolveIn;
        float _GDissolveOut;
        float4 _BColor;
        float _BSize;
        float _BRotate;
        float _BLight;
        float _BLightGain;
        float4 _BDissolveColor;
        float _BDissolveIn;
        float _BDissolveOut;
        float _DrawOrder;
        float _DecalMeshBiasType;
        float _DecalMeshDepthBias;
        float _DecalMeshViewBias;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_DissolveTex);
        SAMPLER(sampler_DissolveTex);
        
            // Graph Functions
            
        void RotateSize_float(float4 _uv, float _size, float _rotate, out float4 Out){
                _rotate = _rotate*(3.14159265359/180);
            
                float2 ruv = float2(((_uv.x - 0.5) * 1/_size) + 0.5 ,((_uv.y - 0.5) * 1/_size) + 0.5);
            
                ruv = ruv - float2(0.5, 0.5);  
            
                ruv = float2(ruv.x * cos(_rotate) - ruv.y * sin(_rotate),ruv.x * sin(_rotate) + ruv.y * cos(_rotate));  
            
                ruv += float2(0.5, 0.5);
            
            
            
                Out = float4(ruv.x,ruv.y,_uv.z,_uv.w);
            
        }
        
        void DTexSet_float(float4 _uvD, float4 _DissolveTex_ST, out float4 Out){
             Out = float4((_uvD.x- 0.5) * _DissolveTex_ST.x+ _DissolveTex_ST.z,(_uvD.y- 0.5) * _DissolveTex_ST.y+ _DissolveTex_ST.w,_uvD.z,_uvD.w);
        }
        
        void TextureLight_float(float4 _tex, float _light, float4 _color, float _lightGain, out float4 Out){
                float4 RtexB = smoothstep( 1-_light,1,_tex);
            
                RtexB = RtexB * 0.2 + RtexB *  _color * _lightGain;
            
                _tex.rgb = _tex.rgb * _color.rgb + RtexB.rgb;
            
                _tex = _tex * _color.a *_tex.a ;
            
                Out = _tex;
        }
        
        void Dissolve_float(float2 _uv, float4 _Dtex, float _DissolveIn, float _DissolveOut, float4 _Color, float4 _tex, out float4 Out){
                float4 spheric = saturate( distance( _uv ,float2(0.5,0.5)) );
            
                spheric =saturate(spheric + spheric * _Dtex);
            
                           
            
                float4 spheric01A =saturate( saturate( (spheric + lerp(-1.01,0.01,1-_DissolveIn ))*100));
            
                float4 spheric01B = saturate( (spheric + lerp(-1.01,0.01,1-_DissolveIn- 0.01))*100);
            
                spheric01B = spheric01A * (1-spheric01B) * _Color * _Color.a;
            
            
            
                _tex = _tex * spheric01A + spheric01B * _tex.a;
            
            
            
                float4 spheric02A = saturate( ((1-spheric) + lerp(-1.01,0.01,_DissolveOut+ 0.01))*100);
            
                float4 spheric02B = saturate( ((1-spheric) + lerp(-1.01,0.01,_DissolveOut))*100);
            
                spheric02B =spheric02A * (1-spheric02B) * _Color * _Color.a;
            
                
            
                _tex = _tex * spheric02A + spheric02B * _tex.a;
            
                Out = _tex;
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
        {
            RGBA = float4(R, G, B, A);
            RGB = float3(R, G, B);
            RG = float2(R, G);
        }
        
            // Graph Vertex
            struct VertexDescription
        {
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            return description;
        }
            
            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float3 NormalTS;
            float NormalAlpha;
            float3 Emission;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            float4 _UV_8cea45496b8c45eab3597a074d15d476_Out_0 = IN.uv0;
            float _Property_7b769f55c7864589aebfac4ddd12b2d6_Out_0 = _RSize;
            float _Property_4acdc5976b734662994ffee635158980_Out_0 = _RRotate;
            float4 _RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3;
            RotateSize_float(_UV_8cea45496b8c45eab3597a074d15d476_Out_0, _Property_7b769f55c7864589aebfac4ddd12b2d6_Out_0, _Property_4acdc5976b734662994ffee635158980_Out_0, _RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3);
            UnityTexture2D _Property_eafddcee5f7f4f66817085ef749c6b35_Out_0 = UnityBuildTexture2DStructNoScale(_DissolveTex);
            float4 _Property_1b4f5806053747519e92207b6d7afbee_Out_0 = _DissolveTex_ST;
            float4 _DTexSetCustomFunction_b0fd3e7232bf4e08ab5d88555c1b5756_Out_3;
            DTexSet_float(_RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3, _Property_1b4f5806053747519e92207b6d7afbee_Out_0, _DTexSetCustomFunction_b0fd3e7232bf4e08ab5d88555c1b5756_Out_3);
            float4 _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0 = SAMPLE_TEXTURE2D(_Property_eafddcee5f7f4f66817085ef749c6b35_Out_0.tex, _Property_eafddcee5f7f4f66817085ef749c6b35_Out_0.samplerstate, _Property_eafddcee5f7f4f66817085ef749c6b35_Out_0.GetTransformedUV((_DTexSetCustomFunction_b0fd3e7232bf4e08ab5d88555c1b5756_Out_3.xy)));
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_R_4 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.r;
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_G_5 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.g;
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_B_6 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.b;
            float _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_A_7 = _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0.a;
            float _Property_a077d5d9cbf747f5875c6c24513b2800_Out_0 = _RDissolveIn;
            float _Property_0ff262777af74848aa81b3dba71541ad_Out_0 = _RDissolveOut;
            float4 _Property_7368f6d6acbe464fb71b082ec611715e_Out_0 = IsGammaSpace() ? LinearToSRGB(_RDissolveColor) : _RDissolveColor;
            UnityTexture2D _Property_c21f55442e394cf284a0766460ec9cac_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0 = SAMPLE_TEXTURE2D(_Property_c21f55442e394cf284a0766460ec9cac_Out_0.tex, _Property_c21f55442e394cf284a0766460ec9cac_Out_0.samplerstate, _Property_c21f55442e394cf284a0766460ec9cac_Out_0.GetTransformedUV((_RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3.xy)));
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_R_4 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.r;
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_G_5 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.g;
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_B_6 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.b;
            float _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_A_7 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_RGBA_0.a;
            float4 _Swizzle_400506fb40164c489617df53e573832a_Out_1 = _SampleTexture2D_470f8f2d8abd4b5daad26b4d9dcaa531_R_4.xxxx;
            float _Property_a5385ead93534fa7aa7ee63377c08787_Out_0 = _RLight;
            float4 _Property_d4de088caa5d43b99fa6421c07169156_Out_0 = IsGammaSpace() ? LinearToSRGB(_RColor) : _RColor;
            float _Property_d8081504f4364890a7d4a0f3971be926_Out_0 = _RLightGain;
            float4 _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3;
            TextureLight_float(_Swizzle_400506fb40164c489617df53e573832a_Out_1, _Property_a5385ead93534fa7aa7ee63377c08787_Out_0, _Property_d4de088caa5d43b99fa6421c07169156_Out_0, _Property_d8081504f4364890a7d4a0f3971be926_Out_0, _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3);
            float4 _DissolveCustomFunction_c1278429f28948e08cf996a8b62479a9_Out_3;
            Dissolve_float((_RotateSizeCustomFunction_24233aab1add4a549917bf1ec5fd5bcd_Out_3.xy), _SampleTexture2D_b00b46e9f80f4d6f8f67cfcfca553230_RGBA_0, _Property_a077d5d9cbf747f5875c6c24513b2800_Out_0, _Property_0ff262777af74848aa81b3dba71541ad_Out_0, _Property_7368f6d6acbe464fb71b082ec611715e_Out_0, _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3, _DissolveCustomFunction_c1278429f28948e08cf996a8b62479a9_Out_3);
            float _Property_3808652b3bd84f0dae18b3e1973a9e4b_Out_0 = _GSize;
            float _Property_4618d5267616447a8c6b7427eb1a57dd_Out_0 = _GRotate;
            float4 _RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3;
            RotateSize_float(_UV_8cea45496b8c45eab3597a074d15d476_Out_0, _Property_3808652b3bd84f0dae18b3e1973a9e4b_Out_0, _Property_4618d5267616447a8c6b7427eb1a57dd_Out_0, _RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3);
            UnityTexture2D _Property_d018d28ee214437a82076451743a8130_Out_0 = UnityBuildTexture2DStructNoScale(_DissolveTex);
            float4 _Property_b4cc761c375e4bf5b21dd8dcf9704990_Out_0 = _DissolveTex_ST;
            float4 _DTexSetCustomFunction_65a258090b234db4ab663709ae4ac4d4_Out_3;
            DTexSet_float(_RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3, _Property_b4cc761c375e4bf5b21dd8dcf9704990_Out_0, _DTexSetCustomFunction_65a258090b234db4ab663709ae4ac4d4_Out_3);
            float4 _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0 = SAMPLE_TEXTURE2D(_Property_d018d28ee214437a82076451743a8130_Out_0.tex, _Property_d018d28ee214437a82076451743a8130_Out_0.samplerstate, _Property_d018d28ee214437a82076451743a8130_Out_0.GetTransformedUV((_DTexSetCustomFunction_65a258090b234db4ab663709ae4ac4d4_Out_3.xy)));
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_R_4 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.r;
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_G_5 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.g;
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_B_6 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.b;
            float _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_A_7 = _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0.a;
            float _Property_6e040bc940b740d3954fb5ef6f7965de_Out_0 = _GDissolveIn;
            float _Property_6dce9fc004da445ab2b9bebb19d43137_Out_0 = _GDissolveOut;
            float4 _Property_5863bb3472fe4bf1800650465375edcd_Out_0 = IsGammaSpace() ? LinearToSRGB(_GDissolveColor) : _GDissolveColor;
            UnityTexture2D _Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0.tex, _Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0.samplerstate, _Property_0364d5bf646c4ad3851365ef82e7fcfe_Out_0.GetTransformedUV((_RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3.xy)));
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_R_4 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.r;
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_G_5 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.g;
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_B_6 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.b;
            float _SampleTexture2D_cddf3472524741888ba91b342532777d_A_7 = _SampleTexture2D_cddf3472524741888ba91b342532777d_RGBA_0.a;
            float4 _Swizzle_c89196372af64bba9bf916bb81898cc9_Out_1 = _SampleTexture2D_cddf3472524741888ba91b342532777d_G_5.xxxx;
            float _Property_07a37b638f724de594b4970af7e3d22f_Out_0 = _GLight;
            float4 _Property_d7f27dbc94dd4dbca6685d8c913bff10_Out_0 = IsGammaSpace() ? LinearToSRGB(_GColor) : _GColor;
            float _Property_a2e2d040f3874da48daa383fde697713_Out_0 = _GLightGain;
            float4 _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3;
            TextureLight_float(_Swizzle_c89196372af64bba9bf916bb81898cc9_Out_1, _Property_07a37b638f724de594b4970af7e3d22f_Out_0, _Property_d7f27dbc94dd4dbca6685d8c913bff10_Out_0, _Property_a2e2d040f3874da48daa383fde697713_Out_0, _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3);
            float4 _DissolveCustomFunction_c794717a85c44b778d53446bc5599b11_Out_3;
            Dissolve_float((_RotateSizeCustomFunction_416da4e45fca41d7a099b13bb44dc33c_Out_3.xy), _SampleTexture2D_4bf720ff584a4d30aa1586f44e581cdc_RGBA_0, _Property_6e040bc940b740d3954fb5ef6f7965de_Out_0, _Property_6dce9fc004da445ab2b9bebb19d43137_Out_0, _Property_5863bb3472fe4bf1800650465375edcd_Out_0, _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3, _DissolveCustomFunction_c794717a85c44b778d53446bc5599b11_Out_3);
            float _Property_8fb0e68fd07a405b921fa2706f8ae7f4_Out_0 = _BSize;
            float _Property_6040d22195bf4819ae19ecc7e8841069_Out_0 = _BRotate;
            float4 _RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3;
            RotateSize_float(_UV_8cea45496b8c45eab3597a074d15d476_Out_0, _Property_8fb0e68fd07a405b921fa2706f8ae7f4_Out_0, _Property_6040d22195bf4819ae19ecc7e8841069_Out_0, _RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3);
            UnityTexture2D _Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0 = UnityBuildTexture2DStructNoScale(_DissolveTex);
            float4 _Property_53bc3e203cdf45779cf450bcccc43ff8_Out_0 = _DissolveTex_ST;
            float4 _DTexSetCustomFunction_e8155feacdd943ddb8f4ff867df48696_Out_3;
            DTexSet_float(_RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3, _Property_53bc3e203cdf45779cf450bcccc43ff8_Out_0, _DTexSetCustomFunction_e8155feacdd943ddb8f4ff867df48696_Out_3);
            float4 _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0 = SAMPLE_TEXTURE2D(_Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0.tex, _Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0.samplerstate, _Property_74ad31f4a3dc4104ab1b33ac5bdf5ea4_Out_0.GetTransformedUV((_DTexSetCustomFunction_e8155feacdd943ddb8f4ff867df48696_Out_3.xy)));
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_R_4 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.r;
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_G_5 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.g;
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_B_6 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.b;
            float _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_A_7 = _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0.a;
            float _Property_df06dff0c2484fb595c137b1754753b9_Out_0 = _BDissolveIn;
            float _Property_d214b0d6c4864bde9efaa3db6db55a86_Out_0 = _BDissolveOut;
            float4 _Property_f2164c96cf7945fd975240ed7e800b18_Out_0 = IsGammaSpace() ? LinearToSRGB(_BDissolveColor) : _BDissolveColor;
            UnityTexture2D _Property_d2937aa927074faf8a7e3502b5f57de2_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0 = SAMPLE_TEXTURE2D(_Property_d2937aa927074faf8a7e3502b5f57de2_Out_0.tex, _Property_d2937aa927074faf8a7e3502b5f57de2_Out_0.samplerstate, _Property_d2937aa927074faf8a7e3502b5f57de2_Out_0.GetTransformedUV((_RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3.xy)));
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_R_4 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.r;
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_G_5 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.g;
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_B_6 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.b;
            float _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_A_7 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_RGBA_0.a;
            float4 _Swizzle_f28776abd40e4239a0500f742ca08953_Out_1 = _SampleTexture2D_c857d3bccfd44f77bf8939a7f88670a2_B_6.xxxx;
            float _Property_ff648668c0b14c5bb553df8ea7ab4c99_Out_0 = _BLight;
            float4 _Property_f2d51e21017b4d62854d0dd0d69a4eeb_Out_0 = IsGammaSpace() ? LinearToSRGB(_BColor) : _BColor;
            float _Property_2358423daf2b46bd88fc2c6f83681dab_Out_0 = _BLightGain;
            float4 _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3;
            TextureLight_float(_Swizzle_f28776abd40e4239a0500f742ca08953_Out_1, _Property_ff648668c0b14c5bb553df8ea7ab4c99_Out_0, _Property_f2d51e21017b4d62854d0dd0d69a4eeb_Out_0, _Property_2358423daf2b46bd88fc2c6f83681dab_Out_0, _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3);
            float4 _DissolveCustomFunction_18ec96fae3434badafbd66e66f5f7fc7_Out_3;
            Dissolve_float((_RotateSizeCustomFunction_8e38adf4f25e4613954b9e456fb28ac3_Out_3.xy), _SampleTexture2D_cbda18d94c7f4fbbafa6d94bcf3f1e47_RGBA_0, _Property_df06dff0c2484fb595c137b1754753b9_Out_0, _Property_d214b0d6c4864bde9efaa3db6db55a86_Out_0, _Property_f2164c96cf7945fd975240ed7e800b18_Out_0, _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3, _DissolveCustomFunction_18ec96fae3434badafbd66e66f5f7fc7_Out_3);
            float4 _Add_b5aafd4ee22e4670a9b2a0eeae0b991b_Out_2;
            Unity_Add_float4(_DissolveCustomFunction_c794717a85c44b778d53446bc5599b11_Out_3, _DissolveCustomFunction_18ec96fae3434badafbd66e66f5f7fc7_Out_3, _Add_b5aafd4ee22e4670a9b2a0eeae0b991b_Out_2);
            float4 _Add_311c3433751947a9b670b04484a11b3b_Out_2;
            Unity_Add_float4(_DissolveCustomFunction_c1278429f28948e08cf996a8b62479a9_Out_3, _Add_b5aafd4ee22e4670a9b2a0eeae0b991b_Out_2, _Add_311c3433751947a9b670b04484a11b3b_Out_2);
            float _Swizzle_19e7c60d34834501ad98d57eecf88a20_Out_1 = _TextureLightCustomFunction_d40ac3629d6a4e7ab265be5bb80a2b91_Out_3.w;
            float _Swizzle_2b2f7ab3dfd748fbb705d617d01b1690_Out_1 = _TextureLightCustomFunction_05339912f1604e07be837c4466ebb76b_Out_3.w;
            float _Swizzle_d4f48dddbcae41cfbf129451e203ecf4_Out_1 = _TextureLightCustomFunction_3a17486c2a944da59ecd5367e81a046f_Out_3.w;
            float _Add_d05af43285cb4cce8638140598473902_Out_2;
            Unity_Add_float(_Swizzle_2b2f7ab3dfd748fbb705d617d01b1690_Out_1, _Swizzle_d4f48dddbcae41cfbf129451e203ecf4_Out_1, _Add_d05af43285cb4cce8638140598473902_Out_2);
            float _Add_747b1754f01448a9bb57f2993c19791e_Out_2;
            Unity_Add_float(_Swizzle_19e7c60d34834501ad98d57eecf88a20_Out_1, _Add_d05af43285cb4cce8638140598473902_Out_2, _Add_747b1754f01448a9bb57f2993c19791e_Out_2);
            float4 _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGBA_4;
            float3 _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGB_5;
            float2 _Combine_94b4ca9170e44d93bdf05a483d08c44b_RG_6;
            Unity_Combine_float(0, 0, 0, _Add_747b1754f01448a9bb57f2993c19791e_Out_2, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGBA_4, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGB_5, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RG_6);
            float4 _Add_2670f6bd6b3f4beb94a86259f697e3cf_Out_2;
            Unity_Add_float4(_Add_311c3433751947a9b670b04484a11b3b_Out_2, _Combine_94b4ca9170e44d93bdf05a483d08c44b_RGBA_4, _Add_2670f6bd6b3f4beb94a86259f697e3cf_Out_2);
            float _Swizzle_6cead8bc7a22455b93e7f26073d331da_Out_1 = _Add_2670f6bd6b3f4beb94a86259f697e3cf_Out_2.w;
            surface.BaseColor = IsGammaSpace() ? float3(0, 0, 0) : SRGBToLinear(float3(0, 0, 0));
            surface.Alpha = _Swizzle_6cead8bc7a22455b93e7f26073d331da_Out_1;
            surface.NormalTS = IN.TangentSpaceNormal;
            surface.NormalAlpha = 1;
            surface.Emission = (_Add_2670f6bd6b3f4beb94a86259f697e3cf_Out_2.xyz);
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            
        //     $features.graphVertex:  $include("VertexAnimation.template.hlsl")
        //                                       ^ ERROR: $include cannot find file : VertexAnimation.template.hlsl. Looked into:
        // Packages/com.unity.shadergraph/Editor/Generation/Templates
        
            
        //     $features.graphPixel:   $include("SharedCode.template.hlsl")
        //                                       ^ ERROR: $include cannot find file : SharedCode.template.hlsl. Looked into:
        // Packages/com.unity.shadergraph/Editor/Generation/Templates
        
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorCopyToSDI' */
        
        
        
            output.TangentSpaceNormal =                         float3(0.0f, 0.0f, 1.0f);
        
        
            output.uv0 =                                        input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
            return output;
        }
        
            // --------------------------------------------------
            // Build Surface Data
        
            uint2 ComputeFadeMaskSeed(uint2 positionSS)
            {
                uint2 fadeMaskSeed;
        
                // Can't use the view direction, it is the same across the entire screen.
                fadeMaskSeed = positionSS;
        
                return fadeMaskSeed;
            }
        
            void GetSurfaceData(Varyings input, half3 viewDirectioWS, uint2 positionSS, float angleFadeFactor, out DecalSurfaceData surfaceData)
            {
                #if (SHADERPASS == SHADERPASS_DBUFFER_PROJECTOR) || (SHADERPASS == SHADERPASS_FORWARD_EMISSIVE_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_SCREEN_SPACE_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_GBUFFER_PROJECTOR)
                    half4x4 normalToWorld = UNITY_ACCESS_INSTANCED_PROP(Decal, _NormalToWorld);
                    half fadeFactor = clamp(normalToWorld[0][3], 0.0f, 1.0f) * angleFadeFactor;
                    float2 scale = float2(normalToWorld[3][0], normalToWorld[3][1]);
                    float2 offset = float2(normalToWorld[3][2], normalToWorld[3][3]);
                    input.texCoord0.xy = input.texCoord0.xy * scale + offset;
                #else
                    #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                        LODDitheringTransition(ComputeFadeMaskSeed(positionSS), unity_LODFade.x);
                    #endif
        
                    half fadeFactor = half(1.0);
                #endif
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = BuildSurfaceDescriptionInputs(input);
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
        
                // setup defaults -- these are used if the graph doesn't output a value
                ZERO_INITIALIZE(DecalSurfaceData, surfaceData);
                surfaceData.occlusion = half(1.0);
                surfaceData.smoothness = half(0);
        
                #ifdef _MATERIAL_AFFECTS_NORMAL
                    surfaceData.normalWS.w = half(1.0);
                #else
                    surfaceData.normalWS.w = half(0.0);
                #endif
        
                surfaceData.emissive.rgb = half3(surfaceDescription.Emission.rgb * fadeFactor);
        
                // copy across graph values, if defined
                surfaceData.baseColor.xyz = half3(surfaceDescription.BaseColor);
                surfaceData.baseColor.w = half(surfaceDescription.Alpha * fadeFactor);
        
                #if (SHADERPASS == SHADERPASS_DBUFFER_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_SCREEN_SPACE_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_GBUFFER_PROJECTOR)
                    #if defined(_MATERIAL_AFFECTS_NORMAL)
                        surfaceData.normalWS.xyz = mul((half3x3)normalToWorld, surfaceDescription.NormalTS.xyz);
                    #else
                        surfaceData.normalWS.xyz = normalToWorld[2].xyz;
                    #endif
                #elif (SHADERPASS == SHADERPASS_DBUFFER_MESH) || (SHADERPASS == SHADERPASS_DECAL_SCREEN_SPACE_MESH) || (SHADERPASS == SHADERPASS_DECAL_GBUFFER_MESH)
                    #if defined(_MATERIAL_AFFECTS_NORMAL)
                        float sgn = input.tangentWS.w;      // should be either +1 or -1
                        float3 bitangent = sgn * cross(input.normalWS.xyz, input.tangentWS.xyz);
                        half3x3 tangentToWorld = half3x3(input.tangentWS.xyz, bitangent.xyz, input.normalWS.xyz);
        
                        // We need to normalize as we use mikkt tangent space and this is expected (tangent space is not normalize)
                        surfaceData.normalWS.xyz = normalize(TransformTangentToWorld(surfaceDescription.NormalTS, tangentToWorld));
                    #else
                        surfaceData.normalWS.xyz = half3(input.normalWS); // Default to vertex normal
                    #endif
                #endif
        
                surfaceData.normalWS.w = surfaceDescription.NormalAlpha * fadeFactor;
        
                // In case of Smoothness / AO / Metal, all the three are always computed but color mask can change
            }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPassDecal.hlsl"
        
            ENDHLSL
        }
        Pass
        { 
            Name "ScenePickingPass"
            Tags 
            { 
                "LightMode" = "Picking"
            }
        
            // Render State
            Cull Back
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            HLSLPROGRAM
        
            // Pragmas
            #pragma target 3.5
        #pragma vertex Vert
        #pragma fragment Frag
        #pragma multi_compile_instancing
        
            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>
        
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        
            // Defines
            
            #define HAVE_MESH_MODIFICATION
        
        
            #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENEPICKINGPASS 1
        
            // HybridV1InjectedBuiltinProperties: <None>
        
            // -- Properties used by ScenePickingPass
            #ifdef SCENEPICKINGPASS
            float4 _SelectionID;
            #endif
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DecalInput.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderVariablesDecal.hlsl"
        
            // --------------------------------------------------
            // Structs and Packing
        
            struct Attributes
        {
             float3 positionOS : POSITION;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
        };
        struct VertexDescriptionInputs
        {
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float4 _DissolveTex_TexelSize;
        float _GroundResidue;
        float4 _RColor;
        float _RSize;
        float _RRotate;
        float _RLight;
        float4 _RDissolveColor;
        float _RLightGain;
        float _RDissolveIn;
        float _RDissolveOut;
        float4 _DissolveTex_ST;
        float4 _MainTex_ST;
        float4 _GColor;
        float _GSize;
        float _GRotate;
        float _GLight;
        float _GLightGain;
        float4 _GDissolveColor;
        float _GDissolveIn;
        float _GDissolveOut;
        float4 _BColor;
        float _BSize;
        float _BRotate;
        float _BLight;
        float _BLightGain;
        float4 _BDissolveColor;
        float _BDissolveIn;
        float _BDissolveOut;
        float _DrawOrder;
        float _DecalMeshBiasType;
        float _DecalMeshDepthBias;
        float _DecalMeshViewBias;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_DissolveTex);
        SAMPLER(sampler_DissolveTex);
        
            // Graph Functions
            // GraphFunctions: <None>
        
            // Graph Vertex
            struct VertexDescription
        {
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            return description;
        }
            
            // Graph Pixel
            struct SurfaceDescription
        {
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            return surface;
        }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            
        //     $features.graphVertex:  $include("VertexAnimation.template.hlsl")
        //                                       ^ ERROR: $include cannot find file : VertexAnimation.template.hlsl. Looked into:
        // Packages/com.unity.shadergraph/Editor/Generation/Templates
        
            
        //     $features.graphPixel:   $include("SharedCode.template.hlsl")
        //                                       ^ ERROR: $include cannot find file : SharedCode.template.hlsl. Looked into:
        // Packages/com.unity.shadergraph/Editor/Generation/Templates
        
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorCopyToSDI' */
        
        
        
        
        
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
            return output;
        }
        
            // --------------------------------------------------
            // Build Surface Data
        
            uint2 ComputeFadeMaskSeed(uint2 positionSS)
            {
                uint2 fadeMaskSeed;
        
                // Can't use the view direction, it is the same across the entire screen.
                fadeMaskSeed = positionSS;
        
                return fadeMaskSeed;
            }
        
            void GetSurfaceData(Varyings input, half3 viewDirectioWS, uint2 positionSS, float angleFadeFactor, out DecalSurfaceData surfaceData)
            {
                #if (SHADERPASS == SHADERPASS_DBUFFER_PROJECTOR) || (SHADERPASS == SHADERPASS_FORWARD_EMISSIVE_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_SCREEN_SPACE_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_GBUFFER_PROJECTOR)
                    half4x4 normalToWorld = UNITY_ACCESS_INSTANCED_PROP(Decal, _NormalToWorld);
                    half fadeFactor = clamp(normalToWorld[0][3], 0.0f, 1.0f) * angleFadeFactor;
                    float2 scale = float2(normalToWorld[3][0], normalToWorld[3][1]);
                    float2 offset = float2(normalToWorld[3][2], normalToWorld[3][3]);
                #else
                    #ifdef LOD_FADE_CROSSFADE // enable dithering LOD transition if user select CrossFade transition in LOD group
                        LODDitheringTransition(ComputeFadeMaskSeed(positionSS), unity_LODFade.x);
                    #endif
        
                    half fadeFactor = half(1.0);
                #endif
        
                SurfaceDescriptionInputs surfaceDescriptionInputs = BuildSurfaceDescriptionInputs(input);
                SurfaceDescription surfaceDescription = SurfaceDescriptionFunction(surfaceDescriptionInputs);
        
                // setup defaults -- these are used if the graph doesn't output a value
                ZERO_INITIALIZE(DecalSurfaceData, surfaceData);
                surfaceData.occlusion = half(1.0);
                surfaceData.smoothness = half(0);
        
                #ifdef _MATERIAL_AFFECTS_NORMAL
                    surfaceData.normalWS.w = half(1.0);
                #else
                    surfaceData.normalWS.w = half(0.0);
                #endif
        
        
                // copy across graph values, if defined
        
                #if (SHADERPASS == SHADERPASS_DBUFFER_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_SCREEN_SPACE_PROJECTOR) || (SHADERPASS == SHADERPASS_DECAL_GBUFFER_PROJECTOR)
                    #if defined(_MATERIAL_AFFECTS_NORMAL)
                    #else
                        surfaceData.normalWS.xyz = normalToWorld[2].xyz;
                    #endif
                #elif (SHADERPASS == SHADERPASS_DBUFFER_MESH) || (SHADERPASS == SHADERPASS_DECAL_SCREEN_SPACE_MESH) || (SHADERPASS == SHADERPASS_DECAL_GBUFFER_MESH)
                    #if defined(_MATERIAL_AFFECTS_NORMAL)
                        float sgn = input.tangentWS.w;      // should be either +1 or -1
                        float3 bitangent = sgn * cross(input.normalWS.xyz, input.tangentWS.xyz);
                        half3x3 tangentToWorld = half3x3(input.tangentWS.xyz, bitangent.xyz, input.normalWS.xyz);
        
                        // We need to normalize as we use mikkt tangent space and this is expected (tangent space is not normalize)
                    #else
                        surfaceData.normalWS.xyz = half3(input.normalWS); // Default to vertex normal
                    #endif
                #endif
        
        
                // In case of Smoothness / AO / Metal, all the three are always computed but color mask can change
            }
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPassDecal.hlsl"
        
            ENDHLSL
        }
    }
    CustomEditorForRenderPipeline "UnityEditor.Rendering.Universal.DecalShaderGraphGUI" "UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset"
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    FallBack "Hidden/Shader Graph/FallbackError"
}