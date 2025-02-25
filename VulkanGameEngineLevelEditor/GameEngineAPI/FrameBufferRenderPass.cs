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
using VulkanGameEngineGameObjectScripts.Vulkan;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class FrameBufferRenderPass
    {
        Vk vk = Vk.GetApi();

        ivec2 RenderPassResolution;
        SampleCountFlags SampleCount;

        VkRenderPass renderPass;
        VkCommandBuffer[] CommandBufferList;
        VkFramebuffer[] FrameBufferList;

        VkDescriptorPool descriptorPool;
        VkDescriptorSetLayout descriptorSetLayout;
        VkDescriptorSet descriptorSet;
        VkPipeline pipeline;
        VkPipelineLayout pipelineLayout;
        VkPipelineCache pipelineCache;

        public FrameBufferRenderPass()
        {

        }

        public void BuildRenderPass(Texture texture)
        {
            RenderPassResolution = new ivec2((int)VulkanRenderer.swapChain.SwapChainResolution.width, (int)VulkanRenderer.swapChain.SwapChainResolution.height);
            SampleCount = SampleCountFlags.Count1Bit;

            CommandBufferList = new VkCommandBuffer[(int)VulkanRenderer.swapChain.SwapChainImageCount];
            FrameBufferList = new VkFramebuffer[(int)VulkanRenderer.swapChain.SwapChainImageCount];

            renderPass = CreateRenderPass();
            FrameBufferList = CreateFramebuffer();
            BuildRenderPipeline(texture);
            VulkanRenderer.CreateCommandBuffers(CommandBufferList);
        }

        public VkRenderPass CreateRenderPass()
        {
            VkRenderPass tempRenderPass;
            List<VkAttachmentDescription> attachmentDescriptionList = new List<VkAttachmentDescription>()
            {
                new VkAttachmentDescription
                {
                    format = VkFormat.VK_FORMAT_R8G8B8A8_UNORM,
                    samples = (uint)VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT,
                    loadOp = AttachmentLoadOp.Clear,
                    storeOp = AttachmentStoreOp.Store,
                    stencilLoadOp = AttachmentLoadOp.DontCare,
                    stencilStoreOp = AttachmentStoreOp.DontCare,
                    initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED,
                    finalLayout = VkImageLayout.VK_IMAGE_LAYOUT_PRESENT_SRC_KHR,
                }
            };

            List<VkAttachmentReference> colorRefsList = new List<VkAttachmentReference>()
            {
                new VkAttachmentReference
                {
                    attachment = 0,
                    layout = Silk.NET.Vulkan.ImageLayout.ColorAttachmentOptimal
                }
            };

            List<VkAttachmentReference> multiSampleReferenceList = new List<VkAttachmentReference>();
            List<VkAttachmentReference> depthReference = new List<VkAttachmentReference>();

            List<VkSubpassDescription> subpassDescriptionList = new List<VkSubpassDescription>();
            fixed (VkAttachmentReference* colorRefs = colorRefsList.ToArray())
            {
                subpassDescriptionList.Add(
                    new VkSubpassDescription
                    {
                        pipelineBindPoint = VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS,
                        colorAttachmentCount = (uint)colorRefsList.Count,
                        pColorAttachments = colorRefs,
                        pResolveAttachments = null,
                        pDepthStencilAttachment = null
                    });
            }

            List<VkSubpassDependency> subpassDependencyList = new List<VkSubpassDependency>()
            {
                new VkSubpassDependency
                {
                                srcSubpass = uint.MaxValue,
                                dstSubpass = 0,
                                srcStageMask = VkPipelineStageFlags.COLOR_ATTACHMENT_OUTPUT_BIT,
                                dstStageMask = VkPipelineStageFlags.COLOR_ATTACHMENT_OUTPUT_BIT,
                                srcAccessMask = 0,
                                dstAccessMask = VkAccessFlags.COLOR_ATTACHMENT_WRITE_BIT,
                }
            };

            fixed (VkAttachmentDescription* attachments = attachmentDescriptionList.ToArray())
            fixed (VkSubpassDescription* description = subpassDescriptionList.ToArray())
            fixed (VkSubpassDependency* dependency = subpassDependencyList.ToArray())
            {
                var renderPassCreateInfo = new VkRenderPassCreateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_RENDER_PASS_CREATE_INFO,
                    pNext = null,
                    flags = 0,
                    attachmentCount = (uint)attachmentDescriptionList.Count(),
                    pAttachments = attachments,
                    subpassCount = (uint)subpassDescriptionList.Count(),
                    pSubpasses = description,
                    dependencyCount = (uint)subpassDependencyList.Count(),
                    pDependencies = dependency
                };

                Vulkan.vkCreateRenderPass(VulkanRenderer.device, &renderPassCreateInfo, null, &tempRenderPass);
                renderPass = tempRenderPass;
            }

            return renderPass;
        }

        public VkFramebuffer[] CreateFramebuffer()
        {
            VkFramebuffer[] frameBufferList = new VkFramebuffer[(int)VulkanRenderer.swapChain.SwapChainImageCount];
            for (int x = 0; x < (int)VulkanRenderer.swapChain.SwapChainImageCount; x++)
            {
                List<VkImageView> TextureAttachmentList = new List<VkImageView>();
                TextureAttachmentList.Add(VulkanRenderer.swapChain.SwapChainImageViews[x]);

                fixed (VkImageView* imageViewPtr = TextureAttachmentList.ToArray())
                {
                    VkFramebufferCreateInfo framebufferInfo = new VkFramebufferCreateInfo()
                    {
                        sType = VkStructureType.VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO,
                        renderPass = renderPass,
                        attachmentCount = TextureAttachmentList.UCount(),
                        pAttachments = imageViewPtr,
                        width = VulkanRenderer.swapChain.SwapChainResolution.width,
                        height = VulkanRenderer.swapChain.SwapChainResolution.height,
                        layers = 1
                    };

                    VkFramebuffer frameBuffer = FrameBufferList[x];
                    Vulkan.vkCreateFramebuffer(VulkanRenderer.device, &framebufferInfo, null, &frameBuffer);
                    frameBufferList[x] = frameBuffer;
                }
            }

            FrameBufferList = frameBufferList;
            return frameBufferList;
        }

        public VkDescriptorPool CreateDescriptorPoolBinding()
        {
            VkDescriptorPool tempDescriptorPool = new VkDescriptorPool();
            List<VkDescriptorPoolSize> DescriptorPoolBinding = new List<VkDescriptorPoolSize>();
            {
                new VkDescriptorPoolSize
                {
                    type = VkDescriptorType.VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER,
                    descriptorCount = VulkanRenderer.swapChain.SwapChainImageCount
                };
            };

            fixed (VkDescriptorPoolSize* ptr = DescriptorPoolBinding.ToArray())
            {
                VkDescriptorPoolCreateInfo poolInfo = new VkDescriptorPoolCreateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_DESCRIPTOR_POOL_CREATE_INFO,
                    maxSets = VulkanRenderer.swapChain.SwapChainImageCount,
                    poolSizeCount = (uint)DescriptorPoolBinding.Count,
                    pPoolSizes = ptr
                };
                Vulkan.vkCreateDescriptorPool(VulkanRenderer.device, in poolInfo, null, out tempDescriptorPool);
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

            VkPipelineVertexInputStateCreateInfo vertexInputInfo = new VkPipelineVertexInputStateCreateInfo();
            List<VkVertexInputBindingDescription> bindingDescriptionList = new List<VkVertexInputBindingDescription>();
            List<VkVertexInputAttributeDescription> AttributeDescriptions = new List<VkVertexInputAttributeDescription>();

            fixed (VkVertexInputBindingDescription* bindingDescription = bindingDescriptionList.ToArray())
            fixed (VkVertexInputAttributeDescription* AttributeDescription = AttributeDescriptions.ToArray())
            {
                vertexInputInfo = new VkPipelineVertexInputStateCreateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_VERTEX_INPUT_STATE_CREATE_INFO
                };
            }

            VkPipelineInputAssemblyStateCreateInfo inputAssembly = new VkPipelineInputAssemblyStateCreateInfo
            {
                topology = PrimitiveTopology.TriangleList
            };

            List<VkViewport> viewportList = new List<VkViewport>()
            {
                new VkViewport
                {
                    x = 0.0f,
                    y = 0.0f,
                    width = (float)RenderPassResolution.x,
                    height = (float)RenderPassResolution.y,
                    minDepth = 0.0f,
                    maxDepth = 1.0f
                }
            };

            List<VkRect2D> rect2DList = new List<VkRect2D>()
            {
                new VkRect2D
                {
                    offset = new VkOffset2D
                    {
                        x = 0,
                        y = 0,
                    },
                    extent =
                    {
                        width = (uint)RenderPassResolution.x,
                        height = (uint)RenderPassResolution.y
                    }
                }
            };

            VkPipelineViewportStateCreateInfo viewportState = new VkPipelineViewportStateCreateInfo();
            fixed (VkViewport* viewport = viewportList.ToArray())
            fixed (VkRect2D* rect2D = rect2DList.ToArray())
            {
                viewportState = new VkPipelineViewportStateCreateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_VIEWPORT_STATE_CREATE_INFO,
                    viewportCount = 1,
                    scissorCount = 1,
                    pScissors = rect2D,
                    pViewports = viewport
                };
            }

            List<VkPipelineColorBlendAttachmentState> blendAttachmentList = new List<VkPipelineColorBlendAttachmentState>()
            {
                new VkPipelineColorBlendAttachmentState
                {
                    blendEnable = true,
                    srcColorBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_SRC_ALPHA,
                    dstColorBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_ONE_MINUS_SRC_ALPHA,
                    colorBlendOp = VkBlendOp.VK_BLEND_OP_ADD,
                    srcAlphaBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_ONE,
                    dstAlphaBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_ZERO,
                    alphaBlendOp = VkBlendOp.VK_BLEND_OP_ADD,
                    colorWriteMask = ColorComponentFlags.RBit | ColorComponentFlags.GBit | ColorComponentFlags.BBit | ColorComponentFlags.ABit
                }
            };

            VkPipelineDepthStencilStateCreateInfo blendDepthAttachment = new VkPipelineDepthStencilStateCreateInfo()
            {
                sType = StructureType.PipelineDepthStencilStateCreateInfo,
                depthTestEnable = true,
                depthWriteEnable = true,
                depthCompareOp = CompareOp.LessOrEqual,
                depthBoundsTestEnable = false,
                stencilTestEnable = false
            };

            VkPipelineRasterizationStateCreateInfo rasterizer = new VkPipelineRasterizationStateCreateInfo()
            {
                sType = StructureType.PipelineRasterizationStateCreateInfo,
                depthClampEnable = false,
                rasterizerDiscardEnable = false,
                polygonMode = PolygonMode.Fill,
                cullMode = CullModeFlags.CullModeNone,
                frontFace = FrontFace.CounterClockwise,
                depthBiasEnable = false,
                depthBiasConstantFactor = 0.0f,
                depthBiasClamp = 0.0f,
                depthBiasSlopeFactor = 0.0f,
                lineWidth = 1.0f
            };

            VkPipelineMultisampleStateCreateInfo multisampling = new VkPipelineMultisampleStateCreateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_MULTISAMPLE_STATE_CREATE_INFO,
                rasterizationSamples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT
            };

            fixed (VkPipelineColorBlendAttachmentState* attachments = blendAttachmentList.ToArray())
            {
                VkPipelineColorBlendStateCreateInfo pipelineColorBlendStateCreateInfo = new VkPipelineColorBlendStateCreateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_COLOR_BLEND_STATE_CREATE_INFO,
                    logicOpEnable = false,
                    logicOp = VkLogicOp.VK_LOGIC_OP_NO_OP,
                    attachmentCount = 1,
                    pAttachments = attachments
                };

                pipelineColorBlendStateCreateInfo.blendConstants[0] = 0.0f;
                pipelineColorBlendStateCreateInfo.blendConstants[1] = 0.0f;
                pipelineColorBlendStateCreateInfo.blendConstants[2] = 0.0f;
                pipelineColorBlendStateCreateInfo.blendConstants[3] = 0.0f;

                VkDynamicState* dynamicStates = stackalloc[] { VkDynamicState.VK_DYNAMIC_STATE_VIEWPORT, VkDynamicState.VK_DYNAMIC_STATE_SCISSOR };
                VkPipelineDynamicStateCreateInfo pipelineDynamicStateCreateInfo = new VkPipelineDynamicStateCreateInfo
                {
                    dynamicStateCount = 2,
                    pDynamicStates = dynamicStates
                };


                VkGraphicsPipelineCreateInfo graphicsPipelineCreateInfo = new VkGraphicsPipelineCreateInfo
                {
                    sType = StructureType.GraphicsPipelineCreateInfo,
                    stageCount = 2,
                    pStages = shadermoduleList,
                    pVertexInputState = &vertexInputInfo,
                    pInputAssemblyState = &inputAssembly,
                    pViewportState = &viewportState,
                    pRasterizationState = &rasterizer,
                    pMultisampleState = &multisampling,
                    pDepthStencilState = &blendDepthAttachment,
                    pColorBlendState = &pipelineColorBlendStateCreateInfo,
                    layout = pipelineLayout,
                    renderPass = renderPass
                };

                vk.CreateGraphicsPipelines(VulkanRenderer.device, new VkPipelineCache(null), 1, &graphicsPipelineCreateInfo, null, out VkPipeline shaderPipeline);
                pipeline = shaderPipeline;
            }
        }

        public VkDescriptorSetLayout CreateDescriptorSetLayout()
        {
            VkDescriptorSetLayout descriptorSetLayout = new VkDescriptorSetLayout();
            List<VkDescriptorSetLayoutBinding> layoutBindingList = new List<VkDescriptorSetLayoutBinding>()
            {
                new VkDescriptorSetLayoutBinding
                {
                    binding = 0,
                    descriptorType = DescriptorType.CombinedImageSampler,
                    descriptorCount = 1,
                    pImmutableSamplers = null,
                    stageFlags = ShaderStageFlags.FragmentBit | ShaderStageFlags.VertexBit,
                }
            };

            fixed (VkDescriptorSetLayoutBinding* ptr = layoutBindingList.ToArray())
            {
                VkDescriptorSetLayoutCreateInfo layoutInfo = new VkDescriptorSetLayoutCreateInfo()
                {
                    SType = StructureType.DescriptorSetLayoutCreateInfo,
                    BindingCount = layoutBindingList.UCount(),
                    PBindings = ptr,
                };

                vk.CreateDescriptorSetLayout(VulkanRenderer.device, &layoutInfo, null, out VkDescriptorSetLayout descriptorsetLayout);
                descriptorSetLayout = descriptorsetLayout;
                return descriptorSetLayout;
            }
        }

        public VkDescriptorSet CreateDescriptorSets()
        {

            VkDescriptorSetLayout* layouts = stackalloc VkDescriptorSetLayout[(int)VulkanRenderer.swapChain.SwapChainImageCount];

            for (int i = 0; i < (int)VulkanRenderer.swapChain.SwapChainImageCount; i++)
            {
                layouts[i] = descriptorSetLayout;
            }

            VkDescriptorSetAllocateInfo allocInfo = new
            (
                descriptorPool: descriptorPool,
                descriptorSetCount: (uint)VulkanRenderer.swapChain.SwapChainImageCount,
                pSetLayouts: layouts
            );
            vk.AllocateDescriptorSets(VulkanRenderer.device, &allocInfo, out VkDescriptorSet tempdescriptorSet);
            descriptorSet = tempdescriptorSet;
            return descriptorSet;
        }

        public void UpdateDescriptorSet(Texture texture)
        {
            VkDescriptorImageInfo ColorDescriptorImage = new VkDescriptorImageInfo
            {
                Sampler = texture.Sampler,
                ImageView = texture.View,
                ImageLayout = Silk.NET.Vulkan.ImageLayout.ReadOnlyOptimal
            };

            List<VkWriteDescriptorSet> descriptorSetList = new List<VkWriteDescriptorSet>();

            VkWriteDescriptorSet descriptorSetWrite = new VkWriteDescriptorSet
            {
                sType = StructureType.WriteDescriptorSet,
                dstSet = descriptorSet,
                dstBinding = 0,
                dstArrayElement = 0,
                descriptorCount = 1,
                descriptorType = DescriptorType.CombinedImageSampler,
                pImageInfo = &ColorDescriptorImage
            };

            descriptorSetList.Add(descriptorSetWrite);


            fixed (VkWriteDescriptorSet* ptr = descriptorSetList.ToArray())
            {
                vk.UpdateDescriptorSets(VulkanRenderer.device, (uint)descriptorSetList.UCount(), ptr, 0, null);
            }
        }

        public VkPipelineLayout CreatePipelineLayout()
        {
            VkPipelineLayout pipelineLayout = new VkPipelineLayout();

            VkDescriptorSetLayout descriptorSetLayoutPtr = descriptorSetLayout;
            VkPipelineLayoutCreateInfo pipelineLayoutInfo = new
            (
                setLayoutCount: 1,
                pSetLayouts: &descriptorSetLayoutPtr,
                pushConstantRangeCount: 0,
                pPushConstantRanges: null
            );
            vk.CreatePipelineLayout(VulkanRenderer.device, &pipelineLayoutInfo, null, out VkPipelineLayout pipelinelayout);
            return pipelinelayout;

        }

        public CommandBuffer Draw()
        {
            var commandIndex = VulkanRenderer.CommandIndex;
            var imageIndex = VulkanRenderer.ImageIndex;
            var commandBuffer = CommandBufferList[(int)commandIndex];
            VkClearValue* clearValues = stackalloc[]
            {
                new VkClearValue(new VkClearColorValue(1, 1, 0, 1))
            };

            var viewport = new Viewport(0.0f, 0.0f, VulkanRenderer.swapChain.SwapChainResolution.width, VulkanRenderer.swapChain.SwapChainResolution.height, 0.0f, 1.0f);
            var scissor = new VkRect2D(new VkOffset2D(0, 0), VulkanRenderer.swapChain.SwapChainResolution);

            VkRenderPassBeginInfo renderPassInfo = new VkRenderPassBeginInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_DEVICE_GROUP_RENDER_PASS_BEGIN_INFO,
                renderPass = renderPass,
                renderArea = new VkRect2D(new VkOffset2D(0, 0), VulkanRenderer.swapChain.SwapChainResolution),
                clearValueCount = 1,
                framebuffer = FrameBufferList[imageIndex],
                pClearValues = clearValues,
                pNext = null
            };

            var commandInfo = new VkCommandBufferBeginInfo(flags: 0);
            Vulkan.vkBeginCommandBuffer(commandBuffer, &commandInfo);
            Vulkan.vkCmdBeginRenderPass(commandBuffer, &renderPassInfo, SubpassContents.Inline);
            Vulkan.vkCmdSetViewport(commandBuffer, 0, 1, &viewport);
            Vulkan.vkCmdSetScissor(commandBuffer, 0, 1, &scissor);
            Vulkan.vkCmdBindPipeline(commandBuffer, PipelineBindPoint.Graphics, pipeline);
            Vulkan.vkCmdBindDescriptorSets(commandBuffer, PipelineBindPoint.Graphics, pipelineLayout, 0, 1, descriptorSet, 0, null);
            Vulkan.vkCmdDraw(commandBuffer, 6, 1, 0, 0);
            Vulkan.vkCmdEndRenderPass(commandBuffer);
            Vulkan.vkEndCommandBuffer(commandBuffer);

            return commandBuffer;
        }
    }
}