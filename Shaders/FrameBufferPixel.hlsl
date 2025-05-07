Texture2D FrameBufferTexture : register(t0);
SamplerState samplerState : register(s0);

struct PSInput
{
    float2 TexCoords : TEXCOORD0;
};

struct PSOutput
{
    float4 outColor : SV_TARGET;
};

static const float Gamma = 2.2f;
static const float Exposure = 1.0f;

PSOutput main(PSInput input)
{
    PSOutput output;
    
    float3 Color = FrameBufferTexture.Sample(samplerState, input.TexCoords).rgb;
    float3 result = Color;

    output.outColor = float4(result, 1.0f);
    return output;
}