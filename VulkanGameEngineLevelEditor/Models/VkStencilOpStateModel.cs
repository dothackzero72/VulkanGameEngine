using System.Runtime.InteropServices;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.Models
{
    public unsafe class VkStencilOpStateModel
    {
        public VkStencilOp failOp { get; set; } = 0;
        public VkStencilOp passOp { get; set; } = 0;
        public VkStencilOp depthFailOp { get; set; } = 0;
        public VkCompareOp compareOp { get; set; } = 0;
        public uint compareMask { get; set; } = 0;
        public uint writeMask { get; set; } = 0;
        public uint reference { get; set; } = 0;
        public VkStencilOpStateModel() { }

        public VkStencilOpState Convert()
        {
            return new VkStencilOpState
            {
                compareMask = compareMask,
                writeMask = writeMask,
                reference = reference,
                compareOp = compareOp,
                depthFailOp = depthFailOp,
                failOp = failOp,
                passOp = passOp
            };
        }

        public VkStencilOpState* ConvertPtr()
        {
            VkStencilOpState* ptr = (VkStencilOpState*)Marshal.AllocHGlobal(sizeof(VkStencilOpState));
            ptr->failOp = failOp;
            ptr->passOp = passOp;
            ptr->depthFailOp = depthFailOp;
            ptr->compareOp = compareOp;
            ptr->compareMask = compareMask;
            ptr->writeMask = writeMask;
            ptr->reference = reference;
            return ptr;
        }
    }
}
