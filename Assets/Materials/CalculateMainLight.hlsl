void CalculateMainLight_float(in float3 WorldPos, out float3 Direction, out float3 LightColor)
{
#ifdef SHADERGRAPH_PREVIEW
    Direction = float3(0.5, 0.5, 0);
    LightColor = 1;
#else
    #if SHADOWS_SCREEN
        half4 clipPos = TranformWorldToHClip(worldPos);
        half4 shadowCoord = ComputeScreenPos(clipPos);
    #else
        half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
    #endif

    #if _MAIN_LIGHT_SHADOWS_CASCADE || _MAIN_LIGHT_SHADOWS
        Light light = GetMainLight(shadowCoord);
    #else
        Light light = GetMainLight(0);
    #endif
    Direction = light.direction;
    LightColor = light.color;
#endif
}