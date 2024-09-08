#include <vulkan/vulkan.h>
#include "io.h"
#include "Macro.h"

DLL_EXPORT VkShaderModule Shader_BuildGLSLShaderFile(const char* path);
DLL_EXPORT VkPipelineShaderStageCreateInfo Shader_CreateShader(VkShaderModule shaderModule, VkShaderStageFlagBits shaderStages);