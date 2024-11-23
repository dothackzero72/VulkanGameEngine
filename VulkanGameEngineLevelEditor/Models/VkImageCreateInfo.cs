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

namespace VulkanGameEngineLevelEditor.Models
{
    [Serializable]
    public unsafe class VkImageCreateInfo : RenderPassEditorBaseModel
    {
        private StructureType _sType = StructureType.ImageCreateInfo;
        private ImageCreateFlags _flags = 0;
        private void* _pNext;
        private ImageType _imageType;
        private Format _format;
        private VkExtent3D _extent = new VkExtent3D();
        private uint _mipLevels;
        private uint _arrayLayers;
        private SampleCountFlags _samples;
        private ImageTiling _tiling;
        private ImageUsageFlags _usage;
        private SharingMode _sharingMode;
        private uint _queueFamilyIndexCount;
        private unsafe uint* _pQueueFamilyIndices;
        private ImageLayout _initialLayout;


        [Browsable(false)]
        [Newtonsoft.Json.JsonIgnore]
        public StructureType sType
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
        public ImageCreateFlags flags
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
        public ImageType imageType
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
        public Format format
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
        public VkExtent3D extent
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
        public SampleCountFlags samples
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
        public ImageTiling tiling
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
        public ImageUsageFlags usage
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
        public SharingMode sharingMode
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
        public ImageLayout initialLayout
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

        public VkImageCreateInfo() : base()
        {
        }

        public VkImageCreateInfo(string jsonPath, ivec2 swapChainResoultion, Format format2) : base()
        {
            LoadJsonComponent(jsonPath);
            extent.width = (uint)swapChainResoultion.x;
            extent.height = (uint)swapChainResoultion.y;
            extent.depth = 1;
            format = format2;
        }

        public VkImageCreateInfo(ivec2 swapChainResoultion, Format format) : base()
        {
            LoadJsonComponent(ConstConfig.DefaultColorAttachmentDescriptionModel);
            _extent = new VkExtent3D((uint)swapChainResoultion.x, (uint)swapChainResoultion.y, 1);
            _format = format;
        }

        public ImageCreateInfo Convert()
        {
            return new ImageCreateInfo()
            {
                SType = StructureType.ImageCreateInfo,
                PNext = null,
                Flags = flags,
                ImageType = imageType,
                Format = format,
                Extent = extent.Convert(),
                MipLevels = mipLevels,
                ArrayLayers = arrayLayers,
                Samples = samples,
                Tiling = tiling,
                Usage = usage,
                SharingMode = sharingMode,
                QueueFamilyIndexCount = queueFamilyIndexCount,
                PQueueFamilyIndices = null,
                InitialLayout = initialLayout
            };
        }

        public void LoadJsonComponent(string jsonPath)
        {
            var obj = base.LoadJsonComponent<VkImageCreateInfo>(jsonPath);
            foreach (PropertyInfo property in typeof(VkImageCreateInfo).GetProperties())
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
