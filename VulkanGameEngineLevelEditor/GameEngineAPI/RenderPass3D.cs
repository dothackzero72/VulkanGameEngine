using GlmSharp;
using Silk.NET.Core.Native;
using Silk.NET.Maths;
using Silk.NET.SDL;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public Texture renderedTexture { get; set; }
        public List<RenderedTexture> RenderedColorTextureList { get; private set; } = new List<RenderedTexture>();
        public JsonRenderPass buildRenderPass { get; set; } = new JsonRenderPass();

        public RenderPass3D() : base()
        {
        }

        public void Create3dRenderPass()
        {
            depthTexture = new DepthTexture(new ivec2((int)VulkanRenderer.swapChain.swapchainExtent.Width, (int)VulkanRenderer.swapChain.swapchainExtent.Height));

            JsonCreateRenderPass(@$"{RenderPassEditorConsts.BasePath}DefaultRenderPass.json");
            CreateDescriptorSetLayout();
            CreateGraphicsPipeline();
            CreateFramebuffer();
            CreateDescriptorPoolBinding();
            allocateDescriptorSets(descriptorpool);
            UpdateDescriptorSet(descriptorset);
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
                VulkanRenderer.CreateShader("C:/Users/dotha/Documents/GitHub/VulkanGameEngine/Shaders/Shader3DVert.spv",  ShaderStageFlags.VertexBit),
                VulkanRenderer.CreateShader("C:/Users/dotha/Documents/GitHub/VulkanGameEngine/Shaders/Shader3DFrag.spv", ShaderStageFlags.FragmentBit)
            };
            shaderpipelineLayout = CreatePipelineLayout();


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
                viewportCount: 1,
                pViewports: null,
                scissorCount: 1,
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

                vk.CreateGraphicsPipelines(VulkanRenderer.device, new PipelineCache(null), 1, &graphicsPipelineCreateInfo, null, out Pipeline pipeline);
                shaderpipeline = pipeline;
            }
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
                        DescriptorCount = MemoryManager.GameObjectMemoryPool.ObjectCount,
                        StageFlags = ShaderStageFlags.VertexBit | ShaderStageFlags.FragmentBit,
                        PImmutableSamplers = null
                    },
                    new DescriptorSetLayoutBinding()
                    {
                        Binding = 1,
                        DescriptorType = DescriptorType.CombinedImageSampler,
                        DescriptorCount = MemoryManager.TextureMemoryPool.ObjectCount,
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
                        Layers = 1
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
                    DescriptorCount = MemoryManager.GameObjectMemoryPool.ObjectCount
                };
                new DescriptorPoolSize
                {
                    Type = DescriptorType.CombinedImageSampler,
                    DescriptorCount = MemoryManager.TextureMemoryPool.ObjectCount
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


        public DescriptorSet allocateDescriptorSets(DescriptorPool descriptorPool354)
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

        public void UpdateDescriptorSet(DescriptorSet descriptorSets)
        {
            List<WriteDescriptorSet> descriptorSetList = new List<WriteDescriptorSet>();
            fixed (DescriptorBufferInfo* gameObjectProperties = MemoryManager.GetGameObjectPropertiesBuffer().ToArray())
            fixed (DescriptorImageInfo* textureProperties = MemoryManager.GetTexturePropertiesBuffer().ToArray())
            {
                for (uint x = 0; x < VulkanRenderer.swapChain.ImageCount; x++)
                {
                    WriteDescriptorSet descriptorSetWrite = new WriteDescriptorSet
                    {
                        SType = StructureType.WriteDescriptorSet,
                        DstSet = descriptorset,
                        DstBinding = 0,
                        DstArrayElement = 0,
                        DescriptorCount = (uint)MemoryManager.GetGameObjectPropertiesBuffer().ToArray().Length,
                        DescriptorType = DescriptorType.StorageBuffer,
                        PImageInfo = null,
                        PBufferInfo = gameObjectProperties,
                        PTexelBufferView = null
                    };

                    WriteDescriptorSet descriptorSetWrite2 = new WriteDescriptorSet
                    {
                        SType = StructureType.WriteDescriptorSet,
                        DstSet = descriptorset,
                        DstBinding = 1,
                        DstArrayElement = 0,
                        DescriptorCount = (uint)MemoryManager.GetTexturePropertiesBuffer().ToArray().Length,
                        DescriptorType = DescriptorType.CombinedImageSampler,
                        PImageInfo = textureProperties,
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
        }

        public PipelineLayout CreatePipelineLayout()
        {
            PipelineLayout pipelineLayout = new PipelineLayout();

            PushConstantRange pushConstantRange = new PushConstantRange()
            {
                StageFlags = ShaderStageFlags.VertexBit | ShaderStageFlags.FragmentBit,
                Offset = 0,
                Size = (uint)sizeof(SceneDataBuffer)
            };

            DescriptorSetLayout descriptorSetLayoutPtr = descriptorSetLayout;
            PipelineLayoutCreateInfo pipelineLayoutInfo = new
            (
                setLayoutCount: 1,
                pSetLayouts: &descriptorSetLayoutPtr,
                pushConstantRangeCount: 1,
                pPushConstantRanges: &pushConstantRange
            );
            vk.CreatePipelineLayout(VulkanRenderer.device, &pipelineLayoutInfo, null, out PipelineLayout pipelinelayout);
            return pipelinelayout;

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

            var descSet = descriptorset;
            var commandInfo = new CommandBufferBeginInfo(flags: 0);


            vk.BeginCommandBuffer(commandBuffer, &commandInfo);
            vk.CmdBeginRenderPass(commandBuffer, &renderPassInfo, SubpassContents.Inline);
            vk.CmdSetViewport(commandBuffer, 0, 1, &viewport);
            vk.CmdSetScissor(commandBuffer, 0, 1, &scissor);
            foreach (var obj in gameObjectList)
            {
                obj.Draw(commandBuffer, shaderpipeline, shaderpipelineLayout, descSet, sceneDataBuffer);
            }
            vk.CmdEndRenderPass(commandBuffer);
            vk.EndCommandBuffer(commandBuffer);

            return commandBuffer;
        }
    }
}