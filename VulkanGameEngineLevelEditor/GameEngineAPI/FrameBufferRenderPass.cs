using GlmSharp;
using Newtonsoft.Json;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineGameObjectScripts.Vulkan;
using VulkanGameEngineLevelEditor.Models;
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

            CreateJsonRenderPass($@"{ConstConfig.RenderPassBasePath}//{ConstConfig.FrameBufferRenderPass}", RenderPassResolution);
            jsonPipelineList.Add(new JsonPipeline<NullVertex>($@"{ConstConfig.PipelineBasePath}//{ConstConfig.FrameBufferPipeline}", renderPass, 0, vertexBinding, vertexAttribute, renderGraphics, RenderPassResolution));
        }

        public unsafe VkCommandBuffer Draw()
        {
            var commandIndex = VulkanRenderer.CommandIndex;
            var imageIndex = VulkanRenderer.ImageIndex;
            var commandBuffer = commandBufferList[(int)commandIndex];

            VkClearValue* clearValues = stackalloc[]
            {
                new VkClearValue(new VkClearColorValue(1, 1, 0, 1))
            };

            VkRenderPassBeginInfo renderPassInfo = new VkRenderPassBeginInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
                renderPass = renderPass,
                renderArea = new VkRect2D(new VkOffset2D(0, 0), VulkanRenderer.SwapChain.SwapChainResolution),
                clearValueCount = 1,
                framebuffer = frameBufferList[(int)imageIndex],
                pClearValues = clearValues,
                pNext = IntPtr.Zero
            };

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