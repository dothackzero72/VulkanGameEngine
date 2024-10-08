using VulkanGameEngineLevelEditor.Vulkan;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using Silk.NET.Vulkan;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class SilkFrameBufferRenderPass : SilkRenderPassBase
    {
        public SilkFrameBufferRenderPass() : base()
        {
        }

        public void BuildRenderPass(Texture renderedTexture)
        {

            var renderPass = CreateRenderPass();
            var frameBuffer = CreateFramebuffer();
            BuildRenderPipeline(renderedTexture);
            for (int x = 0; x < SilkVulkanRenderer.swapChain.ImageCount; x++)
            {
                var commandBuffer = SilkVulkanRenderer.BeginSingleUseCommandBuffer();
                TransitionImageLayout(SilkVulkanRenderer.swapChain.images[x], ImageLayout.Undefined, ImageLayout.ColorAttachmentOptimal, commandBuffer);
                TransitionImageLayout(SilkVulkanRenderer.swapChain.images[x], ImageLayout.ColorAttachmentOptimal, ImageLayout.PresentSrcKhr, commandBuffer);
                SilkVulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);
            }
        }

        public void BuildRenderPipeline(Texture renderedTexture)
        {
            var descriptorPool = CreateDescriptorPoolBinding();
            var descriptorSetLayout = CreateDescriptorSetLayout();
            var descriptorSet = CreateDescriptorSets();
            UpdateDescriptorSet(renderedTexture);
            var descriptorPipelineLayout = CreatePipelineLayout();
            var shaderList = CreateShaders();

            var vertexInput = PipelineVertexInputStateCreate();
            var inputAssembly = PipelineInputAssemblyStateCreate();
            var viewport = PipelineViewportStateCreate();
            var blendingInfo = PipelineColorBlendAttachmentState();
            var depthInfo = PipelineDepthStencilStateCreateInfo();
            var rasterizer = PipelineRasterizationStateCreateInfo();
            var multisampling = PipelineMultisampleStateCreate();


            PipelineColorBlendAttachmentState blendAttachment = new PipelineColorBlendAttachmentState
            {
                BlendEnable = Vk.True,
                SrcColorBlendFactor = BlendFactor.SrcAlpha,
                DstColorBlendFactor = BlendFactor.OneMinusSrcAlpha,
                ColorBlendOp = BlendOp.Add,
                SrcAlphaBlendFactor = BlendFactor.One,
                DstAlphaBlendFactor = BlendFactor.OneMinusSrcColor,
                AlphaBlendOp = BlendOp.Add,
                ColorWriteMask = (ColorComponentFlags)(
                    ColorComponentFlags.RBit |
                    ColorComponentFlags.GBit |
                    ColorComponentFlags.BBit |
                ColorComponentFlags.ABit)
            };

            PipelineColorBlendStateCreateInfo blending = new PipelineColorBlendStateCreateInfo
            {
                SType = StructureType.PipelineColorBlendStateCreateInfo,
                PNext = null,
                Flags = 0,
                LogicOpEnable = Vk.False,
                LogicOp = LogicOp.Copy,
                AttachmentCount = 1,
                PAttachments = &blendAttachment,
            };
            blending.BlendConstants[0] = 0.0f;
            blending.BlendConstants[1] = 0.0f;
            blending.BlendConstants[2] = 0.0f;
            blending.BlendConstants[3] = 0.0f;


            fixed (PipelineShaderStageCreateInfo* shaderlist = shaderList.ToArray())
            {
                var pipelineInfo = new GraphicsPipelineCreateInfo
                {
                    SType = StructureType.GraphicsPipelineCreateInfo,
                    PNext = null,
                    Flags = 0,
                    StageCount = 1,
                    PStages = shaderlist,
                    PVertexInputState = &vertexInput,
                    PInputAssemblyState = &inputAssembly,
                    PViewportState = &viewport,
                    PRasterizationState = &rasterizer,
                    PMultisampleState = &multisampling,
                    PDepthStencilState = &depthInfo,
                    PColorBlendState = &blending,
                    Layout = shaderpipelineLayout,
                    RenderPass = renderPass,
                    Subpass = 0,
                };

                Pipeline pipeline;
                VKConst.vulkan.CreateGraphicsPipelines(SilkVulkanRenderer.device, new PipelineCache(), 1, &pipelineInfo, null, &pipeline);
                shaderpipeline = pipeline;

            }
        }
        public void UpdateRenderPass(Texture texture)
        {
        }

        public CommandBuffer Draw()
        {
            ClearValue clearValue = new ClearValue
            {
                Color = new ClearColorValue(1.0f, 0.0f, 0.0f, 1.0f)
            };

            Viewport viewport = new Viewport
            {
                X= 0.0f,
                Y = 0.0f,
                Width = (float)SilkVulkanRenderer.swapChain.swapchainExtent.Width,
                Height = (float)SilkVulkanRenderer.swapChain.swapchainExtent.Height,
                MinDepth = 0.0f,
                MaxDepth = 1.0f
            };

            Rect2D scissor = new Rect2D
            {
                Offset = new Offset2D { X = 0, Y = 0 },
                Extent = SilkVulkanRenderer.swapChain.swapchainExtent,
            };

            RenderPassBeginInfo renderPassInfo = new RenderPassBeginInfo
            {
                SType = StructureType.RenderPassBeginInfo,
                RenderPass = renderPass,
                Framebuffer = FrameBufferList[(int)SilkVulkanRenderer.ImageIndex],
                RenderArea = scissor,
                ClearValueCount = 1,
                PClearValues = &clearValue
            };

            CommandBufferBeginInfo commandBufferBeginInfo = new CommandBufferBeginInfo
            {
                SType = StructureType.CommandBufferBeginInfo,
                Flags = CommandBufferUsageFlags.SimultaneousUseBit
            };

            int imageIndex = (int)SilkVulkanRenderer.ImageIndex;
            var descripton = descriptorset;
            VKConst.vulkan.BeginCommandBuffer(commandBufferList[imageIndex], &commandBufferBeginInfo);
            VKConst.vulkan.CmdBeginRenderPass(commandBufferList[imageIndex], &renderPassInfo, SubpassContents.Inline);
            VKConst.vulkan.CmdSetViewport(commandBufferList[imageIndex], 0, 1, &viewport);
            VKConst.vulkan.CmdSetScissor(commandBufferList[imageIndex], 0, 1, &scissor);
            VKConst.vulkan.CmdBindPipeline(commandBufferList[imageIndex], PipelineBindPoint.Graphics, shaderpipeline);
            VKConst.vulkan.CmdBindDescriptorSets(commandBufferList[imageIndex], PipelineBindPoint.Graphics, shaderpipelineLayout, 0, 1, &descripton, 0, null);
            VKConst.vulkan.CmdDraw(commandBufferList[imageIndex], 6, 1, 0, 0);
            VKConst.vulkan.CmdEndRenderPass(commandBufferList[imageIndex]);
            VKConst.vulkan.EndCommandBuffer(commandBufferList[imageIndex]);

            return commandBufferList[imageIndex];
        }

        private RenderPass CreateRenderPass()
        {
            var attachmentDescription = new List<AttachmentDescription>()
            {
                new AttachmentDescription
                {
                    Format = Format.R8G8B8A8Unorm,
                    Samples = SampleCountFlags.Count1Bit,
                    LoadOp = AttachmentLoadOp.Clear,
                    StoreOp = AttachmentStoreOp.Store,
                    StencilLoadOp = AttachmentLoadOp.DontCare,
                    StencilStoreOp = AttachmentStoreOp.DontCare,
                    InitialLayout = ImageLayout.Undefined,
                    FinalLayout = ImageLayout.PresentSrcKhr
                }
            };

            AttachmentReference colorAttachmentRef = new AttachmentReference
            {
                Attachment = 0,
                Layout = ImageLayout.ColorAttachmentOptimal,
            };

            var subpass = new SubpassDescription
            {
                PipelineBindPoint = PipelineBindPoint.Graphics,
                ColorAttachmentCount = 1,
                PColorAttachments = &colorAttachmentRef,
                PResolveAttachments = null,
                PDepthStencilAttachment = null
            };

            AttachmentDescription* attachments = (AttachmentDescription*)Marshal.AllocHGlobal(sizeof(AttachmentDescription) * (int)attachmentDescription.Count).ToPointer();
            for (int x = 0; x < attachmentDescription.Count; x++)
            {
                attachments[x] = attachmentDescription[x];
            }

            RenderPassCreateInfo renderPassCreateInfo = new
            (
                attachmentCount: (uint)attachmentDescription.Count,
                pAttachments: attachments,
                subpassCount: 1,
                pSubpasses: &subpass
            );

            RenderPass renderPass2 = new RenderPass();
            SilkVulkanRenderer.vulkan.CreateRenderPass(SilkVulkanRenderer.device, &renderPassCreateInfo, null, (Silk.NET.Vulkan.RenderPass*)&renderPass2);
            renderPass = renderPass2;

            return renderPass;
        }

        private Framebuffer[] CreateFramebuffer()
        {

            Framebuffer[] frameBufferList = FrameBufferList;
            for (int x = 0; x < SilkVulkanRenderer.swapChain.ImageCount; x++)
            {
                List<ImageView> TextureAttachmentList = new List<ImageView>();
                fixed (ImageView* imageViewPtr = SilkVulkanRenderer.swapChain.imageViews)
                {
                    TextureAttachmentList.Add(SilkVulkanRenderer.swapChain.imageViews[x]);

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

        private DescriptorPool CreateDescriptorPoolBinding()
        {
            DescriptorPool descriptorPool = new DescriptorPool();
            List<DescriptorPoolSize> DescriptorPoolBinding = new List<DescriptorPoolSize>();
            {
                new DescriptorPoolSize
                {
                    Type = DescriptorType.CombinedImageSampler,
                    DescriptorCount = 1
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

        private DescriptorSetLayout CreateDescriptorSetLayout()
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
                },
            };

            fixed (DescriptorSetLayoutBinding* ptr = LayoutBindingList.ToArray())
            {
                DescriptorSetLayoutCreateInfo layoutInfo = new DescriptorSetLayoutCreateInfo()
                {
                    SType = StructureType.DescriptorSetLayoutCreateInfo,
                    BindingCount = (uint)LayoutBindingList.Count,
                    PBindings = ptr,
                };
                SilkVulkanRenderer.vulkan.CreateDescriptorSetLayout(SilkVulkanRenderer.device, in layoutInfo, null, out DescriptorSetLayout descriptorsetLayout);

                descriptorSetLayout = descriptorsetLayout;
            }
            return descriptorSetLayout;
        }

        private DescriptorSet CreateDescriptorSets()
        {

            DescriptorSet descriptorSet = new DescriptorSet();
            DescriptorSetLayout layout = descriptorSetLayout;
            DescriptorSetAllocateInfo allocInfo = new DescriptorSetAllocateInfo()
            {
                SType = StructureType.DescriptorSetAllocateInfo,
                DescriptorPool = descriptorpool,
                DescriptorSetCount = 1,
                PSetLayouts = &layout
            };

            SilkVulkanRenderer.vulkan.AllocateDescriptorSets(SilkVulkanRenderer.device, &allocInfo, &descriptorSet);
            descriptorset = descriptorSet;
            return descriptorset;
        }

        private void UpdateDescriptorSet(Texture texture)
        {
            List<DescriptorImageInfo> colorDescriptorImages = new List<DescriptorImageInfo>
            {
                //new DescriptorImageInfo
                //{
                //    Sampler = texture.Sampler,
                //    ImageView = texture.View,
                //    ImageLayout = ImageLayout.ReadOnlyOptimal
                //}
            };

            List<WriteDescriptorSet> descriptorSetList = new List<WriteDescriptorSet>();
            fixed (DescriptorImageInfo* imageInfoPtr = colorDescriptorImages.ToArray())
            {
                for (uint x = 0; x < SilkVulkanRenderer.swapChain.ImageCount; x++)
                {
                    WriteDescriptorSet descriptorSet = new WriteDescriptorSet
                    {
                        SType = StructureType.WriteDescriptorSet,
                        DstSet = descriptorset,
                        DstBinding = 0,
                        DstArrayElement = x,
                        DescriptorCount = 1,
                        DescriptorType = DescriptorType.CombinedImageSampler,
                        PImageInfo = imageInfoPtr + x,
                        PBufferInfo = null,
                        PTexelBufferView = null
                    };

                    descriptorSetList.Add(descriptorSet);
                }
            }

            fixed (WriteDescriptorSet* ptr = descriptorSetList.ToArray())
            {
                SilkVulkanRenderer.vulkan.UpdateDescriptorSets(SilkVulkanRenderer.device, (uint)colorDescriptorImages.UCount(), ptr, 0, null);
            }
        }

        private PipelineLayout CreatePipelineLayout()
        {
            DescriptorSetLayout descriptosetLayout = descriptorSetLayout;

            PipelineLayout shaderPipelineLayout = new PipelineLayout();
            PipelineLayoutCreateInfo pipelineLayoutInfo = new PipelineLayoutCreateInfo
            {
                SType = StructureType.PipelineLayoutCreateInfo,
                SetLayoutCount = 1,
                PSetLayouts = &descriptosetLayout
            };
            SilkVulkanRenderer.vulkan.CreatePipelineLayout(SilkVulkanRenderer.device, &pipelineLayoutInfo, null, &shaderPipelineLayout);

            shaderpipelineLayout = shaderPipelineLayout;
            return shaderPipelineLayout;
        }

        private List<PipelineShaderStageCreateInfo> CreateShaders()
        {
            return new List<PipelineShaderStageCreateInfo>()
            {
                SilkVulkanRenderer.CreateShader("C:/Users/dotha/Documents/GitHub/VulkanGameEngine/Shaders/vertex_shader.spv",  ShaderStageFlags.VertexBit),
                SilkVulkanRenderer.CreateShader("C:/Users/dotha/Documents/GitHub/VulkanGameEngine/Shaders/fragment_shader.spv", ShaderStageFlags.FragmentBit)
            };
        }

        private PipelineVertexInputStateCreateInfo PipelineVertexInputStateCreate()
        {
            return new PipelineVertexInputStateCreateInfo()
            {
                SType = StructureType.PipelineVertexInputStateCreateInfo
            };
        }

        private PipelineInputAssemblyStateCreateInfo PipelineInputAssemblyStateCreate()
        {

            return new PipelineInputAssemblyStateCreateInfo()
            {
                SType = StructureType.PipelineInputAssemblyStateCreateInfo,
                Topology = PrimitiveTopology.TriangleList,
                PrimitiveRestartEnable = Vk.False
            };
        }

        private PipelineViewportStateCreateInfo PipelineViewportStateCreate()
        {
            List<Viewport> viewportList = new List<Viewport>()
            {
                new Viewport
                {
                    X = 0.0f,
                    Y = 0.0f,
                    Width = SilkVulkanRenderer.swapChain.swapchainExtent.Width, // Ensure this is not zero or negative
                    Height = SilkVulkanRenderer.swapChain.swapchainExtent.Height, // Ensure this is not zero or negative
                    MinDepth = 0.0f,
                    MaxDepth = 1.0f
                }
            };

            List<Rect2D> rect2DList = new List<Rect2D>()
            {
                new Rect2D
                {
                    Offset = new Offset2D
                    {
                        X = 0,
                        Y = 0
                    },
                    Extent = new Extent2D
                    {
                        Width = SilkVulkanRenderer.swapChain.swapchainExtent.Width, // Ensure this is correct
                        Height = SilkVulkanRenderer.swapChain.swapchainExtent.Height // Ensure this is correct
                    }
                }
            };

            var viewportState = new PipelineViewportStateCreateInfo();
            fixed (Viewport* viewport = viewportList.ToArray())
            fixed (Rect2D* rect2D = rect2DList.ToArray())
            {
                PipelineViewportStateCreateInfo viewportStateInfo = new PipelineViewportStateCreateInfo()
                {
                    SType = StructureType.PipelineViewportStateCreateInfo,
                    ViewportCount = 1,
                    PViewports = viewport,
                    ScissorCount = 1,
                    PScissors = rect2D,
                };
                viewportState = viewportStateInfo;
            }

            return viewportState;
        }

        private PipelineColorBlendStateCreateInfo PipelineColorBlendAttachmentState()
        {
            List<PipelineColorBlendAttachmentState> blendAttachmentList = new List<PipelineColorBlendAttachmentState>
            {
                new PipelineColorBlendAttachmentState
                {
                    BlendEnable =  Vk.True,
                    SrcColorBlendFactor = BlendFactor.SrcAlpha,
                    DstColorBlendFactor = BlendFactor.OneMinusSrcAlpha,
                    ColorBlendOp = BlendOp.Add,
                    SrcAlphaBlendFactor = BlendFactor.One,
                    DstAlphaBlendFactor = BlendFactor.OneMinusDstAlpha,
                    AlphaBlendOp = BlendOp.Add,
                    ColorWriteMask = (ColorComponentFlags)(
                        ColorComponentFlags.RBit |
                        ColorComponentFlags.GBit |
                        ColorComponentFlags.BBit |
                        ColorComponentFlags.ABit)
                }
            };

            PipelineColorBlendStateCreateInfo colorBlending;
            fixed (PipelineColorBlendAttachmentState* blendAttachmentPtr = blendAttachmentList.ToArray())
            {
                colorBlending = new PipelineColorBlendStateCreateInfo()
                {
                    AttachmentCount = (uint)blendAttachmentList.UCount(),
                    PAttachments = blendAttachmentPtr
                };
            }
            return colorBlending;
        }

        private PipelineDepthStencilStateCreateInfo PipelineDepthStencilStateCreateInfo()
        {

            return new PipelineDepthStencilStateCreateInfo()
            {
                DepthTestEnable = Vk.True,
                DepthWriteEnable = Vk.True,
                DepthCompareOp = CompareOp.Less,
                DepthBoundsTestEnable = Vk.False,
                StencilTestEnable = Vk.False
            };
        }

        private PipelineRasterizationStateCreateInfo PipelineRasterizationStateCreateInfo()
        {
            return new PipelineRasterizationStateCreateInfo()
            {
                SType = StructureType.PipelineRasterizationStateCreateInfo,
                DepthClampEnable = Vk.False,
                RasterizerDiscardEnable = Vk.False,
                PolygonMode = PolygonMode.Fill,
                CullMode = CullModeFlags.None,
                FrontFace = FrontFace.CounterClockwise,
                DepthBiasEnable = Vk.False,
                LineWidth = 1.0f
            };
        }

        private PipelineMultisampleStateCreateInfo PipelineMultisampleStateCreate()
        {
            return new PipelineMultisampleStateCreateInfo()
            {
                SType = StructureType.PipelineMultisampleStateCreateInfo,
                RasterizationSamples = SampleCountFlags.Count1Bit
            };
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
