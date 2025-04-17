using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.Models
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct RenderAreaModel
    {
        public VkRect2D RenderArea {  get; set; }
        public bool UseDefaultRenderArea { get; set; } 
    }
}
