//#include "HLSLShaderCompiler.h"
//#include <algorithm>
//#include <io.c>
//
//Microsoft::WRL::ComPtr<IDxcUtils> HLSLShaderCompiler::dxc_utils;
//Microsoft::WRL::ComPtr<IDxcCompiler3> HLSLShaderCompiler::dxc_compiler;
//Microsoft::WRL::ComPtr<IDxcIncludeHandler> HLSLShaderCompiler::DefaultIncludeHandler;
//
//std::string HLSLShaderCompiler::ConvertLPCWSTRToString(LPCWSTR lpcwszStr)
//{
//    int strLength = WideCharToMultiByte(CP_UTF8, 0, lpcwszStr, -1, nullptr, 0, nullptr, nullptr);
//    std::string str(strLength, 0);
//    WideCharToMultiByte(CP_UTF8, 0, lpcwszStr, -1, &str[0], strLength, nullptr, nullptr);
//    return str;
//}
//
//void HLSLShaderCompiler::uint32ToUnsignedCharString(uint32_t value, std::string& string)
//{
//    string += static_cast<unsigned char>((value >> 24) & 0xFF);
//    string += static_cast<unsigned char>((value >> 16) & 0xFF);
//    string += static_cast<unsigned char>((value >> 8) & 0xFF);
//    string += static_cast<unsigned char>(value & 0xFF);
//}
//
//void HLSLShaderCompiler::SetUpCompiler()
//{
//    DxcCreateInstance(CLSID_DxcUtils, IID_PPV_ARGS(dxc_utils.ReleaseAndGetAddressOf()));
//    DxcCreateInstance(CLSID_DxcCompiler, IID_PPV_ARGS(&dxc_compiler));
//    dxc_utils->CreateDefaultIncludeHandler(&DefaultIncludeHandler);
//}
//
//Microsoft::WRL::ComPtr<IDxcBlob> HLSLShaderCompiler::BuildShader(const std::string filename, VkShaderStageFlagBits stage)
//{
//    FileState cShaderCode = File_Read(filename.c_str());
//    std::string shaderCode(cShaderCode.Data);
//
//    Microsoft::WRL::ComPtr<IDxcUtils> dxc_utils{};
//    Microsoft::WRL::ComPtr<IDxcCompiler3> dxc_compiler{};
//    Microsoft::WRL::ComPtr<IDxcIncludeHandler> DefaultIncludeHandler;
//
//    DxcCreateInstance(CLSID_DxcUtils, IID_PPV_ARGS(dxc_utils.ReleaseAndGetAddressOf()));
//    DxcCreateInstance(CLSID_DxcCompiler, IID_PPV_ARGS(&dxc_compiler));
//    dxc_utils->CreateDefaultIncludeHandler(&DefaultIncludeHandler);
//
//    DxcBuffer src_buffer = {
//        .Ptr = shaderCode.c_str(),
//        .Size = static_cast<uint32_t>(shaderCode.size()),
//        .Encoding = 0
//    };
//
//    std::vector<LPCWSTR> args;
//    args.emplace_back(L"-spirv");
//    args.emplace_back(L"-fspv-target-env=vulkan1.3");
//    switch (stage)
//    {
//        case VkShaderStageFlagBits::VK_SHADER_STAGE_VERTEX_BIT: args.emplace_back(L"-T vs_6_5"); break;
//        case VkShaderStageFlagBits::VK_SHADER_STAGE_FRAGMENT_BIT: args.emplace_back(L"-T ps_6_5"); break;
//        case VkShaderStageFlagBits::VK_SHADER_STAGE_COMPUTE_BIT: args.emplace_back(L"-T cs_6_5"); break;
//        default: args.emplace_back(L"-T lib_6_5"); break;
//    }
//
//    for (int x = 0; x < args.size(); x++)
//    {
//        std::cout << ConvertLPCWSTRToString(args[x]) << std::endl;
//    }
//
//    Microsoft::WRL::ComPtr<IDxcResult> result;
//    dxc_compiler->Compile(&src_buffer, args.data(), static_cast<uint32_t>(args.size()), DefaultIncludeHandler.Get(), IID_PPV_ARGS(&result));
//
//    Microsoft::WRL::ComPtr<IDxcBlob> shader_obj;
//    result->GetOutput(DXC_OUT_OBJECT, IID_PPV_ARGS(&shader_obj), nullptr);
//
//    Microsoft::WRL::ComPtr<IDxcBlobUtf8> error_message;
//    result->GetOutput(DXC_OUT_ERRORS, IID_PPV_ARGS(&error_message), nullptr);
//    if (error_message && error_message->GetStringLength() > 0)
//    {
//        auto string = std::string();
//        string.resize(error_message->GetBufferSize());
//        for (size_t x = 0; x < string.size(); x++)
//        {
//            string[x] = static_cast<const char*>(error_message->GetBufferPointer())[x];
//        }
//
//        std::cout << string << std::endl;
//        std::cout << "Error found in: " + filename << std::endl;
//        Renderer_DestroyRenderer();
//        //GameEngine_DestroyWindow();
//    }
//
//    std::string outPutFileName = std::string(File_GetFileNameFromPath(filename.c_str())) + ".spv";
//    File_Write(static_cast<void*>(&outPutFileName), shaderCode.size(), filename.c_str());
//
//    return shader_obj;
//}
