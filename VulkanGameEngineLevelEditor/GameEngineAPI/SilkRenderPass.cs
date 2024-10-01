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
        public ivec2 RenderPassResolution;
        public SampleCountFlags sampleCount;
        public RenderPass renderPass { get; protected set; }
        public CommandBuffer[] commandBufferList { get; protected set; }
        public List<Framebuffer> FrameBufferList { get; protected set; }
        public DescriptorPool descriptorpool { get; protected set; }
        public DescriptorSetLayout descriptorsetLayout { get; protected set; }
        public DescriptorSet descriptorset { get; protected set; }
        public Pipeline shaderpipeline { get; protected set; }
        public PipelineLayout shaderpipelineLayout { get; protected set; }
        public PipelineCache pipelineCache { get; protected set; }

        public SilkRenderPassBase()
        {
            RenderPassResolution = new ivec2
            {
                x = (int)SilkVulkanRenderer.swapChain.swapchainExtent.Width,
                y = (int)SilkVulkanRenderer.swapChain.swapchainExtent.Height
            };
            sampleCount = SampleCountFlags.Count1Bit;

            FrameBufferList = new List<Framebuffer>();
            for (int x = 0; x < SilkVulkanRenderer.swapChain.ImageCount; x++)
            {
                FrameBufferList.Add(new Framebuffer());
            }

            commandBufferList = new CommandBuffer[SilkVulkanRenderer.MAX_FRAMES_IN_FLIGHT];
            SilkVulkanRenderer.CreateCommandBuffers(commandBufferList);
        }
    }
}
