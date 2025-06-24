using System.Runtime.InteropServices;


namespace VulkanGameEngineLevelEditor.Models
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct RenderAreaModel
    {
        public VkRect2D RenderArea { get; set; }
        public bool UseDefaultRenderArea { get; set; }
    }
}
