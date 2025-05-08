// Define the output struct for the vertex shader
struct VSOutput
{
    float4 Position : SV_POSITION; // Clip-space position
    float2 TexCoord : TEXCOORD0; // Texture coordinates
};

// Main vertex shader function
VSOutput main(uint vertexID : SV_VertexID)
{
    VSOutput output;

    // Calculate texture coordinates
    output.TexCoord = float2((vertexID << 1) & 2, vertexID & 2);

    // Calculate clip-space position
    output.Position = float4(output.TexCoord * 2.0f - 1.0f, 0.0f, 1.0f);

    return output;
}