Shader "LiteMagic/Magic Add"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DissolveTex ("Dissolve Texture", 2D) = "white" {}
        _GroundResidue ("Ground Residue", Range(0.0,1.0)) = 1.0
        [Header(R Texture)] [Space]
        [HDR] _RColor  ("R Color", Color) =  (1,1,1,1)
        _RSize ("R Size", Float) = 1
        _RRotate  ("R Rotate", Float) = 0
        _RLight ("R Light", Range(0.0,1.0)) = 0.2
        _RLightGain ("R Light Gain", Range(1,20)) = 1.0
        [HDR] _RDissolveColor  ("R Dissolve Color", Color) =  (1,1,1,1)
        _RDissolveIn ("R Dissolve Inside", Float) = 0
        _RDissolveOut ("R Dissolve Outside", Float) = 1

        [Header(G Texture)] [Space]
        [HDR] _GColor  ("G Color", Color) =  (1,1,1,1)
        _GSize ("G Size", Float) = 1
        _GRotate  ("G Rotate", Float) = 0
        _GLight ("G Light", Range(0.0,1.0)) = 0.2
        _GLightGain ("R Light Gain", Range(1,20)) = 1.0
        [HDR] _GDissolveColor  ("G Dissolve Color", Color) =  (1,1,1,1)
        _GDissolveIn ("G Dissolve Inside", Float) = 0
        _GDissolveOut ("G Dissolve Outside", Float) = 1

        [Header(B Texture)] [Space]
        [HDR] _BColor  ("B Color", Color) =  (1,1,1,1)
        _BSize ("B Size", Float) = 1
        _BRotate  ("B Rotate", Float) = 0
        _BLight ("B Light", Range(0.0,1.0)) = 0.2
        _BLightGain ("R Light Gain", Range(1,20)) = 1.0
        [HDR] _BDissolveColor  ("B Dissolve Color", Color) =  (1,1,1,1)
        _BDissolveIn ("B Dissolve Inside", Float) = 0
        _BDissolveOut ("B Dissolve Outside", Float) = 1
    }
    SubShader
    {
        // No culling or depth
        Cull Off 
        ZWrite Off 
        //Blend SrcAlpha OneMinusSrcAlpha
        Blend SrcAlpha OneMinusSrcAlpha
        //ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "UnityCG.cginc"
            #include "MagicInclude.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 uv : TEXCOORD0;
                float4 uvR : TEXCOORD1;
                float4 uvG : TEXCOORD2;
                float4 uvB : TEXCOORD3;
                float4 uvD : TEXCOORD4;
                float4 vertex : SV_POSITION;
                UNITY_FOG_COORDS(5)
            };

            float4x4 unity_Projector;
            sampler2D _MainTex;
            sampler2D _DissolveTex;
            float4 _DissolveTex_ST;

            float _GroundResidue;
            

            float _RSize;
            float _RRotate;
            float _RLight;
            float _RLightGain;
            float4 _RColor;
            float4 _RDissolveColor;
            float _RDissolveIn;
            float _RDissolveOut;

            float _GSize;
            float _GRotate;
            float _GLight;
            float _GLightGain;
            float4 _GColor;
            float4 _GDissolveColor;
            float _GDissolveIn;
            float _GDissolveOut;

            float _BSize;
            float _BRotate;
            float _BLight;
            float _BLightGain;
            float4 _BColor;
            float4 _BDissolveColor;
            float _BDissolveIn;
            float _BDissolveOut;


            v2f vert (appdata v)
            {
                v2f o;
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = mul (unity_Projector, v.vertex);

                uvRotateSize_float(o.uv,_RSize,_RRotate,o.uvR);
                uvRotateSize_float(o.uv,_GSize,_GRotate,o.uvG);
                uvRotateSize_float(o.uv,_BSize,_BRotate,o.uvB);

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;

            }


            fixed4 frag (v2f i) : SV_Target
            {
               //R
               fixed4 Rtex = tex2Dproj (_MainTex, UNITY_PROJ_COORD(i.uvR)).rrrr;
               float ground = Rtex.a;
               TextureLight_float(Rtex,_RLight,_RColor,_RLightGain,Rtex);
               float4 DTexuv;
               DTexSet_float(i.uvR,_DissolveTex_ST,DTexuv);
               float4 DTex = tex2Dproj (_DissolveTex,UNITY_PROJ_COORD(DTexuv));
               Dissolve_float(i.uvR.xy,DTex,_RDissolveIn,_RDissolveOut,_RDissolveColor,Rtex,Rtex);

               //G
               fixed4 Gtex = tex2Dproj (_MainTex, UNITY_PROJ_COORD(i.uvG)).gggg;
               ground += Gtex.a;
               TextureLight_float(Gtex,_GLight,_GColor,_GLightGain,Gtex);
               DTexSet_float(i.uvG,_DissolveTex_ST,DTexuv);
               DTex = tex2Dproj (_DissolveTex,UNITY_PROJ_COORD(DTexuv));
               Dissolve_float(i.uvG.xy,DTex,_GDissolveIn,_GDissolveOut,_GDissolveColor,Gtex,Gtex);

               //B
               fixed4 Btex = tex2Dproj (_MainTex, UNITY_PROJ_COORD(i.uvB)).bbbb;
               ground += Btex.a;
               TextureLight_float(Btex,_BLight,_BColor,_BLightGain,Btex);
               DTexSet_float(i.uvB,_DissolveTex_ST,DTexuv);
               DTex = tex2Dproj (_DissolveTex,UNITY_PROJ_COORD(DTexuv));
               Dissolve_float(i.uvB.xy,DTex,_BDissolveIn,_BDissolveOut,_BDissolveColor,Btex,Btex);        
                

                
                fixed4 col = float4(0,0,0,ground * _GroundResidue) + Rtex + Gtex + Btex;
                col.a = saturate(col.a);
                UNITY_APPLY_FOG_COLOR(i.fogCoord, col, unity_FogColor*col.a);
                return col;
            }
            ENDCG
        }
    }
}
