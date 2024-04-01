Shader "Unlit/Outline"
{
	Properties
	{
		_ColorRenderer("Object Color", Color) = (0, 1, 0.94, 1)		
		_ColorOutline("Outline Color", Color) = (1, 0, 0.8, 1)
		_Thickness("Outline Thickness", Range(0,0.2)) = 0.1
	}

	SubShader
	{
		Pass
		{
			Stencil
			{
				Ref 1
				Comp Always
				Pass Replace
			}
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			struct Input
			{
				float4 vertex : POSITION;
			};

			float4 _ColorRenderer;

			float4 vert(Input input) : POSITION
			{
				return UnityObjectToClipPos(input.vertex);
			}

			float4 frag(void) : COLOR
			{
				return _ColorRenderer;
			}
			ENDCG
		}

		Pass
		{
			Cull Off
			Stencil
			{
				Ref 1
				Comp NotEqual
				Pass Keep
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			struct Input
			{
				float4 vertex : POSITION;
				float4 normal : TANGENT;
			};

			float4 _ColorOutline;
			float _Thickness;

			float4 vert(Input input) : SV_POSITION
			{
				return UnityObjectToClipPos(input.vertex + _Thickness * input.normal);
			}

			float4 frag(void) : COLOR
			{
				return _ColorOutline;
			}
			ENDCG
		}
	}
}