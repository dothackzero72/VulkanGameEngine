using Silk.NET.Vulkan;
using System.Runtime.InteropServices;

namespace VulkanGameEngineLevelEditor.Models
{
    public unsafe class VkRect2DModel
    {
        public VkOffset2DModel offset { get; set; }
        public VkExtent2DModel extent { get; set; }

        public VkRect2DModel()
        {
            offset = new VkOffset2DModel();
            extent = new VkExtent2DModel();
        }

        public VkRect2DModel(VkOffset2DModel offset, VkExtent2DModel extent)
        {
            this.offset = offset;
            this.extent = extent;
        }

        public VkRect2DModel(Rect2D other)
        {
            offset = new VkOffset2DModel()
            {
                x = other.Offset.X,
                y = other.Offset.Y
            };
            extent = new VkExtent2DModel()
            {
                width = other.Extent.Width,
                height = other.Extent.Height
            };
        }

        public Rect2D Convert()
        {
            return new Rect2D
            {
                Offset = new Offset2D()
                {
                    X = offset.x,
                    Y = offset.y
                },
                Extent = new Extent2D()
                {
                    Width = extent.width,
                    Height = extent.height
                }
            };
        }

        public Rect2D* ConvertPtr()
        {
            Rect2D* rect = (Rect2D*)Marshal.AllocHGlobal(sizeof(Rect2D));
            rect->Offset = new Offset2D()
            {
                X = offset.x,
                Y = offset.y
            };
            rect->Extent = new Extent2D()
            {
                Width = extent.width,
                Height = extent.height
            };
            return rect;
        }

        public void Dispose()
        {
            // Implement disposal logic if necessary
        }
    }
}
