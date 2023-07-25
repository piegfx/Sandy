#pragma once

enum class LightType
{
    None,
    Directional,
    Point
};

struct LightInfo
{
    float4 color;
    float3 position;
    LightType type;
};