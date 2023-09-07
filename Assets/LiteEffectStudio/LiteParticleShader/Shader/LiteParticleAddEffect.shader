// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


// Upgrade NOTE: upgraded instancing buffer 'MyProperties' to new syntax.


Shader "Lite Effect/Particles/Lite Particle Add"
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
			Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			Blend SrcAlpha One
			//Blend SrcAlpha OneMinusSrcAlpha
			Cull Off Lighting Off ZWrite Off
			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_particles
				#pragma multi_compile_instancing
				#pragma multi_compile __ SOFTPARTICLES_ON
				#pragma multi_compile __ _FADING_ON
				//#pragma target 3.0


				#include "UnityCG.cginc"
				sampler2D _MainTex;
				float4 _MainTex_ST;
				float _light;
				float _lightGain;

				float _isCut;
				float _isRotation;
				float _isCutRotation;
				float _isEmissionGain;

				sampler2D _CutTex;
				float4 _CutTex_ST;

				fixed _MainRotation;
				fixed _CutRotation;


				float _SoftParticlesEnabled;
				float _SoftParticlesNearFadeDistance;
				float _SoftParticlesFarFadeDistance;

				float _CameraFadingEnabled;
				float _CameraNearFadeDistance;
				float _CameraFarFadeDistance;

				sampler2D_float _CameraDepthTexture;

				struct appdata
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					fixed4 uv : TEXCOORD0;
					fixed4 uv01 : TEXCOORD1;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct v2f
				{
					half4 vertex : SV_POSITION;
					half4 uv_MainTex: TEXCOORD0;
					half2 uv_CutOut : TEXCOORD1;
	#if defined(SOFTPARTICLES_ON) || defined(_FADING_ON)
					float4 projPos : TEXCOORD2;
	#endif
					fixed4 color : COLOR;
					UNITY_VERTEX_INPUT_INSTANCE_ID

				};



				v2f vert(appdata v)
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_TRANSFER_INSTANCE_ID(v, o);
					UNITY_INITIALIZE_OUTPUT(v2f, o);
					o.vertex = UnityObjectToClipPos(v.vertex);


					#ifdef SOFTPARTICLES_ON
					o.projPos = ComputeScreenPos (o.vertex);
					COMPUTE_EYEDEPTH(o.projPos.z);
					#else 
					#ifdef _FADING_ON
					o.projPos = ComputeScreenPos (o.vertex);
					COMPUTE_EYEDEPTH(o.projPos.z);
					#endif
					#endif

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

				float _EmissionGain;

				half4 frag(v2f i) : SV_Target
				{
					UNITY_SETUP_INSTANCE_ID(i);

					#ifdef SOFTPARTICLES_ON
					float2	_SoftParticleFadeParams;
					_SoftParticleFadeParams.x = _SoftParticlesNearFadeDistance * _SoftParticlesEnabled;
					_SoftParticleFadeParams.y = 1.0f / (_SoftParticlesFarFadeDistance - _SoftParticlesNearFadeDistance)* _SoftParticlesEnabled;

					if (_SoftParticleFadeParams.x > 0.0 || _SoftParticleFadeParams.y > 0.0) {
					float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
					float partZ = i.projPos.z;
					float fade = saturate(_SoftParticleFadeParams.y * ((sceneZ - _SoftParticleFadeParams.x)  - partZ));
					i.color.a *= fade;	
					}				
					#endif

					#ifdef _FADING_ON
					float2	_CameraFadeParams;
					_CameraFadeParams.x = _CameraNearFadeDistance;
					_CameraFadeParams.y = 1.0f / (_CameraFarFadeDistance - _CameraNearFadeDistance);
					float cameraFade = saturate((i.projPos.z - _CameraFadeParams.x) *  _CameraFadeParams.y);
					i.color.a *= cameraFade;	
					#endif

					float4 c = tex2D(_MainTex, i.uv_MainTex.xy);

					c.rgb *= i.color.rgb * c.a *  i.color.a;
					c.a *= i.color.a;


					float4 ca = lerp(float4(1,1,1,1) , tex2D(_CutTex, i.uv_CutOut).aaaa,_isCut);
					c *= ca;

					fixed4 Light = smoothstep( 1-_light,1,c);

					c.rgb *= lerp(1 , _EmissionGain , _isEmissionGain);

					Light = (Light.r * 0.333 + Light.g * 0.333 + Light.b * 0.333) * Light.a * c.a * _lightGain;
					c += Light*0.2 + Light * i.color;


					return c;
				}
				ENDCG
			}
		}
			Fallback "Transparent/VertexLit"
			CustomEditor "LiteShader.LiteShaderEditor"
}