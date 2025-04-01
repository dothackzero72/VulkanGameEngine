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
    public unsafe class FrameBufferRenderPass
    {
        ivec2 RenderPassResolution;
        SampleCountFlags SampleCount;

        VkRenderPass renderPass;
        List<JsonPipeline<NullVertex>> jsonPipelines = new List<JsonPipeline<NullVertex>>();
        ListPtr<VkCommandBuffer> commandBufferList = new ListPtr<VkCommandBuffer>();
        ListPtr<VkFramebuffer> frameBufferList = new ListPtr<VkFramebuffer>();

        public List<RenderedTexture> RenderedColorTextureList { get; private set; } = new List<RenderedTexture>();
        public DepthTexture depthTexture { get; private set; } = new DepthTexture();

        public FrameBufferRenderPass()
        {

        }

        public void BuildRenderPass(String jsonFile, GPUImport<NullVertex> renderGraphics)
        {
            RenderPassResolution = new ivec2((int)VulkanRenderer.SwapChain.SwapChainResolution.width, (int)VulkanRenderer.SwapChain.SwapChainResolution.height);
            SampleCount = SampleCountFlags.Count1Bit;

            RenderPassBuildInfoModel model = new RenderPassBuildInfoModel(jsonFile);
            RenderPassBuildInfoDLL modelDLL = new RenderPassBuildInfoModel(jsonFile).ToDLL();
            ListPtr<VkVertexInputBindingDescription> vertexBinding = NullVertex.GetBindingDescriptions();
            ListPtr<VkVertexInputAttributeDescription> vertexAttribute = NullVertex.GetAttributeDescriptions();

            renderPass = CreateRenderPass();
           CreateHardcodedFramebuffers();
          //   frameBufferList = new ListPtr<VkFramebuffer>(GameEngineImport.DLL_RenderPass_BuildFrameBuffer(VulkanRenderer.device, renderPass, modelDLL, null, null, VulkanRenderer.SwapChain.imageViews.Ptr, VulkanRenderer.SwapChain.imageViews.UCount, 0, RenderPassResolution), VulkanRenderer.SwapChain.imageViews.UCount);
            jsonPipelines.Add(new JsonPipeline<NullVertex>(ConstConfig.DefaulFrameBufferPipeline, renderPass, 0, vertexBinding, vertexAttribute, renderGraphics));
            VulkanRenderer.CreateCommandBuffers(commandBufferList);
        }

        public VkRenderPass CreateRenderPass()
        {
            VkRenderPass tempRenderPass;
            List<VkAttachmentDescription> attachmentDescriptionList = new List<VkAttachmentDescription>()
            {
                new VkAttachmentDescription
                {
                   format = VkFormat.VK_FORMAT_R8G8B8A8_UNORM,
                    samples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT,
                    loadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_CLEAR,
                    storeOp = VkAttachmentStoreOp.VK_ATTACHMENT_STORE_OP_STORE,
                    stencilLoadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_DONT_CARE,
                    stencilStoreOp = VkAttachmentStoreOp.VK_ATTACHMENT_STORE_OP_DONT_CARE,
                    initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED,
                    finalLayout = VkImageLayout.VK_IMAGE_LAYOUT_PRESENT_SRC_KHR
                }
            };

            VkImageLayout initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;
            VkImageLayout finalLayout = VkImageLayout.VK_IMAGE_LAYOUT_PRESENT_SRC_KHR;
            uint sdf = (uint)VkImageLayout.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL;
            int wer = (int)VkImageLayout.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL;


            List<VkAttachmentReference> colorRefsList = new List<VkAttachmentReference>()
            {
                new VkAttachmentReference
                {
                    attachment = 0,
                    layout = VkImageLayout.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL
                }
            };

            List<VkAttachmentReference> multiSampleReferenceList = new List<VkAttachmentReference>();
            List<VkAttachmentReference> depthReference = new List<VkAttachmentReference>();

            List<VkSubpassDescription> subpassDescriptionList = new List<VkSubpassDescription>();
            fixed (VkAttachmentReference* colorRefs = colorRefsList.ToArray())
            {
                subpassDescriptionList.Add(
                    new VkSubpassDescription
                    {
                        pipelineBindPoint = VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS,
                        colorAttachmentCount = (uint)colorRefsList.Count,
                        pColorAttachments = colorRefs,
                        pResolveAttachments = null,
                        pDepthStencilAttachment = null
                    });
            }

            List<VkSubpassDependency> subpassDependencyList = new List<VkSubpassDependency>()
            {
                new VkSubpassDependency
                {
                    srcSubpass = uint.MaxValue,
                    dstSubpass = 0,
                    srcStageMask = VkPipelineStageFlagBits.VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT,
                    dstStageMask = VkPipelineStageFlagBits.VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT,
                    srcAccessMask = 0,
                    dstAccessMask = VkAccessFlagBits.VK_ACCESS_COLOR_ATTACHMENT_WRITE_BIT,
                }
            };

            fixed (VkAttachmentDescription* attachments = attachmentDescriptionList.ToArray())
            fixed (VkSubpassDescription* description = subpassDescriptionList.ToArray())
            fixed (VkSubpassDependency* dependency = subpassDependencyList.ToArray())
            {
                var renderPassCreateInfo = new VkRenderPassCreateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_RENDER_PASS_CREATE_INFO,
                    pNext = null,
                    flags = 0,
                    attachmentCount = (uint)attachmentDescriptionList.Count(),
                    pAttachments = attachments,
                    subpassCount = (uint)subpassDescriptionList.Count(),
                    pSubpasses = description,
                    dependencyCount = (uint)subpassDependencyList.Count(),
                    pDependencies = dependency
                };

                VkFunc.vkCreateRenderPass(VulkanRenderer.device, &renderPassCreateInfo, null, &tempRenderPass);
                renderPass = tempRenderPass;
            }

            return renderPass;
        }

        private void CreateHardcodedFramebuffers()
        {
            for (int i = 0; i < VulkanRenderer.SwapChain.ImageCount; i++) // 3 images
            {
                nint[] attachments = new nint[]
                {
                      VulkanRenderer.SwapChain.imageViews[i]
                };

                GCHandle attachmentsHandle = GCHandle.Alloc(attachments, GCHandleType.Pinned);
                VkFramebufferCreateInfo fbInfo = new VkFramebufferCreateInfo
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO,
                    renderPass = renderPass,
                    attachmentCount = 1,
                    pAttachments = (nint*)attachmentsHandle.AddrOfPinnedObject(),
                    width = (uint)RenderPassResolution.x,
                    height = (uint)RenderPassResolution.y,
                    layers = 1
                };

                VkFunc.vkCreateFramebuffer(VulkanRenderer.device, &fbInfo, null, out VkFramebuffer fb);
                frameBufferList.Add(fb);
                attachmentsHandle.Free();
            }
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

            VkViewport viewport = new VkViewport
            {
                x = 0.0f,
                y = 0.0f,
                width = VulkanRenderer.SwapChain.SwapChainResolution.width,
                height = VulkanRenderer.SwapChain.SwapChainResolution.height,
                minDepth = 0.0f,
                maxDepth = 1.0f
            };

            VkRect2D scissor = new VkRect2D
            {
                offset = new VkOffset2D { x = 0, y = 0 },
                extent = VulkanRenderer.SwapChain.SwapChainResolution
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
            VkFunc.vkCmdSetViewport(commandBuffer, 0, 1, &viewport);
            VkFunc.vkCmdSetScissor(commandBuffer, 0, 1, &scissor);
            VkFunc.vkCmdBindPipeline(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, jsonPipelines.First().pipeline);
            VkFunc.vkCmdBindDescriptorSets(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, jsonPipelines.First().pipelineLayout, 0, jsonPipelines[0].descriptorSetList.UCount, jsonPipelines[0].descriptorSetList.Ptr, 0, null);
            VkFunc.vkCmdDraw(commandBuffer, 6, 1, 0, 0);
            VkFunc.vkCmdEndRenderPass(commandBuffer);
            VkFunc.vkEndCommandBuffer(commandBuffer);

            return commandBuffer;

        }
    }
}