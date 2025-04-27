using GlmSharp;
using System;
using System.Linq;
using VulkanGameEngineLevelEditor.RenderPassEditor;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class FrameBufferRenderPass : JsonRenderPass<NullVertex>
    {
        public FrameBufferRenderPass()
        {

        }

        public void BuildRenderPass(String jsonFile, GPUImport<NullVertex> renderGraphics)
        {
            RenderPassResolution = new ivec2((int)VulkanRenderer.SwapChain.SwapChainResolution.width, (int)VulkanRenderer.SwapChain.SwapChainResolution.height);
            ListPtr<VkVertexInputBindingDescription> vertexBinding = NullVertex.GetBindingDescriptions();
            ListPtr<VkVertexInputAttributeDescription> vertexAttribute = NullVertex.GetAttributeDescriptions();

            CreateJsonRenderPass(jsonFile, RenderPassResolution);
            jsonPipelineList.Add(new JsonPipeline<NullVertex>(ConstConfig.DefaulFrameBufferPipeline, renderPass, 0, renderGraphics, RenderPassResolution));
        }

        public unsafe VkCommandBuffer Draw()
        {
            var commandIndex = VulkanRenderer.CommandIndex;
            var imageIndex = VulkanRenderer.ImageIndex;
            var commandBuffer = commandBufferList[(int)commandIndex];

            var renderPassInfo = RenderPassInfo;
            renderPassInfo.framebuffer = frameBufferList[(int)imageIndex];

            VkCommandBufferBeginInfo commandInfo = new VkCommandBufferBeginInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
                flags = VkCommandBufferUsageFlagBits.VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT
            };

            VkFunc.vkBeginCommandBuffer(commandBuffer, &commandInfo);
            VkFunc.vkCmdBeginRenderPass(commandBuffer, &renderPassInfo, VkSubpassContents.VK_SUBPASS_CONTENTS_INLINE);
            VkFunc.vkCmdBindPipeline(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, jsonPipelineList.First().pipeline);
            VkFunc.vkCmdBindDescriptorSets(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, jsonPipelineList.First().pipelineLayout, 0, jsonPipelineList[0].descriptorSetList.UCount, jsonPipelineList[0].descriptorSetList.Ptr, 0, null);
            VkFunc.vkCmdDraw(commandBuffer, 6, 1, 0, 0);
            VkFunc.vkCmdEndRenderPass(commandBuffer);
            VkFunc.vkEndCommandBuffer(commandBuffer);

            return commandBuffer;

        }
    }
}