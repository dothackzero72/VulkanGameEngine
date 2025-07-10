using System.Runtime.InteropServices;
using Vulkan;


namespace VulkanGameEngineLevelEditor.Models
{
    public unsafe class VkAttachmentReferenceModel
    {
        public uint attachment { get; set; }
        public VkImageLayout layout { get; set; }

        public VkAttachmentReferenceModel()
        { }

        public VkAttachmentReferenceModel(uint attachment, VkImageLayout layout)
        {
            this.attachment = attachment;
            this.layout = layout;
        }

        public VkAttachmentReference Convert()
        {
            return new VkAttachmentReference
            {
                attachment = attachment,
                layout = layout
            };
        }

        public VkAttachmentReference* ConvertPtr()
        {
            VkAttachmentReference* attachmentReference = (VkAttachmentReference*)Marshal.AllocHGlobal(sizeof(VkAttachmentReference));
            attachmentReference->attachment = attachment;
            attachmentReference->layout = layout;
            return attachmentReference;
        }

    }
}
