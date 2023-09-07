

Shader "Lite Effect/Particles/URP/Lite Particle Blend"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}

		_light("Light", Range(0.0,1.0)) = 0.2
		_lightGain("Light Gain", Range(0,10)) = 1.0
		_isRotation ("isRotation", float) = 0.0
		_MainRotation("Rotation", Range(0,360)) = 0.0

		_isCut ("isCut", float) = 0.0
		_CutTex("Texture (A)", 2D) = "white" {}
		_isCutRotation ("isCutRotation", float) = 0.0
		_CutRotation("Rotation",Range(0,360)) = 0.0

		_isEmissionGain ("isEmissionGain", float) = 0.0
		_EmissionGain("Emission Gain", Range(1, 10)) = 1.0

		[Toggle] _SoftParticlesEnabled ("Soft Particles Enabled", Float) = 0.0
        _SoftParticlesNearFadeDistance("Soft Particles Near Fade", Range(0,10)) = 0.0
        _SoftParticlesFarFadeDistance("Soft Particles Far Fade", Range(0,10)) = 1.0
		[Toggle] _CameraFadingEnabled ("Camera Fading Enabled", Float) = 0.0
		_CameraNearFadeDistance("Camera Near Fade", Float) = 1.0
        _CameraFarFadeDistance("Camera Far Fade", Float) = 2.0
	}

		SubShader
		{
			Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "UniversalPipeline" }
			Blend SrcAlpha OneMinusSrcAlpha

			Cull Off Lighting Off ZWrite Off
			Pass
			{
				HLSLPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_particles
				#pragma multi_compile_instancing

				#pragma shader_feature_local _SOFTPARTICLES_ON
				#pragma shader_feature_local _FADING_ON

				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

CBUFFER_START(UnityPerMaterial)
				float4 _MainTex_ST;
				float _light;
				float _lightGain;
				float4 _CutTex_ST;

				float _isCut;
				float _isRotation;
				float _isCutRotation;
				float _isEmissionGain;

				float _SoftParticlesEnabled;
				float _SoftParticlesNearFadeDistance;
				float _SoftParticlesFarFadeDistance;

				float _CameraFadingEnabled;
				float _CameraNearFadeDistance;
				float _CameraFarFadeDistance;

				float _MainRotation;
				float _CutRotation;
				float _EmissionGain;
CBUFFER_END

				TEXTURE2D(_MainTex);
				SAMPLER(sampler_MainTex);
				TEXTURE2D(_CutTex);
				SAMPLER(sampler_CutTex);

				struct appdata
				{
					float4 vertex : POSITION;
					float4 color : COLOR;
					float4 uv : TEXCOORD0;
					float4 uv01 : TEXCOORD1;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					float4 uv_MainTex: TEXCOORD0;
					float2 uv_CutOut : TEXCOORD1;
					float4 projPos : TEXCOORD2;
					float4 color : COLOR;
					UNITY_VERTEX_INPUT_INSTANCE_ID

				};

				float SoftParticles(float near, float far, float4 projection)
				{
					float fade = 1;
					if (near > 0.0 || far > 0.0)
					{
						float rawDepth = SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, UnityStereoTransformScreenSpaceTex(projection.xy / projection.w)).r;
						float sceneZ = (unity_OrthoParams.w == 0) ? LinearEyeDepth(rawDepth, _ZBufferParams) : LinearDepthToEyeDepth(rawDepth);
						float thisZ = LinearEyeDepth(projection.z / projection.w, _ZBufferParams);
						fade = saturate(far * ((sceneZ - near) - thisZ));
					}
					return fade;
				}

				half CameraFade(float near, float far, float4 projection)
				{
					float thisZ = LinearEyeDepth(projection.z / projection.w, _ZBufferParams);
					return half(saturate((thisZ - near) * far));
				}

				v2f vert(appdata v)
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_TRANSFER_INSTANCE_ID(v, o);
					ZERO_INITIALIZE(v2f, o);

					VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
					o.vertex = TransformObjectToHClip(v.vertex.xyz);
					o.projPos = vertexInput.positionNDC;

					o.color = v.color;		
					float2 ruv = float2((((v.uv.x - 0.5) * 1/(v.uv01.z + 1)) + 0.5 )*_MainTex_ST.x,(((v.uv.y - 0.5) * 1/(v.uv01.z + 1)) + 0.5)*_MainTex_ST.y);		
					o.uv_MainTex.xy =  ruv;
					
					float ro = v.uv01.w;
					ro += _MainRotation * _isRotation;

					o.uv_MainTex.xy -=  float2(0.5 *_MainTex_ST.x , 0.5 *_MainTex_ST.y);

					float s = sin(radians(ro));
					float c = cos(radians(ro));

					float2x2 rotationMatrix = float2x2(c, -s, s, c);

					o.uv_MainTex.xy = mul(o.uv_MainTex.xy, rotationMatrix);
					o.uv_MainTex.xy +=  float2(0.5 *_MainTex_ST.x , 0.5 *_MainTex_ST.y) + _MainTex_ST.zw +  v.uv01.xy;



					o.uv_CutOut = v.uv.xy * _CutTex_ST.xy + _CutTex_ST.zw;
					o.uv_CutOut.xy -=  0.5;

					float s2 = sin(radians(_CutRotation * _isCutRotation));
					float c2 = cos(radians(_CutRotation * _isCutRotation));

					float2x2 rrotationMatrix2 = float2x2(c2, -s2, s2, c2);

					o.uv_CutOut.xy = mul(o.uv_CutOut.xy, rrotationMatrix2);
					o.uv_CutOut.xy += 0.5;

					return o;
				}



				half4 frag(v2f i) : SV_Target
				{
					UNITY_SETUP_INSTANCE_ID(i);

					#ifdef _SOFTPARTICLES_ON
					float2	_SoftParticleFadeParams;
					_SoftParticleFadeParams.x = _SoftParticlesNearFadeDistance;
					_SoftParticleFadeParams.y = 1.0f / (_SoftParticlesFarFadeDistance - _SoftParticlesNearFadeDistance);
					i.color.a *= SoftParticles(_SoftParticleFadeParams.x,_SoftParticleFadeParams.y,i.projPos);			
					#endif

					float4 c = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.uv_MainTex.xy);
 
					c.rgb *= i.color.rgb;
					c.a *= i.color.a;


					float4 ca = lerp(float4(1,1,1,1) ,float4(1,1,1, SAMPLE_TEXTURE2D(_CutTex,sampler_CutTex, i.uv_CutOut).a),_isCut);
					c *= ca;

					float4 Light = smoothstep( 1-_light,1,c.a);

					c.rgb *= lerp(1 , _EmissionGain , _isEmissionGain);

					Light = (Light.r * 0.333 + Light.g * 0.333 + Light.b * 0.333) * Light.a * c.a * _lightGain;
					c.rgb += Light.rgb * 0.2 +  Light.rgb * i.color.rgb ;
					c.a += Light.a;

					c.a = saturate(c.a);



					return c;
				}
				ENDHLSL
			}
		}
			Fallback "Transparent/VertexLit"
			CustomEditor "LiteShader.LiteShaderURPEditor"
}