#pragma once
extern "C"
{
#include <VulkanRenderer.h>
	#include <CShaderCompiler.h>
	#include <io.h>
}
#include <vulkan/vulkan.h>
#include <string>
#include <CShaderCompiler.h>
#include <Windows.h>
#include <dxcapi.h>
#include <wrl/client.h>

class ShaderCompiler
{
private:
	//static Microsoft::WRL::ComPtr<IDxcUtils> dxc_utils;
	//static Microsoft::WRL::ComPtr<IDxcCompiler3> dxc_compiler;
	//static Microsoft::WRL::ComPtr<IDxcIncludeHandler> DefaultIncludeHandler;

	//static std::string ConvertLPCWSTRToString(LPCWSTR lpcwszStr);
	//static void uint32ToUnsignedCharString(uint32 value, std::string& string);
	//static VkShaderModule CompileHLSLShader(const std::string& path, VkShaderStageFlagBits stage);
	static VkShaderModule BuildGLSLShader(const char* path);
	//static Microsoft::WRL::ComPtr<IDxcBlob> BuildHLSLShader(const std::string& path, VkShaderStageFlagBits stage);
public:
	static void SetUpCompiler();
	static VkPipelineShaderStageCreateInfo CreateShader(const std::string& path, VkShaderStageFlagBits shaderStages);
};

