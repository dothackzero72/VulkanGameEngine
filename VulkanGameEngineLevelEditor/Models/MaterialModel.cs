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
        public string Name { get; private set; }
        public string AlbedoMapPath { get; private set; }
        public string MetallicRoughnessMapPath { get; private set; }
        public string MetallicMapPath { get; private set; }
        public string RoughnessMapPath { get; private set; }
        public string AmbientOcclusionMapPath { get; private set; }
        public string NormalMapPath { get; private set; }
        public string DepthMapPath { get; private set; }
        public string AlphaMapPath { get; private set; }
        public string EmissionMapPath { get; private set; }
        public string HeightMapPath { get; private set; }
        public vec3 Albedo { get; private set; }
        public float Metallic { get; private set; }
        public float Roughness { get; private set; }
        public float AmbientOcclusion { get; private set; }
        public vec3 Emission { get; private set; }
        public float Alpha { get; private set; }
    }
}
