using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.Models
{
    public enum VertexTypeEnum
    {
        NullVertex = 0,
        SpriteInstanceVertex = 1,
    }
    public struct VertexLoaderModel
    {
        public VertexTypeEnum vertexType { get; set; }
        public List<VkVertexInputBindingDescription> VertexInputBindingDescriptionList { get; set; }
        public List<VkVertexInputAttributeDescription> vkVertexInputAttributeDescriptionList { get; set; }
    }
}
