#pragma once

#include "../../Common/Math.hlsli"
#include "Lights.hlsli"

float3 FresnelSchlick(const float cosTheta, const float3 f0)
{
    return f0 + (1.0 - f0) * pow(clamp(1.0 - cosTheta, 0.0, 1.0), 5.0);
}

float DistributionGGX(const float3 n, const float3 h, const float roughness)
{
    const float a = roughness * roughness;
    const float a2 = a * a;
    const float nDotH = max(dot(n, h), 0.0);
    const float nDotH2 = nDotH * nDotH;

    const float num = a2;

    float denom = nDotH2 * (a2 - 1.0) + 1.0;
    denom = PI * denom * denom;

    return num / denom;
}

float GeometrySchlickGGX(const float nDotV, const float roughness)
{
    const float r = roughness + 1.0;
    const float k = (r * r) / 8.0;

    const float num = nDotV;
    const float denom = nDotV * (1.0 - k) + k;

    return num / denom;
}

float GeometrySmith(const float3 n, const float3 v, const float3 l, const float roughness)
{
    const float nDotV = max(dot(n, v), 0.0);
    const float nDotL = max(dot(n, l), 0.0);
    const float ggx2 = GeometrySchlickGGX(nDotV, roughness);
    const float ggx1 = GeometrySchlickGGX(nDotL, roughness);

    return ggx1 * ggx2;
}

float3 ProcessLight(const float3 albedo, const float3 normal, const float metallic, const float roughness, const float3 viewDir, const float3 lightPos, const float3 radiance)
{
    const float3 f0 = lerp((float3) 0.04, albedo, metallic);

    const float3 n = normalize(normal);
    const float3 h = normalize(viewDir + lightPos);

    const float ndf = DistributionGGX(n, h, roughness);
    const float g = GeometrySmith(n, viewDir, lightPos, roughness);
    const float3 f = FresnelSchlick(max(dot(h, viewDir), 0.0), f0);

    const float3 kS = f;
    float3 kD = (float3) 1.0 - kS;
    kD *= 1.0 - metallic;

    const float3 numerator = ndf * g * f;
    const float3 denominator = 4.0 * max(dot(n, viewDir), 0.0) * max(dot(n, lightPos), 0.0) + 0.00001;
    const float3 specular = numerator / denominator;

    const float nDotL = max(dot(n, lightPos), 0.0);

    return (kD * albedo / PI + specular) * radiance * nDotL;
}

float3 ProcessLightInfo(LightInfo light, const float3 viewDir, const float3 albedo, const float3 normal, const float metallic, const float roughness)
{
    switch (light.type)
    {
        case LightType::Directional:
        {
            const float3 lightPos = normalize(-light.position.xyz);
            const float3 radiance = light.color.rgb;

            return ProcessLight(albedo, normal, metallic, roughness, viewDir, lightPos, radiance);
        }
        default:
            return (float3) 0.0;
    }
}