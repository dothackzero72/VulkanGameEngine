using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static VulkanGameEngineLevelEditor.Form1;

namespace VulkanGameEngineLevelEditor
{
    public class GameEngineAPI_DLL
    {
        private const string DLLPath = "C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\x64\\Debug\\";
        [DllImport($"{DLLPath}VulkanDLL.dll", CallingConvention = CallingConvention.StdCall)] public static extern VkInstance GameEngineAPI_CreateVulkanInstance();
        [DllImport($"{DLLPath}VulkanDLL.dll", CallingConvention = CallingConvention.Cdecl)] public static extern void GameEngineAPI_GetWin32Extensions(ref uint extensionCount, out IntPtr enabledExtensions);
        const string GameEngineDLL = "C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\x64\\Debug\\VulkanDLL.dll";
        [DllImport(GameEngineDLL, CallingConvention = CallingConvention.StdCall)] private static extern void GameEngineAPI_GetWin32Extensions(ref uint count, out VkSurfaceKHR extensionsPtr);
        [DllImport(GameEngineDLL, CallingConvention = CallingConvention.StdCall)] private static extern void FreeExtensions(IntPtr extensionsPtr);
        [DllImport(GameEngineDLL, CallingConvention = CallingConvention.StdCall)] public static extern VkInstance GameEngineAPI_CreateVulkanInstance(RichTextCallback callback);
        [DllImport(GameEngineDLL, CallingConvention = CallingConvention.StdCall)] public static extern VkDebugUtilsMessengerEXT GameEngineAPI_SetupDebugMessenger(RichTextCallback callback, VkInstance instance);
        [DllImport(GameEngineDLL, CallingConvention = CallingConvention.StdCall)] public static extern int SimpleTestDLL();
        [DllImport(GameEngineDLL, CallingConvention = CallingConvention.StdCall)] public static extern int SimpleTestLIB();

        [DllImport(GameEngineDLL, CallingConvention = CallingConvention.StdCall)] public static extern void GameEngineAPI_DestroyPipelineCache(IntPtr pipelineCache);

        [DllImport(GameEngineDLL, CallingConvention = CallingConvention.StdCall)] public static extern void GameEngineAPI_DestroyFences(VkDevice device, VkSemaphore acquireImageSemaphores, VkSemaphore presentImageSemaphores, VkFence fences, UInt32 semaphoreCount, int bufferCount);

        [DllImport(GameEngineDLL, CallingConvention = CallingConvention.StdCall)] public static extern void GameEngineAPI_DestroyCommandPool(VkDevice device, VkCommandPool commandPool);

        [DllImport(GameEngineDLL, CallingConvention = CallingConvention.StdCall)] public static extern void GameEngineAPI_DestroyDevice(VkDevice device);

        [DllImport(GameEngineDLL, CallingConvention = CallingConvention.StdCall)] public static extern void GameEngineAPI_DestroySurface(VkInstance instance, VkSurfaceKHR surface);

        [DllImport(GameEngineDLL, CallingConvention = CallingConvention.StdCall)] public static extern void GameEngineAPI_DestroyDebugger(VkInstance instance);

        [DllImport(GameEngineDLL, CallingConvention = CallingConvention.StdCall)] public static extern void GameEngineAPI_DestroyInstance(VkInstance instance);

        [DllImport(GameEngineDLL, CallingConvention = CallingConvention.StdCall)] public static extern void GameEngineAPI_DestroyRenderPass(VkRenderPass renderPass);

        [DllImport(GameEngineDLL, CallingConvention = CallingConvention.StdCall)] public static extern void GameEngineAPI_DestroyFrameBuffers(VkFramebuffer frameBufferList);

        [DllImport(GameEngineDLL, CallingConvention = CallingConvention.StdCall)] public static extern void GameEngineAPI_DestroyDescriptorPool(VkDescriptorPool descriptorPool);

        [DllImport(GameEngineDLL, CallingConvention = CallingConvention.StdCall)] public static extern void GameEngineAPI_DestroyDescriptorSetLayout(VkDescriptorSetLayout descriptorSetLayout);

        [DllImport(GameEngineDLL, CallingConvention = CallingConvention.StdCall)] public static extern void GameEngineAPI_DestroyCommandBuffers(VkCommandPool commandPool, VkCommandBuffer commandBufferList);

        [DllImport(GameEngineDLL, CallingConvention = CallingConvention.StdCall)] public static extern void GameEngineAPI_DestroyBuffer(VkBuffer buffer);

        [DllImport(GameEngineDLL, CallingConvention = CallingConvention.StdCall)] public static extern void GameEngineAPI_FreeMemory(VkDeviceMemory memory);

        [DllImport(GameEngineDLL, CallingConvention = CallingConvention.StdCall)] public static extern void GameEngineAPI_DestroyImageView(VkImageView imageView);

        [DllImport(GameEngineDLL, CallingConvention = CallingConvention.StdCall)] public static extern void GameEngineAPI_DestroyImage(VkImage image);

        [DllImport(GameEngineDLL, CallingConvention = CallingConvention.StdCall)] public static extern void GameEngineAPI_DestroySampler(VkSampler sampler);

        [DllImport(GameEngineDLL, CallingConvention = CallingConvention.StdCall)] public static extern void GameEngineAPI_DestroyPipeline(VkPipeline pipeline);

        [DllImport(GameEngineDLL, CallingConvention = CallingConvention.StdCall)] public static extern void GameEngineAPI_DestroyPipelineLayout(VkPipeline pipelineLayout);
    }
}
