#include <vulkan/vulkan.h>
#include "io.h"
#include "Macro.h"

#ifdef __cplusplus
extern "C" {
#endif

VkShaderModule Shader_BuildGLSLShaderFile(const char* path);
VkPipelineShaderStageCreateInfo Shader_CreateShader(VkShaderModule shaderModule, VkShaderStageFlagBits shaderStages);

#ifdef __cplusplus
}
#endif