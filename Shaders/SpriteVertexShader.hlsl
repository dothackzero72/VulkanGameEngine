struct VSInput
{
    float2 SpritePosition : TEXCOORD0;
    float4 UVOffset : TEXCOORD1; 
    float2 SpriteSize : TEXCOORD2; 
    int2 FlipSprite : TEXCOORD3; 
    float4 Color : TEXCOORD4; 
    float4 InstanceTransform0 : TEXCOORD5;
    float4 InstanceTransform1 : TEXCOORD6;
    float4 InstanceTransform2 : TEXCOORD7; 
    float4 InstanceTransform3 : TEXCOORD8; 
    uint MaterialID : TEXCOORD9; 
};

struct VSOutput
{
    float4 Position : SV_POSITION;
    float3 PS_Position : TEXCOORD0; 
    float2 PS_UV : TEXCOORD1; 
    float2 PS_SpriteSize : TEXCOORD2; 
    int2 PS_FlipSprite : TEXCOORD3; 
    float4 PS_Color : TEXCOORD4;
    uint PS_MaterialID : TEXCOORD5; 
    float4 PS_UVOffset : TEXCOORD6;
};

struct Vertex2D
{
    float2 Position;
    float2 UV;
};

struct SceneDataBuffer
{
    int MeshBufferIndex;
    float4x4 Projection;
    float4x4 View;
    float3 CameraPosition;
};

[[vk::push_constant]] SceneDataBuffer sceneData;

VSOutput main(VSInput input, uint vertexID : SV_VertexID)
{
    VSOutput output;
    Vertex2D vertex;
    
    switch (vertexID)
    {
        case 0:
            vertex.Position = float2(input.SpritePosition.x, input.SpritePosition.y + input.SpriteSize.y);
            vertex.UV = float2(input.UVOffset.x, input.UVOffset.y);
            break;
        case 1: 
            vertex.Position = float2(input.SpritePosition.x + input.SpriteSize.x, input.SpritePosition.y + input.SpriteSize.y);
            vertex.UV = float2(input.UVOffset.x + input.UVOffset.z, input.UVOffset.y);
            break;
        case 2: 
            vertex.Position = float2(input.SpritePosition.x + input.SpriteSize.x, input.SpritePosition.y);
            vertex.UV = float2(input.UVOffset.x + input.UVOffset.z, input.UVOffset.y + input.UVOffset.w);
            break;
        case 3:
            vertex.Position = float2(input.SpritePosition.x, input.SpritePosition.y);
            vertex.UV = float2(input.UVOffset.x, input.UVOffset.y + input.UVOffset.w);
            break;
    }
    
    float4x4 InstanceTransform = float4x4(
        input.InstanceTransform0,
        input.InstanceTransform1,
        input.InstanceTransform2,
        input.InstanceTransform3
    );
    
    float4 localPosition = float4(vertex.Position, 0.0, 1.0);
    float4 worldPosition = mul(InstanceTransform, localPosition);
    
    output.PS_Position = worldPosition.xyz;
    output.PS_UV = vertex.UV;
    output.PS_SpriteSize = input.SpriteSize; 
    output.PS_FlipSprite = input.FlipSprite; 
    output.PS_Color = input.Color; 
    output.PS_MaterialID = input.MaterialID; 
    output.PS_UVOffset = input.UVOffset; 
    
    float4 viewPosition = mul(sceneData.View, worldPosition);
    output.Position = mul(sceneData.Projection, viewPosition);

    return output;
}