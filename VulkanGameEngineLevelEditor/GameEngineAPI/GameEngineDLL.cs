using Silk.NET.Vulkan;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.Tests;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class GameEngineDLL
    {
        private const string DLLPath = "C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\x64\\Debug\\VulkanDLL.dll";

        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern Result DLL_Buffer_AllocateMemory(Device device, PhysicalDevice physicalDevice, ref Silk.NET.Vulkan.Buffer bufferData, ref DeviceMemory bufferMemory, MemoryPropertyFlags properties);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern Result DLL_Buffer_CreateBuffer(Device device, PhysicalDevice physicalDevice, ref Silk.NET.Vulkan.Buffer buffer, ref DeviceMemory bufferMemory, IntPtr bufferData, ulong bufferSize, BufferUsageFlags bufferUsage, MemoryPropertyFlags properties);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern Result DLL_Buffer_CreateStagingBuffer(Device device, 
                                                                    PhysicalDevice physicalDevice, 
                                                                    CommandPool commandPool, 
                                                                    Silk.NET.Vulkan.Queue graphicsQueue, 
                                                                    ref Silk.NET.Vulkan.Buffer stagingBuffer, 
                                                                    ref Silk.NET.Vulkan.Buffer buffer, 
                                                                    ref DeviceMemory stagingBufferMemory, 
                                                                    ref DeviceMemory bufferMemory, 
                                                                    void* bufferData, 
                                                                    ulong bufferSize, 
                                                                    BufferUsageFlags bufferUsage, 
                                                                    MemoryPropertyFlags properties);

        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern Result DLL_Buffer_CopyBuffer(Silk.NET.Vulkan.Buffer srcBuffer, Silk.NET.Vulkan.Buffer dstBuffer, ulong size);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern Result DLL_Buffer_UpdateBufferSize(Device device, PhysicalDevice physicalDevice, Silk.NET.Vulkan.Buffer buffer, ref DeviceMemory bufferMemory, IntPtr bufferData, ref ulong oldBufferSize, ulong newBufferSize, BufferUsageFlags bufferUsageFlags, MemoryPropertyFlags propertyFlags);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern Result DLL_Buffer_UpdateBufferMemory(Device device, DeviceMemory bufferMemory, IntPtr dataToCopy, ulong bufferSize);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern Result DLL_Buffer_UnmapBufferMemory(Device device, DeviceMemory bufferMemory, ref bool isMapped);
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr DLL_Buffer_MapBufferMemory(Device device, DeviceMemory bufferMemory, ulong bufferSize, ref bool isMapped);

        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Buffer_DestroyBuffer(Device device, ref Silk.NET.Vulkan.Buffer buffer, ref Silk.NET.Vulkan.Buffer stagingBuffer, ref DeviceMemory bufferMemory, ref DeviceMemory stagingBufferMemory, IntPtr bufferData, ref ulong bufferSize, ref BufferUsageFlags bufferUsageFlags, ref MemoryPropertyFlags propertyFlags);

        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern void DLL_Buffer_UpdateBufferData(Device device, ref DeviceMemory stagingBufferMemory, ref DeviceMemory bufferMemory, void* dataToCopy, ulong bufferSize, bool IsStagingBuffer);
    }
}
