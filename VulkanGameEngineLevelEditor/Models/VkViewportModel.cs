using Silk.NET.Vulkan;

namespace VulkanGameEngineLevelEditor.Models
{
    public class VkViewportModel
    {
        public float x { get; set; }
        public float y { get; set; }
        public float width { get; set; }
        public float height { get; set; }
        public float minDepth { get; set; }
        public float maxDepth { get; set; }

        public VkViewportModel()
        { }

        public VkViewportModel(Viewport other)
        {
            x = other.X;
            y = other.Y;
            width = other.Width;
            height = other.Height;
            minDepth = other.MinDepth;
            maxDepth = other.MaxDepth;
        }

        public void Dispose()
        {
        }
    }
}
