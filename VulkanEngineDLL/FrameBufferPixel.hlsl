// Define the input struct for the pixel shader
struct PSInput
{
    float4 Position : SV_POSITION; // Clip-space position (required by Vulkan)
    float2 TexCoords : TEXCOORD0; // Texture coordinates
};

// Texture and sampler
Texture2D FrameBufferTexture : register(t0);
SamplerState Sampler : register(s0);

// Constants
static const float Gamma = 2.2f;
static const float Exposure = 1.0f;

// Main pixel shader function
float4 main(PSInput input) : SV_TARGET0
{
    // Sample the texture
    float3 Color = FrameBufferTexture.Sample(Sampler, input.TexCoords).rgb;

    // Apply result (no modifications in this case)
    float3 result = Color;

    // Output the final color
    return float4(result, 1.0f);
}