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
    //float1 constant;
    //float1 linear;
    //float1 quadratic;
};