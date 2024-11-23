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
using VulkanGameEngineLevelEditor.RenderPassEditor;
using VulkanGameEngineLevelEditor.EditorEnhancements;

namespace VulkanGameEngineLevelEditor.Models
{
    [Serializable]
    public unsafe class VkSamplerCreateInfo : RenderPassEditorBaseModel
    {
        private StructureType _structureType = StructureType.SamplerCreateInfo;
        private SamplerCreateFlags _flags = 0;
        private void* _pNext = null;
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

        [Browsable(false)]
        [Newtonsoft.Json.JsonIgnore]
        public StructureType structureType
        {
            get => _structureType;
            set
            {
                if (_structureType != value)
                {
                    _structureType = value;
                    OnPropertyChanged(nameof(structureType));
                }
            }
        }

        [Category("Sampler Properties")]
        [Browsable(false)]
        [Newtonsoft.Json.JsonIgnore]
        public SamplerCreateFlags flags
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

        [Category("Sampler Properties")]
        public Filter magFilter
        {
            get => _magFilter;
            set
            {
                if (_magFilter != value)
                {
                    _magFilter = value;
                    OnPropertyChanged(nameof(magFilter));
                }
            }
        }

        [Category("Sampler Properties")]
        public Filter minFilter
        {
            get => _minFilter;
            set
            {
                if (_minFilter != value)
                {
                    _minFilter = value;
                    OnPropertyChanged(nameof(minFilter));
                }
            }
        }

        [Category("Sampler Properties")]
        public SamplerMipmapMode mipmapMode
        {
            get => _mipmapMode;
            set
            {
                if (_mipmapMode != value)
                {
                    _mipmapMode = value;
                    OnPropertyChanged(nameof(mipmapMode));
                }
            }
        }

        [Category("Address Modes")]
        public SamplerAddressMode addressModeU
        {
            get => _addressModeU;
            set
            {
                if (_addressModeU != value)
                {
                    _addressModeU = value;
                    OnPropertyChanged(nameof(addressModeU));
                }
            }
        }

        [Category("Address Modes")]
        public SamplerAddressMode addressModeV
        {
            get => _addressModeV;
            set
            {
                if (_addressModeV != value)
                {
                    _addressModeV = value;
                    OnPropertyChanged(nameof(addressModeV));
                }
            }
        }

        [Category("Address Modes")]
        public SamplerAddressMode addressModeW
        {
            get => _addressModeW;
            set
            {
                if (_addressModeW != value)
                {
                    _addressModeW = value;
                    OnPropertyChanged(nameof(addressModeW));
                }
            }
        }

        [Category("Sampler Properties")]
        public float mipLodBias
        {
            get => _mipLodBias;
            set
            {
                if (_mipLodBias != value)
                {
                    _mipLodBias = value;
                    OnPropertyChanged(nameof(mipLodBias));
                }
            }
        }

        [Category("Anisotropy")]
        public bool anisotropyEnable
        {
            get => _anisotropyEnable;
            set
            {
                if (_anisotropyEnable != value)
                {
                    _anisotropyEnable = value;
                    OnPropertyChanged(nameof(anisotropyEnable));
                }
            }
        }

        [Category("Anisotropy")]
        public float maxAnisotropy
        {
            get => _maxAnisotropy;
            set
            {
                if (_maxAnisotropy != value)
                {
                    _maxAnisotropy = value;
                    OnPropertyChanged(nameof(maxAnisotropy));
                }
            }
        }

        [Category("Comparison")]
        public bool compareEnable
        {
            get => _compareEnable;
            set
            {
                if (_compareEnable != value)
                {
                    _compareEnable = value;
                    OnPropertyChanged(nameof(compareEnable));
                }
            }
        }

        [Category("Comparison")]
        public CompareOp compareOp
        {
            get => _compareOp;
            set
            {
                if (_compareOp != value)
                {
                    _compareOp = value;
                    OnPropertyChanged(nameof(compareOp));
                }
            }
        }

        [Category("LOD")]
        public float minLod
        {
            get => _minLod;
            set
            {
                if (_minLod != value)
                {
                    _minLod = value;
                    OnPropertyChanged(nameof(minLod));
                }
            }
        }

        [Category("LOD")]
        public float maxLod
        {
            get => _maxLod;
            set
            {
                if (_maxLod != value)
                {
                    _maxLod = value;
                    OnPropertyChanged(nameof(maxLod));
                }
            }
        }

        [Category("Border Color")]
        public BorderColor borderColor
        {
            get => _borderColor;
            set
            {
                if (_borderColor != value)
                {
                    _borderColor = value;
                    OnPropertyChanged(nameof(borderColor));
                }
            }
        }

        [Category("Sampler Properties")]
        public bool unnormalizedCoordinates
        {
            get => _unnormalizedCoordinates;
            set
            {
                if (_unnormalizedCoordinates != value)
                {
                    _unnormalizedCoordinates = value;
                    OnPropertyChanged(nameof(unnormalizedCoordinates));
                }
            }
        }

        public VkSamplerCreateInfo() : base()
        {
        }

        public VkSamplerCreateInfo(string jsonFilePath) : base()
        {
            LoadJsonComponent(jsonFilePath);
        }

        public VkSamplerCreateInfo(string name, string jsonFilePath) : base(name)
        {
           // LoadJsonComponent(@"C:\Users\dotha\Documents\GitHub\VulkanGameEngine\RenderPass\  \DefaultSubpassDependency.json");
        }
        public SamplerCreateInfo Convert()
        {
            return new SamplerCreateInfo
            {
                SType = StructureType.SamplerCreateInfo,
                AddressModeU = addressModeU,
                AddressModeV = addressModeV,
                AddressModeW = addressModeW,
                AnisotropyEnable = (uint)(_anisotropyEnable ? 1 : 0),
                MaxAnisotropy = maxAnisotropy,
                CompareEnable = (uint)(_compareEnable ? 1 : 0),
                MinLod = minLod,
                MaxLod = maxLod,
                BorderColor = borderColor,
                UnnormalizedCoordinates = (uint)(_unnormalizedCoordinates ? 1 : 0),
                CompareOp = compareOp,
                Flags = flags,
                MagFilter = magFilter,
                MinFilter = minFilter,
                MipLodBias = mipLodBias,
                MipmapMode = mipmapMode,
                PNext = null
            };
        }

        public void LoadJsonComponent(string jsonPath)
        {
            var obj = base.LoadJsonComponent<VkSamplerCreateInfo>(jsonPath);
            foreach (PropertyInfo property in typeof(VkSamplerCreateInfo).GetProperties())
            {
                if (property.CanWrite)
                {
                    property.SetValue(this, property.GetValue(obj));
                }
            }
        }

        public void SaveJsonComponent()
        {
            base.SaveJsonComponent($@"{ConstConfig.SamplerCreateInfoPath}{this._name}.json", this);
        }
    }
}

