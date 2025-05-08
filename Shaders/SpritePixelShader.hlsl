struct PSInput
{
    float3 PS_Position : TEXCOORD0; 
    float2 PS_UV : TEXCOORD1;
    float2 PS_SpriteSize : TEXCOORD2;
    nointerpolation int2 PS_FlipSprite : TEXCOORD3; 
    float4 PS_Color : TEXCOORD4; 
    nointerpolation uint PS_MaterialID : TEXCOORD5; 
    nointerpolation float4 PS_UVOffset : TEXCOORD6;
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

[[vk::push_constant]] SceneDataBuffer sceneDataBuffer;

struct MeshProperitiesBuffer
{
    int MaterialIndex;
    float4x4 MeshTransform;
};

StructuredBuffer<MeshProperitiesBuffer> meshBuffer : register(t0);

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

Texture2D TextureMap[] : register(t1);
SamplerState Sampler : register(s0);

PSOutput main(PSInput input)
{
    PSOutput output;
    MaterialProperitiesBuffer material = materialBuffer[input.PS_MaterialID];
   
    float2 UV = input.PS_UV;
    if (input.PS_FlipSprite.x == 1)
    {
        UV.x = UV.x + (input.PS_UVOffset.z) - (UV.x - input.PS_UVOffset.x);
    }
    if (input.PS_FlipSprite.y == 1)
    {
        UV.y = UV.y + (input.PS_UVOffset.w) - (UV.y - input.PS_UVOffset.y);
    }
    
    float4 albedoColor = TextureMap[material.AlbedoMap].Sample(Sampler, UV);
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