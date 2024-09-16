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

        override public VkResult CreateBuffer(IntPtr bufferData, uint bufferSize, VkBufferUsageFlags usage, VkMemoryPropertyFlagBits properties)
        {
            VulkanBufferInfo vulkanBufferInfo = SendCBufferInfo();
            return GameEngineDLL.DLL_Buffer_CreateStagingBuffer(ref vulkanBufferInfo, VulkanRenderer.Device, bufferData, bufferSize, usage, properties);
        }

        public void UpdateBuffer(ref VkCommandBuffer commandBuffer, IntPtr data)
        {
            base.UpdateBufferData(data);

            var CommandBuffer = commandBuffer;
            CopyStagingBuffer(CommandBuffer);
        }

        override public void UpdateBufferData(IntPtr bufferData)
        {
            base.UpdateBufferData(bufferData);
        }

        public VkResult CopyStagingBuffer(VkCommandBuffer commandBuffer)
        {
            VulkanBufferInfo vulkanBufferInfo = SendCBufferInfo();
            return GameEngineDLL.DLL_Buffer_CopyStagingBuffer(ref vulkanBufferInfo, commandBuffer, StagingBuffer, Buffer, BufferSize);
        }
    }
}
