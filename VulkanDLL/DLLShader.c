#include "DLLShader.h"

VkShaderModule DLL_Shader_BuildGLSLShaderFile(VkDevice device, const char* path)
{
	return Shader_BuildGLSLShaderFile(device, path);
 }

VkPipelineShaderStageCreateInfo DLL_Shader_CreateShader(VkShaderModule shaderModule, VkShaderStageFlagBits shaderStages)
{
	return Shader_CreateShader(shaderModule, shaderStages);
}