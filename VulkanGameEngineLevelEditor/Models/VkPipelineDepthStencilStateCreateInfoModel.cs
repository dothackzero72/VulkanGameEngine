using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace VulkanGameEngineLevelEditor.Models
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public unsafe struct VkPipelineDepthStencilStateCreateInfoModel
    {
        public VkStructureType sType { get; set; } = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_DEPTH_STENCIL_STATE_CREATE_INFO;
        public VkBool32 depthTestEnable { get; set; }
        public VkBool32 depthWriteEnable { get; set; }
        public VkCompareOp depthCompareOp { get; set; }
        public VkBool32 depthBoundsTestEnable { get; set; }
        public VkBool32 stencilTestEnable { get; set; }
        public VkStencilOpStateModel front { get; set; } = new VkStencilOpStateModel();
        public VkStencilOpStateModel back { get; set; } = new VkStencilOpStateModel();
        public float minDepthBounds { get; set; }
        public float maxDepthBounds { get; set; }
        public uint flags { get; set; } = 0;
        [JsonIgnore]
        public void* pNext { get; set; } = null;
        public VkPipelineDepthStencilStateCreateInfoModel() { }

        public VkPipelineDepthStencilStateCreateInfo Convert()
        {
            front = new VkStencilOpStateModel();
            back = new VkStencilOpStateModel();
            return new VkPipelineDepthStencilStateCreateInfo
            {
                sType = sType,
                depthTestEnable = depthTestEnable,
                depthWriteEnable = depthWriteEnable,
                depthCompareOp = depthCompareOp,
                depthBoundsTestEnable = depthBoundsTestEnable,
                stencilTestEnable = stencilTestEnable,
                minDepthBounds = minDepthBounds,
                maxDepthBounds = maxDepthBounds,
                front = front.Convert(),
                back = back.Convert(),
                flags = 0,
                pNext = null
            };
        }

    }
}
