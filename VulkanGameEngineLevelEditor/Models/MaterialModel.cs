using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.Models
{
    public class MaterialModel
    {
        public string MaterialID { get; set; }
        public string Name { get; set; }
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
        public vec3 Albedo { get; set; }
        public float Metallic { get; set; }
        public float Roughness { get; set; }
        public float AmbientOcclusion { get; set; }
        public vec3 Emission { get; set; }
        public float Alpha { get; set; }
    }
}
