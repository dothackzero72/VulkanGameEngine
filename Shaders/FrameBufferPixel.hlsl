struct PSInput
{
    float4 Position : SV_POSITION; 
    float2 TexCoords : TEXCOORD0; 
};

Texture2D FrameBufferTexture : register(t0);
SamplerState Sampler : register(s0);

static const float Gamma = 2.2f;
static const float Exposure = 1.0f;

float4 main(PSInput input) : SV_TARGET0
{
    float3 Color = FrameBufferTexture.Sample(Sampler, input.TexCoords).rgb;
    float3 result = Color;
   
    return float4(result, 1.0f);
}