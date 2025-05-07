struct VSOutput
{
    float2 fragTexCoord : TEXCOORD0;
    float4 position : SV_POSITION;
};

VSOutput main(uint vertexID : SV_VertexID)
{
    VSOutput output;
    
    float2 fragTexCoord = float2(((vertexID << 1) & 2), (vertexID & 2));
    output.fragTexCoord = fragTexCoord;
    
    float2 position = fragTexCoord * 2.0f - 1.0f;
    output.position = float4(position, 0.0f, 1.0f);

    return output;
}