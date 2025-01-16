#version 460
#extension GL_ARB_separate_shader_objects : enable
#extension GL_EXT_nonuniform_qualifier : enable
#extension GL_EXT_scalar_block_layout : enable
#extension GL_EXT_debug_printf : enable

layout (location = 0) in vec2 inPosition;
layout (location = 1) in vec2 aUV;
layout (location = 2) in vec3 aColor;
//layout (location = 3) in uint aMaterialID;

layout(location = 0) out vec2 FragPos;
layout(location = 1) out vec2 UV;
layout(location = 2) out vec3 Color;
//layout(location = 3) out uint MaterialID;

layout(push_constant) uniform SceneDataBuffer
{
    int	 MeshBufferIndex;
	mat4 Projection;
	mat4 View;
	vec3 CameraPosition;
}sceneData;

struct MeshProperitiesBuffer
{
	int	   MaterialIndex;
	mat4   MeshTransform;
};

struct MaterialProperitiesBuffer
{
	vec3 Albedo;
	float Metallic;
	float Roughness;
	float AmbientOcclusion;
	vec3 Emission;
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


layout(binding = 0) readonly buffer MeshProperities { MeshProperitiesBuffer meshProperties; } meshBuffer[];
layout(binding = 1) uniform sampler2D TextureMap[];
layout(binding = 2) buffer MaterialProperities { MaterialProperitiesBuffer materialProperties; } materialBuffer[];

void main() 
{
    const int meshIndex = sceneData.MeshBufferIndex;

    mat4 meshTransform = meshBuffer[meshIndex].meshProperties.MeshTransform;
    FragPos = vec2(inPosition.xy);    
    Color = aColor;
    UV = aUV;
	//MaterialID = aMaterialID;

    gl_Position = sceneData.Projection * 
                  sceneData.View *  
                  meshTransform *
                  vec4(inPosition, 0.0f, 1.0f);
}