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
using System.Windows.Forms;
using System.Xml.Linq;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using static System.Net.Mime.MediaTypeNames;

namespace VulkanGameEngineLevelEditor.Vulkan
{

    public unsafe static class SilkVulkanRenderer
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
        public static SilkVulkanSwapChain swapChain { get; private set; } = new SilkVulkanSwapChain();
        public static UInt32 ImageIndex { get; private set; } = new UInt32();
        public static UInt32 CommandIndex { get; private set; } = new UInt32();
        public static KhrSurface khrSurface { get; private set; }
        public static bool RebuildRendererFlag { get; private set; }

        public static string[] requiredExtensions;

        private static string[] instanceExtensions = { ExtDebugUtils.ExtensionName };
        private static string[] deviceExtensions = { KhrSwapchain.ExtensionName };
        private static string[] validationLayers = { "VK_LAYER_KHRONOS_validation" };
        public class SurfaceResult
        {
            public SurfaceKHR Surface { get; set; } = new SurfaceKHR();
            public KhrSurface KhrSurface { get; set; }
        }

        public static void CreateVulkanRenderer(IWindow windows, RichTextBox logTextBox)
        {
            window = windows;
            CreateWindow(window);
            CreateInstance(logTextBox);
            CreateSurface(window);
            CreatePhysicalDevice(khrSurface);
            CreateDevice();
            CreateDeviceQueue();
            CreateSemaphores();
            swapChain.CreateSwapChain(window, khrSurface, surface);
            CreateCommandPool();
        }

        public static void CreateVulkanRenderer(IWindow windows, SurfaceKHR surfacekhrt)
        {
            window = windows;
            CreateWindow(window);
            CreateInstance(null);
            CreateSurface(window);
            CreatePhysicalDevice(khrSurface);
            CreateDevice();
            CreateDeviceQueue();
            CreateSemaphores();
            swapChain.CreateSwapChain(window, khrSurface, surfacekhrt);
            CreateCommandPool();
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

                Result result = vulkan.CreateShaderModule(device, &createInfo, null, &shaderModule);
                if (result != Result.Success)
                {
                    Console.WriteLine($"Failed to create shader module: {result}");
                }
            }
            return shaderModule;
        }

        public static void CreateWindow(IWindow tempWindow)
        {
            window = tempWindow;
        }

        public static Instance CreateInstance(RichTextBox logTextBox)
        {

            validationLayers = CheckAvailableValidationLayers(validationLayers);
            if (validationLayers is null)
            {
                throw new NotSupportedException("Validation layers requested, but not available!");
            }

            var vulkanRequiredExtensions = window.VkSurface.GetRequiredExtensions(out uint extensions);
            requiredExtensions = new string[extensions];
            for (var x = 0; x < extensions; x++)
            {
                requiredExtensions[x] = Marshal.PtrToStringAnsi((IntPtr)vulkanRequiredExtensions[x]);
            }
            var extensionsArray = requiredExtensions.Concat(instanceExtensions).ToArray();

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

            createInfo.EnabledExtensionCount = (uint)extensionsArray.Length;
            createInfo.PpEnabledExtensionNames = (byte**)SilkMarshal.StringArrayToPtr(extensionsArray);

            if (validationLayers != null)
            {
                createInfo.EnabledLayerCount = (uint)validationLayers.Length;
                createInfo.PpEnabledLayerNames = (byte**)SilkMarshal.StringArrayToPtr(validationLayers);
            }

            DebugUtilsMessengerCreateInfoEXT debugCreateInfo = SilkVulkanDebug.MakeDebugUtilsMessengerCreateInfoEXT(logTextBox);
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

            return instance;
        }

        public static uint GetMemoryType(uint typeFilter, MemoryPropertyFlags properties)
        {
            PhysicalDeviceMemoryProperties memProperties;
            vulkan.GetPhysicalDeviceMemoryProperties(physicalDevice, &memProperties);

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

        public static SurfaceResult CreateSurface(IWindow windowtemp)
        {
            swapChain.swapchainExtent = new Extent2D((uint)windowtemp.FramebufferSize.X, (uint)windowtemp.FramebufferSize.Y);

            surface = windowtemp.VkSurface.Create<AllocationCallbacks>(((Instance)instance).ToHandle(), null).ToSurface();

            if (!VKConst.vulkan.TryGetInstanceExtension(instance, out KhrSurface khrSurface2))
            {
                throw new NotSupportedException("KHR_surface extension not found.");
            }
            khrSurface = khrSurface2;
            //surface = windowtemp.VkSurface.Create<AllocationCallbacks>(((Instance)instance).ToHandle(), null).ToSurface();

            //KhrSurface sufacekhr;
            //if (!vulkan.TryGetInstanceExtension(instance, out sufacekhr))
            //{
            //    throw new NotSupportedException("KHR_surface extension not found.");
            //}
            //khrSurface = sufacekhr;

            return new SurfaceResult
            {
                Surface = surface,
                KhrSurface = khrSurface
            };
        }

        public static PhysicalDevice CreatePhysicalDevice(KhrSurface tempsurface)
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
                    var formats = GetSurfaceFormatsKHR(tempPhysicalDevice, tempsurface);
                    var presentModes = GetSurfacePresentModesKHR(tempPhysicalDevice, tempsurface);
                    isSwapChainAdequate = (formats != null) && (presentModes != null);
                }

                vulkan.GetPhysicalDeviceFeatures(tempPhysicalDevice, out PhysicalDeviceFeatures supportedFeatures);
                if (isExtensionsSupported &&
                    isSwapChainAdequate &&
                    supportedFeatures.SamplerAnisotropy)
                {
                    physicalDevice = tempPhysicalDevice;
                    return physicalDevice;
                }
            }
            return physicalDevice;
        }

        public static Device CreateDevice()
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
            return device;
        }

        public static CommandPool CreateCommandPool()
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
            return commandPool;
        }

        public struct TimingThings
        {
            public Silk.NET.Vulkan.Semaphore[] acquire { get; set; }
            public Silk.NET.Vulkan.Semaphore[] present { get; set; }
            public Fence[] fences { get; set; }
        }
        public static TimingThings CreateSemaphores()
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

            return new TimingThings
            {
                acquire = AcquireImageSemaphores,
                present = PresentImageSemaphores,
                fences = InFlightFences
            };
        }

        public struct getQueues
        {
            public Silk.NET.Vulkan.Queue graphics { get; set; }
            public Silk.NET.Vulkan.Queue present { get; set; }
        }

        public static getQueues CreateDeviceQueue()
        {
            vulkan.GetDeviceQueue(device, GraphicsFamily, 0, out Silk.NET.Vulkan.Queue graphicsQueuePtr);
            vulkan.GetDeviceQueue(device, PresentFamily, 0, out Silk.NET.Vulkan.Queue presentQueuePtr);
            graphicsQueue = graphicsQueuePtr;
            presentQueue = presentQueuePtr;

            return new getQueues
            {
                graphics = graphicsQueue,
                present = presentQueue,
            };
        }

        public static string[] CheckAvailableValidationLayers(string[] layers)
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

        public static string[] GetExtensionProperteis(PhysicalDevice device)
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

        public static uint FindGraphicsQueueFamily(PhysicalDevice tempPhysicalDevice)
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

        public static uint FindPresentQueueFamily(PhysicalDevice tempPhysicalDevice)
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

        public static SurfaceFormatKHR[] GetSurfaceFormatsKHR(PhysicalDevice tempPhysicalDevice, KhrSurface khrSurface2)
        {
            uint surfaceFormatCount = 0;
            khrSurface2.GetPhysicalDeviceSurfaceFormats(tempPhysicalDevice, surface, &surfaceFormatCount, null);
            SurfaceFormatKHR[] surfaceFormats = new SurfaceFormatKHR[surfaceFormatCount];
            khrSurface2.GetPhysicalDeviceSurfaceFormats(tempPhysicalDevice, surface, &surfaceFormatCount, surfaceFormats);
            return surfaceFormats;
        }

        public static PresentModeKHR[] GetSurfacePresentModesKHR(PhysicalDevice tempPhysicalDevice, KhrSurface khrSurface2)
        {
            uint surfaceFormatCount = 0;
            khrSurface2.GetPhysicalDeviceSurfacePresentModes(tempPhysicalDevice, surface, &surfaceFormatCount, null);
            PresentModeKHR[] surfaceFormats = new PresentModeKHR[surfaceFormatCount];
            khrSurface2.GetPhysicalDeviceSurfacePresentModes(tempPhysicalDevice, surface, &surfaceFormatCount, surfaceFormats);
            return surfaceFormats;
        }

        public static bool GetSurfaceSupport(PhysicalDevice tempPhysicalDevice, uint presentFamilyIndex)
        {
            Bool32 bool32 = false;
            // khrSurface.GetPhysicalDeviceSurfaceSupport(tempPhysicalDevice, presentFamilyIndex, surface, out true);
            return true;
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
