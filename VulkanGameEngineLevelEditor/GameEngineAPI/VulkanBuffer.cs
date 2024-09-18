using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor;
using System.Runtime.InteropServices;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class VulkanBuffer<T> where T : unmanaged
    {
        public VkBuffer Buffer { get; private set; }
        public VkBuffer StagingBuffer { get; private set; }
        public VkDeviceMemory BufferMemory { get; private set; }
        public VkDeviceMemory StagingBufferMemory { get; private set; }
        public VkDeviceSize BufferSize { get; private set; }
        public VkBufferUsageFlags BufferUsage { get; private set; }
        public VkMemoryPropertyFlagBits BufferProperties { get; private set; }
        public IntPtr BufferDeviceAddress { get; private set; }
        public IntPtr BufferHandle { get; private set; }
        public IntPtr BufferData { get; private set; }
        public bool IsMapped { get; private set; }
        public VkDescriptorBufferInfo DescriptorBufferInfo { get; private set; }

        public VulkanBuffer()
        {
        }

        public VulkanBuffer(IntPtr bufferData, UInt32 bufferSize, VkBufferUsageFlags usage, VkMemoryPropertyFlagBits properties)
        {
            BufferSize = bufferSize;
            BufferUsage = usage;
            BufferProperties = properties;
            CreateBuffer(bufferData, bufferSize, usage, properties);
        }

        private VkResult AllocateMemory(VkDevice device, VkPhysicalDevice physicalDevice, ref VkBuffer bufferData, ref VkDeviceMemory bufferMemory, VkMemoryPropertyFlagBits properties)
        {
            var bufferData2 = bufferData;
            var bufferMemory2 = bufferMemory;
            return GameEngineDLL.DLL_Buffer_AllocateMemory(device, VulkanRenderer.PhysicalDevice, ref bufferData2, ref bufferMemory2, properties);
        }

        virtual protected VkResult CreateBuffer(IntPtr bufferData, ulong bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlagBits properties)
        {
            VkBuffer buffer = new VkBuffer();
            VkDeviceMemory bufferMemory = new VkDeviceMemory();

            VkResult result = GameEngineDLL.DLL_Buffer_CreateBuffer(
                VulkanRenderer.Device,
                VulkanRenderer.PhysicalDevice,
                ref buffer,
                ref bufferMemory,
                bufferData,
                bufferSize,
                bufferUsage,
                properties);

            Buffer = buffer;
            BufferMemory = bufferMemory;

            return result;
        }

        virtual protected VkResult CreateStagingBuffer()
        {
            VkBuffer stagingBuffer = StagingBuffer;
            VkDeviceMemory stagingBufferMemory = StagingBufferMemory;
            var result = GameEngineDLL.DLL_Buffer_CreateStagingBuffer(VulkanRenderer.Device, VulkanRenderer.PhysicalDevice, out stagingBuffer, out stagingBufferMemory, BufferData, BufferSize, BufferUsage, BufferProperties);
            return result;
        }

        virtual protected VkResult CopyStagingBuffer(IntPtr commandBuffer)
        {
            return GameEngineDLL.DLL_Buffer_CopyStagingBuffer(commandBuffer, StagingBuffer, Buffer, BufferSize);
        }

        virtual protected VkResult UpdateBufferSize(ulong newBufferSize)
        {
            var buffer = Buffer;
            var bufferMemory = BufferMemory;
            return GameEngineDLL.DLL_Buffer_UpdateBufferSize(VulkanRenderer.Device, VulkanRenderer.PhysicalDevice, out buffer, out bufferMemory, BufferData, ref newBufferSize, BufferSize, BufferUsage, BufferProperties);
        }

        virtual protected VkResult UpdateStagingBufferData(IntPtr bufferData)
        {
            return GameEngineDLL.DLL_Buffer_UpdateStagingBufferMemory(VulkanRenderer.Device, StagingBufferMemory, bufferData, (uint)sizeof(T));
        }

        virtual public VkResult UpdateBufferData(IntPtr bufferData)
        {
            return GameEngineDLL.DLL_Buffer_UpdateBufferMemory(VulkanRenderer.Device, BufferMemory, bufferData, (uint)sizeof(T));
        }

        public VkResult CopyBuffer(VkBuffer srcBuffer, VkBuffer dstBuffer, VkDeviceSize size)
        {
            return GameEngineDLL.DLL_Buffer_CopyBuffer(srcBuffer, dstBuffer, size);
        }

        public void DestroyBuffer()
        {
            var bufferSize = BufferSize;
            var bufferUsage = BufferUsage;
            var bufferProperties = BufferProperties;
            GameEngineDLL.DLL_Buffer_DestroyBuffer(Buffer, BufferMemory, BufferData, ref bufferSize, ref bufferUsage, ref bufferProperties);
        }

        public VkDescriptorBufferInfo GetDescriptorbuffer()
        {
            return new VkDescriptorBufferInfo
            {
                buffer = Buffer,
                offset = 0,
                range = VulkanConsts.VK_WHOLE_SIZE
            };
        }

        public List<T> CheckBufferContents()
        {
            List<T> DataList = new List<T>();
            ulong dataListSize = BufferSize / (ulong)sizeof(T);

            var isMapped = IsMapped;
            IntPtr data = GameEngineDLL.DLL_Buffer_MapBufferMemory(VulkanRenderer.Device, BufferMemory, BufferSize, out isMapped);
            if (data == IntPtr.Zero)
            {
                return DataList;
            }

            for (int x = 0; x < (int)dataListSize; ++x)
            {
                IntPtr newPtr = IntPtr.Add(data, x * (int)sizeof(T));
                T item = Marshal.PtrToStructure<T>(newPtr);
                DataList.Add(item);
            }

            GameEngineDLL.DLL_Buffer_UnmapBufferMemory(VulkanRenderer.Device, BufferMemory, out isMapped);

            return DataList;
        }
    }
}



