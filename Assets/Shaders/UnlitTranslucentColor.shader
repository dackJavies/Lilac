Shader "Unlit/Translucent Color" {
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
		Lighting Off

		Blend SrcAlpha OneMinusSrcAlpha

		Pass {
			Color [_BaseColor]
			SetTexture [_PrimaryTexture] {
				combine Primary, Texture * Primary
			}
		}
	}
}