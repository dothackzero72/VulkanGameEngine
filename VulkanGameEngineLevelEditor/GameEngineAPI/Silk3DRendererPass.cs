using Silk.NET.Core.Native;
using Silk.NET.SDL;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.Tests;
using VulkanGameEngineLevelEditor.Vulkan;
using Vertex = VulkanGameEngineLevelEditor.Tests.Vertex;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class Silk3DRendererPass : SilkRenderPassBase
    {
        Vk vk = Vk.GetApi();
        public Silk3DRendererPass()  : base()
        {

            //(DescriptorType.UniformBuffer, 1, ShaderStageFlags.ShaderStageVertexBit),
            //    (DescriptorType.CombinedImageSampler, 1, ShaderStageFlags.ShaderStageFragmentBit)});
        }

        public void Create3dRenderPass()
        {
           CreateRenderPass();
        }

        public RenderPass CreateRenderPass()
        {
            RenderPass tempRenderPass = new RenderPass();
            List<AttachmentDescription> attachmentDescriptionList = new List<AttachmentDescription>()
            {
                new AttachmentDescription()
                {
                    Format = Format.B8G8R8A8Unorm,
                    Samples = SampleCountFlags.SampleCount1Bit,
                    LoadOp = AttachmentLoadOp.Load,
                    StoreOp = AttachmentStoreOp.Store,
                    StencilLoadOp = AttachmentLoadOp.DontCare,
                    StencilStoreOp = AttachmentStoreOp.DontCare,
                    InitialLayout = ImageLayout.Undefined,
                    FinalLayout = ImageLayout.PresentSrcKhr,
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
                Layout = ImageLayout.DepthAttachmentOptimal
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

                vk.CreateRenderPass(SilkVulkanRenderer.device, &renderPassCreateInfo, null, &tempRenderPass);
                renderPass = tempRenderPass;
            }

            return renderPass;
        }

        public DescriptorSetLayout CreateDescriptorSetLayout()
        {
            List<DescriptorSetLayoutBinding> LayoutBindingList = new List<DescriptorSetLayoutBinding>()
                {
                    new DescriptorSetLayoutBinding()
                    {
                        Binding = 0,
                        DescriptorType = DescriptorType.UniformBuffer,
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
                VKConst.vulkan.CreateDescriptorSetLayout(SilkVulkanRenderer.device, &layoutInfo, null, out DescriptorSetLayout descriptorSetLayout);

                return descriptorSetLayout;
            }
        }

        public DescriptorSet CreateDescriptorSets(DescriptorSet descriptorSets, DescriptorSetLayout descriptorSetLayout, DescriptorPool descPool)
        {

            DescriptorSetLayout* layouts = stackalloc DescriptorSetLayout[SilkVulkanRenderer.MAX_FRAMES_IN_FLIGHT];

            for (int i = 0; i < SilkVulkanRenderer.MAX_FRAMES_IN_FLIGHT; i++)
            {
                layouts[i] = descriptorSetLayout;
            }

            DescriptorSetAllocateInfo allocInfo = new
            (
                descriptorPool: descPool,
                descriptorSetCount: SilkVulkanRenderer.MAX_FRAMES_IN_FLIGHT,
                pSetLayouts: layouts
            );
            SilkVulkanRenderer.vulkan.AllocateDescriptorSets(SilkVulkanRenderer.device, &allocInfo, out DescriptorSet descriptorSet);
            descriptorSets = descriptorSet;
            return descriptorSets;
        }

        public Framebuffer[] CreateFramebuffer(DepthTexture depthTexture)
        {

            Framebuffer[] frameBufferList = new Framebuffer[SilkVulkanRenderer.swapChain.ImageCount];
            for (int x = 0; x < SilkVulkanRenderer.swapChain.ImageCount; x++)
            {
                List<ImageView> TextureAttachmentList = new List<ImageView>();
                TextureAttachmentList.Add(SilkVulkanRenderer.swapChain.imageViews[x]);
                TextureAttachmentList.Add(depthTexture.View);

                fixed (ImageView* imageViewPtr = TextureAttachmentList.ToArray())
                {
                    FramebufferCreateInfo framebufferInfo = new FramebufferCreateInfo()
                    {
                        SType = StructureType.FramebufferCreateInfo,
                        RenderPass = renderPass,
                        AttachmentCount = TextureAttachmentList.UCount(),
                        PAttachments = imageViewPtr,
                        Width = SilkVulkanRenderer.swapChain.swapchainExtent.Width,
                        Height = SilkVulkanRenderer.swapChain.swapchainExtent.Height,
                        Layers = 1
                    };

                    Framebuffer frameBuffer = FrameBufferList[x];
                    SilkVulkanRenderer.vulkan.CreateFramebuffer(SilkVulkanRenderer.device, &framebufferInfo, null, &frameBuffer);
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
                    Type = DescriptorType.UniformBuffer,
                    DescriptorCount = SilkVulkanRenderer.MAX_FRAMES_IN_FLIGHT
                };
                new DescriptorPoolSize
                {
                    Type = DescriptorType.CombinedImageSampler,
                    DescriptorCount = SilkVulkanRenderer.MAX_FRAMES_IN_FLIGHT
                };
            };

            fixed (DescriptorPoolSize* ptr = DescriptorPoolBinding.ToArray())
            {
                DescriptorPoolCreateInfo poolInfo = new DescriptorPoolCreateInfo()
                {
                    SType = StructureType.DescriptorPoolCreateInfo,
                    MaxSets = SilkVulkanRenderer.swapChain.ImageCount,
                    PoolSizeCount = (uint)DescriptorPoolBinding.Count,
                    PPoolSizes = ptr
                };
                SilkVulkanRenderer.vulkan.CreateDescriptorPool(SilkVulkanRenderer.device, in poolInfo, null, out descriptorPool);
            }

            descriptorpool = descriptorPool;
            return descriptorPool;
        }


        public DescriptorSet allocateDescriptorSets(DescriptorPool descriptorPool, DescriptorSetLayout descriptorSetLayout)
        {
            DescriptorSetLayout* layouts = stackalloc DescriptorSetLayout[SilkVulkanRenderer.MAX_FRAMES_IN_FLIGHT];

            for (int i = 0; i < SilkVulkanRenderer.MAX_FRAMES_IN_FLIGHT; i++)
            {
                layouts[i] = descriptorSetLayout;
            }

            DescriptorSetAllocateInfo allocInfo = new
            (
                descriptorPool: descriptorPool,
                descriptorSetCount: SilkVulkanRenderer.MAX_FRAMES_IN_FLIGHT,
                pSetLayouts: layouts
            );
            SilkVulkanRenderer.vulkan.AllocateDescriptorSets(SilkVulkanRenderer.device, &allocInfo, out DescriptorSet descriptorSet);
            return descriptorSet;
        }

        public void UpdateDescriptorSet(DescriptorSet descriptorSets, Texture texture, Silk.NET.Vulkan.Buffer ubobuffers)
        {
            var colorDescriptorImage = new DescriptorImageInfo
            {
                Sampler = texture.Sampler,
                ImageView = texture.View,
                ImageLayout = Silk.NET.Vulkan.ImageLayout.ReadOnlyOptimal
            };


            DescriptorBufferInfo uniformBuffer = new DescriptorBufferInfo
            {
                Buffer = ubobuffers,
                Offset = 0,
                Range = Vk.WholeSize
            };

            List<WriteDescriptorSet> descriptorSetList = new List<WriteDescriptorSet>();
            for (uint x = 0; x < SilkVulkanRenderer.swapChain.ImageCount; x++)
            {
                WriteDescriptorSet descriptorSetWrite = new WriteDescriptorSet
                {
                    SType = StructureType.WriteDescriptorSet,
                    DstSet = descriptorSets,
                    DstBinding = 0,
                    DstArrayElement = 0,
                    DescriptorCount = 1,
                    DescriptorType = DescriptorType.UniformBuffer,
                    PImageInfo = null,
                    PBufferInfo = &uniformBuffer,
                    PTexelBufferView = null
                };

                WriteDescriptorSet descriptorSetWrite2 = new WriteDescriptorSet
                {
                    SType = StructureType.WriteDescriptorSet,
                    DstSet = descriptorSets,
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
                SilkVulkanRenderer.vulkan.UpdateDescriptorSets(SilkVulkanRenderer.device, (uint)descriptorSetList.UCount(), ptr, 0, null);
            }
        }

        public PipelineLayout CreatePipelineLayout(DescriptorSetLayout layout)
        {
            PipelineLayout pipelineLayout = new PipelineLayout();

            DescriptorSetLayout descriptorSetLayoutPtr = layout;
            PipelineLayoutCreateInfo pipelineLayoutInfo = new
            (
                setLayoutCount: 1,
                pSetLayouts: &descriptorSetLayoutPtr,
                pushConstantRangeCount: 0,
                pPushConstantRanges: null
            );
            vk.CreatePipelineLayout(SilkVulkanRenderer.device, &pipelineLayoutInfo, null, out PipelineLayout pipelinelayout);
            return pipelinelayout;

        }

        public Pipeline CreateGraphicsPipeline(PipelineLayout layout)
        {
            List<PipelineShaderStageCreateInfo> shadermoduleList  = new List<PipelineShaderStageCreateInfo>()
            {
                SilkVulkanRenderer.CreateShader("C:/Users/dotha/Documents/GitHub/VulkanGameEngine/Shaders/vertex_shader.spv",  ShaderStageFlags.VertexBit),
                SilkVulkanRenderer.CreateShader("C:/Users/dotha/Documents/GitHub/VulkanGameEngine/Shaders/fragment_shader.spv", ShaderStageFlags.FragmentBit)
            };

            PipelineVertexInputStateCreateInfo pipelineVertexInputStateCreateInfo = new(flags: 0);
            VertexInputBindingDescription vertexInputBindingDescription = new(0, Vertex.GetStride());

            VertexInputAttributeDescription* vertexInputAttributeDescriptions = null;

            if (Vertex.GetStride() > 0)
            {
                vertexInputAttributeDescriptions = (VertexInputAttributeDescription*)Mem.AllocArray<VertexInputAttributeDescription>(Vertex.GetAttributeFormatsAndOffsets().Length);
                for (uint i = 0; i < Vertex.GetAttributeFormatsAndOffsets().Length; i++)
                {
                    vertexInputAttributeDescriptions[i] = new
                    (
                        location: i,
                        binding: 0,
                        format: Vertex.GetAttributeFormatsAndOffsets()[i].Item1,
                        offset: Vertex.GetAttributeFormatsAndOffsets()[i].Item2
                    );
                }

                pipelineVertexInputStateCreateInfo.VertexBindingDescriptionCount = 1;
                pipelineVertexInputStateCreateInfo.PVertexBindingDescriptions = &vertexInputBindingDescription;

                pipelineVertexInputStateCreateInfo.VertexAttributeDescriptionCount = (uint)Vertex.GetAttributeFormatsAndOffsets().Length;
                pipelineVertexInputStateCreateInfo.PVertexAttributeDescriptions = vertexInputAttributeDescriptions;
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

            PipelineColorBlendAttachmentState pipelineColorBlendAttachmentState = new(
                false,
                Silk.NET.Vulkan.BlendFactor.Zero,
                Silk.NET.Vulkan.BlendFactor.Zero,
                BlendOp.Add,
                Silk.NET.Vulkan.BlendFactor.Zero,
                Silk.NET.Vulkan.BlendFactor.Zero,
                BlendOp.Add,
                colorComponentFlags);

            PipelineColorBlendStateCreateInfo pipelineColorBlendStateCreateInfo = new
            (
                logicOpEnable: false,
                logicOp: LogicOp.NoOp,
                attachmentCount: 1,
                pAttachments: &pipelineColorBlendAttachmentState
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

            fixed (PipelineShaderStageCreateInfo* shaderlist = shadermoduleList.ToArray())
            {
                GraphicsPipelineCreateInfo graphicsPipelineCreateInfo = new
                (
                    stageCount: 2,
                    pStages: shaderlist,
                    pVertexInputState: &pipelineVertexInputStateCreateInfo,
                    pInputAssemblyState: &pipelineInputAssemblyStateCreateInfo,
                    pViewportState: &pipelineViewportStateCreateInfo,
                    pRasterizationState: &pipelineRasterizationStateCreateInfo,
                    pMultisampleState: &pipelineMultisampleStateCreateInfo,
                    pDepthStencilState: &pipelineDepthStencilStateCreateInfo,
                    pColorBlendState: &pipelineColorBlendStateCreateInfo,
                    pDynamicState: &pipelineDynamicStateCreateInfo,
                    layout: layout,
                    renderPass: renderPass
                );


                vk.CreateGraphicsPipelines(SilkVulkanRenderer.device, new PipelineCache(null), 1, &graphicsPipelineCreateInfo, null, out Pipeline pipeline);
                return pipeline;
            }
            //SilkMarshal.Free((nint)pipelineShaderStageCreateInfos[0].PName);
            //SilkMarshal.Free((nint)pipelineShaderStageCreateInfos[1].PName);

          //  Mem.FreeArray(vertexInputAttributeDescriptions);

            //vertShaderModule.Dispose();
            // fragShaderModule.Dispose();

         
        }

    }
}
