struct VSInput
{
    float2 SpritePosition : TEXCOORD0;
    float4 UVOffset : TEXCOORD1;
};

struct VSOutput
{
    float4 WorldPos : SV_POSITION;
    float3 PS_Position : TEXCOORD0;
    float2 PS_UV : TEXCOORD1;
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

struct Vertex2D
{
    float2 Position;
    float2 UV;
};

[[vk::push_constant]] SceneDataBuffer sceneData;
StructuredBuffer<MeshProperitiesBuffer> meshBuffer : register(t0);
StructuredBuffer<MaterialProperitiesBuffer> materialBuffer : register(t2);
Texture2D TextureMap[] : register(t1);
SamplerState Sampler : register(s0);

VSOutput main(VSInput input, uint vertexID : SV_VertexID)
{
    VSOutput output;
    Vertex2D vertex;

    float4 localPos = float4(vertex.Position, 0.0f, 1.0f);
    float4 worldPos = mul(meshBuffer[sceneData.MeshBufferIndex].MeshTransform, localPos);
    float4 viewPos = mul(sceneData.View, worldPos);
    float4 clipPos = mul(sceneData.Projection, viewPos);
    
    output.WorldPos = clipPos;
    output.PS_Position = worldPos.xyz;
    output.PS_UV = vertex.UV;
    
    return output;
}