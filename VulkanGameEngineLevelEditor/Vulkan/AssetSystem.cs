using GlmSharp;
using Silk.NET.SDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.GameEngineAPI;

namespace VulkanGameEngineLevelEditor.Vulkan
{
    public unsafe class MaterialStruct
    {
        public string Name { get; set; }
        public Guid MaterialId { get; set; }
        public uint MaterialBufferIndex { get; set; } = 0;

        public vec3 Albedo { get; set; } = new vec3(0.0f, 0.35f, 0.45f);
        public float Metallic { get; set; } = 0.0f;
        public float Roughness { get; set; } = 0.0f;
        public float AmbientOcclusion { get; set; } = 1.0f;
        public vec3 Emission { get; set; } = new vec3(0.0f);
        public float Alpha { get; set; } = 1.0f;

        public Guid AlbedoMapId { get; set; }
        public Guid MetallicRoughnessMapId { get; set; }
        public Guid MetallicMapId { get; set; }
        public Guid RoughnessMapId { get; set; }
        public Guid AmbientOcclusionMapId { get; set; }
        public Guid NormalMapId { get; set; }
        public Guid DepthMapId { get; set; }
        public Guid AlphaMapId { get; set; }
        public Guid EmissionMapId { get; set; }
        public Guid HeightMapId { get; set; }

        public VulkanBuffer<MaterialProperitiesBuffer> MaterialBuffer { get; private set; }

        public MaterialStruct()
        {

        }

        public MaterialStruct(string materialName, Guid materialId)
        {
            Name = materialName;
            MaterialId = materialId;
            MaterialBufferIndex = 0;

            GCHandle handle = GCHandle.Alloc(MaterialBuffer, GCHandleType.Pinned);
            void* ptr = handle.AddrOfPinnedObject().ToPointer();
            MaterialBuffer = new VulkanBuffer<MaterialProperitiesBuffer>(new MaterialProperitiesBuffer(), VkBufferUsageFlagBits.VK_BUFFER_USAGE_SHADER_DEVICE_ADDRESS_BIT |
                                                                                                          VkBufferUsageFlagBits.VK_BUFFER_USAGE_STORAGE_BUFFER_BIT,
                                                                                                          VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT |
                                                                                                          VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT, false);

        }
        
        public void UpdateMaterialBufferIndex(uint bufferIndex)
        {
            MaterialBufferIndex = bufferIndex;
        }

        public void UpdateBuffer()
        {
            MaterialProperitiesBuffer materialBuffer = new MaterialProperitiesBuffer
            {
		        AlbedoMapId = AlbedoMapId != new Guid() ? TextureSystem.TextureList[AlbedoMapId].textureBufferIndex : 0,
		        MetallicRoughnessMapId = MetallicRoughnessMapId != new Guid() ? TextureSystem.TextureList[MetallicRoughnessMapId].textureBufferIndex : 0,
		        MetallicMapId = MetallicMapId != new Guid() ? TextureSystem.TextureList[MetallicMapId].textureBufferIndex : 0,
		        RoughnessMapId = RoughnessMapId != new Guid() ? TextureSystem.TextureList[RoughnessMapId].textureBufferIndex : 0,
		        AmbientOcclusionMapId = AmbientOcclusionMapId != new Guid() ? TextureSystem.TextureList[AmbientOcclusionMapId].textureBufferIndex : 0,
		        NormalMapId = NormalMapId != new Guid() ? TextureSystem.TextureList[NormalMapId].textureBufferIndex : 0,
		        DepthMapId = DepthMapId != new Guid() ? TextureSystem.TextureList[DepthMapId].textureBufferIndex : 0,
		        AlphaMapId = AlphaMapId != new Guid() ? TextureSystem.TextureList[AlphaMapId].textureBufferIndex : 0,
		        EmissionMapId = EmissionMapId != new Guid() ? TextureSystem.TextureList[EmissionMapId].textureBufferIndex : 0,
		        HeightMapId = HeightMapId != new Guid() ? TextureSystem.TextureList[HeightMapId].textureBufferIndex : 0

            };

            GCHandle handle = GCHandle.Alloc(MaterialBuffer, GCHandleType.Pinned);
            void* ptr = handle.AddrOfPinnedObject().ToPointer();
            //MaterialBuffer.UpdateBufferMemory(cRenderer, materialBuffer);
        }
    }

    public static class AssetSystem
    {
        public static Dictionary<Guid, Material> MaterialList { get; private set; } = new Dictionary<Guid, Material>();
        public static Guid LoadMaterial(string materialPath)
        {
            return new Guid();
        }
    }
}
