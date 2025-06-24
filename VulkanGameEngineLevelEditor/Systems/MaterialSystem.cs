using CSScripting;
using GlmSharp;
using Newtonsoft.Json;
using Silk.NET.SDL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Models;

namespace VulkanGameEngineLevelEditor.Systems
{
    public struct Material
    {
        public int VectorMapKey;
        public Guid MaterialId { get; set; } = new Guid();
        public uint ShaderMaterialBufferIndex { get; set; } = 0;
        public int MaterialBufferId { get; set; } = 0;

        public Guid AlbedoMapId { get; set; } = new Guid();
        public Guid MetallicRoughnessMapId { get; set; } = new Guid();
        public Guid MetallicMapId { get; set; } = new Guid();
        public Guid RoughnessMapId { get; set; } = new Guid();
        public Guid AmbientOcclusionMapId { get; set; } = new Guid();
        public Guid NormalMapId { get; set; } = new Guid();
        public Guid DepthMapId { get; set; } = new Guid();
        public Guid AlphaMapId { get; set; } = new Guid();
        public Guid EmissionMapId { get; set; } = new Guid();
        public Guid HeightMapId { get; set; } = new Guid();

        public vec3 Albedo { get; set; } = new vec3(0.0f, 0.35f, 0.45f);
        public vec3 Emission { get; set; } = new vec3(0.0f);
        public float Metallic { get; set; } = 0.0f;
        public float Roughness { get; set; } = 0.0f;
        public float AmbientOcclusion { get; set; } = 1.0f;
        public float Alpha { get; set; } = 1.0f;

        public Material()
        {
        }
    };

    public struct MaterialProperitiesBuffer
    {
        public vec3 Albedo { get; set; } = new vec3(0.0f, 0.35f, 0.45f);
        public float Metallic { get; set; } = 0.0f;
        public float Roughness { get; set; } = 0.0f;
        public float AmbientOcclusion { get; set; } = 1.0f;
        public vec3 Emission { get; set; } = new vec3(0.0f);
        public float Alpha { get; set; } = 1.0f;

        public uint AlbedoMapId { get; set; } = 0;
        public uint MetallicRoughnessMapId { get; set; } = 0;
        public uint MetallicMapId { get; set; } = 0;
        public uint RoughnessMapId { get; set; } = 0;
        public uint AmbientOcclusionMapId { get; set; } = 0;
        public uint NormalMapId { get; set; } = 0;
        public uint DepthMapId { get; set; } = 0;
        public uint AlphaMapId { get; set; } = 0;
        public uint EmissionMapId { get; set; } = 0;
        public uint HeightMapId { get; set; } = 0;

        public MaterialProperitiesBuffer()
        {
        }
    };

    public static unsafe class MaterialSystem
    {
        private static uint NextBufferID = 0;
        public static Dictionary<Guid, Material> MaterialMap { get; private set; } = new Dictionary<Guid, Material>();

        public static Guid LoadMaterial(string materialPath)
        {
            if (materialPath.IsEmpty())
            {
                return new Guid();
            }

            string jsonContent = File.ReadAllText(materialPath);
            Material materialJson = JsonConvert.DeserializeObject<Material>(jsonContent);

            if (MaterialMap.ContainsKey(materialJson.MaterialId))
            {
                return materialJson.MaterialId;
            }

            GraphicsRenderer renderer = RenderSystem.renderer;
            uint NextBufferIndex = ++BufferSystem.NextBufferId;
            MaterialMap[materialJson.MaterialId] = Material_CreateMaterial(ref renderer, NextBufferIndex, out VulkanBuffer buffer, materialPath);
            BufferSystem.VulkanBufferMap[NextBufferIndex] = buffer;

            return materialJson.MaterialId;
        }

        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern Material Material_CreateMaterial(ref GraphicsRenderer renderer, uint bufferIndex, out VulkanBuffer materialBuffer, [MarshalAs(UnmanagedType.LPStr)] string jsonString);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern void Material_UpdateBuffer(GraphicsRenderer renderer, VulkanBuffer materialBuffer, MaterialProperitiesBuffer materialProperties);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern void Material_DestroyBuffer(GraphicsRenderer renderer, VulkanBuffer materialBuffer);
    }
}
