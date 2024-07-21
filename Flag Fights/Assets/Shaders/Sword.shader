// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Sword"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			o.Albedo = tex2D( _TextureSample0, uv_TextureSample0 ).rgb;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float mulTime6 = _Time.y * 3.5;
			float smoothstepResult42 = smoothstep( 0.75 , 1.0 , ( 1.0 - i.uv_texcoord.x ));
			float3 temp_cast_1 = (saturate( ( sin( ( ( 1.0 - ase_vertex3Pos.y ) + mulTime6 ) ) - smoothstepResult42 ) )).xxx;
			o.Emission = temp_cast_1;
			o.Metallic = 1.0;
			o.Smoothness = 0.8;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
521;479;1056;560;1334.977;248.2601;1.721947;False;False
Node;AmplifyShaderEditor.RangedFloatNode;15;-1247.665,107.3726;Inherit;False;Constant;_Float2;Float 2;1;0;Create;True;0;0;0;False;0;False;3.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;5;-1276.944,-46.91705;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;6;-1108.083,104.752;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;35;-1192.833,233.4723;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;47;-1050.427,14.44258;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;8;-887.3964,8.405046;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;37;-942.6628,270.1969;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-926.7398,510.6342;Inherit;False;Constant;_Float6;Float 6;1;0;Create;True;0;0;0;False;0;False;0.75;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;10;-708.2728,-20.63875;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;42;-726.8335,264.4723;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;36;-442.8099,65.1544;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3;-405.8116,-194.8508;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;86cb957541c7c8542a6306ffb82e6833;86cb957541c7c8542a6306ffb82e6833;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;2;-190.6,333.5;Inherit;False;Constant;_Float1;Float 1;0;0;Create;True;0;0;0;False;0;False;0.8;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;1;-187.6,250.5;Inherit;False;Constant;_Float0;Float 0;0;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;14;-212.3135,39.22082;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Sword;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;0;15;0
WireConnection;47;0;5;2
WireConnection;8;0;47;0
WireConnection;8;1;6;0
WireConnection;37;0;35;1
WireConnection;10;0;8;0
WireConnection;42;0;37;0
WireConnection;42;1;43;0
WireConnection;36;0;10;0
WireConnection;36;1;42;0
WireConnection;14;0;36;0
WireConnection;0;0;3;0
WireConnection;0;2;14;0
WireConnection;0;3;1;0
WireConnection;0;4;2;0
ASEEND*/
//CHKSM=9BD8A047EC8A5439A06F52D893D65CAD7C6BFB77