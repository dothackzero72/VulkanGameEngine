#pragma once
#include "VulkanRenderer.h"
#include <ImGui/imgui.h>
#include <../External/glfw/include/GLFW/glfw3.h>
#include <ImGui/backends/imgui_impl_vulkan.h>
//#include <ImGui/backends/imgui_impl_sdl3.h>
#include <ImGui/backends/imgui_impl_glfw.h>
#include <vector>
#include <VulkanWindow.h>
#include <SDLWindow.h>

class InterfaceRenderPass
{
private:
    static VkRenderPass RenderPass;
    static VkDescriptorPool ImGuiDescriptorPool;
    static VkCommandPool ImGuiCommandPool;
    static std::vector<VkFramebuffer> SwapChainFramebuffers;
   
    static void CreateRenderPass()
    {
        VkAttachmentDescription colorAttachment
        {
            .format = VK_FORMAT_B8G8R8A8_UNORM,
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
        VULKAN_RESULT(vkCreateRenderPass(renderer.Device, &renderPassInfo, nullptr, &RenderPass));
    }

    static void CreateRendererFramebuffers()
    {
        SwapChainFramebuffers.resize(renderer.SwapChain.SwapChainImageCount);
        for (size_t x = 0; x < renderer.SwapChain.SwapChainImageCount; x++) 
        {
            std::vector<VkImageView> attachments = 
            {
                renderer.SwapChain.SwapChainImageViews[x]
            };

            VkFramebufferCreateInfo frameBufferInfo =
            {
                .sType = VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO,
                .renderPass = RenderPass,
                .attachmentCount = static_cast<uint32>(attachments.size()),
                .pAttachments = attachments.data(),
                .width = renderer.SwapChain.SwapChainResolution.width,
                .height = renderer.SwapChain.SwapChainResolution.height,
                .layers = 1
            };
            VULKAN_RESULT(vkCreateFramebuffer(renderer.Device, &frameBufferInfo, nullptr, &SwapChainFramebuffers[x]));
        }
    }

    static void DestroyCommandBuffers()
    {
        Renderer_DestroyCommandBuffers(&ImGuiCommandPool, ImGuiCommandBuffers.data());
        ImGuiCommandBuffers.clear();
    }

    static void DestroyFrameBuffers()
    {
        Renderer_DestroyFrameBuffers(SwapChainFramebuffers.data());
        SwapChainFramebuffers.clear();
    }

    static void check_vk_result(VkResult err)
    {
        if (err == 0) return;
        printf("VkResult %d\n", err);
        if (err < 0)
            abort();
    }

public:
    static std::vector<VkCommandBuffer> ImGuiCommandBuffers;

    static void StartUp()
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
        
        CreateRenderPass();
        CreateRendererFramebuffers();

        VkCommandPoolCreateInfo poolInfo
        {
            .sType = VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO,
            .flags = VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT,
            .queueFamilyIndex = renderer.SwapChain.GraphicsFamily
        };
        VULKAN_RESULT(Renderer_CreateCommandPool(&ImGuiCommandPool, &poolInfo));

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
        VULKAN_RESULT(Renderer_CreateDescriptorPool(&ImGuiDescriptorPool, &pool_info));

        ImGuiCommandBuffers.resize(renderer.SwapChain.SwapChainImageCount);
        for (size_t x = 0; x < renderer.SwapChain.SwapChainImageCount; x++)
        {
            VkCommandBufferAllocateInfo commandBufferAllocateInfo
            {
                .sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO,
                .commandPool = ImGuiCommandPool,
                .level = VK_COMMAND_BUFFER_LEVEL_PRIMARY,
                .commandBufferCount = 1
            };
            VULKAN_RESULT(vkAllocateCommandBuffers(renderer.Device, &commandBufferAllocateInfo, &ImGuiCommandBuffers[x]));
        }

        ImGui_ImplVulkan_InitInfo init_info =
        {
            .Instance = renderer.Instance,
            .PhysicalDevice = renderer.PhysicalDevice,
            .Device = renderer.Device,
            .QueueFamily = renderer.SwapChain.GraphicsFamily,
            .Queue = renderer.SwapChain.GraphicsQueue,
            .DescriptorPool = ImGuiDescriptorPool,
            .RenderPass = RenderPass,
            .MinImageCount = renderer.SwapChain.SwapChainImageCount,
            .ImageCount = renderer.SwapChain.SwapChainImageCount,
            .PipelineCache = VK_NULL_HANDLE,
            .Allocator = nullptr,
            .CheckVkResultFn = check_vk_result
        };
        ImGui_ImplVulkan_Init(&init_info);
        ImGui_ImplVulkan_CreateFontsTexture();
    }

    static void TransitionImageLayout(VkCommandBuffer commandBuffer, VkImage image, VkFormat format, VkImageLayout oldLayout, VkImageLayout newLayout) {
        VkImageMemoryBarrier barrier = {};
        barrier.sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER;
        barrier.oldLayout = oldLayout;
        barrier.newLayout = newLayout;
        barrier.srcQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED;
        barrier.dstQueueFamilyIndex = VK_QUEUE_FAMILY_IGNORED;
        barrier.image = image;
        barrier.subresourceRange.aspectMask = VK_IMAGE_ASPECT_COLOR_BIT;
        barrier.subresourceRange.baseMipLevel = 0;
        barrier.subresourceRange.levelCount = 1;
        barrier.subresourceRange.baseArrayLayer = 0;
        barrier.subresourceRange.layerCount = 1;

        VkPipelineStageFlags sourceStage;
        VkPipelineStageFlags destinationStage;

        if (oldLayout == VK_IMAGE_LAYOUT_UNDEFINED && newLayout == VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL) {
            barrier.srcAccessMask = 0;
            barrier.dstAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT;
            sourceStage = VK_PIPELINE_STAGE_TOP_OF_PIPE_BIT;
            destinationStage = VK_PIPELINE_STAGE_TRANSFER_BIT;
        }
        else if (oldLayout == VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL && newLayout == VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL) {
            barrier.srcAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT;
            barrier.dstAccessMask = VK_ACCESS_SHADER_READ_BIT;
            sourceStage = VK_PIPELINE_STAGE_TRANSFER_BIT;
            destinationStage = VK_PIPELINE_STAGE_FRAGMENT_SHADER_BIT;
        }
        else {
            //throw std::invalid_argument("Unsupported layout transition!");
        }

        vkCmdPipelineBarrier(
            commandBuffer,
            sourceStage, destinationStage,
            0,
            0, nullptr,
            0, nullptr,
            1, &barrier
        );
    }

     static VkCommandBuffer Draw()
    {
         std::vector<VkClearValue> clearValues
         {
             VkClearValue {.color = { {0.0f, 0.0f, 0.0f, 1.0f} } },
             VkClearValue {.depthStencil = { 1.0f, 0 } }
         };

         VkRenderPassBeginInfo renderPassInfo
         {
             .sType = VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
             .renderPass = RenderPass,
             .framebuffer = SwapChainFramebuffers[renderer.ImageIndex],
             .renderArea
             {
                 .offset = { 0, 0 },
                 .extent = renderer.SwapChain.SwapChainResolution,
             },
             .clearValueCount = static_cast<uint32>(clearValues.size()),
             .pClearValues = clearValues.data()
         };

        VkCommandBufferBeginInfo beginInfo
        {
            .sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
            .flags = VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT
        };
        
        VULKAN_RESULT(vkBeginCommandBuffer(ImGuiCommandBuffers[renderer.CommandIndex], &beginInfo));
        vkCmdBeginRenderPass(ImGuiCommandBuffers[renderer.CommandIndex], &renderPassInfo, VK_SUBPASS_CONTENTS_INLINE);
        ImGui_ImplVulkan_RenderDrawData(ImGui::GetDrawData(), ImGuiCommandBuffers[renderer.CommandIndex]);
        vkCmdEndRenderPass(ImGuiCommandBuffers[renderer.CommandIndex]);
        VULKAN_RESULT(vkEndCommandBuffer(ImGuiCommandBuffers[renderer.CommandIndex]));
       
        return ImGuiCommandBuffers[renderer.CommandIndex];
    }

    static void RebuildSwapChain()
    {
        Renderer_DestroyRenderPass(&RenderPass);
        Renderer_DestroyFrameBuffers(SwapChainFramebuffers.data());
        CreateRenderPass();
        CreateRendererFramebuffers();
    }

    static void Destroy()
    {
        DestroyCommandBuffers();
        ImGui_ImplVulkan_Shutdown(); 
        Renderer_DestroyDescriptorPool(&ImGuiDescriptorPool);
        Renderer_DestroyCommnadPool(&ImGuiCommandPool);
        Renderer_DestroyRenderPass(&RenderPass);
        DestroyFrameBuffers();
        switch (vulkanWindow->WindowType)
        {
            //case SDL: ImGui_ImplSDL3_Shutdown(); break;
            case GLFW: ImGui_ImplGlfw_Shutdown(); break;
        }
        ImGui::DestroyContext(); 
    }
};
