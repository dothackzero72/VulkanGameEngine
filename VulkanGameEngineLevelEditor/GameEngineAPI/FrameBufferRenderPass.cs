using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public class FrameBufferRenderPass : Renderpass
    {
        public void BuildRenderPass(Texture renderedTexture)
        {
            List<VkAttachmentDescription> attachmentDescriptionList = new List<VkAttachmentDescription>()
            {
                new VkAttachmentDescription
                {
                    format = VkFormat.VK_FORMAT_B8G8R8A8_UNORM,
                    samples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT,
                    loadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_CLEAR,
                    storeOp = VkAttachmentStoreOp.VK_ATTACHMENT_STORE_OP_STORE,
                    stencilLoadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_DONT_CARE,
                    stencilStoreOp = VkAttachmentStoreOp.VK_ATTACHMENT_STORE_OP_DONT_CARE,
                    initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED,
                    finalLayout = VkImageLayout.VK_IMAGE_LAYOUT_PRESENT_SRC_KHR
                }
            };

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
            {
                new VkSubpassDescription
                {
                    pipelineBindPoint = VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS,
                    colorAttachmentCount = (uint)colorRefsList.Count(),
                    pColorAttachments = Marshal.AllocHGlobal(Marshal.SizeOf<VkAttachmentReference>() * colorRefsList.Count()),
                    pResolveAttachments = Marshal.AllocHGlobal(Marshal.SizeOf<VkAttachmentReference>() * multiSampleReferenceList.Count()),
                    pDepthStencilAttachment = depthReference.ToPointer()
                };
            };

            List<VkSubpassDependency> subpassDependencyList = new List<VkSubpassDependency>()
            {
                new VkSubpassDependency
                {
                    srcSubpass = VulkanConsts.VK_SUBPASS_EXTERNAL,
                    dstSubpass = 0,
                    srcStageMask = VkPipelineStageFlags.VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT,
                    dstStageMask = VkPipelineStageFlags.VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT,
                    srcAccessMask = 0,
                    dstAccessMask = VkAccessFlags.VK_ACCESS_COLOR_ATTACHMENT_WRITE_BIT
                },
            };

            var renderPassCreateInfo = new RenderPassCreateInfoStruct()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_RENDER_PASS_CREATE_INFO,
                pNext = IntPtr.Zero, // You can expand this if needed with additional extensions
                flags = 0, // Use appropriate flags
                attachmentCount = (uint)attachmentDescriptionList.Count(),
                pAttachmentList = Marshal.AllocHGlobal(Marshal.SizeOf<VkAttachmentDescription>() * attachmentDescriptionList.Count()),
                subpassCount = (uint)subpassDescriptionList.Count(),
                pSubpassDescriptionList = Marshal.AllocHGlobal(Marshal.SizeOf<VkSubpassDescription>() * subpassDescriptionList.Count()),
                dependencyCount = (uint)subpassDependencyList.Count(),
                pSubpassDependencyList = Marshal.AllocHGlobal(Marshal.SizeOf<VkSubpassDependency>() * subpassDependencyList.Count()),
            };
            VulkanRenderer.CreateRenderPass(renderPassCreateInfo);

            Marshal.FreeHGlobal(renderPassCreateInfo.pAttachmentList);
            Marshal.FreeHGlobal(renderPassCreateInfo.pSubpassDescriptionList);
            Marshal.FreeHGlobal(renderPassCreateInfo.pSubpassDependencyList);

            for (int x = 0; x < VulkanRenderer.SwapChainImageCount; x++)
            {
                List<VkImageView> TextureAttachmentList = new List<VkImageView>();
                TextureAttachmentList.Add(VulkanRenderer.SwapChainImageViews[x]);

                VkFramebufferCreateInfo framebufferInfo = new VkFramebufferCreateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO,
                    renderPass = RenderPass,
                    attachmentCount = (uint)TextureAttachmentList.Count(),
                    pAttachments = TextureAttachmentList.ToPointer(),
                    width = VulkanRenderer.SwapChainResolution.Width,
                    height = VulkanRenderer.SwapChainResolution.Height,
                    layers = 1
                };
                VulkanRenderer.CreateFrameBuffer(Helper.GetObjectPtr(FrameBufferList[x]), framebufferInfo);
            }
            BuildRenderPipeline(renderedTexture);
        }

        public void BuildRenderPipeline(Texture renderedTexture)
        {
            List<VkDescriptorPoolSize> DescriptorPoolBinding = new List<VkDescriptorPoolSize>();
            {
                new VkDescriptorPoolSize
                {
                    type = VkDescriptorType.VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER,
                    descriptorCount = 1
                };
            };

            VkDescriptorPoolCreateInfo poolInfo = new VkDescriptorPoolCreateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_DESCRIPTOR_POOL_CREATE_INFO,
                maxSets = VulkanRenderer.SwapChainImageCount,
                poolSizeCount = (uint)DescriptorPoolBinding.Count,
                pPoolSizes = DescriptorPoolBinding.ToPointer()
            };
            VulkanRenderer.CreateDescriptorPool(DescriptorPool, poolInfo);

            List<VkDescriptorSetLayoutBinding> LayoutBindingList = new List<VkDescriptorSetLayoutBinding>()
            {
                new VkDescriptorSetLayoutBinding()
                {
                    binding = 0,
                    descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER,
                    descriptorCount = 1,
                    stageFlags = VkShaderStageFlags.VK_SHADER_STAGE_FRAGMENT_BIT,
                    pImmutableSamplers = IntPtr.Zero
                },
                new VkDescriptorSetLayoutBinding()
                {
                    binding = 1,
                    descriptorType = VkDescriptorType.VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER,
                    descriptorCount = 1,
                    stageFlags = VkShaderStageFlags.VK_SHADER_STAGE_FRAGMENT_BIT,
                    pImmutableSamplers = IntPtr.Zero
                },
            };

            VkDescriptorSetLayoutCreateInfo layoutInfo = new VkDescriptorSetLayoutCreateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_DESCRIPTOR_SET_LAYOUT_CREATE_INFO,
                bindingCount = (uint)LayoutBindingList.Count,
                pBindings = LayoutBindingList.ToPointer(),
            };
            VulkanRenderer.CreateDescriptorSetLayout(DescriptorSetLayout, layoutInfo);

            VkDescriptorSetAllocateInfo allocInfo = new VkDescriptorSetAllocateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_DESCRIPTOR_SET_ALLOCATE_INFO,
                descriptorPool = DescriptorPool,
                descriptorSetCount = 1,
                pSetLayouts = Helper.GetObjectPtr(DescriptorSetLayout)
            };
            VulkanRenderer.AllocateDescriptorSets(DescriptorSetLayout, allocInfo);

            List<VkDescriptorImageInfo> ColorDescriptorImage = new List<VkDescriptorImageInfo>()
            {
                new VkDescriptorImageInfo
                {
                    sampler = renderedTexture.Sampler,
                    imageView = renderedTexture.View,
                    imageLayout = VkImageLayout.VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL
                }
            };
            for (uint x = 0; x < VulkanRenderer.SwapChainImageCount; x++)
            {
                List<VkWriteDescriptorSet> descriptorSets = new List<VkWriteDescriptorSet>()
                {
                    new VkWriteDescriptorSet
                    {
                                sType = VkStructureType.VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET,
                                dstSet = DescriptorSet,
                                dstBinding = 0,
                                dstArrayElement = 0,
                                descriptorCount = 1,
                                descriptorType =  VkDescriptorType.VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER,
                                pImageInfo = ColorDescriptorImage.ToPointer(),
                    }
                };
                VulkanRenderer.UpdateDescriptorSet(descriptorSets);
            }

            VkPipelineVertexInputStateCreateInfo vertexInputInfo = new VkPipelineVertexInputStateCreateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_VERTEX_INPUT_STATE_CREATE_INFO
            };

            VkPipelineInputAssemblyStateCreateInfo inputAssembly = new VkPipelineInputAssemblyStateCreateInfo()
            {
                sType =   VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_INPUT_ASSEMBLY_STATE_CREATE_INFO,
                topology =  VkPrimitiveTopology.VK_PRIMITIVE_TOPOLOGY_TRIANGLE_LIST,
                primitiveRestartEnable = false
            };

            List<VkViewport> viewport = new List<VkViewport>()
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

            List<VkRect2D> rect2D = new List<VkRect2D>()
            {
                new VkRect2D
                {
                    Offset = new VkOffset2D
                    {
                        X = 0,
                        Y = 0,
                    },
                    Extent = new VkExtent2D()
                    {
                       Width =  (uint)RenderPassResolution.x,
                       Height = (uint)RenderPassResolution.y
                    }
                }
            };

            VkPipelineViewportStateCreateInfo viewportState = new VkPipelineViewportStateCreateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_VIEWPORT_STATE_CREATE_INFO,
                viewportCount = 1,
                pViewports = viewport.ToPointer(),
                scissorCount = 1,
                pScissors = rect2D.ToPointer(),
            };

            List<VkPipelineColorBlendAttachmentState> blendAttachmentList = new List<VkPipelineColorBlendAttachmentState>()
            {
                new VkPipelineColorBlendAttachmentState
                {
                    blendEnable = true,
                    srcColorBlendFactor =  VkBlendFactor.VK_BLEND_FACTOR_SRC_ALPHA,
                    dstColorBlendFactor =  VkBlendFactor.VK_BLEND_FACTOR_ONE_MINUS_SRC_ALPHA,
                    colorBlendOp =  VkBlendOp.VK_BLEND_OP_ADD,
                    srcAlphaBlendFactor =  VkBlendFactor.VK_BLEND_FACTOR_ONE,
                    dstAlphaBlendFactor =   VkBlendFactor.VK_BLEND_FACTOR_ONE_MINUS_SRC_ALPHA,
                    alphaBlendOp =  VkBlendOp.VK_BLEND_OP_ADD,
                    colorWriteMask =  VkColorComponentFlagBits.VK_COLOR_COMPONENT_R_BIT |
                                      VkColorComponentFlagBits.VK_COLOR_COMPONENT_G_BIT |
                                      VkColorComponentFlagBits.VK_COLOR_COMPONENT_B_BIT |
                                      VkColorComponentFlagBits.VK_COLOR_COMPONENT_A_BIT
                }
            };

            VkPipelineDepthStencilStateCreateInfo blendDepthAttachment = new VkPipelineDepthStencilStateCreateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_DEPTH_STENCIL_STATE_CREATE_INFO,
                depthTestEnable = true,
                depthWriteEnable = true,
                depthCompareOp =  VkCompareOp.VK_COMPARE_OP_LESS,
                depthBoundsTestEnable = false,
                stencilTestEnable = false
            };

            VkPipelineRasterizationStateCreateInfo rasterizer = new VkPipelineRasterizationStateCreateInfo()
            {
                sType =  VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_RASTERIZATION_STATE_CREATE_INFO,
                depthClampEnable =  false,
                rasterizerDiscardEnable = false,
                polygonMode =  VkPolygonMode.VK_POLYGON_MODE_FILL,
                cullMode =  VkCullModeFlags.VK_CULL_MODE_NONE,
                frontFace =  VkFrontFace.VK_FRONT_FACE_COUNTER_CLOCKWISE,
                depthBiasEnable = false,
                lineWidth = 1.0f
            };

            VkPipelineMultisampleStateCreateInfo multisampling = new VkPipelineMultisampleStateCreateInfo()
            {
                sType =  VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_MULTISAMPLE_STATE_CREATE_INFO,
                rasterizationSamples =  VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT
            };

            VkPipelineColorBlendStateCreateInfo colorBlending = new VkPipelineColorBlendStateCreateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_COLOR_BLEND_STATE_CREATE_INFO,
                attachmentCount = (uint)blendAttachmentList.Count(),
                pAttachments = blendAttachmentList.ToPointer()
            };

            VkPipelineLayoutCreateInfo pipelineLayoutInfo = new VkPipelineLayoutCreateInfo()
            {
                sType =  VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_LAYOUT_CREATE_INFO,
                setLayoutCount = 1,
                pSetLayouts = DescriptorSetLayout
            };
            VulkanRenderer.CreatePipelineLayout(ShaderPipelineLayout, pipelineLayoutInfo);

            List<VkPipelineShaderStageCreateInfo> PipelineShaderStageList = new List<VkPipelineShaderStageCreateInfo>()
            {
                VulkanRenderer.CreateShader("C:/Users/dotha/Documents/GitHub/2D-Game-Engine/Shaders/FrameBufferShaderVert.spv",  VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT),
                VulkanRenderer.CreateShader("C:/Users/dotha/Documents/GitHub/2D-Game-Engine/Shaders/FrameBufferShaderFrag.spv", VkShaderStageFlagBits.VK_SHADER_STAGE_FRAGMENT_BIT)
            };
            List<VkGraphicsPipelineCreateInfo> pipelineInfo = new List<VkGraphicsPipelineCreateInfo>()
            {
                new VkGraphicsPipelineCreateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_GRAPHICS_PIPELINE_CREATE_INFO,
                    stageCount = (uint)PipelineShaderStageList.Count(),
                    pStages = PipelineShaderStageList.ToPointer(),
                    pVertexInputState = Helper.GetObjectPtr(vertexInputInfo),
                    pInputAssemblyState = Helper.GetObjectPtr(inputAssembly),
                    pViewportState = Helper.GetObjectPtr(viewportState),
                    pRasterizationState = Helper.GetObjectPtr(rasterizer),
                    pMultisampleState = Helper.GetObjectPtr(multisampling),
                    pDepthStencilState = Helper.GetObjectPtr(blendDepthAttachment),
                    pColorBlendState = Helper.GetObjectPtr(colorBlending),
                    layout = Helper.GetObjectPtr(ShaderPipelineLayout),
                    renderPass = RenderPass,
                    subpass = 0,
                    basePipelineHandle = IntPtr.Zero,
                }
            };
            VulkanRenderer.CreateGraphicsPipelines(ShaderPipeline, pipelineInfo);
        }

        public void UpdateRenderPass(Texture texture)
        {

        }

        public VkCommandBuffer Draw()
        {
            List<VkClearValue> clearValues = new List<VkClearValue>()
            {
                new VkClearValue
                {
                    color = new VkClearColorValue(new float[] { 1.0f, 1.0f, 1.0f, 1.0f }) // Using the float constructor
                }
            };

            List<VkViewport> viewport = new List<VkViewport>()
            {
                new VkViewport
                {
                    x = 0.0f,
                    y = 0.0f,
                    width = (float)VulkanRenderer.SwapChainResolution.Width,
                    height = (float)VulkanRenderer.SwapChainResolution.Height,
                    minDepth = 0.0f,
                    maxDepth = 1.0f
                }
            };


            List<VkRect2D> rect2D = new List<VkRect2D>()
            {
                new VkRect2D
                {
                    Offset = new VkOffset2D() 
                    { 
                        X = 0,
                        Y = 0
                    },
                    Extent = new VkExtent2D()
                    {
                        Width = (uint)VulkanRenderer.SwapChainResolution.Width,
                        Height = (uint)VulkanRenderer.SwapChainResolution.Height,
                    }
                }
            };

            VkRenderPassBeginInfo renderPassInfo = new VkRenderPassBeginInfo()
            {
                sType =  VkStructureType.VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
                renderPass = RenderPass,
                framebuffer = FrameBufferList[(int)VulkanRenderer.ImageIndex],
                renderArea = new VkRect2D
                {
                    Offset = new VkOffset2D()
                    {
                        X = 0,
                        Y = 0
                    },
                    Extent = new VkExtent2D()
                    {
                        Width = (uint)VulkanRenderer.SwapChainResolution.Width,
                        Height = (uint)VulkanRenderer.SwapChainResolution.Height,
                    }
                },
                clearValueCount = (uint)clearValues.Count,
                pClearValues = clearValues.ToPointer()
            };

            VkCommandBufferBeginInfo CommandBufferBeginInfo = new VkCommandBufferBeginInfo()
            {
                sType =  VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
                flags = (uint)VkCommandBufferUsageFlagBits.VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT
            };

            int imageIndex = (int)VulkanRenderer.ImageIndex;

            VulkanAPI.vkBeginCommandBuffer(CommandBufferList[imageIndex], in CommandBufferBeginInfo);
            VulkanAPI.vkCmdBeginRenderPass(CommandBufferList[imageIndex], in renderPassInfo, VkSubpassContents.VK_SUBPASS_CONTENTS_INLINE);
            VulkanAPI.vkCmdSetViewport(CommandBufferList[imageIndex], 0, (uint)viewport.Count, viewport.ToArray());
            VulkanAPI.vkCmdSetScissor(CommandBufferList[imageIndex], 0, (uint)rect2D.Count, rect2D.ToArray());
            VulkanAPI.vkCmdBindPipeline(CommandBufferList[imageIndex], VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, ShaderPipeline);
            VulkanAPI.vkCmdBindDescriptorSets(CommandBufferList[imageIndex], VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, ShaderPipelineLayout, 0, 1, new[] { DescriptorSet }, 0, new uint[0]);
            VulkanAPI.vkCmdDraw(CommandBufferList[imageIndex], 6, 1, 0, 0);
            VulkanAPI.vkCmdEndRenderPass(CommandBufferList[imageIndex]);
            VulkanAPI.vkEndCommandBuffer(CommandBufferList[imageIndex]);
            return CommandBufferList[(int)VulkanRenderer.ImageIndex];
        }
    }
}
