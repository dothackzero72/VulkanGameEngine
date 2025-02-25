using Silk.NET.Core;
using Silk.NET.Core.Contexts;
using Silk.NET.Core.Native;
using Silk.NET.Maths;
using Silk.NET.SDL;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.EXT;
using Silk.NET.Vulkan.Extensions.KHR;
using Silk.NET.Windowing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using VulkanGameEngineGameObjectScripts.Vulkan;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Models;
using static System.Net.Mime.MediaTypeNames;
using Image = Silk.NET.Vulkan.Image;

namespace VulkanGameEngineLevelEditor.Vulkan
{
    public struct SwapChainState
    {
        public uint SwapChainImageCount;
        public uint GraphicsFamily;
        public uint PresentFamily;
        public VkQueue GraphicsQueue;
        public VkQueue PresentQueue;
        public VkFormat Format;
        public VkColorSpaceKHR ColorSpace;
        public VkPresentModeKHR PresentMode;

        public VkImage[] SwapChainImages;
        public VkImageView[] SwapChainImageViews;
        public VkExtent2D SwapChainResolution;
        public VkSwapchainKHR Swapchain;
    }

    public unsafe static class VulkanRenderer
    {
        public static Vk vk = Vk.GetApi();
        public const int MAX_FRAMES_IN_FLIGHT = 2;
        public static IntPtr windowPtr { get; set; }
        public static Instance instance { get; private set; }
        public static DebugUtilsMessengerEXT debugMessenger { get; private set; }
        public static SurfaceKHR surface { get; private set; }
        public static PhysicalDevice physicalDevice { get; private set; }
        public static uint GraphicsFamily { get; private set; }
        public static uint PresentFamily { get; private set; }
        public static Device device { get; private set; }
        public static Silk.NET.Vulkan.Queue graphicsQueue { get; private set; }
        public static Silk.NET.Vulkan.Queue presentQueue { get; private set; }
        public static CommandPool commandPool { get; private set; }
        public static Fence[] InFlightFences { get; private set; }
        public static Silk.NET.Vulkan.Semaphore[] AcquireImageSemaphores { get; private set; }
        public static Silk.NET.Vulkan.Semaphore[] PresentImageSemaphores { get; private set; }
        public static SwapChainState swapChain { get; private set; } = new SwapChainState();
        public static UInt32 ImageIndex { get; private set; } = new UInt32();
        public static UInt32 CommandIndex { get; private set; } = new UInt32();
        public static KhrSurface khrSurface { get; private set; }
        public static bool RebuildRendererFlag { get; private set; }

        public static string[] requiredExtensions;
        private static string[] instanceExtensions = { ExtDebugUtils.ExtensionName };
        private static string[] deviceExtensions = { KhrSwapchain.ExtensionName };
        private static string[] validationLayers = { "VK_LAYER_KHRONOS_validation" };

        public static void CreateVulkanRenderer(IntPtr window, Extent2D swapChainResolution)
        {
            windowPtr = window;
            instance = new Instance(GameEngineImport.DLL_Renderer_CreateVulkanInstance());
            debugMessenger = new DebugUtilsMessengerEXT(GameEngineImport.DLL_Renderer_SetupDebugMessenger(instance.Handle));
            CreateSurface(windowPtr);
            physicalDevice = new PhysicalDevice(GameEngineImport.DLL_Renderer_SetUpPhysicalDevice(instance.Handle, surface.Handle, GraphicsFamily, PresentFamily));
            device = new Device(GameEngineImport.DLL_Renderer_SetUpDevice(physicalDevice.Handle, GraphicsFamily, PresentFamily));
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

            SwapChainState swapChainState = new SwapChainState();
            swapChainState.Swapchain = SwapChain_SetUpSwapChain();
            swapChainState.SwapChainImages = SwapChain_SetUpSwapChainImages();
            swapChainState.SwapChainImageViews = SwapChain_SetUpSwapChainImageViews();
            swapChainState.SwapChainResolution.width = width;
            swapChainState.SwapChainResolution.height = height;
        }

        public static void CreateCommandBuffers(VkCommandBuffer[] commandBufferList)
        {
            for (int x = 0; x < MAX_FRAMES_IN_FLIGHT; x++)
            {
                CommandBufferAllocateInfo commandBufferAllocateInfo = new CommandBufferAllocateInfo()
                {
                    SType = StructureType.CommandBufferAllocateInfo,
                    CommandPool = commandPool,
                    Level = CommandBufferLevel.Primary,
                    CommandBufferCount = 1
                };

                vk.AllocateCommandBuffers(device, in commandBufferAllocateInfo, out commandBufferList[x]);
            }
        }

        public static void CreateCommandBuffers(CommandBuffer commandBuffer)
        {
            CommandBufferAllocateInfo commandBufferAllocateInfo = new CommandBufferAllocateInfo()
            {
                SType = StructureType.CommandBufferAllocateInfo,
                CommandPool = commandPool,
                Level = CommandBufferLevel.Primary,
                CommandBufferCount = 1
            };
            vk.AllocateCommandBuffers(device, in commandBufferAllocateInfo, out commandBuffer);
        }

        public static Result StartFrame()
        {
            CommandIndex = (CommandIndex + 1) % MAX_FRAMES_IN_FLIGHT;

            var fence = InFlightFences[(int)CommandIndex];
            var imageSemaphore = AcquireImageSemaphores[(int)CommandIndex];

            vk.WaitForFences(device, 1, &fence, Vk.True, ulong.MaxValue);
            vk.ResetFences(device, 1, &fence);

            var imageIndex = ImageIndex;
            Result result = swapChain.khrSwapchain.AcquireNextImage(device, swapChain.Swapchain, ulong.MaxValue, imageSemaphore, fence, &imageIndex);
            ImageIndex = imageIndex;

            if (result == Result.ErrorOutOfDateKhr)
            {
                RebuildRendererFlag = true;
                return result;
            }

            return result;
        }

        public static unsafe Result EndFrame(List<CommandBuffer> commandBufferSubmitList)
        {
            var fence = InFlightFences[(int)CommandIndex];
            var presentSemaphore = PresentImageSemaphores[(int)CommandIndex];
            var imageSemaphore = AcquireImageSemaphores[(int)CommandIndex];

            vk.WaitForFences(device, 1, &fence, Vk.True, ulong.MaxValue);
            vk.ResetFences(device, 1, &fence);
            InFlightFences[(int)CommandIndex] = fence;

            PipelineStageFlags[] waitStages = new PipelineStageFlags[]
            {
                PipelineStageFlags.ColorAttachmentOutputBit
            };

            fixed (PipelineStageFlags* pWaitStages = waitStages)
            {
                var commandBufferCount = commandBufferSubmitList.Count;
                var commandBuffersPtr = (CommandBuffer*)Marshal.AllocHGlobal(commandBufferCount * sizeof(CommandBuffer));

                try
                {
                    for (int i = 0; i < commandBufferCount; i++)
                    {
                        commandBuffersPtr[i] = commandBufferSubmitList[i];
                    }

                    SubmitInfo submitInfo = new SubmitInfo()
                    {
                        SType = StructureType.SubmitInfo,
                        WaitSemaphoreCount = 1,
                        PWaitSemaphores = &imageSemaphore,
                        PWaitDstStageMask = pWaitStages,
                        CommandBufferCount = (uint)commandBufferSubmitList.Count,
                        PCommandBuffers = commandBuffersPtr,
                        SignalSemaphoreCount = 1,
                        PSignalSemaphores = &presentSemaphore
                    };

                    Result submitResult = vk.QueueSubmit(graphicsQueue, 1, &submitInfo, fence);
                    if (submitResult != Result.Success)
                    {
                        return submitResult;
                    }

                    var imageIndex = ImageIndex;
                    var swapchain = swapChain.Swapchain;
                    PresentInfoKHR presentInfo = new PresentInfoKHR()
                    {
                        SType = StructureType.PresentInfoKhr,
                        WaitSemaphoreCount = 1,
                        PWaitSemaphores = &presentSemaphore,
                        SwapchainCount = 1,
                        PSwapchains = &swapchain,
                        PImageIndices = &imageIndex
                    };

                    Result result = swapChain.khrSwapchain.QueuePresent(presentQueue, in presentInfo);
                    if (result == Result.ErrorOutOfDateKhr || result == Result.SuboptimalKhr)
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

        public static PipelineShaderStageCreateInfo CreateShader(string path, ShaderStageFlags shaderStage)
        {
            byte[] shaderBytes = File.ReadAllBytes(path);
            ShaderModule shaderModule = CreateShaderModule(shaderBytes);
            IntPtr pName = Marshal.StringToHGlobalAnsi("main");
            PipelineShaderStageCreateInfo shaderStageInfo = new PipelineShaderStageCreateInfo()
            {
                SType = StructureType.PipelineShaderStageCreateInfo,
                Stage = shaderStage,
                Module = shaderModule,
                PName = (byte*)pName,
                PNext = null,
                Flags = 0
            };

            return shaderStageInfo;
        }

        public static ShaderModule CreateShaderModule(byte[] code)
        {
            ShaderModule shaderModule = new ShaderModule();
            fixed (byte* codePtr = code)
            {
                var createInfo = new ShaderModuleCreateInfo
                {
                    SType = StructureType.ShaderModuleCreateInfo,
                    CodeSize = (nuint)code.Length,
                    PCode = (uint*)codePtr
                };

                Result result = vk.CreateShaderModule(device, &createInfo, null, &shaderModule);
                if (result != Result.Success)
                {
                    Console.WriteLine($"Failed to create shader module: {result}");
                }
            }
            return shaderModule;
        }

        public static uint GetMemoryType(uint typeFilter, MemoryPropertyFlags properties)
        {
            PhysicalDeviceMemoryProperties memProperties;
            vk.GetPhysicalDeviceMemoryProperties(physicalDevice, &memProperties);

            for (uint x = 0; x < memProperties.MemoryTypeCount; x++)
            {
                if ((typeFilter & (1 << (int)x)) != 0 &&
                    (memProperties.MemoryTypes[(int)x].PropertyFlags & properties) == properties)
                {
                    return x;
                }
            }

            return uint.MaxValue;
        }

        [DllImport("vulkan-1.dll")]
        public static extern int vkCreateWin32SurfaceKHR(Instance instance, ref Win32SurfaceCreateInfoKHR pCreateInfo, AllocationCallbacks* pAllocator, out SurfaceKHR pSurface);

        public static void CreateSurface(IntPtr windowtemp)
        {
            var surfaceCreateInfo = new Win32SurfaceCreateInfoKHR
            {
                SType = StructureType.Win32SurfaceCreateInfoKhr,
                Hwnd = (IntPtr)windowtemp,
                Hinstance = Marshal.GetHINSTANCE(typeof(Program).Module)
            };

            if (vkCreateWin32SurfaceKHR(instance, ref surfaceCreateInfo, null, out SurfaceKHR surfacePtr) != 0)
            {
                MessageBox.Show("Failed to create Vulkan surface.");
                return;
            }

            surface = surfacePtr;
        }

        public static void CreateCommandPool()
        {
            CommandPool commandpool = new CommandPool();
            CommandPoolCreateInfo CommandPoolCreateInfo = new CommandPoolCreateInfo()
            {
                SType = StructureType.CommandPoolCreateInfo,
                Flags = CommandPoolCreateFlags.ResetCommandBufferBit,
                QueueFamilyIndex = GraphicsFamily
            };
            vk.CreateCommandPool(device, &CommandPoolCreateInfo, null, &commandpool);
            commandPool = commandpool;
        }

        public static void CreateSemaphores()
        {
            AcquireImageSemaphores = new Silk.NET.Vulkan.Semaphore[MAX_FRAMES_IN_FLIGHT];
            PresentImageSemaphores = new Silk.NET.Vulkan.Semaphore[MAX_FRAMES_IN_FLIGHT];
            InFlightFences = new Fence[MAX_FRAMES_IN_FLIGHT];

            SemaphoreTypeCreateInfo semaphoreTypeCreateInfo = new SemaphoreTypeCreateInfo()
            {
                SType = StructureType.SemaphoreTypeCreateInfo,
                SemaphoreType = SemaphoreType.Binary,
                InitialValue = 0,
                PNext = null
            };

            SemaphoreCreateInfo semaphoreCreateInfo = new SemaphoreCreateInfo()
            {
                SType = StructureType.SemaphoreCreateInfo,
                PNext = &semaphoreTypeCreateInfo
            };

            FenceCreateInfo fenceInfo = new FenceCreateInfo()
            {
                SType = StructureType.FenceCreateInfo,
                Flags = FenceCreateFlags.SignaledBit
            };

            for (int x = 0; x < MAX_FRAMES_IN_FLIGHT; x++)
            {
                vk.CreateSemaphore(device, in semaphoreCreateInfo, null, out AcquireImageSemaphores[x]);
                vk.CreateSemaphore(device, in semaphoreCreateInfo, null, out PresentImageSemaphores[x]);
                vk.CreateFence(device, in fenceInfo, null, out InFlightFences[x]);
            }
        }

        public static void CreateDeviceQueue()
        {
            GameEngineImport.DLL_Renderer_GetDeviceQueue(device.Handle, GraphicsFamily, PresentFamily, out VkQueue graphicsQueuePtr, out VkQueue presentQueuePtr);
            graphicsQueue = new Silk.NET.Vulkan.Queue(graphicsQueuePtr);
            presentQueue = new Silk.NET.Vulkan.Queue(presentQueuePtr);
        }

        public static CommandBuffer BeginSingleUseCommandBuffer()
        {
            CommandBuffer commandBuffer = new CommandBuffer();
            CommandBufferAllocateInfo allocInfo = new CommandBufferAllocateInfo()
            {
                SType = StructureType.CommandBufferAllocateInfo,
                Level = CommandBufferLevel.Primary,
                CommandPool = commandPool,
                CommandBufferCount = 1
            };
            vk.AllocateCommandBuffers(device, &allocInfo, &commandBuffer);

            CommandBufferBeginInfo beginInfo = new CommandBufferBeginInfo()
            {
                SType = StructureType.CommandBufferBeginInfo,
                Flags = CommandBufferUsageFlags.OneTimeSubmitBit
            };
            vk.BeginCommandBuffer(commandBuffer, &beginInfo);
            return commandBuffer;
        }

        public static Result EndSingleUseCommandBuffer(CommandBuffer commandBuffer)
        {
            SubmitInfo submitInfo = new SubmitInfo()
            {
                SType = StructureType.SubmitInfo,
                CommandBufferCount = 1,
                PCommandBuffers = &commandBuffer
            };

            Fence fence = new Fence();
            vk.EndCommandBuffer(commandBuffer);
            vk.QueueSubmit(graphicsQueue, 1, &submitInfo, fence);
            vk.QueueWaitIdle(graphicsQueue);
            vk.FreeCommandBuffers(device, commandPool, 1, &commandBuffer);
            return Result.Success;
        }

        //SwapChain
        public static VkSurfaceCapabilitiesKHR SwapChain_GetSurfaceCapabilities()
        {
            return GameEngineImport.DLL_SwapChain_GetSurfaceCapabilities(VulkanRenderer.physicalDevice.Handle, VulkanRenderer.surface.Handle);
        }

        public static VkResult SwapChain_GetQueueFamilies()
        {
            return GameEngineImport.DLL_SwapChain_GetQueueFamilies(physicalDevice.Handle, surface.Handle, swapChain.GraphicsFamily, swapChain.PresentFamily);
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
            return GameEngineImport.DLL_SwapChain_SetUpSwapChain(device.Handle, physicalDevice.Handle, surface.Handle, swapChain.GraphicsFamily, swapChain.PresentFamily, swapChain.SwapChainResolution.width, swapChain.SwapChainResolution.height, out uint swapChainImageCount);
        }

        public static VkImage[] SwapChain_SetUpSwapChainImages()
        {
            return GameEngineImport.DLL_SwapChain_SetUpSwapChainImages(device.Handle, swapChain.Swapchain);
        }

        public static VkImageView[] SwapChain_SetUpSwapChainImageViews()
        {
            return GameEngineImport.DLL_SwapChain_SetUpSwapChainImageViews(device.Handle, swapChainImageList, out VkSurfaceFormatKHR swapChainImageFormat);
        }

        public static VkSurfaceFormatKHR[] SwapChain_GetPhysicalDeviceFormats()
        {
            return GameEngineImport.DLL_SwapChain_GetPhysicalDeviceFormats(physicalDevice.Handle, surface.Handle);
        }
    }
}
