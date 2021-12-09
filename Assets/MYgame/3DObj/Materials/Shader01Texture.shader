Shader "Unlit/Shader01Texture"
{
	Properties
	{

		_Color("Main Color", Color) = (1,1,1,1)
		_PlayerLightPos("_PlayerLightPos", Vector) = (1, 0, 0, 0)
		[NoScaleOffset]_MainTex("Texture", 2D) = "white" {}
	}

	SubShader
	{
		Tags
		{
			  "RenderType" = "Transparent"
			  "Queue" = "Transparent"
		}

		Pass
		{
			//Cull Back
			//ZWrite Off
			//ZTest LEqual
			//ZTest Always
			//ZTest GEqual
		   // Blend One One  // additive
			Blend SrcAlpha OneMinusSrcAlpha
			//Blend SrcAlpha OneMinusDstAlpha

		  //  Blend DstColor Zero  // multiply

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			#define TAU 6.28318530718

			sampler2D _MainTex;
			float4 _Color;
			float4 _PlayerLightPos;


			struct MeshData
			{
				float4 vertex   : POSITION;
				float3 nomals   : NORMAL;
				//float4 tangent  : TANGENT;
				//float4 color    : COLOR;
				float4 uv0      : TEXCOORD0;
			};

			struct Interpolators
			{
				float4 vertex		: SV_POSITION;
				float3 normal		: TEXCOORD0;
				float2 uv			: TEXCOORD1;
				float3 vertexWord   : TEXCOORD2;
			};


			Interpolators vert(MeshData v)
			{
				Interpolators o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.vertexWord = mul(unity_ObjectToWorld, v.vertex);
				o.vertexWord.y = 0;
				o.normal = UnityObjectToWorldNormal(v.nomals);  // World normal
				//o.uv = (v.uv + _Offset) * _Scale;
				o.uv = v.uv0;

				return o;
			}


			float4 frag(Interpolators i) : SV_Target
			{
				float4 col = tex2D(_MainTex, i.uv);

				float3 PlayPos = float3(_PlayerLightPos.x, 0, _PlayerLightPos.z);

				float lTempRatio = 1 - (distance(PlayPos, i.vertexWord) / 1.5);
				lTempRatio = lTempRatio - 0.5;
				float lTemp2 = clamp(lTempRatio, 0.0, 1.0);
				float lTempAlpha = lTemp2 > 0 ? 1 : 0;

				return float4(col.rgb, lTempAlpha * col.a);
			}
			ENDCG
		}
	}
}