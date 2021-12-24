

Shader "UI/MiniMapUIShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Mask("Mask", 2D) = "white" {} 
		_OutlineColor("OutlineColor", Color) = (1.0, 1.0, 1.0, 1.0)
		_OutlineWidth("OutlineWidth", Int) = 1
	}
		SubShader
		{
			Tags { "RenderType" = "Transparent" "RenderQueue" = "Transparent" }
			LOD 100
			ZWrite Off

			Blend SrcAlpha OneMinusSrcAlpha

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				// make fog work
				#pragma multi_compile_fog

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				sampler2D _Mask;
				float2 samples[9] = { float2(-1.0, -1.0f),
									  float2(0.0, -1.0f),
									  float2(1.0, -1.0),
									  float2(-1.0, 0.0),
									  float2(0.0, 0.0),
									  float2(1.0, 0.0),
									  float2(-1.0, 1.0),
									  float2(0.0, 1.0),
									  float2(1.0, 1.0), };
				float4 _MainTex_ST;
				float4 _MainTex_TexelSize;
				float4 _OutlineColor;
				int _OutlineWidth;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					// sample the texture
					fixed4 col = tex2D(_MainTex, i.uv);

					float4 c = (float4)0;
					if (col.a != 0.f)
					{
						for (int index = 0; index < 9; index++)
						{
							c = tex2D(_Mask, i.uv + samples[index] * _OutlineWidth);
							if (c.r == 0)
								return _OutlineColor;
						}
					}
					return col;
				}
				ENDCG
			}
		}
}
