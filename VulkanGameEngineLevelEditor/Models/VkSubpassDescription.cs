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
    public unsafe class VkSubpassDescription
    {
        public SubpassDescriptionFlags Flags { get; set; }
        public PipelineBindPoint PipelineBindPoint { get; set; }
        public uint InputAttachmentCount { get; set; }
        public VkAttachmentReference* pInputAttachments { get; set; }
        public uint ColorAttachmentCount { get; set; }
        public VkAttachmentReference* pColorAttachments { get; set; }
        public VkAttachmentReference* pResolveAttachments { get; set; }
        public VkAttachmentReference* pDepthStencilAttachment { get; set; }
        public uint PreserveAttachmentCount { get; set; }
        public uint* pPreserveAttachments { get; set; }

        public VkSubpassDescription()
        { }

        public VkSubpassDescription(SubpassDescription other)
        {
            Flags = other.Flags;
            PipelineBindPoint = other.PipelineBindPoint;
            InputAttachmentCount = other.InputAttachmentCount;

            if (other.PInputAttachments != null)
            {
                pInputAttachments = (VkAttachmentReference*)Marshal.AllocHGlobal((IntPtr)(sizeof(VkAttachmentReference) * InputAttachmentCount));
                for (uint i = 0; i < InputAttachmentCount; i++)
                {
                    pInputAttachments[i] = new VkAttachmentReference(other.PInputAttachments[i]);
                }
            }
            else
            {
                pInputAttachments = null;
            }

            ColorAttachmentCount = other.ColorAttachmentCount;

            if (other.PColorAttachments != null)
            {
                pColorAttachments = (VkAttachmentReference*)Marshal.AllocHGlobal((IntPtr)(sizeof(VkAttachmentReference) * ColorAttachmentCount));
                for (uint i = 0; i < ColorAttachmentCount; i++)
                {
                    pColorAttachments[i] = new VkAttachmentReference(other.PColorAttachments[i]);
                }
            }
            else
            {
                pColorAttachments = null;
            }

            if (other.PResolveAttachments != null)
            {
                pResolveAttachments = (VkAttachmentReference*)Marshal.AllocHGlobal((IntPtr)(sizeof(VkAttachmentReference) * other.ColorAttachmentCount));
                for (uint i = 0; i < other.ColorAttachmentCount; i++)
                {
                    pResolveAttachments[i] = new VkAttachmentReference(other.PResolveAttachments[i]);
                }
            }
            else
            {
                pResolveAttachments = null;
            }

            if (other.PDepthStencilAttachment != null)
            {
                pDepthStencilAttachment = (VkAttachmentReference*)Marshal.AllocHGlobal(sizeof(VkAttachmentReference));
                *pDepthStencilAttachment = new VkAttachmentReference(other.PDepthStencilAttachment[0]);
            }
            else
            {
                pDepthStencilAttachment = null;
            }

            PreserveAttachmentCount = other.PreserveAttachmentCount;

            if (other.PPreserveAttachments != null)
            {
                pPreserveAttachments = (uint*)Marshal.AllocHGlobal((IntPtr)(sizeof(uint) * PreserveAttachmentCount));
                for (uint i = 0; i < PreserveAttachmentCount; i++)
                {
                    pPreserveAttachments[i] = other.PPreserveAttachments[i];
                }
            }
            else
            {
                pPreserveAttachments = null;
            }
        }

        public SubpassDescription Convert()
        {
            return new SubpassDescription
            {
                Flags = Flags,
                PipelineBindPoint = PipelineBindPoint,
                InputAttachmentCount = InputAttachmentCount,
                PInputAttachments = (AttachmentReference*)pInputAttachments,
                ColorAttachmentCount = ColorAttachmentCount,
                PColorAttachments = (AttachmentReference*)pColorAttachments,
                PResolveAttachments = (AttachmentReference*)pResolveAttachments,
                PDepthStencilAttachment = (AttachmentReference*)pDepthStencilAttachment,
                PreserveAttachmentCount = PreserveAttachmentCount,
                PPreserveAttachments = pPreserveAttachments
            };
        }

        public void Dispose()
        {
            if (pInputAttachments != null)
            {
                Marshal.FreeHGlobal((IntPtr)pInputAttachments);
            }
            if (pColorAttachments != null)
            {
                Marshal.FreeHGlobal((IntPtr)pColorAttachments);
            }
            if (pResolveAttachments != null)
            {
                Marshal.FreeHGlobal((IntPtr)pResolveAttachments);
            }
            if (pDepthStencilAttachment != null)
            {
                Marshal.FreeHGlobal((IntPtr)pDepthStencilAttachment);
            }
            if (pPreserveAttachments != null)
            {
                Marshal.FreeHGlobal((IntPtr)pPreserveAttachments);
            }
        }
    }
}
