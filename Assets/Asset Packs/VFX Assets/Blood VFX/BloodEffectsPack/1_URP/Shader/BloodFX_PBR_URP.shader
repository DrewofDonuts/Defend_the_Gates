Shader "BloodEffectsPack/URP/BloodFX_PBR_URP"
{
	Properties{

		[Header(ViewDirMask)]
		_ViewDirMaskThreshold("ViewDirMaskThreshold", range(0,1)) = 0.1

		[Header(Color)]
		_BaseColor("BaseColor", Color) = (1, 1, 1, 1)
		_ColorIntensity("ColorIntensity", float) = 1.0
		_AlbedoPower("AlbedoPower", float) = 1.0


		_HueShift("HueShift", Range(-180,180)) = 0
		_AmbientColorIntensity("AmbientColorIntensity", float) = 1.0

		[Header(Main)]
		[NoScaleOffset] _MainTex("Texture", 2D) = "white" {}
		[Header(Smoothness)]
		_Smoothness("Smoothness", Range(0,1)) = 0


		[Header(Normal)]
		[Toggle(_NORMALMAP_ON)] _NORMALMAP_ON("NORMALMAP_ON", float) = 0
		[NoScaleOffset] _NormalTex("NormalTex", 2D) = "bump"{}
		_NormalScale("NormalScale", float) = 0.0

		[Header(Specularity)]
		[Toggle(_SPECULARHIGHLIGHTS_OFF)] _SPECULARHIGHLIGHTS_OFF("SPECULARHIGHLIGHTS_OFF", float) = 0.0
		[HideInInspector] _Cutoff("AlphaCutoff", float) = 0.01

	}
		SubShader{

			HLSLINCLUDE
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
				CBUFFER_START(UnityPerMaterial)

				float _ViewDirMaskThreshold;
				float _HueShift;
				float _AmbientColorIntensity;

				float4 _BaseColor;
				float _ColorIntensity;
				float _AlbedoPower;

				float _Smoothness;
				float _NormalScale;
				float _UseSpecularity;

				float _Cutoff;

				CBUFFER_END
			ENDHLSL


			Tags
			{
				"RenderType" = "Transparent" 
				"Queue" = "Transparent"
				"RenderPipeline" = "UniversalPipeline"
			}

			Pass {

				Tags
				{
					"LightMode" = "UniversalForward"
				}

				Blend SrcAlpha OneMinusSrcAlpha
				ZWrite Off
				

				HLSLPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma shader_feature_local _ _NORMALMAP_ON
				#pragma shader_feature_local _ _SPECULARHIGHLIGHTS_OFF
				#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
				#pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
				#pragma multi_compile _ LIGHTMAP_ON
				#pragma multi_compile_fog

				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceData.hlsl"

				struct Attributes {
					float4 positionOS	: POSITION;
					float2 uv 		: TEXCOORD0;
					float2 staticLightmapUV   : TEXCOORD1;
					float4 color		: COLOR;
					float4 normalOS		: NORMAL;
					#ifdef _NORMALMAP_ON
						float4 tangentOS    : TANGENT;
					#endif
				};

				struct Varyings {
					float4 positionCS	: SV_POSITION;
					float2 uv		: TEXCOORD0;
					float4 color		: COLOR;
					float3 normalWS		: NORMAL;
					float3 positionWS	: TEXCOORD2;
					real fogFactor : TEXCOORD3;
					DECLARE_LIGHTMAP_OR_SH(staticLightmapUV, vertexSH, 4);

					#ifdef _ADDITIONAL_LIGHTS_VERTEX
						float3 vertexLighting : TEXCOORD5;
					#endif

					#ifndef _SPECULARHIGHLIGHTS_OFF
					#ifdef _NORMALMAP_ON
						half4 tangentWS	: TEXCOORD6;
					#endif
					#endif
				};

				half3 SampleNormal(float2 uv, TEXTURE2D_PARAM(bumpMap, sampler_bumpMap), half scale = half(1.0))
				{
					half4 n = SAMPLE_TEXTURE2D(bumpMap, sampler_bumpMap, uv);
					return UnpackNormalScale(n, scale);
				}
				float3 ApplyHueShift(float3 aColor, float hue)
				{
					float angle = radians(hue);
					float3 k = float3(0.57735, 0.57735, 0.57735);
					float cosAngle = cos(angle);
					return aColor * cosAngle + cross(k, aColor) * sin(angle) + k * dot(k, aColor) * (1 - cosAngle);
				}


				TEXTURE2D(_MainTex);
				SAMPLER(sampler_MainTex);
			
				TEXTURE2D(_NormalTex);
				SAMPLER(sampler_NormalTex);

				Varyings vert(Attributes IN) {
					Varyings OUT;

					VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.positionOS.xyz);
					OUT.positionCS = positionInputs.positionCS;
					OUT.fogFactor = ComputeFogFactor(positionInputs.positionCS.z);


					OUT.uv = IN.uv;
					OUT.color = IN.color;

					OUT.positionWS = positionInputs.positionWS;


					#if defined(_NORMALMAP_ON)  && !defined(_SPECULARHIGHLIGHTS_OFF)
					
						VertexNormalInputs normalInputs = GetVertexNormalInputs(IN.normalOS.xyz, IN.tangentOS);
						
					#else
						VertexNormalInputs normalInputs = GetVertexNormalInputs(IN.normalOS.xyz);
					#endif

					OUT.normalWS = normalInputs.normalWS;

					#ifndef _SPECULARHIGHLIGHTS_OFF
					#ifdef _NORMALMAP_ON
						real sign = IN.tangentOS.w * GetOddNegativeScale();
						half4 tangentWS = half4(normalInputs.tangentWS.xyz, sign);
						OUT.tangentWS = tangentWS;
					#endif
					#endif


					OUT.vertexSH = SampleSH(OUT.normalWS);

					#ifdef _ADDITIONAL_LIGHTS_VERTEX
						OUT.vertexLighting = VertexLighting(OUT.positionWS, OUT.normalWS);
					#endif

					return OUT;
				}

				half4 frag(Varyings IN) : SV_Target{




					float3 viewDir = normalize(_WorldSpaceCameraPos - IN.positionWS);
					float viewDirMask = step(_ViewDirMaskThreshold, abs(dot(viewDir, IN.normalWS)));



					float2 uv = IN.uv;
					float4 mainTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
					mainTex.rgb = pow(abs(mainTex.rgb), _AlbedoPower) * 2.0;
					
					mainTex = mainTex * _BaseColor * IN.color;
					mainTex.rgb = ApplyHueShift(mainTex.rgb, _HueShift);

					float4 col = mainTex;
					col.rgb *= _ColorIntensity;



					float metallic = 0.0;
					float smoothness = _Smoothness;

			
					#ifndef _SPECULARHIGHLIGHTS_OFF
					#ifdef _NORMALMAP_ON  
						half3 normalTS = SampleNormal(IN.uv, TEXTURE2D_ARGS(_NormalTex, sampler_NormalTex), _NormalScale);
						float sgn = IN.tangentWS.w; 
						float3 bitangent = sgn * cross(IN.normalWS.xyz, IN.tangentWS.xyz);
						half3x3 tangentToWorld = half3x3(IN.tangentWS.xyz, bitangent.xyz, IN.normalWS.xyz);
						IN.normalWS = TransformTangentToWorld(normalTS, tangentToWorld);
					#endif
					#endif


					SurfaceData surfaceData;
					surfaceData.albedo = col.rgb;
					surfaceData.specular = half3(0,0,0);
					surfaceData.metallic = metallic;
					surfaceData.smoothness = smoothness;
					surfaceData.normalTS = half3(0, 0, 1);
					surfaceData.emission = half3(0,0,0);
					surfaceData.occlusion = 1.0;
					surfaceData.alpha = col.a;
					surfaceData.clearCoatMask = 0;
					surfaceData.clearCoatSmoothness = 1;


					BRDFData brdfData;
					InitializeBRDFData(surfaceData, brdfData);
					BRDFData noClearCoat = (BRDFData)0;

					half4 shadowCoord = TransformWorldToShadowCoord(IN.positionWS.xyz);
					Light mainLight = GetMainLight(shadowCoord);
					IN.normalWS = NormalizeNormalPerPixel(IN.normalWS);




					half3 viewDirectionWS = GetWorldSpaceNormalizeViewDir(IN.positionWS);
					half3 bakedGI = half3(0, 0, 0);
					bakedGI = IN.vertexSH;

					half3 GI_Base = GlobalIllumination(brdfData, bakedGI, 1, IN.normalWS, viewDirectionWS);
					half3 giColor = GI_Base;

					half3 lightingColor = LightingPhysicallyBased(brdfData, noClearCoat, mainLight, IN.normalWS, viewDirectionWS, 0.0, false);
					col.rgb += lightingColor.rgb;
				
			
				

					#ifdef _ADDITIONAL_LIGHTS
						uint pixelLightCount = GetAdditionalLightsCount();
						for (uint lightIndex = 0u; lightIndex < pixelLightCount; ++lightIndex)
						{
							Light light = GetAdditionalLight(lightIndex, IN.positionWS);
							col.rgb += LightingPhysicallyBased(brdfData, noClearCoat,light,IN.normalWS, viewDirectionWS, 0.0, false);
						}
					#endif


	
					#ifdef _ADDITIONAL_LIGHTS_VERTEX
					col.rgb += surfaceData.albedo * IN.vertexLighting;
					#endif


					col.rgb += mainTex.rgb * _AmbientColorIntensity;
					col.rgb += giColor/10.0;
					col.a *= viewDirMask;

					//Fog
					col.rgb = MixFog(col.rgb, IN.fogFactor);


					return col;
				}
			ENDHLSL
		}

			Pass {

				Name "ShadowCaster"
				Tags { "LightMode" = "ShadowCaster" }

				ZWrite On
				ZTest LEqual

				HLSLPROGRAM
				#pragma vertex ShadowPassVertex
				#pragma fragment ShadowPassFragment


				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"


				half3 _LightDirection;
				half3 _LightPosition;


				TEXTURE2D(_MainTex);
				SAMPLER(sampler_MainTex);


				struct Attributes
				{
					half4 positionOS   : POSITION;
					half3 normalOS     : NORMAL;
					half2 texcoord     : TEXCOORD0;

				};

				struct Varyings
				{
					half2 uv           : TEXCOORD0;
					half4 positionCS   : SV_POSITION;
				};

				half4 GetShadowPositionHClip(Attributes input)
				{
					half3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
					half3 normalWS = TransformObjectToWorldNormal(input.normalOS);

				#if _CASTING_PUNCTUAL_LIGHT_SHADOW
					half3 lightDirectionWS = normalize(_LightPosition - positionWS);
				#else
					half3 lightDirectionWS = _LightDirection;
				#endif

					half4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS));

				#if UNITY_REVERSED_Z
					positionCS.z = min(positionCS.z, UNITY_NEAR_CLIP_VALUE);
				#else
					positionCS.z = max(positionCS.z, UNITY_NEAR_CLIP_VALUE);
				#endif

					return positionCS;
				}

				Varyings ShadowPassVertex(Attributes input)
				{
					Varyings output;
					UNITY_SETUP_INSTANCE_ID(input);

					output.uv = input.texcoord;
					output.positionCS = GetShadowPositionHClip(input);
					return output;
				}

				half4 ShadowPassFragment(Varyings input) : SV_TARGET
				{
					half alpha = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv).a * _BaseColor.a;
					clip(alpha - _Cutoff);
					return 0;
				}
				ENDHLSL

			}

		}
}