#include <vulkan/vulkan.h>
#include "CFile.h"
#include "Macro.h"
#include "CVulkanRenderer.h"

#ifdef __cplusplus
extern "C" {
#endif

	DLL_EXPORT VkShaderModule Shader_BuildGLSLShaderFile(VkDevice device, const char* path);
	DLL_EXPORT VkPipelineShaderStageCreateInfo Shader_CreateShader(VkShaderModule shaderModule, VkShaderStageFlagBits shaderStages);

#ifdef __cplusplus
}
#endif