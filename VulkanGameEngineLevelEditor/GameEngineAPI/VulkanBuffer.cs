using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Silk.NET.Core.Native;
using Silk.NET.Vulkan;
using VulkanGameEngineLevelEditor.Tests;
using VulkanGameEngineLevelEditor.Vulkan;
using static VulkanGameEngineLevelEditor.GameEngineAPI.GameEngineDLL;
public unsafe class VulkanBuffer<T> : IDisposable where T : unmanaged
{
    private Device _device;
    private PhysicalDevice _physicalDevice;
    private CommandPool _commandPool;
    private Silk.NET.Vulkan.Queue _graphicsQueue;

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

    private Result CreateBuffer(void* bufferData)
    {
        return DLL_Buffer_CreateBuffer(_device, _physicalDevice, ref Buffer, ref BufferMemory, (IntPtr)bufferData, BufferSize, BufferUsage, BufferProperties);
    }

    private Result CreateStagingBuffer(void* bufferData)
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

    public void UpdateBufferData(IntPtr bufferData)
    {
       DLL_Buffer_UpdateBufferData(_device, ref StagingBufferMemory, ref BufferMemory,  &bufferData, BufferSize, IsStagingBuffer);
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


//using Silk.NET.Core.Native;
//using Silk.NET.SDL;
//using Silk.NET.Vulkan;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Runtime.InteropServices;
//using VulkanGameEngineLevelEditor.GameEngineAPI;
//using VulkanGameEngineLevelEditor.Tests;
//using VulkanGameEngineLevelEditor.Vulkan;
//using static VulkanGameEngineLevelEditor.GameEngineAPI.GameEngineDLL;

//public unsafe class VulkanBuffer<T> : IDisposable where T : unmanaged
//{
//	std::shared_ptr<VkDevice> _device;
//    std::shared_ptr<VkPhysicalDevice> _physicalDevice;
//    std::shared_ptr<VkCommandPool> _commandPool;
//    std::shared_ptr<VkQueue> _graphicsQueue;

//    protected:
//	VkBuffer StagingBuffer = VK_NULL_HANDLE;
//    VkDeviceMemory StagingBufferMemory = VK_NULL_HANDLE;
//    VkDeviceMemory BufferMemory = VK_NULL_HANDLE;
//    VkDeviceSize BufferSize = 0;
//    VkBufferUsageFlags BufferUsage;
//    VkMemoryPropertyFlags BufferProperties;
//    uint64 BufferDeviceAddress = 0;
//    VkAccelerationStructureKHR BufferHandle = VK_NULL_HANDLE;
//    void* BufferData;
//    bool IsMapped = false;
//    bool IsStagingBuffer = false;

//    VkResult CreateBuffer(void* bufferData)
//    {
//        VkResult result = Buffer_CreateBuffer(*_device.get(), *_physicalDevice.get(), &Buffer, &BufferMemory, bufferData, BufferSize, BufferProperties, BufferUsage);
//        return result;
//    }

//    VkResult CreateStagingBuffer(void* bufferData)
//    {
//        return Buffer_CreateStagingBuffer(*_device.get(), *_physicalDevice.get(), *_commandPool.get(), *_graphicsQueue.get(), &StagingBuffer, &Buffer, &StagingBufferMemory, &BufferMemory, bufferData, BufferSize, BufferUsage, BufferProperties);
//    }

//    VkResult UpdateBufferSize(VkBuffer buffer, VkDeviceMemory bufferMemory, VkDeviceSize newBufferSize)
//    {
//        VkResult result = Buffer_UpdateBufferSize(*_device.get(), *_physicalDevice.get(), buffer, &bufferMemory, BufferData, &BufferSize, newBufferSize, BufferUsage, BufferProperties);
//        DestroyBuffer();
//        return result;
//    }

//    public:
//	VkBuffer Buffer = VK_NULL_HANDLE;
//    VkDescriptorBufferInfo DescriptorBufferInfo;

//    VulkanBuffer()
//    {
//        _device = std::make_shared<VkDevice>(cRenderer.Device);
//        _physicalDevice = std::make_shared<VkPhysicalDevice>(cRenderer.PhysicalDevice);
//        _commandPool = std::make_shared<VkCommandPool>(cRenderer.CommandPool);
//        _graphicsQueue = std::make_shared<VkQueue>(cRenderer.SwapChain.GraphicsQueue);
//    }

//    VulkanBuffer(void* bufferData, uint32 bufferElementCount, VkBufferUsageFlags usage, VkMemoryPropertyFlags properties, bool isStagingBuffer)
//    {
//        _device = std::make_shared<VkDevice>(cRenderer.Device);
//        _physicalDevice = std::make_shared<VkPhysicalDevice>(cRenderer.PhysicalDevice);
//        _commandPool = std::make_shared<VkCommandPool>(cRenderer.CommandPool);
//        _graphicsQueue = std::make_shared<VkQueue>(cRenderer.SwapChain.GraphicsQueue);

//        BufferSize = sizeof(T) * bufferElementCount;
//        BufferProperties = properties;
//        IsStagingBuffer = isStagingBuffer;
//        BufferUsage = usage;

//        if (isStagingBuffer)
//        {
//            CreateStagingBuffer(bufferData);
//        }
//        else
//        {
//            CreateBuffer(bufferData);
//        }
//    }

//    virtual ~VulkanBuffer()
//    {
//    }

//    static VkResult CopyBuffer(VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size)
//    {
//        return Buffer_CopyBuffer(*_device.get(), *_commandPool.get(), cRenderer.SwapChain.GraphicsQueue, srcBuffer, dstBuffer, size);
//    }

//    void UpdateBufferData(T& bufferData)
//    {
//        Buffer_UpdateBufferData(*_device.get(), &BufferMemory, static_cast<void*>(&bufferData), BufferSize, IsStagingBuffer);
//    }

//    void UpdateBufferData(T& bufferData, VkDeviceMemory bufferMemory)
//    {
//        Buffer_UpdateBufferData(*_device.get(), &BufferMemory, static_cast<void*>(&bufferData), BufferSize, IsStagingBuffer);
//    }

//    void UpdateBufferData(List<T>& bufferData, VkDeviceMemory bufferMemory)
//    {
//        const VkDeviceSize newBufferSize = sizeof(T) * bufferData.size();
//        if (BufferSize != newBufferSize)
//        {
//            if (UpdateBufferSize(newBufferSize) != VK_SUCCESS)
//            {
//                RENDERER_ERROR("Failed to update buffer size.");
//                return;
//            }
//        }

//        if (!IsMapped)
//        {
//            RENDERER_ERROR("Buffer is not mapped! Cannot update data.");
//            return;
//        }

//        Buffer_UpdateBufferData(*_device.get(), &StagingBufferMemory, &BufferMemory, static_cast<void*>(bufferData.data()), BufferSize, IsStagingBuffer);
//    }

//    void UpdateBufferData(void* bufferData, VkDeviceSize bufferListCount, VkDeviceMemory bufferMemory)
//    {
//        const VkDeviceSize newBufferSize = sizeof(T) * bufferListCount;
//        if (BufferSize != newBufferSize)
//        {
//            if (UpdateBufferSize(newBufferSize) != VK_SUCCESS)
//            {
//                RENDERER_ERROR("Failed to update buffer size.");
//                return;
//            }
//        }

//        Buffer_UpdateBufferData(*_device.get(), &StagingBufferMemory, &BufferMemory, static_cast<void*>(&bufferData), BufferSize, IsStagingBuffer);
//    }

//    void UpdateBufferData(void* bufferData, VkDeviceMemory bufferMemory)
//    {
//        Buffer_UpdateBufferData(*_device.get(), &StagingBufferMemory, &BufferMemory, bufferData, BufferSize, IsStagingBuffer);
//    }

//    void UpdateBufferData(void* bufferData)
//    {
//        if (BufferSize < sizeof(T))
//        {
//            RENDERER_ERROR("Buffer does not contain enough data for a single T object.");
//            return;
//        }

//        Buffer_UpdateBufferData(*_device.get(), &StagingBufferMemory, &BufferMemory, bufferData, BufferSize, IsStagingBuffer);
//    }

//    std::vector<T> CheckBufferContents()
//    {
//        std::vector<T> DataList;
//        size_t dataListSize = BufferSize / sizeof(T);

//        void* data = Buffer_MapBufferMemory(_device.get(), BufferMemory, BufferSize, *IsMapped);
//        if (data == nullptr)
//        {
//            std::cerr << "Failed to map buffer memory\n";
//            return DataList;
//        }

//        char* newPtr = static_cast<char*>(data);
//        for (size_t x = 0; x < dataListSize; ++x)
//        {
//            DataList.emplace_back(*reinterpret_cast<T*>(newPtr));
//            newPtr += sizeof(T);
//        }
//        Buffer_UnmapBufferMemory(_device.get(), BufferMemory, *IsMapped);

//        return DataList;
//    }

//    VkDescriptorBufferInfo* GetDescriptorbuffer()
//    {
//        DescriptorBufferInfo = VkDescriptorBufferInfo

//        {
//			.buffer = Buffer,
//			.offset = 0,
//			.range = VK_WHOLE_SIZE

//        };
//        return &DescriptorBufferInfo;
//    }

//    void DestroyBuffer()
//    {
//        Buffer_DestroyBuffer(*_device.get(), &Buffer, &StagingBuffer, &BufferMemory, &StagingBufferMemory, &BufferData, &BufferSize, &BufferUsage, &BufferProperties);
//    }

//    //public Silk.NET.Vulkan.Buffer _bufferHandle;
//    //private DeviceMemory _bufferMemory;
//    //private Silk.NET.Vulkan.Buffer _stagingBufferHandle;
//    //private DeviceMemory _stagingBufferMemory;
//    //public  ulong _bufferSize = 0;
//    //private BufferUsageFlags _bufferUsage;
//    //private MemoryPropertyFlags _memoryProperties;
//    //private bool _isMapped = false;
//    //private bool _isBuffered = false;
//    //public VulkanBuffer(void* bufferData, uint bufferElementCount, MemoryPropertyFlags properties, bool isBuffered)
//    //{
//    //    _bufferSize = (ulong)(sizeof(T) * bufferElementCount);
//    //    _memoryProperties = properties;
//    //    _isBuffered = isBuffered;
//    //    _bufferUsage = BufferUsageFlags.VertexBufferBit |
//    //                   BufferUsageFlags.IndexBufferBit |
//    //                   BufferUsageFlags.TransferSrcBit |
//    //                   BufferUsageFlags.TransferDstBit |
//    //                   BufferUsageFlags.StorageBufferBit |
//    //                   BufferUsageFlags.UniformBufferBit;

//    //    if (isBuffered)
//    //    {
//    //        CreateStagingBuffer(bufferData);
//    //    }
//    //    else
//    //    {
//    //        (_bufferHandle, _bufferMemory) = CreateBuffer(bufferData);
//    //    }
//    //}

//    //public void CreateStagingBuffer(void* bufferData)
//    //{
//    //    (_stagingBufferHandle, _stagingBufferMemory) = CreateBuffer(bufferData);
//    //    CopyBufferMemory(bufferData, _stagingBufferMemory);
//    //    (_bufferHandle, _bufferMemory) = CreateBuffer(bufferData);
//    //    CopyBuffer(_stagingBufferHandle, _bufferHandle, _bufferSize);
//    //}

//    //private void CopyBufferMemory(void* bufferData, DeviceMemory deviceMemory)
//    //{
//    //    void* mappedMemory;
//    //    var result = VKConst.vulkan.MapMemory(SilkVulkanRenderer.device, deviceMemory, 0, _bufferSize, 0, &mappedMemory);
//    //    System.Buffer.MemoryCopy(bufferData, mappedMemory, _bufferSize, _bufferSize);
//    //    VKConst.vulkan.UnmapMemory(SilkVulkanRenderer.device, deviceMemory);
//    //}

//    //private new (Silk.NET.Vulkan.Buffer, DeviceMemory) CreateBuffer(void* bufferData)
//    //{
//    //    var bufferCreateInfo = new BufferCreateInfo
//    //    {
//    //        SType = StructureType.BufferCreateInfo,
//    //        Size = _bufferSize,
//    //        Usage = BufferUsageFlags.BufferUsageTransferSrcBit,
//    //        SharingMode = SharingMode.Exclusive
//    //    };

//    //    VKConst.vulkan.CreateBuffer(SilkVulkanRenderer.device, &bufferCreateInfo, null, out Silk.NET.Vulkan.Buffer bufferHandle);

//    //    var bufferMemory = AllocateBufferMemory(bufferHandle);
//    //    BindBufferMemory(bufferHandle, bufferMemory);
//    //    if (bufferData != null)
//    //    {
//    //        UpdateBufferData(bufferData, bufferMemory);
//    //    }

//    //    return new(bufferHandle, bufferMemory);
//    //}

//    //private DeviceMemory AllocateBufferMemory(Silk.NET.Vulkan.Buffer bufferHandle)
//    //{
//    //    VKConst.vulkan.GetBufferMemoryRequirements(SilkVulkanRenderer.device, bufferHandle, out var memRequirements);
//    //    var allocInfo = new MemoryAllocateInfo
//    //    {
//    //        SType = StructureType.MemoryAllocateInfo,
//    //        AllocationSize = memRequirements.Size,
//    //        MemoryTypeIndex = SilkVulkanRenderer.GetMemoryType(memRequirements.MemoryTypeBits, _memoryProperties)
//    //    };

//    //    VKConst.vulkan.AllocateMemory(SilkVulkanRenderer.device, &allocInfo, null, out DeviceMemory bufferMemory);
//    //    return bufferMemory;
//    //}

//    //private void BindBufferMemory(Silk.NET.Vulkan.Buffer buffer, DeviceMemory bufferMemory)
//    //{
//    //    VKConst.vulkan.BindBufferMemory(SilkVulkanRenderer.device, buffer, bufferMemory, 0);
//    //}

//    //private void UpdateBufferData(void* data, DeviceMemory deviceMemory)
//    //{
//    //    if (_isMapped)
//    //    {
//    //        Console.Error.WriteLine("Buffer already mapped!");
//    //        return;
//    //    }

//    //    if (data == null)
//    //    {
//    //        throw new ArgumentNullException(nameof(data), "Data pointer cannot be null.");
//    //    }

//    //    ulong elementCount = _bufferSize / (ulong)sizeof(T);

//    //    void* mappedMemory;
//    //    var result = VKConst.vulkan.MapMemory(SilkVulkanRenderer.device, deviceMemory, 0, _bufferSize, 0, &mappedMemory);
//    //    if (result != Result.Success)
//    //    {
//    //        throw new InvalidOperationException("Failed to map Vulkan buffer memory.");
//    //    }

//    //    System.Buffer.MemoryCopy(data, mappedMemory, _bufferSize, _bufferSize);
//    //    VKConst.vulkan.UnmapMemory(SilkVulkanRenderer.device, deviceMemory);
//    //    _isMapped = false;
//    //}


//    //private void CopyBuffer(Silk.NET.Vulkan.Buffer srcBuffer, Silk.NET.Vulkan.Buffer dstBuffer, ulong size)
//    //{
//    //    var copyRegion = new BufferCopy
//    //    {
//    //        SrcOffset = 0,
//    //        DstOffset = 0,
//    //        Size = (ulong)_bufferSize
//    //    };

//    //    var commandBuffer = SilkVulkanRenderer.BeginSingleUseCommandBuffer();
//    //    VKConst.vulkan.CmdCopyBuffer(commandBuffer, srcBuffer, dstBuffer, new BufferCopy[] { copyRegion });
//    //    SilkVulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);
//    //}

//    ////public static Silk.NET.Vulkan.Buffer CreateBuffer(void* bufferData, ulong bufferSize, BufferUsageFlags usage, MemoryPropertyFlags properties)
//    ////{
//    ////    var bufferCreateInfo = new BufferCreateInfo
//    ////    {
//    ////        SType = StructureType.BufferCreateInfo,
//    ////        Size = bufferSize,
//    ////        Usage = usage,
//    ////        SharingMode = SharingMode.Exclusive
//    ////    };

//    ////    VKConst.vulkan.CreateBuffer(SilkVulkanRenderer.device, &bufferCreateInfo, null, out Silk.NET.Vulkan.Buffer bufferHandle);

//    ////    var bufferMemory = AllocateBufferMemory(bufferHandle, properties);
//    ////    BindBufferMemory(_buffer);

//    ////    if (bufferData != null)
//    ////    {
//    ////        UpdateBufferData(bufferData);
//    ////    }

//    ////    return bufferHandle;
//    ////}

//    //public static DeviceMemory AllocateBufferMemory(Silk.NET.Vulkan.Buffer buffer, MemoryPropertyFlags properties)
//    //{
//    //    VKConst.vulkan.GetBufferMemoryRequirements(SilkVulkanRenderer.device, buffer, out var memRequirements);
//    //    var allocInfo = new MemoryAllocateInfo
//    //    {
//    //        SType = StructureType.MemoryAllocateInfo,
//    //        AllocationSize = memRequirements.Size,
//    //        MemoryTypeIndex = SilkVulkanRenderer.GetMemoryType(memRequirements.MemoryTypeBits, properties)
//    //    };

//    //    VKConst.vulkan.AllocateMemory(SilkVulkanRenderer.device, &allocInfo, null, out DeviceMemory bufferMemory);
//    //    return bufferMemory;
//    //}

//    //public static void BindBufferMemory(ref Silk.NET.Vulkan.Buffer buffer, ref DeviceMemory deviceMemory)
//    //{
//    //    VKConst.vulkan.BindBufferMemory(SilkVulkanRenderer.device, buffer, deviceMemory, 0);
//    //}

//    ////public static void UpdateBufferData(void* data)
//    ////{
//    ////    if (_isMapped)
//    ////    {
//    ////        Console.Error.WriteLine("Buffer already mapped!");
//    ////        return;
//    ////    }

//    ////    if (data == null)
//    ////    {
//    ////        throw new ArgumentNullException(nameof(data), "Data pointer cannot be null.");
//    ////    }

//    ////    ulong elementCount = _bufferSize / (ulong)sizeof(T);

//    ////    void* mappedMemory;
//    ////    var result = VKConst.vulkan.MapMemory(SilkVulkanRenderer.device, _bufferMemory, 0, _bufferSize, 0, &mappedMemory);
//    ////    if (result != Result.Success)
//    ////    {
//    ////        throw new InvalidOperationException("Failed to map Vulkan buffer memory.");
//    ////    }

//    ////    System.Buffer.MemoryCopy(data, mappedMemory, _bufferSize, _bufferSize);
//    ////    VKConst.vulkan.UnmapMemory(SilkVulkanRenderer.device, _bufferMemory);
//    ////    _isMapped = false;
//    ////}

//    ////public static void CopyBuffer(Silk.NET.Vulkan.Buffer srcBuffer, Silk.NET.Vulkan.Buffer dstBuffer, ulong size)
//    ////{
//    ////    CommandBuffer commandBuffer = SilkVulkanRenderer.BeginSingleUseCommandBuffer();

//    ////    var copyRegion = new BufferCopy
//    ////    {
//    ////        SrcOffset = 0,
//    ////        DstOffset = 0,
//    ////        Size = size
//    ////    };
//    ////    VKConst.vulkan.CmdCopyBuffer(commandBuffer, srcBuffer, dstBuffer, 1, new[] { copyRegion });
//    ////    SilkVulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);
//    ////}

//    //private CommandBuffer BeginSingleTimeCommands()
//    //{
//    //    var allocInfo = new CommandBufferAllocateInfo
//    //    {
//    //        SType = StructureType.CommandBufferAllocateInfo,
//    //        CommandPool = SilkVulkanRenderer.commandPool,
//    //        Level = CommandBufferLevel.Primary,
//    //        CommandBufferCount = 1
//    //    };

//    //    VKConst.vulkan.AllocateCommandBuffers(SilkVulkanRenderer.device, allocInfo, out var commandBuffer);

//    //    var beginInfo = new CommandBufferBeginInfo
//    //    {
//    //        SType = StructureType.CommandBufferBeginInfo,
//    //        Flags = CommandBufferUsageFlags.OneTimeSubmitBit
//    //    };

//    //    VKConst.vulkan.BeginCommandBuffer(commandBuffer, beginInfo);
//    //    return commandBuffer;
//    //}

//    //private void EndSingleTimeCommands(CommandBuffer commandBuffer)
//    //{
//    //    VKConst.vulkan.EndCommandBuffer(commandBuffer);

//    //    var submitInfo = new SubmitInfo
//    //    {
//    //        SType = StructureType.SubmitInfo,
//    //        CommandBufferCount = 1,
//    //        PCommandBuffers = &commandBuffer
//    //    };

//    //    VKConst.vulkan.QueueSubmit(SilkVulkanRenderer.graphicsQueue, 1, new[] { submitInfo }, new Fence(null));
//    //    VKConst.vulkan.QueueWaitIdle(SilkVulkanRenderer.graphicsQueue);

//    //    VKConst.vulkan.FreeCommandBuffers(SilkVulkanRenderer.device, SilkVulkanRenderer.commandPool, 1, ref commandBuffer);
//    //}

//    //public void Dispose()
//    //{
//    //    VKConst.vulkan.FreeMemory(SilkVulkanRenderer.device, _bufferMemory, null);
//    //    VKConst.vulkan.DestroyBuffer(SilkVulkanRenderer.device, _bufferHandle, null);
//    //    VKConst.vulkan.FreeMemory(SilkVulkanRenderer.device, _stagingBufferMemory, null);
//    //    VKConst.vulkan.DestroyBuffer(SilkVulkanRenderer.device, _stagingBufferHandle, null);
//    //}
//}