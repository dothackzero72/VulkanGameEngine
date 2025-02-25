using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.Models
{
    public unsafe class VkAttachmentReference
    {
        public uint attachment { get; set; }
        public VkImageLayout layout { get; set; }

        public VkAttachmentReference()
        { }

        public VkAttachmentReference(uint attachment, VkImageLayout layout)
        {
            this.attachment = attachment;
            this.layout = layout;
        }
    }
}
