struct VSOutput
{
    float4 position: SV_Position;
    float3 texCoord: TEXCOORD0;
};

cbuffer CameraMatrices : register(b0)
{
    float4x4 projection;
    float4x4 view;
    float4   position;
}

TextureCube cubemap : register(t1);
SamplerState state  : register(s1);

VSOutput VertexShader(const float3 position: POSITION)
{
    VSOutput output;

    const float4x4 view3x3 = float4x4(
        view._m00, view._m01, view._m02, 0.0,
        view._m10, view._m11, view._m12, 0.0,
        view._m20, view._m21, view._m22, 0.0,
        0.0,       0.0,       0.0,       1.0
    );
    
    const float4 pos = mul(projection, mul(view3x3, float4(position, 1.0)));
    output.position = pos.xyww;
    output.texCoord = position;

    return output;
}

float4 PixelShader(const VSOutput input) : SV_Target0 {
    return cubemap.Sample(state, input.texCoord);
}