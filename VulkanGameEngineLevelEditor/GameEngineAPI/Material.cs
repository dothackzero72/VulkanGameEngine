using CSScripting;
using GlmSharp;
using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;
using VulkanGameEngineLevelEditor.Models;
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
        [JsonIgnore]
        public uint MaterialBufferIndex { get; private set; } = 0;
        [JsonIgnore]
        public VulkanBuffer<MaterialProperitiesBuffer> MaterialBuffer { get; private set; }
        [JsonIgnore]
        public MaterialProperitiesBuffer MaterialInfo { get; set; } = new MaterialProperitiesBuffer();
        [JsonIgnore]
        public Texture AlbedoMap { get; set; }
        [JsonIgnore]
        public Texture MetallicRoughnessMap { get; set; }
        [JsonIgnore]
        public Texture MetallicMap { get; set; }
        [JsonIgnore]
        public Texture RoughnessMap { get; set; }
        [JsonIgnore]
        public Texture AmbientOcclusionMap { get; set; }
        [JsonIgnore]
        public Texture NormalMap { get; set; }
        [JsonIgnore]
        public Texture DepthMap { get; set; }
        [JsonIgnore]
        public Texture AlphaMap { get; set; }
        [JsonIgnore]
        public Texture EmissionMap { get; set; }
        [JsonIgnore]
        public Texture HeightMap { get; set; }


        public Guid MaterialID { get; private set; }
        public string Name { get; set; } = string.Empty;
        public string AlbedoMapPath { get; set; }
        public string MetallicRoughnessMapPath { get; set; }
        public string MetallicMapPath { get; set; }
        public string RoughnessMapPath { get; set; }
        public string AmbientOcclusionMapPath { get; set; }
        public string NormalMapPath { get; set; }
        public string DepthMapPath { get; set; }
        public string AlphaMapPath { get; set; }
        public string EmissionMapPath { get; set; }
        public string HeightMapPath { get; set; }
        public vec3 Albedo { get; set; } = new vec3(0.0f, 0.35f, 0.45f);
        public float Metallic { get; set; } = 0.0f;
        public float Roughness { get; set; } = 0.0f;
        public float AmbientOcclusion { get; set; } = 1.0f;
        public vec3 Emission { get; set; } = new vec3(0.0f);
        public float Alpha { get; set; } = 1.0f;

        public Material()
        {

        }

        public Material(MaterialModel model)
        {
            Name = model.Name;
            MaterialID = new Guid(model.MaterialID);
            MaterialBufferIndex = 0;

            GCHandle handle = GCHandle.Alloc(MaterialInfo, GCHandleType.Pinned);
            void* ptr = handle.AddrOfPinnedObject().ToPointer();

            MaterialBuffer = new VulkanBuffer<MaterialProperitiesBuffer>(MaterialInfo, VkBufferUsageFlagBits.VK_BUFFER_USAGE_SHADER_DEVICE_ADDRESS_BIT |
                                                                                       VkBufferUsageFlagBits.VK_BUFFER_USAGE_STORAGE_BUFFER_BIT,
                                                                                       VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT |
                                                                                       VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT, false);

            Albedo = model.Albedo;
            Metallic = model.Metallic;
            Roughness = model.Roughness;
            AmbientOcclusion = model.AmbientOcclusion;
            Emission = model.Emission;
            Alpha = model.Alpha;

            if (!model.AlbedoMapPath.IsEmpty())
            {
                SetAlbedoMap(model.AlbedoMapPath);
            }
            if (!model.MetallicMapPath.IsEmpty())
            {
                SetMetallicRoughnessMap(model.MetallicMapPath);
            }
            if (!model.MetallicMapPath.IsEmpty())
            {
                SetMetallicMap(model.MetallicMapPath);
            }
            if (!model.RoughnessMapPath.IsEmpty())
            {
                SetRoughnessMap(model.RoughnessMapPath);
            }
            if (!model.MetallicMapPath.IsEmpty())
            {
                SetAmbientOcclusionMap(model.AmbientOcclusionMapPath);
            }
            if (!model.NormalMapPath.IsEmpty())
            {
                SetNormalMap(model.NormalMapPath);
            }
            if (!model.DepthMapPath.IsEmpty())
            {
                SetDepthMap(model.DepthMapPath);
            }
            if (!model.AlphaMapPath.IsEmpty())
            {
                SetAlphaMap(model.AlphaMapPath);
            }
            if (!model.EmissionMapPath.IsEmpty())
            {
                SetEmissionMap(model.EmissionMapPath);
            }
            if (!model.HeightMapPath.IsEmpty())
            {
                SetHeightMap(model.HeightMapPath);
            }
        }

        public Material(string materialName)
        {
            Name = materialName;
            MaterialID = Guid.NewGuid();
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

        public void SetAlbedoMap(string texturePath)
        {
            AlbedoMapPath = texturePath;
            AlbedoMap = new Texture(texturePath, VkFormat.VK_FORMAT_R8G8B8A8_SRGB, VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, TextureTypeEnum.kType_DiffuseTextureMap, false);

            var updatedInfo = MaterialInfo;
            updatedInfo.AlbedoMap = AlbedoMap.TextureBufferIndex;
            MaterialInfo = updatedInfo;

            GCHandle handle = GCHandle.Alloc(MaterialInfo, GCHandleType.Pinned);
            void* ptr = handle.AddrOfPinnedObject().ToPointer();
            MaterialBuffer.UpdateBufferMemory(ptr, (ulong)sizeof(MaterialProperitiesBuffer));
        }

        public void SetMetallicRoughnessMap(string texturePath)
        {
            MetallicRoughnessMapPath = texturePath;
            MetallicRoughnessMap = new Texture(texturePath, VkFormat.VK_FORMAT_R8G8B8A8_UNORM, VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, TextureTypeEnum.kType_MetallicTextureMap, false);

            var updatedInfo = MaterialInfo;
            updatedInfo.MetallicRoughnessMap = MetallicRoughnessMap.TextureBufferIndex;
            MaterialInfo = updatedInfo;

            GCHandle handle = GCHandle.Alloc(MaterialInfo, GCHandleType.Pinned);
            void* ptr = handle.AddrOfPinnedObject().ToPointer();
            MaterialBuffer.UpdateBufferMemory(ptr, (ulong)sizeof(MaterialProperitiesBuffer));
        }

        public void SetMetallicMap(string texturePath)
        {
            MetallicMapPath = texturePath;
            MetallicMap = new Texture(texturePath, VkFormat.VK_FORMAT_R8G8B8A8_UNORM, VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, TextureTypeEnum.kType_MetallicTextureMap, false);

            var updatedInfo = MaterialInfo;
            updatedInfo.MetallicMap = MetallicMap.TextureBufferIndex;
            MaterialInfo = updatedInfo;

            GCHandle handle = GCHandle.Alloc(MaterialInfo, GCHandleType.Pinned);
            void* ptr = handle.AddrOfPinnedObject().ToPointer();
            MaterialBuffer.UpdateBufferMemory(ptr, (ulong)sizeof(MaterialProperitiesBuffer));
        }

        public void SetRoughnessMap(string texturePath)
        {
            RoughnessMapPath = texturePath;
            RoughnessMap = new Texture(texturePath, VkFormat.VK_FORMAT_R8G8B8A8_UNORM, VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, TextureTypeEnum.kType_MetallicTextureMap, false);

            var updatedInfo = MaterialInfo;
            updatedInfo.RoughnessMap = RoughnessMap.TextureBufferIndex;
            MaterialInfo = updatedInfo;

            GCHandle handle = GCHandle.Alloc(MaterialInfo, GCHandleType.Pinned);
            void* ptr = handle.AddrOfPinnedObject().ToPointer();
            MaterialBuffer.UpdateBufferMemory(ptr, (ulong)sizeof(MaterialProperitiesBuffer));
        }

        public void SetAmbientOcclusionMap(string texturePath)
        {
            AmbientOcclusionMapPath = texturePath;
            AmbientOcclusionMap = new Texture(texturePath, VkFormat.VK_FORMAT_R8G8B8A8_UNORM, VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, TextureTypeEnum.kType_AmbientOcclusionTextureMap, false);

            var updatedInfo = MaterialInfo;
            updatedInfo.AmbientOcclusionMap = AmbientOcclusionMap.TextureBufferIndex;
            MaterialInfo = updatedInfo;

            GCHandle handle = GCHandle.Alloc(MaterialInfo, GCHandleType.Pinned);
            void* ptr = handle.AddrOfPinnedObject().ToPointer();
            MaterialBuffer.UpdateBufferMemory(ptr, (ulong)sizeof(MaterialProperitiesBuffer));
        }

        public void SetNormalMap(string texturePath)
        {
            NormalMapPath = texturePath;
            NormalMap = new Texture(texturePath, VkFormat.VK_FORMAT_R8G8B8A8_UNORM, VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, TextureTypeEnum.kType_NormalTextureMap, false);

            var updatedInfo = MaterialInfo;
            updatedInfo.NormalMap = NormalMap.TextureBufferIndex;
            MaterialInfo = updatedInfo;

            GCHandle handle = GCHandle.Alloc(MaterialInfo, GCHandleType.Pinned);
            void* ptr = handle.AddrOfPinnedObject().ToPointer();
            MaterialBuffer.UpdateBufferMemory(ptr, (ulong)sizeof(MaterialProperitiesBuffer));
        }

        public void SetDepthMap(string texturePath)
        {
            DepthMapPath = texturePath;
            DepthMap = new Texture(texturePath, VkFormat.VK_FORMAT_R8G8B8A8_UNORM, VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, TextureTypeEnum.kType_DepthTextureMap, false);

            var updatedInfo = MaterialInfo;
            updatedInfo.DepthMap = DepthMap.TextureBufferIndex;
            MaterialInfo = updatedInfo;

            GCHandle handle = GCHandle.Alloc(MaterialInfo, GCHandleType.Pinned);
            void* ptr = handle.AddrOfPinnedObject().ToPointer();
            MaterialBuffer.UpdateBufferMemory(ptr, (ulong)sizeof(MaterialProperitiesBuffer));
        }

        public void SetAlphaMap(string texturePath)
        {
            AlphaMapPath = texturePath;
            AlphaMap = new Texture(texturePath, VkFormat.VK_FORMAT_R8G8B8A8_UNORM, VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, TextureTypeEnum.kType_AlphaTextureMap, false);

            var updatedInfo = MaterialInfo;
            updatedInfo.AlphaMap = AlphaMap.TextureBufferIndex;
            MaterialInfo = updatedInfo;

            GCHandle handle = GCHandle.Alloc(MaterialInfo, GCHandleType.Pinned);
            void* ptr = handle.AddrOfPinnedObject().ToPointer();
            MaterialBuffer.UpdateBufferMemory(ptr, (ulong)sizeof(MaterialProperitiesBuffer));
        }

        public void SetEmissionMap(string texturePath)
        {
            EmissionMapPath = texturePath;
            EmissionMap = new Texture(texturePath, VkFormat.VK_FORMAT_R8G8B8A8_UNORM, VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, TextureTypeEnum.kType_EmissionTextureMap, false);

            var updatedInfo = MaterialInfo;
            updatedInfo.EmissionMap = EmissionMap.TextureBufferIndex;
            MaterialInfo = updatedInfo;

            GCHandle handle = GCHandle.Alloc(MaterialInfo, GCHandleType.Pinned);
            void* ptr = handle.AddrOfPinnedObject().ToPointer();
            MaterialBuffer.UpdateBufferMemory(ptr, (ulong)sizeof(MaterialProperitiesBuffer));
        }

        public void SetHeightMap(string texturePath)
        {
            HeightMapPath = texturePath;
            HeightMap = new Texture(texturePath, VkFormat.VK_FORMAT_R8G8B8A8_UNORM, VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, TextureTypeEnum.kType_HeightMap, false);

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
