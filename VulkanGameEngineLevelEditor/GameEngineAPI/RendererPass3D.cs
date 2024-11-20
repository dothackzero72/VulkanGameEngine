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
using ImageLayout = Silk.NET.Vulkan.ImageLayout;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class RendererPass3D : SilkRenderPassBase
    {
        Vk vk = Vk.GetApi();
        public DepthTexture depthTexture { get; set; }
        public Texture texture { get; set; }
        public Texture renderedTexture { get; set; }

        public RendererPass3D() : base()
        {
        }

        public void Create3dRenderPass()
        {
            depthTexture = new DepthTexture(new ivec2((int)VulkanRenderer.swapChain.swapchainExtent.Width, (int)VulkanRenderer.swapChain.swapchainExtent.Height));
            texture = new GameEngineAPI.Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\VulkanGameEngineLevelEditor\\bin\\Debug\\awesomeface.png", Format.R8G8B8A8Unorm, TextureTypeEnum.kType_DiffuseTextureMap);

            string jsonContent = File.ReadAllText(RenderPassEditorConsts.Default2DPipeline);
            RenderPipelineModel model = JsonConvert.DeserializeObject<RenderPipelineModel>(jsonContent);

            CreateRenderPass();
            LoadDescriptorSets(model);
            CreateGraphicsPipeline();
            CreateFramebuffer();
        }

        public RenderPass CreateRenderPass()
        {
            RenderPass tempRenderPass = new RenderPass();
            List<AttachmentDescription> attachmentDescriptionList = new List<AttachmentDescription>()
            {
                new AttachmentDescription()
                {
                    Format = (Format)44 ,
                    Samples = (SampleCountFlags)1,
                    LoadOp = (AttachmentLoadOp)1,
                    StoreOp = 0,
                    StencilLoadOp = (AttachmentLoadOp)2,
                    StencilStoreOp = (AttachmentStoreOp)1,
                    InitialLayout = 0,
                    FinalLayout = (ImageLayout)1000314000,
                },
                 new AttachmentDescription()
                    {
                        Format = Format.D32Sfloat,
                        Samples = SampleCountFlags.SampleCount1Bit,
                        LoadOp = AttachmentLoadOp.Clear,
                        StoreOp = AttachmentStoreOp.DontCare,
                        StencilLoadOp = AttachmentLoadOp.DontCare,
                        StencilStoreOp = AttachmentStoreOp.DontCare,
                        InitialLayout = ImageLayout.Undefined,
                        FinalLayout = ImageLayout.DepthStencilAttachmentOptimal,
                 }
            };

            List<AttachmentReference> colorRefsList = new List<AttachmentReference>()
                    {
                        new AttachmentReference
                        {
                            Attachment = 0,
                            Layout = ImageLayout.ColorAttachmentOptimal
                        }
                    };

            var depthReference = new AttachmentReference
            {
                Attachment = 1,
                Layout = ImageLayout.DepthStencilAttachmentOptimal
            };


            List<SubpassDependency> subpassDependencyList = new List<SubpassDependency>
                        {
                            new SubpassDependency
                            {
                                SrcSubpass = uint.MaxValue,
                                DstSubpass = 0,
                                SrcStageMask = PipelineStageFlags.ColorAttachmentOutputBit,
                                DstStageMask = PipelineStageFlags.ColorAttachmentOutputBit, // Changed to Early Fragment Tests
                                SrcAccessMask = 0,
                                DstAccessMask = AccessFlags.ColorAttachmentWriteBit, // Ensure this access mask is relevant to the chosen stage mask
                            }
                        };


            List<SubpassDescription> subpassDescriptionList = new List<SubpassDescription>();
            fixed (AttachmentReference* colorRefs = colorRefsList.ToArray())
            {
                subpassDescriptionList = new List<SubpassDescription>
                        {
                            new SubpassDescription
                            {
                                PipelineBindPoint = PipelineBindPoint.Graphics,
                                ColorAttachmentCount = (uint)colorRefsList.Count,
                                PColorAttachments = colorRefs,
                                PResolveAttachments = null,
                                PDepthStencilAttachment = &depthReference
                            }
                        };
            }

            fixed (AttachmentDescription* attachments = attachmentDescriptionList.ToArray())
            fixed (SubpassDescription* description = subpassDescriptionList.ToArray())
            fixed (SubpassDependency* dependency = subpassDependencyList.ToArray())
            {
                var renderPassCreateInfo = new RenderPassCreateInfo()
                {
                    SType = StructureType.RenderPassCreateInfo,
                    PNext = null,
                    Flags = 0,
                    AttachmentCount = (uint)attachmentDescriptionList.Count(),
                    PAttachments = attachments,
                    SubpassCount = (uint)subpassDescriptionList.Count(),
                    PSubpasses = description,
                    DependencyCount = (uint)subpassDependencyList.Count(),
                    PDependencies = dependency
                };

                vk.CreateRenderPass(VulkanRenderer.device, &renderPassCreateInfo, null, &tempRenderPass);
                renderPass = tempRenderPass;
            }

            return renderPass;
        }

        void CreateGraphicsPipeline()
        {
            PipelineShaderStageCreateInfo* shadermoduleList = stackalloc[]
            {
                VulkanRenderer.CreateShader("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Shaders\\Shader2DVert.spv",  ShaderStageFlags.VertexBit),
                VulkanRenderer.CreateShader("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Shaders\\Shader2DFrag.spv", ShaderStageFlags.FragmentBit)
            };
            shaderpipelineLayout = CreatePipelineLayout();


            PipelineVertexInputStateCreateInfo vertexInputInfo = new PipelineVertexInputStateCreateInfo();
            List<VertexInputBindingDescription> bindingDescriptionList = Vertex2D.GetBindingDescriptions();
            List<VertexInputAttributeDescription> AttributeDescriptions = Vertex2D.GetAttributeDescriptions();

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
                cullMode: CullModeFlags.None,
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
           attachmentCount: 2,  // Match this to the render pass
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

     

        private void LoadDescriptorSets(RenderPipelineModel model)
        {
            var meshProperties = MemoryManager.GetGameObjectPropertiesBuffer();
            var textures = MemoryManager.GetTexturePropertiesBuffer();

            //CreateDescriptorPool
            {
                List<DescriptorPoolSize> descriptorPoolSizeList = new List<DescriptorPoolSize>();
                foreach (var binding in model.PipelineDescriptorModelsList)
                {
                    switch (binding.BindingPropertiesList)
                    {
                        case DescriptorBindingPropertiesEnum.kMeshPropertiesDescriptor:
                            {
                                descriptorPoolSizeList.Add(new DescriptorPoolSize()
                                {
                                    Type = DescriptorType.StorageBuffer,
                                    DescriptorCount = meshProperties.UCount()
                                });
                                break;
                            }
                        case DescriptorBindingPropertiesEnum.kTextureDescriptor:
                            {
                                descriptorPoolSizeList.Add(new DescriptorPoolSize()
                                {
                                    Type = DescriptorType.CombinedImageSampler,
                                    DescriptorCount = textures.UCount()
                                });
                                break;
                            }
                        default:
                            {
                                throw new Exception($"{binding} case hasn't been handled yet");
                            }
                    }
                }

                fixed (DescriptorPoolSize* descriptorPoolSize = descriptorPoolSizeList.ToArray())
                {
                    DescriptorPoolCreateInfo poolCreateInfo = new DescriptorPoolCreateInfo()
                    {
                        SType = StructureType.DescriptorPoolCreateInfo,
                        MaxSets = 500,
                        PPoolSizes = descriptorPoolSize,
                        PoolSizeCount = descriptorPoolSizeList.UCount(),
                        Flags = 0,
                        PNext = null
                    };
                    vk.CreateDescriptorPool(VulkanRenderer.device, in poolCreateInfo, null, out DescriptorPool descriptorPoolPtr);
                    descriptorpool = descriptorPoolPtr;
                }
            }

            //CreateDescriptorSetLayout
            {
                List<DescriptorSetLayoutBinding> descriptorSetLayoutBindingList = new List<DescriptorSetLayoutBinding>();
                foreach (var binding in model.PipelineDescriptorModelsList)
                {
                    switch (binding.BindingPropertiesList)
                    {
                        case DescriptorBindingPropertiesEnum.kMeshPropertiesDescriptor:
                            {
                                descriptorSetLayoutBindingList.Add(new DescriptorSetLayoutBinding()
                                {
                                    Binding = binding.BindingNumber,
                                    DescriptorCount = meshProperties.UCount(),
                                    DescriptorType = DescriptorType.StorageBuffer,
                                    PImmutableSamplers = null,
                                    StageFlags = ShaderStageFlags.FragmentBit | ShaderStageFlags.VertexBit
                                });
                                break;
                            }
                        case DescriptorBindingPropertiesEnum.kTextureDescriptor:
                            {
                                descriptorSetLayoutBindingList.Add(new DescriptorSetLayoutBinding()
                                {
                                    Binding = binding.BindingNumber,
                                    DescriptorCount = textures.UCount(),
                                    DescriptorType = DescriptorType.CombinedImageSampler,
                                    PImmutableSamplers = null,
                                    StageFlags = ShaderStageFlags.FragmentBit | ShaderStageFlags.VertexBit
                                });
                                break;
                            }
                        default:
                            {
                                throw new Exception($"{binding} case hasn't been handled yet");
                            }
                    }
                }

                fixed (DescriptorSetLayoutBinding* descriptorSetLayouts = descriptorSetLayoutBindingList.ToArray())
                {
                    DescriptorSetLayoutCreateInfo descriptorSetLayoutCreateInfo = new DescriptorSetLayoutCreateInfo()
                    {
                        SType = StructureType.DescriptorSetLayoutCreateInfo,
                        BindingCount = descriptorSetLayoutBindingList.UCount(),
                        PBindings = descriptorSetLayouts,
                        Flags = 0,
                        PNext = null
                    };
                    vk.CreateDescriptorSetLayout(VulkanRenderer.device, &descriptorSetLayoutCreateInfo, null, out DescriptorSetLayout descriptorsetLayoutPtr);
                    descriptorSetLayoutList.Add(descriptorsetLayoutPtr);
                }
            }

            //AllocateDescriptorSets
            {
                DescriptorSetLayout* layouts = stackalloc DescriptorSetLayout[VulkanRenderer.MAX_FRAMES_IN_FLIGHT];

                for (int x = 0; x < VulkanRenderer.MAX_FRAMES_IN_FLIGHT; x++)
                {
                    layouts[x] = descriptorSetLayoutList[0];
                }

                DescriptorSetAllocateInfo allocInfo = new
                (
                    descriptorPool: descriptorpool,
                    descriptorSetCount: VulkanRenderer.MAX_FRAMES_IN_FLIGHT,
                    pSetLayouts: layouts
                );
                vk.AllocateDescriptorSets(VulkanRenderer.device, &allocInfo, out DescriptorSet descriptorSetPtr);
                descriptorset = descriptorSetPtr;
            }

            //UpdateDescriptorSets
            {
                List<WriteDescriptorSet> descriptorSetList = new List<WriteDescriptorSet>();
                for (uint x = 0; x < VulkanRenderer.swapChain.ImageCount; x++)
                {
                    foreach (var binding in model.PipelineDescriptorModelsList)
                    {
                        switch (binding.BindingPropertiesList)
                        {
                            case DescriptorBindingPropertiesEnum.kMeshPropertiesDescriptor:
                                {
                                    fixed (DescriptorBufferInfo* meshInfo = meshProperties.ToArray())
                                    {
                                        descriptorSetList.Add(new WriteDescriptorSet()
                                        {
                                            SType = StructureType.WriteDescriptorSet,
                                            DescriptorCount = meshProperties.UCount(),
                                            DescriptorType = DescriptorType.StorageBuffer,
                                            DstBinding = binding.BindingNumber,
                                            DstArrayElement = 0,
                                            DstSet = descriptorset,
                                            PBufferInfo = meshInfo,
                                        });
                                    }
                                    break;
                                }
                            case DescriptorBindingPropertiesEnum.kTextureDescriptor:
                                {
                                    fixed (DescriptorImageInfo* texturesPtr = textures.ToArray())
                                    {
                                        descriptorSetList.Add(new WriteDescriptorSet()
                                        {
                                            SType = StructureType.WriteDescriptorSet,
                                            DescriptorCount = meshProperties.UCount(),
                                            DescriptorType = DescriptorType.CombinedImageSampler,
                                            DstBinding = binding.BindingNumber,
                                            DstArrayElement = 0,
                                            DstSet = descriptorset,
                                            PImageInfo = texturesPtr
                                        });
                                    }
                                    break;
                                }
                            default:
                                {
                                    throw new Exception($"{binding} case hasn't been handled yet");
                                }
                        }
                    }
                    fixed (WriteDescriptorSet* writeDescriptorSet = descriptorSetList.ToArray())
                    {
                        vk.UpdateDescriptorSets(VulkanRenderer.device, descriptorSetList.UCount(), writeDescriptorSet, 0, null);
                    }
                }
            }
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
            List<DescriptorPoolSize> DescriptorPoolBinding = new List<DescriptorPoolSize>()
            {
                new DescriptorPoolSize
                {
                    Type = DescriptorType.UniformBuffer,
                    DescriptorCount = VulkanRenderer.MAX_FRAMES_IN_FLIGHT
                },
                new DescriptorPoolSize
                {
                    Type = DescriptorType.CombinedImageSampler,
                    DescriptorCount = VulkanRenderer.MAX_FRAMES_IN_FLIGHT
                }
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

        public void UpdateDescriptorSet(DescriptorSet descriptorSets)
        {
            var meshProperties = MemoryManager.GetGameObjectPropertiesBuffer();
            var textures = MemoryManager.GetTexturePropertiesBuffer();



            List<WriteDescriptorSet> descriptorSetList = new List<WriteDescriptorSet>();
            for (uint x = 0; x < VulkanRenderer.swapChain.ImageCount; x++)
            {
                WriteDescriptorSet descriptorSetWrite;
                fixed (DescriptorBufferInfo* meshInfo = meshProperties.ToArray())
                {
                    descriptorSetWrite = new WriteDescriptorSet
                    {
                        SType = StructureType.WriteDescriptorSet,
                        DstSet = descriptorset,
                        DstBinding = 0,
                        DstArrayElement = 0,
                        DescriptorCount = 1,
                        DescriptorType = DescriptorType.UniformBuffer,
                        PImageInfo = null,
                        PBufferInfo = meshInfo,
                        PTexelBufferView = null
                    };
                }

                WriteDescriptorSet descriptorSetWrite2;
                fixed (DescriptorImageInfo* texturesPtr = textures.ToArray())
                {
                    descriptorSetWrite2 = new WriteDescriptorSet
                    {
                        SType = StructureType.WriteDescriptorSet,
                        DstSet = descriptorset,
                        DstBinding = 1,
                        DstArrayElement = 0,
                        DescriptorCount = 1,
                        DescriptorType = DescriptorType.CombinedImageSampler,
                        PImageInfo = texturesPtr,
                        PBufferInfo = null,
                        PTexelBufferView = null
                    };
                }

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

            fixed (DescriptorSetLayout* descriptorSet234 = descriptorSetLayoutList.ToArray())
            {
                PipelineLayoutCreateInfo pipelineLayoutInfo = new
                (
                    setLayoutCount: descriptorSetLayoutList.UCount(),
                    pSetLayouts: descriptorSet234,
                    pushConstantRangeCount: 0,
                    pPushConstantRanges: null
                );
                vk.CreatePipelineLayout(VulkanRenderer.device, &pipelineLayoutInfo, null, out PipelineLayout pipelinelayout);
                return pipelinelayout;
            }

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

            var descSet = descriptorset;
            var commandInfo = new CommandBufferBeginInfo(flags: 0);

            vk.BeginCommandBuffer(commandBuffer, &commandInfo);
            vk.CmdBeginRenderPass(commandBuffer, &renderPassInfo, SubpassContents.Inline);
            vk.CmdSetViewport(commandBuffer, 0, 1, &viewport);
            vk.CmdSetScissor(commandBuffer, 0, 1, &scissor);
            vk.CmdBindPipeline(commandBuffer, PipelineBindPoint.Graphics, shaderpipeline);
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