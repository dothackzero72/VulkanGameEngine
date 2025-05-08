// Input struct from vertex shader
struct PSInput
{
    float3 PS_Position : TEXCOORD0; // World-space position
    float2 PS_UV : TEXCOORD1; // UV coordinates
    float2 PS_SpriteSize : TEXCOORD2; // Sprite size
    nointerpolation int2 PS_FlipSprite : TEXCOORD3; // Flip flags
    float4 PS_Color : TEXCOORD4; // Color tint
    nointerpolation uint PS_MaterialID : TEXCOORD5; // Material index
    nointerpolation float4 PS_UVOffset : TEXCOORD6; // UV offsets
};

// Output struct to render target
struct PSOutput
{
    float4 OutputColor : SV_TARGET0; // Final color output
};

struct SceneDataBuffer
{
int MeshBufferIndex; // Unused in fragment shader
float4x4 Projection; // Projection matrix (unused)
float4x4 View; // View matrix (unused)
float3 CameraPosition; // Camera position (unused)
};

// Constant buffer replacing push constants
[[vk::push_constant]] SceneDataBuffer sceneDataBuffer;

// Mesh properties buffer
struct MeshProperitiesBuffer
{
    int MaterialIndex;
    float4x4 MeshTransform;
};

StructuredBuffer<MeshProperitiesBuffer> meshBuffer : register(t0);

// Material properties buffer
struct MaterialProperitiesBuffer
{
    float3 Albedo;
    float Metallic;
    float Roughness;
    float AmbientOcclusion;
    float3 Emission;
    float Alpha;
    uint AlbedoMap;
    uint MetallicRoughnessMap;
    uint MetallicMap;
    uint RoughnessMap;
    uint AmbientOcclusionMap;
    uint NormalMap;
    uint DepthMap;
    uint AlphaMap;
    uint EmissionMap;
    uint HeightMap;
};

StructuredBuffer<MaterialProperitiesBuffer> materialBuffer : register(t2);

// Texture and sampler
Texture2D TextureMap[] : register(t1);
SamplerState Sampler : register(s0);

// Main pixel shader function
PSOutput main(PSInput input)
{
    PSOutput output;

    // Get material properties
    MaterialProperitiesBuffer material = materialBuffer[input.PS_MaterialID];

    // Adjust UV based on flip flags
    float2 UV = input.PS_UV;
    if (input.PS_FlipSprite.x == 1)
    {
        UV.x = UV.x + (input.PS_UVOffset.z) - (UV.x - input.PS_UVOffset.x);
    }
    if (input.PS_FlipSprite.y == 1)
    {
        UV.y = UV.y + (input.PS_UVOffset.w) - (UV.y - input.PS_UVOffset.y);
    }

    // Sample albedo texture
    float4 albedoColor = TextureMap[material.AlbedoMap].Sample(Sampler, UV);
    material.Albedo = albedoColor.rgb;
    material.Alpha = albedoColor.a;

    // Discard if alpha is zero
    if (material.Alpha == 0.0f)
    {
        discard;
    }

    // Apply gamma correction
    float gamma = 2.2f;
    float3 color = pow(material.Albedo, 1.0f / gamma);
    output.OutputColor = float4(color, material.Alpha);

    return output;
}