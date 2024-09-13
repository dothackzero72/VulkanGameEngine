#include "pch.h"
#include "CppUnitTest.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace GameEngineUnitTests
{
	TEST_CLASS(GameEngineUnitTests)
	{
	public:
		
		TEST_METHOD(DLL_Renderer_CreateVulkanInstance)
		{
			DLL_Renderer_CreateVulkanInstance();
		}
	};
}
