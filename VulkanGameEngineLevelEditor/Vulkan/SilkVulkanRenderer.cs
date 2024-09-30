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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using static System.Net.Mime.MediaTypeNames;

namespace VulkanGameEngineLevelEditor.Vulkan
{

    public unsafe static class VulkanRenderer
    {
        public static Vk vulkan = Vk.GetApi();
        public const int MAX_FRAMES_IN_FLIGHT = 2;
        public static IWindow window { get; set; }
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
        public static VulkanSwapChain swapChain { get; private set; } = new VulkanSwapChain();
        public static UInt32 ImageIndex { get; private set; } = new UInt32();
        public static UInt32 CommandIndex { get; private set; } = new UInt32();
        public static KhrSurface khrSurface { get; private set; }
        public static bool RebuildRendererFlag { get; private set; }

        public static string[] requiredExtensions;

        private static string[] instanceExtensions = { ExtDebugUtils.ExtensionName };
        private static string[] deviceExtensions = { KhrSwapchain.ExtensionName };
        private static string[] validationLayers = { "VK_LAYER_KHRONOS_validation" };

        public static void CreateVulkanRenderer()
        {
            validationLayers = CheckAvailableValidationLayers(validationLayers);
            if (validationLayers is null)
            {
                throw new NotSupportedException("Validation layers requested, but not available!");
            }

            CreateWindow();
            CreateInstance();
            CreateSurface();
            CreatePhysicalDevice();
            CreateDevice();
            CreateDeviceQueue();
            swapChain.CreateSwapChain();
            CreateCommandPool();
            CreateSemaphores();
        }

        public static void CreateCommandBuffers(CommandBuffer[] commandBufferList)
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

                vulkan.AllocateCommandBuffers(device, in commandBufferAllocateInfo, out commandBufferList[x]);
            }
        }

        public static Result StartFrame()
        {
            CommandIndex = (CommandIndex + 1) % MAX_FRAMES_IN_FLIGHT;

            var fence = InFlightFences[(int)CommandIndex];
            var imageSemaphore = AcquireImageSemaphores[(int)CommandIndex];

            vulkan.WaitForFences(device, 1, &fence, Vk.True, ulong.MaxValue);
            vulkan.ResetFences(device, 1, &fence);

            var imageIndex = ImageIndex;
            Result result = swapChain.khrSwapchain.AcquireNextImage(device, swapChain.swapChain, ulong.MaxValue, imageSemaphore, fence, &imageIndex);
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

            vulkan.WaitForFences(device, 1, &fence, Vk.True, ulong.MaxValue);
            vulkan.ResetFences(device, 1, &fence);
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

                    Result submitResult = vulkan.QueueSubmit(graphicsQueue, 1, &submitInfo, fence);
                    if (submitResult != Result.Success)
                    {
                        return submitResult;
                    }

                    var imageIndex = ImageIndex;
                    var swapchain = swapChain.swapChain;
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

        private static ShaderModule CreateShaderModule(byte[] code)
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

                Result result = vulkan.CreateShaderModule(device, &createInfo, null, &shaderModule);
                if (result != Result.Success)
                {
                    Console.WriteLine($"Failed to create shader module: {result}");
                }
            }
            return shaderModule;
        }

        private static void CreateWindow()
        {
            var windows = new SilkVulkanWindow();
            window = windows.CreateWindow("Vulkan Level Editor", new Vector2D<int>(1280, 720), out requiredExtensions);
        }

        private static void CreateInstance()
        {
            var extensions = requiredExtensions.Concat(instanceExtensions).ToArray();

            ApplicationInfo applicationInfo = new
          (
              pApplicationName: (byte*)Marshal.StringToHGlobalAnsi("Level Editor Play Test"),
              applicationVersion: new Version32(1, 0, 0),
              pEngineName: (byte*)Marshal.StringToHGlobalAnsi(""),
              engineVersion: new Version32(1, 0, 0),
              apiVersion: Vk.Version13
          );

            InstanceCreateInfo createInfo = new
            (
                pApplicationInfo: &applicationInfo
            );

            createInfo.EnabledExtensionCount = (uint)extensions.Length;
            createInfo.PpEnabledExtensionNames = (byte**)SilkMarshal.StringArrayToPtr(extensions);

            if (validationLayers != null)
            {
                createInfo.EnabledLayerCount = (uint)validationLayers.Length;
                createInfo.PpEnabledLayerNames = (byte**)SilkMarshal.StringArrayToPtr(validationLayers);
            }

            DebugUtilsMessengerCreateInfoEXT debugCreateInfo = VulkanDebug.MakeDebugUtilsMessengerCreateInfoEXT();
            createInfo.PNext = &debugCreateInfo;


            Result result = vulkan.CreateInstance(in createInfo, null, out Instance vkInstance);
            instance = vkInstance;
            if (result != Result.Success)
            {
                throw new Exception("Failed to create instance!");
            }

            ExtDebugUtils debugUtils;
            if (!vulkan.TryGetInstanceExtension(instance, out debugUtils))
            {
                throw new Exception("Failed to create debug messenger.");
            }

            if (debugUtils.CreateDebugUtilsMessenger(instance, &debugCreateInfo, null,
                out DebugUtilsMessengerEXT DebugMessenger) != Result.Success)
            {
                throw new Exception("Failed to create debug messenger.");
            }

            debugMessenger = DebugMessenger;

            SilkMarshal.Free((nint)createInfo.PpEnabledLayerNames);
            SilkMarshal.Free((nint)createInfo.PpEnabledExtensionNames);

            Marshal.FreeHGlobal((nint)applicationInfo.PApplicationName);
            Marshal.FreeHGlobal((nint)applicationInfo.PEngineName);
        }

        private static void CreateSurface()
        {

            surface = window.VkSurface.Create<AllocationCallbacks>(((Instance)instance).ToHandle(), null).ToSurface();

            KhrSurface sufacekhr;
            if (!vulkan.TryGetInstanceExtension(instance, out sufacekhr))
            {
                throw new NotSupportedException("KHR_surface extension not found.");
            }
            khrSurface = sufacekhr;
        }

        private static void CreatePhysicalDevice()
        {
            uint deviceCount = 0;
            vulkan.EnumeratePhysicalDevices(instance, &deviceCount, null);
            PhysicalDevice[] physicalDevices = new PhysicalDevice[deviceCount];
            vulkan.EnumeratePhysicalDevices(instance, &deviceCount, physicalDevices);

            foreach (var tempPhysicalDevice in physicalDevices)
            {
                string[] availableExtensions = GetExtensionProperteis(tempPhysicalDevice);
                bool isExtensionsSupported = deviceExtensions.All(e => availableExtensions.Contains(e));
                uint tempGraphicsFamily = FindGraphicsQueueFamily(tempPhysicalDevice);
                uint tempPresentFamily = FindPresentQueueFamily(tempPhysicalDevice);

                bool isSwapChainAdequate = false;
                if (isExtensionsSupported)
                {
                    var formats = GetSurfaceFormatsKHR(tempPhysicalDevice);
                    var presentModes = GetSurfacePresentModesKHR(tempPhysicalDevice);
                    isSwapChainAdequate = (formats != null) && (presentModes != null);
                }

                vulkan.GetPhysicalDeviceFeatures(tempPhysicalDevice, out PhysicalDeviceFeatures supportedFeatures);
                if (isExtensionsSupported &&
                    isSwapChainAdequate &&
                    supportedFeatures.SamplerAnisotropy)
                {
                    physicalDevice = tempPhysicalDevice;
                    break;
                }
            }
        }

        private static void CreateDevice()
        {
            HashSet<uint> queueFamilyList = new() { GraphicsFamily, PresentFamily };
            using var memory = GlobalMemory.Allocate(queueFamilyList.Count * sizeof(DeviceQueueCreateInfo));
            var queueCreate = (DeviceQueueCreateInfo*)Unsafe.AsPointer(ref memory.GetPinnableReference());

            float queuePriority = 1.0f;

            uint x = 0;
            foreach (var queueId in queueFamilyList)
            {
                DeviceQueueCreateInfo queueCreateInfo = new DeviceQueueCreateInfo
                (
                    queueFamilyIndex: queueId,
                    queueCount: 1,
                    pQueuePriorities: &queuePriority
                );
                queueCreate[x++] = queueCreateInfo;
            }

            PhysicalDeviceFeatures deviceFeatures = new PhysicalDeviceFeatures()
            {
                SamplerAnisotropy = true
            };

            DeviceCreateInfo createInfo = new DeviceCreateInfo
            {
                SType = StructureType.DeviceCreateInfo,
                PQueueCreateInfos = queueCreate,
                QueueCreateInfoCount = (uint)queueFamilyList.Count,
                PEnabledFeatures = &deviceFeatures,
                EnabledExtensionCount = (uint)deviceExtensions.Length,
                PpEnabledExtensionNames = (byte**)SilkMarshal.StringArrayToPtr(deviceExtensions),
                EnabledLayerCount = (uint)validationLayers.Length,
                PpEnabledLayerNames = (byte**)SilkMarshal.StringArrayToPtr(validationLayers)
            };

            var result = vulkan.CreateDevice(physicalDevice, in createInfo, null, out Device devicePtr);
            if (result != Result.Success)
            {
                // Handle the error accordingly
                throw new Exception("Failed to create Vulkan device");
            }

            device = devicePtr;
        }

        private static void CreateCommandPool()
        {
            CommandPool commandpool = new CommandPool();
            CommandPoolCreateInfo CommandPoolCreateInfo = new CommandPoolCreateInfo()
            {
                SType = StructureType.CommandPoolCreateInfo,
                Flags = CommandPoolCreateFlags.ResetCommandBufferBit,
                QueueFamilyIndex = GraphicsFamily
            };
            vulkan.CreateCommandPool(device, &CommandPoolCreateInfo, null, &commandpool);
            commandPool = commandpool;
        }

        private static void CreateSemaphores()
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
                vulkan.CreateSemaphore(device, in semaphoreCreateInfo, null, out AcquireImageSemaphores[x]);
                vulkan.CreateSemaphore(device, in semaphoreCreateInfo, null, out PresentImageSemaphores[x]);
                vulkan.CreateFence(device, in fenceInfo, null, out InFlightFences[x]);
            }
        }

        private static void CreateDeviceQueue()
        {
            vulkan.GetDeviceQueue(device, GraphicsFamily, 0, out Silk.NET.Vulkan.Queue graphicsQueuePtr);
            vulkan.GetDeviceQueue(device, PresentFamily, 0, out Silk.NET.Vulkan.Queue presentQueuePtr);
            graphicsQueue = graphicsQueuePtr;
            presentQueue = presentQueuePtr;
        }

        private static string[] CheckAvailableValidationLayers(string[] layers)
        {
            uint nrLayers = 0;
            vulkan.EnumerateInstanceLayerProperties(&nrLayers, null);

            LayerProperties[] availableLayers = new LayerProperties[nrLayers];

            vulkan.EnumerateInstanceLayerProperties(&nrLayers, availableLayers);

            var availableLayerNames = availableLayers.Select(availableLayer => Marshal.PtrToStringAnsi((nint)availableLayer.LayerName)).ToArray();

            if (layers.All(validationLayerName => availableLayerNames.Contains(validationLayerName)))
            {
                return layers;
            }

            return null;
        }

        private static string[] GetExtensionProperteis(PhysicalDevice device)
        {
            uint extensionCount = 0;
            vulkan.EnumerateDeviceExtensionProperties(device, (byte*)null, &extensionCount, null);
            ExtensionProperties[] availableExtensions = new ExtensionProperties[extensionCount];
            vulkan.EnumerateDeviceExtensionProperties(device, (byte*)null, &extensionCount, availableExtensions);

            var extensions = new string[extensionCount];
            for (int x = 0; x < extensionCount; x++)
            {
                var extension = availableExtensions[x];
                extensions[x] = Marshal.PtrToStringAnsi((nint)extension.ExtensionName);
            }

            return extensions;
        }

        private static uint FindGraphicsQueueFamily(PhysicalDevice tempPhysicalDevice)
        {
            uint queueFamilyCount = 0;
            vulkan.GetPhysicalDeviceQueueFamilyProperties(tempPhysicalDevice, &queueFamilyCount, null);
            QueueFamilyProperties[] graphicsFamily = new QueueFamilyProperties[queueFamilyCount];
            vulkan.GetPhysicalDeviceQueueFamilyProperties(tempPhysicalDevice, &queueFamilyCount, graphicsFamily);

            uint tempGraphicsFamily = 0;
            for (uint x = 0; x < graphicsFamily.Length; x++)
            {
                if (graphicsFamily[x].QueueFlags.HasFlag(QueueFlags.GraphicsBit))
                {
                    tempGraphicsFamily = x;
                    break;
                }
            }

            return tempGraphicsFamily;
        }

        private static uint FindPresentQueueFamily(PhysicalDevice tempPhysicalDevice)
        {
            uint queueFamilyCount = 0;
            vulkan.GetPhysicalDeviceQueueFamilyProperties(tempPhysicalDevice, &queueFamilyCount, null);
            QueueFamilyProperties[] presentFamily = new QueueFamilyProperties[queueFamilyCount];
            vulkan.GetPhysicalDeviceQueueFamilyProperties(tempPhysicalDevice, &queueFamilyCount, presentFamily);

            uint tempPresentFamily = 0;
            for (uint x = 0; x < presentFamily.Length; x++)
            {
                bool isPresentSupport = GetSurfaceSupport(tempPhysicalDevice, x);
                if (isPresentSupport)
                {
                    tempPresentFamily = x;
                    break;
                }
            }

            return tempPresentFamily;
        }

        private static SurfaceFormatKHR[] GetSurfaceFormatsKHR(PhysicalDevice tempPhysicalDevice)
        {
            uint surfaceFormatCount = 0;
            khrSurface.GetPhysicalDeviceSurfaceFormats(tempPhysicalDevice, surface, &surfaceFormatCount, null);
            SurfaceFormatKHR[] surfaceFormats = new SurfaceFormatKHR[surfaceFormatCount];
            khrSurface.GetPhysicalDeviceSurfaceFormats(tempPhysicalDevice, surface, &surfaceFormatCount, surfaceFormats);
            return surfaceFormats;
        }

        private static PresentModeKHR[] GetSurfacePresentModesKHR(PhysicalDevice tempPhysicalDevice)
        {
            uint surfaceFormatCount = 0;
            khrSurface.GetPhysicalDeviceSurfacePresentModes(tempPhysicalDevice, surface, &surfaceFormatCount, null);
            PresentModeKHR[] surfaceFormats = new PresentModeKHR[surfaceFormatCount];
            khrSurface.GetPhysicalDeviceSurfacePresentModes(tempPhysicalDevice, surface, &surfaceFormatCount, surfaceFormats);
            return surfaceFormats;
        }

        private static bool GetSurfaceSupport(PhysicalDevice tempPhysicalDevice, uint presentFamilyIndex)
        {
            Bool32 bool32 = false;
            // khrSurface.GetPhysicalDeviceSurfaceSupport(tempPhysicalDevice, presentFamilyIndex, surface, out true);
            return true;
        }

        public static  CommandBuffer BeginSingleUseCommandBuffer()
        {
            CommandBuffer commandBuffer = new CommandBuffer();
            CommandBufferAllocateInfo allocInfo = new CommandBufferAllocateInfo()
            {
                SType = StructureType.CommandBufferAllocateInfo,
                Level = CommandBufferLevel.Primary,
                CommandPool = commandPool,
                CommandBufferCount = 1
            };
            vulkan.AllocateCommandBuffers(device, &allocInfo, &commandBuffer);

            CommandBufferBeginInfo beginInfo = new CommandBufferBeginInfo()
            {
                SType = StructureType.CommandBufferBeginInfo,
                Flags = CommandBufferUsageFlags.OneTimeSubmitBit
            };
            vulkan.BeginCommandBuffer(commandBuffer, &beginInfo);
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
            vulkan.EndCommandBuffer(commandBuffer);
            vulkan.QueueSubmit(graphicsQueue, 1, &submitInfo, fence);
            vulkan.QueueWaitIdle(graphicsQueue);
            vulkan.FreeCommandBuffers(device, commandPool, 1, &commandBuffer);
            return Result.Success;
        }

    }
}
