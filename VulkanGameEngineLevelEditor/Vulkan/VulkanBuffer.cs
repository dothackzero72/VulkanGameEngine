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

namespace VulkanGameEngineLevelEditor.Vulkan
{
    public unsafe class VulkanBuffer<T> : IDisposable where T : unmanaged
    {
        protected Vk vk = Vk.GetApi();
        protected VkDevice _device { get; set; }
        protected VkPhysicalDevice _physicalDevice { get; set; }
        protected VkCommandPool _commandPool { get; set; }
        protected VkQueue _graphicsQueue { get; set; }
        public VkBuffer StagingBuffer;
        public VkDeviceMemory StagingBufferMemory;
        public VkDeviceMemory BufferMemory;
        public ulong BufferSize = 0;
        public VkBufferUsageFlagBits BufferUsage;
        public VkMemoryPropertyFlagBits BufferProperties;
        public ulong BufferDeviceAddress = 0;
        public IntPtr BufferData;
        public bool IsMapped = false;
        public bool IsStagingBuffer = false;
        public VkBuffer Buffer;
        public VkDescriptorBufferInfo DescriptorBufferInfo;

        public VulkanBuffer()
        {
            _device = VulkanRenderer.device;
            _physicalDevice = VulkanRenderer.physicalDevice;
            _commandPool = VulkanRenderer.commandPool;
            _graphicsQueue = VulkanRenderer.graphicsQueue;
        }

        public VulkanBuffer(void* bufferData, uint bufferElementCount, VkBufferUsageFlagBits usage, VkMemoryPropertyFlagBits properties, bool isStagingBuffer)
        {
            _device = VulkanRenderer.device;
            _physicalDevice = VulkanRenderer.physicalDevice;
            _commandPool = VulkanRenderer.commandPool;
            _graphicsQueue = VulkanRenderer.graphicsQueue;

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
                VkResult result = DLLCreateBuffer(bufferData);
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

        protected VkResult DLLCreateBuffer(void* bufferData)
        {
            return DLL_Buffer_CreateBuffer(_device, _physicalDevice, ref Buffer, ref BufferMemory, (IntPtr)bufferData, BufferSize, BufferUsage, BufferProperties);
        }
        protected Result CreateBuffer(void* bufferData)
        {
            CBuffer.CreateBuffer(out VkBuffer buffer, out VkDeviceMemory bufferMemory, bufferData, BufferSize, BufferUsage, BufferProperties);
            Buffer = buffer;
            BufferMemory = bufferMemory;
            return Result.Success;
        }

        private VkDeviceMemory AllocateBufferMemory(VkBuffer bufferHandle)
        {
            VkFunc.vkGetBufferMemoryRequirements(VulkanRenderer.device, bufferHandle, out var memRequirements);
            var allocInfo = new VkMemoryAllocateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_EXPORT_MEMORY_ALLOCATE_INFO,
                allocationSize = memRequirements.size,
                memoryTypeIndex = VulkanRenderer.GetMemoryType(memRequirements.memoryTypeBits, BufferProperties)
            };

            VkFunc.vkAllocateMemory(VulkanRenderer.device, &allocInfo, null, out VkDeviceMemory bufferMemory);
            return bufferMemory;
        }
        protected VkResult CreateStagingBuffer(void* bufferData)
        {
            return DLL_Buffer_CreateStagingBuffer(_device, _physicalDevice, _commandPool, _graphicsQueue, ref StagingBuffer, ref Buffer, ref StagingBufferMemory, ref BufferMemory, bufferData, BufferSize, BufferUsage, BufferProperties);
        }

        private VkResult UpdateBufferSize(VkBuffer buffer, ref VkDeviceMemory bufferMemory, ulong newBufferSize)
        {
            var result = DLL_Buffer_UpdateBufferSize(_device, _physicalDevice, buffer, ref bufferMemory, BufferData, ref BufferSize, newBufferSize, BufferUsage, BufferProperties);
            DestroyBuffer();
            return result;
        }

        public static VkResult CopyBuffer(ref VkBuffer srcBuffer, ref VkBuffer dstBuffer, ulong size)
        {
            return DLL_Buffer_CopyBuffer(srcBuffer, dstBuffer, size);
        }

        virtual public void UpdateBufferData(void* dataToCopy)
        {
            if (IsStagingBuffer)
            {
                void* stagingMappedData;
                void* mappedData;
                VkResult result = VkFunc.vkMapMemory(_device, StagingBufferMemory, 0, BufferSize, 0, &stagingMappedData);
                result = VkFunc.vkMapMemory(_device, BufferMemory, 0, BufferSize, 0, &mappedData);
                System.Buffer.MemoryCopy(dataToCopy, stagingMappedData, BufferSize, BufferSize);
                System.Buffer.MemoryCopy(stagingMappedData, mappedData, BufferSize, BufferSize);
                VkFunc.vkUnmapMemory(_device, BufferMemory);
                VkFunc.vkUnmapMemory(_device, StagingBufferMemory);
            }
            else
            {
                void* mappedData;
                var result = VkFunc.vkMapMemory(_device, BufferMemory, 0, BufferSize, 0, &mappedData);
                System.Buffer.MemoryCopy(dataToCopy, mappedData, BufferSize, BufferSize);
                VkFunc.vkUnmapMemory(_device, BufferMemory);
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

        public VkDescriptorBufferInfo* GetDescriptorBuffer()
        {
            DescriptorBufferInfo = new VkDescriptorBufferInfo
            {
                buffer = Buffer,
                offset = 0,
                range = Vk.WholeSize
            };

            var bufferInfo = DescriptorBufferInfo;
            return &bufferInfo;
        }

        public void DestroyBuffer()
        {
            DLL_Buffer_DestroyBuffer(_device, ref Buffer, ref StagingBuffer, ref BufferMemory, ref StagingBufferMemory, BufferData, ref BufferSize, ref BufferUsage, ref BufferProperties);
        }
    }
}