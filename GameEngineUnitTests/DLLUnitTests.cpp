#include "pch.h"
//#include "CppUnitTest.h"
////#include <gtest/gtest.h>
//#include <iostream>
//#include <tuple>
//#include <vector>
//#include <vulkan/vulkan_core.h>
//#include <wtypes.h>
//#include <Windows.h>
//#include <cstdint>
//
//using uint32 = uint32_t;
//
//class GameEngineUnitTests : public ::testing::Test {
//private:
//    HMODULE hModule = nullptr;
//
//protected:
//    using DLL_Renderer_CreateVulkanInstance = VkInstance(*)();
//    using DLL_Renderer_SetupDebugMessenger = void(*)(VkInstance);
//    using DLL_Renderer_SetUpPhysicalDevice = void(*)(VkInstance, VkPhysicalDevice*);
//    using DLL_Renderer_SetUpDevice = void(*)(VkInstance, VkPhysicalDevice, VkDevice*);
//    using DLL_Renderer_SetUpCommandPool = void(*)(VkDevice, VkCommandPool*);
//    using DLL_Renderer_SetUpSemaphores = void(*)(VkDevice, VkSemaphore*, VkSemaphore*);
//    using DLL_Renderer_GetDeviceQueue = void(*)(VkDevice, uint32_t, uint32_t, VkQueue*);
//    using DLL_Renderer_GetSurfaceFormats = void(*)(VkPhysicalDevice, VkSurfaceKHR, std::vector<VkSurfaceFormatKHR>&);
//    using DLL_Renderer_GetPresentModes = void(*)(VkPhysicalDevice, VkSurfaceKHR, std::vector<VkPresentModeKHR>&);
//    using DLL_Renderer_GetRayTracingSupport = bool(*)();
//    using DLL_Renderer_GetRendererFeatures = void(*)(VkPhysicalDevice, VkPhysicalDeviceFeatures2*);
//    using DLL_Renderer_GetWin32Extensions = void(*)(uint32_t*, VkExtensionProperties*);
//    using DLL_Renderer_StartFrame = void(*)(VkDevice, VkSemaphore, VkSemaphore, uint32_t);
//    using DLL_Renderer_EndFrame = void(*)(VkDevice, VkQueue);
//    using DLL_Renderer_BeginCommandBuffer = void(*)(VkCommandBuffer);
//    using DLL_Renderer_EndCommandBuffer = void(*)(VkCommandBuffer);
//    using DLL_Renderer_SubmitDraw = void(*)(VkQueue, VkCommandBuffer);
//
//    DLL_Renderer_CreateVulkanInstance createVulkanInstance = nullptr;
//    DLL_Renderer_SetupDebugMessenger setupDebugMessenger = nullptr;
//    DLL_Renderer_SetUpPhysicalDevice setUpPhysicalDevice = nullptr;
//    DLL_Renderer_SetUpDevice setUpDevice = nullptr;
//    DLL_Renderer_SetUpCommandPool setUpCommandPool = nullptr;
//    DLL_Renderer_SetUpSemaphores setUpSemaphores = nullptr;
//    DLL_Renderer_GetDeviceQueue getDeviceQueue = nullptr;
//    DLL_Renderer_GetSurfaceFormats getSurfaceFormats = nullptr;
//    DLL_Renderer_GetPresentModes getPresentModes = nullptr;
//    DLL_Renderer_GetRayTracingSupport getRayTracingSupport = nullptr;
//    DLL_Renderer_GetRendererFeatures getRendererFeatures = nullptr;
//    DLL_Renderer_GetWin32Extensions getWin32Extensions = nullptr;
//    DLL_Renderer_StartFrame startFrame = nullptr;
//    DLL_Renderer_EndFrame endFrame = nullptr;
//    DLL_Renderer_BeginCommandBuffer beginCommandBuffer = nullptr;
//    DLL_Renderer_EndCommandBuffer endCommandBuffer = nullptr;
//    DLL_Renderer_SubmitDraw submitDraw = nullptr;
//
//    void SetUp() override {
//        hModule = LoadLibrary(L"../x64/Debug/VulkanDLL.dll");
//        ASSERT_NE(hModule, nullptr);
//
//        createVulkanInstance = (DLL_Renderer_CreateVulkanInstance)GetProcAddress(hModule, "DLL_Renderer_CreateVulkanInstance");
//        setupDebugMessenger = (DLL_Renderer_SetupDebugMessenger)GetProcAddress(hModule, "DLL_Renderer_SetupDebugMessenger");
//        setUpPhysicalDevice = (DLL_Renderer_SetUpPhysicalDevice)GetProcAddress(hModule, "DLL_Renderer_SetUpPhysicalDevice");
//        setUpDevice = (DLL_Renderer_SetUpDevice)GetProcAddress(hModule, "DLL_Renderer_SetUpDevice");
//        setUpCommandPool = (DLL_Renderer_SetUpCommandPool)GetProcAddress(hModule, "DLL_Renderer_SetUpCommandPool");
//        setUpSemaphores = (DLL_Renderer_SetUpSemaphores)GetProcAddress(hModule, "DLL_Renderer_SetUpSemaphores");
//        getDeviceQueue = (DLL_Renderer_GetDeviceQueue)GetProcAddress(hModule, "DLL_Renderer_GetDeviceQueue");
//        getSurfaceFormats = (DLL_Renderer_GetSurfaceFormats)GetProcAddress(hModule, "DLL_Renderer_GetSurfaceFormats");
//        getPresentModes = (DLL_Renderer_GetPresentModes)GetProcAddress(hModule, "DLL_Renderer_GetPresentModes");
//        getRayTracingSupport = (DLL_Renderer_GetRayTracingSupport)GetProcAddress(hModule, "DLL_Renderer_GetRayTracingSupport");
//        getRendererFeatures = (DLL_Renderer_GetRendererFeatures)GetProcAddress(hModule, "DLL_Renderer_GetRendererFeatures");
//        getWin32Extensions = (DLL_Renderer_GetWin32Extensions)GetProcAddress(hModule, "DLL_Renderer_GetWin32Extensions");
//        startFrame = (DLL_Renderer_StartFrame)GetProcAddress(hModule, "DLL_Renderer_StartFrame");
//        endFrame = (DLL_Renderer_EndFrame)GetProcAddress(hModule, "DLL_Renderer_EndFrame");
//        beginCommandBuffer = (DLL_Renderer_BeginCommandBuffer)GetProcAddress(hModule, "DLL_Renderer_BeginCommandBuffer");
//        endCommandBuffer = (DLL_Renderer_EndCommandBuffer)GetProcAddress(hModule, "DLL_Renderer_EndCommandBuffer");
//        submitDraw = (DLL_Renderer_SubmitDraw)GetProcAddress(hModule, "DLL_Renderer_SubmitDraw");
//
//        ASSERT_NE(createVulkanInstance, nullptr);
//        ASSERT_NE(setupDebugMessenger, nullptr);
//        ASSERT_NE(setUpPhysicalDevice, nullptr);
//        ASSERT_NE(setUpDevice, nullptr);
//        ASSERT_NE(setUpCommandPool, nullptr);
//        ASSERT_NE(setUpSemaphores, nullptr);
//        ASSERT_NE(getDeviceQueue, nullptr);
//        ASSERT_NE(getSurfaceFormats, nullptr);
//        ASSERT_NE(getPresentModes, nullptr);
//        ASSERT_NE(getRayTracingSupport, nullptr);
//        ASSERT_NE(getRendererFeatures, nullptr);
//        ASSERT_NE(getWin32Extensions, nullptr);
//        ASSERT_NE(startFrame, nullptr);
//        ASSERT_NE(endFrame, nullptr);
//        ASSERT_NE(beginCommandBuffer, nullptr);
//        ASSERT_NE(endCommandBuffer, nullptr);
//        ASSERT_NE(submitDraw, nullptr);
//    }
//
//    void TearDown() override {
//        if (hModule) {
//            FreeLibrary(hModule);
//            hModule = nullptr;
//        }
//    }
//
//public:
//};
//
//// The actual test cases
//TEST_F(GameEngineUnitTests, CreateVulkanInstance_Test) {
//    VkInstance instance = createVulkanInstance();
//    ASSERT_NE(instance, nullptr) << "Failed to create Vulkan instance.";
//}
//
//TEST_F(GameEngineUnitTests, SetupDebugMessenger_Test) {
//    VkInstance instance = createVulkanInstance();
//    ASSERT_NE(instance, nullptr) << "Failed to create Vulkan instance.";
//    setupDebugMessenger(instance);
//    SUCCEED(); // This marks the test as successful if no assertion failed.
//}