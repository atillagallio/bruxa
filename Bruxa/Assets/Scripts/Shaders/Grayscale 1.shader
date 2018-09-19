Shader "Custom/GrayscaledSprite"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Grayscale("GrayScale", float) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;

			uniform float _Grayscale;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				half brightness = dot(col.rgb, half3(0.3, 0.59, 0.11));
				half3 gray = brightness;
				// just invert the colors
				return fixed4(lerp(col.rgb, brightness, _Grayscale), col.a);
			}
			ENDCG
		}

	}
}
