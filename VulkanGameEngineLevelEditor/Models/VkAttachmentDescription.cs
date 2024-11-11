using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.RenderPassEditor;

namespace VulkanGameEngineLevelEditor.Models
{
    [Serializable]
    public unsafe class VkAttachmentDescription : RenderPassEditorBaseModel
    {
        private StructureType _structureType;
        private AttachmentDescriptionFlags _flags;
        private void* _pNext;
        private Format _format;
        private SampleCountFlags _samples;
        private AttachmentLoadOp _loadOp;
        private AttachmentStoreOp _storeOp;
        private AttachmentLoadOp _stencilLoadOp;
        private AttachmentStoreOp _stencilStoreOp;
        private ImageLayout _initialLayout;
        private ImageLayout _finalLayout;

        [Category("Attachment Description")]
        public AttachmentDescriptionFlags flags
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

        [Category("Attachment Description")]
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

        [Category("Attachment Description")]
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

        [Category("Load/Store Operations")]
        public AttachmentLoadOp loadOp
        {
            get => _loadOp;
            set
            {
                if (_loadOp != value)
                {
                    _loadOp = value;
                    OnPropertyChanged(nameof(loadOp));
                }
            }
        }

        [Category("Load/Store Operations")]
        public AttachmentStoreOp storeOp
        {
            get => _storeOp;
            set
            {
                if (_storeOp != value)
                {
                    _storeOp = value;
                    OnPropertyChanged(nameof(storeOp));
                }
            }
        }

        [Category("Load/Store Operations")]
        public AttachmentLoadOp stencilLoadOp
        {
            get => _stencilLoadOp;
            set
            {
                if (_stencilLoadOp != value)
                {
                    _stencilLoadOp = value;
                    OnPropertyChanged(nameof(stencilLoadOp));
                }
            }
        }

        [Category("Load/Store Operations")]
        public AttachmentStoreOp stencilStoreOp
        {
            get => _stencilStoreOp;
            set
            {
                if (_stencilStoreOp != value)
                {
                    _stencilStoreOp = value;
                    OnPropertyChanged(nameof(stencilStoreOp));
                }
            }
        }

        [Category("Attachment Description")]
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

        [Category("Attachment Description")]
        public ImageLayout finalLayout
        {
            get => _finalLayout;
            set
            {
                if (_finalLayout != value)
                {
                    _finalLayout = value;
                    OnPropertyChanged(nameof(finalLayout));
                }
            }
        }

        public VkAttachmentDescription() : base()
        {
        }

        public VkAttachmentDescription(string jsonFilePath) : base()
        {
            LoadJsonComponent(jsonFilePath);
        }

        public VkAttachmentDescription(string name, string jsonFilePath) : base(name)
        {
            LoadJsonComponent(RenderPassEditorConsts.DefaultColorAttachmentDescriptionModel);
        }

        public VkAttachmentDescription(AttachmentDescription other)
        {
            flags = other.Flags;
            format = other.Format;
            samples = other.Samples;
            loadOp = other.LoadOp;
            storeOp = other.StoreOp;
            stencilLoadOp = other.StencilLoadOp;
            stencilStoreOp = other.StencilStoreOp;
            initialLayout = other.InitialLayout;
            finalLayout = other.FinalLayout;
        }

        public AttachmentDescription Convert()
        {
            return new AttachmentDescription
            {
                Flags = flags,
                Format = format,
                Samples = samples,
                LoadOp = loadOp,
                StoreOp = storeOp,
                StencilLoadOp = stencilLoadOp,
                StencilStoreOp = stencilStoreOp,
                InitialLayout = initialLayout,
                FinalLayout = finalLayout
            };
        }

        public AttachmentDescription* ConvertPtr()
        {
            AttachmentDescription* attachmentDescription = (AttachmentDescription*)Marshal.AllocHGlobal(sizeof(AttachmentDescription));
            attachmentDescription->Flags = flags;
            attachmentDescription->Format = format;
            attachmentDescription->Samples = samples;
            attachmentDescription->LoadOp = loadOp;
            attachmentDescription->StoreOp = storeOp;
            attachmentDescription->StencilLoadOp = stencilLoadOp;
            attachmentDescription->StencilStoreOp = stencilStoreOp;
            attachmentDescription->InitialLayout = initialLayout;
            attachmentDescription->FinalLayout = finalLayout;
            return attachmentDescription;
        }

        public void LoadJsonComponent(string jsonPath)
        {
            var obj = base.LoadJsonComponent<VkAttachmentDescription>(jsonPath);
            foreach (PropertyInfo property in typeof(VkAttachmentDescription).GetProperties())
            {
                if (property.CanWrite)
                {
                    property.SetValue(this, property.GetValue(obj));
                }
            }
        }

        public void SaveJsonComponent()
        {
            base.SaveJsonComponent($@"{RenderPassEditorConsts.AttachmentDescriptionModelPath}{this._name}.json", this);
        }
    }
}
