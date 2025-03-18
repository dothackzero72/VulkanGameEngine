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
            VkSurfaceFormatKHR[] compatibleSwapChainFormatList = SwapChain_GetPhysicalDeviceFormats();
            GameEngineImport.DLL_SwapChain_GetQueueFamilies(physicalDevice, surface, out uint graphicsFamily, out uint presentFamily);
            VkPresentModeKHR[] compatiblePresentModesList = Renderer_GetSurfacePresentModes();
            VkSurfaceFormatKHR swapChainImageFormat = SwapChain_FindSwapSurfaceFormat(compatibleSwapChainFormatList);
            VkPresentModeKHR swapChainPresentMode = SwapChain_FindSwapPresentMode(compatiblePresentModesList);
            VkSurfaceCapabilitiesKHR SurfaceCapabilities = GameEngineImport.DLL_SwapChain_GetSurfaceCapabilities(physicalDevice, surface, out uint width, out uint height);

            SwapChain.SwapChainResolution = new VkExtent2D
            {
                height = height,
                width = width
            };
            SwapChain.Swapchain = GameEngineImport.DLL_SwapChain_SetUpSwapChain(device, physicalDevice, surface, GraphicsFamily, PresentFamily, SwapChain.SwapChainResolution.width, SwapChain.SwapChainResolution.height, out uint swapChainImageCount);
            SwapChain.Images = SwapChain_SetUpSwapChainImages(swapChainImageCount);
            SwapChain.imageViews = SwapChain_SetUpSwapChainImageViews(swapChainImageFormat, swapChainImageCount);

            SwapChain.ImageCount = swapChainImageCount;
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

            VkResult result = VkFunc.vkAcquireNextImageKHR(device, SwapChain.Swapchain, ulong.MaxValue, imageSemaphore, fence, out var imageIndex);
            ImageIndex = imageIndex;

            if (result == VkResult.VK_ERROR_OUT_OF_DATE_KHR)
            {
                RebuildRendererFlag = true;
                return result;
            }

            return result;
        }

        public static unsafe VkResult EndFrame(VkCommandBuffer[] commandBufferSubmitList)
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
                var commandBufferCount = commandBufferSubmitList.Length;
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
                        commandBufferCount = (uint)commandBufferSubmitList.Length,
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

        public static VkSurfaceFormatKHR SwapChain_FindSwapSurfaceFormat(VkSurfaceFormatKHR[] availableFormats)
        {
            if (availableFormats == null || 
                availableFormats.Length == 0)
            {
                throw new ArgumentException("Available formats cannot be null or empty.");
            }

            fixed (VkSurfaceFormatKHR* ptr = availableFormats)
            {
                return GameEngineImport.DLL_SwapChain_FindSwapSurfaceFormat(ptr, (uint)availableFormats.Length);
            }
        }

        public static VkPresentModeKHR SwapChain_FindSwapPresentMode(VkPresentModeKHR[] availablePresentModes)
        {
            if (availablePresentModes == null ||
                availablePresentModes.Length == 0)
            {
                throw new ArgumentException("Available formats cannot be null or empty.");
            }

            fixed (VkPresentModeKHR* ptr = availablePresentModes)
            {
                return GameEngineImport.DLL_SwapChain_FindSwapPresentMode(ptr, (uint)availablePresentModes.Length);
            }
        }

        public static VkImage[] SwapChain_SetUpSwapChainImages(uint swapChainImageCount)
        {
            VkImage* swapChainImagePtr = GameEngineImport.DLL_SwapChain_SetUpSwapChainImages(device, SwapChain.Swapchain, swapChainImageCount);
            if (swapChainImagePtr == null)
            {
                return Array.Empty<VkImage>();
            }

            VkImage[] formats = new VkImage[swapChainImageCount];
            IntPtr ptr = (IntPtr)swapChainImagePtr;
            for (uint x = 0; x < swapChainImageCount; x++)
            {
                formats[x] = Marshal.PtrToStructure<VkImage>(ptr + (int)(x * Marshal.SizeOf<VkImage>()));
            }

            GameEngineImport.DLL_Tools_DeleteAllocatedPtr(swapChainImagePtr);
            return formats;
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

        public static VkImageView[] SwapChain_SetUpSwapChainImageViews(VkSurfaceFormatKHR swapChainImageFormat, uint swapChainImageCount)
        {
            fixed (VkImage* imagePtr = SwapChain.Images)
            {
                VkImageView* swapChainImagePtr = GameEngineImport.DLL_SwapChain_SetUpSwapChainImageViews(device, imagePtr, swapChainImageFormat, swapChainImageCount);
                if (swapChainImagePtr == null)
                {
                    return Array.Empty<VkImageView>();
                }

                VkImageView[] imageViewList = new VkImageView[swapChainImageCount];
                IntPtr ptr = (IntPtr)swapChainImagePtr;
                for (uint x = 0; x < swapChainImageCount; x++)
                {
                    imageViewList[x] = Marshal.PtrToStructure<VkImageView>(ptr + (int)(x * Marshal.SizeOf<VkImageView>()));
                }

                GameEngineImport.DLL_Tools_DeleteAllocatedPtr(swapChainImagePtr);
                return imageViewList;
            }
        }

        public static VkSurfaceFormatKHR[] SwapChain_GetPhysicalDeviceFormats()
        {
            VkSurfaceFormatKHR* surfaceFormatListPtr = GameEngineImport.DLL_SwapChain_GetPhysicalDeviceFormats(physicalDevice, surface, out uint count);
            if (surfaceFormatListPtr == null || count == 0)
            {
                return Array.Empty<VkSurfaceFormatKHR>();
            }

            VkSurfaceFormatKHR[] formats = new VkSurfaceFormatKHR[count];
            IntPtr ptr = (IntPtr)surfaceFormatListPtr;
            for (uint x = 0; x < count; x++)
            {
                formats[x] = Marshal.PtrToStructure<VkSurfaceFormatKHR>(ptr + (int)(x * Marshal.SizeOf<VkSurfaceFormatKHR>()));
            }

            GameEngineImport.DLL_Tools_DeleteAllocatedPtr(surfaceFormatListPtr);
            return formats;
        }

        public static VkPresentModeKHR[] Renderer_GetSurfacePresentModes()
        {
            VkPresentModeKHR* presentModeListPtr =  GameEngineImport.DLL_Renderer_GetSurfacePresentModes(physicalDevice, surface, out uint count);
            if (presentModeListPtr == null || count == 0)
            {
                return Array.Empty<VkPresentModeKHR>();
            }

            VkPresentModeKHR[] presentModes = new VkPresentModeKHR[count];
            IntPtr ptr = (IntPtr)presentModeListPtr;
            for (uint x = 0; x < count; x++)
            {
                presentModes[x] = *(presentModeListPtr + x);
            }

            GameEngineImport.DLL_Tools_DeleteAllocatedPtr(presentModeListPtr);
            return presentModes;
        }
    }
}
