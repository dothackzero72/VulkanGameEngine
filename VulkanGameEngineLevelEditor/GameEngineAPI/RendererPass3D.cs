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
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.RenderPassEditor;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class RenderPass3D : SilkRenderPassBase
    {
        Vk vk = Vk.GetApi();
        public DepthTexture depthTexture { get; set; }
        public Texture texture { get; set; }
        public List<RenderedTexture> RenderedColorTextureList { get; private set; } = new List<RenderedTexture>();
        public JsonPipeline Renderer3D { get; set; } = new JsonPipeline();

        public RenderPass3D() : base()
        {
        }

        public void Create3dRenderPass()
        {
            depthTexture = new DepthTexture(new ivec2((int)VulkanRenderer.swapChain.swapchainExtent.Width, (int)VulkanRenderer.swapChain.swapchainExtent.Height));
            texture = new GameEngineAPI.Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\VulkanGameEngineLevelEditor\\bin\\Debug\\awesomeface.png", Format.R8G8B8A8Unorm, TextureTypeEnum.kType_DiffuseTextureMap);

            FrameBufferList = new Framebuffer[VulkanRenderer.MAX_FRAMES_IN_FLIGHT];
            commandBufferList = new CommandBuffer[VulkanRenderer.MAX_FRAMES_IN_FLIGHT];
            VulkanRenderer.CreateCommandBuffers(commandBufferList);

            JsonCreateRenderPass(@$"{RenderPassEditorConsts.RenderPassBasePath}DefaultRenderPass.json");
            CreateFramebuffer();
            Renderer3D = new JsonPipeline("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Pipelines\\DefaultPipeline.json", renderPass, (uint)sizeof(SceneDataBuffer));
        }

        public void JsonCreateRenderPass(string jsonPath)
        {
            CreateRenderPass(new RenderPassBuildInfoModel(jsonPath));
        }

        public RenderPass CreateRenderPass(RenderPassBuildInfoModel model)
        {
            var SwapChainResuloution = new ivec2((int)VulkanRenderer.swapChain.swapchainExtent.Width, (int)VulkanRenderer.swapChain.swapchainExtent.Height);
            var a = new RenderPassBuildInfoModel
            {
                _name = "BasicRenderPass",
                RenderedTextureInfoModelList = new List<RenderedTextureInfoModel>
                {
                    new RenderedTextureInfoModel()
                    {
                        IsRenderedToSwapchain = true,
                        RenderedTextureInfoName = "ColorRenderTexture",
                        AttachmentDescription = new VkAttachmentDescription(RenderPassEditorConsts.DefaultColorAttachmentDescriptionModel),
                        ImageCreateInfo = new VkImageCreateInfo(RenderPassEditorConsts.DefaultCreateColorImageInfo, SwapChainResuloution, Format.R8G8B8A8Unorm),
                        SamplerCreateInfo = new VkSamplerCreateInfo(RenderPassEditorConsts.DefaultColorSamplerCreateInfo),
                        TextureType = RenderedTextureType.ColorRenderedTexture
                    },
                    new RenderedTextureInfoModel()
                    {
                        IsRenderedToSwapchain = false,
                        RenderedTextureInfoName = "DepthRenderedTexture",
                        AttachmentDescription = new VkAttachmentDescription(RenderPassEditorConsts.DefaultDepthAttachmentDescriptionModel),
                        ImageCreateInfo = new VkImageCreateInfo(RenderPassEditorConsts.DefaultCreateDepthImageInfo, SwapChainResuloution, Format.D32Sfloat),
                        SamplerCreateInfo = new VkSamplerCreateInfo(RenderPassEditorConsts.DefaultDepthSamplerCreateInfo),
                        TextureType = RenderedTextureType.DepthRenderedTexture
                    }
                },
                SubpassDependencyList = new List<VkSubpassDependency>() { new VkSubpassDependency(RenderPassEditorConsts.DefaultSubpassDependencyModel) },
            };

            List<AttachmentDescription> attachmentDescriptionList = new List<AttachmentDescription>();
            List<AttachmentReference> inputAttachmentReferenceList = new List<AttachmentReference>();
            List<AttachmentReference> colorAttachmentReferenceList = new List<AttachmentReference>();
            List<AttachmentReference> resolveAttachmentReferenceList = new List<AttachmentReference>();
            List<SubpassDescription> preserveAttachmentReferenceList = new List<SubpassDescription>();
            List<AttachmentReference> depthReferenceList = new List<AttachmentReference>();
            foreach (RenderedTextureInfoModel renderedTextureInfoModel in a.RenderedTextureInfoModelList)
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
                            depthReferenceList.Add(new AttachmentReference
                            {
                                Attachment = (uint)(colorAttachmentReferenceList.Count + resolveAttachmentReferenceList.Count),
                                Layout = Silk.NET.Vulkan.ImageLayout.DepthAttachmentOptimal
                            });
                            break;
                        }
                    case RenderedTextureType.InputAttachmentTexture:
                        {
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
                var subpassDesc = new SubpassDescription
                {
                    PipelineBindPoint = PipelineBindPoint.Graphics,
                    ColorAttachmentCount = colorAttachmentReferenceList.UCount(),
                    PColorAttachments = colorAttachments,
                    PDepthStencilAttachment = null,
                    PResolveAttachments = resolveAttachments,
                    InputAttachmentCount = inputAttachmentReferenceList.UCount(),
                    PInputAttachments = inputAttachments,
                    PreserveAttachmentCount = preserveAttachmentReferenceList.UCount(),
                    Flags = SubpassDescriptionFlags.None,
                    PPreserveAttachments = null
                };
                if (depthReferenceList.Count() > 0)
                {
                    subpassDesc.PDepthStencilAttachment = depthAttachment;
                }
                subpassDescriptionList.Add(subpassDesc);
            }

            List<SubpassDependency> subPassList = new List<SubpassDependency>();
            foreach (VkSubpassDependency subpass in a.SubpassDependencyList)
            {
                subPassList.Add(subpass.Convert());
            }

            fixed (AttachmentDescription* attachments = attachmentDescriptionList.ToArray())
            fixed (SubpassDescription* description = subpassDescriptionList.ToArray())
            fixed (SubpassDependency* dependency = subPassList.ToArray())
            fixed (AttachmentReference* depthAttachmentDesc = depthReferenceList.ToArray())
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
                    PDependencies = dependency
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
                TextureAttachmentList.Add(VulkanRenderer.swapChain.imageViews[x]);

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
            return Renderer3D.Draw(commandBufferList, renderPass, FrameBufferList, RenderPassResolution, gameObjectList, sceneDataBuffer);
        }
    }
}