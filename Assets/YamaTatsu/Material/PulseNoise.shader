Shader "Unlit/PulseNoise"
{
	Properties
	{
		//メインテクスチャ　2D
		_MainTex ("Sprite Texture", 2D) = "white" {}
		//ゆがめる量
		_Amount ("Distort",Float) = 0.0
	}
	SubShader
	{
		//レンダラータイプを
		Tags { "RenderType"="Transparent" }
		LOD 100

		Pass
		{
			CGPROGRAM
			//"vert" 関数を頂点シェーダーとしｔｒ使います
			#pragma vertex vert
			//"frag"関数をピクセル(フラグメント)シェーダーとして使います
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			//頂点シェーダー入力
			struct appdata
			{
				//頂点位置
				float4 vertex : POSITION;
				//テクスチャ座標
				float2 uv : TEXCOORD0;
			};
			
			//vertexシェーダーの出力
			struct v2f
			{
				//テクスチャ座標
				float2 uv : TEXCOORD0;
				//クリップスペース位置
				float4 vertex : SV_POSITION;
			};

			//サンプリングするテクスチャ
			sampler2D _MainTex;
			//
			float4 _MainTex_ST;
			//ゆがめる量
			float _Amount;
			
			//頂点シェーダー
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			//テクスチャを横にずらす処理
			fixed4 frag (v2f i) : SV_Target
			{
				float2 uv = i.uv;
				float x = 2 * uv.y;
				uv.x += _Amount*sin(10 * x)*(-(x - 1)*(x - 1) + 1);
				//テクスチャをサンプリングしてそれを返す
				fixed4 col = tex2D(_MainTex, uv);
				return col;
			}
			ENDCG
		}
	}
}
