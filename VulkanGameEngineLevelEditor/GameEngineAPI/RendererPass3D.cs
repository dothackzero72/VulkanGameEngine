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
using VulkanGameEngineGameObjectScripts.Vulkan;
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
        JsonPipeline<Vertex3D> jsonPipeline { get; set; }
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
                //RenderedTextureInfoModelList = new List<RenderedTextureInfoModel>
                //{
                //    new RenderedTextureInfoModel
                //    {
                //        ImageCreateInfo = new VkImageCreateInfoModel()
                //        {
                //            imageType = VkImageType.VK_IMAGE_TYPE_2D,
                //            format = VkFormat.VK_FORMAT_R8G8B8A8_SNORM,
                //            mipLevels = 1,
                //            arrayLayers = 1,
                //            samples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT,
                //            tiling = 0,
                //            usage = VkImageUsageFlagBits.VK_IMAGE_USAGE_TRANSFER_SRC_BIT |
                //            VkImageUsageFlagBits.VK_IMAGE_USAGE_SAMPLED_BIT |
                //            VkImageUsageFlagBits.VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT |
                //            VkImageUsageFlagBits.VK_IMAGE_USAGE_TRANSFER_DST_BIT,
                //            sharingMode = 0,
                //            initialLayout =  0,
                //            _name = "bnvnb"
                //        },
                //        SamplerCreateInfo = new VkSamplerCreateInfoModel()
                //        {
                //            magFilter = 0,
                //            minFilter = 0,
                //            mipmapMode = (VkSamplerMipmapMode)1,
                //            addressModeU = 0,
                //            addressModeV = 0,
                //            addressModeW = 0,
                //            mipLodBias = 0.0f,
                //            anisotropyEnable = true,
                //            maxAnisotropy = 16.0f,
                //            compareEnable = false,
                //            compareOp = (VkCompareOp)7,
                //            minLod = 0.0f,
                //            maxLod = 1.0f,
                //            borderColor = (VkBorderColor)3,
                //            unnormalizedCoordinates = false,
                //            _name = "tyuiy"
                //        },
                //        AttachmentDescription = new VkAttachmentDescriptionModel()
                //        {
                //            format = VkFormat.VK_FORMAT_R8G8B8A8_SNORM,
                //            samples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT,
                //            loadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_CLEAR,
                //            storeOp = VkAttachmentStoreOp.VK_ATTACHMENT_STORE_OP_STORE,
                //            stencilLoadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_DONT_CARE,
                //            stencilStoreOp = VkAttachmentStoreOp.VK_ATTACHMENT_STORE_OP_DONT_CARE,
                //            initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL,
                //            finalLayout = VkImageLayout.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL,
                //        },
                //        IsRenderedToSwapchain = true,
                //        TextureType = RenderedTextureType.ColorRenderedTexture,
                //        RenderedTextureInfoName = "texture",
                //        _name = "ColorAttachment"
                //    },
                //      new RenderedTextureInfoModel
                //    {
                //        ImageCreateInfo = new VkImageCreateInfoModel()
                //        {
                //            imageType = (VkImageType)1,
                //            format = VkFormat.VK_FORMAT_D32_SFLOAT,
                //            mipLevels = 1,
                //            arrayLayers = 1,
                //            samples = (VkSampleCountFlagBits)1,
                //            tiling = 0,
                //            usage =  VkImageUsageFlagBits.VK_IMAGE_USAGE_TRANSFER_SRC_BIT |
                //                     VkImageUsageFlagBits.VK_IMAGE_USAGE_SAMPLED_BIT |
                //                     VkImageUsageFlagBits.VK_IMAGE_USAGE_DEPTH_STENCIL_ATTACHMENT_BIT |
                //                     VkImageUsageFlagBits.VK_IMAGE_USAGE_TRANSFER_DST_BIT,
                //            sharingMode = 0,
                //            queueFamilyIndexCount = 0,
                //            pQueueFamilyIndices = null,
                //            initialLayout = 0,
                //            _name = "DefaultColorTextureCreateInfo"
                //        },
                //        SamplerCreateInfo = new VkSamplerCreateInfoModel()
                //        {
                //            magFilter = 0,
                //            minFilter = 0,
                //            mipmapMode = (VkSamplerMipmapMode)1,
                //            addressModeU = 0,
                //            addressModeV = 0,
                //            addressModeW = 0,
                //            mipLodBias = 0.0f,
                //            anisotropyEnable = true,
                //            maxAnisotropy = 16.0f,
                //            compareEnable = false,
                //            compareOp = (VkCompareOp)7,
                //            minLod = 0.0f,
                //            maxLod = 1.0f,
                //            borderColor = (VkBorderColor)2,
                //            unnormalizedCoordinates = false,
                //            _name = "DefaultColorSampleCreateInfo"
                //        },
                //        AttachmentDescription = new VkAttachmentDescriptionModel()
                //        {
                //            format = VkFormat.VK_FORMAT_D32_SFLOAT,
                //            samples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT,
                //            loadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_CLEAR,
                //            storeOp = VkAttachmentStoreOp.VK_ATTACHMENT_STORE_OP_DONT_CARE,
                //            stencilLoadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_DONT_CARE,
                //            stencilStoreOp = VkAttachmentStoreOp.VK_ATTACHMENT_STORE_OP_DONT_CARE,
                //            initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED,
                //            finalLayout = VkImageLayout.VK_IMAGE_LAYOUT_DEPTH_STENCIL_ATTACHMENT_OPTIMAL,
                //        },
                //        IsRenderedToSwapchain = false,
                //        TextureType = RenderedTextureType.DepthRenderedTexture,
                //        RenderedTextureInfoName = "texture",
                //        _name = "depthAttachment"
                //    }
                //},
                //SubpassDependencyList = new List<VkSubpassDependencyModel>()
                //{
                //     new VkSubpassDependencyModel
                //            {
                //                srcSubpass = uint.MaxValue,
                //                dstSubpass = 0,
                //                srcStageMask = VkPipelineStageFlagBits.COLOR_ATTACHMENT_OUTPUT_BIT,
                //                dstStageMask = VkPipelineStageFlagBits.COLOR_ATTACHMENT_OUTPUT_BIT,
                //                srcAccessMask = 0,
                //                dstAccessMask = VkAccessFlagBits.VK_ACCESS_COLOR_ATTACHMENT_WRITE_BIT, 
                //            }
                //}
            };

            string jsonString = JsonConvert.SerializeObject(modelInfo, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(ConstConfig.Default2DRenderPass, jsonString);

            string jsonContent2 = File.ReadAllText(ConstConfig.Default2DRenderPass);
            RenderPassBuildInfoModel model2 = JsonConvert.DeserializeObject<RenderPassBuildInfoModel>(jsonContent2);

            CreateRenderPass(model2);
            CreateFramebuffer();

            ListPtr<VkVertexInputBindingDescription> vertexBinding = NullVertex.GetBindingDescriptions();
            ListPtr<VkVertexInputAttributeDescription> vertexAttribute = NullVertex.GetAttributeDescriptions();
            jsonPipeline = new JsonPipeline<Vertex3D>(ConstConfig.Default2DPipeline, renderPass, (uint)sizeof(SceneDataBuffer), vertexBinding, vertexAttribute, new GPUImport<Vertex3D>());
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
            //foreach (RenderedTextureInfoModel renderedTextureInfoModel in model2.RenderedTextureInfoModelList)
            //{
            //    attachmentDescriptionList.Add(renderedTextureInfoModel.AttachmentDescription.Convert());
            //    switch (renderedTextureInfoModel.TextureType)
            //    {
            //        case RenderedTextureType.ColorRenderedTexture:
            //            {
            //                RenderedColorTextureList.Add(new RenderedTexture(RenderPassResolution, renderedTextureInfoModel.ImageCreateInfo.Convert(), renderedTextureInfoModel.SamplerCreateInfo.Convert()));
            //                colorAttachmentReferenceList.Add(new VkAttachmentReference
            //                {
            //                    attachment = colorAttachmentReferenceList.UCount(),
            //                    layout = VkImageLayout.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL
            //                });
            //                break;
            //            }
            //        case RenderedTextureType.DepthRenderedTexture:
            //            {
            //                depthTexture = new DepthTexture(RenderPassResolution,renderedTextureInfoModel.ImageCreateInfo.Convert(), renderedTextureInfoModel.SamplerCreateInfo.Convert());
            //                depthReferenceList.Add(new VkAttachmentReference
            //                {
            //                    attachment = (uint)(colorAttachmentReferenceList.Count + resolveAttachmentReferenceList.Count),
            //                    layout = VkImageLayout.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL
            //                });
            //                break;
            //            }
            //        case RenderedTextureType.InputAttachmentTexture:
            //            {
            //                RenderedColorTextureList.Add(new RenderedTexture(RenderPassResolution, renderedTextureInfoModel.ImageCreateInfo.Convert(), renderedTextureInfoModel.SamplerCreateInfo.Convert()));
            //                inputAttachmentReferenceList.Add(new VkAttachmentReference
            //                {
            //                    attachment = (uint)(inputAttachmentReferenceList.Count()),
            //                    layout = VkImageLayout.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL
            //                });
            //                break;
            //            }
            //        case RenderedTextureType.ResolveAttachmentTexture:
            //            {
            //                RenderedColorTextureList.Add(new RenderedTexture(RenderPassResolution, renderedTextureInfoModel.ImageCreateInfo.Convert(), renderedTextureInfoModel.SamplerCreateInfo.Convert()));
            //                resolveAttachmentReferenceList.Add(new VkAttachmentReference
            //                {
            //                    attachment = (uint)(colorAttachmentReferenceList.UCount() + 1),
            //                   layout = VkImageLayout.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL
            //                });
            //                break;
            //            }
            //        default:
            //            {
            //                MessageBox.Show("Something went wrong building render pass: Attachment Problems.");
            //                break;
            //            }
            //    }
            //}

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
            //foreach (VkSubpassDependencyModel subpass in model2.SubpassDependencyList)
            //{
            //    subPassList.Add(subpass.Convert());
            //}

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
                VkFunc.vkCreateRenderPass(VulkanRenderer.device, &renderPassCreateInfo, null, out tempRenderPass);
                renderPass = tempRenderPass;
            }
            return renderPass;
        }

        public void CreateFramebuffer()
        {
            VkFramebuffer[] frameBufferList = new VkFramebuffer[VulkanRenderer.SwapChain.ImageCount];
            for (int x = 0; x < VulkanRenderer.SwapChain.ImageCount; x++)
            {
                List<VkImageView> TextureAttachmentList = new List<VkImageView>();
                foreach (var texture in RenderedColorTextureList)
                {
                  
                        TextureAttachmentList.Add(VulkanRenderer.SwapChain.imageViews[x]);
               
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
                    FrameBufferList[x] = frameBuffer;
                }
            }
        }

        public VkCommandBuffer Draw(List<GameObject> gameObjectList, SceneDataBuffer sceneDataBuffer)
        {
            var commandIndex = VulkanRenderer.CommandIndex;
            var imageIndex = VulkanRenderer.ImageIndex;
            var commandBuffer = commandBufferList[(int)commandIndex];

            List<VkClearValue> clearValues = new List<VkClearValue>
{
                new VkClearValue
                { 
                    Color = new VkClearColorValue(1, 0, 0, 1) 
                },
                new VkClearValue
                { 
                    DepthStencil = new VkClearDepthStencilValue(0.0f, 1.0f) 
                }
            };

            fixed (VkClearValue* pClearValue = clearValues.ToArray())
            {
                VkRenderPassBeginInfo renderPassInfo = new VkRenderPassBeginInfo
                {
                    renderPass = renderPass,
                    framebuffer = FrameBufferList[(int)imageIndex],
                    clearValueCount = 2,
                    pClearValues = pClearValue,
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
                var scissor = new VkRect2D(new VkOffset2D(0, 0), VulkanRenderer.SwapChain.SwapChainResolution);

                var commandInfo = new VkCommandBufferBeginInfo { flags = 0 };
                VkFunc.vkBeginCommandBuffer(commandBuffer, &commandInfo);
                VkFunc.vkCmdBeginRenderPass(commandBuffer, &renderPassInfo, VkSubpassContents.VK_SUBPASS_CONTENTS_INLINE);
                VkFunc.vkCmdSetViewport(commandBuffer, 0, 1, &viewport);
                VkFunc.vkCmdSetScissor(commandBuffer, 0, 1, &scissor);
                VkFunc.vkCmdBindPipeline(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, jsonPipeline.pipeline);
                foreach (var obj in gameObjectList)
                {
                    obj.Draw(commandBuffer, jsonPipeline.pipeline, jsonPipeline.pipelineLayout, jsonPipeline.descriptorSetList, sceneDataBuffer);
                }
                VkFunc.vkCmdEndRenderPass(commandBuffer);
                VkFunc.vkEndCommandBuffer(commandBuffer);

                return commandBuffer;
            }
        }
    }
}