#version 460
#extension GL_ARB_separate_shader_objects : enable
#extension GL_EXT_nonuniform_qualifier : enable
#extension GL_EXT_scalar_block_layout : enable
#extension GL_EXT_debug_printf : enable

layout (location = 0) in vec2 inPosition;
layout (location = 1) in vec2 aUV;
layout (location = 2) in vec3 aColor;

layout(location = 0) out vec2 FragPos;
layout(location = 1) out vec2 UV;
layout(location = 2) out vec3 Color;

layout(push_constant) uniform SceneDataBuffer
{
	mat4 Projection;
	mat4 View;
	vec3 CameraPosition;
}sceneData;

struct MeshProperitiesBuffer
{
	int	   MeshIndex;
	int	   MaterialIndex;
	mat4   MeshTransform;
};

layout(binding = 0) readonly buffer MeshProperities { MeshProperitiesBuffer meshProperties; } meshBuffer;
layout(binding = 1) uniform sampler2D TextureMap;

void main() 
{
    mat4 MeshTransform = meshBuffer.meshProperties.MeshTransform;
    FragPos = vec2(inPosition.xy);    
    Color = aColor;
    UV = aUV;
    gl_Position = sceneData.Projection * 
                  sceneData.View *   
                  vec4(inPosition, 0.0f, 1.0f);
}