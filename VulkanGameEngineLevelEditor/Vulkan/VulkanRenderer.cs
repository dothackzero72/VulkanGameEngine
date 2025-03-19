using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.EXT;
using Silk.NET.Vulkan.Extensions.KHR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Models;

namespace VulkanGameEngineLevelEditor.Vulkan
{
    public class SwapChainState
    {
        public uint ImageCount { get;  set; }
        public VkFormat Format { get;  set; }
        public VkColorSpaceKHR ColorSpace { get;  set; }
        public VkPresentModeKHR PresentMode { get;  set; }
        public ListPtr<VkImage> Images { get;  set; }
        public ListPtr<VkImageView> imageViews { get;  set; }
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
        public static ListPtr<VkFence> InFlightFences { get; private set; }
        public static ListPtr<VkSemaphore> AcquireImageSemaphores { get; private set; }
        public static ListPtr<VkSemaphore> PresentImageSemaphores { get; private set; } 
        public static SwapChainState SwapChain { get; set; } = new SwapChainState();
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
            CreateSwapChain(windowPtr);
            CreateSemaphores(MAX_FRAMES_IN_FLIGHT);
            CreateCommandPool();
        }

        public static void CreateSwapChain(IntPtr window)
        {
            ListPtr<VkSurfaceFormatKHR> compatibleSwapChainFormatList = SwapChain_GetPhysicalDeviceFormats();
            GameEngineImport.DLL_SwapChain_GetQueueFamilies(physicalDevice, surface, out uint graphicsFamily, out uint presentFamily);
            ListPtr<VkPresentModeKHR> compatiblePresentModesList = Renderer_GetSurfacePresentModes();
            VkSurfaceFormatKHR swapChainImageFormat = SwapChain_FindSwapSurfaceFormat(compatibleSwapChainFormatList);
            VkPresentModeKHR swapChainPresentMode = SwapChain_FindSwapPresentMode(compatiblePresentModesList);
            VkSurfaceCapabilitiesKHR SurfaceCapabilities = GameEngineImport.DLL_SwapChain_GetSurfaceCapabilities(physicalDevice, surface, out uint width, out uint height);

            SwapChain.SwapChainResolution = new VkExtent2D
            {
                height = height,
                width = width
            };
            SwapChain.Swapchain = GameEngineImport.DLL_SwapChain_SetUpSwapChain(device, physicalDevice, surface, GraphicsFamily, PresentFamily, SwapChain.SwapChainResolution.width, SwapChain.SwapChainResolution.height, out uint swapChainImageCount);
            SwapChain_SetUpSwapChainImages(swapChainImageCount);
            SwapChain_SetUpSwapChainImageViews(swapChainImageFormat, swapChainImageCount);
            SwapChain.ImageCount = swapChainImageCount;
        }

        public static void CreateCommandBuffers(ListPtr<VkCommandBuffer> commandBufferList)
        {
            for (int x = 0; x < SwapChain.imageViews.Count; x++)
            {
                VkCommandBufferAllocateInfo commandBufferAllocateInfo = new VkCommandBufferAllocateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO,
                    commandPool = commandPool,
                    level = VkCommandBufferLevel.VK_COMMAND_BUFFER_LEVEL_PRIMARY,
                    commandBufferCount = 1
                };

                VkCommandBuffer commandBuffer = new VkCommandBuffer();
                VkFunc.vkAllocateCommandBuffers(device, in commandBufferAllocateInfo, out commandBuffer);
                commandBufferList.Add(commandBuffer);
            }
        }

        public static VkResult StartFrame()
        {
            CommandIndex = (CommandIndex + 1) % MAX_FRAMES_IN_FLIGHT;

            var fence = InFlightFences[(int)CommandIndex];
            var imageSemaphore = AcquireImageSemaphores[(int)CommandIndex];

            VkFunc.vkWaitForFences(device, 1, &fence, true, ulong.MaxValue);
            VkFunc.vkResetFences(device, 1, &fence);

            VkResult result = VkFunc.vkAcquireNextImageKHR(device, SwapChain.Swapchain, ulong.MaxValue, imageSemaphore, fence, out var imageIndex);
            ImageIndex = imageIndex;

            if (result == VkResult.VK_ERROR_OUT_OF_DATE_KHR)
            {
                RebuildRendererFlag = true;
                return result;
            }

            return result;
        }

        public static unsafe VkResult EndFrame(ListPtr<VkCommandBuffer> commandBufferSubmitList)
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
                VkSubmitInfo submitInfo = new VkSubmitInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_SUBMIT_INFO,
                    waitSemaphoreCount = 1,
                    pWaitSemaphores = &imageSemaphore,
                    pWaitDstStageMask = pWaitStages,
                    commandBufferCount = commandBufferSubmitList.UCount,
                    pCommandBuffers = commandBufferSubmitList.Ptr,
                    signalSemaphoreCount = 1,
                    pSignalSemaphores = &presentSemaphore
                };

                VkResult submitResult = VkFunc.vkQueueSubmit(graphicsQueue, 1, &submitInfo, fence);
                if (submitResult != VkResult.VK_SUCCESS)
                {
                    return submitResult;
                }

                var imageIndex = ImageIndex;
                var swapchain = SwapChain.Swapchain;
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
                    codeSize = code.Length,
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
            return GameEngineImport.DLL_Tools_GetMemoryType(VulkanRenderer.physicalDevice, typeFilter, properties);
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
                sType = VkStructureType.VK_STRUCTURE_TYPE_SUBMIT_INFO,
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
        public static VkSurfaceFormatKHR SwapChain_FindSwapSurfaceFormat(ListPtr<VkSurfaceFormatKHR> availableFormats)
        {
            if (availableFormats == null ||
                availableFormats.UCount == 0)
            {
                throw new ArgumentException("Available formats cannot be null or empty.");
            }

            return GameEngineImport.DLL_SwapChain_FindSwapSurfaceFormat(availableFormats.Ptr, availableFormats.UCount);
        }

        public static VkPresentModeKHR SwapChain_FindSwapPresentMode(ListPtr<VkPresentModeKHR> availablePresentModes)
        {
            if (availablePresentModes == null ||
                availablePresentModes.UCount == 0)
            {
                throw new ArgumentException("Available formats cannot be null or empty.");
            }

            return GameEngineImport.DLL_SwapChain_FindSwapPresentMode(availablePresentModes.Ptr, availablePresentModes.UCount);
        }

        public static void SwapChain_SetUpSwapChainImages(uint swapChainImageCount)
        {
            VkImage* images = GameEngineImport.DLL_SwapChain_SetUpSwapChainImages(device, SwapChain.Swapchain, swapChainImageCount);
            SwapChain.Images = new ListPtr<VkImage>(images, swapChainImageCount);
        }

        public static void CreateSemaphores(uint swapChainImageCount)
        {
            InFlightFences = new ListPtr<VkFence>(swapChainImageCount);
            AcquireImageSemaphores = new ListPtr<VkSemaphore>(swapChainImageCount);
            PresentImageSemaphores = new ListPtr<VkSemaphore>(swapChainImageCount);

            VkResult result = GameEngineImport.DLL_Renderer_SetUpSemaphores(device, InFlightFences.Ptr, AcquireImageSemaphores.Ptr, PresentImageSemaphores.Ptr, swapChainImageCount);
            if (result != VkResult.VK_SUCCESS)
            {
                throw new Exception($"Failed to set up semaphores: {result}");
            }
        }

        public static void SwapChain_SetUpSwapChainImageViews(VkSurfaceFormatKHR swapChainImageFormat, uint swapChainImageCount)
        {
            VkImageView* swapChainImagePtr = GameEngineImport.DLL_SwapChain_SetUpSwapChainImageViews(device, SwapChain.Images.Ptr, swapChainImageFormat, swapChainImageCount);
            SwapChain.imageViews = new ListPtr<nint>(swapChainImagePtr, swapChainImageCount);
        }

        public static ListPtr<VkSurfaceFormatKHR> SwapChain_GetPhysicalDeviceFormats()
        {
            VkSurfaceFormatKHR* surfaceFormatListPtr = GameEngineImport.DLL_SwapChain_GetPhysicalDeviceFormats(physicalDevice, surface, out uint count);
            return new ListPtr<VkSurfaceFormatKHR>(surfaceFormatListPtr, count);
        }

        public static ListPtr<VkPresentModeKHR> Renderer_GetSurfacePresentModes()
        {
            VkPresentModeKHR* presentModeListPtr =  GameEngineImport.DLL_Renderer_GetSurfacePresentModes(physicalDevice, surface, out uint count);
            return new ListPtr<VkPresentModeKHR>(presentModeListPtr, count);
        }

        public static void Destroy()
        {
            InFlightFences.Dispose();
            AcquireImageSemaphores.Dispose();
            PresentImageSemaphores.Dispose();

            InFlightFences = null;
            AcquireImageSemaphores = null;
            PresentImageSemaphores = null;
        }
    }
}
