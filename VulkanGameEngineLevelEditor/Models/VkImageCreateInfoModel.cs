using GlmSharp;
using Silk.NET.Core.Attributes;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.EditorEnhancements;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.RenderPassEditor;
using Newtonsoft.Json;
using VulkanGameEngineLevelEditor.Vulkan;
using System.Runtime.InteropServices;
using AutoMapper;

namespace VulkanGameEngineLevelEditor.Models
{
    [Serializable]
    public unsafe class VkImageCreateInfoModel : RenderPassEditorBaseModel
    {
        private VkStructureType _sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO;
        private VkImageCreateFlagBits _flags = 0;
        private void* _pNext;
        private VkImageType _imageType;
        private VkFormat _format;
        private VkExtent3DModel _extent = new VkExtent3DModel();
        private uint _mipLevels;
        private uint _arrayLayers;
        private VkSampleCountFlagBits _samples;
        private VkImageTiling _tiling;
        private VkImageUsageFlagBits _usage;
        private VkSharingMode _sharingMode;
        private uint _queueFamilyIndexCount;
        private unsafe uint* _pQueueFamilyIndices;
        private VkImageLayout _initialLayout;

        [Browsable(false)]
        [Newtonsoft.Json.JsonIgnore]
        public VkStructureType sType
        {
            get => _sType;
            set
            {
                if (_sType != value)
                {
                    _sType = value;
                    OnPropertyChanged(nameof(sType));
                }
            }
        }

        [Category("Image Properties")]
        [Browsable(false)]
        [Newtonsoft.Json.JsonIgnore]
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public VkImageCreateFlagBits flags
        {
            get => _flags;
            set
            {
                if (_flags != value)
                {
                    _flags = value;
                    OnPropertyChanged(nameof(flags));
                }
            }
        }

        [Browsable(false)]
        [Newtonsoft.Json.JsonIgnore]
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public void* pNext
        {
            get => _pNext;
            set
            {
                if (_pNext != value)
                {
                    _pNext = value;
                    OnPropertyChanged(nameof(pNext));
                }
            }
        }

        [Category("Image Properties")]
        public VkImageType imageType
        {
            get => _imageType;
            set
            {
                if (_imageType != value)
                {
                    _imageType = value;
                    OnPropertyChanged(nameof(imageType));
                }
            }
        }

        [Category("Image Properties")]
        public VkFormat format
        {
            get => _format;
            set
            {
                if (_format != value)
                {
                    _format = value;
                    OnPropertyChanged(nameof(format));
                }
            }
        }

        [Category("Image Properties")]
        [Browsable(false)]
        [Newtonsoft.Json.JsonIgnore]
        public VkExtent3DModel extent
        {
            get => _extent;
            set
            {
                if (_extent != value)
                {
                    _extent = value;
                    OnPropertyChanged(nameof(extent));
                }
            }
        }

        [Category("Image Properties")]
        public uint mipLevels
        {
            get => _mipLevels;
            set
            {
                if (_mipLevels != value)
                {
                    _mipLevels = value;
                    OnPropertyChanged(nameof(mipLevels));
                }
            }
        }

        [Category("Image Properties")]
        public uint arrayLayers
        {
            get => _arrayLayers;
            set
            {
                if (_arrayLayers != value)
                {
                    _arrayLayers = value;
                    OnPropertyChanged(nameof(arrayLayers));
                }
            }
        }

        [Category("Image Properties")]
        public VkSampleCountFlagBits samples
        {
            get => _samples;
            set
            {
                if (_samples != value)
                {
                    _samples = value;
                    OnPropertyChanged(nameof(samples));
                }
            }
        }

        [Category("Image Properties")]
        public VkImageTiling tiling
        {
            get => _tiling;
            set
            {
                if (_tiling != value)
                {
                    _tiling = value;
                    OnPropertyChanged(nameof(tiling));
                }
            }
        }

        [Category("Image Properties")]
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public VkImageUsageFlagBits usage
        {
            get => _usage;
            set
            {
                if (_usage != value)
                {
                    _usage = value;
                    OnPropertyChanged(nameof(usage));
                }
            }
        }

        [Category("Image Properties")]
        public VkSharingMode sharingMode
        {
            get => _sharingMode;
            set
            {
                if (_sharingMode != value)
                {
                    _sharingMode = value;
                    OnPropertyChanged(nameof(sharingMode));
                }
            }
        }

        [Category("Queue Family")]
        [Browsable(false)]
        [Newtonsoft.Json.JsonIgnore]
        public uint queueFamilyIndexCount
        {
            get => _queueFamilyIndexCount;
            set
            {
                if (_queueFamilyIndexCount != value)
                {
                    _queueFamilyIndexCount = value;
                    OnPropertyChanged(nameof(queueFamilyIndexCount));
                }
            }
        }

        [Category("Queue Family")]
        [Browsable(false)]
        [Newtonsoft.Json.JsonIgnore]
        public unsafe uint* pQueueFamilyIndices
        {
            get => _pQueueFamilyIndices;
            set
            {
                if (_pQueueFamilyIndices != value)
                {
                    _pQueueFamilyIndices = value;
                    OnPropertyChanged(nameof(pQueueFamilyIndices));
                }
            }
        }

        [Category("Image Layout")]
        public VkImageLayout initialLayout
        {
            get => _initialLayout;
            set
            {
                if (_initialLayout != value)
                {
                    _initialLayout = value;
                    OnPropertyChanged(nameof(initialLayout));
                }
            }
        }

        public VkImageCreateInfoModel() : base()
        {
        }

        public VkImageCreateInfoModel(string jsonPath, ivec2 swapChainResoultion, VkFormat format2) : base()
        {
            LoadJsonComponent(jsonPath);
            extent.width = (uint)swapChainResoultion.x;
            extent.height = (uint)swapChainResoultion.y;
            extent.depth = 1;
            format = format2;
        }

        public VkImageCreateInfoModel(ivec2 swapChainResoultion, VkFormat format) : base()
        {
            LoadJsonComponent(ConstConfig.DefaultColorAttachmentDescriptionModel);
            _extent = new VkExtent3DModel((uint)swapChainResoultion.x, (uint)swapChainResoultion.y, 1);
            _format = format;
        }

        public VkImageCreateInfo Convert()
        {
            return new VkImageCreateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO,
                pNext = null,
                flags = flags,
                imageType = imageType,
                format = format,
                extent = extent.Convert(),
                mipLevels = mipLevels,
                arrayLayers = arrayLayers,
                samples = samples,
                tiling = tiling,
                usage = usage,
                sharingMode = sharingMode,
                queueFamilyIndexCount = queueFamilyIndexCount,
                pQueueFamilyIndices = null,
                initialLayout = initialLayout
            };
        }

        public void LoadJsonComponent(string jsonPath)
        {
            var obj = base.LoadJsonComponent<VkImageCreateInfoModel>(jsonPath);
            foreach (PropertyInfo property in typeof(VkImageCreateInfoModel).GetProperties())
            {
                if (property.CanWrite)
                {
                    property.SetValue(this, property.GetValue(obj));
                }
            }
        }

        public void SaveJsonComponent()
        {
            base.SaveJsonComponent($@"{ConstConfig.CreateImageInfoPath}{this._name}.json", this);
        }
    }
}
