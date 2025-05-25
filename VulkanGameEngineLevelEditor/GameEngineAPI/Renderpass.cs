using GlmSharp;
using Silk.NET.Vulkan;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public class SilkRenderPassBase
    {
        public ivec2 RenderPassResolution { get; set; }
        public SampleCountFlags sampleCount { get; set; }
        public VkRenderPass renderPass { get; protected set; }
        public ListPtr<VkCommandBuffer> commandBufferList { get; protected set; }
        public ListPtr<VkFramebuffer> FrameBufferList { get; protected set; }
        //public DescriptorPool descriptorpool { get; protected set; }
        //public List<DescriptorSetLayout> descriptorSetLayoutList { get; protected set; } = new List<DescriptorSetLayout>();
        //public DescriptorSet descriptorset { get; protected set; }
        //public Pipeline shaderpipeline { get; protected set; }
        //public PipelineLayout shaderpipelineLayout { get; protected set; }
        //public PipelineCache pipelineCache { get; protected set; }

        public SilkRenderPassBase()
        {
            RenderPassResolution = new ivec2
            {
                x = (int)RenderSystem.SwapChainResolution.width,
                y = (int)RenderSystem.SwapChainResolution.height
            };
            sampleCount = SampleCountFlags.Count1Bit;

            FrameBufferList = new ListPtr<VkFramebuffer>(RenderSystem.SwapChainImageCount);
            commandBufferList = new ListPtr<VkCommandBuffer>(RenderSystem.SwapChainImageCount);
            RenderSystem.CreateCommandBuffers(commandBufferList);
        }
    }
}
