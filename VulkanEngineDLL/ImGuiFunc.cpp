#include "ImGuiFunc.h"
#include "VulkanError.h"

void ImGui_StartUp(const RendererState& rendererState, VkDescriptorPool& ImGuiDescriptorPool, VkCommandBuffer& ImGuiCommandBuffer)
{
    IMGUI_CHECKVERSION();
    ImGui::CreateContext();
    ImGuiIO& io = ImGui::GetIO(); (void)io;

    ImGui::StyleColorsDark();
    switch (vulkanWindow->WindowType)
    {
        //case SDL: ImGui_ImplSDL3_InitForVulkan((SDL_Window*)vulkanWindow->WindowHandle); break;
    case GLFW: ImGui_ImplGlfw_InitForVulkan((GLFWwindow*)vulkanWindow->WindowHandle, true); break;
    }

    Vector<VkFramebuffer> frameBufferList;
    VkRenderPass renderPass = ImGui_CreateRenderPass(rendererState);
    ImGui_CreateRendererFramebuffers(rendererState, renderPass, frameBufferList);

    VkDescriptorPoolSize poolSizes[] =
    {
        { VK_DESCRIPTOR_TYPE_SAMPLER, 1000 },
        { VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER, 1000 },
        { VK_DESCRIPTOR_TYPE_SAMPLED_IMAGE, 1000 },
        { VK_DESCRIPTOR_TYPE_STORAGE_IMAGE, 1000 },
        { VK_DESCRIPTOR_TYPE_UNIFORM_TEXEL_BUFFER, 1000 },
        { VK_DESCRIPTOR_TYPE_STORAGE_TEXEL_BUFFER, 1000 },
        { VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER, 1000 },
        { VK_DESCRIPTOR_TYPE_STORAGE_BUFFER, 1000 },
        { VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER_DYNAMIC, 1000 },
        { VK_DESCRIPTOR_TYPE_STORAGE_BUFFER_DYNAMIC, 1000 },
        { VK_DESCRIPTOR_TYPE_INPUT_ATTACHMENT, 1000 }
    };
    VkDescriptorPoolCreateInfo pool_info =
    {
        .sType = VK_STRUCTURE_TYPE_DESCRIPTOR_POOL_CREATE_INFO,
        .flags = VK_DESCRIPTOR_POOL_CREATE_FREE_DESCRIPTOR_SET_BIT,
        .maxSets = 1000 * IM_ARRAYSIZE(poolSizes),
        .poolSizeCount = (uint32)IM_ARRAYSIZE(poolSizes),
        .pPoolSizes = poolSizes
    };
    VULKAN_RESULT(Renderer_CreateDescriptorPool(rendererState.Device, &ImGuiDescriptorPool, &pool_info));

    for (size_t x = 0; x < cRenderer.SwapChainImageCount; x++)
    {
        VkCommandBufferAllocateInfo commandBufferAllocateInfo
        {
            .sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO,
            .commandPool = cRenderer.CommandPool,
            .level = VK_COMMAND_BUFFER_LEVEL_PRIMARY,
            .commandBufferCount = 1
        };
        VULKAN_RESULT(vkAllocateCommandBuffers(rendererState.Device, &commandBufferAllocateInfo, &ImGuiCommandBuffer));
    }

    ImGui_ImplVulkan_InitInfo init_info =
    {
        .Instance = rendererState.Instance,
        .PhysicalDevice = rendererState.PhysicalDevice,
        .Device = rendererState.Device,
        .QueueFamily = rendererState.GraphicsFamily,
        .Queue = rendererState.GraphicsQueue,
        .DescriptorPool = ImGuiDescriptorPool,
        .RenderPass = renderPass,
        .MinImageCount = rendererState.SwapChainImageCount,
        .ImageCount = rendererState.SwapChainImageCount,
        .PipelineCache = VK_NULL_HANDLE,
        .Allocator = nullptr,
        .CheckVkResultFn = ImGui_VkResult
    };
    ImGui_ImplVulkan_Init(&init_info);
    ImGui_ImplVulkan_CreateFontsTexture();
}

VkRenderPass ImGui_CreateRenderPass(const RendererState& rendererState)
{
    VkRenderPass renderPass = VK_NULL_HANDLE;
    VkAttachmentDescription colorAttachment
    {
        .format = VK_FORMAT_R8G8B8A8_UNORM,
        .samples = VK_SAMPLE_COUNT_1_BIT,
        .loadOp = VK_ATTACHMENT_LOAD_OP_DONT_CARE,
        .storeOp = VK_ATTACHMENT_STORE_OP_STORE,
        .stencilLoadOp = VK_ATTACHMENT_LOAD_OP_DONT_CARE,
        .stencilStoreOp = VK_ATTACHMENT_STORE_OP_DONT_CARE,
        .initialLayout = VK_IMAGE_LAYOUT_UNDEFINED,
        .finalLayout = VK_IMAGE_LAYOUT_PRESENT_SRC_KHR
    };

    VkAttachmentReference colorAttachmentRef
    {
        .attachment = 0,
        .layout = VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL
    };

    VkSubpassDescription subpass
    {
        .pipelineBindPoint = VK_PIPELINE_BIND_POINT_GRAPHICS,
        .colorAttachmentCount = 1,
        .pColorAttachments = &colorAttachmentRef
    };

    VkSubpassDependency dependency
    {
        .srcSubpass = VK_SUBPASS_EXTERNAL,
        .dstSubpass = 0,
        .srcStageMask = VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT,
        .dstStageMask = VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT,
        .srcAccessMask = 0,
        .dstAccessMask = VK_ACCESS_COLOR_ATTACHMENT_WRITE_BIT
    };


    VkRenderPassCreateInfo renderPassInfo
    {
        .sType = VK_STRUCTURE_TYPE_RENDER_PASS_CREATE_INFO,
        .attachmentCount = 1,
        .pAttachments = &colorAttachment,
        .subpassCount = 1,
        .pSubpasses = &subpass,
        .dependencyCount = 1,
        .pDependencies = &dependency
    };
    VULKAN_RESULT(vkCreateRenderPass(rendererState.Device, &renderPassInfo, nullptr, &renderPass));
    return renderPass;
}

void ImGui_CreateRendererFramebuffers(const RendererState& rendererState, const VkRenderPass& renderPass, Vector<VkFramebuffer>& frameBuffers)
{
    frameBuffers.resize(rendererState.SwapChainImageCount);
    for (size_t x = 0; x < rendererState.SwapChainImageCount; x++)
    {
        std::vector<VkImageView> attachments =
        {
            rendererState.SwapChainImageViews[x]
        };

        VkFramebufferCreateInfo frameBufferInfo =
        {
            .sType = VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO,
            .renderPass = renderPass,
            .attachmentCount = static_cast<uint32>(attachments.size()),
            .pAttachments = attachments.data(),
            .width = rendererState.SwapChainResolution.width,
            .height = rendererState.SwapChainResolution.height,
            .layers = 1
        };
        VULKAN_RESULT(vkCreateFramebuffer(rendererState.Device, &frameBufferInfo, nullptr, &frameBuffers[x]));
    }
}

void ImGui_VkResult(VkResult err)
{
    if (err == 0) return;
    printf("VkResult %d\n", err);
    if (err < 0)
        abort();
}
