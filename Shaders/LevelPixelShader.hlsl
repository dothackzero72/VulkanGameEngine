struct PSInput
{
    float3 PS_Position : TEXCOORD0;
    float2 PS_UV : TEXCOORD1;
};

struct PSOutput
{
    float4 OutputColor : SV_TARGET0;
};

struct SceneDataBuffer
{
    int MeshBufferIndex;
    float4x4 Projection;
    float4x4 View;
    float3 CameraPosition;
};

struct MeshProperitiesBuffer
{
    int MaterialIndex;
    float4x4 MeshTransform;
};

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

[[vk::push_constant]] SceneDataBuffer sceneDataBuffer;
StructuredBuffer<MeshProperitiesBuffer> meshBuffer : register(t0);
StructuredBuffer<MaterialProperitiesBuffer> materialBuffer : register(t2);
Texture2D TextureMap[] : register(t1);
SamplerState Sampler : register(s0);

PSOutput main(PSInput input)
{
    PSOutput output;
    MaterialProperitiesBuffer material = materialBuffer[meshBuffer[sceneDataBuffer.MeshBufferIndex].MaterialIndex];
    
    float4 albedoColor = TextureMap[material.AlbedoMap].Sample(Sampler, input.PS_UV);
    material.Albedo = albedoColor.rgb;
    material.Alpha = albedoColor.a;
    
    if (material.Alpha == 0.0f)
    {
        discard;
    }
    
    float gamma = 2.2f;
    float3 color = pow(material.Albedo, 1.0f / gamma);
    output.OutputColor = float4(color, material.Alpha);

    return output;
}