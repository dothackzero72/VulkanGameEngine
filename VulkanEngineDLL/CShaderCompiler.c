#include "CShaderCompiler.h"

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

VkShaderModule Shader_BuildGLSLShaderFile(VkDevice device, const char* path)
{
    FileState file = File_Read(path);

    VkShaderModuleCreateInfo shaderModuleCreateInfo =
    {
        .sType = VK_STRUCTURE_TYPE_SHADER_MODULE_CREATE_INFO,
        .codeSize = file.Size,
        .pCode = (const uint32*)file.Data
    };

    VkShaderModule shaderModule = VK_NULL_HANDLE;
    VULKAN_RESULT(vkCreateShaderModule(device, &shaderModuleCreateInfo, NULL, &shaderModule));

    return shaderModule;
}

bool Shader_BuildGLSLShaders(const char* command)
{
    STARTUPINFO startUpInfo = { sizeof(startUpInfo) };
    PROCESS_INFORMATION processInfo;
    if (CreateProcess(NULL, (LPSTR)command, NULL, NULL, FALSE, 0, NULL, NULL, &startUpInfo, &processInfo)) {
        WaitForSingleObject(processInfo.hProcess, INFINITE);

        DWORD exitCode;
        GetExitCodeProcess(processInfo.hProcess, &exitCode);
        CloseHandle(processInfo.hProcess);
        CloseHandle(processInfo.hThread);
        return exitCode == 0;
    }
    return false;
}
