using GlmSharp;
using Silk.NET.SDL;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.GameEngineAPI;


namespace VulkanGameEngineLevelEditor.Systems
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

    public static unsafe class BufferSystem
    {
        public static uint NextBufferId = 0;
        public static Dictionary<uint, VulkanBuffer> VulkanBufferMap = new Dictionary<uint, VulkanBuffer>();

        private static BufferTypeEnum GetBufferType<T>()
        {
            if (typeof(T) == typeof(uint)) { return BufferTypeEnum.BufferType_UInt; }
            else if (typeof(T) == typeof(mat4)) { return BufferTypeEnum.BufferType_Mat4; }
            else if (typeof(T) == typeof(MaterialProperitiesBuffer)) { return BufferTypeEnum.BufferType_MaterialProperitiesBuffer; }
            //else if (typeof(T) == typeof(MeshPropertiesStruct)) { return BufferTypeEnum.BufferType_MeshPropertiesStruct; }
            else if (typeof(T) == typeof(SpriteInstanceStruct)) { return BufferTypeEnum.BufferType_SpriteInstanceStruct; }
            else if (typeof(T) == typeof(Vertex2D)) { return BufferTypeEnum.BufferType_Vector2D; }
            else
            {
                throw new Exception("Buffer Type doesn't exist");
            }
        }

        public static uint CreateVulkanBuffer<T>(GraphicsRenderer renderer, T bufferData, VkBufferUsageFlagBits usage, VkMemoryPropertyFlagBits properties, bool usingStagingBuffer)
        {

            BufferTypeEnum bufferTypeEnum = GetBufferType<T>();
            VkDeviceSize bufferElementSize = (ulong)sizeof(T);
            uint bufferElementCount = 1;

            uint nextBufferId = ++NextBufferId;
            VulkanBufferMap[nextBufferId] = VulkanBuffer_CreateVulkanBuffer(renderer, nextBufferId, &bufferData, bufferElementSize, bufferElementCount, bufferTypeEnum, usage, properties, usingStagingBuffer);
            return NextBufferId;
        }

        public static uint CreateVulkanBuffer<T>(GraphicsRenderer renderer, List<T> bufferData, VkBufferUsageFlagBits usage, VkMemoryPropertyFlagBits properties, bool usingStagingBuffer)
        {
            GCHandle handle = GCHandle.Alloc(bufferData.ToArray(), GCHandleType.Pinned);
            try
            {
                BufferTypeEnum bufferTypeEnum = GetBufferType<T>();
                VkDeviceSize bufferElementSize = (ulong)sizeof(T);
                uint bufferElementCount = bufferData.UCount();

                uint nextBufferId = ++NextBufferId;
                VulkanBufferMap[nextBufferId] = VulkanBuffer_CreateVulkanBuffer(renderer, nextBufferId, (void*)handle.AddrOfPinnedObject(), bufferElementSize, bufferElementCount, bufferTypeEnum, usage, properties, usingStagingBuffer);
                return NextBufferId;
            }
            finally
            {
                handle.Free();
            }
        }

        public static void UpdateBufferMemory<T>(GraphicsRenderer renderer, uint bufferId, T bufferData)
        {
            BufferTypeEnum bufferTypeEnum = GetBufferType<T>();
            if (VulkanBufferMap[bufferId].BufferType != bufferTypeEnum)
            {
                //throw std::runtime_error("Buffer type doesn't match");
            }

            VkDeviceSize bufferElementSize = (ulong)sizeof(T);
            uint bufferElementCount = 1;

            VulkanBuffer_UpdateBufferMemory(renderer, VulkanBufferMap[bufferId], &bufferData, bufferElementSize, bufferElementCount);
        }

        public static void UpdateBufferMemory<T>(GraphicsRenderer renderer, uint bufferId, List<T> bufferData)
        {
            GCHandle handle = GCHandle.Alloc(bufferData.ToArray(), GCHandleType.Pinned);
            try
            {
                BufferTypeEnum bufferTypeEnum = GetBufferType<T>();
                if (VulkanBufferMap[bufferId].BufferType != bufferTypeEnum)
                {
                    // throw std::runtime_error("Buffer type doesn't match");
                }

                VkDeviceSize bufferElementSize = (VkDeviceSize)sizeof(T);
                uint bufferElementCount = bufferData.UCount();

                VulkanBuffer_UpdateBufferMemory(renderer, VulkanBufferMap[bufferId], (void*)handle.AddrOfPinnedObject(), bufferElementSize, bufferElementCount);
            }
            finally
            {
                handle.Free();
            }
        }

        public static void DestroyBuffer(GraphicsRenderer renderer, uint vulkanBufferId)
        {
            VulkanBuffer_DestroyBuffer(renderer, VulkanBufferMap[vulkanBufferId]);
        }

        public static void DestroyAllBuffers()
        {
            foreach (var buffer in VulkanBufferMap)
            {
                VulkanBuffer_DestroyBuffer(RenderSystem.renderer, buffer.Value);
            }
        }

        public static VkResult CopyBuffer(GraphicsRenderer renderer, VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size)
        {
            return VulkanBuffer_CopyBuffer(renderer, srcBuffer, dstBuffer, size);
        }

        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VulkanBuffer VulkanBuffer_CreateVulkanBuffer(GraphicsRenderer renderer, uint bufferId, VkDeviceSize bufferElementSize, uint bufferElementCount, BufferTypeEnum bufferTypeEnum, VkBufferUsageFlagBits usage, VkMemoryPropertyFlagBits properties, bool usingStagingBuffer);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VulkanBuffer VulkanBuffer_CreateVulkanBuffer(GraphicsRenderer renderer, uint bufferId, void* bufferData, VkDeviceSize bufferElementSize, uint bufferElementCount, BufferTypeEnum bufferTypeEnum, VkBufferUsageFlagBits usage, VkMemoryPropertyFlagBits properties, bool usingStagingBuffer);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VulkanBuffer VulkanBuffer_CreateVulkanBuffer(GraphicsRenderer renderer, VulkanBuffer vulkanBuffer, uint bufferId, void* bufferData, VkDeviceSize bufferElementSize, uint bufferElementCount, BufferTypeEnum bufferTypeEnum, VkBufferUsageFlagBits usage, VkMemoryPropertyFlagBits properties, bool usingStagingBuffer);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern void VulkanBuffer_UpdateBufferMemory(GraphicsRenderer renderer, VulkanBuffer vulkanBuffer, void* bufferData, VkDeviceSize bufferElementSize, uint bufferElementCount);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern VkResult VulkanBuffer_CopyBuffer(GraphicsRenderer renderer, VkBuffer* srcBuffer, VkBuffer* dstBuffer, VkDeviceSize size);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern void VulkanBuffer_DestroyBuffer(GraphicsRenderer renderer, VulkanBuffer vulkanBuffer);
    }
}
