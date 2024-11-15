using GlmSharp;
using Newtonsoft.Json;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class JsonRenderPass
    {
        Vk vk = Vk.GetApi();
        public Device _device { get; private set; }
        public ivec2 RenderPassResolution { get; set; }
        public SampleCountFlags SampleCount { get; set; }
        public RenderPass renderPass { get; protected set; }
        public CommandBuffer[] commandBufferList { get; protected set; }
        public Framebuffer[] FrameBufferList { get; protected set; }
        public List<RenderedTexture> RenderedColorTextureList { get; private set; } = new List<RenderedTexture>();
        public DepthTexture depthTexture { get; private set; }
        public List<JsonPipeline> pipelineList { get; protected set; }
        public JsonRenderPass()
        {
        }

        public JsonRenderPass(Device device)
        {
            _device = device;
        }

        public void JsonCreateRenderPass(string jsonPath, ivec2 renderPassResolution, SampleCountFlags sampleCount = SampleCountFlags.Count1Bit)
        {
            RenderPassResolution = renderPassResolution;
            SampleCount = sampleCount;
            CreateRenderPass(new RenderPassBuildInfoModel(jsonPath));
            //CreateFramebuffer();
            //pipelineList.Add(new JsonPipeline("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Pipelines", renderPass, (uint)sizeof(SceneDataBuffer)));
        }

        public void SaveRenderPass()
        {
            RenderPassBuildInfoModel model = new RenderPassBuildInfoModel()
            {
                RenderedTextureInfoModelList = new List<RenderedTextureInfoModel>()
                {
                    new RenderedTextureInfoModel()
                    {
                            _name = "Texture2DRenderpass",
                            IsRenderedToSwapchain = false,
                            TextureType = RenderedTextureType.ColorRenderedTexture,
                            AttachmentDescription = new VkAttachmentDescription()
                            {
                                format = Format.B8G8R8A8Unorm,
                                samples = SampleCountFlags.SampleCount1Bit,
                                loadOp = AttachmentLoadOp.Clear,
                                storeOp = AttachmentStoreOp.Store,
                                stencilLoadOp = AttachmentLoadOp.DontCare,
                                stencilStoreOp = AttachmentStoreOp.DontCare,
                                initialLayout = Silk.NET.Vulkan.ImageLayout.Undefined,
                                finalLayout = Silk.NET.Vulkan.ImageLayout.ReadOnlyOptimal
                            },
                            ImageCreateInfo = new VkImageCreateInfo()
                            {
                                sType = StructureType.ImageCreateInfo,
                                imageType = ImageType.ImageType2D,
                                format = Format.B8G8R8A8Unorm,
                                extent = new VkExtent3D(1, 1, 1),
                                mipLevels = 1,
                                arrayLayers = 1,
                                samples = SampleCountFlags.SampleCount1Bit,
                                tiling = ImageTiling.Optimal,
                                usage = ImageUsageFlags.ImageUsageTransferSrcBit |
                                        ImageUsageFlags.ImageUsageTransferDstBit |
                                        ImageUsageFlags.ImageUsageSampledBit |
                                        ImageUsageFlags.ImageUsageColorAttachmentBit,
                                sharingMode = SharingMode.Exclusive,
                                initialLayout = Silk.NET.Vulkan.ImageLayout.Undefined,
                            },
                            SamplerCreateInfo = new VkSamplerCreateInfo()
                            {
                                structureType = StructureType.SamplerCreateInfo,
                                magFilter = Filter.Nearest,
                                minFilter = Filter.Nearest,
                                mipmapMode = SamplerMipmapMode.Linear,
                                addressModeU = SamplerAddressMode.Repeat,
                                addressModeV = SamplerAddressMode.Repeat,
                                addressModeW = SamplerAddressMode.Repeat,
                                mipLodBias = 0.0f,
                                anisotropyEnable = true,
                                maxAnisotropy = 16.0f,
                                compareEnable = false,
                                compareOp = CompareOp.Always,
                                minLod = 0.0f,
                                maxLod = 1,
                                borderColor = BorderColor.IntOpaqueBlack,
                                unnormalizedCoordinates = false,
                            },
                            RenderedTextureInfoName = "Rendered2DTexture"
                    }
                },
                SubpassDependencyList = new List<VkSubpassDependency>()
                {
                    new VkSubpassDependency()
                    {
                         srcSubpass = (~0U),
                         dstSubpass = 0,
                         srcStageMask = (VkPipelineStageFlags)PipelineStageFlags.ColorAttachmentOutputBit,
                         dstStageMask = (VkPipelineStageFlags)PipelineStageFlags.ColorAttachmentOutputBit,
                         srcAccessMask = 0,
                         dstAccessMask = (VkAccessFlags)AccessFlags.ColorAttachmentWriteBit
                    }
                }
            };

            string finalfilePath = @"C:\Users\dotha\Documents\GitHub\VulkanGameEngine\Pipelines\Default2DPipeline.json";
            string jsonString = JsonConvert.SerializeObject(model, Formatting.Indented);
            File.WriteAllText(finalfilePath, jsonString);
        }

        public void CreateRenderPass(RenderPassBuildInfoModel model)
        {
            List<AttachmentDescription> attachmentDescriptionList = new List<AttachmentDescription>();
            List<AttachmentReference> inputAttachmentReferenceList = new List<AttachmentReference>();
            List<AttachmentReference> colorAttachmentReferenceList = new List<AttachmentReference>();
            List<AttachmentReference> resolveAttachmentReferenceList = new List<AttachmentReference>();
            List<SubpassDescription> preserveAttachmentReferenceList = new List<SubpassDescription>();
            AttachmentReference depthReference = new AttachmentReference();
            foreach (RenderedTextureInfoModel renderedTextureInfoModel in model.RenderedTextureInfoModelList)
            {
                attachmentDescriptionList.Add(renderedTextureInfoModel.AttachmentDescription.Convert());
                switch (renderedTextureInfoModel.TextureType)
                {
                    case RenderedTextureType.ColorRenderedTexture:
                        {
                            if (!renderedTextureInfoModel.IsRenderedToSwapchain)
                            {
                                RenderedColorTextureList.Add(new RenderedTexture());
                            }
                            else
                            {
                                RenderedColorTextureList.Add(new RenderedTexture(renderedTextureInfoModel.ImageCreateInfo, renderedTextureInfoModel.SamplerCreateInfo));
                            }
                            colorAttachmentReferenceList.Add(new AttachmentReference
                            {
                                Attachment = colorAttachmentReferenceList.UCount(),
                                Layout = Silk.NET.Vulkan.ImageLayout.ColorAttachmentOptimal
                            });
                            break;
                        }
                    case RenderedTextureType.DepthRenderedTexture:
                        {
                            depthTexture = new DepthTexture(renderedTextureInfoModel.ImageCreateInfo, renderedTextureInfoModel.SamplerCreateInfo);
                            depthReference = new AttachmentReference
                            {
                                Attachment = (uint)(colorAttachmentReferenceList.Count + resolveAttachmentReferenceList.Count),
                                Layout = Silk.NET.Vulkan.ImageLayout.DepthAttachmentOptimal
                            };
                            break;
                        }
                    case RenderedTextureType.InputAttachmentTexture:
                        {
                            RenderedColorTextureList.Add(new RenderedTexture(renderedTextureInfoModel.ImageCreateInfo, renderedTextureInfoModel.SamplerCreateInfo));
                            inputAttachmentReferenceList.Add(new AttachmentReference
                            {
                                Attachment = (uint)(inputAttachmentReferenceList.Count()),
                                Layout = Silk.NET.Vulkan.ImageLayout.ColorAttachmentOptimal
                            });
                            break;
                        }
                    case RenderedTextureType.ResolveAttachmentTexture:
                        {
                            RenderedColorTextureList.Add(new RenderedTexture(renderedTextureInfoModel.ImageCreateInfo, renderedTextureInfoModel.SamplerCreateInfo));
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
            var depthAttachment = &depthReference;
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

            List<VkSubpassDependency> subPassList = new List<VkSubpassDependency>();
            foreach (VkSubpassDependency subpass in model.SubpassDependencyList)
            {
                subPassList.Add(subpass);
            }

            fixed (AttachmentDescription* attachments = attachmentDescriptionList.ToArray())
            fixed (SubpassDescription* description = subpassDescriptionList.ToArray())
            fixed (VkSubpassDependency* dependency = subPassList.ToArray())
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
                    PDependencies = (SubpassDependency*)dependency,

                };

                var tempRenderPass = new RenderPass();
                vk.CreateRenderPass(VulkanRenderer.device, &renderPassCreateInfo, null, &tempRenderPass);
                renderPass = tempRenderPass;
            }
        }

        private void CreateFramebuffer()
        {
            Framebuffer[] frameBufferList = new Framebuffer[VulkanRenderer.swapChain.ImageCount];
            for (int x = 0; x < VulkanRenderer.swapChain.ImageCount; x++)
            {
                List<ImageView> TextureAttachmentList = new List<ImageView>();
                TextureAttachmentList.Add(VulkanRenderer.swapChain.imageViews[x]);
                TextureAttachmentList.Add(depthTexture.View);

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
                new ClearValue(null, new ClearDepthStencilValue(1.0f, 0))
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

            var descSet = pipelineList.First().descriptorSet;
            var commandInfo = new CommandBufferBeginInfo(flags: 0);

            vk.BeginCommandBuffer(commandBuffer, &commandInfo);
            vk.CmdBeginRenderPass(commandBuffer, &renderPassInfo, SubpassContents.Inline);
            vk.CmdSetViewport(commandBuffer, 0, 1, &viewport);
            vk.CmdSetScissor(commandBuffer, 0, 1, &scissor);
            vk.CmdBindPipeline(commandBuffer, PipelineBindPoint.Graphics, pipelineList.First().pipeline);
            foreach (var obj in gameObjectList)
            {
                obj.Draw(commandBuffer, pipelineList.First().pipeline, pipelineList.First().pipelineLayout, descSet, sceneDataBuffer);
            }
            vk.CmdEndRenderPass(commandBuffer);
            vk.EndCommandBuffer(commandBuffer);

            return commandBuffer;
        }
    }
}
