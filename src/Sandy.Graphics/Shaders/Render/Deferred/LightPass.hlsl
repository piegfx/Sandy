#include "../Lighting/Lighting.hlsli"

struct VSInput
{
    uint id: SV_VertexID;
};

struct VSOutput
{
    float4 position: SV_Position;
    float2 texCoord: TEXCOORD0;
};

struct PSOutput
{
    float4 color: SV_Target0;
};

Texture2D albedoTexture             : register(t2);
SamplerState albedoState            : register(s2);
Texture2D fragPosTexture            : register(t3);
SamplerState fragPosState           : register(s3);
Texture2D normalTexture             : register(t4);
SamplerState normalState            : register(s4);
Texture2D metallicRoughnessTexture  : register(t5);
SamplerState metallicRoughnessState : register(s5);

[[vk::constant_id(0)]] const uint IS_OPENGL = 0;

cbuffer CameraMatrices : register(b0)
{
    float4x4 projection;
    float4x4 view;
    float4   position;
}

cbuffer SceneInfo : register(b1)
{
    LightInfo directionalLight;

    float ambientMultiplier;
}

VSOutput VertexShader(const in VSInput input)
{
    VSOutput output;

    const float4 vertices[] = {
        float4(-1.0,  1.0, 0.0, 0.0),
        float4( 1.0,  1.0, 1.0, 0.0),
        float4( 1.0, -1.0, 1.0, 1.0),
        float4(-1.0, -1.0, 0.0, 1.0),
    };

    const uint indices[] = {
        0, 1, 3,
        1, 2, 3
    };

    const float4 vertex = vertices[indices[input.id]];
    output.position = float4(vertex.xy, 0.0, 1.0);

    // Because OpenGL is OpenGL and starts with texture coordinates in the BOTTOM LEFT we have to flip the darn texture
    // coordinates so the thing looks correct. Thanks opengl!!!!!!111111111 best api ever!!!!!!!!!!
    if (IS_OPENGL)
        output.texCoord = float2(vertex.z, 1.0 - vertex.w);
    else
        output.texCoord = vertex.zw;

    return output;
}

PSOutput PixelShader(const in VSOutput input)
{
    PSOutput output;

    const float4 albedoCheck = albedoTexture.Sample(albedoState, input.texCoord);
    if (albedoCheck.a < 0.5)
        discard;

    const float3 albedo = pow(albedoCheck.xyz, 2.2);

    const float3 fragPos = fragPosTexture.Sample(fragPosState, input.texCoord).rgb;

    const float3 normal = normalTexture.Sample(normalState, input.texCoord).rgb;

    const float3 metallicRoughness = metallicRoughnessTexture.Sample(metallicRoughnessState, input.texCoord).rgb;
    const float metallic = metallicRoughness.r;
    const float roughness = metallicRoughness.g;
    const float ao = metallicRoughness.b;

    float3 color = (float3) ambientMultiplier * albedo * ao;

    const float3 viewDir = normalize(position.xyz - fragPos);
    
    color += ProcessLightInfo(directionalLight, viewDir, albedo, normal, metallic, roughness);

    color = color / (color + (float3) 1.0);

    output.color = float4(pow(color, (float3) (1.0 / 2.2)), 1.0);
    
    return output;
}