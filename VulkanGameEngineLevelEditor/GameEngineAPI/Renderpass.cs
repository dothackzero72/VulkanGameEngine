using GlmSharp;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public class SilkRenderPassBase
    {
        public ivec2 RenderPassResolution { get; set; }
        public SampleCountFlags sampleCount { get; set; }
        public RenderPass renderPass { get; protected set; }
        public CommandBuffer[] commandBufferList { get; protected set; }
        public Framebuffer[] FrameBufferList { get; protected set; }
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
                x = (int)VulkanRenderer.swapChain.swapchainExtent.Width,
                y = (int)VulkanRenderer.swapChain.swapchainExtent.Height
            };
            sampleCount = SampleCountFlags.Count1Bit;

            FrameBufferList = new Framebuffer[VulkanRenderer.MAX_FRAMES_IN_FLIGHT];
            commandBufferList = new CommandBuffer[VulkanRenderer.MAX_FRAMES_IN_FLIGHT];
            VulkanRenderer.CreateCommandBuffers(commandBufferList);
        }
    }
}
