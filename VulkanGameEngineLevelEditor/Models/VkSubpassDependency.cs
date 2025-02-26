using Newtonsoft.Json;
using Silk.NET.Core.Attributes;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VulkanGameEngineLevelEditor.EditorEnhancements;
using VulkanGameEngineLevelEditor.RenderPassEditor;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.Models
{
    [Serializable]
    public class VkSubpassDependency : RenderPassEditorBaseModel
    {
        private uint _srcSubpass;
        private uint _dstSubpass;
        private VkPipelineStageFlagBits _srcStageMask;
        private VkPipelineStageFlagBits _dstStageMask;
        private VkAccessFlags _srcAccessMask;
        private VkAccessFlags _dstAccessMask;
        private VkAccessFlagBits _dependencyFlags;


        [Category("Subpass Dependency")]
        public uint srcSubpass
        {
            get => _srcSubpass;
            set
            {
                if (_srcSubpass != value)
                {
                    _srcSubpass = value;
                    OnPropertyChanged(nameof(srcSubpass));
                }
            }
        }

        [Category("Subpass Dependency")]
        public uint dstSubpass
        {
            get => _dstSubpass;
            set
            {
                if (_dstSubpass != value)
                {
                    _dstSubpass = value;
                    OnPropertyChanged(nameof(dstSubpass));
                }
            }
        }

        [Category("Pipeline Stages")]
        [Browsable(true)]
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public VkPipelineStageFlagBits srcStageMask
        {
            get => _srcStageMask;
            set
            {
                if (_srcStageMask != value)
                {
                    _srcStageMask = value;
                    OnPropertyChanged(nameof(srcStageMask));
                }
            }
        }

        [Category("Pipeline Stages")]
        [Browsable(true)]
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public VkPipelineStageFlagBits dstStageMask
        {
            get => _dstStageMask;
            set
            {
                if (_dstStageMask != value)
                {
                    _dstStageMask = value;
                    OnPropertyChanged(nameof(dstStageMask));
                }
            }
        }

        [Category("Access Masks")]
        [Browsable(true)]
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public VkAccessFlags srcAccessMask
        {
            get => _srcAccessMask;
            set
            {
                if (_srcAccessMask != value)
                {
                    _srcAccessMask = value;
                    OnPropertyChanged(nameof(srcAccessMask));
                }
            }
        }

        [Category("Access Masks")]
        [Browsable(true)]
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public VkAccessFlags dstAccessMask
        {
            get => _dstAccessMask;
            set
            {
                if (_dstAccessMask != value)
                {
                    _dstAccessMask = value;
                    OnPropertyChanged(nameof(dstAccessMask));
                }
            }
        }

        [Category("Subpass Dependency")]
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public VkAccessFlagBits dependencyFlags
        {
            get => _dependencyFlags;
            set
            {
                if (_dependencyFlags != value)
                {
                    _dependencyFlags = value;
                    OnPropertyChanged(nameof(dependencyFlags));
                }
            }
        }

        public VkSubpassDependency() : base()
        {
        }
        public VkSubpassDependency(string jsonFilePath) : base()
        {
            LoadJsonComponent(jsonFilePath);
        }

        public VkSubpassDependency(string name, string jsonFilePath) : base(name)
        {
            LoadJsonComponent(ConstConfig.DefaultSubpassDependencyModel);
        }

        public VkSubpassDependency(uint? srcSubpass = null, uint? dstSubpass = null, VkPipelineStageFlagBits? srcStageMask = null, VkPipelineStageFlagBits? dstStageMask = null, VkAccessFlags? srcAccessMask = null, VkAccessFlags? dstAccessMask = null, VkAccessFlagBits? dependencyFlags = null)
        {
            if (srcSubpass.HasValue)
            {
                this.srcSubpass = srcSubpass.Value;
            }

            if (dstSubpass.HasValue)
            {
                this.dstSubpass = dstSubpass.Value;
            }

            if (srcStageMask.HasValue)
            {
                this.srcStageMask = srcStageMask.Value;
            }

            if (dstStageMask.HasValue)
            {
                this.dstStageMask = dstStageMask.Value;
            }

            if (srcAccessMask.HasValue)
            {
                this.srcAccessMask = srcAccessMask.Value;
            }

            if (dstAccessMask.HasValue)
            {
                this.dstAccessMask = dstAccessMask.Value;
            }

            if (dependencyFlags.HasValue)
            {
                this.dependencyFlags = dependencyFlags.Value;
            }
        }

        public void LoadJsonComponent(string jsonPath)
        {
            var obj = base.LoadJsonComponent<VkSubpassDependency>(jsonPath);
            foreach (PropertyInfo property in typeof(VkSubpassDependency).GetProperties())
            {
                if (property.CanWrite)
                {
                    property.SetValue(this, property.GetValue(obj));
                }
            }
        }

        public void SaveJsonComponent()
        {
            base.SaveJsonComponent($@"{ConstConfig.SubpassDependencyModelPath}{this._name}.json", this);
        }
    }
}