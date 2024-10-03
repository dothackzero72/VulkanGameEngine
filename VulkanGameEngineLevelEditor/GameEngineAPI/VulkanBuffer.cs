using Silk.NET.Core.Native;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VulkanGameEngineLevelEditor.Tests;
using VulkanGameEngineLevelEditor.Vulkan;

public unsafe class VulkanBuffer<T> : IDisposable where T : unmanaged
{
    public Silk.NET.Vulkan.Buffer _bufferHandle;
    private DeviceMemory _bufferMemory;
    private Silk.NET.Vulkan.Buffer _stagingBufferHandle;
    private DeviceMemory _stagingBufferMemory;
    public  ulong _bufferSize = 0;
    private BufferUsageFlags _bufferUsage;
    private MemoryPropertyFlags _memoryProperties;
    private bool _isMapped = false;
    private bool _isBuffered = false;
    public VulkanBuffer(void* bufferData, uint bufferElementCount, MemoryPropertyFlags properties, bool isBuffered)
    {
        _bufferSize = (ulong)(sizeof(T) * bufferElementCount);
        _memoryProperties = properties;
        _isBuffered = isBuffered;
        _bufferUsage = BufferUsageFlags.VertexBufferBit |
                       BufferUsageFlags.IndexBufferBit |
                       BufferUsageFlags.TransferSrcBit |
                       BufferUsageFlags.TransferDstBit |
                       BufferUsageFlags.StorageBufferBit |
                       BufferUsageFlags.UniformBufferBit;

        if (isBuffered)
        {
            CreateStagingBuffer(bufferData);
        }
        else
        {
            (_bufferHandle, _bufferMemory) = CreateBuffer(bufferData);
        }
    }

    public void CreateStagingBuffer(void* bufferData)
    {
        (_stagingBufferHandle, _stagingBufferMemory) = CreateBuffer(bufferData);
        CopyBufferMemory(bufferData, _stagingBufferMemory);
        (_bufferHandle, _bufferMemory) = CreateBuffer(bufferData);
        CopyBuffer(_stagingBufferHandle, _bufferHandle, _bufferSize);
    }

    private void CopyBufferMemory(void* bufferData, DeviceMemory deviceMemory)
    {
        void* mappedMemory;
        var result = VKConst.vulkan.MapMemory(SilkVulkanRenderer.device, deviceMemory, 0, _bufferSize, 0, &mappedMemory);
        System.Buffer.MemoryCopy(bufferData, mappedMemory, _bufferSize, _bufferSize);
        VKConst.vulkan.UnmapMemory(SilkVulkanRenderer.device, deviceMemory);
    }

    private new (Silk.NET.Vulkan.Buffer, DeviceMemory) CreateBuffer(void* bufferData)
    {
        var bufferCreateInfo = new BufferCreateInfo
        {
            SType = StructureType.BufferCreateInfo,
            Size = _bufferSize,
            Usage = BufferUsageFlags.BufferUsageTransferSrcBit,
            SharingMode = SharingMode.Exclusive
        };

        VKConst.vulkan.CreateBuffer(SilkVulkanRenderer.device, &bufferCreateInfo, null, out Silk.NET.Vulkan.Buffer bufferHandle);

        var bufferMemory = AllocateBufferMemory(bufferHandle);
        BindBufferMemory(bufferHandle, bufferMemory);
        if (bufferData != null)
        {
            UpdateBufferData(bufferData, bufferMemory);
        }

        return new(bufferHandle, bufferMemory);
    }

    private DeviceMemory AllocateBufferMemory(Silk.NET.Vulkan.Buffer bufferHandle)
    {
        VKConst.vulkan.GetBufferMemoryRequirements(SilkVulkanRenderer.device, bufferHandle, out var memRequirements);
        var allocInfo = new MemoryAllocateInfo
        {
            SType = StructureType.MemoryAllocateInfo,
            AllocationSize = memRequirements.Size,
            MemoryTypeIndex = SilkVulkanRenderer.GetMemoryType(memRequirements.MemoryTypeBits, _memoryProperties)
        };

        VKConst.vulkan.AllocateMemory(SilkVulkanRenderer.device, &allocInfo, null, out DeviceMemory bufferMemory);
        return bufferMemory;
    }

    private void BindBufferMemory(Silk.NET.Vulkan.Buffer buffer, DeviceMemory bufferMemory)
    {
        VKConst.vulkan.BindBufferMemory(SilkVulkanRenderer.device, buffer, bufferMemory, 0);
    }

    private void UpdateBufferData(void* data, DeviceMemory deviceMemory)
    {
        if (_isMapped)
        {
            Console.Error.WriteLine("Buffer already mapped!");
            return;
        }

        if (data == null)
        {
            throw new ArgumentNullException(nameof(data), "Data pointer cannot be null.");
        }

        ulong elementCount = _bufferSize / (ulong)sizeof(T);

        void* mappedMemory;
        var result = VKConst.vulkan.MapMemory(SilkVulkanRenderer.device, deviceMemory, 0, _bufferSize, 0, &mappedMemory);
        if (result != Result.Success)
        {
            throw new InvalidOperationException("Failed to map Vulkan buffer memory.");
        }

        System.Buffer.MemoryCopy(data, mappedMemory, _bufferSize, _bufferSize);
        VKConst.vulkan.UnmapMemory(SilkVulkanRenderer.device, deviceMemory);
        _isMapped = false;
    }


    private void CopyBuffer(Silk.NET.Vulkan.Buffer srcBuffer, Silk.NET.Vulkan.Buffer dstBuffer, ulong size)
    {
        var copyRegion = new BufferCopy
        {
            SrcOffset = 0,
            DstOffset = 0,
            Size = (ulong)_bufferSize
        };

        var commandBuffer = SilkVulkanRenderer.BeginSingleUseCommandBuffer();
        VKConst.vulkan.CmdCopyBuffer(commandBuffer, srcBuffer, dstBuffer, new BufferCopy[] { copyRegion });
        SilkVulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);
    }

    //public static Silk.NET.Vulkan.Buffer CreateBuffer(void* bufferData, ulong bufferSize, BufferUsageFlags usage, MemoryPropertyFlags properties)
    //{
    //    var bufferCreateInfo = new BufferCreateInfo
    //    {
    //        SType = StructureType.BufferCreateInfo,
    //        Size = bufferSize,
    //        Usage = usage,
    //        SharingMode = SharingMode.Exclusive
    //    };

    //    VKConst.vulkan.CreateBuffer(SilkVulkanRenderer.device, &bufferCreateInfo, null, out Silk.NET.Vulkan.Buffer bufferHandle);

    //    var bufferMemory = AllocateBufferMemory(bufferHandle, properties);
    //    BindBufferMemory(_buffer);

    //    if (bufferData != null)
    //    {
    //        UpdateBufferData(bufferData);
    //    }

    //    return bufferHandle;
    //}

    public static DeviceMemory AllocateBufferMemory(Silk.NET.Vulkan.Buffer buffer, MemoryPropertyFlags properties)
    {
        VKConst.vulkan.GetBufferMemoryRequirements(SilkVulkanRenderer.device, buffer, out var memRequirements);
        var allocInfo = new MemoryAllocateInfo
        {
            SType = StructureType.MemoryAllocateInfo,
            AllocationSize = memRequirements.Size,
            MemoryTypeIndex = SilkVulkanRenderer.GetMemoryType(memRequirements.MemoryTypeBits, properties)
        };

        VKConst.vulkan.AllocateMemory(SilkVulkanRenderer.device, &allocInfo, null, out DeviceMemory bufferMemory);
        return bufferMemory;
    }

    public static void BindBufferMemory(ref Silk.NET.Vulkan.Buffer buffer, ref DeviceMemory deviceMemory)
    {
        VKConst.vulkan.BindBufferMemory(SilkVulkanRenderer.device, buffer, deviceMemory, 0);
    }

    //public static void UpdateBufferData(void* data)
    //{
    //    if (_isMapped)
    //    {
    //        Console.Error.WriteLine("Buffer already mapped!");
    //        return;
    //    }

    //    if (data == null)
    //    {
    //        throw new ArgumentNullException(nameof(data), "Data pointer cannot be null.");
    //    }

    //    ulong elementCount = _bufferSize / (ulong)sizeof(T);

    //    void* mappedMemory;
    //    var result = VKConst.vulkan.MapMemory(SilkVulkanRenderer.device, _bufferMemory, 0, _bufferSize, 0, &mappedMemory);
    //    if (result != Result.Success)
    //    {
    //        throw new InvalidOperationException("Failed to map Vulkan buffer memory.");
    //    }

    //    System.Buffer.MemoryCopy(data, mappedMemory, _bufferSize, _bufferSize);
    //    VKConst.vulkan.UnmapMemory(SilkVulkanRenderer.device, _bufferMemory);
    //    _isMapped = false;
    //}

    //public static void CopyBuffer(Silk.NET.Vulkan.Buffer srcBuffer, Silk.NET.Vulkan.Buffer dstBuffer, ulong size)
    //{
    //    CommandBuffer commandBuffer = SilkVulkanRenderer.BeginSingleUseCommandBuffer();

    //    var copyRegion = new BufferCopy
    //    {
    //        SrcOffset = 0,
    //        DstOffset = 0,
    //        Size = size
    //    };
    //    VKConst.vulkan.CmdCopyBuffer(commandBuffer, srcBuffer, dstBuffer, 1, new[] { copyRegion });
    //    SilkVulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);
    //}

    private CommandBuffer BeginSingleTimeCommands()
    {
        var allocInfo = new CommandBufferAllocateInfo
        {
            SType = StructureType.CommandBufferAllocateInfo,
            CommandPool = SilkVulkanRenderer.commandPool,
            Level = CommandBufferLevel.Primary,
            CommandBufferCount = 1
        };

        VKConst.vulkan.AllocateCommandBuffers(SilkVulkanRenderer.device, allocInfo, out var commandBuffer);

        var beginInfo = new CommandBufferBeginInfo
        {
            SType = StructureType.CommandBufferBeginInfo,
            Flags = CommandBufferUsageFlags.OneTimeSubmitBit
        };

        VKConst.vulkan.BeginCommandBuffer(commandBuffer, beginInfo);
        return commandBuffer;
    }

    private void EndSingleTimeCommands(CommandBuffer commandBuffer)
    {
        VKConst.vulkan.EndCommandBuffer(commandBuffer);

        var submitInfo = new SubmitInfo
        {
            SType = StructureType.SubmitInfo,
            CommandBufferCount = 1,
            PCommandBuffers = &commandBuffer
        };

        VKConst.vulkan.QueueSubmit(SilkVulkanRenderer.graphicsQueue, 1, new[] { submitInfo }, new Fence(null));
        VKConst.vulkan.QueueWaitIdle(SilkVulkanRenderer.graphicsQueue);

        VKConst.vulkan.FreeCommandBuffers(SilkVulkanRenderer.device, SilkVulkanRenderer.commandPool, 1, ref commandBuffer);
    }

    public void Dispose()
    {
        VKConst.vulkan.FreeMemory(SilkVulkanRenderer.device, _bufferMemory, null);
        VKConst.vulkan.DestroyBuffer(SilkVulkanRenderer.device, _bufferHandle, null);
        VKConst.vulkan.FreeMemory(SilkVulkanRenderer.device, _stagingBufferMemory, null);
        VKConst.vulkan.DestroyBuffer(SilkVulkanRenderer.device, _stagingBufferHandle, null);
    }
}