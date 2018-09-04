Shader "Unlit/Non-permeating 3D Text Shader" {
	Properties {
		_PrimaryTexture ("Primary Texture", 2D) = "white" {}
		_BaseColor 		("Base Color", Color) = (1, 1, 1, 1)
	}
 
	SubShader {
		Tags {
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
		}

		Cull Back
		ZWrite Off

		Blend SrcAlpha OneMinusSrcAlpha

		Pass {
			Color [_BaseColor]
			SetTexture [_PrimaryTexture] {
				combine Primary, Texture * Primary
			}
		}
	}
}