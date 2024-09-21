#pragma once
#include "DLL.h"
#include <CShaderCompiler.h>

	DLL_EXPORT VkShaderModule DLL_Shader_BuildGLSLShaderFile(const char* path);
	DLL_EXPORT VkPipelineShaderStageCreateInfo DLL_Shader_CreateShader(VkShaderModule shaderModule, VkShaderStageFlagBits shaderStages);
