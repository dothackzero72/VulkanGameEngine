using AutoMapper;
using GlmSharp;
using Newtonsoft.Json;
using Silk.NET.Core.Native;
using Silk.NET.Maths;
using Silk.NET.SDL;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineGameObjectScripts.Vulkan;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.RenderPassEditor;
using VulkanGameEngineLevelEditor.Vulkan;
using ImageLayout = Silk.NET.Vulkan.ImageLayout;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class JsonRenderPass<T>
    {
        Vk vk = Vk.GetApi();
        public List<RenderedTexture> RenderedColorTextureList { get; private set; } = new List<RenderedTexture>();
        public DepthTexture depthTexture { get; private set; } = new DepthTexture();
        public ivec2 RenderPassResolution { get; set; }
        public VkRenderPass renderPass { get; protected set; }
        public ListPtr<VkCommandBuffer> commandBufferList { get; protected set; } = new ListPtr<VkCommandBuffer>();
        public ListPtr<VkFramebuffer> FrameBufferList { get; protected set; } = new ListPtr<VkFramebuffer>();
        protected List<JsonPipeline<T>> jsonPipelineList { get; set; } = new List<JsonPipeline<T>>();
        public VkSampleCountFlagBits SampleCountFlags { get; protected set; }
        public JsonRenderPass() : base()
        {
        }

        public void CreateJsonRenderPass(string jsonPath, GPUImport<T> renderGraphics, ivec2 renderPassResolution, VkSampleCountFlagBits sampleCount = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT)
        {
            RenderPassResolution = renderPassResolution;
            SampleCountFlags = sampleCount;

            string jsonContent = File.ReadAllText(jsonPath);
            RenderPassBuildInfoModel model = JsonConvert.DeserializeObject<RenderPassBuildInfoModel>(jsonContent);
            foreach (var item in model.RenderedTextureInfoModelList)
            {
                item.ImageCreateInfo.extent = new VkExtent3DModel
                {
                    width = (uint)renderPassResolution.x,
                    height = (uint)renderPassResolution.y,
                    depth = 1
                };
            }

            FrameBufferList = new ListPtr<VkFramebuffer>(VulkanRenderer.SwapChain.ImageCount);
            commandBufferList = new ListPtr<VkCommandBuffer>(VulkanRenderer.SwapChain.ImageCount);

            ListPtr<VkVertexInputBindingDescription> vertexBinding = NullVertex.GetBindingDescriptions();
            ListPtr<VkVertexInputAttributeDescription> vertexAttribute = NullVertex.GetAttributeDescriptions();

            CreateRenderPass(model);
            CreateFramebuffer();
            jsonPipelineList.Add(new JsonPipeline<T>(ConstConfig.Default2DPipeline, renderPass, (uint)sizeof(SceneDataBuffer), vertexBinding, vertexAttribute, renderGraphics));
            VulkanRenderer.CreateCommandBuffers(commandBufferList);
        }

        public VkRenderPass CreateRenderPass(RenderPassBuildInfoModel model2)
        {
            List<VkAttachmentDescription> attachmentDescriptionList = new List<VkAttachmentDescription>();
            List<VkAttachmentReference> inputAttachmentReferenceList = new List<VkAttachmentReference>();
            List<VkAttachmentReference> colorAttachmentReferenceList = new List<VkAttachmentReference>();
            List<VkAttachmentReference> resolveAttachmentReferenceList = new List<VkAttachmentReference>();
            List<VkSubpassDescription> preserveAttachmentReferenceList = new List<VkSubpassDescription>();
            List<VkAttachmentReference> depthReferenceList = new List<VkAttachmentReference>();
            foreach (RenderedTextureInfoModel renderedTextureInfoModel in model2.RenderedTextureInfoModelList)
            {
                attachmentDescriptionList.Add(renderedTextureInfoModel.AttachmentDescription.Convert());
                switch (renderedTextureInfoModel.TextureType)
                {
                    case RenderedTextureType.ColorRenderedTexture:
                        {
                            RenderedColorTextureList.Add(new RenderedTexture(VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, renderedTextureInfoModel.ImageCreateInfo.Convert(), renderedTextureInfoModel.SamplerCreateInfo.Convert()));
                            colorAttachmentReferenceList.Add(new VkAttachmentReference
                            {
                                attachment = colorAttachmentReferenceList.UCount(),
                                layout = VkImageLayout.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL
                            });
                            break;
                        }
                    case RenderedTextureType.DepthRenderedTexture:
                        {
                            depthTexture = new DepthTexture(VkImageAspectFlagBits.VK_IMAGE_ASPECT_DEPTH_BIT, renderedTextureInfoModel.ImageCreateInfo.Convert(), renderedTextureInfoModel.SamplerCreateInfo.Convert());
                            depthReferenceList.Add(new VkAttachmentReference
                            {
                                attachment = (uint)(colorAttachmentReferenceList.Count + resolveAttachmentReferenceList.Count),
                                layout = VkImageLayout.VK_IMAGE_LAYOUT_DEPTH_ATTACHMENT_OPTIMAL
                            });
                            break;
                        }
                    case RenderedTextureType.InputAttachmentTexture:
                        {
                            RenderedColorTextureList.Add(new RenderedTexture(VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, renderedTextureInfoModel.ImageCreateInfo.Convert(), renderedTextureInfoModel.SamplerCreateInfo.Convert()));
                            inputAttachmentReferenceList.Add(new VkAttachmentReference
                            {
                                attachment = (uint)(inputAttachmentReferenceList.Count()),
                                layout = VkImageLayout.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL
                            });
                            break;
                        }
                    case RenderedTextureType.ResolveAttachmentTexture:
                        {
                            RenderedColorTextureList.Add(new RenderedTexture(VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, renderedTextureInfoModel.ImageCreateInfo.Convert(), renderedTextureInfoModel.SamplerCreateInfo.Convert()));
                            resolveAttachmentReferenceList.Add(new VkAttachmentReference
                            {
                                attachment = (uint)(colorAttachmentReferenceList.UCount() + 1),
                                layout = VkImageLayout.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL
                            });
                            break;
                        }
                    default:
                        {
                            MessageBox.Show("Something went wrong building render pass: Attachment Problems.");
                            break;
                        }
                }
            }

            List<VkSubpassDescription> subpassDescriptionList = new List<VkSubpassDescription>();
            fixed (VkAttachmentReference* depthAttachment = depthReferenceList.ToArray())
            fixed (VkAttachmentReference* colorAttachments = colorAttachmentReferenceList.ToArray())
            fixed (VkAttachmentReference* inputAttachments = inputAttachmentReferenceList.ToArray())
            fixed (VkAttachmentReference* resolveAttachments = resolveAttachmentReferenceList.ToArray())
            fixed (VkSubpassDescription* preserveAttachments = preserveAttachmentReferenceList.ToArray())
            {
                subpassDescriptionList.Add(new VkSubpassDescription
                {
                    pipelineBindPoint = VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS,
                    colorAttachmentCount = colorAttachmentReferenceList.UCount(),
                    pColorAttachments = colorAttachments,
                    pDepthStencilAttachment = depthAttachment,
                    pResolveAttachments = resolveAttachments,
                    inputAttachmentCount = inputAttachmentReferenceList.UCount(),
                    pInputAttachments = inputAttachments,
                    preserveAttachmentCount = preserveAttachmentReferenceList.UCount(),
                    flags = 0,
                    pPreserveAttachments = null
                });
            }

            List<VkSubpassDependency> subPassList = new List<VkSubpassDependency>();
            foreach (VkSubpassDependencyModel subpass in model2.SubpassDependencyList)
            {
                subPassList.Add(subpass.Convert());
            }

            fixed (VkAttachmentDescription* attachments = attachmentDescriptionList.ToArray())
            fixed (VkSubpassDescription* description = subpassDescriptionList.ToArray())
            fixed (VkSubpassDependency* dependency = subPassList.ToArray())
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
                    dependencyCount = (uint)subPassList.Count(),
                    pDependencies = dependency,

                };

                var tempRenderPass = new VkRenderPass();
                VkFunc.vkCreateRenderPass(VulkanRenderer.device, &renderPassCreateInfo, null, &tempRenderPass);
                renderPass = tempRenderPass;
            }
            return renderPass;
        }

        public void CreateFramebuffer()
        {
           
            ListPtr<VkFramebuffer> frameBufferList = new ListPtr<VkFramebuffer>(VulkanRenderer.SwapChain.ImageCount);
            for (int x = 0; x < VulkanRenderer.SwapChain.ImageCount; x++)
            {
                ListPtr<VkImageView> textureAttachmentList = new ListPtr<VkImageView>();
                foreach (var texture in RenderedColorTextureList)
                {
                    textureAttachmentList.Add(RenderedColorTextureList.First().View);
                }
                if (depthTexture != null)
                {
                    textureAttachmentList.Add(depthTexture.View);
                }


                VkFramebufferCreateInfo framebufferInfo = new VkFramebufferCreateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO,
                    renderPass = renderPass,
                    attachmentCount = textureAttachmentList.UCount,
                    pAttachments = textureAttachmentList.Ptr,
                    width = (uint)RenderPassResolution.x,
                    height = (uint)RenderPassResolution.y,
                    layers = 1,
                    flags = 0,
                    pNext = null
                };

                VkFramebuffer frameBuffer = FrameBufferList[x];
                VkFunc.vkCreateFramebuffer(VulkanRenderer.device, &framebufferInfo, null, &frameBuffer);
                frameBufferList[x] = frameBuffer;

            }

            FrameBufferList = frameBufferList;
        }

        public virtual void Update(float deltaTime)
        {

        }

        public virtual VkCommandBuffer Draw(List<GameObject> gameObjectList, SceneDataBuffer sceneDataBuffer)
        {
            var commandIndex = VulkanRenderer.CommandIndex;
            var imageIndex = VulkanRenderer.ImageIndex;
            var commandBuffer = commandBufferList[(int)commandIndex];

            using ListPtr<VkClearValue> clearValues = new ListPtr<VkClearValue>();
            clearValues.Add(new VkClearValue { Color = new VkClearColorValue(1, 0, 0, 1) });
            clearValues.Add(new VkClearValue { DepthStencil = new VkClearDepthStencilValue(0.0f, 1.0f) });
            

            VkRenderPassBeginInfo renderPassInfo = new VkRenderPassBeginInfo
            {
                renderPass = renderPass,
                framebuffer = FrameBufferList[(int)imageIndex],
                clearValueCount = clearValues.UCount,
                pClearValues = clearValues.Ptr,
                renderArea = new(new VkOffset2D(0, 0), VulkanRenderer.SwapChain.SwapChainResolution)
            };


            var viewport = new VkViewport
            {
                x = 0.0f,
                y = 0.0f,
                width = VulkanRenderer.SwapChain.SwapChainResolution.width,
                height = VulkanRenderer.SwapChain.SwapChainResolution.height,
                minDepth = 0.0f,
                maxDepth = 1.0f
            };
            var scissor = new VkRect2D
            {
                offset = new VkOffset2D(0, 0),
                extent = VulkanRenderer.SwapChain.SwapChainResolution,
            };

            var commandInfo = new VkCommandBufferBeginInfo { flags = 0 };
            VkFunc.vkBeginCommandBuffer(commandBuffer, &commandInfo);
            VkFunc.vkCmdBeginRenderPass(commandBuffer, &renderPassInfo, VkSubpassContents.VK_SUBPASS_CONTENTS_INLINE);
            VkFunc.vkCmdSetViewport(commandBuffer, 0, 1, &viewport);
            VkFunc.vkCmdSetScissor(commandBuffer, 0, 1, &scissor);
            VkFunc.vkCmdBindPipeline(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, jsonPipelineList[0].pipeline);
            foreach (var obj in gameObjectList)
            {
                obj.Draw(commandBuffer, jsonPipelineList[0].pipeline, jsonPipelineList[0].pipelineLayout, jsonPipelineList[0].descriptorSetList, sceneDataBuffer);
            }
            VkFunc.vkCmdEndRenderPass(commandBuffer);
            VkFunc.vkEndCommandBuffer(commandBuffer);

            return commandBuffer;
        }

        public virtual void Destroy()
        {
        }
    }
}