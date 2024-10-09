using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.Core.Attributes;
using Silk.NET.Core;
using Silk.NET.Vulkan;

namespace VulkanGameEngineLevelEditor.Models
{
    public unsafe class SamplerCreateInfoModel
    {
        public SamplerCreateFlags Flags;
        public Filter MagFilter;
        public Filter MinFilter;
        public SamplerMipmapMode MipmapMode;
        public SamplerAddressMode AddressModeU;
        public SamplerAddressMode AddressModeV;
        public SamplerAddressMode AddressModeW;
        public float MipLodBias;
        public Bool32 AnisotropyEnable;
        public float MaxAnisotropy;
        public Bool32 CompareEnable;
        public CompareOp CompareOp;
        public float MinLod;
        public float MaxLod;
        public BorderColor BorderColor;
        public Bool32 UnnormalizedCoordinates;

        public SamplerCreateInfo ConvertToVulkan()
        {
            return new SamplerCreateInfo()
            {
                SType = StructureType.SamplerCreateInfo,
                AddressModeU = AddressModeU,
                AddressModeV = AddressModeV,
                AddressModeW = AddressModeW,
                AnisotropyEnable = AnisotropyEnable,
                MaxAnisotropy = MaxAnisotropy,
                CompareEnable = CompareEnable,
                MinLod = MinLod,
                MaxLod = MaxLod,
                BorderColor = BorderColor,
                UnnormalizedCoordinates = UnnormalizedCoordinates,
                CompareOp = CompareOp,
                Flags = Flags,
                MagFilter = MagFilter,
                MinFilter = MinFilter,
                MipLodBias = MipLodBias,
                MipmapMode = MipmapMode,
                PNext = null
            };
        }
    }
}

