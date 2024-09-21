#include "DLLShader.h"

VkShaderModule DLL_Shader_BuildGLSLShaderFile(const char* path)
{
	return Shader_BuildGLSLShaderFile(path);
 }

VkPipelineShaderStageCreateInfo DLL_Shader_CreateShader(VkShaderModule shaderModule, VkShaderStageFlagBits shaderStages)
{
	return Shader_CreateShader(shaderModule, shaderStages);
}