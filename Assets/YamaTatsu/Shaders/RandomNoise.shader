Shader "Custom/RandomNoise" {
	Properties {
		_MainTex("Albedo (RGB)",2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		//サンプリングするテクスチャ
		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		//ランダムの処理
		float random(fixed2 p) {
			return frac(sin(dot(p, fixed2(12.9898, 78.233))) * 43758.5453);
		}

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {

			float c = random(IN.uv_MainTex);
			o.Albedo = fixed4(c, c, c, 1);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
