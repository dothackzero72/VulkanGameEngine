using Silk.NET.Core.Native;
using Silk.NET.Vulkan;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class GameEngineDLL
    {
        private const string DLLPath = "C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\x64\\Debug\\VulkanEngineDLL.dll";

        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern Result DLL_Buffer_AllocateMemory(VkDevice device, VkPhysicalDevice physicalDevice, ref VkBuffer bufferData, ref VkDeviceMemory bufferMemory, VkMemoryPropertyFlagBits properties);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern Result DLL_Buffer_CreateBuffer(VkDevice device, VkPhysicalDevice physicalDevice, ref VkBuffer buffer, ref VkDeviceMemory bufferMemory, IntPtr bufferData, ulong bufferSize, VkBufferUsageFlagBits bufferUsage, VkMemoryPropertyFlagBits properties);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern Result DLL_Buffer_CreateStagingBuffer(VkDevice device,
                                                                    VkPhysicalDevice physicalDevice,
                                                                    VkCommandPool commandPool,
                                                                    VkQueue graphicsQueue, 
                                                                    ref VkBuffer stagingBuffer, 
                                                                    ref VkBuffer buffer, 
                                                                    ref VkDeviceMemory stagingBufferMemory, 
                                                                    ref VkDeviceMemory bufferMemory, 
                                                                    void* bufferData, 
                                                                    ulong bufferSize,
                                                                    VkBufferUsageFlagBits bufferUsage,
                                                                    VkMemoryPropertyFlagBits properties);

        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern Result DLL_Buffer_CopyBuffer(VkBuffer srcBuffer, VkBuffer dstBuffer, ulong size);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern Result DLL_Buffer_UpdateBufferSize(VkDevice device, VkPhysicalDevice physicalDevice, VkBuffer buffer, ref VkDeviceMemory bufferMemory, IntPtr bufferData, ref ulong oldBufferSize, ulong newBufferSize, VkBufferUsageFlagBits bufferUsageFlags, VkMemoryPropertyFlagBits propertyFlags);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern Result Buffer_UpdateBufferMemory(VkDevice device, VkDeviceMemory bufferMemory, IntPtr dataToCopy, ulong bufferSize);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern Result DLL_Buffer_UnmapBufferMemory(VkDevice device, VkDeviceMemory bufferMemory, ref bool isMapped);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr DLL_Buffer_MapBufferMemory(VkDevice device, VkDeviceMemory bufferMemory, ulong bufferSize, ref bool isMapped);

        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Buffer_DestroyBuffer(VkDevice device, ref VkBuffer buffer, ref VkBuffer stagingBuffer, ref VkDeviceMemory bufferMemory, ref VkDeviceMemory stagingBufferMemory, IntPtr bufferData, ref ulong bufferSize, ref VkBufferUsageFlagBits bufferUsageFlags, ref VkMemoryPropertyFlagBits propertyFlags);

        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Buffer_UpdateBufferData(VkDevice device, ref VkDeviceMemory stagingBufferMemory, ref VkDeviceMemory bufferMemory, void* dataToCopy, ulong bufferSize, bool IsStagingBuffer);

        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern Result DLL_Renderer_GetSurfaceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, out VkSurfaceFormatKHR surfaceFormats, out uint surfaceFormatCount);

        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern Result DLL_SwapChain_GetPhysicalDevicePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, out VkPresentModeKHR compatiblePresentModesList, out uint presentModeCount);

    }
}
