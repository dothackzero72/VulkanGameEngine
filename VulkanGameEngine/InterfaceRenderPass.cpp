#include "InterfaceRenderPass.h"

VkRenderPass InterfaceRenderPass::RenderPass = VK_NULL_HANDLE;
VkDescriptorPool InterfaceRenderPass::ImGuiDescriptorPool = VK_NULL_HANDLE;
std::vector<VkFramebuffer> InterfaceRenderPass::SwapChainFramebuffers;
VkCommandBuffer InterfaceRenderPass::ImGuiCommandBuffer;
//
//   /* void InterfaceRenderPass::check_vk_result(VkResult err)
//    {
//        if (err == 0) return;
//        printf("VkResult %d\n", err);
//        if (err < 0)
//            abort();
//    }
//
//    VkResult InterfaceRenderPass::CreateRenderPass()
//    {
//        VkAttachmentDescription colorAttachment
//        {
//            .format = VK_FORMAT_B8G8R8A8_UNORM,
//            .samples = VK_SAMPLE_COUNT_1_BIT,
//            .loadOp = VK_ATTACHMENT_LOAD_OP_DONT_CARE,
//            .storeOp = VK_ATTACHMENT_STORE_OP_STORE,
//            .stencilLoadOp = VK_ATTACHMENT_LOAD_OP_DONT_CARE,
//            .stencilStoreOp = VK_ATTACHMENT_STORE_OP_DONT_CARE,
//            .initialLayout = VK_IMAGE_LAYOUT_UNDEFINED,
//            .finalLayout = VK_IMAGE_LAYOUT_PRESENT_SRC_KHR
//        };
//
//        VkAttachmentReference colorAttachmentRef
//        {
//            .attachment = 0,
//            .layout = VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL
//        };
//
//        VkSubpassDescription subpass
//        {
//            .pipelineBindPoint = VK_PIPELINE_BIND_POINT_GRAPHICS,
//            .colorAttachmentCount = 1,
//            .pColorAttachments = &colorAttachmentRef
//        };
//
//        VkSubpassDependency dependency
//        {
//            .srcSubpass = VK_SUBPASS_EXTERNAL,
//            .dstSubpass = 0,
//            .srcStageMask = VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT,
//            .dstStageMask = VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT,
//            .srcAccessMask = 0,
//            .dstAccessMask = VK_ACCESS_COLOR_ATTACHMENT_WRITE_BIT
//        };
//
//        VkRenderPassCreateInfo renderPassInfo
//        {
//            .sType = VK_STRUCTURE_TYPE_RENDER_PASS_CREATE_INFO,
//            .attachmentCount = 1,
//            .pAttachments = &colorAttachment,
//            .subpassCount = 1,
//            .pSubpasses = &subpass,
//            .dependencyCount = 1,
//            .pDependencies = &dependency
//        };
//        return vkCreateRenderPass(global.Renderer.Device, &renderPassInfo, nullptr, &RenderPass);
//    }
//
//    VkResult InterfaceRenderPass::CreateRendererFramebuffers()
//    {
//        SwapChainFramebuffers.resize(global.Renderer.SwapChain.SwapChainImageCount);
//        for (size_t x = 0; x < global.Renderer.SwapChain.SwapChainImageCount; x++)
//        {
//            std::vector<VkImageView> attachments =
//            {
//                global.Renderer.SwapChain.SwapChainImageViews[x]
//            };
//
//            VkFramebufferCreateInfo frameBufferInfo =
//            {
//                .sType = VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO,
//                .renderPass = RenderPass,
//                .attachmentCount = static_cast<uint32_t>(attachments.size()),
//                .pAttachments = attachments.data(),
//                .width = global.Renderer.SwapChain.SwapChainResolution.width,
//                .height = global.Renderer.SwapChain.SwapChainResolution.height,
//                .layers = 1
//            };
//            return vkCreateFramebuffer(global.Renderer.Device, &frameBufferInfo, nullptr, &SwapChainFramebuffers[x]);
//        }
//    }
//
//    VkResult InterfaceRenderPass::CreateCommandPool()
//    {
//        VkCommandPoolCreateInfo poolInfo
//        {
//            .sType = VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO,
//            .flags = VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT,
//            .queueFamilyIndex = global.Renderer.SwapChain.GraphicsFamily
//        };
//        return Renderer_CreateCommandPool(&ImGuiCommandPool, &poolInfo);
//    }
//
//    VkResult InterfaceRenderPass::CreateDescriptorPool()
//    {
//        VkDescriptorPoolSize poolSizes[] =
//        {
//            { VK_DESCRIPTOR_TYPE_SAMPLER, 1000 },
//            { VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER, 1000 },
//            { VK_DESCRIPTOR_TYPE_SAMPLED_IMAGE, 1000 },
//            { VK_DESCRIPTOR_TYPE_STORAGE_IMAGE, 1000 },
//            { VK_DESCRIPTOR_TYPE_UNIFORM_TEXEL_BUFFER, 1000 },
//            { VK_DESCRIPTOR_TYPE_STORAGE_TEXEL_BUFFER, 1000 },
//            { VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER, 1000 },
//            { VK_DESCRIPTOR_TYPE_STORAGE_BUFFER, 1000 },
//            { VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER_DYNAMIC, 1000 },
//            { VK_DESCRIPTOR_TYPE_STORAGE_BUFFER_DYNAMIC, 1000 },
//            { VK_DESCRIPTOR_TYPE_INPUT_ATTACHMENT, 1000 }
//        };
//
//        VkDescriptorPoolCreateInfo pool_info =
//        {
//            .sType = VK_STRUCTURE_TYPE_DESCRIPTOR_POOL_CREATE_INFO,
//            .flags = VK_DESCRIPTOR_POOL_CREATE_FREE_DESCRIPTOR_SET_BIT,
//            .maxSets = 1000 * IM_ARRAYSIZE(poolSizes),
//            .poolSizeCount = (uint32_t)IM_ARRAYSIZE(poolSizes),
//            .pPoolSizes = poolSizes
//        };
//        return Renderer_CreateDescriptorPool(&ImGuiDescriptorPool, &pool_info);
//    }
//
//    VkResult InterfaceRenderPass::AllocateCommandBuffers()
//    {
//        ImGuiCommandBuffers.resize(global.Renderer.SwapChain.SwapChainImageCount);
//        for (size_t x = 0; x < global.Renderer.SwapChain.SwapChainImageCount; x++)
//        {
//            VkCommandBufferAllocateInfo commandBufferAllocateInfo
//            {
//                .sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO,
//                .commandPool = ImGuiCommandPool,
//                .level = VK_COMMAND_BUFFER_LEVEL_PRIMARY,
//                .commandBufferCount = 1
//            };
//            VULKAN_RESULT(vkAllocateCommandBuffers(global.Renderer.Device, &commandBufferAllocateInfo, &ImGuiCommandBuffers[x]));
//        }
//        return VK_SUCCESS;
//    }
//
//
//    void InterfaceRenderPass::StartUp()
//    {
//        IMGUI_CHECKVERSION();
//        ImGui::CreateContext();
//        ImGuiIO& io = ImGui::GetIO(); (void)io;
//
//        ImGui::StyleColorsDark();
//        ImGui_ImplSDL2_InitForVulkan(global.Window.windoww);
//
//        VULKAN_RESULT(CreateRenderPass());
//        VULKAN_RESULT(CreateRendererFramebuffers());
//        VULKAN_RESULT(CreateCommandPool());
//        VULKAN_RESULT(CreateDescriptorPool());
//        VULKAN_RESULT(AllocateCommandBuffers());
//
//        ImGui_ImplVulkan_InitInfo init_info =
//        {
//            .Instance = global.Renderer.Instance,
//            .PhysicalDevice = global.Renderer.PhysicalDevice,
//            .Device = global.Renderer.Device,
//            .QueueFamily = global.Renderer.SwapChain.GraphicsFamily,
//            .Queue = global.Renderer.SwapChain.GraphicsQueue,
//            .DescriptorPool = ImGuiDescriptorPool,
//            .RenderPass = RenderPass,
//            .MinImageCount = global.Renderer.SwapChain.SwapChainImageCount,
//            .ImageCount = global.Renderer.SwapChain.SwapChainImageCount,
//            .PipelineCache = VK_NULL_HANDLE,
//            .Allocator = nullptr,
//            .CheckVkResultFn = check_vk_result
//        };
//        ImGui_ImplVulkan_Init(&init_info);
//        ImGui_ImplVulkan_CreateFontsTexture();
//    }
//
//    VkCommandBuffer InterfaceRenderPass::Draw()
//    {
//        std::vector<VkClearValue> clearValues
//        {
//            VkClearValue {.color = { {0.0f, 0.0f, 0.0f, 1.0f} } },
//            VkClearValue {.depthStencil = { 1.0f, 0 } }
//        };
//
//        VkRenderPassBeginInfo renderPassInfo
//        {
//            .sType = VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
//            .renderPass = RenderPass,
//            .framebuffer = SwapChainFramebuffers[global.Renderer.ImageIndex],
//            .renderArea
//            {
//                .offset = { 0, 0 },
//                .extent = global.Renderer.SwapChain.SwapChainResolution,
//            },
//            .clearValueCount = static_cast<uint32_t>(clearValues.size()),
//            .pClearValues = clearValues.data()
//        };
//
//        VkCommandBufferBeginInfo beginInfo
//        {
//            .sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
//            .flags = VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT
//        };
//
//        VULKAN_RESULT(vkBeginCommandBuffer(ImGuiCommandBuffers[global.Renderer.CommandIndex], &beginInfo));
//        vkCmdBeginRenderPass(ImGuiCommandBuffers[global.Renderer.CommandIndex], &renderPassInfo, VK_SUBPASS_CONTENTS_INLINE);
//        ImGui_ImplVulkan_RenderDrawData(ImGui::GetDrawData(), ImGuiCommandBuffers[global.Renderer.CommandIndex]);
//        vkCmdEndRenderPass(ImGuiCommandBuffers[global.Renderer.CommandIndex]);
//        VULKAN_RESULT(vkEndCommandBuffer(ImGuiCommandBuffers[global.Renderer.CommandIndex]));
//
//        return ImGuiCommandBuffers[global.Renderer.CommandIndex];
//    }
//
//    void InterfaceRenderPass::RebuildSwapChain()
//    {
//        Renderer_DestroyRenderPass(&RenderPass);
//        Renderer_DestroyFrameBuffers(SwapChainFramebuffers.data());
//        CreateRenderPass();
//        CreateRendererFramebuffers();
//    }
//
//    void InterfaceRenderPass::Destroy()
//    {
//        Renderer_DestroyCommandBuffers(&ImGuiCommandPool, ImGuiCommandBuffers.data());
//        ImGui_ImplVulkan_Shutdown();
//        Renderer_DestroyDescriptorPool(&ImGuiDescriptorPool);
//        Renderer_DestroyCommnadPool(&ImGuiCommandPool);
//        Renderer_DestroyRenderPass(&RenderPass);
//        Renderer_DestroyFrameBuffers(SwapChainFramebuffers.data());
//        SwapChainFramebuffers.clear();
//        ImGui_ImplSDL2_Shutdown();
//        ImGui::DestroyContext();
//    }*/
