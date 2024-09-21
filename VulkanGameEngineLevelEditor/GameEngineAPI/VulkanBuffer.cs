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
        public VkBuffer Buffer { get; protected set; }
        public VkBuffer StagingBuffer { get; protected set; }
        public VkDeviceMemory BufferMemory { get; protected set; }
        public VkDeviceMemory StagingBufferMemory { get; protected set; }
        public VkDeviceSize BufferSize { get; protected set; }
        public VkBufferUsageFlags BufferUsage { get; protected set; }
        public VkMemoryPropertyFlagBits BufferProperties { get; protected set; }
        public IntPtr BufferDeviceAddress { get; protected set; }
        public IntPtr BufferHandle { get; protected set; }
        public IntPtr BufferData { get; protected set; }
        public bool IsMapped { get; protected set; }
        public VkDescriptorBufferInfo DescriptorBufferInfo { get; protected set; }

        public VulkanBuffer()
        {
        }

        public VulkanBuffer(T data, VkBufferUsageFlags usage, VkMemoryPropertyFlagBits properties)
        {
            List<T> dataList = new List<T> { data };
            BufferSize = (UInt32)sizeof(T) * (UInt32)dataList.Count;
            BufferUsage = usage;
            BufferProperties = properties;
            CreateBuffer(dataList, usage, properties);
        }

        public VulkanBuffer(List<T> dataList, VkBufferUsageFlags usage, VkMemoryPropertyFlagBits properties)
        {
            BufferSize = (UInt32)sizeof(T) * (UInt32)dataList.Count;
            BufferUsage = usage;
            BufferProperties = properties;
            CreateBuffer(dataList, usage, properties);
        }

        public VulkanBuffer(IntPtr data, ulong bufferSize, VkBufferUsageFlags usage, VkMemoryPropertyFlagBits properties)
        {
            BufferSize = bufferSize;
            BufferUsage = usage;
            BufferProperties = properties;
            CreateBuffer(data, bufferSize, usage, properties);
        }

        private VkResult AllocateMemory(VkDevice device, VkPhysicalDevice physicalDevice, VkBuffer bufferData, ref VkDeviceMemory bufferMemory, VkMemoryPropertyFlagBits properties)
        {
            var bufferData2 = bufferData;
            var bufferMemory2 = bufferMemory;
           return GameEngineDLL.DLL_Buffer_AllocateMemory(device, VulkanRenderer.PhysicalDevice, out bufferData2, out bufferMemory2, properties);
        }

        virtual protected VkResult CreateBuffer(List<T> dataList, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlagBits properties)
        {
            GCHandle dataListHandle = GCHandle.Alloc(dataList.ToArray(), GCHandleType.Pinned);
            IntPtr dataListPtr = dataListHandle.AddrOfPinnedObject();

            VkBuffer buffer = new VkBuffer();
            VkDeviceMemory bufferMemory = new VkDeviceMemory();

            VkResult result = GameEngineDLL.DLL_Buffer_CreateBuffer(
                VulkanRenderer.Device,
                VulkanRenderer.PhysicalDevice,
                out buffer,
                out bufferMemory,
                dataListPtr,
                BufferSize,
                bufferUsage,
                properties);

            Buffer = buffer;
            BufferMemory = bufferMemory;

            dataListHandle.Free();

            return result;
        }

        virtual protected VkResult CreateBuffer(IntPtr bufferData, ulong bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlagBits properties)
        {
            VkBuffer buffer = new VkBuffer();
            VkDeviceMemory bufferMemory = new VkDeviceMemory();

            VkResult result = GameEngineDLL.DLL_Buffer_CreateBuffer(
                VulkanRenderer.Device,
                VulkanRenderer.PhysicalDevice,
                out buffer,
                out bufferMemory,
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
            return GameEngineDLL.DLL_Buffer_UpdateBufferSize(VulkanRenderer.Device, VulkanRenderer.PhysicalDevice, buffer, out bufferMemory, BufferData, out newBufferSize, BufferSize, BufferUsage, BufferProperties);
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
            var buffer = Buffer;
            var stagingBuffer = StagingBuffer;
            var bufferMemory = BufferMemory;
            var stagingBufferMemory = StagingBufferMemory;
            var bufferSize = BufferSize;
            var bufferUsage = BufferUsage;
            var bufferProperties = BufferProperties;
            GameEngineDLL.DLL_Buffer_DestroyBuffer(VulkanRenderer.Device, ref buffer, ref stagingBuffer, ref bufferMemory, ref stagingBufferMemory, BufferData, ref bufferSize, ref bufferUsage, ref bufferProperties);
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



