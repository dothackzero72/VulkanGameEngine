using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
    public class RenderedTextureInfoModel : RenderPassEditorBaseModel
    {
        public bool IsRenderedToSwapchain { get; set; } = false;
        private string _renderedTextureInfoName = string.Empty;
        private VkImageCreateInfo _imageCreateInfo = new VkImageCreateInfo();
        private VkSamplerCreateInfo _samplerCreateInfo = new VkSamplerCreateInfo();
        private VkAttachmentDescription _attachmentDescription = new VkAttachmentDescription();
        private RenderedTextureType _textureType;

        public string RenderedTextureInfoName
        {
            get => _renderedTextureInfoName;
            set
            {
                if (_renderedTextureInfoName != value)
                {
                    _renderedTextureInfoName = value;
                    OnPropertyChanged(nameof(RenderedTextureInfoName));
                }
            }
        }

        [Category("Image")]
        public VkImageCreateInfo ImageCreateInfo
        {
            get => _imageCreateInfo;
            set
            {
                if (_imageCreateInfo != value)
                {
                    _imageCreateInfo = value;
                    OnPropertyChanged(nameof(ImageCreateInfo));
                }
            }
        }

        [Category("Sampler")]
        public VkSamplerCreateInfo SamplerCreateInfo
        {
            get => _samplerCreateInfo;
            set
            {
                if (_samplerCreateInfo != value)
                {
                    _samplerCreateInfo = value;
                    OnPropertyChanged(nameof(SamplerCreateInfo));
                }
            }
        }

        [Category("Attachment")]
        public VkAttachmentDescription AttachmentDescription
        { 
            get => _attachmentDescription;
            set
            {
                if (_attachmentDescription != value)
                {
                    _attachmentDescription = value;
                    OnPropertyChanged(nameof(AttachmentDescription));
                }
            }
        }

        [Category("Texture")]
        public RenderedTextureType TextureType
        {
            get => _textureType;
            set
            {
                if (_textureType != value)
                {
                    _textureType = value;
                    OnPropertyChanged(nameof(TextureType));
                }
            }
        }

        public RenderedTextureInfoModel() : base()
        {
        }

        public RenderedTextureInfoModel(string jsonFilePath, bool isRenderedToSwapChain) : base()
        {
            IsRenderedToSwapchain = isRenderedToSwapChain;
            LoadJsonComponent(jsonFilePath);
        }

        public RenderedTextureInfoModel(string name, string jsonFilePath, bool isRenderedToSwapChain) : base(name)
        {
            IsRenderedToSwapchain = isRenderedToSwapChain;
            LoadJsonComponent(jsonFilePath);
        }

        public void LoadJsonComponent(string jsonPath)
        {
            var obj = base.LoadJsonComponent<RenderedTextureInfoModel>(jsonPath);
            foreach (PropertyInfo property in typeof(RenderedTextureInfoModel).GetProperties())
            {
                if (property.CanWrite)
                {
                    property.SetValue(this, property.GetValue(obj));
                }
            }
        }

    }
}
