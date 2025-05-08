// Struct for generated vertex data (position and UV)
struct Vertex2D
{
    float2 Position; // 2D position in local space
    float2 UV; // Texture coordinates
};

// Input struct for per-instance data
struct VSInput
{
    float2 SpritePosition : TEXCOORD0; // Base position of the sprite
    float4 UVOffset : TEXCOORD1; // UV offsets (x: startU, y: startV, z: endU, w: endV)
    float2 SpriteSize : TEXCOORD2; // Width and height of the sprite
    int2 FlipSprite : TEXCOORD3; // Flip flags (x, y)
    float4 Color : TEXCOORD4; // Color tint
    float4 InstanceTransform0 : TEXCOORD5; // Row 0 of the 4x4 instance transform matrix
    float4 InstanceTransform1 : TEXCOORD6; // Row 1
    float4 InstanceTransform2 : TEXCOORD7; // Row 2
    float4 InstanceTransform3 : TEXCOORD8; // Row 3
    uint MaterialID : TEXCOORD9; // Material index
};

// Output struct to the pixel shader
struct VSOutput
{
    float4 Position : SV_POSITION; // Clip-space position (required by Vulkan)
    float3 PS_Position : TEXCOORD0; // World-space position
    float2 PS_UV : TEXCOORD1; // UV coordinates
    float2 PS_SpriteSize : TEXCOORD2; // Sprite size
    int2 PS_FlipSprite : TEXCOORD3; // Flip flags
    float4 PS_Color : TEXCOORD4; // Color tint
    uint PS_MaterialID : TEXCOORD5; // Material index
    float4 PS_UVOffset : TEXCOORD6; // UV offsets
};

struct SceneDataBuffer
{
    int MeshBufferIndex; // Unused in fragment shader
    float4x4 Projection; // Projection matrix (unused)
    float4x4 View; // View matrix (unused)
    float3 CameraPosition; // Camera position (unused)
};

// Constant buffer replacing push constants
[[vk::push_constant]] SceneDataBuffer sceneData;

// Main vertex shader function
VSOutput main(VSInput input, uint vertexID : SV_VertexID)
{
    VSOutput output;
    Vertex2D vertex;

    // Generate quad vertices and UVs based on vertexID (0-3)
    switch (vertexID)
    {
        case 0: // Top-left
            vertex.Position = float2(input.SpritePosition.x, input.SpritePosition.y + input.SpriteSize.y);
            vertex.UV = float2(input.UVOffset.x, input.UVOffset.y);
            break;
        case 1: // Top-right
            vertex.Position = float2(input.SpritePosition.x + input.SpriteSize.x, input.SpritePosition.y + input.SpriteSize.y);
            vertex.UV = float2(input.UVOffset.x + input.UVOffset.z, input.UVOffset.y);
            break;
        case 2: // Bottom-right
            vertex.Position = float2(input.SpritePosition.x + input.SpriteSize.x, input.SpritePosition.y);
            vertex.UV = float2(input.UVOffset.x + input.UVOffset.z, input.UVOffset.y + input.UVOffset.w);
            break;
        case 3: // Bottom-left
            vertex.Position = float2(input.SpritePosition.x, input.SpritePosition.y);
            vertex.UV = float2(input.UVOffset.x, input.UVOffset.y + input.UVOffset.w);
            break;
    }

    // Reconstruct the instance transform matrix from the input rows
    float4x4 InstanceTransform = float4x4(
        input.InstanceTransform0,
        input.InstanceTransform1,
        input.InstanceTransform2,
        input.InstanceTransform3
    );

    // Transform the vertex position to world space
    float4 localPosition = float4(vertex.Position, 0.0, 1.0); // 2D position with z=0, w=1
    float4 worldPosition = mul(InstanceTransform, localPosition);

    // Pass data to the pixel shader
    output.PS_Position = worldPosition.xyz; // World-space position
    output.PS_UV = vertex.UV; // UV coordinates
    output.PS_SpriteSize = input.SpriteSize; // Sprite dimensions
    output.PS_FlipSprite = input.FlipSprite; // Flip flags
    output.PS_Color = input.Color; // Color tint
    output.PS_MaterialID = input.MaterialID; // Material index
    output.PS_UVOffset = input.UVOffset; // UV offsets

    // Transform to clip space for Vulkan
    float4 viewPosition = mul(sceneData.View, worldPosition);
    output.Position = mul(Projection, viewPosition);

    return output;
}