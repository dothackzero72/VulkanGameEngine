using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngine.Structs
{
    public enum BufferTypeEnum
    {
        BufferType_Undefined,
        BufferType_UInt,
        BufferType_Mat4,
        BufferType_MaterialProperitiesBuffer,
        BufferType_SpriteInstanceStruct,
        BufferType_MeshPropertiesStruct,
        BufferType_SpriteMesh,
        BufferType_LevelLayerMesh,
        BufferType_Material,
        BufferType_Vector2D
    };

    public unsafe struct VulkanBuffer
    {
        public uint BufferId { get; set; } = 0;
        public VkBuffer Buffer { get; set; } = VulkanCSConst.VK_NULL_HANDLE;
        public VkBuffer StagingBuffer { get; set; } = VulkanCSConst.VK_NULL_HANDLE;
        public VkDeviceMemory StagingBufferMemory { get; set; } = VulkanCSConst.VK_NULL_HANDLE;
        public VkDeviceMemory BufferMemory { get; set; } = VulkanCSConst.VK_NULL_HANDLE;
        public VkDeviceSize BufferSize { get; set; } = 0;
        public VkBufferUsageFlagBits BufferUsage { get; set; } = 0;
        public VkMemoryPropertyFlagBits BufferProperties { get; set; } = 0;
        public VkDeviceSize BufferDeviceAddress { get; set; } = 0;
        public nint VkAccelerationStructureKHR { get; set; } = VulkanCSConst.VK_NULL_HANDLE;
        public BufferTypeEnum BufferType { get; set; } = BufferTypeEnum.BufferType_Undefined;
        public void* BufferData { get; set; } = null;
        public bool IsMapped { get; set; } = false;
        public bool UsingStagingBuffer { get; set; } = false;
        public VulkanBuffer()
        {
        }
    };

}
