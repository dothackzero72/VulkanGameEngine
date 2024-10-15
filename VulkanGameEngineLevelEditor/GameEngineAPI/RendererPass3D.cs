using GlmSharp;
using Silk.NET.Core.Native;
using Silk.NET.Maths;
using Silk.NET.SDL;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class RendererPass3D : SilkRenderPassBase
    {
        Vk vk = Vk.GetApi();
        public DepthTexture depthTexture { get; set; }
        public Texture texture { get; set; }
        public Texture renderedTexture { get; set; }
        public VulkanBuffer<Vertex3D> vertexBuffer { get; set; }
        public VulkanBuffer<ushort> indexBuffer { get; set; }
        public VulkanBuffer<UniformBufferObject> uniformBuffers { get; set; }

        UniformBufferObject ubo;

        readonly Vertex3D[] vertices = new Vertex3D[]
{
            new Vertex3D(new (-0.5f, -0.5f, 0.0f), new (1.0f, 0.0f, 0.0f), new (1.0f, 0.0f)),
            new Vertex3D(new (0.5f, -0.5f, 0.0f), new (0.0f, 1.0f, 0.0f), new (0.0f, 0.0f)),
            new Vertex3D(new (0.5f, 0.5f, 0.0f), new (0.0f, 0.0f, 1.0f), new (0.0f, 1.0f)),
            new Vertex3D(new (-0.5f, 0.5f, 0.0f), new (1.0f, 1.0f, 1.0f), new (1.0f, 1.0f)),

            new Vertex3D(new (-0.5f, -0.5f, -0.5f), new (1.0f, 0.0f, 0.0f), new (1.0f, 0.0f)),
            new Vertex3D(new (0.5f, -0.5f, -0.5f), new (0.0f, 1.0f, 0.0f), new (0.0f, 0.0f)),
            new Vertex3D(new (0.5f, 0.5f, -0.5f), new (0.0f, 0.0f, 1.0f), new (0.0f, 1.0f)),
            new Vertex3D(new (-0.5f, 0.5f, -0.5f), new (1.0f, 1.0f, 1.0f), new (1.0f, 1.0f))
};

        readonly ushort[] indices = new ushort[]
        {
            0, 1, 2, 2, 3, 0,
            4, 5, 6, 6, 7, 4
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct UniformBufferObject
        {
            public Matrix4X4<float> model;
            public Matrix4X4<float> view;
            public Matrix4X4<float> proj;

        }

        public RendererPass3D() : base()
        {
        }

        public void Create3dRenderPass()
        {
            depthTexture = new DepthTexture(new ivec2((int)VulkanRenderer.swapChain.swapchainExtent.Width, (int)VulkanRenderer.swapChain.swapchainExtent.Height));
            texture = new GameEngineAPI.Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\VulkanGameEngineLevelEditor\\bin\\Debug\\awesomeface.png", Format.R8G8B8A8Unorm, TextureTypeEnum.kType_DiffuseTextureMap);

            GCHandle vhandle = GCHandle.Alloc(vertices, GCHandleType.Pinned);
            IntPtr vpointer = vhandle.AddrOfPinnedObject();
            vertexBuffer = new VulkanBuffer<Vertex3D>((void*)vpointer, (uint)vertices.Count(), BufferUsageFlags.BufferUsageTransferSrcBit | BufferUsageFlags.BufferUsageTransferDstBit | BufferUsageFlags.VertexBufferBit, MemoryPropertyFlags.MemoryPropertyHostVisibleBit | MemoryPropertyFlags.MemoryPropertyHostCoherentBit, true);

            GCHandle fhandle = GCHandle.Alloc(indices, GCHandleType.Pinned);
            IntPtr fpointer = fhandle.AddrOfPinnedObject();
            indexBuffer = new VulkanBuffer<ushort>((void*)fpointer, (uint)indices.Count(), BufferUsageFlags.BufferUsageTransferSrcBit | BufferUsageFlags.BufferUsageTransferDstBit | BufferUsageFlags.IndexBufferBit, MemoryPropertyFlags.MemoryPropertyHostVisibleBit | MemoryPropertyFlags.MemoryPropertyHostCoherentBit, true);


            CreateRenderPass();
            CreateDescriptorSetLayout();
            CreateGraphicsPipeline();
            CreateFramebuffer();
            CreateUniformBuffers();
            CreateDescriptorPoolBinding();
            allocateDescriptorSets(descriptorpool);
            UpdateDescriptorSet(descriptorset);
        }

        public RenderPass CreateRenderPass()
        {
            RenderPass tempRenderPass = new RenderPass();
            List<AttachmentDescription> attachmentDescriptionList = new List<AttachmentDescription>()
            {
                new AttachmentDescription()
                {
                    Format = Format.R8G8B8A8Unorm ,
                    Samples = SampleCountFlags.SampleCount1Bit,
                    LoadOp = AttachmentLoadOp.Clear,
                    StoreOp = AttachmentStoreOp.Store,
                    StencilLoadOp = AttachmentLoadOp.DontCare,
                    StencilStoreOp = AttachmentStoreOp.DontCare,
                    InitialLayout = ImageLayout.ColorAttachmentOptimal,
                    FinalLayout = ImageLayout.ColorAttachmentOptimal,
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
                VulkanRenderer.CreateShader("vertshader.spv",  ShaderStageFlags.VertexBit),
                VulkanRenderer.CreateShader("fragshader.spv", ShaderStageFlags.FragmentBit)
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
                    Type = DescriptorType.UniformBuffer,
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
            var colorDescriptorImage = new DescriptorImageInfo
            {
                Sampler = texture.Sampler,
                ImageView = texture.View,
                ImageLayout = Silk.NET.Vulkan.ImageLayout.ReadOnlyOptimal
            };


            DescriptorBufferInfo uniformBuffer = new DescriptorBufferInfo
            {
                Buffer = uniformBuffers.Buffer,
                Offset = 0,
                Range = Vk.WholeSize
            };

            List<WriteDescriptorSet> descriptorSetList = new List<WriteDescriptorSet>();
            for (uint x = 0; x < VulkanRenderer.swapChain.ImageCount; x++)
            {
                WriteDescriptorSet descriptorSetWrite = new WriteDescriptorSet
                {
                    SType = StructureType.WriteDescriptorSet,
                    DstSet = descriptorset,
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
            vk.CreatePipelineLayout(VulkanRenderer.device, &pipelineLayoutInfo, null, out PipelineLayout pipelinelayout);
            return pipelinelayout;

        }

        void CreateUniformBuffers()
        {
            GCHandle uhandle = GCHandle.Alloc(ubo, GCHandleType.Pinned);
            IntPtr upointer = uhandle.AddrOfPinnedObject();
            uniformBuffers = new VulkanBuffer<UniformBufferObject>((void*)upointer, 1, BufferUsageFlags.UniformBufferBit | BufferUsageFlags.BufferUsageTransferSrcBit | BufferUsageFlags.BufferUsageTransferDstBit, MemoryPropertyFlags.MemoryPropertyHostVisibleBit | MemoryPropertyFlags.MemoryPropertyHostCoherentBit, true);
        }

        public void UpdateUniformBuffer(long startTime)
        {
            float secondsPassed = (float)TimeSpan.FromTicks(DateTime.Now.Ticks - startTime).TotalSeconds;

            ubo.model = Matrix4X4.CreateFromAxisAngle(
                new Vector3D<float>(0, 0, 1),
                secondsPassed * Scalar.DegreesToRadians(90.0f));

            ubo.view = Matrix4X4.CreateLookAt(
                new Vector3D<float>(2.0f, 2.0f, 2.0f),
                new Vector3D<float>(0.0f, 0.0f, 0.0f),
                new Vector3D<float>(0.0f, 0.0f, -0.1f));

            ubo.proj = Matrix4X4.CreatePerspectiveFieldOfView(
                Scalar.DegreesToRadians(45.0f),
                VulkanRenderer.swapChain.swapchainExtent.Width / (float)VulkanRenderer.swapChain.swapchainExtent.Height,
                0.1f,
                10.0f);

            ubo.proj.M11 *= -1;
            uint dataSize = (uint)Marshal.SizeOf(ubo);
            void* dataPtr = Unsafe.AsPointer(ref ubo);

            uniformBuffers.UpdateBufferData(dataPtr);
        }

        public CommandBuffer Draw()
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
            var vertexbuffer = vertexBuffer.Buffer;
            var commandInfo = new CommandBufferBeginInfo(flags: 0);


            vk.BeginCommandBuffer(commandBuffer, &commandInfo);
            vk.CmdBeginRenderPass(commandBuffer, &renderPassInfo, SubpassContents.Inline);
            vk.CmdBindPipeline(commandBuffer, PipelineBindPoint.Graphics, shaderpipeline);
            vk.CmdBindVertexBuffers(commandBuffer, 0, 1, &vertexbuffer, 0);
            vk.CmdBindIndexBuffer(commandBuffer, indexBuffer.Buffer, 0, IndexType.Uint16);
            vk.CmdBindDescriptorSets(commandBuffer, PipelineBindPoint.Graphics, shaderpipelineLayout, 0, 1, descSet, 0, null);
            vk.CmdSetViewport(commandBuffer, 0, 1, &viewport);
            vk.CmdSetScissor(commandBuffer, 0, 1, &scissor);
            vk.CmdDrawIndexed(commandBuffer, (uint)indices.Length, 1, 0, 0, 0);
            vk.CmdEndRenderPass(commandBuffer);
            vk.EndCommandBuffer(commandBuffer);

            return commandBuffer;
        }
    }
}