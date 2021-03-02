Shader "Nick/LED_Display_Alt_Tri_2Pass" {

	Properties {
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_NormTex("Normal Map", 2D) = "bump" {}
		_OcclusionTex("Occlusion Map", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		//--
		_Size("LED Size", Range(0, 3)) = 0.5
		_Brightness("LED Brightness", Range(1, 200)) = 1.0
	}

	SubShader {
		Tags { "RenderType" = "Opaque" }
		LOD 200

		//Pass {
			 CGPROGRAM
			 #pragma surface surf Standard fullforwardshadows
			 #pragma target 3.0 
			 
			 sampler2D _MainTex;
			 sampler2D _NormTex;
			 sampler2D _OcclusionTex;

			 struct Input {
				 float2 uv_MainTex;
			 };

			 half _Glossiness;
			 half _Metallic;
			 fixed4 _Color;

			 void surf(Input IN, inout SurfaceOutputStandard o) {
				 // Albedo comes from a texture tinted by color
				 fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				 fixed4 Occ = tex2D(_OcclusionTex, IN.uv_MainTex);
				 o.Albedo = c.rgb * Occ; // Combine normal color with the vertex color
				 o.Emission = c.rgb;
				 o.Normal = UnpackNormal(tex2D(_NormTex, IN.uv_MainTex));
				 // Metallic and smoothness come from slider variables
				 o.Metallic = _Metallic;
				 o.Smoothness = _Glossiness;
				 o.Alpha = c.a;
			 }
			 ENDCG
		//}

		Pass {
			CGPROGRAM
			#pragma require geometry
			#pragma vertex vert
			#pragma fragment frag
			#pragma geometry geom

			#include "UnityCG.cginc"

			float _Size;
			float _Brightness;

			struct appdata {
				float4 vertex : POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2g {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			struct g2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			v2g vert(appdata_full v) {
				v2g o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.vertex = mul(unity_ObjectToWorld, v.vertex);
				//o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				o.color = v.color;

				return o;
			}

			[maxvertexcount(3)]
			void geom(triangle v2g IN[3], inout TriangleStream<g2f> tristream) {
				g2f o;

				float3 up = float3(0, 1, 0);
				float3 look = _WorldSpaceCameraPos - IN[0].vertex;
				look.y = 0;
				look = normalize(look);
				float3 right = cross(up, look);

				float halfS = 0.5f * _Size;

				float4 v[3];
				v[0] = float4(IN[0].vertex + (halfS * 2) * right - (halfS * 2) * up, 1.0f);
				//v[0] = float4(IN[0].vertex + halfS * right - halfS * up, 1.0f);
				v[1] = float4(IN[0].vertex + halfS * right + halfS * up, 1.0f);
				v[2] = float4(IN[0].vertex - halfS * right - halfS * up, 1.0f);

				float4x4 vp;
				#if UNITY_VERSION >= 560 
				vp = mul(UNITY_MATRIX_MVP, unity_WorldToObject);
				#else 
				#if UNITY_SHADER_NO_UPGRADE 
				vp = mul(UNITY_MATRIX_MVP, unity_WorldToObject);
				#endif
				#endif

				o.pos = mul(vp, v[0]);
				o.uv = float2(1.0f, 0.0f);
				o.color = IN[0].color; 
				tristream.Append(o);

				o.pos = mul(vp, v[1]);
				o.uv = float2(1.0f, 1.0f);
				o.color = IN[0].color;
				tristream.Append(o);

				o.pos = mul(vp, v[2]);
				o.uv = float2(0.0f, 0.0f);
				o.color = IN[0].color;
				tristream.Append(o);
			}

			fixed4 frag(g2f i) : SV_Target {
				return  i.color * _Brightness;
			} 
			ENDCG
		}
	}
}