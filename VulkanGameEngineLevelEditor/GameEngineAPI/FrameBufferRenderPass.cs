using GlmSharp;
using Silk.NET.SDL;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.Vulkan;
using static System.Windows.Forms.DataFormats;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class FrameBufferRenderPass
    {
        Vk vk = Vk.GetApi();

        ivec2 RenderPassResolution;
        SampleCountFlags SampleCount;

        RenderPass renderPass;
        CommandBuffer[] CommandBufferList;
        Framebuffer[] FrameBufferList;

        DescriptorPool descriptorPool;
        DescriptorSetLayout descriptorSetLayout;
        DescriptorSet descriptorSet;
        Pipeline pipeline;
        PipelineLayout pipelineLayout;
        PipelineCache pipelineCache;

        public FrameBufferRenderPass()
        {

        }

        public void BuildRenderPass(Texture texture)
        {
            RenderPassResolution = new ivec2((int)VulkanRenderer.swapChain.swapchainExtent.Width, (int)VulkanRenderer.swapChain.swapchainExtent.Height);
            SampleCount = SampleCountFlags.Count1Bit;

            CommandBufferList = new CommandBuffer[(int)VulkanRenderer.swapChain.ImageCount];
            FrameBufferList = new Framebuffer[(int)VulkanRenderer.swapChain.ImageCount];

            renderPass = CreateRenderPass();
            FrameBufferList = CreateFramebuffer();
            BuildRenderPipeline(texture);
            VulkanRenderer.CreateCommandBuffers(CommandBufferList);
        }

        public RenderPass CreateRenderPass()
        {
            RenderPass tempRenderPass;
            List<AttachmentDescription> attachmentDescriptionList = new List<AttachmentDescription>()
            {
                new AttachmentDescription
                {
                    Format = Silk.NET.Vulkan.Format.R8G8B8A8Unorm,
                    Samples = SampleCountFlags.SampleCount1Bit,
                    LoadOp = AttachmentLoadOp.Clear,
                    StoreOp = AttachmentStoreOp.Store,
                    StencilLoadOp = AttachmentLoadOp.DontCare,
                    StencilStoreOp = AttachmentStoreOp.DontCare,
                    InitialLayout = Silk.NET.Vulkan.ImageLayout.Undefined,
                    FinalLayout = Silk.NET.Vulkan.ImageLayout.PresentSrcKhr,
                }
            };

            List<AttachmentReference> colorRefsList = new List<AttachmentReference>()
            {
                new AttachmentReference
                {
                    Attachment = 0,
                    Layout = Silk.NET.Vulkan.ImageLayout.ColorAttachmentOptimal
                }
            };

            List<AttachmentReference> multiSampleReferenceList = new List<AttachmentReference>();
            List<AttachmentReference> depthReference = new List<AttachmentReference>();

            List<SubpassDescription> subpassDescriptionList = new List<SubpassDescription>();
            fixed (AttachmentReference* colorRefs = colorRefsList.ToArray())
            {
                subpassDescriptionList.Add(
                    new SubpassDescription
                    {
                        PipelineBindPoint = PipelineBindPoint.Graphics,
                        ColorAttachmentCount = (uint)colorRefsList.Count,
                        PColorAttachments = colorRefs,
                        PResolveAttachments = null,
                        PDepthStencilAttachment = null
                    });
            }

            List<SubpassDependency> subpassDependencyList = new List<SubpassDependency>()
            {
                new SubpassDependency
                {
                                SrcSubpass = uint.MaxValue,
                                DstSubpass = 0,
                                SrcStageMask = PipelineStageFlags.ColorAttachmentOutputBit,
                                DstStageMask = PipelineStageFlags.ColorAttachmentOutputBit,
                                SrcAccessMask = 0,
                                DstAccessMask = AccessFlags.ColorAttachmentWriteBit,
                }
            };

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

        public Framebuffer[] CreateFramebuffer()
        {
            Framebuffer[] frameBufferList = new Framebuffer[(int)VulkanRenderer.swapChain.ImageCount];
            for (int x = 0; x < (int)VulkanRenderer.swapChain.ImageCount; x++)
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
            DescriptorPool tempDescriptorPool = new DescriptorPool();
            List<DescriptorPoolSize> DescriptorPoolBinding = new List<DescriptorPoolSize>();
            {
                new DescriptorPoolSize
                {
                    Type = DescriptorType.UniformBuffer,
                    DescriptorCount = VulkanRenderer.swapChain.ImageCount
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
                vk.CreateDescriptorPool(VulkanRenderer.device, in poolInfo, null, out tempDescriptorPool);
            }

            descriptorPool = tempDescriptorPool;
            return descriptorPool;
        }

        public void BuildRenderPipeline(Texture texture)
        {
            descriptorPool = CreateDescriptorPoolBinding();
            descriptorSetLayout = CreateDescriptorSetLayout();
            descriptorSet = CreateDescriptorSets();
            UpdateDescriptorSet(texture);
            pipelineLayout = CreatePipelineLayout();

            var vertexShaderModule = VulkanRenderer.CreateShader("C:/Users/dotha/Documents/GitHub/VulkanGameEngine/Shaders/FrameBufferShaderVert.spv", ShaderStageFlags.VertexBit);
            var fragmentShaderModule = VulkanRenderer.CreateShader("C:/Users/dotha/Documents/GitHub/VulkanGameEngine/Shaders/FrameBufferShaderFrag.spv", ShaderStageFlags.FragmentBit);
            PipelineShaderStageCreateInfo* shadermoduleList = stackalloc[]
            {
        vertexShaderModule,
        fragmentShaderModule
    };

            PipelineVertexInputStateCreateInfo vertexInputInfo = new PipelineVertexInputStateCreateInfo();
            List<VertexInputBindingDescription> bindingDescriptionList = new List<VertexInputBindingDescription>();
            List<VertexInputAttributeDescription> AttributeDescriptions = new List<VertexInputAttributeDescription>();

            fixed (VertexInputBindingDescription* bindingDescription = bindingDescriptionList.ToArray())
            fixed (VertexInputAttributeDescription* AttributeDescription = AttributeDescriptions.ToArray())
            {
                vertexInputInfo.SType = StructureType.PipelineVertexInputStateCreateInfo;
                vertexInputInfo.VertexBindingDescriptionCount = (uint)bindingDescriptionList.Count;
                vertexInputInfo.PVertexBindingDescriptions = bindingDescription;
                vertexInputInfo.VertexAttributeDescriptionCount = (uint)AttributeDescriptions.Count;
                vertexInputInfo.PVertexAttributeDescriptions = AttributeDescription;
            }

            PipelineInputAssemblyStateCreateInfo inputAssembly = new(topology: PrimitiveTopology.TriangleList);

            List<Viewport> viewportList = new List<Viewport>()
            {
                new Viewport
                {
                    X = 0.0f,
                    Y = 0.0f,
                    Width = (float)RenderPassResolution.x,
                    Height = (float)RenderPassResolution.y,
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
                        Y = 0,
                    },
                    Extent =
                    {
                        Width = (uint)RenderPassResolution.x,
                        Height = (uint)RenderPassResolution.y
                    }
                }
            };

            PipelineViewportStateCreateInfo viewportState = new PipelineViewportStateCreateInfo();
            fixed (Viewport* viewport = viewportList.ToArray())
            fixed (Rect2D* rect2D = rect2DList.ToArray())
            {
                viewportState = new PipelineViewportStateCreateInfo()
                {
                    SType = StructureType.PipelineViewportStateCreateInfo,
                    ViewportCount = 1,
                    ScissorCount = 1,
                    PScissors = rect2D,
                    PViewports = viewport
                };
            }

            List<PipelineColorBlendAttachmentState> blendAttachmentList = new List<PipelineColorBlendAttachmentState>()
            {
                new PipelineColorBlendAttachmentState
                {
                    BlendEnable = true,
                    SrcColorBlendFactor = Silk.NET.Vulkan.BlendFactor.SrcAlpha,
                    DstColorBlendFactor = Silk.NET.Vulkan.BlendFactor.OneMinusSrcAlpha,
                    ColorBlendOp = BlendOp.Add,
                    SrcAlphaBlendFactor = Silk.NET.Vulkan.BlendFactor.One,
                    DstAlphaBlendFactor = Silk.NET.Vulkan.BlendFactor.Zero,
                    AlphaBlendOp = BlendOp.Add,
                    ColorWriteMask = ColorComponentFlags.RBit | ColorComponentFlags.GBit | ColorComponentFlags.BBit | ColorComponentFlags.ABit
                }
            };

            PipelineDepthStencilStateCreateInfo blendDepthAttachment = new PipelineDepthStencilStateCreateInfo()
            {
                SType = StructureType.PipelineDepthStencilStateCreateInfo,
                DepthTestEnable = Vk.True,
                DepthWriteEnable = Vk.True,
                DepthCompareOp = CompareOp.LessOrEqual,
                DepthBoundsTestEnable = Vk.False,
                StencilTestEnable = Vk.False
            };

            PipelineRasterizationStateCreateInfo rasterizer = new PipelineRasterizationStateCreateInfo()
            {
                SType = StructureType.PipelineRasterizationStateCreateInfo,
                DepthClampEnable = false,
                RasterizerDiscardEnable = false,
                PolygonMode = PolygonMode.Fill,
                CullMode = CullModeFlags.CullModeBackBit,
                FrontFace = FrontFace.CounterClockwise,
                DepthBiasEnable = false,
                DepthBiasConstantFactor = 0.0f,
                DepthBiasClamp = 0.0f,
                DepthBiasSlopeFactor = 0.0f,
                LineWidth = 1.0f
            };

            PipelineMultisampleStateCreateInfo multisampling = new PipelineMultisampleStateCreateInfo()
            {
                SType = StructureType.PipelineMultisampleStateCreateInfo,
                RasterizationSamples = SampleCountFlags.Count1Bit
            };

            fixed (PipelineColorBlendAttachmentState* attachments = blendAttachmentList.ToArray())
            {
                PipelineColorBlendStateCreateInfo pipelineColorBlendStateCreateInfo = new PipelineColorBlendStateCreateInfo()
                {
                    SType = StructureType.PipelineColorBlendStateCreateInfo,
                    LogicOpEnable = false,
                    LogicOp = LogicOp.NoOp,
                    AttachmentCount = 1,
                    PAttachments = attachments
                };

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


                GraphicsPipelineCreateInfo graphicsPipelineCreateInfo = new GraphicsPipelineCreateInfo
                {
                    SType = StructureType.GraphicsPipelineCreateInfo,
                    StageCount = 2,
                    PStages = shadermoduleList,
                    PVertexInputState = &vertexInputInfo,
                    PInputAssemblyState = &inputAssembly,
                    PViewportState = &viewportState,
                    PRasterizationState = &rasterizer,
                    PMultisampleState = &multisampling,
                    PDepthStencilState = &blendDepthAttachment,
                    PColorBlendState = &pipelineColorBlendStateCreateInfo,
                    Layout = pipelineLayout,
                    RenderPass = renderPass
                };

                vk.CreateGraphicsPipelines(VulkanRenderer.device, new PipelineCache(null), 1, &graphicsPipelineCreateInfo, null, out Pipeline shaderPipeline);
                pipeline = shaderPipeline;
            }
        }

        public DescriptorSetLayout CreateDescriptorSetLayout()
        {
            DescriptorSetLayout descriptorSetLayout = new DescriptorSetLayout();
            List<DescriptorSetLayoutBinding> layoutBindingList = new List<DescriptorSetLayoutBinding>()
            {
                new DescriptorSetLayoutBinding
                {
                    Binding = 0,
                    DescriptorType = DescriptorType.CombinedImageSampler,
                    DescriptorCount = 1,
                    PImmutableSamplers = null,
                    StageFlags = ShaderStageFlags.FragmentBit | ShaderStageFlags.VertexBit,
                }
            };

            fixed (DescriptorSetLayoutBinding* ptr = layoutBindingList.ToArray())
            {
                DescriptorSetLayoutCreateInfo layoutInfo = new DescriptorSetLayoutCreateInfo()
                {
                    SType = StructureType.DescriptorSetLayoutCreateInfo,
                    BindingCount = layoutBindingList.UCount(),
                    PBindings = ptr,
                };

                vk.CreateDescriptorSetLayout(VulkanRenderer.device, &layoutInfo, null, out DescriptorSetLayout descriptorsetLayout);
                descriptorSetLayout = descriptorsetLayout;
                return descriptorSetLayout;
            }
        }

        public DescriptorSet CreateDescriptorSets()
        {

            DescriptorSetLayout* layouts = stackalloc DescriptorSetLayout[(int)VulkanRenderer.swapChain.ImageCount];

            for (int i = 0; i < (int)VulkanRenderer.swapChain.ImageCount; i++)
            {
                layouts[i] = descriptorSetLayout;
            }

            DescriptorSetAllocateInfo allocInfo = new
            (
                descriptorPool: descriptorPool,
                descriptorSetCount: (uint)VulkanRenderer.swapChain.ImageCount,
                pSetLayouts: layouts
            );
            vk.AllocateDescriptorSets(VulkanRenderer.device, &allocInfo, out DescriptorSet tempdescriptorSet);
            descriptorSet = tempdescriptorSet;
            return descriptorSet;
        }

        public void UpdateDescriptorSet(Texture texture)
        {
            DescriptorImageInfo ColorDescriptorImage = new DescriptorImageInfo
            {
                Sampler = texture.Sampler,
                ImageView = texture.View,
                ImageLayout = Silk.NET.Vulkan.ImageLayout.ReadOnlyOptimal
            };

            List<WriteDescriptorSet> descriptorSetList = new List<WriteDescriptorSet>();
            
                WriteDescriptorSet descriptorSetWrite = new WriteDescriptorSet
                {
                    SType = StructureType.WriteDescriptorSet,
                    DstSet = descriptorSet,
                    DstBinding = 0,
                    DstArrayElement = 0,
                    DescriptorCount = 1,
                    DescriptorType = DescriptorType.CombinedImageSampler,
                    PImageInfo = &ColorDescriptorImage
                };

                descriptorSetList.Add(descriptorSetWrite);
            

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

        public CommandBuffer Draw()
        {
            var commandIndex = VulkanRenderer.CommandIndex;
            var imageIndex = VulkanRenderer.ImageIndex;
            var commandBuffer = CommandBufferList[(int)commandIndex];
            ClearValue* clearValues = stackalloc[]
            {
                new ClearValue(new ClearColorValue(1, 1, 0, 1))
            };

            var viewport = new Viewport(0.0f, 0.0f, VulkanRenderer.swapChain.swapchainExtent.Width, VulkanRenderer.swapChain.swapchainExtent.Height, 0.0f, 1.0f);
            var scissor = new Rect2D(new Offset2D(0, 0), VulkanRenderer.swapChain.swapchainExtent);

            RenderPassBeginInfo renderPassInfo = new
                        (
                            renderPass: renderPass,
                            framebuffer: FrameBufferList[imageIndex],
                            clearValueCount: 1,
                            pClearValues: clearValues,
                            renderArea: new(new Offset2D(0, 0), VulkanRenderer.swapChain.swapchainExtent)
            );

            var commandInfo = new CommandBufferBeginInfo(flags: 0);
            vk.BeginCommandBuffer(commandBuffer, &commandInfo);
            vk.CmdBeginRenderPass(commandBuffer, &renderPassInfo, SubpassContents.Inline);
            vk.CmdSetViewport(commandBuffer, 0, 1, &viewport);
            vk.CmdSetScissor(commandBuffer, 0, 1, &scissor);
            vk.CmdBindPipeline(commandBuffer, PipelineBindPoint.Graphics, pipeline);
            vk.CmdBindDescriptorSets(commandBuffer, PipelineBindPoint.Graphics, pipelineLayout, 0, 1, descriptorSet, 0, null);
            vk.CmdDraw(commandBuffer, 6, 1, 0, 0);
            vk.CmdEndRenderPass(commandBuffer);
            vk.EndCommandBuffer(commandBuffer);


            return commandBuffer;
        }
    }
}