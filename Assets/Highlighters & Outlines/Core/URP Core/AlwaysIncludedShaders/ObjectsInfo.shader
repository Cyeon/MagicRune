Shader "Highlighters/ObjectsInfo"
{
   HLSLINCLUDE

		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderVariablesFunctions.hlsl"

		TEXTURE2D(_SceneDepthMask);
		SAMPLER(sampler_SceneDepthMask);

        TEXTURE2D(_MainTex);
		SAMPLER(sampler_MainTex);

        float _Cutoff;
        int useDepth ;

		struct Attributes
            {
                float4 position     : POSITION;
                float2 texcoord     : TEXCOORD0;
                float3 normal        : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

        struct Varyings
        {
            float2 uv           : TEXCOORD0;
            float4 positionCS   : SV_POSITION;
            float3 positionVS    : TEXCOORD2;
            float4 screenPos    : TEXCOORD1;
            float3 normal        : NORMAL;
            float3 worldPos     : TEXCOORD3;
            UNITY_VERTEX_INPUT_INSTANCE_ID
            UNITY_VERTEX_OUTPUT_STEREO
        };

         Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                VertexPositionInputs positionInputs = GetVertexPositionInputs(input.position.xyz);

                output.uv = input.texcoord;
                output.positionCS = positionInputs.positionCS;
                output.screenPos = positionInputs.positionNDC;
			    output.positionVS = positionInputs.positionVS;

                float3 positionWS = TransformObjectToWorld( input.position.xyz );
				output.worldPos = positionWS;

                float3 ase_worldNormal = TransformObjectToWorldNormal(input.normal);
                output.normal = ase_worldNormal;

                return output;
            }

		 float4 frag(Varyings input) : SV_TARGET
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

			    //Behind R channel
			    //Front G channel

                // TODO add something to alpha channel, thnik about depth (clip space position) to use in ordering 
                // or just delete the alpha channel

                float3 WorldPosition = input.worldPos;
                float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_worldNormal = input.normal.xyz;

                float Power = 1.4f;
                float rimBeforePower = 1.0 - saturate(dot(ase_worldNormal, ase_worldViewDir));

                float textureAlpha =  SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv).a;
                
                clip(textureAlpha - _Cutoff);
                
                if(useDepth)
                {
                    float4 mask = SAMPLE_TEXTURE2D(_SceneDepthMask, sampler_SceneDepthMask, input.screenPos.xy / input.screenPos.w);

                    #ifdef UNITY_REVERSED_Z 
				    if (mask.r > input.positionCS.z)
			            {
			                return float4(1,0,rimBeforePower,1); // Back
			            }
                    #else
				    if (mask.r <= input.positionCS.z)
			            {
			                return float4(1,0,rimBeforePower,1); // Back
			            }
                    #endif
                    
                    return float4(0,1,rimBeforePower,1); // Front

                }
			    return float4(1,0,rimBeforePower,1); // Back
                
             }

	ENDHLSL

	SubShader
	{
		Tags { "RenderPipeline" = "UniversalPipeline" }

		ZWrite On
		ZTest LEqual
		Lighting Off

		Pass
		{
			Name "Off"
		    Cull Off

			 HLSLPROGRAM
	        #pragma multi_compile_instancing

            #pragma vertex vert
            #pragma fragment frag
            ENDHLSL
		}

        Pass
		{
			Name "Front"
		    Cull Front

			 HLSLPROGRAM
	        #pragma multi_compile_instancing

            #pragma vertex vert
            #pragma fragment frag
            ENDHLSL
		}

        Pass
		{
			Name "Back"
		    Cull Back

			 HLSLPROGRAM
	        #pragma multi_compile_instancing

            #pragma vertex vert
            #pragma fragment frag
            ENDHLSL
		}
	}
}
