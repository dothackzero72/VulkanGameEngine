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
    DxcCreateInstance(CLSID_DxcUtils, IID_PPV_ARGS(dxc_utils.ReleaseAndGetAddressOf()));
    DxcCreateInstance(CLSID_DxcCompiler, IID_PPV_ARGS(&dxc_compiler));
    dxc_utils->CreateDefaultIncludeHandler(&DefaultIncludeHandler);
}

VkPipelineShaderStageCreateInfo ShaderSystem::CreateShader(VkDevice device, const String& filename, VkShaderStageFlagBits shaderStages)
{
    VkShaderModule shaderModule = VK_NULL_HANDLE;
    if (fileSystem.GetFileExtention(filename) == "hlsl")
    {
        shaderModule = BuildHLSLShader(filename.c_str(), shaderStages);
    }
    else
    {
        shaderModule = BuildGLSLShader(device, filename.c_str());
    }
    return Shader_CreateShader(shaderModule, shaderStages);
}


String ShaderSystem::ConvertLPCWSTRToString(LPCWSTR lpcwszStr)
{
    int strLength = WideCharToMultiByte(CP_UTF8, 0, lpcwszStr, -1, nullptr, 0, nullptr, nullptr);
    String str(strLength, 0);
    WideCharToMultiByte(CP_UTF8, 0, lpcwszStr, -1, &str[0], strLength, nullptr, nullptr);
    return str;
}

void ShaderSystem::uint32ToUnsignedCharString(uint32 value, String& string)
{
    string += static_cast<unsigned char>((value >> 24) & 0xFF);
    string += static_cast<unsigned char>((value >> 16) & 0xFF);
    string += static_cast<unsigned char>((value >> 8) & 0xFF);
    string += static_cast<unsigned char>(value & 0xFF);
}

VkShaderModule ShaderSystem::BuildGLSLShader(VkDevice device, const char* path)
{
    return Shader_BuildGLSLShaderFile(device, path);
}

VkShaderModule ShaderSystem::BuildHLSLShader(const String& filePath, VkShaderStageFlagBits stage)
{
    const String& filePathNoExt = fileSystem.RemoveFileExtention(filePath);
    if (fileSystem.FileExists(filePath))
    {
        time_t shaderCodeLastModifiedTime = fileSystem.LastModifiedTime(filePath);
        time_t compiledCodeLastModifiedTime = fileSystem.LastModifiedTime(filePathNoExt + ".spv");
        if (shaderCodeLastModifiedTime > compiledCodeLastModifiedTime)
        {
            Microsoft::WRL::ComPtr<IDxcBlob> spriv_buffer = CompileHLSLShader(filePath, stage);

            VkShaderModule shaderModule = VK_NULL_HANDLE;
            VkShaderModuleCreateInfo ShaderModuleCreateInfo = VkShaderModuleCreateInfo
            {
                .sType = VK_STRUCTURE_TYPE_SHADER_MODULE_CREATE_INFO,
                .codeSize = spriv_buffer->GetBufferSize(),
                .pCode = (uint32*)spriv_buffer->GetBufferPointer(),
            };
            VULKAN_RESULT(vkCreateShaderModule(renderSystem.renderer.Device, &ShaderModuleCreateInfo, nullptr, &shaderModule));

            return shaderModule;
        }
        else
        {
            std::ifstream file(filePath, std::ios::ate | std::ios::binary);

            if (!file.is_open()) {
                throw std::runtime_error("Failed to open file: " + filePath);
            }

            size_t fileSize = (size_t)file.tellg();
            std::vector<char> buffer(fileSize);

            file.seekg(0);
            file.read(buffer.data(), fileSize);

            file.close();

            VkShaderModuleCreateInfo createInfo = VkShaderModuleCreateInfo
            {
                .sType = VK_STRUCTURE_TYPE_SHADER_MODULE_CREATE_INFO,
                .codeSize = buffer.size(),
                .pCode = reinterpret_cast<const uint32_t*>(buffer.data()),
            };

            VkShaderModule shaderModule = VK_NULL_HANDLE;
            VULKAN_RESULT(vkCreateShaderModule(renderSystem.renderer.Device, &createInfo, nullptr, &shaderModule));

            return shaderModule;
        }
    }
    else
    {
        Microsoft::WRL::ComPtr<IDxcBlob> spriv_buffer = CompileHLSLShader(filePath, stage);

        VkShaderModule shaderModule = VK_NULL_HANDLE;
        VkShaderModuleCreateInfo ShaderModuleCreateInfo = VkShaderModuleCreateInfo
        {
            .sType = VK_STRUCTURE_TYPE_SHADER_MODULE_CREATE_INFO,
            .codeSize = spriv_buffer->GetBufferSize(),
            .pCode = (uint32*)spriv_buffer->GetBufferPointer(),
        };
        VULKAN_RESULT(vkCreateShaderModule(renderSystem.renderer.Device, &ShaderModuleCreateInfo, nullptr, &shaderModule));

        return shaderModule;
    }
    return VK_NULL_HANDLE;
}

Microsoft::WRL::ComPtr<IDxcBlob> ShaderSystem::CompileHLSLShader(const String& filename, VkShaderStageFlagBits stage)
{
    const char* cShaderCode = fileSystem.ReadFile(filename.c_str());
    if (!cShaderCode) {
        std::cerr << "Failed to read file: " << filename << std::endl;
        return nullptr;
    }
    String shaderCode(cShaderCode);

    if (shaderCode.size() >= 3 &&
        static_cast<unsigned char>(shaderCode[0]) == 0xEF &&
        static_cast<unsigned char>(shaderCode[1]) == 0xBB &&
        static_cast<unsigned char>(shaderCode[2]) == 0xBF) {
        shaderCode = shaderCode.substr(3);
    }

    DxcBuffer src_buffer = {
        .Ptr = shaderCode.c_str(),
        .Size = static_cast<uint32_t>(shaderCode.size()),
        .Encoding = 0
    };

    std::vector<LPCWSTR> args;
    args.emplace_back(L"-spirv");
    args.emplace_back(L"-fspv-target-env=vulkan1.3");
    switch (stage) {
    case VK_SHADER_STAGE_VERTEX_BIT: args.emplace_back(L"-T vs_6_5"); break;
    case VK_SHADER_STAGE_FRAGMENT_BIT: args.emplace_back(L"-T ps_6_5"); break;
    case VK_SHADER_STAGE_COMPUTE_BIT: args.emplace_back(L"-T cs_6_5"); break;
    default: args.emplace_back(L"-T lib_6_5"); break;
    }

    Microsoft::WRL::ComPtr<IDxcResult> result;
    dxc_compiler->Compile(&src_buffer, args.data(), static_cast<uint32_t>(args.size()),
        DefaultIncludeHandler.Get(), IID_PPV_ARGS(&result));

    Microsoft::WRL::ComPtr<IDxcBlob> shader_obj;
    result->GetOutput(DXC_OUT_OBJECT, IID_PPV_ARGS(&shader_obj), nullptr);

    Microsoft::WRL::ComPtr<IDxcBlobUtf8> error_message;
    result->GetOutput(DXC_OUT_ERRORS, IID_PPV_ARGS(&error_message), nullptr);
    if (error_message && error_message->GetStringLength() > 0) {
        std::cout << "Compiler error: " << error_message->GetStringPointer() << std::endl;
        std::cout << "Error in file: " << filename << std::endl;
    }

    if (shader_obj) {
        String spvFileName = String(File_GetFileNameFromPath(filename.c_str())) + ".spv";
        std::ofstream spvFile(spvFileName, std::ios::binary);
        spvFile.write(static_cast<const char*>(shader_obj->GetBufferPointer()), shader_obj->GetBufferSize());
        spvFile.close();
    }

    return shader_obj;
}
