using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.Models
{
    public unsafe class VkAttachmentReference
    {
        public uint attachment { get; set; }
        public Silk.NET.Vulkan.ImageLayout layout { get; set; }

        public VkAttachmentReference()
        { }

        public VkAttachmentReference(AttachmentReference other)
        {
            attachment = other.Attachment;
            layout = other.Layout;
        }

        public AttachmentReference Convert()
        {
            return new AttachmentReference
            {
                Attachment = attachment,
                Layout = layout
            };
        }

        public AttachmentReference* ConvertPtr()
        {
            AttachmentReference* attachmentReference = (AttachmentReference*)Marshal.AllocHGlobal(sizeof(AttachmentReference));
            attachmentReference->Attachment = attachment;
            attachmentReference->Layout = layout;
            return attachmentReference;
        }

        public static implicit operator VkAttachmentReference(AttachmentReference other)
        {
            return new VkAttachmentReference(other);
        }
    }
}
