using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.GameEngine.Structs
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
}
