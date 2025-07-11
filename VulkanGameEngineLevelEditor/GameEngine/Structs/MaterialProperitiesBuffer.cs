using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.GameEngine.Structs
{
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
}
