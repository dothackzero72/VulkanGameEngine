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
    public unsafe class Silk3DRendererPass : SilkRenderPassBase
    {
        Vk vk = Vk.GetApi();
        public DepthTexture depthTexture;
        public Texture texture;
        public VulkanBuffer<Vertex3D> vertexBuffer;
        public VulkanBuffer<ushort> indexBuffer;
        public VulkanBuffer<UniformBufferObject> uniformBuffers;
        public Texture renderedColorTexture;
        private readonly Object BufferLock = new Object();

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

        public Silk3DRendererPass()  : base()
        {

            //(DescriptorType.UniformBuffer, 1, ShaderStageFlags.ShaderStageVertexBit),
            //    (DescriptorType.CombinedImageSampler, 1, ShaderStageFlags.ShaderStageFragmentBit)});
        }

        public void Create3dRenderPass()
        {
            renderedColorTexture = new Texture(RenderPassResolution);
            depthTexture = new DepthTexture(new ivec2((int)SilkVulkanRenderer.swapChain.swapchainExtent.Width, (int)SilkVulkanRenderer.swapChain.swapchainExtent.Height));
            texture = new GameEngineAPI.Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\VulkanGameEngineLevelEditor\\bin\\Debug\\awesomeface.png", Format.R8G8B8A8Srgb, TextureTypeEnum.kType_DiffuseTextureMap);

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

            for (int x = 0; x < SilkVulkanRenderer.swapChain.ImageCount; x++)
            {
                var commandBuffer = SilkVulkanRenderer.BeginSingleUseCommandBuffer();
                TransitionImageLayout(renderedColorTexture.Image, ImageLayout.Undefined, ImageLayout.ColorAttachmentOptimal, commandBuffer);
                TransitionImageLayout(renderedColorTexture.Image, ImageLayout.ColorAttachmentOptimal, ImageLayout.PresentSrcKhr, commandBuffer);
                SilkVulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);
            }

        }

        public RenderPass CreateRenderPass()
        {
            RenderPass tempRenderPass = new RenderPass();
            List<AttachmentDescription> attachmentDescriptionList = new List<AttachmentDescription>()
            {
                new AttachmentDescription()
                {
                    Format = Format.R8G8B8A8Unorm,
                    Samples = SampleCountFlags.SampleCount1Bit,
                    LoadOp = AttachmentLoadOp.Clear,
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
                        StoreOp = AttachmentStoreOp.Store,
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

        void CreateGraphicsPipeline()
        {
            PipelineShaderStageCreateInfo* shadermoduleList = stackalloc[]
            {
                SilkVulkanRenderer.CreateShader("vertshader.spv",  ShaderStageFlags.VertexBit),
                SilkVulkanRenderer.CreateShader("fragshader.spv", ShaderStageFlags.FragmentBit)
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

            vk.CreateGraphicsPipelines(SilkVulkanRenderer.device, new PipelineCache(null), 1, &graphicsPipelineCreateInfo, null, out Pipeline pipeline);
            shaderpipeline = pipeline;

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
                VKConst.vulkan.CreateDescriptorSetLayout(SilkVulkanRenderer.device, &layoutInfo, null, out DescriptorSetLayout descriptorsetLayout);
                descriptorSetLayout = descriptorsetLayout;
                return descriptorSetLayout;
            }
        }

        public DescriptorSet CreateDescriptorSets(DescriptorSet descriptorSets, DescriptorPool descPool)
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

        public Framebuffer[] CreateFramebuffer()
        {

            Framebuffer[] frameBufferList = new Framebuffer[SilkVulkanRenderer.swapChain.ImageCount];
            for (int x = 0; x < SilkVulkanRenderer.swapChain.ImageCount; x++)
            {
                List<ImageView> TextureAttachmentList = new List<ImageView>();
                TextureAttachmentList.Add(renderedColorTexture.View);
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


        public DescriptorSet allocateDescriptorSets(DescriptorPool descriptorPool354)
        {
            DescriptorSetLayout* layouts = stackalloc DescriptorSetLayout[SilkVulkanRenderer.MAX_FRAMES_IN_FLIGHT];

            for (int i = 0; i < SilkVulkanRenderer.MAX_FRAMES_IN_FLIGHT; i++)
            {
                layouts[i] = descriptorSetLayout;
            }

            DescriptorSetAllocateInfo allocInfo = new
            (
                descriptorPool: descriptorpool,
                descriptorSetCount: SilkVulkanRenderer.MAX_FRAMES_IN_FLIGHT,
                pSetLayouts: layouts
            );
            SilkVulkanRenderer.vulkan.AllocateDescriptorSets(SilkVulkanRenderer.device, &allocInfo, out DescriptorSet descriptorSet);
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
            for (uint x = 0; x < SilkVulkanRenderer.swapChain.ImageCount; x++)
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
                SilkVulkanRenderer.vulkan.UpdateDescriptorSets(SilkVulkanRenderer.device, (uint)descriptorSetList.UCount(), ptr, 0, null);
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
            vk.CreatePipelineLayout(SilkVulkanRenderer.device, &pipelineLayoutInfo, null, out PipelineLayout pipelinelayout);
            return pipelinelayout;

        }

        void CreateUniformBuffers()
        {
            GCHandle uhandle = GCHandle.Alloc(ubo, GCHandleType.Pinned);
            IntPtr upointer = uhandle.AddrOfPinnedObject();
            uniformBuffers = new VulkanBuffer<UniformBufferObject>((void*)upointer, 1, BufferUsageFlags.BufferUsageTransferSrcBit | BufferUsageFlags.BufferUsageTransferDstBit | BufferUsageFlags.BufferUsageUniformBufferBit , MemoryPropertyFlags.MemoryPropertyHostVisibleBit | MemoryPropertyFlags.MemoryPropertyHostCoherentBit, false);
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
                SilkVulkanRenderer.swapChain.swapchainExtent.Width / (float)SilkVulkanRenderer.swapChain.swapchainExtent.Height,
                0.1f,
                10.0f);

            ubo.proj.M11 *= -1;
            uint dataSize = (uint)Marshal.SizeOf(ubo);
            void* dataPtr = Unsafe.AsPointer(ref ubo);

            uniformBuffers.UpdateBufferData(dataPtr);
        }

        public CommandBuffer Draw()
        {
            var commandIndex = SilkVulkanRenderer.CommandIndex;
            var imageIndex = SilkVulkanRenderer.ImageIndex;
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
                renderArea: new(new Offset2D(0, 0), SilkVulkanRenderer.swapChain.swapchainExtent)
            );

            var viewport = new Viewport(0.0f, 0.0f, SilkVulkanRenderer.swapChain.swapchainExtent.Width, SilkVulkanRenderer.swapChain.swapchainExtent.Height, 0.0f, 1.0f);
            var scissor = new Rect2D(new Offset2D(0, 0), SilkVulkanRenderer.swapChain.swapchainExtent);

            var descSet = descriptorset;
            var vertexbuffer = vertexBuffer.Buffer;
            var commandInfo = new CommandBufferBeginInfo(flags: 0);


            VKConst.vulkan.BeginCommandBuffer(commandBuffer, &commandInfo);
            VKConst.vulkan.CmdBeginRenderPass(commandBuffer, &renderPassInfo, SubpassContents.Inline);
            VKConst.vulkan.CmdBindPipeline(commandBuffer, PipelineBindPoint.Graphics, shaderpipeline);
            VKConst.vulkan.CmdBindVertexBuffers(commandBuffer, 0, 1, &vertexbuffer, 0);
            VKConst.vulkan.CmdBindIndexBuffer(commandBuffer, indexBuffer.Buffer, 0, IndexType.Uint16);
            VKConst.vulkan.CmdBindDescriptorSets(commandBuffer, PipelineBindPoint.Graphics, shaderpipelineLayout, 0, 1, &descSet, 0, null);
            VKConst.vulkan.CmdSetViewport(commandBuffer, 0, 1, &viewport);
            VKConst.vulkan.CmdSetScissor(commandBuffer, 0, 1, &scissor);
            VKConst.vulkan.CmdDrawIndexed(commandBuffer, (uint)indices.Length, 1, 0, 0, 0);
            VKConst.vulkan.CmdEndRenderPass(commandBuffer);
            VKConst.vulkan.EndCommandBuffer(commandBuffer);

            return commandBuffer;
        }

        private void TransitionImageLayout(Image image,
                                  ImageLayout oldLayout,
                                  ImageLayout newLayout,
                                  CommandBuffer commandBuffer)
        {
            var barrier = new ImageMemoryBarrier
            {
                SType = StructureType.ImageMemoryBarrier,
                OldLayout = oldLayout,
                NewLayout = newLayout,
                SrcQueueFamilyIndex = Vk.QueueFamilyIgnored,
                DstQueueFamilyIndex = Vk.QueueFamilyIgnored,
                Image = image,
                SubresourceRange = new ImageSubresourceRange
                {

                    AspectMask = ImageAspectFlags.ColorBit,
                    BaseMipLevel = 0,
                    LevelCount = 1,
                    BaseArrayLayer = 0,
                    LayerCount = 1
                }
            };

            PipelineStageFlags sourceStage = PipelineStageFlags.AllGraphicsBit;
            PipelineStageFlags destinationStage = PipelineStageFlags.AllGraphicsBit;

            if (oldLayout == ImageLayout.Undefined && newLayout == ImageLayout.AttachmentOptimal)
            {
                barrier.SrcAccessMask = 0;
                barrier.DstAccessMask = AccessFlags.ColorAttachmentWriteBit;

                sourceStage = PipelineStageFlags.TopOfPipeBit;
                destinationStage = PipelineStageFlags.ColorAttachmentOutputBit;
            }
            else if (oldLayout == ImageLayout.AttachmentOptimal && newLayout == ImageLayout.PresentSrcKhr)
            {
                barrier.SrcAccessMask = AccessFlags.ColorAttachmentWriteBit;
                barrier.DstAccessMask = 0;

                sourceStage = PipelineStageFlags.ColorAttachmentOutputBit;
                destinationStage = PipelineStageFlags.BottomOfPipeBit;
            }

            VKConst.vulkan.CmdPipelineBarrier(commandBuffer, sourceStage, destinationStage, 0, 0, null, 0, null, 1, &barrier);
        }
    }
}
