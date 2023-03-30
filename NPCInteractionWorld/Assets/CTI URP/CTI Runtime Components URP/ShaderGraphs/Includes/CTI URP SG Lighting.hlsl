void CTI_SimpleTranslucentLighting_float(
   float3   PositionWS,
   half3    NormalWS,
   half3    TangentWS,
   half3    BitangentWS,

   half3    Albedo,
   half3    NormalTS,
   half3    Transmission,
   half     TransmissionMask,

   // out half3 Direction,
   // out half3 Color,
   // out half DistanceAtten,
   // out half ShadowAtten

   out half3 o_NormalWS,
   out half3 o_Transmission
)
{
   #ifdef SHADERGRAPH_PREVIEW
      o_NormalWS = half3(0,1,0);
      o_Transmission = 0;
   #else

half3 translucency = 1;

      half3 viewDirWS = GetWorldSpaceNormalizeViewDir(PositionWS);
      half3x3 tangentToWorld = half3x3(TangentWS, BitangentWS, NormalWS);
      o_NormalWS = NormalizeNormalPerPixel(TransformTangentToWorld(NormalTS, tangentToWorld));

      half4 shadowCoord = TransformWorldToShadowCoord(PositionWS);
      Light mainLight = GetMainLight(shadowCoord);

      half w = 0.3; // 0.4
      half NdotL = saturate((dot(o_NormalWS, mainLight.direction) + w) / ((1 + w) * (1 + w)));

      half3 transLightDir = mainLight.direction + NormalWS * Transmission.z;
      half transDot = dot( transLightDir, -viewDirWS );
      transDot = exp2(saturate(transDot) * Transmission.y - Transmission.y);
      o_Transmission = TransmissionMask * transDot * (1.0 - NdotL) * mainLight.color * mainLight.shadowAttenuation * Albedo * Transmission.x;

   #endif
}

void SampleSH_half(half3 normalWS, out half3 Ambient)
{
    // LPPV is not supported in Ligthweight Pipeline
    real4 SHCoefficients[7];
    SHCoefficients[0] = unity_SHAr;
    SHCoefficients[1] = unity_SHAg;
    SHCoefficients[2] = unity_SHAb;
    SHCoefficients[3] = unity_SHBr;
    SHCoefficients[4] = unity_SHBg;
    SHCoefficients[5] = unity_SHBb;
    SHCoefficients[6] = unity_SHC;

    Ambient = max(half3(0, 0, 0), SampleSH9(SHCoefficients, normalWS));
}