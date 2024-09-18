#include "RenderPass2D.h"
#include <CVulkanRenderer.h>
#include "ShaderCompiler.h"

RenderPass2D::RenderPass2D() : RenderPass()
{
}

RenderPass2D::~RenderPass2D()
{
}

void RenderPass2D::BuildRenderPass(std::shared_ptr<Mesh2D> mesh)
{
    RenderedTexture = std::make_shared<RenderedColorTexture>(RenderedColorTexture(RenderPassResolution, VK_FORMAT_R32G32B32A32_SFLOAT));

    std::vector<VkAttachmentDescription> attachmentDescriptionList
    {
        VkAttachmentDescription
        {
            .format = RenderedTexture->GetTextureByteFormat(),
            .samples = VK_SAMPLE_COUNT_1_BIT,
            .loadOp = VK_ATTACHMENT_LOAD_OP_CLEAR,
            .storeOp = VK_ATTACHMENT_STORE_OP_STORE,
            .stencilLoadOp = VK_ATTACHMENT_LOAD_OP_DONT_CARE,
            .stencilStoreOp = VK_ATTACHMENT_STORE_OP_DONT_CARE,
            .initialLayout = VK_IMAGE_LAYOUT_UNDEFINED,
            .finalLayout = VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL
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

    RenderPassCreateInfoStruct renderPassCreateInfo
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
    VULKAN_RESULT(VulkanRenderer::CreateRenderPass(renderPassCreateInfo));

    for (size_t x = 0; x < renderer.SwapChain.SwapChainImageCount; x++)
    {
        std::vector<VkImageView> TextureAttachmentList;
        TextureAttachmentList.emplace_back(RenderedTexture->View);

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
        VULKAN_RESULT(vkCreateFramebuffer(renderer.Device, &framebufferInfo, nullptr, &FrameBufferList[x]));
    }
    BuildRenderPipeline(mesh);
}

void RenderPass2D::BuildRenderPipeline(std::shared_ptr<Mesh2D> mesh)
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
        .maxSets = renderer.SwapChain.SwapChainImageCount,
        .poolSizeCount = static_cast<uint32>(DescriptorPoolBinding.size()),
        .pPoolSizes = DescriptorPoolBinding.data(),
    };
    VULKAN_RESULT(VulkanRenderer::CreateDescriptorPool(DescriptorPool, poolInfo));

    std::vector<VkDescriptorSetLayoutBinding> LayoutBindingList =
    {
        VkDescriptorSetLayoutBinding
        {
            0,
            VK_DESCRIPTOR_TYPE_STORAGE_BUFFER,
            1,
            VK_SHADER_STAGE_VERTEX_BIT | VK_SHADER_STAGE_FRAGMENT_BIT,
            nullptr
        },
        VkDescriptorSetLayoutBinding
        {
            1,
            VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER,
            1,
            VK_SHADER_STAGE_FRAGMENT_BIT,
            nullptr
        },
    };

    VkDescriptorSetLayoutCreateInfo layoutInfo
    {
        .sType = VK_STRUCTURE_TYPE_DESCRIPTOR_SET_LAYOUT_CREATE_INFO,
        .bindingCount = static_cast<uint32>(LayoutBindingList.size()),
        .pBindings = LayoutBindingList.data(),
    };
    VULKAN_RESULT(VulkanRenderer::CreateDescriptorSetLayout(DescriptorSetLayout, layoutInfo));

    VkDescriptorSetAllocateInfo allocInfo =
    {
         .sType = VK_STRUCTURE_TYPE_DESCRIPTOR_SET_ALLOCATE_INFO,
        .descriptorPool = DescriptorPool,
        .descriptorSetCount = 1,
        .pSetLayouts = &DescriptorSetLayout
    };
    VULKAN_RESULT(VulkanRenderer::AllocateDescriptorSets(DescriptorSet, allocInfo));

    for (size_t x = 0; x < renderer.SwapChain.SwapChainImageCount; x++)
    {
        std::vector<VkWriteDescriptorSet> descriptorSets
        {
            CreateStorageDescriptorSet(mesh, 0),
            CreateTextureDescriptorSet(RenderedTexture, 1)
        };
        VulkanRenderer::UpdateDescriptorSet(descriptorSets);
    }

    std::vector<VkVertexInputBindingDescription> bindingDescriptionList
    {
        Vertex2D::getBindingDescriptions()
    };
    std::vector<VkVertexInputAttributeDescription> AttributeDescriptions
    {
        Vertex2D::getAttributeDescriptions()
    };

    VkPipelineVertexInputStateCreateInfo vertexInputInfo{};
    vertexInputInfo.sType = VK_STRUCTURE_TYPE_PIPELINE_VERTEX_INPUT_STATE_CREATE_INFO;
    vertexInputInfo.vertexBindingDescriptionCount = static_cast<uint32>(bindingDescriptionList.size());
    vertexInputInfo.pVertexBindingDescriptions = bindingDescriptionList.data();
    vertexInputInfo.vertexAttributeDescriptionCount = static_cast<uint32>(AttributeDescriptions.size());
    vertexInputInfo.pVertexAttributeDescriptions = AttributeDescriptions.data();

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

    VkPushConstantRange pushConstantRange
    {
        .stageFlags = VK_SHADER_STAGE_VERTEX_BIT | VK_SHADER_STAGE_FRAGMENT_BIT,
        .offset = 0,
        .size = sizeof(SceneDataBuffer)
    };

    VkPipelineLayoutCreateInfo pipelineLayoutInfo
    {
        .sType = VK_STRUCTURE_TYPE_PIPELINE_LAYOUT_CREATE_INFO,
        .setLayoutCount = 1,
        .pSetLayouts = &DescriptorSetLayout,
    };
    pipelineLayoutInfo.pushConstantRangeCount = 1;
    pipelineLayoutInfo.pPushConstantRanges = &pushConstantRange;
    VULKAN_RESULT(VulkanRenderer::CreatePipelineLayout(ShaderPipelineLayout, pipelineLayoutInfo));

    std::vector<VkPipelineShaderStageCreateInfo> PipelineShaderStageList
    {
        ShaderCompiler::CreateShader("C:/Users/dotha/Documents/GitHub/2D-Game-Engine/Shaders/Shader2DVert.spv", VK_SHADER_STAGE_VERTEX_BIT),
        ShaderCompiler::CreateShader("C:/Users/dotha/Documents/GitHub/2D-Game-Engine/Shaders/Shader2DFrag.spv", VK_SHADER_STAGE_FRAGMENT_BIT)
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
    VULKAN_RESULT(VulkanRenderer::CreateGraphicsPipelines(ShaderPipeline, pipelineInfo));

    for (auto& shader : PipelineShaderStageList)
    {
        vkDestroyShaderModule(renderer.Device, shader.module, nullptr);
    }
}

void RenderPass2D::UpdateRenderPass(std::shared_ptr<Mesh2D> mesh)
{
    VulkanRenderer::DestroyFrameBuffers(FrameBufferList);
    VulkanRenderer::DestroyRenderPass(RenderPassPtr);
    VulkanRenderer::DestroyPipeline(ShaderPipeline);
    VulkanRenderer::DestroyPipelineLayout(ShaderPipelineLayout);
    VulkanRenderer::DestroyPipelineCache(PipelineCache);
    VulkanRenderer::DestroyDescriptorSetLayout(DescriptorSetLayout);
    VulkanRenderer::DestroyDescriptorPool(DescriptorPool);

    RenderPassResolution = glm::ivec2((int)renderer.SwapChain.SwapChainResolution.width, (int)renderer.SwapChain.SwapChainResolution.height);
    SampleCount = VK_SAMPLE_COUNT_1_BIT;
    BuildRenderPass(mesh);
}

VkCommandBuffer RenderPass2D::Draw(std::shared_ptr<Mesh2D> mesh, SceneDataBuffer& sceneProperties)
{
    std::vector<VkClearValue> clearValues
    {
        VkClearValue{.color = { {0.0f, 0.0f, 0.0f, 1.0f} } }
    };

    VkRenderPassBeginInfo renderPassInfo
    {
        .sType = VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
        .renderPass = RenderPassPtr,
        .framebuffer = FrameBufferList[renderer.ImageIndex],
        .renderArea
        {
            .offset = {0, 0},
            .extent =
            {
                static_cast<uint32>(renderer.SwapChain.SwapChainResolution.width),
                static_cast<uint32>(renderer.SwapChain.SwapChainResolution.height)
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

    VULKAN_RESULT(vkBeginCommandBuffer(CommandBufferList[renderer.CommandIndex], &CommandBufferBeginInfo));
    vkCmdBeginRenderPass(CommandBufferList[renderer.CommandIndex], &renderPassInfo, VK_SUBPASS_CONTENTS_INLINE);
    mesh->Draw(CommandBufferList[renderer.CommandIndex], ShaderPipeline, ShaderPipelineLayout, DescriptorSet, sceneProperties);
    vkCmdEndRenderPass(CommandBufferList[renderer.CommandIndex]);
    VULKAN_RESULT(VulkanRenderer::EndCommandBuffer(&CommandBufferList[renderer.CommandIndex]));
    return CommandBufferList[renderer.CommandIndex];
}

void RenderPass2D::Destroy()
{
    RenderPass::Destroy();
    VulkanRenderer::DestroyPipeline(ShaderPipeline);
    VulkanRenderer::DestroyPipelineLayout(ShaderPipelineLayout);
    VulkanRenderer::DestroyPipelineCache(PipelineCache);
    VulkanRenderer::DestroyDescriptorSetLayout(DescriptorSetLayout);
    VulkanRenderer::DestroyDescriptorPool(DescriptorPool);
}
