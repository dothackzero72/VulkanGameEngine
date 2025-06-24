using Newtonsoft.Json;
using System.Runtime.InteropServices;

namespace VulkanGameEngineLevelEditor.Models
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public unsafe class VkPipelineInputAssemblyStateCreateInfoModel
    {
        public VkStructureType sType { get; set; } = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_INPUT_ASSEMBLY_STATE_CREATE_INFO;
        public VkPrimitiveTopology topology { get; set; }
        public VkBool32 primitiveRestartEnable { get; set; }
        public uint flags { get; set; } = 0;
        [JsonIgnore]
        public void* pNext { get; set; } = null;
        public VkPipelineInputAssemblyStateCreateInfoModel()
        {
        }

        public VkPipelineInputAssemblyStateCreateInfo Convert()
        {
            return new VkPipelineInputAssemblyStateCreateInfo
            {
                sType = sType,
                topology = topology,
                primitiveRestartEnable = primitiveRestartEnable,
                flags = 0,
                pNext = null
            };
        }
    }
}
