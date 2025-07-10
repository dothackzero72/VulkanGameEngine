using System.Runtime.InteropServices;
using Vulkan;


namespace VulkanGameEngineLevelEditor.Models
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct RenderAreaModel
    {
        public VkRect2D RenderArea { get; set; }
        public bool UseDefaultRenderArea { get; set; }
    }
}
