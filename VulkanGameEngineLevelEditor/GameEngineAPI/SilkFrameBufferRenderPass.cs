using System;
using System.Collections.Generic;
using System.IO;
using Silk.NET.Core.Native;
using Silk.NET.SDL;
using Silk.NET.Vulkan;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class SilkFrameBufferRenderPass : SilkRenderPassBase
    {
        private readonly Vk vk;
        private readonly Device device;
        public Texture texture;
        public SilkFrameBufferRenderPass(Device device, Texture texture)
        {
            vk = Vk.GetApi();
            this.device = device;
            this.texture = texture;

            CreateRenderPass();
            CreateFramebuffer();
            CreateDescriptorPoolBinding();
            CreateDescriptorSetLayout();
            allocateDescriptorSets();
            
            UpdateDescriptorSet();
            CreatePipelineLayout();
            CreateGraphicsPipeline();
            commandBufferList = new CommandBuffer[SilkVulkanRenderer.MAX_FRAMES_IN_FLIGHT];
            SilkVulkanRenderer.CreateCommandBuffers(commandBufferList);
        }
        public DescriptorSetLayout CreateDescriptorSetLayout()
        {
            List<DescriptorSetLayoutBinding> LayoutBindingList = new List<DescriptorSetLayoutBinding>()
                {
                    new DescriptorSetLayoutBinding()
                    {
                        Binding = 0,
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
                TextureAttachmentList.Add(SilkVulkanRenderer.swapChain.imageViews[x]);

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

        public DescriptorSet allocateDescriptorSets()
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

        public void UpdateDescriptorSet()
        {
            var colorDescriptorImage = new DescriptorImageInfo
            {
                Sampler = texture.Sampler,
                ImageView = texture.View,
                ImageLayout = Silk.NET.Vulkan.ImageLayout.ReadOnlyOptimal
            };

            List<WriteDescriptorSet> descriptorSetList = new List<WriteDescriptorSet>();
            for (uint x = 0; x < SilkVulkanRenderer.swapChain.ImageCount; x++)
            {
                WriteDescriptorSet descriptorSetWrite2 = new WriteDescriptorSet
                {
                    SType = StructureType.WriteDescriptorSet,
                    DstSet = descriptorset,
                    DstBinding = 0,
                    DstArrayElement = 0,
                    DescriptorCount = 1,
                    DescriptorType = DescriptorType.CombinedImageSampler,
                    PImageInfo = &colorDescriptorImage,
                    PBufferInfo = null,
                    PTexelBufferView = null
                };
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
            shaderpipelineLayout = pipelinelayout;
            return pipelinelayout;

        }

        // Create the Render Pass
        private RenderPass CreateRenderPass()
        {
            var colorAttachment = new AttachmentDescription
                {
                    Format = Format.B8G8R8A8Unorm,
                    Samples = SampleCountFlags.SampleCount1Bit,
                    LoadOp = AttachmentLoadOp.Clear,
                    StoreOp = AttachmentStoreOp.Store,
                    StencilLoadOp = AttachmentLoadOp.DontCare,
                    StencilStoreOp = AttachmentStoreOp.DontCare,
                    InitialLayout = ImageLayout.Undefined,
                    FinalLayout = ImageLayout.ColorAttachmentOptimal
                
            };

            List<AttachmentReference> colorRefsList = new List<AttachmentReference>()
                    {
                        new AttachmentReference
                        {
                            Attachment = 0,
                            Layout = ImageLayout.ColorAttachmentOptimal
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
                                PDepthStencilAttachment =null
                            }
                        };
            }
            fixed (SubpassDescription* description = subpassDescriptionList.ToArray())
            {
                var renderPassInfo = new RenderPassCreateInfo
                {
                    SType = StructureType.RenderPassCreateInfo,
                    AttachmentCount = 1,
                    PAttachments = &colorAttachment,
                    SubpassCount = 1,
                    PSubpasses = description,
                    DependencyCount = 0
                };

                vk.CreateRenderPass(device, &renderPassInfo, null, out RenderPass renderPasS);
                renderPass = renderPasS;
            }

         
            return renderPass;
        }

        // Create Graphics Pipeline
        private Pipeline CreateGraphicsPipeline()
        {
            PipelineShaderStageCreateInfo* shadermoduleList = stackalloc[]
                        {
                SilkVulkanRenderer.CreateShader("C:/Users/dotha/Documents/GitHub/VulkanGameEngine/Shaders/vertex_shader.spv",  ShaderStageFlags.VertexBit),
                SilkVulkanRenderer.CreateShader("C:/Users/dotha/Documents/GitHub/VulkanGameEngine/Shaders/fragment_shader.spv", ShaderStageFlags.FragmentBit)
            };

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

            vk.CreateGraphicsPipelines(device, new PipelineCache(null), 1, &graphicsPipelineCreateInfo, null, out Pipeline pipeline);
            shaderpipeline = pipeline;
            return pipeline;
        }

        // Cleanup allocated Vulkan resources.
        //public void Cleanup()
        //{
        //    vk.DestroyPipeline(device, pipeline, null);
        //    vk.DestroyPipelineLayout(device, pipelineLayout, null);
        //    vk.DestroyShaderModule(device, vertexShaderModule, null);
        //    vk.DestroyShaderModule(device, fragmentShaderModule, null);
        //    vk.DestroyRenderPass(device, renderPass, null);
        //}
        public CommandBuffer Draw()
        {
            var commandIndex = SilkVulkanRenderer.CommandIndex;
            var imageIndex = SilkVulkanRenderer.ImageIndex;
            var commandBuffer = commandBufferList[commandIndex];

            //ClearValue[] clearValues = { new ClearValue(new ClearColorValue(0.0f, 0.0f, 1.0f, 1.0f)) }; // Blue clear color

            ClearValue* clearValues = stackalloc[]
{
                new ClearValue(new ClearColorValue(0, 0, 1, 1)),
            };

            RenderPassBeginInfo renderPassInfo = new RenderPassBeginInfo
            {
                RenderPass = renderPass,
                Framebuffer = FrameBufferList[imageIndex],
                RenderArea = new Rect2D(new Offset2D(0, 0), SilkVulkanRenderer.swapChain.swapchainExtent),
                ClearValueCount = (uint)1,
                PClearValues = clearValues
            };

            var viewport = new Viewport(0, 0, SilkVulkanRenderer.swapChain.swapchainExtent.Width, SilkVulkanRenderer.swapChain.swapchainExtent.Height, 0.0f, 1.0f);
            var scissor = new Rect2D(new Offset2D(0, 0), SilkVulkanRenderer.swapChain.swapchainExtent);

            var commandInfo = new CommandBufferBeginInfo { Flags = CommandBufferUsageFlags.OneTimeSubmitBit };

            vk.BeginCommandBuffer(commandBuffer, &commandInfo);

            vk.CmdBeginRenderPass(commandBuffer, &renderPassInfo, SubpassContents.Inline);
            vk.CmdSetViewport(commandBuffer, 0, 1, &viewport);
            vk.CmdSetScissor(commandBuffer, 0, 1, &scissor);

            vk.CmdBindPipeline(commandBuffer, PipelineBindPoint.Graphics, shaderpipeline);


            // Bind descriptor set
            var set = descriptorset;
            vk.CmdBindDescriptorSets(commandBuffer, PipelineBindPoint.Graphics, shaderpipelineLayout, 0, 1, &set, 0, null);

            // Ensure correct vertex count if drawing triangles
            vk.CmdDraw(commandBuffer, 6, 1, 0, 0);

            vk.CmdEndRenderPass(commandBuffer);
            vk.EndCommandBuffer(commandBuffer);

            return commandBuffer;
        }
        public void Dispose()
        {
        }
    }
}