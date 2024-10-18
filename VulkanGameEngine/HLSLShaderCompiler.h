//#include <Windows.h>
//#include <dxcapi.h>
//#include <wrl/client.h>
//#include <vector>
//#include <string>
//#include <iostream>
//#include <fstream>
//#include <vulkan/vulkan.h>
//#include <fstream>
//
//#include <io.h>
//#include <renderer.h>
//
//class HLSLShaderCompiler
//{
//private:
//	static Microsoft::WRL::ComPtr<IDxcUtils> dxc_utils;
//	static Microsoft::WRL::ComPtr<IDxcCompiler3> dxc_compiler;
//	static Microsoft::WRL::ComPtr<IDxcIncludeHandler> DefaultIncludeHandler;
//
//	static String ConvertLPCWSTRToString(LPCWSTR lpcwszStr);
//	static void uint32ToUnsignedCharString(uint32_t value, String& string);
//public:
//	static void SetUpCompiler();
//	static Microsoft::WRL::ComPtr<IDxcBlob> BuildShader(const String filename, VkShaderStageFlagBits stage);
//};
