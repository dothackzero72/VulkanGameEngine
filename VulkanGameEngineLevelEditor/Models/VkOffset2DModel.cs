using Silk.NET.Vulkan;
using System.Runtime.InteropServices;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.Models
{
    public unsafe class VkOffset2DModel
    {
        public int x { get; set; }
        public int y { get; set; }

        public VkOffset2DModel() { }
        public VkOffset2DModel(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public VkOffset2DModel(Offset2D other)
        {
            x = other.X;
            y = other.Y;
        }

        public VkOffset2D Convert()
        {
            return new VkOffset2D
            {
                x = x,
                y = y
            };
        }

        public VkOffset2D* ConvertPtr()
        {
            VkOffset2D* nativeOffset = (VkOffset2D*)Marshal.AllocHGlobal(sizeof(VkOffset2D));
            nativeOffset->x = x;
            nativeOffset->y = y;
            return nativeOffset;
        }

        public void Dispose()
        {
            // Implement disposal logic if necessary
        }
    }
}
