﻿//Courtesy of Code Avarice

Shader "Custom/DisolveShader" {
	Properties {
        _Color("Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _Emission("Emission", Color) = (1.0, 1.0, 1.0, 1.0)
        _EmissionMap("Emission Map", 2D) = "white" {}

		_MainTex("Texture (RGB)", 2D) = "white" {}
		_SliceGuide ("SliceGuide (RGB)", 2D) = "white" {}
		_SliceAmount ("Slice Amount", Range(0.0, 1.0)) = 0.5
		
		_BurnSize ("BurnSize", Range(0.0, 1.0)) = 0.15
		_BurnRamp ("Burn Ramp (RGB)", 2D) = "white" {}	
	}
	
	SubShader {
		Tags {"RenderType" = "Opaque"}
		Cull Off

		CGPROGRAM
		
		//remove "addshadow" if we're not using shadows, which we probably should
		#pragma surface surf Lambert addshadow
		
		struct Input {
			float2 uv_MainTex;
            float2 uv_EmissionMap;
			float2 uv_SliceGuide;
			float _SliceAmount;
		};
		
        uniform float4 _Color;
        uniform float4 _Emission;
        sampler2D _EmissionMap;

		sampler2D _MainTex;
		sampler2D _SliceGuide;
		float _SliceAmount;
		
		sampler2D _BurnRamp;
		float _BurnSize;
		
		void surf(Input In, inout SurfaceOutput o) {
			clip(tex2D(_SliceGuide, In.uv_SliceGuide).rgb - _SliceAmount);
			o.Albedo = (tex2D(_MainTex, In.uv_MainTex).rgb) * _Color;

            o.Emission = (tex2D(_EmissionMap, In.uv_EmissionMap).rgb) * _Emission;

			half test = tex2D(_SliceGuide, In.uv_MainTex).rgb - _SliceAmount;
			
			if (test < _BurnSize && _SliceAmount > 0 && _SliceAmount < 1) {
				o.Emission = tex2D(_BurnRamp, float2(test * (1/_BurnSize), 0));
				o.Albedo *= o.Emission;
			}
			
		}
		
		ENDCG
		
	}
	
	Fallback "Diffuse"
}