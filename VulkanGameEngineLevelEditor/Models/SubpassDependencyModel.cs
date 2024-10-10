using Silk.NET.Core.Attributes;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.Models
{
    public class SubpassDependencyModel
    {
        public string Name { get; set; }
        public uint SrcSubpass { get; set; }
        public uint DstSubpass { get; set; }
        public PipelineStageFlags SrcStageMask { get; set; }
        public PipelineStageFlags DstStageMask { get; set; }
        public AccessFlags SrcAccessMask { get; set; }
        public AccessFlags DstAccessMask { get; set; }
        public DependencyFlags DependencyFlags { get; set; }

        public SubpassDependencyModel(string name) 
        { 
            Name = name;
        }

        public SubpassDependencyModel(uint? srcSubpass = null, uint? dstSubpass = null, PipelineStageFlags? srcStageMask = null, PipelineStageFlags? dstStageMask = null, AccessFlags? srcAccessMask = null, AccessFlags? dstAccessMask = null, DependencyFlags? dependencyFlags = null)
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
                DstAccessMask = DstAccessMask,
                SrcAccessMask = SrcAccessMask,
                SrcStageMask = SrcStageMask,
                DstStageMask = DstStageMask,
                DependencyFlags = DependencyFlags,
                DstSubpass = DstSubpass,
                SrcSubpass = SrcSubpass
            };
        }
    }
}
