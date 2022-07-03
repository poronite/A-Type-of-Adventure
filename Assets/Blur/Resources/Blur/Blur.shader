Shader "Hidden/wOUShLab/Blur"
{
  HLSLINCLUDE

        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
  
        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
        float2 _MainTex_TexelSize;
        float _BlurAmount;
        static float _Directions = 16;
        static float _Quality = 5;
        static const float pi = 6.28318530718;
  
        float4 Frag(VaryingsDefault i) : SV_Target
        {
            float2 uv = i.texcoord;
            float offset = _BlurAmount / _ScreenParams;
            float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
            
            for(float d = 0.0; d < pi; d += pi / _Directions)
                {
                    for(float i = 1.0 / _Quality; i <= 1.0; i += 1.0 /_Quality)
                    {
                        col += SAMPLE_TEXTURE2D( _MainTex, sampler_MainTex, uv + float2(cos(d), sin(d)) * offset * i);		
                    }
                }

            col.rgb /= _Quality * _Directions - 15.0;
            
            return col;
        }
  ENDHLSL
    
  SubShader
  {
      Cull Off ZWrite Off ZTest Always
      Pass
      {
          HLSLPROGRAM
              #pragma vertex VertDefault
              #pragma fragment Frag
          ENDHLSL
      }
  }
}

