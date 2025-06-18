using CSScripting;
using GlmSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.Models;

namespace VulkanGameEngineLevelEditor.Vulkan
{
    public struct MaterialStruct
    {
        public const int TextureCount = 10;

        public int VectorMapKey;
        public Guid materialGuid;
        public uint ShaderMaterialBufferIndex;
        public int MaterialBufferId;

        public Guid AlbedoMapId;
        public Guid MetallicRoughnessMapId;
        public Guid MetallicMapId;
        public Guid RoughnessMapId;
        public Guid AmbientOcclusionMapId;
        public Guid NormalMapId;
        public Guid DepthMapId;
        public Guid AlphaMapId;
        public Guid EmissionMapId;
        public Guid HeightMapId;

        public vec3 Albedo = new vec3(0.0f, 0.35f, 0.45f);
        public vec3 Emission = new vec3(0.0f);
        public float Metallic = 0.0f;
        public float Roughness = 0.0f;
        public float AmbientOcclusion = 1.0f;
        public float Alpha = 1.0f;

        public MaterialStruct()
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

    public static class MaterialSystem
    {
        private static int NextBufferID = 0;
        public static Dictionary<Guid, MaterialStruct> MaterialMap { get; private set; } = new Dictionary<Guid, MaterialStruct>();

        public static Guid LoadMaterial(String materialPath)
        {
            if (materialPath.IsEmpty() ||
                materialPath == "")
            {
                return Guid.Empty;
            }

            string jsonContent = File.ReadAllText(materialPath);
            MaterialStruct model = JsonConvert.DeserializeObject<MaterialStruct>(jsonContent);

            if(MaterialMap.ContainsKey(model.materialGuid))
            {
                return model.materialGuid;
            }

            int bufferIndex = ++NextBufferID;

            return model.materialGuid;
        }
    }
}
