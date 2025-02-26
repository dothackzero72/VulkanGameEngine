using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.EXT;
using Silk.NET.Vulkan.Extensions.KHR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.Models;

namespace VulkanGameEngineLevelEditor.Vulkan
{
    public struct SwapChainState
    {
        public uint ImageCount { get;  set; }
        public uint GraphicsFamily { get;  set; }
        public uint PresentFamily { get;  set; }
        public VkQueue GraphicsQueue { get;  set; }
        public VkQueue PresentQueue { get;  set; }
        public VkFormat Format { get;  set; }
        public VkColorSpaceKHR ColorSpace { get;  set; }
        public VkPresentModeKHR PresentMode { get;  set; }

        public VkImage[] Images { get;  set; }
        public VkImageView[] imageViews { get;  set; }
        public VkExtent2D SwapChainResolution { get;  set; }
        public VkSwapchainKHR Swapchain { get;  set; }
    }

    public unsafe static class VulkanRenderer
    {
        public static Vk vk = Vk.GetApi();
        public const int MAX_FRAMES_IN_FLIGHT = 3;
        public static IntPtr windowPtr { get; set; }
        public static VkInstance instance { get; private set; }
        public static VkDebugUtilsMessengerEXT debugMessenger { get; private set; }
        public static VkSurfaceKHR surface { get; private set; }
        public static VkPhysicalDevice physicalDevice { get; private set; }
        public static uint GraphicsFamily { get; private set; }
        public static uint PresentFamily { get; private set; }
        public static VkDevice device { get; private set; }
        public static VkQueue graphicsQueue { get; private set; }
        public static VkQueue presentQueue { get; private set; }
        public static VkCommandPool commandPool { get; private set; }
        public static VkFence[] InFlightFences { get; private set; }
        public static VkSemaphore[] AcquireImageSemaphores { get; private set; }
        public static VkSemaphore[] PresentImageSemaphores { get; private set; }
        public static SwapChainState swapChain { get; private set; } = new SwapChainState();
        public static UInt32 ImageIndex { get; private set; } = new UInt32();
        public static UInt32 CommandIndex { get; private set; } = new UInt32();
        public static bool RebuildRendererFlag { get; private set; }

        public static void CreateVulkanRenderer(IntPtr window, Extent2D swapChainResolution)
        {
            windowPtr = window;
            instance = GameEngineImport.DLL_Renderer_CreateVulkanInstance();
            debugMessenger = GameEngineImport.DLL_Renderer_SetupDebugMessenger(instance);
            CreateSurface(windowPtr);
            physicalDevice = GameEngineImport.DLL_Renderer_SetUpPhysicalDevice(instance, surface, GraphicsFamily, PresentFamily);
            device = GameEngineImport.DLL_Renderer_SetUpDevice(physicalDevice, GraphicsFamily, PresentFamily);
            CreateDeviceQueue();
            CreateSemaphores();
            CreateSwapChain(windowPtr);
            CreateCommandPool();
        }

        public static void CreateSwapChain(IntPtr window)
        {
            uint width = 0;
            uint height = 0;
            uint surfaceFormatCount = 0;
            uint presentModeCount = 0;

            VkSurfaceCapabilitiesKHR surfaceCapabilities = SwapChain_GetSurfaceCapabilities();
            VkSurfaceFormatKHR[] compatibleSwapChainFormatList = SwapChain_GetPhysicalDeviceFormats();
            SwapChain_GetQueueFamilies();
            VkPresentModeKHR[] compatiblePresentModesList = SwapChain_GetPhysicalDeviceFormats();
            VkSurfaceFormatKHR swapChainImageFormat = SwapChain_FindSwapSurfaceFormat(compatibleSwapChainFormatList);
            VkPresentModeKHR swapChainPresentMode = SwapChain_FindSwapPresentMode(compatiblePresentModesList);
            //vulkanWindow->GetFrameBufferSize(vulkanWindow, &width, &height);

            SwapChainState swapChainState = new SwapChainState
            {
                Swapchain = SwapChain_SetUpSwapChain(),
                Images = SwapChain_SetUpSwapChainImages(),
                imageViews = SwapChain_SetUpSwapChainImageViews(),
                SwapChainResolution = new VkExtent2D
                {
                    height = height,
                    width = width
                }
            };
        }

        public static void CreateCommandBuffers(VkCommandBuffer[] commandBufferList)
        {
            for (int x = 0; x < MAX_FRAMES_IN_FLIGHT; x++)
            {
                VkCommandBufferAllocateInfo commandBufferAllocateInfo = new VkCommandBufferAllocateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO,
                    commandPool = commandPool,
                    level = VkCommandBufferLevel.VK_COMMAND_BUFFER_LEVEL_PRIMARY,
                    commandBufferCount = 1
                };

                VkFunc.vkAllocateCommandBuffers(device, in commandBufferAllocateInfo, out commandBufferList[x]);
            }
        }

        public static void CreateCommandBuffers(VkCommandBuffer commandBuffer)
        {
            VkCommandBufferAllocateInfo commandBufferAllocateInfo = new VkCommandBufferAllocateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO,
                commandPool = commandPool,
                level = VkCommandBufferLevel.VK_COMMAND_BUFFER_LEVEL_PRIMARY,
                commandBufferCount = 1
            };
            VkFunc.vkAllocateCommandBuffers(device, in commandBufferAllocateInfo, out commandBuffer);
        }

        public static VkResult StartFrame()
        {
            CommandIndex = (CommandIndex + 1) % MAX_FRAMES_IN_FLIGHT;

            var fence = InFlightFences[(int)CommandIndex];
            var imageSemaphore = AcquireImageSemaphores[(int)CommandIndex];

            VkFunc.vkWaitForFences(device, 1, &fence, true, ulong.MaxValue);
            VkFunc.vkResetFences(device, 1, &fence);

            VkResult result = VkFunc.vkAcquireNextImageKHR(device, swapChain.Swapchain, ulong.MaxValue, imageSemaphore, fence, out var imageIndex);
            ImageIndex = imageIndex;

            if (result == VkResult.VK_ERROR_OUT_OF_DATE_KHR)
            {
                RebuildRendererFlag = true;
                return result;
            }

            return result;
        }

        public static unsafe VkResult EndFrame(List<VkCommandBuffer> commandBufferSubmitList)
        {
            var fence = InFlightFences[(int)CommandIndex];
            var presentSemaphore = PresentImageSemaphores[(int)CommandIndex];
            var imageSemaphore = AcquireImageSemaphores[(int)CommandIndex];

            VkFunc.vkWaitForFences(device, 1, &fence, true, ulong.MaxValue);
            VkFunc.vkResetFences(device, 1, &fence);
            InFlightFences[(int)CommandIndex] = fence;

            VkPipelineStageFlagBits[] waitStages = new VkPipelineStageFlagBits[]
            {
                VkPipelineStageFlagBits.COLOR_ATTACHMENT_OUTPUT_BIT
            };

            fixed (VkPipelineStageFlagBits* pWaitStages = waitStages)
            {
                var commandBufferCount = commandBufferSubmitList.Count;
                var commandBuffersPtr = (VkCommandBuffer*)Marshal.AllocHGlobal(commandBufferCount * sizeof(VkCommandBuffer));

                try
                {
                    for (int i = 0; i < commandBufferCount; i++)
                    {
                        commandBuffersPtr[i] = commandBufferSubmitList[i];
                    }

                    VkSubmitInfo submitInfo = new VkSubmitInfo()
                    {
                        sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_SUBMIT_INFO,
                        waitSemaphoreCount = 1,
                        pWaitSemaphores = &imageSemaphore,
                        pWaitDstStageMask = pWaitStages,
                        commandBufferCount = (uint)commandBufferSubmitList.Count,
                        pCommandBuffers = commandBuffersPtr,
                        signalSemaphoreCount = 1,
                        pSignalSemaphores = &presentSemaphore
                    };

                    VkResult submitResult = VkFunc.vkQueueSubmit(graphicsQueue, 1, &submitInfo, fence);
                    if (submitResult != VkResult.VK_SUCCESS)
                    {
                        return submitResult;
                    }

                    var imageIndex = ImageIndex;
                    var swapchain = swapChain.Swapchain;
                    VkPresentInfoKHR presentInfo = new VkPresentInfoKHR()
                    {
                        sType = VkStructureType.VK_STRUCTURE_TYPE_DEVICE_GROUP_PRESENT_INFO_KHR,
                        waitSemaphoreCount = 1,
                        pWaitSemaphores = &presentSemaphore,
                        swapchainCount = 1,
                        pSwapchains = &swapchain,
                        pImageIndices = &imageIndex
                    };

                    VkResult result = VkFunc.vkQueuePresentKHR(presentQueue, in presentInfo);
                    if (result == VkResult.VK_ERROR_OUT_OF_DATE_KHR || result == VkResult.VK_SUBOPTIMAL_KHR)
                    {
                        RebuildRendererFlag = true;
                    }

                    return result;
                }
                finally
                {
                    Marshal.FreeHGlobal((IntPtr)commandBuffersPtr);
                }
            }
        }

        public static VkPipelineShaderStageCreateInfo CreateShader(string path, VkShaderStageFlagBits shaderStage)
        {
            byte[] shaderBytes = File.ReadAllBytes(path);
            VkShaderModule shaderModule = CreateShaderModule(shaderBytes);
            IntPtr pName = Marshal.StringToHGlobalAnsi("main");
            VkPipelineShaderStageCreateInfo shaderStageInfo = new VkPipelineShaderStageCreateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_SHADER_STAGE_CREATE_INFO,
                stage = shaderStage,
                module = shaderModule,
                pName = (char*)pName,
                pNext = null,
                flags = 0
            };

            return shaderStageInfo;
        }

        public static VkShaderModule CreateShaderModule(byte[] code)
        {
            VkShaderModule shaderModule = new VkShaderModule();
            fixed (byte* codePtr = code)
            {
                var createInfo = new VkShaderModuleCreateInfo
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_SHADER_MODULE_CREATE_INFO,
                    codeSize = (nuint)code.Length,
                    pCode = (uint*)codePtr
                };

                VkResult result = VkFunc.vkCreateShaderModule(device, &createInfo, null, &shaderModule);
                if (result != VkResult.VK_SUCCESS)
                {
                    Console.WriteLine($"Failed to create shader module: {result}");
                }
            }
            return shaderModule;
        }

        public static uint GetMemoryType(uint typeFilter, VkMemoryPropertyFlagBits properties)
        {
            VkFunc.vkGetPhysicalDeviceMemoryProperties(physicalDevice, out VkPhysicalDeviceMemoryProperties memProperties);

            for (uint x = 0; x < memProperties.memoryTypeCount; x++)
            {
                if ((typeFilter & (1 << (int)x)) != 0 &&
                    (memProperties.memoryTypes[(int)x].propertyFlags & properties) == properties)
                {
                    return x;
                }
            }

            return uint.MaxValue;
        }

        public static void CreateSurface(IntPtr windowtemp)
        {
            var surfaceCreateInfo = new VkWin32SurfaceCreateInfoKHR
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_WIN32_SURFACE_CREATE_INFO_KHR,
                hwnd = (IntPtr)windowtemp,
                hinstance = Marshal.GetHINSTANCE(typeof(Program).Module)
            };

            if (VkFunc.vkCreateWin32SurfaceKHR(instance, ref surfaceCreateInfo, null, out VkSurfaceKHR surfacePtr) != 0)
            {
                MessageBox.Show("Failed to create Vulkan surface.");
                return;
            }

            surface = surfacePtr;
        }

        public static void CreateCommandPool()
        {
            VkCommandPool commandpool = new VkCommandPool();
            VkCommandPoolCreateInfo CommandPoolCreateInfo = new VkCommandPoolCreateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO,
                flags = VkCommandPoolCreateFlagBits.VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT,
                queueFamilyIndex = GraphicsFamily
            };
            VkFunc.vkCreateCommandPool(device, &CommandPoolCreateInfo, null, &commandpool);
            commandPool = commandpool;
        }

        public static void CreateSemaphores()
        {
            AcquireImageSemaphores = new VkSemaphore[MAX_FRAMES_IN_FLIGHT];
            PresentImageSemaphores = new VkSemaphore[MAX_FRAMES_IN_FLIGHT];
            InFlightFences = new VkFence[MAX_FRAMES_IN_FLIGHT];

            VkSemaphoreTypeCreateInfo semaphoreTypeCreateInfo = new VkSemaphoreTypeCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_EXPORT_SEMAPHORE_CREATE_INFO,
                semaphoreType = VkSemaphoreType.VK_SEMAPHORE_TYPE_BINARY,
                initialValue = 0,
                pNext = null
            };

            VkSemaphoreCreateInfo semaphoreCreateInfo = new VkSemaphoreCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_EXPORT_SEMAPHORE_CREATE_INFO,
                pNext = &semaphoreTypeCreateInfo
            };

            VkFenceCreateInfo fenceInfo = new VkFenceCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_EXPORT_FENCE_CREATE_INFO,
                flags = VkFenceCreateFlagBits.VK_FENCE_CREATE_SIGNALED_BIT
            };

            for (int x = 0; x < MAX_FRAMES_IN_FLIGHT; x++)
            {
                VkFunc.vkCreateSemaphore(device, in semaphoreCreateInfo, null, out AcquireImageSemaphores[x]);
                VkFunc.vkCreateSemaphore(device, in semaphoreCreateInfo, null, out PresentImageSemaphores[x]);
                VkFunc.vkCreateFence(device, in fenceInfo, null, out InFlightFences[x]);
            }
        }

        public static void CreateDeviceQueue()
        {
            GameEngineImport.DLL_Renderer_GetDeviceQueue(device, GraphicsFamily, PresentFamily, out VkQueue graphicsQueuePtr, out VkQueue presentQueuePtr);
            graphicsQueue = graphicsQueuePtr;
            presentQueue = presentQueuePtr;
        }

        public static VkCommandBuffer BeginSingleUseCommandBuffer()
        {
            VkCommandBuffer commandBuffer = new VkCommandBuffer();
            VkCommandBufferAllocateInfo allocInfo = new VkCommandBufferAllocateInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO,
                level = VkCommandBufferLevel.VK_COMMAND_BUFFER_LEVEL_PRIMARY,
                commandPool = commandPool,
                commandBufferCount = 1
            };
            VkFunc.vkAllocateCommandBuffers(device, in allocInfo, out commandBuffer);

            VkCommandBufferBeginInfo beginInfo = new VkCommandBufferBeginInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
                flags = VkCommandBufferUsageFlagBits.VK_COMMAND_BUFFER_USAGE_ONE_TIME_SUBMIT_BIT
            };
            VkFunc.vkBeginCommandBuffer(commandBuffer, &beginInfo);
            return commandBuffer;
        }

        public static VkResult EndSingleUseCommandBuffer(VkCommandBuffer commandBuffer)
        {
            VkSubmitInfo submitInfo = new VkSubmitInfo()
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_SUBMIT_INFO,
                commandBufferCount = 1,
                pCommandBuffers = &commandBuffer
            };

            VkFence fence = new VkFence();
            VkFunc.vkEndCommandBuffer(commandBuffer);
            VkFunc.vkQueueSubmit(graphicsQueue, 1, &submitInfo, fence);
            VkFunc.vkQueueWaitIdle(graphicsQueue);
            VkFunc.vkFreeCommandBuffers(device, commandPool, 1, &commandBuffer);
            return VkResult.VK_SUCCESS;
        }

        //SwapChain
        public static VkSurfaceCapabilitiesKHR SwapChain_GetSurfaceCapabilities()
        {
            return GameEngineImport.DLL_SwapChain_GetSurfaceCapabilities(physicalDevice, surface);
        }

        public static VkResult SwapChain_GetQueueFamilies()
        {
            return GameEngineImport.DLL_SwapChain_GetQueueFamilies(physicalDevice, surface, swapChain.GraphicsFamily, swapChain.PresentFamily);
        }

        public static VkSurfaceFormatKHR SwapChain_FindSwapSurfaceFormat(VkSurfaceFormatKHR[] availableFormats)
        {
            return GameEngineImport.DLL_SwapChain_FindSwapSurfaceFormat(availableFormats);
        }

        public static VkPresentModeKHR SwapChain_FindSwapPresentMode(VkPresentModeKHR[] availablePresentModes)
        {
            return GameEngineImport.DLL_SwapChain_FindSwapPresentMode(availablePresentModes);
        }

        public static VkSwapchainKHR SwapChain_SetUpSwapChain()
        {
            return GameEngineImport.DLL_SwapChain_SetUpSwapChain(device, physicalDevice, surface, swapChain.GraphicsFamily, swapChain.PresentFamily, swapChain.SwapChainResolution.width, swapChain.SwapChainResolution.height, out uint swapChainImageCount);
        }

        public static VkImage[] SwapChain_SetUpSwapChainImages()
        {
            return GameEngineImport.DLL_SwapChain_SetUpSwapChainImages(device, swapChain.Swapchain);
        }

        public static VkImageView[] SwapChain_SetUpSwapChainImageViews()
        {
            return GameEngineImport.DLL_SwapChain_SetUpSwapChainImageViews(device, swapChainImageList, out VkSurfaceFormatKHR swapChainImageFormat);
        }

        public static VkSurfaceFormatKHR[] SwapChain_GetPhysicalDeviceFormats()
        {
            return GameEngineImport.DLL_SwapChain_GetPhysicalDeviceFormats(physicalDevice, surface);
        }
    }
}
