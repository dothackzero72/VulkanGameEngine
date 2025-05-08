#pragma once
extern "C"
{
#include <CShaderCompiler.h>
}
#include <Windows.h>
#include <dxcapi.h>
#include <wrl/client.h>
#include <vector>
#include <string>
#include <iostream>
#include <fstream>
#include <vulkan/vulkan.h>
#include <fstream>
#include "VulkanRenderer.h"
#include "TypeDef.h"
#include "File.h"


class ShaderSystem
{
private:
	Microsoft::WRL::ComPtr<IDxcUtils> dxc_utils;
	Microsoft::WRL::ComPtr<IDxcCompiler3> dxc_compiler;
	Microsoft::WRL::ComPtr<IDxcIncludeHandler> DefaultIncludeHandler;

	String ConvertLPCWSTRToString(LPCWSTR lpcwszStr);
	void uint32ToUnsignedCharString(uint32 value, String& string);
	VkShaderModule BuildHLSLShader(const String& path, VkShaderStageFlagBits stage);
	VkShaderModule BuildGLSLShader(VkDevice device, const char* path);
	Microsoft::WRL::ComPtr<IDxcBlob> CompileHLSLShader(const String& path, VkShaderStageFlagBits stage);

public:
	ShaderSystem();
	~ShaderSystem();

	void StartUp();
	VkPipelineShaderStageCreateInfo CreateShader(VkDevice device, const String& path, VkShaderStageFlagBits shaderStages);
};
extern ShaderSystem shaderSystem;

