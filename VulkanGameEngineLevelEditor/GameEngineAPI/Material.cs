using CSScripting;
using GlmSharp;
using Newtonsoft.Json;
using Silk.NET.SDL;
using System;
using System.Runtime.InteropServices;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.Systems;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Material
    {
        int VectorMapKey;             
        Guid materialGuid;                
        uint ShaderMaterialBufferIndex;    
        int MaterialBufferId;               

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

        vec3 Albedo = new vec3(0.0f, 0.35f, 0.45f);  
        vec3 Emission = new vec3(0.0f);         
        float Metallic = 0.0f;             
        float Roughness = 0.0f;            
        float AmbientOcclusion = 1.0f;       
        float Alpha = 1.0f;                 

        public Material()
        {
        }
    }
}
