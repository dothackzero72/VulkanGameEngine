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
using System.Xml;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.RenderPassEditor;
using VulkanGameEngineLevelEditor.Vulkan;
using ImageLayout = Silk.NET.Vulkan.ImageLayout;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class RendererPass3D : SilkRenderPassBase
    {
        Vk vk = Vk.GetApi();
        public List<RenderedTexture> RenderedColorTextureList { get; private set; } = new List<RenderedTexture>();
        public DepthTexture depthTexture { get; private set; }
        //public Texture texture { get; set; }
      //  public Texture renderedTexture { get; set; }
       // JsonRenderPass jsonRenderer { get; set; }
        JsonPipeline jsonPipeline { get; set; }
        public RendererPass3D() : base()
        {
        }

        public void Create3dRenderPass()
        {
            //depthTexture = new DepthTexture(new ivec2((int)VulkanRenderer.swapChain.swapchainExtent.Width, (int)VulkanRenderer.swapChain.swapchainExtent.Height));
            //texture = new GameEngineAPI.Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\VulkanGameEngineLevelEditor\\bin\\Debug\\awesomeface.png", Format.R8G8B8A8Unorm, TextureTypeEnum.kType_DiffuseTextureMap);

            string jsonContent = File.ReadAllText(ConstConfig.Default2DPipeline);
            RenderPipelineModel model = JsonConvert.DeserializeObject<RenderPipelineModel>(jsonContent);

            //jsonRenderer = new JsonRenderPass(VulkanRenderer.device);
            //jsonRenderer.JsonCreateRenderPass(RenderPassEditorConsts.Default2DRenderPass, new ivec2((int)VulkanRenderer.swapChain.swapchainExtent.Width, (int)VulkanRenderer.swapChain.swapchainExtent.Height));

            RenderPassBuildInfoModel modelInfo = new RenderPassBuildInfoModel()
            {
                RenderedTextureInfoModelList = new List<RenderedTextureInfoModel>
                {
                    new RenderedTextureInfoModel
                    {
                        ImageCreateInfo = new VkImageCreateInfo()
                        {
                            imageType = VkImageType.VK_IMAGE_TYPE_2D,
                            format = VkFormat.VK_FORMAT_R8G8B8A8_SNORM,
                            mipLevels = 1,
                            arrayLayers = 1,
                            samples = SampleCountFlags.Count1Bit,
                            tiling = 0,
                            usage = VkImageUsageFlagBits.ImageUsageTransferSrcBit |
                                    VkImageUsageFlagBits.SampledBit |
                                    VkImageUsageFlagBits.ImageUsageColorAttachmentBit |
                                    VkImageUsageFlagBits.ImageUsageTransferDstBit,
                            sharingMode = 0,
                            initialLayout =  0,
                            _name = "bnvnb"
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
                            borderColor = (VkBorderColor)3,
                            unnormalizedCoordinates = false,
                            _name = "tyuiy"
                        },
                        AttachmentDescription = new VkAttachmentDescription()
                        {
                            format = VkFormat.R8G8B8A8Unorm,
                            samples = VkSampleCountFlags.SampleCount1Bit,
                            loadOp = VkAttachmentLoadOp.Clear,
                            storeOp = VkAttachmentStoreOp.Store,
                            stencilLoadOp = VkAttachmentLoadOp.DontCare,
                            stencilStoreOp = VkAttachmentStoreOp.DontCare,
                            initialLayout = VkImageLayout.ColorAttachmentOptimal,
                            finalLayout = VkImageLayout.ColorAttachmentOptimal,
                        },
                        IsRenderedToSwapchain = true,
                        TextureType = RenderedTextureType.ColorRenderedTexture,
                        RenderedTextureInfoName = "texture",
                        _name = "ColorAttachment"
                    },
                      new RenderedTextureInfoModel
                    {
                        ImageCreateInfo = new VkImageCreateInfo()
                        {
                            imageType = (VkImageType)1,
                            format = VkFormat.D32Sfloat,
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
                            _name = "DefaultColorTextureCreateInfo"
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
                            _name = "DefaultColorSampleCreateInfo"
                        },
                        AttachmentDescription = new VkAttachmentDescription()
                        {
                            format = VkVkFormat.D32Sfloat,
                            samples = VkSampleCountFlags.SampleCount1Bit,
                            loadOp = VkVkAttachmentLoadOp.Clear,
                            storeOp = VkVkAttachmentStoreOp.DontCare,
                            stencilLoadOp = VkVkAttachmentLoadOp.DontCare,
                            stencilStoreOp = VkVkAttachmentStoreOp.DontCare,
                            initialLayout = VkVkImageLayout.Undefined,
                            finalLayout = VkVkImageLayout.DepthStencilAttachmentOptimal,
                        },
                        IsRenderedToSwapchain = false,
                        TextureType = VkRenderedTextureType.DepthRenderedTexture,
                        RenderedTextureInfoName = "texture",
                        _name = "depthAttachment"
                    }
                },
                SubpassDependencyList = new List<VkSubpassDependency>()
                {
                     new VkSubpassDependency
                            {
                                srcSubpass = uint.MaxValue,
                                dstSubpass = 0,
                                srcStageMask = VkPipelineStageFlags.ColorAttachmentOutputBit,
                                dstStageMask = VkPipelineStageFlags.ColorAttachmentOutputBit, // Changed to Early Fragment Tests
                                srcAccessMask = 0,
                                dstAccessMask = (VkAccessFlags)AccessFlags.ColorAttachmentWriteBit, // Ensure this access mask is relevant to the chosen stage mask
                            }
                }
            };

            string jsonString = JsonConvert.SerializeObject(modelInfo, VkFormatting.Indented);
            File.WriteAllText(ConstConfig.Default2DRenderPass, jsonString);

            string jsonContent2 = File.ReadAllText(ConstConfig.Default2DRenderPass);
            RenderPassBuildInfoModel model2 = JsonConvert.DeserializeObject<RenderPassBuildInfoModel>(jsonContent2);

            CreateRenderPass(model2);
            CreateFramebuffer();
            jsonPipeline = new JsonPipeline(ConstConfig.Default2DPipeline, renderPass, (uint)sizeof(SceneDataBuffer));
            //LoadDescriptorSets(model);
            //CreateGraphicsPipeline();

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
                            depthTexture = new DepthTexture(RenderPassResolution,renderedTextureInfoModel.ImageCreateInfo, renderedTextureInfoModel.SamplerCreateInfo);
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
            VkFramebuffer[] frameBufferList = new VkFramebuffer[VulkanRenderer.swapChain.ImageCount];
            for (int x = 0; x < VulkanRenderer.swapChain.ImageCount; x++)
            {
                List<VkImageView> TextureAttachmentList = new List<VkImageView>();
                foreach (var texture in RenderedColorTextureList)
                {
                  
                        TextureAttachmentList.Add(VulkanRenderer.swapChain.imageViews[x]);
               
                }
                if (depthTexture != null)
                {
                    TextureAttachmentList.Add(depthTexture.View);
                }

                fixed (VkImageView* imageViewPtr = TextureAttachmentList.ToArray())
                {
                    VkFramebufferCreateInfo framebufferInfo = new VkFramebufferCreateInfo()
                    {
                        sType = VkStructureType.VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO,
                        renderPass = renderPass,
                        attachmentCount = TextureAttachmentList.UCount(),
                        pAttachments = imageViewPtr,
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
            }

            FrameBufferList = frameBufferList;
        }

        public CommandBuffer Draw(List<GameObject> gameObjectList, SceneDataBuffer sceneDataBuffer)
        {
            var commandIndex = VulkanRenderer.CommandIndex;
            var imageIndex = VulkanRenderer.ImageIndex;
            var commandBuffer = commandBufferList[commandIndex];
            ClearValue* clearValues = stackalloc[]
{
                new ClearValue(new ClearColorValue(1, 0, 0, 1)),
                new ClearValue(null, new ClearDepthStencilValue(1.0f))
            };

            VkRenderPassBeginInfo renderPassInfo = new VkRenderPassBeginInfo
            {
                renderPass = renderPass,
                framebuffer = FrameBufferList[imageIndex],
                clearValueCount = 2,
                pClearValues = clearValues,
                renderArea = new(new VkOffset2D(0, 0), VulkanRenderer.swapChain.SwapChainResolution)
            };

            var viewport = new VkViewport(0.0f, 0.0f, VulkanRenderer.swapChain.SwapChainResolution.width, VulkanRenderer.swapChain.SwapChainResolution.height, 0.0f, 1.0f);
            var scissor = new VkRect2D(new VkOffset2D(0, 0), VulkanRenderer.swapChain.SwapChainResolution);

            var descSet = jsonPipeline.descriptorSet;
            var commandInfo = new VkCommandBufferBeginInfo(flags: 0);

            VkFunc.vkBeginCommandBuffer(commandBuffer, &commandInfo);
            VkFunc.vkCmdBeginRenderPass(commandBuffer, &renderPassInfo, VkSubpassContents.Inline);
            VkFunc.vkCmdSetViewport(commandBuffer, 0, 1, &viewport);
            VkFunc.vkCmdSetScissor(commandBuffer, 0, 1, &scissor);
            VkFunc.vkCmdBindPipeline(commandBuffer, VkPipelineBindPoint.Graphics, jsonPipeline.pipeline);
            foreach (var obj in gameObjectList)
            {
                obj.Draw(commandBuffer, jsonPipeline.pipeline, jsonPipeline.pipelineLayout, descSet, sceneDataBuffer);
            }
            VkFunc.vkCmdEndRenderPass(commandBuffer);
            VkFunc.vkEndCommandBuffer(commandBuffer);

            return commandBuffer;
        }
    }
}