using CSScripting;
using GlmSharp;
using Newtonsoft.Json;
using Silk.NET.SDL;
using System;
using System.Runtime.InteropServices;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct Material
    {
        Guid MaterialId;
        uint MaterialBufferIndex;

        vec3 Albedo;
        float Metallic;
        float Roughness;
        float AmbientOcclusion;
        vec3 Emission;
        float Alpha;

        Guid AlbedoMapId;
        Guid MetallicRoughnessMapId;
        Guid MetallicMapId;
        Guid RoughnessMapId;
        Guid AmbientOcclusionMapId;
        Guid NormalMapId;
        Guid DepthMapId;
        Guid AlphaMapId;
        Guid EmissionMapId;
        Guid HeightMapId;

        VulkanBuffer<MaterialProperitiesBuffer> MaterialBuffer;
    }
}
