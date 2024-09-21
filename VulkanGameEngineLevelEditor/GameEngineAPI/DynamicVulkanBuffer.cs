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
    public unsafe class DynamicVulkanBuffer<T> : VulkanBuffer<T> where T : unmanaged
    {
        public DynamicVulkanBuffer(List<T> vertexList)
        {
        }

        public DynamicVulkanBuffer(T data, VkBufferUsageFlags usage, VkMemoryPropertyFlagBits properties) : base(data, usage, properties)
        {
            List<T> dataList = new List<T> { data };
            CreateBuffer(dataList, usage, properties);
        }

        public DynamicVulkanBuffer(List<T> dataList, VkBufferUsageFlags usage, VkMemoryPropertyFlagBits properties) : base(dataList, usage, properties)
        {
            CreateBuffer(dataList, usage, properties);
        }

        override protected VkResult CreateBuffer(List<T> dataList, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlagBits properties)
        {
            GCHandle dataListHandle = GCHandle.Alloc(dataList.ToArray(), GCHandleType.Pinned);
            IntPtr dataListPtr = dataListHandle.AddrOfPinnedObject();

            VkBuffer stagingBuffer = StagingBuffer;
            VkDeviceMemory stagingBufferMemory = StagingBufferMemory;

            GameEngineDLL.DLL_Buffer_CreateStagingBuffer(VulkanRenderer.Device, VulkanRenderer.PhysicalDevice, out stagingBuffer, out stagingBufferMemory, dataListPtr, BufferSize, bufferUsage, properties);

            StagingBuffer = stagingBuffer;
            StagingBufferMemory = stagingBufferMemory;

            dataListHandle.Free();
            return VkResult.VK_SUCCESS;
        }

        override protected VkResult CopyStagingBuffer(VkCommandBuffer commandBuffer)
        {
            return base.CopyStagingBuffer(commandBuffer);
        }

        override public VkResult UpdateBufferData(IntPtr bufferData)
        {
            return base.UpdateStagingBufferData(bufferData);
        }

    }
}
