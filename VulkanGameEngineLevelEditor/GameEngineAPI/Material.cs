using GlmSharp;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public struct MaterialProperitiesBuffer
    {
        public vec3 Albedo = new vec3(0.0f, 0.35f, 0.45f);
        public float Metallic = 0.0f;
        public float Roughness = 0.0f;
        public float AmbientOcclusion = 1.0f;
        public vec3 Emission = new vec3(0.0f);
        public float Alpha = 1.0f;

        public uint AlbedoMap = uint.MaxValue;
        public uint MetallicRoughnessMap = uint.MaxValue;
        public uint MetallicMap = uint.MaxValue;
        public uint RoughnessMap = uint.MaxValue;
        public uint AmbientOcclusionMap = uint.MaxValue;
        public uint NormalMap = uint.MaxValue;
        public uint DepthMap = uint.MaxValue;
        public uint AlphaMap = uint.MaxValue;
        public uint EmissionMap = uint.MaxValue;
        public uint HeightMap = uint.MaxValue;

        public MaterialProperitiesBuffer()
        {
        }
    };

    public class Material
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
            //MaterialBuffer = new VulkanBuffer<MaterialProperitiesBuffer>(MaterialInfo, 1, BufferUsageFlags.ShaderDeviceAddressBit |
            //                                                                           BufferUsageFlags.StorageBufferBit,
            //                                                                           MemoryPropertyFlags.HostVisibleBit |
            //                                                                           MemoryPropertyFlags.HostVisibleBit, false);
        }

        private void UpdateBuffer()
        {
            //MaterialInfo.Albedo = Albedo;
            //MaterialInfo.Metallic = Metallic;
            //MaterialInfo.Roughness = Roughness;
            //MaterialInfo.AmbientOcclusion = AmbientOcclusion;
            //MaterialInfo.Emission = Emission;
            //MaterialInfo.Alpha = Alpha;

            //MaterialBuffer.UpdateBufferData(MaterialInfo);
        }
    }
}
