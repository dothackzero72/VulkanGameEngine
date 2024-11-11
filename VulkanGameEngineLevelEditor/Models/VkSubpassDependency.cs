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
        private VkPipelineStageFlags _srcStageMask;
        private VkPipelineStageFlags _dstStageMask;
        private VkAccessFlags _srcAccessMask;
        private VkAccessFlags _dstAccessMask;
        private VkDependencyFlags _dependencyFlags;


        [Category("Subpass Dependency")]
        public uint SrcSubpass
        {
            get => _srcSubpass;
            set
            {
                if (_srcSubpass != value)
                {
                    _srcSubpass = value;
                    OnPropertyChanged(nameof(SrcSubpass));
                }
            }
        }

        [Category("Subpass Dependency")]
        public uint DstSubpass
        {
            get => _dstSubpass;
            set
            {
                if (_dstSubpass != value)
                {
                    _dstSubpass = value;
                    OnPropertyChanged(nameof(DstSubpass));
                }
            }
        }

        [Category("Pipeline Stages")]
        [Browsable(true)]
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public VkPipelineStageFlags SrcStageMask
        {
            get => _srcStageMask;
            set
            {
                if (_srcStageMask != value)
                {
                    _srcStageMask = value;
                    OnPropertyChanged(nameof(SrcStageMask));
                }
            }
        }

        [Category("Pipeline Stages")]
        [Browsable(true)]
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public VkPipelineStageFlags DstStageMask
        {
            get => _dstStageMask;
            set
            {
                if (_dstStageMask != value)
                {
                    _dstStageMask = value;
                    OnPropertyChanged(nameof(DstStageMask));
                }
            }
        }

        [Category("Access Masks")]
        [Browsable(true)]
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public VkAccessFlags SrcAccessMask
        {
            get => _srcAccessMask;
            set
            {
                if (_srcAccessMask != value)
                {
                    _srcAccessMask = value;
                    OnPropertyChanged(nameof(SrcAccessMask));
                }
            }
        }

        [Category("Access Masks")]
        [Browsable(true)]
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public VkAccessFlags DstAccessMask
        {
            get => _dstAccessMask;
            set
            {
                if (_dstAccessMask != value)
                {
                    _dstAccessMask = value;
                    OnPropertyChanged(nameof(DstAccessMask));
                }
            }
        }

        [Category("Subpass Dependency")]
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public VkDependencyFlags DependencyFlags
        {
            get => _dependencyFlags;
            set
            {
                if (_dependencyFlags != value)
                {
                    _dependencyFlags = value;
                    OnPropertyChanged(nameof(DependencyFlags));
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
            LoadJsonComponent(RenderPassEditorConsts.DefaultSubpassDependencyModel);
        }

        public VkSubpassDependency(uint? srcSubpass = null, uint? dstSubpass = null, VkPipelineStageFlags? srcStageMask = null, VkPipelineStageFlags? dstStageMask = null, VkAccessFlags? srcAccessMask = null, VkAccessFlags? dstAccessMask = null, VkDependencyFlags? dependencyFlags = null)
        {
            if (srcSubpass.HasValue)
            {
                SrcSubpass = srcSubpass.Value;
            }

            if (dstSubpass.HasValue)
            {
                DstSubpass = dstSubpass.Value;
            }

            if (srcStageMask.HasValue)
            {
                SrcStageMask = srcStageMask.Value;
            }

            if (dstStageMask.HasValue)
            {
                DstStageMask = dstStageMask.Value;
            }

            if (srcAccessMask.HasValue)
            {
                SrcAccessMask = srcAccessMask.Value;
            }

            if (dstAccessMask.HasValue)
            {
                DstAccessMask = dstAccessMask.Value;
            }

            if (dependencyFlags.HasValue)
            {
                DependencyFlags = dependencyFlags.Value;
            }
        }

        public SubpassDependency ConvertToVulkan()
        {
            return new SubpassDependency()
            {
                DstAccessMask = (AccessFlags)DstAccessMask,
                SrcAccessMask = (AccessFlags)SrcAccessMask,
                SrcStageMask = (PipelineStageFlags)SrcStageMask,
                DstStageMask = (PipelineStageFlags)DstStageMask,
                DependencyFlags = (DependencyFlags)DependencyFlags,
                DstSubpass = DstSubpass,
                SrcSubpass = SrcSubpass
            };
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
            base.SaveJsonComponent($@"{RenderPassEditorConsts.SubpassDependencyModelPath}{this._name}.json", this);
        }
    }
}