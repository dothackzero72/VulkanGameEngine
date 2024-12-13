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
        public RenderPass renderPass { get; protected set; }
        public CommandBuffer[] commandBufferList { get; protected set; }
        public Framebuffer[] FrameBufferList { get; protected set; }
        JsonPipeline jsonPipeline { get; set; }
        public JsonRenderPass() : base()
        {
        }

        public void CreateJsonRenderPass(string jsonPath, ivec2 renderPassResolution, SampleCountFlags sampleCount = SampleCountFlags.Count1Bit)
        {
            RenderPassResolution = renderPassResolution;

            string jsonContent2 = File.ReadAllText(ConstConfig.Default2DRenderPass);
            RenderPassBuildInfoModel model2 = JsonConvert.DeserializeObject<RenderPassBuildInfoModel>(jsonContent2);

            FrameBufferList = new Framebuffer[VulkanRenderer.MAX_FRAMES_IN_FLIGHT];
            commandBufferList = new CommandBuffer[VulkanRenderer.MAX_FRAMES_IN_FLIGHT];
            VulkanRenderer.CreateCommandBuffers(commandBufferList);

            CreateRenderPass(model2);
            CreateFramebuffer();
            jsonPipeline = new JsonPipeline(ConstConfig.Default2DPipeline, renderPass, (uint)sizeof(SceneDataBuffer));
        }

        public RenderPass CreateRenderPass(RenderPassBuildInfoModel model2)
        {
            List<AttachmentDescription> attachmentDescriptionList = new List<AttachmentDescription>();
            List<AttachmentReference> inputAttachmentReferenceList = new List<AttachmentReference>();
            List<AttachmentReference> colorAttachmentReferenceList = new List<AttachmentReference>();
            List<AttachmentReference> resolveAttachmentReferenceList = new List<AttachmentReference>();
            List<SubpassDescription> preserveAttachmentReferenceList = new List<SubpassDescription>();
            List<AttachmentReference> depthReferenceList = new List<AttachmentReference>();
            foreach (RenderedTextureInfoModel renderedTextureInfoModel in model2.RenderedTextureInfoModelList)
            {
                attachmentDescriptionList.Add(renderedTextureInfoModel.AttachmentDescription.Convert());
                switch (renderedTextureInfoModel.TextureType)
                {
                    case RenderedTextureType.ColorRenderedTexture:
                        {
                            RenderedColorTextureList.Add(new RenderedTexture(RenderPassResolution, renderedTextureInfoModel.ImageCreateInfo, renderedTextureInfoModel.SamplerCreateInfo));
                            colorAttachmentReferenceList.Add(new AttachmentReference
                            {
                                Attachment = colorAttachmentReferenceList.UCount(),
                                Layout = Silk.NET.Vulkan.ImageLayout.ColorAttachmentOptimal
                            });
                            break;
                        }
                    case RenderedTextureType.DepthRenderedTexture:
                        {
                            depthTexture = new DepthTexture(RenderPassResolution, renderedTextureInfoModel.ImageCreateInfo, renderedTextureInfoModel.SamplerCreateInfo);
                            depthReferenceList.Add(new AttachmentReference
                            {
                                Attachment = (uint)(colorAttachmentReferenceList.Count + resolveAttachmentReferenceList.Count),
                                Layout = Silk.NET.Vulkan.ImageLayout.DepthAttachmentOptimal
                            });
                            break;
                        }
                    case RenderedTextureType.InputAttachmentTexture:
                        {
                            RenderedColorTextureList.Add(new RenderedTexture(RenderPassResolution, renderedTextureInfoModel.ImageCreateInfo, renderedTextureInfoModel.SamplerCreateInfo));
                            inputAttachmentReferenceList.Add(new AttachmentReference
                            {
                                Attachment = (uint)(inputAttachmentReferenceList.Count()),
                                Layout = Silk.NET.Vulkan.ImageLayout.ColorAttachmentOptimal
                            });
                            break;
                        }
                    case RenderedTextureType.ResolveAttachmentTexture:
                        {
                            RenderedColorTextureList.Add(new RenderedTexture(RenderPassResolution, renderedTextureInfoModel.ImageCreateInfo, renderedTextureInfoModel.SamplerCreateInfo));
                            resolveAttachmentReferenceList.Add(new AttachmentReference
                            {
                                Attachment = (uint)(colorAttachmentReferenceList.UCount() + 1),
                                Layout = Silk.NET.Vulkan.ImageLayout.ColorAttachmentOptimal
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

            List<SubpassDescription> subpassDescriptionList = new List<SubpassDescription>();
            fixed (AttachmentReference* depthAttachment = depthReferenceList.ToArray())
            fixed (AttachmentReference* colorAttachments = colorAttachmentReferenceList.ToArray())
            fixed (AttachmentReference* inputAttachments = inputAttachmentReferenceList.ToArray())
            fixed (AttachmentReference* resolveAttachments = resolveAttachmentReferenceList.ToArray())
            fixed (SubpassDescription* preserveAttachments = preserveAttachmentReferenceList.ToArray())
            {
                subpassDescriptionList.Add(new SubpassDescription
                {
                    PipelineBindPoint = PipelineBindPoint.Graphics,
                    ColorAttachmentCount = colorAttachmentReferenceList.UCount(),
                    PColorAttachments = colorAttachments,
                    PDepthStencilAttachment = depthAttachment,
                    PResolveAttachments = resolveAttachments,
                    InputAttachmentCount = inputAttachmentReferenceList.UCount(),
                    PInputAttachments = inputAttachments,
                    PreserveAttachmentCount = preserveAttachmentReferenceList.UCount(),
                    Flags = SubpassDescriptionFlags.None,
                    PPreserveAttachments = null
                });
            }

            List<SubpassDependency> subPassList = new List<SubpassDependency>();
            foreach (VkSubpassDependency subpass in model2.SubpassDependencyList)
            {
                subPassList.Add(subpass.Convert());
            }

            fixed (AttachmentDescription* attachments = attachmentDescriptionList.ToArray())
            fixed (SubpassDescription* description = subpassDescriptionList.ToArray())
            fixed (SubpassDependency* dependency = subPassList.ToArray())
            {
                var renderPassCreateInfo = new RenderPassCreateInfo()
                {
                    SType = StructureType.RenderPassCreateInfo,
                    PNext = null,
                    Flags = RenderPassCreateFlags.None,
                    AttachmentCount = (uint)attachmentDescriptionList.Count(),
                    PAttachments = attachments,
                    SubpassCount = (uint)subpassDescriptionList.Count(),
                    PSubpasses = description,
                    DependencyCount = (uint)subPassList.Count(),
                    PDependencies = dependency,

                };

                var tempRenderPass = new RenderPass();
                vk.CreateRenderPass(VulkanRenderer.device, &renderPassCreateInfo, null, &tempRenderPass);
                renderPass = tempRenderPass;
            }
            return renderPass;
        }

        public void CreateFramebuffer()
        {
            Framebuffer[] frameBufferList = new Framebuffer[VulkanRenderer.swapChain.ImageCount];
            for (int x = 0; x < VulkanRenderer.swapChain.ImageCount; x++)
            {
                List<ImageView> TextureAttachmentList = new List<ImageView>();
                foreach (var texture in RenderedColorTextureList)
                {

                    TextureAttachmentList.Add(VulkanRenderer.swapChain.imageViews[x]);

                }
                if (depthTexture != null)
                {
                    TextureAttachmentList.Add(depthTexture.View);
                }

                fixed (ImageView* imageViewPtr = TextureAttachmentList.ToArray())
                {
                    FramebufferCreateInfo framebufferInfo = new FramebufferCreateInfo()
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

                    Framebuffer frameBuffer = FrameBufferList[x];
                    vk.CreateFramebuffer(VulkanRenderer.device, &framebufferInfo, null, &frameBuffer);
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

            RenderPassBeginInfo renderPassInfo = new
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

            vk.BeginCommandBuffer(commandBuffer, &commandInfo);
            vk.CmdBeginRenderPass(commandBuffer, &renderPassInfo, SubpassContents.Inline);
            vk.CmdSetViewport(commandBuffer, 0, 1, &viewport);
            vk.CmdSetScissor(commandBuffer, 0, 1, &scissor);
            vk.CmdBindPipeline(commandBuffer, PipelineBindPoint.Graphics, jsonPipeline.pipeline);
            foreach (var obj in gameObjectList)
            {
                obj.Draw(commandBuffer.Handle, jsonPipeline.pipeline.Handle, jsonPipeline.pipelineLayout.Handle, descSet.Handle, sceneDataBuffer);
            }
            vk.CmdEndRenderPass(commandBuffer);
            vk.EndCommandBuffer(commandBuffer);

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
                            imageType = ImageType.ImageType2D,
                            format= Format.R8G8B8A8Unorm,
                            mipLevels = 1,
                            arrayLayers = 1,
                            samples = SampleCountFlags.Count1Bit,
                            tiling = 0,
                            usage = ImageUsageFlags.ImageUsageTransferSrcBit |
                                    ImageUsageFlags.SampledBit |
                                    ImageUsageFlags.ImageUsageColorAttachmentBit |
                                    ImageUsageFlags.ImageUsageTransferDstBit,
                            sharingMode = 0,
                            initialLayout =  0,
                            _name = "bnvnb"
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
                            _name = "tyuiy"
                        },
                        AttachmentDescription = new VkAttachmentDescription()
                        {
                            format = Format.R8G8B8A8Unorm,
                            samples = SampleCountFlags.SampleCount1Bit,
                            loadOp = AttachmentLoadOp.Clear,
                            storeOp = AttachmentStoreOp.Store,
                            stencilLoadOp = AttachmentLoadOp.DontCare,
                            stencilStoreOp = AttachmentStoreOp.DontCare,
                            initialLayout = ImageLayout.Undefined,
                            finalLayout = ImageLayout.ShaderReadOnlyOptimal,
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
                            imageType = (ImageType)1,
                            format = Format.D32Sfloat,
                            mipLevels = 1,
                            arrayLayers = 1,
                            samples = (SampleCountFlags)1,
                            tiling = 0,
                            usage =  ImageUsageFlags.ImageUsageTransferSrcBit |
                                    ImageUsageFlags.SampledBit |
                                    ImageUsageFlags.DepthStencilAttachmentBit |
                                    ImageUsageFlags.ImageUsageTransferDstBit,
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
                            borderColor = (BorderColor)2,
                            unnormalizedCoordinates = false,
                            _name = "DefaultColorSampleCreateInfo"
                        },
                        AttachmentDescription = new VkAttachmentDescription()
                        {
                            format = Format.D32Sfloat,
                            samples = SampleCountFlags.SampleCount1Bit,
                            loadOp = AttachmentLoadOp.Clear,
                            storeOp = AttachmentStoreOp.DontCare,
                            stencilLoadOp = AttachmentLoadOp.DontCare,
                            stencilStoreOp = AttachmentStoreOp.DontCare,
                            initialLayout = ImageLayout.Undefined,
                            finalLayout = ImageLayout.DepthStencilAttachmentOptimal,
                        },
                        IsRenderedToSwapchain = false,
                        TextureType = RenderedTextureType.DepthRenderedTexture,
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
                                srcStageMask = (VkPipelineStageFlags)PipelineStageFlags.ColorAttachmentOutputBit,
                                dstStageMask = (VkPipelineStageFlags)PipelineStageFlags.ColorAttachmentOutputBit, // Changed to Early Fragment Tests
                                srcAccessMask = 0,
                                dstAccessMask = (VkAccessFlags)AccessFlags.ColorAttachmentWriteBit, // Ensure this access mask is relevant to the chosen stage mask
                            }
                },
                RenderPipelineList = new List<RenderPipelineModel>(),
                _name = "pipeline"
            };

            string jsonString = JsonConvert.SerializeObject(modelInfo, Formatting.Indented);
            File.WriteAllText(ConstConfig.Default2DRenderPass, jsonString);
        }
    }
}