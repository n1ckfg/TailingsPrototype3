// https://forum.unity.com/threads/how-do-i-make-a-simple-unlit-cutout-shader.517017/

Shader "Nick/UnlitLumaCutoutDouble" {

	Properties {
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		_Cutoff("Luma cutoff", Range(0,1)) = 0.5
		_Height("Height cutoff", Range(0, 0.01)) = 0.0
	}

	SubShader {
		Tags {"Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout"}
		LOD 100

		Lighting Off
		Cull Off

		Pass {
			CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members test)
#pragma exclude_renderers d3d11
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				float4 localvertex : TEXCOORD1;
				UNITY_FOG_COORDS(2)
				UNITY_VERTEX_OUTPUT_STEREO
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _Cutoff;
			fixed _Height;

			v2f vert(appdata_t v) {
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.localvertex = v.vertex;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target {
				fixed4 col = fixed4(0,0,0,1);
				if (i.localvertex.y > _Height) col = tex2D(_MainTex, i.texcoord);
				fixed avg = (col.r + col.g + col.b) / 3.0;
				clip(avg - _Cutoff);
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}

}