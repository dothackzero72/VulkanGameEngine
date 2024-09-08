#include "CShaderCompiler.h"
#include "VulkanRenderer.h"

VkPipelineShaderStageCreateInfo Shader_CreateShader(VkShaderModule shaderModule, VkShaderStageFlagBits shaderStages)
{
    VkPipelineShaderStageCreateInfo pipelineShaderStageCreateInfo =
    {
        .sType = VK_STRUCTURE_TYPE_PIPELINE_SHADER_STAGE_CREATE_INFO,
        .stage = shaderStages,
        .module = shaderModule,
        .pName = "main"
    };

    return pipelineShaderStageCreateInfo;
}

VkShaderModule Shader_BuildGLSLShaderFile(const char* path)
{
    FileState file = File_Read(path);

    VkShaderModuleCreateInfo shaderModuleCreateInfo =
    {
        .sType = VK_STRUCTURE_TYPE_SHADER_MODULE_CREATE_INFO,
        .codeSize = file.Size,
        .pCode = (const uint32*)file.Data
    };

    VkShaderModule shaderModule = VK_NULL_HANDLE;
    VULKAN_RESULT(vkCreateShaderModule(renderer.Device, &shaderModuleCreateInfo, NULL, &shaderModule));

    return shaderModule;
}