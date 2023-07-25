struct VSInput
{
    float2 position: POSITION0;
    float2 worldPos: POSITION1;
    float2 texCoord: TEXCOORD0;
    float4 tint:     COLOR0;
    float1 rotation: TEXCOORD1;
};

struct VSOutput
{
    float4 position: SV_Position;
    float2 texCoord: TEXCOORD0;
    float4 tint:     TEXCOORD1;
};

struct PSOutput
{
    float4 color: SV_Target0;
};

cbuffer SpriteMatrices : register(b0)
{
    float4x4 projection;
}

Texture2D sprite   : register(t1);
SamplerState state : register(s1);

VSOutput VertexShader(const in VSInput input)
{
    VSOutput output;

    const float cosRot = cos(input.rotation);
    const float sinRot = sin(input.rotation);
    const float2x2 rotMatrix = float2x2(
        float2(cosRot, -sinRot),
        float2(sinRot, cosRot)
    );

    output.position = mul(projection, float4(mul(rotMatrix, input.position) + input.worldPos, 0.0, 1.0));

    output.texCoord = input.texCoord;
    output.tint = input.tint;

    return output;
}

PSOutput PixelShader(const in VSOutput input)
{
    PSOutput output;

    output.color = sprite.Sample(state, input.texCoord) * input.tint;
    
    return output;
}