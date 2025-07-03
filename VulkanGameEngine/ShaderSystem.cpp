#include "ShaderSystem.h"
#include "RenderSystem.h"

ShaderSystem shaderSystem = ShaderSystem();


ShaderSystem::ShaderSystem()
{
}

ShaderSystem::~ShaderSystem()
{
}

void ShaderSystem::StartUp()
{
    Shader_StartUp();
}

VkPipelineShaderStageCreateInfo ShaderSystem::CreateShader(VkDevice device, const String& filename, VkShaderStageFlagBits shaderStages)
{
    return Shader_CreateShader(device, filename, shaderStages);
}