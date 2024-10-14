using GlmSharp;
using Silk.NET.Core.Attributes;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.EditorEnhancements;
using VulkanGameEngineLevelEditor.GameEngineAPI;

namespace VulkanGameEngineLevelEditor.Models
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ImageCreateInfoModel : RenderPassEditorBaseModel
    {
        [Category("Image Properties")]
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        private ImageCreateFlags _flags;
        private ImageType _imageType;
        private Format _format;
        private Extent3DModel _extent = new Extent3DModel();
        private uint _mipLevels;
        private uint _arrayLayers;
        private SampleCountFlags _samples;
        private ImageTiling _tiling;
        private ImageUsageFlags _usage;
        private SharingMode _sharingMode;
        private uint _queueFamilyIndexCount;
        private unsafe uint* _pQueueFamilyIndices;
        private ImageLayout _initialLayout;

        [Browsable(true)]
        [Category("Image Properties")]
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public ImageCreateFlags Flags
        {
            get => _flags;
            set
            {
                if (_flags != value)
                {
                    _flags = value;
                    OnPropertyChanged(nameof(Flags));
                }
            }
        }

        [Category("Image Properties")]
        public ImageType ImageType
        {
            get => _imageType;
            set
            {
                if (_imageType != value)
                {
                    _imageType = value;
                    OnPropertyChanged(nameof(ImageType));
                }
            }
        }

        [Category("Image Properties")]
        public Format Format
        {
            get => _format;
            set
            {
                if (_format != value)
                {
                    _format = value;
                    OnPropertyChanged(nameof(Format));
                }
            }
        }

        [Category("Image Properties")]
        public Extent3DModel Extent
        {
            get => _extent;
            set
            {
                if (_extent != value)
                {
                    _extent = value;
                    OnPropertyChanged(nameof(Extent));
                }
            }
        }

        [Category("Image Properties")]
        public uint MipLevels
        {
            get => _mipLevels;
            set
            {
                if (_mipLevels != value)
                {
                    _mipLevels = value;
                    OnPropertyChanged(nameof(MipLevels));
                }
            }
        }

        [Category("Image Properties")]
        public uint ArrayLayers
        {
            get => _arrayLayers;
            set
            {
                if (_arrayLayers != value)
                {
                    _arrayLayers = value;
                    OnPropertyChanged(nameof(ArrayLayers));
                }
            }
        }

        [Category("Image Properties")]
        public SampleCountFlags Samples
        {
            get => _samples;
            set
            {
                if (_samples != value)
                {
                    _samples = value;
                    OnPropertyChanged(nameof(Samples));
                }
            }
        }

        [Category("Image Properties")]
        public ImageTiling Tiling
        {
            get => _tiling;
            set
            {
                if (_tiling != value)
                {
                    _tiling = value;
                    OnPropertyChanged(nameof(Tiling));
                }
            }
        }

        [Category("Image Properties")]
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public ImageUsageFlags Usage
        {
            get => _usage;
            set
            {
                if (_usage != value)
                {
                    _usage = value;
                    OnPropertyChanged(nameof(Usage));
                }
            }
        }

        [Category("Image Properties")]
        public SharingMode SharingMode
        {
            get => _sharingMode;
            set
            {
                if (_sharingMode != value)
                {
                    _sharingMode = value;
                    OnPropertyChanged(nameof(SharingMode));
                }
            }
        }

        [Category("Queue Family")]
        public uint QueueFamilyIndexCount
        {
            get => _queueFamilyIndexCount;
            set
            {
                if (_queueFamilyIndexCount != value)
                {
                    _queueFamilyIndexCount = value;
                    OnPropertyChanged(nameof(QueueFamilyIndexCount));
                }
            }
        }

        [Category("Queue Family")]
        public unsafe uint* PQueueFamilyIndices
        {
            get => _pQueueFamilyIndices;
            set
            {
                if (_pQueueFamilyIndices != value)
                {
                    _pQueueFamilyIndices = value;
                    OnPropertyChanged(nameof(PQueueFamilyIndices));
                }
            }
        }

        [Category("Image Layout")]
        public ImageLayout InitialLayout
        {
            get => _initialLayout;
            set
            {
                if (_initialLayout != value)
                {
                    _initialLayout = value;
                    OnPropertyChanged(nameof(InitialLayout));
                }
            }
        }

        public ImageCreateInfoModel() : base()
        {
        }

        public ImageCreateInfoModel(ivec2 swapChainResoultion, Format format) : base()
        {
            _extent = new Extent3DModel((uint)swapChainResoultion.x, (uint)swapChainResoultion.y, 1);
            _format = format;
        }

        public ImageCreateInfoModel(string name, ivec2 swapChainResoultion, Format format) : base(name)
        {
            _extent = new Extent3DModel((uint)swapChainResoultion.x, (uint)swapChainResoultion.y, 1);
            _format = format;
        }

        public ImageCreateInfo ConvertToVulkan()
        {
            return new ImageCreateInfo()
            {
                SType = StructureType.ImageCreateInfo,
                PNext = null,
                Flags = Flags,
                ImageType = ImageType,
                Format = Format,
                Extent = Extent.ConvertToVulkan(),
                MipLevels = MipLevels,
                ArrayLayers = ArrayLayers,
                Samples = Samples,
                Tiling = Tiling,
                Usage = Usage,
                SharingMode = SharingMode,
                QueueFamilyIndexCount = QueueFamilyIndexCount,
                PQueueFamilyIndices = null,
                InitialLayout = InitialLayout
            };
        }

    }
}
