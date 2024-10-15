using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.RenderPassEditor;

namespace VulkanGameEngineLevelEditor.Models
{
    [Serializable]
    public class AttachmentDescriptionModel : RenderPassEditorBaseModel
    {
        private AttachmentDescriptionFlags _flags;
        private Format _format;
        private SampleCountFlags _samples;
        private AttachmentLoadOp _loadOp;
        private AttachmentStoreOp _storeOp;
        private AttachmentLoadOp _stencilLoadOp;
        private AttachmentStoreOp _stencilStoreOp;
        private ImageLayout _initialLayout;
        private ImageLayout _finalLayout;

        [Category("Attachment Description")]
        public AttachmentDescriptionFlags Flags
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

        [Category("Attachment Description")]
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

        [Category("Attachment Description")]
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

        [Category("Load/Store Operations")]
        public AttachmentLoadOp LoadOp
        {
            get => _loadOp;
            set
            {
                if (_loadOp != value)
                {
                    _loadOp = value;
                    OnPropertyChanged(nameof(LoadOp));
                }
            }
        }

        [Category("Load/Store Operations")]
        public AttachmentStoreOp StoreOp
        {
            get => _storeOp;
            set
            {
                if (_storeOp != value)
                {
                    _storeOp = value;
                    OnPropertyChanged(nameof(StoreOp));
                }
            }
        }

        [Category("Load/Store Operations")]
        public AttachmentLoadOp StencilLoadOp
        {
            get => _stencilLoadOp;
            set
            {
                if (_stencilLoadOp != value)
                {
                    _stencilLoadOp = value;
                    OnPropertyChanged(nameof(StencilLoadOp));
                }
            }
        }

        [Category("Load/Store Operations")]
        public AttachmentStoreOp StencilStoreOp
        {
            get => _stencilStoreOp;
            set
            {
                if (_stencilStoreOp != value)
                {
                    _stencilStoreOp = value;
                    OnPropertyChanged(nameof(StencilStoreOp));
                }
            }
        }

        [Category("Attachment Description")]
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

        [Category("Attachment Description")]
        public ImageLayout FinalLayout
        {
            get => _finalLayout;
            set
            {
                if (_finalLayout != value)
                {
                    _finalLayout = value;
                    OnPropertyChanged(nameof(FinalLayout));
                }
            }
        }

        public AttachmentDescriptionModel() : base()
        {
        }

        public AttachmentDescriptionModel(string jsonFilePath) : base()
        {
            LoadJsonComponent(jsonFilePath);
        }

        public AttachmentDescriptionModel(string name, string jsonFilePath) : base(name)
        {
            LoadJsonComponent(RenderPassEditorConsts.DefaultColorAttachmentDescriptionModel);
        }

        public AttachmentDescription ConvertToVulkan()
        {
            return new AttachmentDescription
            {
                Flags = Flags,
                Format = Format,
                Samples = Samples,
                LoadOp = LoadOp,
                StoreOp = StoreOp,
                StencilLoadOp = StencilLoadOp,
                StencilStoreOp = StencilStoreOp,
                InitialLayout = InitialLayout,
                FinalLayout = FinalLayout
            };
        }

        public void LoadJsonComponent(string jsonPath)
        {
            var obj = base.LoadJsonComponent<AttachmentDescriptionModel>(jsonPath);
            foreach (PropertyInfo property in typeof(AttachmentDescriptionModel).GetProperties())
            {
                if (property.CanWrite)
                {
                    property.SetValue(this, property.GetValue(obj));
                }
            }
        }
    }
}
