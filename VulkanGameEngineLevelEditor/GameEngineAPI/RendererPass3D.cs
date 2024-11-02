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
        public Texture renderedTexture { get; set; }
        public VulkanBuffer<Vertex3D> vertexBuffer { get; set; }
        public VulkanBuffer<ushort> indexBuffer { get; set; }
        public VulkanBuffer<UniformBufferObject> uniformBuffers { get; set; }
        public List<RenderedTexture> RenderedColorTextureList { get; private set; } = new List<RenderedTexture>();
        public JsonRenderPass buildRenderPass { get; set; } = new JsonRenderPass();
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

            JsonCreateRenderPass(@$"{RenderPassEditorConsts.BasePath}DefaultRenderPass.json");
            //CreateDescriptorSetLayout();
            //CreateDescriptorPoolBinding();
            //allocateDescriptorSets();
            //UpdateDescriptorSet();
            //CreateFramebuffer();
         //   CreateGraphicsPipeline();


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
                        AttachmentDescription = new AttachmentDescriptionModel(RenderPassEditorConsts.DefaultColorAttachmentDescriptionModel),
                        ImageCreateInfo = new ImageCreateInfoModel(RenderPassEditorConsts.DefaultCreateColorImageInfo, SwapChainResuloution, Format.R8G8B8A8Unorm),
                        SamplerCreateInfo = new SamplerCreateInfoModel(RenderPassEditorConsts.DefaultColorSamplerCreateInfo),
                        TextureType = RenderedTextureType.ColorRenderedTexture
                    },
                    new RenderedTextureInfoModel()
                    {
                        IsRenderedToSwapchain = false,
                        RenderedTextureInfoName = "DepthRenderedTexture",
                        AttachmentDescription = new AttachmentDescriptionModel(RenderPassEditorConsts.DefaultDepthAttachmentDescriptionModel),
                        ImageCreateInfo = new ImageCreateInfoModel(RenderPassEditorConsts.DefaultCreateDepthImageInfo, SwapChainResuloution, Format.D32Sfloat),
                        SamplerCreateInfo = new SamplerCreateInfoModel(RenderPassEditorConsts.DefaultDepthSamplerCreateInfo),
                        TextureType = RenderedTextureType.DepthRenderedTexture
                    }
                },
                SubpassDependencyList = new List<SubpassDependencyModel>() { new SubpassDependencyModel(RenderPassEditorConsts.DefaultSubpassDependencyModel) },
                SwapChainResuloution = SwapChainResuloution
            };

            List<AttachmentDescription> attachmentDescriptionList = new List<AttachmentDescription>();
            List<AttachmentReference> inputAttachmentReferenceList = new List<AttachmentReference>();
            List<AttachmentReference> colorAttachmentReferenceList = new List<AttachmentReference>();
            List<AttachmentReference> resolveAttachmentReferenceList = new List<AttachmentReference>();
            List<SubpassDescription> preserveAttachmentReferenceList = new List<SubpassDescription>();
            AttachmentReference depthReference = new AttachmentReference();
            foreach (RenderedTextureInfoModel renderedTextureInfoModel in a.RenderedTextureInfoModelList)
            {
                attachmentDescriptionList.Add(renderedTextureInfoModel.AttachmentDescription.ConvertToVulkan());
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
                            depthReference = new AttachmentReference
                            {
                                Attachment = (uint)(colorAttachmentReferenceList.Count + resolveAttachmentReferenceList.Count),
                                Layout = Silk.NET.Vulkan.ImageLayout.DepthAttachmentOptimal
                            };
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

            List<SubpassDependency> subPassList = new List<SubpassDependency>();
            foreach (SubpassDependencyModel subpass in a.SubpassDependencyList)
            {
                subPassList.Add(subpass.ConvertToVulkan());
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

        void CreateGraphicsPipeline()
        {

            PipelineShaderStageCreateInfo* shadermoduleList = stackalloc[]
            {
                VulkanRenderer.CreateShader("vertshader.spv",  ShaderStageFlags.VertexBit),
                VulkanRenderer.CreateShader("fragshader.spv", ShaderStageFlags.FragmentBit)
            };

            DescriptorSetLayout descriptorSetLayoutPtr = descriptorSetLayout;
            PipelineLayoutCreateInfo pipelineLayoutInfo = new
            (
                setLayoutCount: 1,
                pSetLayouts: &descriptorSetLayoutPtr,
                pushConstantRangeCount: 0,
                pPushConstantRanges: null
            );
            vk.CreatePipelineLayout(VulkanRenderer.device, &pipelineLayoutInfo, null, out PipelineLayout pipelinelayout);
            shaderpipelineLayout = pipelinelayout;

            PipelineVertexInputStateCreateInfo vertexInputInfo = new PipelineVertexInputStateCreateInfo();
            List<VertexInputBindingDescription> bindingDescriptionList = Vertex3D.GetBindingDescriptions();
            List<VertexInputAttributeDescription> AttributeDescriptions = Vertex3D.GetAttributeDescriptions();

            fixed (VertexInputBindingDescription* bindingDescription = bindingDescriptionList.ToArray())
            fixed (VertexInputAttributeDescription* AttributeDescription = AttributeDescriptions.ToArray())
            {
                vertexInputInfo.SType = StructureType.PipelineVertexInputStateCreateInfo;
                vertexInputInfo.VertexBindingDescriptionCount = (uint)bindingDescriptionList.Count;
                vertexInputInfo.PVertexBindingDescriptions = bindingDescription;
                vertexInputInfo.VertexAttributeDescriptionCount = (uint)AttributeDescriptions.Count;
                vertexInputInfo.PVertexAttributeDescriptions = AttributeDescription;
            }

            PipelineInputAssemblyStateCreateInfo pipelineInputAssemblyStateCreateInfo = new(topology: PrimitiveTopology.TriangleList);




            PipelineViewportStateCreateInfo pipelineViewportStateCreateInfo = new
            (
                viewportCount: 0,
                pViewports: null,
                scissorCount: 0,
                pScissors: null
            );

            PipelineRasterizationStateCreateInfo pipelineRasterizationStateCreateInfo = new
            (
                depthClampEnable: false,
                rasterizerDiscardEnable: false,
                polygonMode: PolygonMode.Fill,
                cullMode: CullModeFlags.CullModeBackBit,
                frontFace: FrontFace.CounterClockwise,
                depthBiasEnable: false,
                depthBiasConstantFactor: 0.0f,
                depthBiasClamp: 0.0f,
                depthBiasSlopeFactor: 0.0f,
                lineWidth: 1.0f
            );

            PipelineMultisampleStateCreateInfo pipelineMultisampleStateCreateInfo = new(rasterizationSamples: SampleCountFlags.SampleCount1Bit);

            StencilOpState stencilOpState = new(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep, CompareOp.Always);

            PipelineDepthStencilStateCreateInfo pipelineDepthStencilStateCreateInfo = new
            (
                depthTestEnable: true,
                depthWriteEnable: true,
                depthCompareOp: CompareOp.LessOrEqual,
                depthBoundsTestEnable: false,
                stencilTestEnable: false,
                front: stencilOpState,
                back: stencilOpState
            );

            ColorComponentFlags colorComponentFlags =
                ColorComponentFlags.ColorComponentRBit |
                ColorComponentFlags.ColorComponentGBit |
                ColorComponentFlags.ColorComponentBBit |
                ColorComponentFlags.ColorComponentABit;

            List<PipelineColorBlendAttachmentState> pipelineColorBlendAttachmentState = new List<PipelineColorBlendAttachmentState>()
            {
                new(
                    blendEnable: true,
                    srcColorBlendFactor: Silk.NET.Vulkan.BlendFactor.SrcAlpha,
                    dstColorBlendFactor: Silk.NET.Vulkan.BlendFactor.OneMinusSrcAlpha,
                    colorBlendOp: BlendOp.Add,
                    srcAlphaBlendFactor: Silk.NET.Vulkan.BlendFactor.One,
                    dstAlphaBlendFactor: Silk.NET.Vulkan.BlendFactor.Zero,
                    alphaBlendOp: BlendOp.Add,
                    colorWriteMask: ColorComponentFlags.RBit | ColorComponentFlags.GBit | ColorComponentFlags.BBit | ColorComponentFlags.ABit
                )
            };



            fixed (PipelineColorBlendAttachmentState* attachments = pipelineColorBlendAttachmentState.ToArray())
            {
                PipelineColorBlendStateCreateInfo pipelineColorBlendStateCreateInfo = new
               (
                   logicOpEnable: false,
                   logicOp: LogicOp.NoOp,
                   attachmentCount: 2,
                   pAttachments: attachments
               );

                pipelineColorBlendStateCreateInfo.BlendConstants[0] = 0.0f;
                pipelineColorBlendStateCreateInfo.BlendConstants[1] = 0.0f;
                pipelineColorBlendStateCreateInfo.BlendConstants[2] = 0.0f;
                pipelineColorBlendStateCreateInfo.BlendConstants[3] = 0.0f;

                DynamicState* dynamicStates = stackalloc[] { DynamicState.Viewport, DynamicState.Scissor };

                PipelineDynamicStateCreateInfo pipelineDynamicStateCreateInfo = new
                (
                    dynamicStateCount: 2,
                    pDynamicStates: dynamicStates
                );

                GraphicsPipelineCreateInfo graphicsPipelineCreateInfo = new
                (
                    stageCount: 2,
                    pStages: shadermoduleList,
                    pVertexInputState: &vertexInputInfo,
                    pInputAssemblyState: &pipelineInputAssemblyStateCreateInfo,
                    pViewportState: &pipelineViewportStateCreateInfo,
                    pRasterizationState: &pipelineRasterizationStateCreateInfo,
                    pMultisampleState: &pipelineMultisampleStateCreateInfo,
                    pDepthStencilState: &pipelineDepthStencilStateCreateInfo,
                    pColorBlendState: &pipelineColorBlendStateCreateInfo,
                    pDynamicState: &pipelineDynamicStateCreateInfo,
                    layout: shaderpipelineLayout,
                    renderPass: renderPass
                );

                //var jsonObj = new RenderPipelineModel
                //{
                //    _name = "DefaultPipeline",
                //    VertexShader = "vertshader.spv",
                //    FragmentShader = "fragshader.spv",
                //    PipelineColorBlendAttachmentStateList = pipelineColorBlendAttachmentState,
                //    PipelineDepthStencilStateCreateInfo = pipelineDepthStencilStateCreateInfo,
                //    PipelineMultisampleStateCreateInfo = pipelineMultisampleStateCreateInfo,
                //    PipelineRasterizationStateCreateInfo = pipelineRasterizationStateCreateInfo,
                //    ScissorList = new List<Rect2D>(),
                //    ViewportList = new List<Viewport>(),
                //    PipelineColorBlendStateCreateInfoModel = new PipelineColorBlendStateCreateInfoModel()
                //    {
                //        LogicOpEnable = false,
                //        LogicOp = LogicOp.NoOp,
                //        BlendConstants = new float[4] { 0.0f, 0.0f, 0.0f, 0.0f }
                //    },
                //    PipelineInputAssemblyStateCreateInfo = new PipelineInputAssemblyStateCreateInfoModel()
                //    {
                //        PrimitiveRestartEnable = false,
                //        Topology = PrimitiveTopology.TriangleList
                //    },
                //    PipelineDescriptorModelsList = new List<PipelineDescriptorModel>()
                //    {
                //        new PipelineDescriptorModel
                //        {
                //            BindingNumber = 0,
                //            BindingPropertiesList = DescriptorBindingPropertiesEnum.kMeshPropertiesDescriptor,
                //            descriptorType = DescriptorType.StorageBuffer
                //        },
                //        new PipelineDescriptorModel
                //        {
                //            BindingNumber = 1,
                //            BindingPropertiesList = DescriptorBindingPropertiesEnum.kTextureDescriptor,
                //            descriptorType = DescriptorType.CombinedImageSampler
                //        }
                //    },
                //    LayoutBindingList = new List<DescriptorSetLayoutBinding>()
                //    {
                //        new DescriptorSetLayoutBinding()
                //        {
                //            Binding = 0,
                //            DescriptorType = DescriptorType.StorageBuffer,
                //            DescriptorCount = 1,
                //            StageFlags = ShaderStageFlags.VertexBit | ShaderStageFlags.FragmentBit,
                //            PImmutableSamplers = null
                //        },
                //        new DescriptorSetLayoutBinding()
                //        {
                //            Binding = 1,
                //            DescriptorType = DescriptorType.CombinedImageSampler,
                //            DescriptorCount = 1,
                //            StageFlags = ShaderStageFlags.FragmentBit,
                //            PImmutableSamplers = null
                //        }
                //    }
                //};

                //string finalfilePath = @"C:\Users\dotha\Documents\GitHub\VulkanGameEngine\Pipelines\DefaultPipeline.json";
                //string jsonString = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                //File.WriteAllText(finalfilePath, jsonString);

                vk.CreateGraphicsPipelines(VulkanRenderer.device, new PipelineCache(null), 1, &graphicsPipelineCreateInfo, null, out Pipeline pipeline);
                shaderpipeline = pipeline;
            };

        

            //SilkMarshal.Free((nint)pipelineShaderStageCreateInfos[0].PName);
            //SilkMarshal.Free((nint)pipelineShaderStageCreateInfos[1].PName);

            // Mem.FreeArray(vertexInputAttributeDescriptions);

            //vertShaderModule.Dispose();
            // fragShaderModule.Dispose();
        }

        public DescriptorSetLayout CreateDescriptorSetLayout()
        {
            List<DescriptorSetLayoutBinding> LayoutBindingList = new List<DescriptorSetLayoutBinding>()
                {
                    new DescriptorSetLayoutBinding()
                    {
                        Binding = 0,
                        DescriptorType = DescriptorType.StorageBuffer,
                        DescriptorCount = 1,
                        StageFlags = ShaderStageFlags.VertexBit | ShaderStageFlags.FragmentBit,
                        PImmutableSamplers = null
                    },
                    new DescriptorSetLayoutBinding()
                    {
                        Binding = 1,
                        DescriptorType = DescriptorType.CombinedImageSampler,
                        DescriptorCount = 1,
                        StageFlags = ShaderStageFlags.FragmentBit,
                        PImmutableSamplers = null
                    }
                };

            fixed (DescriptorSetLayoutBinding* ptr = LayoutBindingList.ToArray())
            {
                DescriptorSetLayoutCreateInfo layoutInfo = new DescriptorSetLayoutCreateInfo()
                {
                    SType = StructureType.DescriptorSetLayoutCreateInfo,
                    BindingCount = (uint)LayoutBindingList.Count,
                    PBindings = ptr,
                };
                vk.CreateDescriptorSetLayout(VulkanRenderer.device, &layoutInfo, null, out DescriptorSetLayout descriptorsetLayout);
                descriptorSetLayout = descriptorsetLayout;
                return descriptorSetLayout;
            }
        }

        public DescriptorSet CreateDescriptorSets(DescriptorSet descriptorSets, DescriptorPool descPool)
        {

            DescriptorSetLayout* layouts = stackalloc DescriptorSetLayout[VulkanRenderer.MAX_FRAMES_IN_FLIGHT];

            for (int i = 0; i < VulkanRenderer.MAX_FRAMES_IN_FLIGHT; i++)
            {
                layouts[i] = descriptorSetLayout;
            }

            DescriptorSetAllocateInfo allocInfo = new
            (
                descriptorPool: descPool,
                descriptorSetCount: VulkanRenderer.MAX_FRAMES_IN_FLIGHT,
                pSetLayouts: layouts
            );
            vk.AllocateDescriptorSets(VulkanRenderer.device, &allocInfo, out DescriptorSet descriptorSet);
            descriptorSets = descriptorSet;
            return descriptorSets;
        }

        public Framebuffer[] CreateFramebuffer()
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
                        Width = VulkanRenderer.swapChain.swapchainExtent.Width,
                        Height = VulkanRenderer.swapChain.swapchainExtent.Height,
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
            return frameBufferList;
        }

        public DescriptorPool CreateDescriptorPoolBinding()
        {
            DescriptorPool descriptorPool = new DescriptorPool();
            List<DescriptorPoolSize> DescriptorPoolBinding = new List<DescriptorPoolSize>();
            {
                new DescriptorPoolSize
                {
                    Type = DescriptorType.StorageBuffer,
                    DescriptorCount = VulkanRenderer.MAX_FRAMES_IN_FLIGHT
                };
                new DescriptorPoolSize
                {
                    Type = DescriptorType.CombinedImageSampler,
                    DescriptorCount = VulkanRenderer.MAX_FRAMES_IN_FLIGHT
                };
            };

            fixed (DescriptorPoolSize* ptr = DescriptorPoolBinding.ToArray())
            {
                DescriptorPoolCreateInfo poolInfo = new DescriptorPoolCreateInfo()
                {
                    SType = StructureType.DescriptorPoolCreateInfo,
                    MaxSets = VulkanRenderer.swapChain.ImageCount,
                    PoolSizeCount = (uint)DescriptorPoolBinding.Count,
                    PPoolSizes = ptr
                };
                vk.CreateDescriptorPool(VulkanRenderer.device, in poolInfo, null, out descriptorPool);
            }

            descriptorpool = descriptorPool;
            return descriptorPool;
        }


        public DescriptorSet allocateDescriptorSets()
        {
            DescriptorSetLayout* layouts = stackalloc DescriptorSetLayout[VulkanRenderer.MAX_FRAMES_IN_FLIGHT];

            for (int i = 0; i < VulkanRenderer.MAX_FRAMES_IN_FLIGHT; i++)
            {
                layouts[i] = descriptorSetLayout;
            }

            DescriptorSetAllocateInfo allocInfo = new
            (
                descriptorPool: descriptorpool,
                descriptorSetCount: VulkanRenderer.MAX_FRAMES_IN_FLIGHT,
                pSetLayouts: layouts
            );
            vk.AllocateDescriptorSets(VulkanRenderer.device, &allocInfo, out DescriptorSet descriptorSet);
            descriptorset = descriptorSet;
            return descriptorSet;
        }

        public void UpdateDescriptorSet()
        {
            var colorDescriptorImage = new DescriptorImageInfo
            {
                Sampler = texture.Sampler,
                ImageView = texture.View,
                ImageLayout = Silk.NET.Vulkan.ImageLayout.ReadOnlyOptimal
            };

            List<WriteDescriptorSet> descriptorSetList = new List<WriteDescriptorSet>();
            for (uint x = 0; x < VulkanRenderer.swapChain.ImageCount; x++)
            {
                var gameObjectBufferInfoList = MemoryManager.GetGameObjectPropertiesBuffer();
                var meshBuffer = gameObjectBufferInfoList[0];
                WriteDescriptorSet descriptorSetWrite = new WriteDescriptorSet
                {
                    SType = StructureType.WriteDescriptorSet,
                    DstSet = descriptorset,
                    DstBinding = 0,
                    DstArrayElement = 0,
                    DescriptorCount = 1,
                    DescriptorType = DescriptorType.StorageBuffer,
                    PImageInfo = null,
                    PBufferInfo = &meshBuffer,
                    PTexelBufferView = null,
                };

                WriteDescriptorSet descriptorSetWrite2 = new WriteDescriptorSet
                {
                    SType = StructureType.WriteDescriptorSet,
                    DstSet = descriptorset,
                    DstBinding = 1,
                    DstArrayElement = 0,
                    DescriptorCount = 1,
                    DescriptorType = DescriptorType.CombinedImageSampler,
                    PImageInfo = &colorDescriptorImage,
                    PBufferInfo = null,
                    PTexelBufferView = null
                };

                descriptorSetList.Add(descriptorSetWrite);
                descriptorSetList.Add(descriptorSetWrite2);
            }


            fixed (WriteDescriptorSet* ptr = descriptorSetList.ToArray())
            {
                vk.UpdateDescriptorSets(VulkanRenderer.device, (uint)descriptorSetList.UCount(), ptr, 0, null);
            }
        }

        public PipelineLayout CreatePipelineLayout()
        {
            PipelineLayout pipelineLayout = new PipelineLayout();

            DescriptorSetLayout descriptorSetLayoutPtr = descriptorSetLayout;
            PipelineLayoutCreateInfo pipelineLayoutInfo = new
            (
                setLayoutCount: 1,
                pSetLayouts: &descriptorSetLayoutPtr,
                pushConstantRangeCount: 0,
                pPushConstantRanges: null
            );
            vk.CreatePipelineLayout(VulkanRenderer.device, &pipelineLayoutInfo, null, out PipelineLayout pipelinelayout2);
            shaderpipelineLayout = pipelinelayout2;
            return shaderpipelineLayout;

        }

        public CommandBuffer Draw(List<GameObject> gameObjectList, SceneDataBuffer sceneDataBuffer)
        {
            return Renderer3D.Draw(commandBufferList, renderPass, FrameBufferList, RenderPassResolution, gameObjectList, sceneDataBuffer);
        }
    }
}