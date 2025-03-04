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
            RenderPassResolution = new ivec2((int)VulkanRenderer.SwapChain.SwapChainResolution.width, (int)VulkanRenderer.SwapChain.SwapChainResolution.height);
            SampleCount = SampleCountFlags.Count1Bit;

            CommandBufferList = new VkCommandBuffer[(int)VulkanRenderer.SwapChain.ImageCount];
            FrameBufferList = new VkFramebuffer[(int)VulkanRenderer.SwapChain.ImageCount];

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
                    samples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT,
                    loadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_CLEAR,
                    storeOp = VkAttachmentStoreOp.VK_ATTACHMENT_STORE_OP_STORE,
                    stencilLoadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_DONT_CARE,
                    stencilStoreOp = VkAttachmentStoreOp.VK_ATTACHMENT_STORE_OP_DONT_CARE,
                    initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED,
                    finalLayout = VkImageLayout.VK_IMAGE_LAYOUT_PRESENT_SRC_KHR,
                }
            };

            VkImageLayout initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;
            VkImageLayout finalLayout = VkImageLayout.VK_IMAGE_LAYOUT_PRESENT_SRC_KHR;
            uint sdf = (uint)VkImageLayout.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL;
            int wer = (int)VkImageLayout.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL;


            List<VkAttachmentReference> colorRefsList = new List<VkAttachmentReference>()
            {
                new VkAttachmentReference
                {
                    attachment = 0,
                    layout = VkImageLayout.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL
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
                    srcStageMask = VkPipelineStageFlagBits.COLOR_ATTACHMENT_OUTPUT_BIT,
                    dstStageMask = VkPipelineStageFlagBits.COLOR_ATTACHMENT_OUTPUT_BIT,
                    srcAccessMask = 0,
                    dstAccessMask = VkAccessFlagBits.VK_ACCESS_COLOR_ATTACHMENT_WRITE_BIT,
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

                VkFunc.vkCreateRenderPass(VulkanRenderer.device, &renderPassCreateInfo, null, &tempRenderPass);
                renderPass = tempRenderPass;
            }

            return renderPass;
        }

        public VkFramebuffer[] CreateFramebuffer()
        {
            VkFramebuffer[] frameBufferList = new VkFramebuffer[(int)VulkanRenderer.SwapChain.ImageCount];
            for (int x = 0; x < (int)VulkanRenderer.SwapChain.ImageCount; x++)
            {
                List<VkImageView> TextureAttachmentList = new List<VkImageView>();
                TextureAttachmentList.Add(VulkanRenderer.SwapChain.imageViews[x]);

                fixed (VkImageView* imageViewPtr = TextureAttachmentList.ToArray())
                {
                    VkFramebufferCreateInfo framebufferInfo = new VkFramebufferCreateInfo()
                    {
                        sType = VkStructureType.VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO,
                        renderPass = renderPass,
                        attachmentCount = TextureAttachmentList.UCount(),
                        pAttachments = imageViewPtr,
                        width = VulkanRenderer.SwapChain.SwapChainResolution.width,
                        height = VulkanRenderer.SwapChain.SwapChainResolution.height,
                        layers = 1
                    };

                    VkFramebuffer frameBuffer = FrameBufferList[x];
                    VkFunc.vkCreateFramebuffer(VulkanRenderer.device, &framebufferInfo, null, &frameBuffer);
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
                    descriptorCount = VulkanRenderer.SwapChain.ImageCount
                };
            };

            fixed (VkDescriptorPoolSize* ptr = DescriptorPoolBinding.ToArray())
            {
                VkDescriptorPoolCreateInfo poolInfo = new VkDescriptorPoolCreateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_DESCRIPTOR_POOL_CREATE_INFO,
                    maxSets = VulkanRenderer.SwapChain.ImageCount,
                    poolSizeCount = (uint)DescriptorPoolBinding.Count,
                    pPoolSizes = ptr
                };
                VkFunc.vkCreateDescriptorPool(VulkanRenderer.device, in poolInfo, null, out tempDescriptorPool);
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

            var vertexShaderModule = VulkanRenderer.CreateShader("C:/Users/dotha/Documents/GitHub/VulkanGameEngine/Shaders/FrameBufferShaderVert.spv", VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT);
            var fragmentShaderModule = VulkanRenderer.CreateShader("C:/Users/dotha/Documents/GitHub/VulkanGameEngine/Shaders/FrameBufferShaderFrag.spv", VkShaderStageFlagBits.VK_SHADER_STAGE_FRAGMENT_BIT);
            VkPipelineShaderStageCreateInfo* shadermoduleList = stackalloc[]
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
                topology = VkPrimitiveTopology.VK_PRIMITIVE_TOPOLOGY_TRIANGLE_LIST
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
                    extent =new VkExtent2D
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
                    colorWriteMask = VkColorComponentFlagBits.VK_COLOR_COMPONENT_R_BIT | VkColorComponentFlagBits.VK_COLOR_COMPONENT_G_BIT | VkColorComponentFlagBits.VK_COLOR_COMPONENT_B_BIT | VkColorComponentFlagBits.VK_COLOR_COMPONENT_A_BIT
                }
            };

            VkPipelineDepthStencilStateCreateInfo blendDepthAttachment = new VkPipelineDepthStencilStateCreateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_DEPTH_STENCIL_STATE_CREATE_INFO,
                depthTestEnable = true,
                depthWriteEnable = true,
                depthCompareOp = VkCompareOp.VK_COMPARE_OP_LESS_OR_EQUAL,
                depthBoundsTestEnable = false,
                stencilTestEnable = false
            };

            VkPipelineRasterizationStateCreateInfo rasterizer = new VkPipelineRasterizationStateCreateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_RASTERIZATION_STATE_CREATE_INFO,
                depthClampEnable = false,
                rasterizerDiscardEnable = false,
                polygonMode = VkPolygonMode.VK_POLYGON_MODE_FILL,
                cullMode = VkCullModeFlagBits.VK_CULL_MODE_NONE,
                frontFace = VkFrontFace.VK_FRONT_FACE_COUNTER_CLOCKWISE,
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

                //pipelineColorBlendStateCreateInfo.blendConstants[0] = 0.0f;
                //pipelineColorBlendStateCreateInfo.blendConstants[1] = 0.0f;
                //pipelineColorBlendStateCreateInfo.blendConstants[2] = 0.0f;
                //pipelineColorBlendStateCreateInfo.blendConstants[3] = 0.0f;

                VkDynamicState* dynamicStates = stackalloc[] { VkDynamicState.VK_DYNAMIC_STATE_VIEWPORT, VkDynamicState.VK_DYNAMIC_STATE_SCISSOR };
                VkPipelineDynamicStateCreateInfo pipelineDynamicStateCreateInfo = new VkPipelineDynamicStateCreateInfo
                {
                    dynamicStateCount = 2,
                    pDynamicStates = dynamicStates
                };


                VkGraphicsPipelineCreateInfo graphicsPipelineCreateInfo = new VkGraphicsPipelineCreateInfo
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_GRAPHICS_PIPELINE_CREATE_INFO,
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

                VkFunc.vkCreateGraphicsPipelines(VulkanRenderer.device, null, 1, &graphicsPipelineCreateInfo, null, out VkPipeline shaderPipeline);
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
                    descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER,
                    descriptorCount = 1,
                    pImmutableSamplers = null,
                    stageFlags = VkShaderStageFlagBits.VK_SHADER_STAGE_FRAGMENT_BIT | VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT,
                }
            };

            fixed (VkDescriptorSetLayoutBinding* ptr = layoutBindingList.ToArray())
            {
                VkDescriptorSetLayoutCreateInfo layoutInfo = new VkDescriptorSetLayoutCreateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_DESCRIPTOR_SET_LAYOUT_CREATE_INFO,
                    bindingCount = layoutBindingList.UCount(),
                    pBindings = ptr,
                };

                VkFunc.vkCreateDescriptorSetLayout(VulkanRenderer.device, &layoutInfo, null, out VkDescriptorSetLayout descriptorsetLayout);
                descriptorSetLayout = descriptorsetLayout;
                return descriptorSetLayout;
            }
        }

        public VkDescriptorSet CreateDescriptorSets()
        {

            VkDescriptorSetLayout* layouts = stackalloc VkDescriptorSetLayout[(int)VulkanRenderer.SwapChain.ImageCount];

            for (int i = 0; i < (int)VulkanRenderer.SwapChain.ImageCount; i++)
            {
                layouts[i] = descriptorSetLayout;
            }

            VkDescriptorSetAllocateInfo allocInfo = new VkDescriptorSetAllocateInfo
            {
                descriptorPool = descriptorPool,
                descriptorSetCount = (uint)VulkanRenderer.SwapChain.ImageCount,
                pSetLayouts = layouts
            };
            VkFunc.vkAllocateDescriptorSets(VulkanRenderer.device, &allocInfo, out VkDescriptorSet tempdescriptorSet);
            descriptorSet = tempdescriptorSet;
            return descriptorSet;
        }

        public void UpdateDescriptorSet(Texture texture)
        {
            VkDescriptorImageInfo ColorDescriptorImage = new VkDescriptorImageInfo
            {
                sampler = texture.Sampler,
                imageView = texture.View,
                imageLayout = VkImageLayout.VK_IMAGE_LAYOUT_DEPTH_READ_ONLY_OPTIMAL
            };

            List<VkWriteDescriptorSet> descriptorSetList = new List<VkWriteDescriptorSet>();

            VkWriteDescriptorSet descriptorSetWrite = new VkWriteDescriptorSet
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET,
                dstSet = descriptorSet,
                dstBinding = 0,
                dstArrayElement = 0,
                descriptorCount = 1,
                descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER,
                pImageInfo = &ColorDescriptorImage
            };

            descriptorSetList.Add(descriptorSetWrite);


            fixed (VkWriteDescriptorSet* ptr = descriptorSetList.ToArray())
            {
                VkFunc.vkUpdateDescriptorSets(VulkanRenderer.device, (uint)descriptorSetList.UCount(), ptr, 0, null);
            }
        }

        public VkPipelineLayout CreatePipelineLayout()
        {
            VkPipelineLayout pipelineLayout = new VkPipelineLayout();

            VkDescriptorSetLayout descriptorSetLayoutPtr = descriptorSetLayout;
            VkPipelineLayoutCreateInfo pipelineLayoutInfo = new VkPipelineLayoutCreateInfo
            {
                setLayoutCount = 1,
                pSetLayouts = &descriptorSetLayoutPtr,
                pushConstantRangeCount = 0,
                pPushConstantRanges = null
            };
            VkFunc.vkCreatePipelineLayout(VulkanRenderer.device, &pipelineLayoutInfo, null, out VkPipelineLayout pipelinelayout);
            return pipelinelayout;

        }

        public VkCommandBuffer Draw()
        {
            var commandIndex = VulkanRenderer.CommandIndex;
            var imageIndex = VulkanRenderer.ImageIndex;
            var commandBuffer = CommandBufferList[(int)commandIndex];
            VkClearValue[] clearValues = new VkClearValue[]
            {
                new VkClearValue
                {
                    color = new VkClearColorValue(1, 1, 0, 1)
                }
            };

            var viewport = new VkViewport
            {
                x = 0.0f,
                y = 0.0f,
                width = VulkanRenderer.SwapChain.SwapChainResolution.width,
                height = VulkanRenderer.SwapChain.SwapChainResolution.height,
                minDepth = 0.0f,
                maxDepth = 1.0f
            };
            var scissor = new VkRect2D(new VkOffset2D(0, 0), VulkanRenderer.SwapChain.SwapChainResolution);

            fixed (VkClearValue* pClearValue = clearValues.ToArray())
            {
                VkRenderPassBeginInfo renderPassInfo = new VkRenderPassBeginInfo
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_DEVICE_GROUP_RENDER_PASS_BEGIN_INFO,
                    renderPass = renderPass,
                    renderArea = new VkRect2D(new VkOffset2D(0, 0), VulkanRenderer.SwapChain.SwapChainResolution),
                    clearValueCount = 1,
                    framebuffer = FrameBufferList[imageIndex],
                    pClearValues = pClearValue,
                    pNext = null
                };

                var commandInfo = new VkCommandBufferBeginInfo { flags = 0 };
                VkFunc.vkBeginCommandBuffer(commandBuffer, &commandInfo);
                VkFunc.vkCmdBeginRenderPass(commandBuffer, &renderPassInfo, VkSubpassContents.VK_SUBPASS_CONTENTS_INLINE);
                VkFunc.vkCmdSetViewport(commandBuffer, 0, 1, &viewport);
                VkFunc.vkCmdSetScissor(commandBuffer, 0, 1, &scissor);
                VkFunc.vkCmdBindPipeline(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, pipeline);
                VkFunc.vkCmdBindDescriptorSets(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, pipelineLayout, 0, 1, ref descriptorSet, 0, null);
                VkFunc.vkCmdDraw(commandBuffer, 6, 1, 0, 0);
                VkFunc.vkCmdEndRenderPass(commandBuffer);
                VkFunc.vkEndCommandBuffer(commandBuffer);

                return commandBuffer;
            }
        }
    }
}