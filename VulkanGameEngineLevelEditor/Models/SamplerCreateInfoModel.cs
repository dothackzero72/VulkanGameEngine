using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.Core.Attributes;
using Silk.NET.Core;
using Silk.NET.Vulkan;
using System.ComponentModel;
using System.Reflection;

namespace VulkanGameEngineLevelEditor.Models
{
    [Serializable]
    public class SamplerCreateInfoModel : RenderPassEditorBaseModel
    {
        private SamplerCreateFlags _flags;
        private Filter _magFilter;
        private Filter _minFilter;
        private SamplerMipmapMode _mipmapMode;
        private SamplerAddressMode _addressModeU;
        private SamplerAddressMode _addressModeV;
        private SamplerAddressMode _addressModeW;
        private float _mipLodBias;
        private bool _anisotropyEnable;
        private float _maxAnisotropy;
        private bool _compareEnable;
        private CompareOp _compareOp;
        private float _minLod;
        private float _maxLod;
        private BorderColor _borderColor;
        private bool _unnormalizedCoordinates;

        [Category("Sampler Properties")]
        public SamplerCreateFlags Flags
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

        [Category("Sampler Properties")]
        public Filter MagFilter
        {
            get => _magFilter;
            set
            {
                if (_magFilter != value)
                {
                    _magFilter = value;
                    OnPropertyChanged(nameof(MagFilter));
                }
            }
        }

        [Category("Sampler Properties")]
        public Filter MinFilter
        {
            get => _minFilter;
            set
            {
                if (_minFilter != value)
                {
                    _minFilter = value;
                    OnPropertyChanged(nameof(MinFilter));
                }
            }
        }

        [Category("Sampler Properties")]
        public SamplerMipmapMode MipmapMode
        {
            get => _mipmapMode;
            set
            {
                if (_mipmapMode != value)
                {
                    _mipmapMode = value;
                    OnPropertyChanged(nameof(MipmapMode));
                }
            }
        }

        [Category("Address Modes")]
        public SamplerAddressMode AddressModeU
        {
            get => _addressModeU;
            set
            {
                if (_addressModeU != value)
                {
                    _addressModeU = value;
                    OnPropertyChanged(nameof(AddressModeU));
                }
            }
        }

        [Category("Address Modes")]
        public SamplerAddressMode AddressModeV
        {
            get => _addressModeV;
            set
            {
                if (_addressModeV != value)
                {
                    _addressModeV = value;
                    OnPropertyChanged(nameof(AddressModeV));
                }
            }
        }

        [Category("Address Modes")]
        public SamplerAddressMode AddressModeW
        {
            get => _addressModeW;
            set
            {
                if (_addressModeW != value)
                {
                    _addressModeW = value;
                    OnPropertyChanged(nameof(AddressModeW));
                }
            }
        }

        [Category("Sampler Properties")]
        public float MipLodBias
        {
            get => _mipLodBias;
            set
            {
                if (_mipLodBias != value)
                {
                    _mipLodBias = value;
                    OnPropertyChanged(nameof(MipLodBias));
                }
            }
        }

        [Category("Anisotropy")]
        public bool AnisotropyEnable
        {
            get => _anisotropyEnable;
            set
            {
                if (_anisotropyEnable != value)
                {
                    _anisotropyEnable = value;
                    OnPropertyChanged(nameof(AnisotropyEnable));
                }
            }
        }

        [Category("Anisotropy")]
        public float MaxAnisotropy
        {
            get => _maxAnisotropy;
            set
            {
                if (_maxAnisotropy != value)
                {
                    _maxAnisotropy = value;
                    OnPropertyChanged(nameof(MaxAnisotropy));
                }
            }
        }

        [Category("Comparison")]
        public bool CompareEnable
        {
            get => _compareEnable;
            set
            {
                if (_compareEnable != value)
                {
                    _compareEnable = value;
                    OnPropertyChanged(nameof(CompareEnable));
                }
            }
        }

        [Category("Comparison")]
        public CompareOp CompareOp
        {
            get => _compareOp;
            set
            {
                if (_compareOp != value)
                {
                    _compareOp = value;
                    OnPropertyChanged(nameof(CompareOp));
                }
            }
        }

        [Category("LOD")]
        public float MinLod
        {
            get => _minLod;
            set
            {
                if (_minLod != value)
                {
                    _minLod = value;
                    OnPropertyChanged(nameof(MinLod));
                }
            }
        }

        [Category("LOD")]
        public float MaxLod
        {
            get => _maxLod;
            set
            {
                if (_maxLod != value)
                {
                    _maxLod = value;
                    OnPropertyChanged(nameof(MaxLod));
                }
            }
        }

        [Category("Border Color")]
        public BorderColor BorderColor
        {
            get => _borderColor;
            set
            {
                if (_borderColor != value)
                {
                    _borderColor = value;
                    OnPropertyChanged(nameof(BorderColor));
                }
            }
        }

        [Category("Sampler Properties")]
        public bool UnnormalizedCoordinates
        {
            get => _unnormalizedCoordinates;
            set
            {
                if (_unnormalizedCoordinates != value)
                {
                    _unnormalizedCoordinates = value;
                    OnPropertyChanged(nameof(UnnormalizedCoordinates));
                }
            }
        }

        public SamplerCreateInfoModel() : base()
        {
        }

        public SamplerCreateInfoModel(string jsonFilePath) : base()
        {
            LoadJsonComponent(jsonFilePath);
        }

        public SamplerCreateInfoModel(string name, string jsonFilePath) : base(name)
        {
           // LoadJsonComponent(@"C:\Users\dotha\Documents\GitHub\VulkanGameEngine\RenderPass\  \DefaultSubpassDependency.json");
        }

        public SamplerCreateInfo ConvertToVulkan()
        {
            return new SamplerCreateInfo()
            {
                SType = StructureType.SamplerCreateInfo,
                AddressModeU = AddressModeU,
                AddressModeV = AddressModeV,
                AddressModeW = AddressModeW,
                AnisotropyEnable = (uint)(_anisotropyEnable ? 1 : 0),
                MaxAnisotropy = MaxAnisotropy,
                CompareEnable = (uint)(_compareEnable ? 1 : 0),
                MinLod = MinLod,
                MaxLod = MaxLod,
                BorderColor = BorderColor,
                UnnormalizedCoordinates = (uint)(_unnormalizedCoordinates ? 1 : 0),
                CompareOp = CompareOp,
                Flags = Flags,
                MagFilter = MagFilter,
                MinFilter = MinFilter,
                MipLodBias = MipLodBias,
                MipmapMode = MipmapMode,
                PNext = null
            };
        }

        public void LoadJsonComponent(string jsonPath)
        {
            var obj = base.LoadJsonComponent<SamplerCreateInfoModel>(jsonPath);
            foreach (PropertyInfo property in typeof(SamplerCreateInfoModel).GetProperties())
            {
                if (property.CanWrite)
                {
                    property.SetValue(this, property.GetValue(obj));
                }
            }
        }
    }
}

