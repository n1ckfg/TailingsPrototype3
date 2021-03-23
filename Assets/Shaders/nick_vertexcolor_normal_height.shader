// http://answers.unity3d.com/questions/923726/unity-5-standard-shader-support-for-vertex-colors.html

Shader "Nick/VertexColor_Normal_Height" {

     Properties {
		 _Height ("Height", Range(0, 1)) = 0
         _Color ("Color", Color) = (1,1,1,1)
		 _MainTex("Albedo (RGB)", 2D) = "white" {}
		 _NormTex("Normal Map", 2D) = "bump" {}
		 _OcclusionTex("Occlusion Map", 2D) = "white" {}
		 _Glossiness ("Smoothness", Range(0,1)) = 0.5
         _Metallic ("Metallic", Range(0,1)) = 0.0
     }

     SubShader {
         Tags { "RenderType"="Opaque" }
         LOD 200
         
         CGPROGRAM
         #pragma surface surf Standard vertex:vert fullforwardshadows
         #pragma target 3.0
         struct Input {
             float2 uv_MainTex;
			 float3 vertexColor; // Vertex color stored here by vert() method
			 float3 vertex; // Vertex color stored here by vert() method
		 };
         
         struct v2f {
           float4 pos : SV_POSITION;
           fixed4 color : COLOR;
         };
 
         void vert (inout appdata_full v, out Input o)
         {
             UNITY_INITIALIZE_OUTPUT(Input, o);
             o.vertexColor = v.color; // Save the Vertex Color in the Input for the surf() method
			 o.vertex = v.vertex;
         }
 
		 sampler2D _MainTex;
		 sampler2D _NormTex;
		 sampler2D _OcclusionTex;

		 half _Height;
         half _Glossiness;
         half _Metallic;
         fixed4 _Color;
 
         void surf (Input IN, inout SurfaceOutputStandard o) 
         {
             // Albedo comes from a texture tinted by color
             fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			 fixed4 Occ = tex2D(_OcclusionTex, IN.uv_MainTex);
			 
			 half h = _Height + (_SinTime[1]/10);
			 //if (IN.vertex.y > h / 100) {
				 //o.Albedo = IN.vertexColor * Occ; // Combine normal color with the vertex color
				 //o.Emission = -Occ;
			 //} else {
				 o.Albedo = c.rgb * IN.vertexColor * Occ;
				 o.Emission = c.rgb * IN.vertexColor;
			 //}

			 o.Normal = UnpackNormal(tex2D(_NormTex, IN.uv_MainTex));
			 // Metallic and smoothness come from slider variables
             o.Metallic = _Metallic;
             o.Smoothness = _Glossiness;
             o.Alpha = c.a;
         }
         ENDCG
     } 

     FallBack "Diffuse"

 }