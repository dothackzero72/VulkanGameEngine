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
using VulkanGameEngineLevelEditor.Vulkan;
using VulkanGameEngineLevelEditor;
using AutoMapper;

namespace VulkanGameEngineLevelEditor.Models
{
    [Serializable]
    public unsafe class VkSamplerCreateInfoModel : RenderPassEditorBaseModel
    {
        IMapper _mapper;
        private VkStructureType _sType = VkStructureType.VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO;
        private VkSamplerCreateFlagBits _flags = 0;
        private void* _pNext = null;
        private VkFilter _magFilter;
        private VkFilter _minFilter;
        private VkSamplerMipmapMode _mipmapMode;
        private VkSamplerAddressMode _addressModeU;
        private VkSamplerAddressMode _addressModeV;
        private VkSamplerAddressMode _addressModeW;
        private float _mipLodBias;
        private bool _anisotropyEnable;
        private float _maxAnisotropy;
        private bool _compareEnable;
        private VkCompareOp _compareOp;
        private float _minLod;
        private float _maxLod;
        private VkBorderColor _borderColor;
        private bool _unnormalizedCoordinates;

        public VkSamplerCreateInfoDLL DLL => _mapper.Map<VkSamplerCreateInfoDLL>(this);

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

        [Category("Sampler Properties")]
        [Browsable(false)]
        [Newtonsoft.Json.JsonIgnore]
        public VkSamplerCreateFlagBits flags
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
        public VkFilter magFilter
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
        public VkFilter minFilter
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
        public VkSamplerMipmapMode mipmapMode
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
        public VkSamplerAddressMode addressModeU
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
        public VkSamplerAddressMode addressModeV
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
        public VkSamplerAddressMode addressModeW
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
        public VkCompareOp compareOp
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
        public VkBorderColor borderColor
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

        public VkSamplerCreateInfoModel() : base()
        {
        }

        public VkSamplerCreateInfoModel(string jsonFilePath) : base()
        {
            LoadJsonComponent(jsonFilePath);
        }

        public VkSamplerCreateInfoModel(string name, string jsonFilePath) : base(name)
        {
           // LoadJsonComponent(@"C:\Users\dotha\Documents\GitHub\VulkanGameEngine\RenderPass\  \DefaultSubpassDependency.json");
        }

        public VkSamplerCreateInfo Convert()
        {
            return new VkSamplerCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO,
                addressModeU = addressModeU,
                addressModeV = addressModeV,
                addressModeW = addressModeW,
                anisotropyEnable = anisotropyEnable,
                maxAnisotropy = maxAnisotropy,
                compareEnable = compareEnable,
                minLod = minLod,
                maxLod = maxLod,
                borderColor = borderColor,
                unnormalizedCoordinates = unnormalizedCoordinates,
                compareOp = compareOp,
                flags = flags,
                magFilter = magFilter,
                minFilter = minFilter,
                mipLodBias = mipLodBias,
                mipmapMode = mipmapMode,
                pNext = null
            };
        }

        public VkSamplerCreateInfoDLL ToDLL()
        {
            fixed (byte* namePtr = System.Text.Encoding.UTF8.GetBytes(_name + "\0"))
            {
                return new VkSamplerCreateInfoDLL
                {
                    Name = (IntPtr)namePtr,
                    _addressModeU = addressModeU,
                    _addressModeV = addressModeV,
                    _addressModeW = addressModeW,
                    _pNext = null,
                    _anisotropyEnable = anisotropyEnable,
                    _maxAnisotropy = maxAnisotropy,
                    _compareEnable = _compareEnable,
                    _minFilter = _minFilter,
                    _magFilter = _magFilter,
                    _borderColor = _borderColor,
                    _unnormalizedCoordinates = _unnormalizedCoordinates,
                    _compareOp = _compareOp,
                    _flags = _flags,
                    _maxLod = _maxLod,
                    _minLod = _minLod,
                    _mipLodBias = _mipLodBias,
                    _mipmapMode = _mipmapMode,
                    _sType = _sType,
                };
            }
        }

        public void LoadJsonComponent(string jsonPath)
        {
            var obj = base.LoadJsonComponent<VkSamplerCreateInfoModel>(jsonPath);
            foreach (PropertyInfo property in typeof(VkSamplerCreateInfoModel).GetProperties())
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

