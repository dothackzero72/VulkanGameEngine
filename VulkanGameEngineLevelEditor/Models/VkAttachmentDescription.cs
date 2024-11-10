using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.Models
{
    public unsafe class VkAttachmentDescription
    {
        public AttachmentDescriptionFlags flags { get; set; }
        public Format format { get; set; }
        public SampleCountFlags samples { get; set; }
        public AttachmentLoadOp loadOp { get; set; }
        public AttachmentStoreOp storeOp { get; set; }
        public AttachmentLoadOp stencilLoadOp { get; set; }
        public AttachmentStoreOp stencilStoreOp { get; set; }
        public Silk.NET.Vulkan.ImageLayout initialLayout { get; set; }
        public Silk.NET.Vulkan.ImageLayout finalLayout { get; set; }

        public VkAttachmentDescription()
        { }

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

        public static implicit operator VkAttachmentDescription(AttachmentDescription other)
        {
            return new VkAttachmentDescription(other);
        }
    }
}
