using GlmSharp;
using Silk.NET.Vulkan;
using SixLabors.ImageSharp.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public struct MaterialProperitiesBuffer
    {
        public vec3 Albedo { get; set; } = new vec3(0.0f, 0.35f, 0.45f);
        public float Metallic { get; set; } = 0.0f;
        public float Roughness { get; set; } = 0.0f;
        public float AmbientOcclusion { get; set; } = 1.0f;
        public vec3 Emission { get; set; } = new vec3(0.0f);
        public float Alpha { get; set; } = 1.0f;

        public uint AlbedoMap { get; set; } = uint.MaxValue;
        public uint MetallicRoughnessMap { get; set; } = uint.MaxValue;
        public uint MetallicMap { get; set; } = uint.MaxValue;
        public uint RoughnessMap { get; set; } = uint.MaxValue;
        public uint AmbientOcclusionMap { get; set; } = uint.MaxValue;
        public uint NormalMap { get; set; } = uint.MaxValue;
        public uint DepthMap { get; set; } = uint.MaxValue;
        public uint AlphaMap { get; set; } = uint.MaxValue;
        public uint EmissionMap { get; set; } = uint.MaxValue;
        public uint HeightMap { get; set; } = uint.MaxValue;

        public MaterialProperitiesBuffer()
        {
        }
    };

    public unsafe class Material
    {
        private static uint NextMaterialId = 0;
        public uint MaterialID { get; private set; } = 0;
        public uint MaterialBufferIndex { get; private set; } = 0;
        String Name { get; set; }
        
        public VulkanBuffer<MaterialProperitiesBuffer> MaterialBuffer { get; private set; }
        public MaterialProperitiesBuffer MaterialInfo { get; set; } = new MaterialProperitiesBuffer();

        public vec3 Albedo { get; set; } = new vec3(0.0f, 0.35f, 0.45f);
        public float Metallic { get; set; } = 0.0f;
        public float Roughness { get; set; } = 0.0f;
        public float AmbientOcclusion { get; set; } = 1.0f; 
        public vec3 Emission { get; set; } = new vec3(0.0f);
        public float Alpha { get; set; } = 1.0f;

        public Texture AlbedoMap { get; set; } = new Texture();
        public Texture MetallicRoughnessMap { get; set; } = new Texture();
        public Texture MetallicMap { get; set; } = new Texture();
        public Texture RoughnessMap { get; set; } = new Texture();
        public Texture AmbientOcclusionMap { get; set; } = new Texture();
        public Texture NormalMap { get; set; } = new Texture();
        public Texture DepthMap { get; set; } = new Texture();
        public Texture AlphaMap { get; set; } = new Texture();
        public Texture EmissionMap { get; set; } = new Texture();
        public Texture HeightMap { get; set; } = new Texture();

        public Material()
        {

        }

        public Material(string materialName)
        {
            Name = materialName;
            MaterialID = ++NextMaterialId;
            MaterialBufferIndex = 0;

            GCHandle handle = GCHandle.Alloc(MaterialInfo, GCHandleType.Pinned);
            void* ptr = handle.AddrOfPinnedObject().ToPointer();

            MaterialBuffer = new VulkanBuffer<MaterialProperitiesBuffer>(MaterialInfo, VkBufferUsageFlagBits.VK_BUFFER_USAGE_SHADER_DEVICE_ADDRESS_BIT |
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
            MaterialInfo = new MaterialProperitiesBuffer
            {
                Albedo = MaterialInfo.Albedo,
                Metallic = MaterialInfo.Metallic,
                Roughness = MaterialInfo.Roughness,
                AmbientOcclusion = MaterialInfo.AmbientOcclusion,
                Emission = MaterialInfo.Emission,
                Alpha = MaterialInfo.Alpha,
                AlbedoMap = MaterialInfo.AlbedoMap,
                MetallicRoughnessMap = MaterialInfo.MetallicRoughnessMap,
                MetallicMap = MaterialInfo.MetallicMap,
                RoughnessMap = MaterialInfo.RoughnessMap,
                AmbientOcclusionMap = MaterialInfo.AmbientOcclusionMap,
                NormalMap = MaterialInfo.NormalMap,
                DepthMap = MaterialInfo.DepthMap,
                AlphaMap = MaterialInfo.AlphaMap,
                EmissionMap = MaterialInfo.EmissionMap,
                HeightMap = MaterialInfo.HeightMap
            };

            GCHandle handle = GCHandle.Alloc(MaterialInfo, GCHandleType.Pinned);
            void* ptr = handle.AddrOfPinnedObject().ToPointer();
            MaterialBuffer.UpdateBufferMemory(ptr, (ulong)sizeof(MaterialProperitiesBuffer));
        }

        public void Destroy()
        {
            //AlbedoMap.reset();
            //MetallicRoughnessMap.reset();
            //MetallicMap.reset();
            //RoughnessMap.reset();
            //AmbientOcclusionMap.reset();
            //NormalMap.reset();
            //DepthMap.reset();
            //AlphaMap.reset();
            //EmissionMap.reset();
            //HeightMap.reset();
            MaterialBuffer.DestroyBuffer();
        }

        public void SetAlbedo(vec3 color)
        {
            Albedo = color;

            var updatedInfo = MaterialInfo;
            updatedInfo.Albedo = color; 
            MaterialInfo = updatedInfo; 

            GCHandle handle = GCHandle.Alloc(MaterialInfo, GCHandleType.Pinned);
            void* ptr = handle.AddrOfPinnedObject().ToPointer();
            MaterialBuffer.UpdateBufferMemory(ptr, (ulong)sizeof(MaterialProperitiesBuffer));
        }

        public void SetMetallic(float value)
        {
            Metallic = value;

            var updatedInfo = MaterialInfo;
            updatedInfo.Metallic = value;
            MaterialInfo = updatedInfo;

            GCHandle handle = GCHandle.Alloc(MaterialInfo, GCHandleType.Pinned);
            void* ptr = handle.AddrOfPinnedObject().ToPointer();
            MaterialBuffer.UpdateBufferMemory(ptr, (ulong)sizeof(MaterialProperitiesBuffer));
        }

        public void SetRoughness(float value)
        {
            Roughness = value;

            var updatedInfo = MaterialInfo;
            updatedInfo.Roughness = value;
            MaterialInfo = updatedInfo;

            GCHandle handle = GCHandle.Alloc(MaterialInfo, GCHandleType.Pinned);
            void* ptr = handle.AddrOfPinnedObject().ToPointer();
            MaterialBuffer.UpdateBufferMemory(ptr, (ulong)sizeof(MaterialProperitiesBuffer));
        }

        public void SetAmbientOcclusion(float value)
        {
            AmbientOcclusion = value;

            var updatedInfo = MaterialInfo;
            updatedInfo.AmbientOcclusion = value;
            MaterialInfo = updatedInfo;

            GCHandle handle = GCHandle.Alloc(MaterialInfo, GCHandleType.Pinned);
            void* ptr = handle.AddrOfPinnedObject().ToPointer();
            MaterialBuffer.UpdateBufferMemory(ptr, (ulong)sizeof(MaterialProperitiesBuffer));
        }

        public void SetEmission(vec3 color)
        {
            Emission = color;

            var updatedInfo = MaterialInfo;
            updatedInfo.Emission = color;
            MaterialInfo = updatedInfo;

            GCHandle handle = GCHandle.Alloc(MaterialInfo, GCHandleType.Pinned);
            void* ptr = handle.AddrOfPinnedObject().ToPointer();
            MaterialBuffer.UpdateBufferMemory(ptr, (ulong)sizeof(MaterialProperitiesBuffer));
        }

        public void SetAlpha(float value)
        {
            Alpha = value;

            var updatedInfo = MaterialInfo;
            updatedInfo.Alpha = value;
            MaterialInfo = updatedInfo;

            GCHandle handle = GCHandle.Alloc(MaterialInfo, GCHandleType.Pinned);
            void* ptr = handle.AddrOfPinnedObject().ToPointer();
            MaterialBuffer.UpdateBufferMemory(ptr, (ulong)sizeof(MaterialProperitiesBuffer));
        }

        public void SetAlbedoMap(Texture texture)
        {
            AlbedoMap = texture;

            var updatedInfo = MaterialInfo;
            updatedInfo.AlbedoMap = AlbedoMap.TextureBufferIndex;
            MaterialInfo = updatedInfo;

            GCHandle handle = GCHandle.Alloc(MaterialInfo, GCHandleType.Pinned);
            void* ptr = handle.AddrOfPinnedObject().ToPointer();
            MaterialBuffer.UpdateBufferMemory(ptr, (ulong)sizeof(MaterialProperitiesBuffer));
        }

        public void SetMetallicRoughnessMap(Texture texture)
        {
            MetallicRoughnessMap = texture;

            var updatedInfo = MaterialInfo;
            updatedInfo.MetallicRoughnessMap = MetallicRoughnessMap.TextureBufferIndex;
            MaterialInfo = updatedInfo;

            GCHandle handle = GCHandle.Alloc(MaterialInfo, GCHandleType.Pinned);
            void* ptr = handle.AddrOfPinnedObject().ToPointer();
            MaterialBuffer.UpdateBufferMemory(ptr, (ulong)sizeof(MaterialProperitiesBuffer));
        }

        public void SetMetallicMap(Texture texture)
        {
            MetallicMap = texture;

            var updatedInfo = MaterialInfo;
            updatedInfo.MetallicMap = MetallicMap.TextureBufferIndex;
            MaterialInfo = updatedInfo;

            GCHandle handle = GCHandle.Alloc(MaterialInfo, GCHandleType.Pinned);
            void* ptr = handle.AddrOfPinnedObject().ToPointer();
            MaterialBuffer.UpdateBufferMemory(ptr, (ulong)sizeof(MaterialProperitiesBuffer));
        }

        public void SetRoughnessMap(Texture texture)
        {
            RoughnessMap = texture;

            var updatedInfo = MaterialInfo;
            updatedInfo.RoughnessMap = RoughnessMap.TextureBufferIndex;
            MaterialInfo = updatedInfo;

            GCHandle handle = GCHandle.Alloc(MaterialInfo, GCHandleType.Pinned);
            void* ptr = handle.AddrOfPinnedObject().ToPointer();
            MaterialBuffer.UpdateBufferMemory(ptr, (ulong)sizeof(MaterialProperitiesBuffer));
        }

        public void SetAmbientOcclusionMap(Texture texture)
        {
            AmbientOcclusionMap = texture;

            var updatedInfo = MaterialInfo;
            updatedInfo.AmbientOcclusionMap = AmbientOcclusionMap.TextureBufferIndex;
            MaterialInfo = updatedInfo;

            GCHandle handle = GCHandle.Alloc(MaterialInfo, GCHandleType.Pinned);
            void* ptr = handle.AddrOfPinnedObject().ToPointer();
            MaterialBuffer.UpdateBufferMemory(ptr, (ulong)sizeof(MaterialProperitiesBuffer));
        }

        public void SetNormalMap(Texture texture)
        {
            NormalMap = texture;

            var updatedInfo = MaterialInfo;
            updatedInfo.NormalMap = NormalMap.TextureBufferIndex;
            MaterialInfo = updatedInfo;

            GCHandle handle = GCHandle.Alloc(MaterialInfo, GCHandleType.Pinned);
            void* ptr = handle.AddrOfPinnedObject().ToPointer();
            MaterialBuffer.UpdateBufferMemory(ptr, (ulong)sizeof(MaterialProperitiesBuffer));
        }

        public void SetDepthMap(Texture texture)
        {
            DepthMap = texture;

            var updatedInfo = MaterialInfo;
            updatedInfo.DepthMap = DepthMap.TextureBufferIndex;
            MaterialInfo = updatedInfo;

            GCHandle handle = GCHandle.Alloc(MaterialInfo, GCHandleType.Pinned);
            void* ptr = handle.AddrOfPinnedObject().ToPointer();
            MaterialBuffer.UpdateBufferMemory(ptr, (ulong)sizeof(MaterialProperitiesBuffer));
        }

        public void SetAlphaMap(Texture texture)
        {
            AlphaMap = texture;

            var updatedInfo = MaterialInfo;
            updatedInfo.AlphaMap = AlphaMap.TextureBufferIndex;
            MaterialInfo = updatedInfo;

            GCHandle handle = GCHandle.Alloc(MaterialInfo, GCHandleType.Pinned);
            void* ptr = handle.AddrOfPinnedObject().ToPointer();
            MaterialBuffer.UpdateBufferMemory(ptr, (ulong)sizeof(MaterialProperitiesBuffer));
        }

        public void SetEmissionMap(Texture texture)
        {
            EmissionMap = texture;

            var updatedInfo = MaterialInfo;
            updatedInfo.EmissionMap = EmissionMap.TextureBufferIndex;
            MaterialInfo = updatedInfo;

            GCHandle handle = GCHandle.Alloc(MaterialInfo, GCHandleType.Pinned);
            void* ptr = handle.AddrOfPinnedObject().ToPointer();
            MaterialBuffer.UpdateBufferMemory(ptr, (ulong)sizeof(MaterialProperitiesBuffer));
        }

        public void SetHeightMap(Texture texture)
        {
            HeightMap = texture;

            var updatedInfo = MaterialInfo;
            updatedInfo.HeightMap = HeightMap.TextureBufferIndex;
            MaterialInfo = updatedInfo;

            GCHandle handle = GCHandle.Alloc(MaterialInfo, GCHandleType.Pinned);
            void* ptr = handle.AddrOfPinnedObject().ToPointer();
            MaterialBuffer.UpdateBufferMemory(ptr, (ulong)sizeof(MaterialProperitiesBuffer));
        }

        public VkDescriptorBufferInfo GetMaterialPropertiesBuffer()
        {
            UpdateBuffer();
            return new VkDescriptorBufferInfo
            {
                buffer = MaterialBuffer.Buffer,
                offset = 0,
                range = UInt64.MaxValue
            };
        }
    }
}
