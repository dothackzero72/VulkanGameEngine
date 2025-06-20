#include "ImGuiFunc.h"
#include "VulkanError.h"

ImGuiRenderer ImGui_StartUp(const RendererState& rendererState)
{
    ImGuiRenderer imGui;

    IMGUI_CHECKVERSION();
    ImGui::CreateContext();
    ImGuiIO& io = ImGui::GetIO(); (void)io;

    ImGui::StyleColorsDark();
    switch (vulkanWindow->WindowType)
    {
        //case SDL: ImGui_ImplSDL3_InitForVulkan((SDL_Window*)vulkanWindow->WindowHandle); break;
    case GLFW: ImGui_ImplGlfw_InitForVulkan((GLFWwindow*)vulkanWindow->WindowHandle, true); break;
    }

    imGui.RenderPass = ImGui_CreateRenderPass(rendererState);
    imGui.SwapChainFramebuffers = ImGui_CreateRendererFramebuffers(rendererState, imGui.RenderPass);

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
    VULKAN_RESULT(Renderer_CreateDescriptorPool(rendererState.Device, &imGui.ImGuiDescriptorPool, &pool_info));

    for (size_t x = 0; x < rendererState.SwapChainImageCount; x++)
    {
        VkCommandBufferAllocateInfo commandBufferAllocateInfo
        {
            .sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO,
            .commandPool = rendererState.CommandPool,
            .level = VK_COMMAND_BUFFER_LEVEL_PRIMARY,
            .commandBufferCount = 1
        };
        VULKAN_RESULT(vkAllocateCommandBuffers(rendererState.Device, &commandBufferAllocateInfo, &imGui.ImGuiCommandBuffer));
    }

    ImGui_ImplVulkan_InitInfo init_info =
    {
        .Instance = rendererState.Instance,
        .PhysicalDevice = rendererState.PhysicalDevice,
        .Device = rendererState.Device,
        .QueueFamily = rendererState.GraphicsFamily,
        .Queue = rendererState.GraphicsQueue,
        .DescriptorPool = imGui.ImGuiDescriptorPool,
        .RenderPass = imGui.RenderPass,
        .MinImageCount = static_cast<uint32>(rendererState.SwapChainImageCount),
        .ImageCount = static_cast<uint32>(rendererState.SwapChainImageCount),
        .PipelineCache = VK_NULL_HANDLE,
        .Allocator = nullptr,
        .CheckVkResultFn = ImGui_VkResult
    };
    ImGui_ImplVulkan_Init(&init_info);
    ImGui_ImplVulkan_CreateFontsTexture();

    return imGui;
}

void ImGui_StartFrame()
{
    ImGui_ImplVulkan_NewFrame();
    ImGui_ImplGlfw_NewFrame();
    ImGui::NewFrame();
    ImGui::Begin("Button Window");
    ImGui::Text("Application average %.3f ms/frame (%.1f FPS)", 1000.0f / ImGui::GetIO().Framerate, ImGui::GetIO().Framerate);
} 

void ImGui_EndFrame()
{
    ImGui::End();
    ImGui::Render();
}

VkCommandBuffer ImGui_Draw(const RendererState& rendererState, ImGuiRenderer& imGui)
{
    std::vector<VkClearValue> clearValues
    {
        VkClearValue {.color = { {0.0f, 0.0f, 0.0f, 1.0f} } },
        VkClearValue {.depthStencil = { 1.0f, 0 } }
    };

    VkRenderPassBeginInfo renderPassInfo
    {
        .sType = VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
        .renderPass = imGui.RenderPass,
        .framebuffer = imGui.SwapChainFramebuffers[rendererState.ImageIndex],
        .renderArea
        {
            .offset = { 0, 0 },
            .extent = rendererState.SwapChainResolution,
        },
        .clearValueCount = static_cast<uint32>(clearValues.size()),
        .pClearValues = clearValues.data()
    };

    VkCommandBufferBeginInfo beginInfo
    {
        .sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
        .flags = VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT
    };

    VULKAN_RESULT(vkResetCommandBuffer(imGui.ImGuiCommandBuffer, 0));
    VULKAN_RESULT(vkBeginCommandBuffer(imGui.ImGuiCommandBuffer, &beginInfo));
    vkCmdBeginRenderPass(imGui.ImGuiCommandBuffer, &renderPassInfo, VK_SUBPASS_CONTENTS_INLINE);
    ImGui_ImplVulkan_RenderDrawData(ImGui::GetDrawData(), imGui.ImGuiCommandBuffer);
    vkCmdEndRenderPass(imGui.ImGuiCommandBuffer);
    VULKAN_RESULT(vkEndCommandBuffer(imGui.ImGuiCommandBuffer));

    return imGui.ImGuiCommandBuffer;
}

void RebuildSwapChain(RendererState& rendererState, ImGuiRenderer& imGuiRenderer)
{
    Renderer_DestroyRenderPass(rendererState.Device, &imGuiRenderer.RenderPass);
    Renderer_DestroyFrameBuffers(rendererState.Device, &imGuiRenderer.SwapChainFramebuffers[0], rendererState.SwapChainImageCount);
    imGuiRenderer.RenderPass = ImGui_CreateRenderPass(rendererState);
    imGuiRenderer.SwapChainFramebuffers = ImGui_CreateRendererFramebuffers(rendererState, imGuiRenderer.RenderPass);
}

void ImGui_Destroy(RendererState& rendererState, ImGuiRenderer& imGuiRenderer)
{
    ImGui_ImplVulkan_Shutdown();
    Renderer_DestroyCommandBuffers(rendererState.Device, &rendererState.CommandPool, &imGuiRenderer.ImGuiCommandBuffer, 1);
    Renderer_DestroyDescriptorPool(rendererState.Device, &imGuiRenderer.ImGuiDescriptorPool);
    Renderer_DestroyRenderPass(rendererState.Device, &imGuiRenderer.RenderPass);
    Renderer_DestroyFrameBuffers(rendererState.Device, &imGuiRenderer.SwapChainFramebuffers[0], rendererState.SwapChainImageCount);
    switch (vulkanWindow->WindowType)
    {
        //case SDL: ImGui_ImplSDL3_Shutdown(); break;
        case GLFW: ImGui_ImplGlfw_Shutdown(); break;
    }
    ImGui::DestroyContext();
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

Vector<VkFramebuffer> ImGui_CreateRendererFramebuffers(const RendererState& rendererState, const VkRenderPass& renderPass)
{
    Vector<VkFramebuffer> frameBuffers = Vector<VkFramebuffer>(rendererState.SwapChainImageCount);
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
    return frameBuffers;
}

void ImGui_VkResult(VkResult err)
{
    if (err == 0) return;
    printf("VkResult %d\n", err);
    if (err < 0)
        abort();
}
