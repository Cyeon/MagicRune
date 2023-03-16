Shader "Highlighters/Overlay"
{
	Properties
	{
         
	}

    HLSLINCLUDE

		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

		TEXTURE2D(_ObjectsInfo);
		SAMPLER(sampler_ObjectsInfo);
		float2 _ObjectsInfo_TexelSize;

		TEXTURE2D(_MeshOutlineObjects);
		SAMPLER(sampler_MeshOutlineObjects);

		// General

		int _DepthMask;
		float behindtestout;

		// Mesh Outline
		int _UseMeshOutline;

		// Inner Glow

		#pragma shader_feature _INNERGLOW 

		int _UseSingleInnerGlow;
		float4 _RimColorFront;
		float _RimPowerFront;
		float4 _RimColorBack;
		float _RimPowerBack;

		// Overlay

		#pragma shader_feature  _OVERLAY

		int _UseSingleOverlay;

		float4	_OverlayColorFront;
		float	_OverlayBackgroundFront;
		int		_OverlayUseTexFront;
		float	_OverlayTillingFront;
		float	_OverlayRotationFront;

		float4	_OverlayColorBack;
		float	_OverlayBackgroundBack;
		int		_OverlayUseTexBack;
		float	_OverlayTillingBack;
		float	_OverlayRotationBack;

		TEXTURE2D(_OverlayTexFront);
		SAMPLER(sampler__OverlayTexFront);

		TEXTURE2D(_OverlayTexBack);
		SAMPLER(sampler__OverlayTexBack);

		// ----------------------

		float4 _RenderingBounds;

		// ----------------------


		struct Attributes
		{
			float4 positionOS : POSITION;
			float2 uv : TEXCOORD0;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};
		
		struct Varyings
		{
			float4 positionCS : SV_POSITION;
			float3 positionVS : TEXCOORD1;
			float2 uv : TEXCOORD0;
			UNITY_VERTEX_INPUT_INSTANCE_ID
            UNITY_VERTEX_OUTPUT_STEREO
		};

		Varyings vert(Attributes input)
		{
			Varyings output = (Varyings)0;

			UNITY_SETUP_INSTANCE_ID(input);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

			VertexPositionInputs positionInputs = GetVertexPositionInputs(input.positionOS.xyz);

			//output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
			output.positionCS = positionInputs.positionCS;
			output.positionVS = positionInputs.positionVS;
			output.uv = input.uv;

			return output;
		}
		

		// --------------
		// Fills functions

		float4 BackFillInnerGlow(float rimBeforePower)
		{
			float4 col = float4(0,0,0,0); 

			#ifdef _INNERGLOW
			col += _RimColorBack *  pow(abs(rimBeforePower),_RimPowerBack);
			#endif

			return saturate(col);
		}

		float4 BackFillOverlay(float overlayTex)
		{
			float4 col = float4(0,0,0,0); 

			#ifdef _OVERLAY
			overlayTex = saturate (overlayTex + _OverlayBackgroundBack);
			col += _OverlayColorBack *  overlayTex;
			#endif

			return saturate(col);
		}

		float4 FrontFillInnerGlow(float rimBeforePower)
		{
			float4 col = float4(0,0,0,0); 

			#ifdef _INNERGLOW
			col += _RimColorFront *  pow(abs(rimBeforePower),_RimPowerFront);
			#endif

			return saturate(col);
		}

		float4 FrontFillOverlay(float overlayTex)
		{
			float4 col = float4(0,0,0,0); 

			#ifdef _OVERLAY
			overlayTex = saturate (overlayTex + _OverlayBackgroundFront);
			col += _OverlayColorFront *  overlayTex;
			#endif

			return saturate(col);
		}

		float4 NoMaskFill(float overlayTex, float rimBeforePower)
		{
			float4 col = float4(0,0,0,0); 

			#ifdef _INNERGLOW

			col += _RimColorFront *  pow(abs(rimBeforePower),_RimPowerFront);

			#endif

			#ifdef _OVERLAY

			overlayTex = saturate (overlayTex + _OverlayBackgroundFront);
			col += _OverlayColorFront * overlayTex;

			#endif

			return saturate(col);
		}

		// --------------
		// Helpers
		
		float2 Rotate_Degrees(float2 UV, float Rotation) // , float2 Center
		{
			Rotation = Rotation * (3.1415926f/180.0f);
			//UV -= Center;
			float s = sin(Rotation);
			float c = cos(Rotation);
			float2x2 rMatrix = float2x2(c, -s, s, c);
			rMatrix *= 0.5;
			rMatrix += 0.5;
			rMatrix = rMatrix * 2 - 1;
			UV.xy = mul(UV.xy, rMatrix);
			//UV += Center;
			return UV;
		}

		// --------------

		bool ShouldRender(float3 positionVS)
		{
			//return true;

			if(_RenderingBounds.x < positionVS.x && _RenderingBounds.y < positionVS.y && _RenderingBounds.z > positionVS.x && _RenderingBounds.w > positionVS.y)
			{
				return true;
			}

			return false;
		}

		// --------------


		float4 frag(Varyings i) : SV_Target
		{
			UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

			if(!ShouldRender(i.positionVS)) return float4(0,0,0,0);


			float2 uv = UnityStereoTransformScreenSpaceTex(i.uv);
			float4 mask = SAMPLE_TEXTURE2D(_ObjectsInfo, sampler_ObjectsInfo, uv);

			// ------------

			// Overlays

			//Behind R channel
			//Front G channel
			//Combine B channel

			float2 AspecRatioUvs = float2((( _ScreenParams.x / _ScreenParams.y )).x,1) * uv.xy;
			
			float overlayTexFront;
			float overlayTexBack;

			if(_OverlayUseTexFront)
			{
				float2 overlayUVs = Rotate_Degrees(AspecRatioUvs,_OverlayRotationFront) * _OverlayTillingFront;
				overlayTexFront = SAMPLE_TEXTURE2D(_OverlayTexFront, sampler__OverlayTexFront, overlayUVs ).r;
			}
			else overlayTexFront = 1;

			if(_OverlayUseTexBack)
			{
				float2 overlayUVs = Rotate_Degrees(AspecRatioUvs,_OverlayRotationBack) * _OverlayTillingBack;
				overlayTexBack = SAMPLE_TEXTURE2D(_OverlayTexBack, sampler__OverlayTexBack, overlayUVs ).r;
			}
			else overlayTexBack = 1;

			// ------------

			// Mesh Outline
			float4 meshOutlineColor = float4(0,0,0,0);
			
			if(_UseMeshOutline)
			{
				meshOutlineColor = SAMPLE_TEXTURE2D(_MeshOutlineObjects, sampler_MeshOutlineObjects, uv.xy);
			}

			// ------------

			//_DepthMask -- Behind | Front | Both | Disable 
			// mask.b is rimBeforePower value

            if(_DepthMask == 3) // No Depth Mask
			{
				if(mask.r > 0) 
				{
					return NoMaskFill(overlayTexFront, mask.b);;
				}
				if(_UseMeshOutline)
				{
					//return lerp(outerGlow.rgba,meshOutlineColor.rgba,meshOutlineColor.a);
					return meshOutlineColor;
				}
				else return float4(0,0,0,0);
			}
			
			else if(_DepthMask == 0) // Behind Draw
			{
				if (mask.r > 0)
				{
					return BackFillInnerGlow(mask.b) + BackFillOverlay(overlayTexBack);
				}
				if(_UseMeshOutline && mask.g <= 0) 
				{
					//return lerp(outerGlow.rgba,meshOutlineColor.rgba,meshOutlineColor.a);
					return meshOutlineColor;
				}
				else return float4(0,0,0,0);
			}
			
			else if(_DepthMask == 1) // Front Draw
			{
				if (mask.g > 0)
				{
					return FrontFillInnerGlow(mask.b) + FrontFillOverlay(overlayTexFront);
				}
				if(_UseMeshOutline )
				{
					//return lerp(outerGlow.rgba,meshOutlineColor.rgba,meshOutlineColor.a);
					return meshOutlineColor;
				}
				else return float4(0,0,0,0);
			}

			else if(_DepthMask == 2) // Both Draw
			{
				float4 colFront = float4(0,0,0,0);
				float4 colBack = float4(0,0,0,0);

				if(_UseSingleInnerGlow)
				{
					float4 innerGlow = FrontFillInnerGlow(mask.b);
					colFront += innerGlow;
					colBack += innerGlow;
				}
				else
				{
					colFront += FrontFillInnerGlow(mask.b);
					colBack += BackFillInnerGlow(mask.b);
				}

				if(_UseSingleOverlay)
				{
					float4 overlay = FrontFillOverlay(overlayTexFront);
					colFront += overlay;
					colBack += overlay;
				}
				else
				{
					colFront += FrontFillOverlay(overlayTexFront);
					colBack += BackFillOverlay(overlayTexBack);
				}

				if (mask.r > 0)
				{
					return colBack;
				}
				if (mask.g > 0)
				{
					return colFront;
				}

				if(_UseMeshOutline)
				{
					//return lerp(outerGlow.rgba,meshOutlineColor.rgba,meshOutlineColor.a);
					return meshOutlineColor;
				}
				else return float4(0,0,0,0);
			}
           
			return float4(0,0,0,0);
		}

	ENDHLSL

	SubShader
	{
		Tags{ "RenderPipeline" = "UniversalPipeline" }

		//Cull Off
		ZWrite Off
		//ZTest Always
		Lighting Off
		//Blend SrcAlpha OneMinusSrcAlpha

		Pass

		{
			Name "OverlayPass"
			Blend SrcAlpha OneMinusSrcAlpha

			HLSLPROGRAM

			#pragma multi_compile_instancing
			#pragma vertex vert
			#pragma fragment frag

			ENDHLSL
		}
	}

}