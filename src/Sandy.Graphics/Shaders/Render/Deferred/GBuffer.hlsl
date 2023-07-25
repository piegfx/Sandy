struct VSInput
{
    float3 position: POSITION0;
    float2 texCoord: TEXCOORD0;
    float4 color:    COLOR0;
    float3 normal:   NORMAL0;
    float3 tangent:  TANGENT0;
};

struct VSOutput
{
    float4 position: SV_Position;
    float2 texCoord: TEXCOORD0;
    float3 fragPos:  TEXCOORD1;
    float3 normal:   TEXCOORD2;
    float4 color:    COLOR0;
};

struct PSOutput
{
    float4 albedo:            SV_Target0;
    float4 fragPos:           SV_Target1;
    float4 normal:            SV_Target2;
    float4 metallicRoughness: SV_Target3;
};

cbuffer CameraMatrices : register(b0)
{
    float4x4 projection;
    float4x4 view;
    float4   position;
}

cbuffer DrawInfo : register(b2)
{
    float4x4 world;
}

Texture2D albedoTexture     : register(t3);
SamplerState albedoState    : register(s3);
Texture2D normalTexture     : register(t4);
SamplerState normalState    : register(s4);
Texture2D metallicTexture   : register(t5);
SamplerState metallicState  : register(s5);
Texture2D roughnessTexture  : register(t6);
SamplerState roughnessState : register(s6);
Texture2D occlusionTexture  : register(t7);
SamplerState occlusionState : register(s7);

VSOutput VertexShader(const in VSInput input)
{
    VSOutput output;

    const float4 fragPos = mul(world, float4(input.position, 1.0));
    output.position = mul(projection, mul(view, fragPos));

    output.fragPos = fragPos.xyz;
    
    output.texCoord = input.texCoord;
    output.color = input.color;

    output.normal = mul((float3x3) world, input.normal);
    
    return output;
}

PSOutput PixelShader(const in VSOutput input)
{
    PSOutput output;

    float3 outColor = albedoTexture.Sample(albedoState, input.texCoord).xyz * input.color.xyz;
    
    // Output alpha MUST be 1.0.
    output.albedo = float4(outColor, 1.0);

    output.fragPos = float4(input.fragPos, 1.0);

    // Calculate normal and output our normal map.
    // TODO: Calculate TBN matrix in the vertex shader.
    const float3 tangentNormal = normalTexture.Sample(normalState, input.texCoord).xyz * 2.0 - 1.0;
    const float3 q1 = ddx(input.fragPos);
    const float3 q2 = ddy(input.fragPos);
    const float2 uv1 = ddx(input.texCoord);
    const float2 uv2 = ddy(input.texCoord);

    const float3 n = normalize(input.normal);
    const float3 t = normalize(q1 * uv2.y - q2 * uv1.y);
    const float3 b = -normalize(cross(n, t));
    const float3x3 tbn = float3x3(t, b, n);

    output.normal = float4(normalize(mul(tangentNormal, tbn)), 1.0);
    
    //output.normal = float4(normalTexture.Sample(normalState, input.texCoord).xyz, 1.0);
    
    output.metallicRoughness = float4(
        metallicTexture.Sample(metallicState, input.texCoord).r,
        roughnessTexture.Sample(roughnessState, input.texCoord).r,
        occlusionTexture.Sample(occlusionState, input.texCoord).r,
        1.0
    );
    
    return output;
}