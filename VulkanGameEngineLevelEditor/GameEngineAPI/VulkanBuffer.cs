using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using Silk.NET.Core.Native;
using Silk.NET.Maths;
using Silk.NET.Vulkan;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Vulkan;
using static VulkanGameEngineLevelEditor.GameEngineAPI.GameEngineDLL;
public unsafe class VulkanBuffer<T> : IDisposable where T : unmanaged
{
    protected Vk vk = Vk.GetApi();
    protected Device _device;
    protected PhysicalDevice _physicalDevice;
    protected CommandPool _commandPool;
    protected Silk.NET.Vulkan.Queue _graphicsQueue;

    // Vulkan buffer-related variables
    public Silk.NET.Vulkan.Buffer StagingBuffer;
    public DeviceMemory StagingBufferMemory;
    public DeviceMemory BufferMemory;
    public ulong BufferSize = 0;

    public BufferUsageFlags BufferUsage;
    public MemoryPropertyFlags BufferProperties;
    public ulong BufferDeviceAddress = 0;
    public IntPtr BufferData;
    public bool IsMapped = false;
    public bool IsStagingBuffer = false;

    public Silk.NET.Vulkan.Buffer Buffer;
    public DescriptorBufferInfo DescriptorBufferInfo;

    public VulkanBuffer()
    {
        _device = SilkVulkanRenderer.device;
        _physicalDevice = SilkVulkanRenderer.physicalDevice;
        _commandPool = SilkVulkanRenderer.commandPool;
        _graphicsQueue = SilkVulkanRenderer.graphicsQueue;
    }

    public VulkanBuffer(void* bufferData, uint bufferElementCount, BufferUsageFlags usage, MemoryPropertyFlags properties, bool isStagingBuffer)
    {
        _device = SilkVulkanRenderer.device;
        _physicalDevice = SilkVulkanRenderer.physicalDevice;
        _commandPool = SilkVulkanRenderer.commandPool;
        _graphicsQueue = SilkVulkanRenderer.graphicsQueue;

        BufferSize = (uint)(sizeof(T) * bufferElementCount);
        BufferProperties = properties;
        IsStagingBuffer = isStagingBuffer;
        BufferUsage = usage;

        if (isStagingBuffer)
        {
            CreateStagingBuffer(bufferData);
        }
        else
        {
           Result result = CreateBuffer(bufferData);
        }
    }

    ~VulkanBuffer()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        DestroyBuffer();
    }

    protected Result CreateBuffer(void* bufferData)
    {
        return DLL_Buffer_CreateBuffer(_device, _physicalDevice, ref Buffer, ref BufferMemory, (IntPtr)bufferData, BufferSize, BufferUsage, BufferProperties);
    }

    private DeviceMemory AllocateBufferMemory(Silk.NET.Vulkan.Buffer bufferHandle)
    {
        VKConst.vulkan.GetBufferMemoryRequirements(SilkVulkanRenderer.device, bufferHandle, out var memRequirements);
        var allocInfo = new MemoryAllocateInfo
        {
            SType = StructureType.MemoryAllocateInfo,
            AllocationSize = memRequirements.Size,
            MemoryTypeIndex = SilkVulkanRenderer.GetMemoryType(memRequirements.MemoryTypeBits, BufferProperties)
        };

        VKConst.vulkan.AllocateMemory(SilkVulkanRenderer.device, &allocInfo, null, out DeviceMemory bufferMemory);
        return bufferMemory;
    }
    protected Result CreateStagingBuffer(void* bufferData)
    {
        return DLL_Buffer_CreateStagingBuffer(_device, _physicalDevice, _commandPool, _graphicsQueue, ref StagingBuffer, ref Buffer, ref StagingBufferMemory, ref BufferMemory, bufferData, BufferSize, BufferUsage, BufferProperties);
    }

    private Result UpdateBufferSize(Silk.NET.Vulkan.Buffer buffer, ref DeviceMemory bufferMemory, ulong newBufferSize)
    {
        var result = DLL_Buffer_UpdateBufferSize(_device, _physicalDevice, buffer, ref bufferMemory, BufferData, ref BufferSize, newBufferSize, BufferUsage, BufferProperties);
        DestroyBuffer();
        return result;
    }

    public static Result CopyBuffer(ref Silk.NET.Vulkan.Buffer srcBuffer, ref Silk.NET.Vulkan.Buffer dstBuffer, ulong size)
    {
        return DLL_Buffer_CopyBuffer(srcBuffer, dstBuffer, size);
    }

    virtual public void UpdateBufferData(void* dataToCopy)
    {
        if (IsStagingBuffer)
        {
            void* stagingMappedData;
            void* mappedData;
            Result result = vk.MapMemory(_device, StagingBufferMemory, 0, BufferSize, 0, &stagingMappedData);
            result = vk.MapMemory(_device, BufferMemory, 0, BufferSize, 0, &mappedData);
            System.Buffer.MemoryCopy(dataToCopy, stagingMappedData, BufferSize, BufferSize);
            System.Buffer.MemoryCopy(stagingMappedData, mappedData, BufferSize, BufferSize);
            vk.UnmapMemory(_device, BufferMemory);
            vk.UnmapMemory(_device, StagingBufferMemory);
        }
        else
        {
            void* mappedData;
            var result = vk.MapMemory(_device, BufferMemory, 0, BufferSize, 0, &mappedData);
            System.Buffer.MemoryCopy(dataToCopy, mappedData, BufferSize, BufferSize);
            vk.UnmapMemory(_device, BufferMemory);
        }
    }

    public List<T> CheckBufferContents()
    {
        List<T> dataList = new List<T>();
        //uint dataListSize = (uint)BufferSize / (uint)sizeof(T);

        //void* data = DLL_Buffer_MapBufferMemory(_device, BufferMemory, (uint)BufferSize, ref IsMapped);
        //if (data == null)
        //{
        //    Console.WriteLine("Failed to map buffer memory");
        //    return dataList;
        //}

        //for (uint x = 0; x < dataListSize; ++x)
        //{
        //    dataList.Add(Marshal.PtrToStructure<T>((IntPtr)bufferData));
        //    data += sizeof(T);
        //}

        //DLL_Buffer_UnmapBufferMemory(_device, BufferMemory, out IsMapped);
        return dataList;
    }

    public DescriptorBufferInfo* GetDescriptorBuffer()
    {
        ulong offset = 0; // Your offset calculation
        ulong alignment = 64;
        DescriptorBufferInfo = new DescriptorBufferInfo
        {
            Buffer = Buffer,
            Offset = 0,
            Range = Vk.WholeSize
        };

        var bufferInfo = DescriptorBufferInfo;
        return &bufferInfo;
    }

    public void DestroyBuffer()
    {
        DLL_Buffer_DestroyBuffer(_device, ref Buffer, ref StagingBuffer, ref BufferMemory, ref StagingBufferMemory, BufferData, ref BufferSize, ref BufferUsage, ref BufferProperties);
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct UniformBufferObject
{
    public Matrix4X4<float> model;
    public Matrix4X4<float> view;
    public Matrix4X4<float> proj;

}
