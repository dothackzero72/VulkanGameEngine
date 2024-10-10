using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.Models
{
    public enum RenderedTextureType
    {
        ColorRenderedTexture,
        DepthRenderedTexture,
        InputAttachmentTexture,
        ResolveAttachmentTexture
    };

    [Serializable]
    public class RenderedTextureInfoModel
    {
        public string RenderedTextureInfoName = string.Empty;
        private ImageCreateInfoModel   _ImageCreateInfo = new ImageCreateInfoModel();
        private SamplerCreateInfoModel _SamplerCreateInfo = new SamplerCreateInfoModel();
        private AttachmentDescription  _attachmentDescription = new AttachmentDescription();
        public RenderedTextureType TextureType { get; set; }
        public ImageCreateInfoModel ImageCreateInfo 
        {
            get => _ImageCreateInfo;
            set
            {
                if (_ImageCreateInfo != value)
                {
                    _ImageCreateInfo = value;
                    OnPropertyChanged(nameof(ImageCreateInfoModel));
                }
            }
        }
        public SamplerCreateInfoModel SamplerCreateInfo 
        {
            get => _SamplerCreateInfo;
            set
            {
                if (_SamplerCreateInfo != value)
                {
                    _SamplerCreateInfo = value;
                    OnPropertyChanged(nameof(SamplerCreateInfoModel));
                }
            }
        }

        public AttachmentDescription AttachmentDescription
        {
            get => _attachmentDescription;
            set
            {
             
                    _attachmentDescription = value;
                    OnPropertyChanged(nameof(AttachmentDescription));
                
            }
        }

        public RenderedTextureInfoModel(string name)
        {
            RenderedTextureInfoName = name;
            TextureType = RenderedTextureType.ColorRenderedTexture;
            ImageCreateInfo = new ImageCreateInfoModel()
            {
                Flags = ImageCreateFlags.None,
                ImageType = Silk.NET.Vulkan.ImageType.Type2D,
                Format = Format.Undefined,
                Extent = new Extent3DModel { Width = 256, Height = 256, Depth = 1 },
                MipLevels = 1,
                ArrayLayers = 1,
                Samples = SampleCountFlags.SampleCount1Bit,
                Tiling = ImageTiling.Linear,
                Usage = ImageUsageFlags.None,
                SharingMode = SharingMode.Exclusive,
                InitialLayout = Silk.NET.Vulkan.ImageLayout.Undefined
            };
            SamplerCreateInfo = new SamplerCreateInfoModel()
            {
                Flags = 0,
                MagFilter = Filter.Linear,
                MinFilter = Filter.Linear,
                MipmapMode = SamplerMipmapMode.Linear,
                AddressModeU = SamplerAddressMode.Repeat,
                AddressModeV = SamplerAddressMode.Repeat,
                AddressModeW = SamplerAddressMode.Repeat,
                MipLodBias = 0.0f,
                AnisotropyEnable = Vk.False,
                MaxAnisotropy = 1.0f,
                CompareEnable = Vk.False,
                CompareOp = CompareOp.Always,
                MinLod = 0.0f,
                MaxLod = float.MaxValue,
                BorderColor = BorderColor.FloatTransparentBlack,
                UnnormalizedCoordinates = Vk.False
            };
        }
        public RenderedTextureInfoModel()
        {
            TextureType = RenderedTextureType.ColorRenderedTexture;
            ImageCreateInfo = new ImageCreateInfoModel()
            {
                Flags = ImageCreateFlags.None,
                ImageType = Silk.NET.Vulkan.ImageType.Type2D,
                Format = Format.Undefined,
                Extent = new Extent3DModel { Width = 256, Height = 256, Depth = 1 },
                MipLevels = 1,
                ArrayLayers = 1,
                Samples = SampleCountFlags.SampleCount1Bit,
                Tiling = ImageTiling.Linear,
                Usage = ImageUsageFlags.None,
                SharingMode = SharingMode.Exclusive,
                InitialLayout = Silk.NET.Vulkan.ImageLayout.Undefined
            };
            SamplerCreateInfo = new SamplerCreateInfoModel()
            {
                Flags = 0,
                MagFilter = Filter.Linear,
                MinFilter = Filter.Linear,
                MipmapMode = SamplerMipmapMode.Linear,
                AddressModeU = SamplerAddressMode.Repeat,
                AddressModeV = SamplerAddressMode.Repeat,
                AddressModeW = SamplerAddressMode.Repeat,
                MipLodBias = 0.0f,
                AnisotropyEnable = Vk.False,
                MaxAnisotropy = 1.0f,
                CompareEnable = Vk.False,
                CompareOp = CompareOp.Always,
                MinLod = 0.0f,
                MaxLod = float.MaxValue,
                BorderColor = BorderColor.FloatTransparentBlack,
                UnnormalizedCoordinates = Vk.False
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
