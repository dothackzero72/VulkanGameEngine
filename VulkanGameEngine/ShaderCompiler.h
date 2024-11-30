#pragma once
extern "C"
{

	#include <CShaderCompiler.h>
	#include <io.h>
}
#include <CoreVulkanRenderer.h>
#include <string>
#include <CShaderCompiler.h>
#include <Windows.h>
#include <dxcapi.h>
#include <wrl/client.h>
#include "Typedef.h"

class ShaderCompiler
{
private:
	//static Microsoft::WRL::ComPtr<IDxcUtils> dxc_utils;
	//static Microsoft::WRL::ComPtr<IDxcCompiler3> dxc_compiler;
	//static Microsoft::WRL::ComPtr<IDxcIncludeHandler> DefaultIncludeHandler;

	//static String ConvertLPCWSTRToString(LPCWSTR lpcwszStr);
	//static void uint32ToUnsignedCharString(uint32 value, String& string);
	//static VkShaderModule CompileHLSLShader(const String& path, VkShaderStageFlagBits stage);
	static VkShaderModule BuildGLSLShader(const char* path);
	//static Microsoft::WRL::ComPtr<IDxcBlob> BuildHLSLShader(const String& path, VkShaderStageFlagBits stage);
public:
	static void SetUpCompiler();
	static VkPipelineShaderStageCreateInfo CreateShader(const String& path, VkShaderStageFlagBits shaderStages);
};

