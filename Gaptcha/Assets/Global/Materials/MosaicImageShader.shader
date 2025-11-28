Shader "Debuff/MosaicImageShader"
{
    Properties
    {
        _OverrideTex("Override Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
    }

        SubShader
        {
            Tags
            {
                "RenderType" = "Transparent"
                "Queue" = "Transparent"
                "IgnoreProjector" = "True"
                "PreviewType" = "Plane"
                "CanUseSpriteAtlas" = "False"
            }

            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            Lighting Off
            ZWrite Off

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                sampler2D _OverrideTex;
                float4 _OverrideTex_TexelSize;
                float4 _Color;

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float4 vertex : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);

                    // 텍스처의 실제 width/height 읽기
                    float texWidth = _OverrideTex_TexelSize.z;
                    float texHeight = _OverrideTex_TexelSize.w;

                    // 자동 타일링 비례값 (텍스처가 작을수록 더 많이 반복됨)
                    float2 autoTile = float2(
                         1000/ texWidth,   // 원하는 기준값(예: 100px)
                        1000/ texHeight
                    );

                    o.uv = v.uv * autoTile;

                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    return tex2D(_OverrideTex, i.uv) * _Color;
                }
                ENDCG
            }
        }

            FallBack "Sprites/Default"
}