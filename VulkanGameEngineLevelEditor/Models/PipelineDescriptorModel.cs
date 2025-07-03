using System;
using System.Runtime.InteropServices;
using VulkanGameEngineLevelEditor.GameEngineAPI;


namespace VulkanGameEngineLevelEditor.Models
{
    public enum DescriptorBindingPropertiesEnum : UInt32
    {
        kMeshPropertiesDescriptor,
        kTextureDescriptor,
        kMaterialDescriptor,
        kBRDFMapDescriptor,
        kIrradianceMapDescriptor,
        kPrefilterMapDescriptor,
        kCubeMapDescriptor,
        kEnvironmentDescriptor,
        kSunLightDescriptor,
        kDirectionalLightDescriptor,
        kPointLightDescriptor,
        kSpotLightDescriptor,
        kReflectionViewDescriptor,
        kDirectionalShadowDescriptor,
        kPointShadowDescriptor,
        kSpotShadowDescriptor,
        kViewTextureDescriptor,
        kViewDepthTextureDescriptor,
        kCubeMapSamplerDescriptor,
        kRotatingPaletteTextureDescriptor,
        kMathOpperation1Descriptor,
        kMathOpperation2Descriptor,
    };

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct PipelineDescriptorModel
    {
        public uint BindingNumber;
        public DescriptorBindingPropertiesEnum BindingPropertiesList;
        public VkDescriptorType DescriptorType;

        public PipelineDescriptorModel(uint bindingNumber, DescriptorBindingPropertiesEnum properties, VkDescriptorType type)
        {
            BindingNumber = bindingNumber;
            BindingPropertiesList = properties;
            DescriptorType = type;
        }
    }
}
