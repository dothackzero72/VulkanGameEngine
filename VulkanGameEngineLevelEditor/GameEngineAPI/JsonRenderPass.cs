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
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.RenderPassEditor;
using VulkanGameEngineLevelEditor.Vulkan;
using ImageLayout = Silk.NET.Vulkan.ImageLayout;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class JsonRenderPass
    {
        Vk vk = Vk.GetApi();
        public List<RenderedTexture> RenderedColorTextureList { get; private set; } = new List<RenderedTexture>();
        public DepthTexture depthTexture { get; private set; }
        public ivec2 RenderPassResolution { get; set; }
        public VkRenderPass renderPass { get; protected set; }
        public VkCommandBuffer[] commandBufferList { get; protected set; }
        public VkFramebuffer[] FrameBufferList { get; protected set; }
        JsonPipeline jsonPipeline { get; set; }
        public JsonRenderPass() : base()
        {
        }

        public void CreateJsonRenderPass(string jsonPath, ivec2 renderPassResolution, SampleCountFlags sampleCount = SampleCountFlags.Count1Bit)
        {
            RenderPassResolution = renderPassResolution;
            SaveRenderPass();
            string jsonContent2 = File.ReadAllText(ConstConfig.Default2DRenderPass);
            RenderPassBuildInfoModel model2 = JsonConvert.DeserializeObject<RenderPassBuildInfoModel>(jsonContent2);

            FrameBufferList = new VkFramebuffer[VulkanRenderer.MAX_FRAMES_IN_FLIGHT];
            commandBufferList = new VkCommandBuffer[VulkanRenderer.MAX_FRAMES_IN_FLIGHT];
            VulkanRenderer.CreateCommandBuffers(commandBufferList);

            CreateRenderPass(model2);
            CreateFramebuffer();
            jsonPipeline = new JsonPipeline(ConstConfig.Default2DPipeline, renderPass, (uint)sizeof(SceneDataBuffer));
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
                attachmentDescriptionList.Add(renderedTextureInfoModel.AttachmentDescription);
                switch (renderedTextureInfoModel.TextureType)
                {
                    case RenderedTextureType.ColorRenderedTexture:
                        {
                            RenderedColorTextureList.Add(new RenderedTexture(RenderPassResolution, renderedTextureInfoModel.ImageCreateInfo, renderedTextureInfoModel.SamplerCreateInfo));
                            colorAttachmentReferenceList.Add(new VkAttachmentReference
                            {
                                attachment = colorAttachmentReferenceList.UCount(),
                                layout = VkImageLayout.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL
                            });
                            break;
                        }
                    case RenderedTextureType.DepthRenderedTexture:
                        {
                            depthTexture = new DepthTexture(RenderPassResolution, renderedTextureInfoModel.ImageCreateInfo, renderedTextureInfoModel.SamplerCreateInfo);
                            depthReferenceList.Add(new VkAttachmentReference
                            {
                                attachment = (uint)(colorAttachmentReferenceList.Count + resolveAttachmentReferenceList.Count),
                                layout = VkImageLayout.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL
                            });
                            break;
                        }
                    case RenderedTextureType.InputAttachmentTexture:
                        {
                            RenderedColorTextureList.Add(new RenderedTexture(RenderPassResolution, renderedTextureInfoModel.ImageCreateInfo, renderedTextureInfoModel.SamplerCreateInfo));
                            inputAttachmentReferenceList.Add(new VkAttachmentReference
                            {
                                attachment = (uint)(inputAttachmentReferenceList.Count()),
                                layout = VkImageLayout.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL
                            });
                            break;
                        }
                    case RenderedTextureType.ResolveAttachmentTexture:
                        {
                            RenderedColorTextureList.Add(new RenderedTexture(RenderPassResolution, renderedTextureInfoModel.ImageCreateInfo, renderedTextureInfoModel.SamplerCreateInfo));
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
            foreach (VkSubpassDependency subpass in model2.SubpassDependencyList)
            {
                subPassList.Add(subpass);
            }

            fixed (VkAttachmentDescription* attachments = attachmentDescriptionList.ToArray())
            fixed (VkSubpassDescription* description = subpassDescriptionList.ToArray())
            fixed (VkSubpassDependency* dependency = subPassList.ToArray())
            {
                var renderPassCreateInfo = new VkRenderPassCreateInfo()
                {
                    sType = StructureType.RenderPassCreateInfo,
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
            VkFramebuffer[] frameBufferList = new VkFramebuffer[VulkanRenderer.swapChain.ImageCount];
            for (int x = 0; x < VulkanRenderer.swapChain.ImageCount; x++)
            {
                List<ImageView> TextureAttachmentList = new List<ImageView>();
                foreach (var texture in RenderedColorTextureList)
                {
                    TextureAttachmentList.Add(RenderedColorTextureList.First().View);
                }
                if (depthTexture != null)
                {
                    TextureAttachmentList.Add(depthTexture.View);
                }

                fixed (VkImageView* imageViewPtr = TextureAttachmentList.ToArray())
                {
                    VkFramebufferCreateInfo framebufferInfo = new VkFramebufferCreateInfo()
                    {
                        SType = StructureType.FramebufferCreateInfo,
                        RenderPass = renderPass,
                        AttachmentCount = TextureAttachmentList.UCount(),
                        PAttachments = imageViewPtr,
                        Width = (uint)RenderPassResolution.x,
                        Height = (uint)RenderPassResolution.y,
                        Layers = 1,
                        Flags = 0,
                        PNext = null
                    };

                    VkFramebuffer frameBuffer = FrameBufferList[x];
                    VkFunc.vkCreateFramebuffer(VulkanRenderer.device, &framebufferInfo, null, &frameBuffer);
                    frameBufferList[x] = frameBuffer;
                }
            }

            FrameBufferList = frameBufferList;
        }

        public VkCommandBuffer Draw(List<GameObject> gameObjectList, SceneDataBuffer sceneDataBuffer)
        {
            var commandIndex = VulkanRenderer.CommandIndex;
            var imageIndex = VulkanRenderer.ImageIndex;
            var commandBuffer = commandBufferList[commandIndex];
            VkClearValue* clearValues = stackalloc[]
{
                new VkClearValue(new VkClearColorValue(1, 0, 0, 1)),
                new VkClearValue(null, new VkClearDepthStencilValue(1.0f))
            };

            VkRenderPassBeginInfo renderPassInfo = new
            (
                renderPass: renderPass,
                framebuffer: FrameBufferList[imageIndex],
                clearValueCount: 2,
                pClearValues: clearValues,
                renderArea: new(new Offset2D(0, 0), VulkanRenderer.swapChain.swapchainExtent)
            );

            var viewport = new Viewport(0.0f, 0.0f, VulkanRenderer.swapChain.swapchainExtent.Width, VulkanRenderer.swapChain.swapchainExtent.Height, 0.0f, 1.0f);
            var scissor = new Rect2D(new Offset2D(0, 0), VulkanRenderer.swapChain.swapchainExtent);

            var descSet = jsonPipeline.descriptorSet;
            var commandInfo = new CommandBufferBeginInfo(flags: 0);

            VkFunc.vkBeginCommandBuffer(commandBuffer, &commandInfo);
            VkFunc.vkCmdBeginRenderPass(commandBuffer, &renderPassInfo, VkSubpassContents.Inline);
            VkFunc.vkCmdSetViewport(commandBuffer, 0, 1, &viewport);
            VkFunc.vkCmdSetScissor(commandBuffer, 0, 1, &scissor);
            VkFunc.vkCmdBindPipeline(commandBuffer, VkPipelineBindPoint.Graphics, jsonPipeline.pipeline);
            foreach (var obj in gameObjectList)
            {
                obj.Draw(commandBuffer.Handle, jsonPipeline.pipeline.Handle, jsonPipeline.pipelineLayout.Handle, descSet.Handle, sceneDataBuffer);
            }
            VkFunc.vkCmdEndRenderPass(commandBuffer);
            VkFunc.vkEndCommandBuffer(commandBuffer);

            return commandBuffer;
        }

        private void SaveRenderPass()
        {
            RenderPassBuildInfoModel modelInfo = new RenderPassBuildInfoModel()
            {
                RenderedTextureInfoModelList = new List<RenderedTextureInfoModel>
                {
                    new RenderedTextureInfoModel
                    {
                        ImageCreateInfo = new VkImageCreateInfo()
                        {
                            imageType = VkImageType.ImageType2D,
                            format= VkFormat.R8G8B8A8Unorm,
                            mipLevels = 1,
                            arrayLayers = 1,
                            samples = VkSampleCountFlags.Count1Bit,
                            tiling = 0,
                            usage = VkImageUsageFlagBits.VK_IMAGE_USAGE_TRANSFER_SRC_BIT |
                            VkImageUsageFlagBits.VK_IMAGE_USAGE_SAMPLED_BIT |
                            VkImageUsageFlagBits.VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT |
                            VkImageUsageFlagBits.VK_IMAGE_USAGE_TRANSFER_DST_BIT,
                            sharingMode = 0,
                            initialLayout =  0,
                            _name = string.Empty
                        },
                        SamplerCreateInfo = new VkSamplerCreateInfo()
                        {
                            magFilter = 0,
                            minFilter = 0,
                            mipmapMode = (SamplerMipmapMode)1,
                            addressModeU = 0,
                            addressModeV = 0,
                            addressModeW = 0,
                            mipLodBias = 0.0f,
                            anisotropyEnable = true,
                            maxAnisotropy = 16.0f,
                            compareEnable = false,
                            compareOp = (CompareOp)7,
                            minLod = 0.0f,
                            maxLod = 1.0f,
                            borderColor = (BorderColor)3,
                            unnormalizedCoordinates = false,
                            _name = string.Empty
                        },
                        AttachmentDescription = new VkAttachmentDescription()
                        {
                            format = VkFormat.VK_FORMAT_R8G8B8A8_UNORM,
                            samples = VkSampleCountFlags.SampleCount1Bit,
                            loadOp = VkAttachmentLoadOp.Clear,
                            storeOp = VkAttachmentStoreOp.Store,
                            stencilLoadOp = VkAttachmentLoadOp.DontCare,
                            stencilStoreOp = VkAttachmentStoreOp.DontCare,
                            initialLayout = VkImageLayout.Undefined,
                            finalLayout = VkImageLayout.ShaderReadOnlyOptimal,
                            _name = string.Empty
                        },
                        IsRenderedToSwapchain = true,
                        TextureType = RenderedTextureType.ColorRenderedTexture,
                        RenderedTextureInfoName = "texture",
                        _name = string.Empty
                    },
                      new RenderedTextureInfoModel
                    {
                        ImageCreateInfo = new VkImageCreateInfo()
                        {
                            imageType = (ImageType)1,
                            format = Format.D32Sfloat,
                            mipLevels = 1,
                            arrayLayers = 1,
                            samples = (SampleCountFlags)1,
                            tiling = 0,
                            usage =  VkImageUsageFlags.ImageUsageTransferSrcBit |
                                    VkImageUsageFlags.SampledBit |
                                    VkImageUsageFlags.DepthStencilAttachmentBit |
                                    VkImageUsageFlags.ImageUsageTransferDstBit,
                            sharingMode = 0,
                            queueFamilyIndexCount = 0,
                            pQueueFamilyIndices = null,
                            initialLayout = 0,
                            _name = string.Empty
                        },
                        SamplerCreateInfo = new VkSamplerCreateInfo()
                        {
                            magFilter = 0,
                            minFilter = 0,
                            mipmapMode = (VkSamplerMipmapMode)1,
                            addressModeU = 0,
                            addressModeV = 0,
                            addressModeW = 0,
                            mipLodBias = 0.0f,
                            anisotropyEnable = true,
                            maxAnisotropy = 16.0f,
                            compareEnable = false,
                            compareOp = (VkCompareOp)7,
                            minLod = 0.0f,
                            maxLod = 1.0f,
                            borderColor = (VkBorderColor)2,
                            unnormalizedCoordinates = false,
                            _name = string.Empty
                        },
                        AttachmentDescription = new VkAttachmentDescription()
                        {
                            format = VkFormat.D32Sfloat,
                            samples = VkSampleCountFlagBits.SampleCount1Bit,
                            loadOp = VkAttachmentLoadOp.Clear,
                            storeOp = VkAttachmentStoreOp.DontCare,
                            stencilLoadOp = VkAttachmentLoadOp.DontCare,
                            stencilStoreOp = VkAttachmentStoreOp.DontCare,
                            initialLayout = VkImageLayout.Undefined,
                            finalLayout = VkImageLayout.DepthStencilAttachmentOptimal,
                            _name = string.Empty
                        },
                        IsRenderedToSwapchain = false,
                        TextureType = RenderedTextureType.DepthRenderedTexture,
                        RenderedTextureInfoName = "texture",
                        _name = string.Empty
                    }
                },
                SubpassDependencyList = new List<VkSubpassDependency>()
                {
                     new VkSubpassDependency
                            {
                                srcSubpass = uint.MaxValue,
                                dstSubpass = 0,
                                srcStageMask = VkPipelineStageFlagBits.ColorAttachmentOutputBit,
                                dstStageMask = VkPipelineStageFlagBits.ColorAttachmentOutputBit, // Changed to Early Fragment Tests
                                srcAccessMask = 0,
                                dstAccessMask = VkAccessFlagBits.ColorAttachmentWriteBit, // Ensure this access mask is relevant to the chosen stage mask,
                                _name = string.Empty
                            },
                },
                RenderPipelineList = new List<RenderPipelineModel>(),
                _name = "Default2DRenderPass"
            };

            string jsonString = JsonConvert.SerializeObject(modelInfo, Formatting.Indented);
            File.WriteAllText(ConstConfig.Default2DRenderPass, jsonString);
        }
    }
}