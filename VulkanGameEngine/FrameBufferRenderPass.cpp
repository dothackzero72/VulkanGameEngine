#include "FrameBufferRenderPass.h"
#include <global.h>
#include "ShaderCompiler.h"
#include <stdexcept>

FrameBufferRenderPass::FrameBufferRenderPass() : RenderPass()
{
}

FrameBufferRenderPass::~FrameBufferRenderPass()
{
}

void FrameBufferRenderPass::BuildRenderPass(std::shared_ptr<Texture> texture)
{
    std::vector<VkAttachmentDescription> attachmentDescriptionList
    {
        VkAttachmentDescription
        {
            .format = VK_FORMAT_B8G8R8A8_UNORM,
            .samples = VK_SAMPLE_COUNT_1_BIT,
            .loadOp = VK_ATTACHMENT_LOAD_OP_CLEAR,
            .storeOp = VK_ATTACHMENT_STORE_OP_STORE,
            .stencilLoadOp = VK_ATTACHMENT_LOAD_OP_DONT_CARE,
            .stencilStoreOp = VK_ATTACHMENT_STORE_OP_DONT_CARE,
            .initialLayout = VK_IMAGE_LAYOUT_UNDEFINED,
            .finalLayout = VK_IMAGE_LAYOUT_PRESENT_SRC_KHR
        }
    };

    std::vector<VkAttachmentReference> colorRefsList
    {
        VkAttachmentReference
        {
            .attachment = 0,
            .layout = VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL
        }
    };

    std::vector<VkAttachmentReference> multiSampleReferenceList{};
    std::vector<VkAttachmentReference> depthReference{};

    std::vector<VkSubpassDescription> subpassDescriptionList
    {
        VkSubpassDescription
        {
            .pipelineBindPoint = VK_PIPELINE_BIND_POINT_GRAPHICS,
            .colorAttachmentCount = static_cast<uint32>(colorRefsList.size()),
            .pColorAttachments = colorRefsList.data(),
            .pResolveAttachments = multiSampleReferenceList.data(),
            .pDepthStencilAttachment = depthReference.data()
        }
    };

    std::vector<VkSubpassDependency> subpassDependencyList
    {
        VkSubpassDependency
        {
            .srcSubpass = VK_SUBPASS_EXTERNAL,
            .dstSubpass = 0,
            .srcStageMask = VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT,
            .dstStageMask = VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT,
            .srcAccessMask = 0,
            .dstAccessMask = VK_ACCESS_COLOR_ATTACHMENT_WRITE_BIT
        },
    };

    Renderer_RenderPassCreateInfoStruct renderPassCreateInfo
    {
      .pRenderPass = &RenderPassPtr,
      .pAttachmentList = attachmentDescriptionList.data(),
      .pSubpassDescriptionList = subpassDescriptionList.data(),
      .pSubpassDependencyList = subpassDependencyList.data(),
      .AttachmentCount = static_cast<uint32>(attachmentDescriptionList.size()),
      .SubpassCount = static_cast<uint32>(subpassDescriptionList.size()),
      .DependencyCount = static_cast<uint32>(subpassDependencyList.size()),
      .Width = static_cast<uint32>(RenderPassResolution.x),
      .Height = static_cast<uint32>(RenderPassResolution.y)
    };
    VULKAN_RESULT(Renderer_CreateRenderPass(&renderPassCreateInfo));

    for (size_t x = 0; x < VulkanRenderer.SwapChain.SwapChainImageCount; x++)
    {
        std::vector<VkImageView> TextureAttachmentList;
        TextureAttachmentList.emplace_back(VulkanRenderer.SwapChain.SwapChainImageViews[x]);

        VkFramebufferCreateInfo framebufferInfo
        {
            .sType = VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO,
            .renderPass = RenderPassPtr,
            .attachmentCount = static_cast<uint32>(TextureAttachmentList.size()),
            .pAttachments = TextureAttachmentList.data(),
            .width = static_cast<uint32>(RenderPassResolution.x),
            .height = static_cast<uint32>(RenderPassResolution.y),
            .layers = 1
        };
        VULKAN_RESULT(vkCreateFramebuffer(VulkanRenderer.Device, &framebufferInfo, nullptr, &FrameBufferList[x]));
    }
    BuildRenderPipeline(texture);
}

void FrameBufferRenderPass::BuildRenderPipeline(std::shared_ptr<Texture> texture)
{
    std::vector<VkDescriptorPoolSize> DescriptorPoolBinding =
    {
        VkDescriptorPoolSize
        {
            .type = VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER,
            .descriptorCount = 1
        }
    };
    VkDescriptorPoolCreateInfo poolInfo =
    {
        .sType = VK_STRUCTURE_TYPE_DESCRIPTOR_POOL_CREATE_INFO,
        .maxSets = VulkanRenderer.SwapChain.SwapChainImageCount,
        .poolSizeCount = static_cast<uint32>(DescriptorPoolBinding.size()),
        .pPoolSizes = DescriptorPoolBinding.data(),
    };
    VULKAN_RESULT(Renderer_CreateDescriptorPool(&DescriptorPool, &poolInfo));


    std::vector<VkDescriptorSetLayoutBinding> LayoutBindingList =
    {
        { 0, VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER, 1, VK_SHADER_STAGE_FRAGMENT_BIT, nullptr },
        { 1, VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER, 1, VK_SHADER_STAGE_FRAGMENT_BIT, nullptr },
    };
    VkDescriptorSetLayoutCreateInfo layoutInfo
    {
        .sType = VK_STRUCTURE_TYPE_DESCRIPTOR_SET_LAYOUT_CREATE_INFO,
        .bindingCount = static_cast<uint32>(LayoutBindingList.size()),
        .pBindings = LayoutBindingList.data(),
    };
    VULKAN_RESULT(Renderer_CreateDescriptorSetLayout(&DescriptorSetLayout, &layoutInfo));

    VkDescriptorSetAllocateInfo allocInfo =
    {
         .sType = VK_STRUCTURE_TYPE_DESCRIPTOR_SET_ALLOCATE_INFO,
        .descriptorPool = DescriptorPool,
        .descriptorSetCount = 1,
        .pSetLayouts = &DescriptorSetLayout
    };
    VULKAN_RESULT(Renderer_AllocateDescriptorSets(&DescriptorSet, &allocInfo));

    std::vector<VkDescriptorImageInfo> ColorDescriptorImage
    {
        VkDescriptorImageInfo
        {
            .sampler = texture->Sampler,
            .imageView = texture->View,
            .imageLayout = VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL
        }
    };
    for (size_t x = 0; x < VulkanRenderer.SwapChain.SwapChainImageCount; x++)
    {
        std::vector<VkWriteDescriptorSet> descriptorSets
        {
            VkWriteDescriptorSet
            {
                .sType = VK_STRUCTURE_TYPE_WRITE_DESCRIPTOR_SET,
                .dstSet = DescriptorSet,
                .dstBinding = 0,
                .dstArrayElement = 0,
                .descriptorCount = 1,
                .descriptorType = VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER,
                .pImageInfo = ColorDescriptorImage.data(),
            }
        };
        Renderer_UpdateDescriptorSet(descriptorSets.data(), static_cast<uint32_t>(descriptorSets.size()));
    }

    VkPipelineVertexInputStateCreateInfo vertexInputInfo
    {
        .sType = VK_STRUCTURE_TYPE_PIPELINE_VERTEX_INPUT_STATE_CREATE_INFO
    };

    VkPipelineInputAssemblyStateCreateInfo inputAssembly =
    {
        .sType = VK_STRUCTURE_TYPE_PIPELINE_INPUT_ASSEMBLY_STATE_CREATE_INFO,
        .topology = VK_PRIMITIVE_TOPOLOGY_TRIANGLE_LIST,
        .primitiveRestartEnable = VK_FALSE
    };

    std::vector<VkViewport> viewport
    {
        VkViewport
        {
            .x = 0.0f,
            .y = 0.0f,
            .width = (float)RenderPassResolution.x,
            .height = (float)RenderPassResolution.y,
            .minDepth = 0.0f,
            .maxDepth = 1.0f
        }
    };

    std::vector<VkRect2D> rect2D
    {
        VkRect2D
        {
            .offset = { 0, 0 },
            .extent = 
            { 
                static_cast<uint32>(RenderPassResolution.x), 
                static_cast<uint32>(RenderPassResolution.y) 
            }
        }
    };

    VkPipelineViewportStateCreateInfo viewportState
    {
        .sType = VK_STRUCTURE_TYPE_PIPELINE_VIEWPORT_STATE_CREATE_INFO,
        .viewportCount = 1,
        .pViewports = viewport.data(),
        .scissorCount = 1,
        .pScissors = rect2D.data(),
    };

    std::vector<VkPipelineColorBlendAttachmentState> blendAttachmentList
    {
        VkPipelineColorBlendAttachmentState
        {
            .blendEnable = VK_TRUE,
            .srcColorBlendFactor = VK_BLEND_FACTOR_SRC_ALPHA,
            .dstColorBlendFactor = VK_BLEND_FACTOR_ONE_MINUS_SRC_ALPHA,
            .colorBlendOp = VK_BLEND_OP_ADD,
            .srcAlphaBlendFactor = VK_BLEND_FACTOR_ONE,
            .dstAlphaBlendFactor = VK_BLEND_FACTOR_ONE_MINUS_SRC_ALPHA,
            .alphaBlendOp = VK_BLEND_OP_ADD,
            .colorWriteMask = VK_COLOR_COMPONENT_R_BIT |
                VK_COLOR_COMPONENT_G_BIT |
                VK_COLOR_COMPONENT_B_BIT |
                VK_COLOR_COMPONENT_A_BIT
        }
    };

    VkPipelineDepthStencilStateCreateInfo blendDepthAttachment
    {
        .sType = VK_STRUCTURE_TYPE_PIPELINE_DEPTH_STENCIL_STATE_CREATE_INFO,
        .depthTestEnable = VK_TRUE,
        .depthWriteEnable = VK_TRUE,
        .depthCompareOp = VK_COMPARE_OP_LESS,
        .depthBoundsTestEnable = VK_FALSE,
        .stencilTestEnable = VK_FALSE
    };

    VkPipelineRasterizationStateCreateInfo rasterizer
    {
        .sType = VK_STRUCTURE_TYPE_PIPELINE_RASTERIZATION_STATE_CREATE_INFO,
        .depthClampEnable = VK_FALSE,
        .rasterizerDiscardEnable = VK_FALSE,
        .polygonMode = VK_POLYGON_MODE_FILL,
        .cullMode = VK_CULL_MODE_NONE,
        .frontFace = VK_FRONT_FACE_COUNTER_CLOCKWISE,
        .depthBiasEnable = VK_FALSE,
        .lineWidth = 1.0f
    };

    VkPipelineMultisampleStateCreateInfo multisampling
    {
        .sType = VK_STRUCTURE_TYPE_PIPELINE_MULTISAMPLE_STATE_CREATE_INFO,
        .rasterizationSamples = VK_SAMPLE_COUNT_1_BIT
    };

    VkPipelineColorBlendStateCreateInfo colorBlending
    {
        .sType = VK_STRUCTURE_TYPE_PIPELINE_COLOR_BLEND_STATE_CREATE_INFO,
        .attachmentCount = static_cast<uint32>(blendAttachmentList.size()),
        .pAttachments = blendAttachmentList.data()
    };

    VkPipelineLayoutCreateInfo pipelineLayoutInfo
    {
        .sType = VK_STRUCTURE_TYPE_PIPELINE_LAYOUT_CREATE_INFO,
        .setLayoutCount = 1,
        .pSetLayouts = &DescriptorSetLayout
    };
    VULKAN_RESULT(Renderer_CreatePipelineLayout(&ShaderPipelineLayout, &pipelineLayoutInfo));

    std::vector<VkPipelineShaderStageCreateInfo> PipelineShaderStageList
    {
        ShaderCompiler::CreateShader("C:/Users/dotha/Documents/GitHub/2D-Game-Engine/Shaders/FrameBufferShaderVert.spv", VK_SHADER_STAGE_VERTEX_BIT),
        ShaderCompiler::CreateShader("C:/Users/dotha/Documents/GitHub/2D-Game-Engine/Shaders/FrameBufferShaderFrag.spv", VK_SHADER_STAGE_FRAGMENT_BIT)
    };
    std::vector<VkGraphicsPipelineCreateInfo> pipelineInfo
    {
        VkGraphicsPipelineCreateInfo
        {
            .sType = VK_STRUCTURE_TYPE_GRAPHICS_PIPELINE_CREATE_INFO,
            .stageCount = static_cast<uint32>(PipelineShaderStageList.size()),
            .pStages = PipelineShaderStageList.data(),
            .pVertexInputState = &vertexInputInfo,
            .pInputAssemblyState = &inputAssembly,
            .pViewportState = &viewportState,
            .pRasterizationState = &rasterizer,
            .pMultisampleState = &multisampling,
            .pDepthStencilState = &blendDepthAttachment,
            .pColorBlendState = &colorBlending,
            .layout = ShaderPipelineLayout,
            .renderPass = RenderPassPtr,
            .subpass = 0,
            .basePipelineHandle = VK_NULL_HANDLE
        }
    };
   VULKAN_RESULT(Renderer_CreateGraphicsPipelines(&ShaderPipeline, pipelineInfo.data(), static_cast<uint32>(pipelineInfo.size())));

   for (auto& shader : PipelineShaderStageList)
   {
       vkDestroyShaderModule(VulkanRenderer.Device, shader.module, nullptr);
   }
}

void FrameBufferRenderPass::UpdateRenderPass(std::shared_ptr<Texture> texture)
{
    RenderPassResolution = glm::ivec2((int)VulkanRenderer.SwapChain.SwapChainResolution.width, (int)VulkanRenderer.SwapChain.SwapChainResolution.height);
    SampleCount = VK_SAMPLE_COUNT_1_BIT;

    Renderer_DestroyFrameBuffers(FrameBufferList.data());
    Renderer_DestroyRenderPass(&RenderPassPtr);
    Renderer_DestroyPipeline(&ShaderPipeline);
    Renderer_DestroyPipelineLayout(&ShaderPipelineLayout);
    Renderer_DestroyPipelineCache(&PipelineCache);
    Renderer_DestroyDescriptorSetLayout(&DescriptorSetLayout);
    Renderer_DestroyDescriptorPool(&DescriptorPool);
    BuildRenderPass(texture);
}

VkCommandBuffer FrameBufferRenderPass::Draw()
{
    std::vector<VkClearValue> clearValues
    {
        VkClearValue{.color = { {1.0f, 1.0f, 1.0f, 1.0f} } }
    };

    std::vector<VkViewport> viewport
    {
        VkViewport
        {
            .x = 0.0f,
            .y = 0.0f,
            .width = (float)VulkanRenderer.SwapChain.SwapChainResolution.width,
            .height = (float)VulkanRenderer.SwapChain.SwapChainResolution.height,
            .minDepth = 0.0f,
            .maxDepth = 1.0f
        }
    };

    std::vector<VkRect2D> rect2D
    {
        VkRect2D
        {
            .offset = { 0, 0 },
            .extent =
            {
                static_cast<uint32>(VulkanRenderer.SwapChain.SwapChainResolution.width),
                static_cast<uint32>(VulkanRenderer.SwapChain.SwapChainResolution.height)
            }
        }
    };

    VkRenderPassBeginInfo renderPassInfo
    {
        .sType = VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
        .renderPass = RenderPassPtr,
        .framebuffer = FrameBufferList[VulkanRenderer.ImageIndex],
        .renderArea
        {
            .offset = {0, 0},
            .extent = 
            { 
                static_cast<uint32>(VulkanRenderer.SwapChain.SwapChainResolution.width),
                static_cast<uint32>(VulkanRenderer.SwapChain.SwapChainResolution.height)
            }
        },
        .clearValueCount = static_cast<uint32>(clearValues.size()),
        .pClearValues = clearValues.data()
    };

    VkCommandBufferBeginInfo CommandBufferBeginInfo
    {
        .sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
        .flags = VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT
    };

    VULKAN_RESULT(vkBeginCommandBuffer(CommandBufferList[VulkanRenderer.CommandIndex], &CommandBufferBeginInfo));
    vkCmdBeginRenderPass(CommandBufferList[VulkanRenderer.CommandIndex], &renderPassInfo, VK_SUBPASS_CONTENTS_INLINE);
    vkCmdSetViewport(CommandBufferList[VulkanRenderer.CommandIndex], 0, static_cast<uint32>(viewport.size()), viewport.data());
    vkCmdSetScissor(CommandBufferList[VulkanRenderer.CommandIndex], 0, static_cast<uint32>(rect2D.size()), rect2D.data());
    vkCmdBindPipeline(CommandBufferList[VulkanRenderer.CommandIndex], VK_PIPELINE_BIND_POINT_GRAPHICS, ShaderPipeline);
    vkCmdBindDescriptorSets(CommandBufferList[VulkanRenderer.CommandIndex], VK_PIPELINE_BIND_POINT_GRAPHICS, ShaderPipelineLayout, 0, 1, &DescriptorSet, 0, nullptr);
    vkCmdDraw(CommandBufferList[VulkanRenderer.CommandIndex], 6, 1, 0, 0);
    vkCmdEndRenderPass(CommandBufferList[VulkanRenderer.CommandIndex]);
    VULKAN_RESULT(Renderer_EndCommandBuffer(&CommandBufferList[VulkanRenderer.CommandIndex]));
    return CommandBufferList[VulkanRenderer.CommandIndex];
}

void FrameBufferRenderPass::Destroy()
{
    RenderPass::Destroy();
    Renderer_DestroyPipeline(&ShaderPipeline);
    Renderer_DestroyPipelineLayout(&ShaderPipelineLayout);
    Renderer_DestroyPipelineCache(&PipelineCache);
    Renderer_DestroyDescriptorSetLayout(&DescriptorSetLayout);
    Renderer_DestroyDescriptorPool(&DescriptorPool);
}
