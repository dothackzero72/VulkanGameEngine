using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class DynamicVulkanBuffer<T> : VulkanBuffer<T> where T : unmanaged
    {
        public DynamicVulkanBuffer()
        {
        }

        public DynamicVulkanBuffer(IntPtr bufferData, uint bufferSize, VkBufferUsageFlags usage, VkMemoryPropertyFlagBits properties)

        {
            base.CreateBuffer(bufferData, bufferSize, usage, properties);
        }

        override protected VkResult CreateBuffer(IntPtr bufferData, ulong bufferSize, VkBufferUsageFlags bufferUsage, VkMemoryPropertyFlagBits properties)
        {
            VkBuffer stagingBuffer = StagingBuffer;
            VkDeviceMemory stagingBufferMemory = StagingBufferMemory;
            return GameEngineDLL.DLL_Buffer_CreateStagingBuffer(VulkanRenderer.Device, VulkanRenderer.PhysicalDevice, out stagingBuffer, out stagingBufferMemory, bufferData, bufferSize, bufferUsage, properties);
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
