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
using System.Runtime.InteropServices.ComTypes;
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
        public static void CreateVulkanRenderer(IntPtr window, Extent2D swapChainResolution)
        {
            windowPtr = window;
            CreateInstance();
            CreateSurface(windowPtr);
            CreatePhysicalDevice();
            CreateDevice();
            CreateDeviceQueue();
            CreateSemaphores();
            swapChain.CreateSwapChain(windowPtr);
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

                Result result = vk.CreateShaderModule(device, &createInfo, null, &shaderModule);
                if (result != Result.Success)
                {
                    Console.WriteLine($"Failed to create shader module: {result}");
                }
            }
            return shaderModule;
        }

        public static Instance CreateInstance()
        {
            validationLayers = CheckAvailableValidationLayers(validationLayers);
            if (validationLayers is null)
            {
                throw new NotSupportedException("Validation layers requested, but not available!");
            }

            uint extensionCount = 0;
            GetWin32Extensions(ref extensionCount, out string[] enabledExtensions);

            ApplicationInfo applicationInfo = new
            (
                pApplicationName: (byte*)Marshal.StringToHGlobalAnsi("Level Editor Play Test"),
                applicationVersion: new Version32(1, 0, 0),
                pEngineName: (byte*)Marshal.StringToHGlobalAnsi(""),
                engineVersion: new Version32(1, 0, 0),
                apiVersion: Vk.Version13
            );

            byte** validationLayersPtr = CreateStringPointerArray(validationLayers);
            byte** extensionsPtr = CreateStringPointerArray(enabledExtensions);

            var debugInfo = new DebugUtilsMessengerCreateInfoEXT
            (
                messageSeverity: DebugUtilsMessageSeverityFlagsEXT.VerboseBitExt |
                                 DebugUtilsMessageSeverityFlagsEXT.InfoBitExt |
                                 DebugUtilsMessageSeverityFlagsEXT.WarningBitExt |
                                 DebugUtilsMessageSeverityFlagsEXT.ErrorBitExt,
                messageType: DebugUtilsMessageTypeFlagsEXT.GeneralBitExt |
                             DebugUtilsMessageTypeFlagsEXT.ValidationBitExt |
                             DebugUtilsMessageTypeFlagsEXT.PerformanceBitExt,
                pfnUserCallback: new PfnDebugUtilsMessengerCallbackEXT(SilkVulkanDebug.MessageCallback)
            );

            InstanceCreateInfo vulkanCreateInfo = new InstanceCreateInfo
            {
                SType = StructureType.InstanceCreateInfo,
                PApplicationInfo = &applicationInfo,
                EnabledLayerCount = (uint)validationLayers.Length,
                PpEnabledLayerNames = validationLayersPtr, 
                EnabledExtensionCount = extensionCount,
                PpEnabledExtensionNames = extensionsPtr, 
                PNext = (void*)(&debugInfo) 
            };

            Result result = vk.CreateInstance(in vulkanCreateInfo, null, out Instance vkInstance);
            instance = vkInstance;
            if (result != Result.Success)
            {
                throw new Exception("Failed to create instance!");
            }

            Marshal.FreeHGlobal((nint)applicationInfo.PApplicationName);
            Marshal.FreeHGlobal((nint)applicationInfo.PEngineName);
            FreeStringPointerArray(validationLayersPtr, validationLayers.Length);

            return instance;
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

        public static PhysicalDevice CreatePhysicalDevice()
        {
            uint deviceCount = 0;
            vk.EnumeratePhysicalDevices(instance, &deviceCount, null);
            PhysicalDevice[] physicalDevices = new PhysicalDevice[deviceCount];
            vk.EnumeratePhysicalDevices(instance, &deviceCount, physicalDevices);

            foreach (var tempPhysicalDevice in physicalDevices)
            {
                string[] availableExtensions = GetExtensionProperteis(tempPhysicalDevice);
                bool isExtensionsSupported = deviceExtensions.All(e => availableExtensions.Contains(e));
                uint tempGraphicsFamily = FindGraphicsQueueFamily(tempPhysicalDevice);
                uint tempPresentFamily = FindPresentQueueFamily(tempPhysicalDevice);

                bool isSwapChainAdequate = false;
                if (isExtensionsSupported)
                {
                    var surfaceFormats = GetSurfaceFormatsKHR(tempPhysicalDevice);
                    var presentModes = GetSurfacePresentModesKHR(tempPhysicalDevice);
                    isSwapChainAdequate = (surfaceFormats != null) && (presentModes != null);
                }

                vk.GetPhysicalDeviceFeatures(tempPhysicalDevice, out PhysicalDeviceFeatures supportedFeatures);
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
                SamplerAnisotropy = true,

                DepthBiasClamp = true,
                DepthBounds = true,
                DepthClamp = true
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

            var result = vk.CreateDevice(physicalDevice, in createInfo, null, out Device devicePtr);
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
            vk.CreateCommandPool(device, &CommandPoolCreateInfo, null, &commandpool);
            commandPool = commandpool;
            return commandPool;
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
            vk.GetDeviceQueue(device, GraphicsFamily, 0, out Silk.NET.Vulkan.Queue graphicsQueuePtr);
            vk.GetDeviceQueue(device, PresentFamily, 0, out Silk.NET.Vulkan.Queue presentQueuePtr);
            graphicsQueue = graphicsQueuePtr;
            presentQueue = presentQueuePtr;
        }

        public static string[] CheckAvailableValidationLayers(string[] layers)
        {
            uint nrLayers = 0;
            vk.EnumerateInstanceLayerProperties(&nrLayers, null);

            LayerProperties[] availableLayers = new LayerProperties[nrLayers];

            vk.EnumerateInstanceLayerProperties(&nrLayers, availableLayers);

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
            vk.EnumerateDeviceExtensionProperties(device, (byte*)null, &extensionCount, null);
            ExtensionProperties[] availableExtensions = new ExtensionProperties[extensionCount];
            vk.EnumerateDeviceExtensionProperties(device, (byte*)null, &extensionCount, availableExtensions);

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
            vk.GetPhysicalDeviceQueueFamilyProperties(tempPhysicalDevice, &queueFamilyCount, null);
            QueueFamilyProperties[] graphicsFamily = new QueueFamilyProperties[queueFamilyCount];
            vk.GetPhysicalDeviceQueueFamilyProperties(tempPhysicalDevice, &queueFamilyCount, graphicsFamily);

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
            vk.GetPhysicalDeviceQueueFamilyProperties(tempPhysicalDevice, &queueFamilyCount, null);
            QueueFamilyProperties[] presentFamily = new QueueFamilyProperties[queueFamilyCount];
            vk.GetPhysicalDeviceQueueFamilyProperties(tempPhysicalDevice, &queueFamilyCount, presentFamily);

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

        [DllImport("vulkan-1.dll")]
        public static extern int vkGetPhysicalDeviceSurfaceFormatsKHR(PhysicalDevice physicalDevice, SurfaceKHR surface, ref uint pSurfaceFormatCount, IntPtr pSurfaceFormats);
        public static SurfaceFormatKHR[] GetSurfaceFormatsKHR(PhysicalDevice physicalDevice)
        {
            uint formatCount = 0;
            vkGetPhysicalDeviceSurfaceFormatsKHR(physicalDevice, surface, ref formatCount, IntPtr.Zero);

            var formats = new SurfaceFormatKHR[formatCount];
            int structureSize = Marshal.SizeOf<SurfaceFormatKHR>();
            IntPtr formatPtr = Marshal.AllocHGlobal(structureSize * (int)formatCount);
            try
            {
                vkGetPhysicalDeviceSurfaceFormatsKHR(physicalDevice, surface, ref formatCount, formatPtr);
                for (int x = 0; x < formatCount; x++)
                {
                    formats[x] = Marshal.PtrToStructure<SurfaceFormatKHR>(formatPtr + x * Marshal.SizeOf<SurfaceFormatKHR>());
                    Console.WriteLine($"Format: {formats[x].Format}, Color Space: {formats[x].ColorSpace}");
                }
            }
            finally
            {
                Marshal.FreeHGlobal(formatPtr);
            }
            return formats;
        }


        [DllImport("vulkan-1.dll")]
        public static extern int vkGetPhysicalDeviceSurfacePresentModesKHR(PhysicalDevice physicalDevice, SurfaceKHR surface, ref uint pPresentModeCount, IntPtr pPresentModes);
        public static PresentModeKHR[] GetSurfacePresentModesKHR(PhysicalDevice physicalDevice)
        {
            uint presentModeCount = 0;
            vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice, surface, ref presentModeCount, IntPtr.Zero);

            var presentModes = new PresentModeKHR[presentModeCount];
            IntPtr presentModePtr = Marshal.AllocHGlobal((int)presentModeCount * sizeof(int));
            try
            {
                vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice, surface, ref presentModeCount, presentModePtr);
                for (int x = 0; x < presentModeCount; x++)
                {
                    presentModes[x] = (PresentModeKHR)Marshal.ReadInt32(presentModePtr + x * sizeof(int));
                    Console.WriteLine($"Present Mode: {presentModes[x]}");
                }
            }
            finally
            {
                Marshal.FreeHGlobal(presentModePtr);
            }
            return presentModes;
        }


        public static bool GetSurfaceSupport(PhysicalDevice tempPhysicalDevice, uint presentFamilyIndex)
        {
            Bool32 bool32 = false;
            // khrSurface.GetPhysicalDeviceSurfaceSupport(tempPhysicalDevice, presentFamilyIndex, surface, out true);
            return true;
        }

        [DllImport("vulkan-1.dll")]
        public static extern Result vkEnumerateInstanceExtensionProperties(IntPtr pLayerName, ref uint pPropertyCount, IntPtr pProperties);
        public static void GetWin32Extensions(ref uint extensionCount, out string[] enabledExtensions)
        {
            enabledExtensions = null;
            Result result = vkEnumerateInstanceExtensionProperties(IntPtr.Zero, ref extensionCount, IntPtr.Zero);
            if (result != Result.Success)
            {
                MessageBox.Show($"Failed to enumerate instance extension properties. Error: {result}");
                return;
            }

            IntPtr extensionsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<ExtensionProperties>() * (int)extensionCount);
            try
            {
                result = vkEnumerateInstanceExtensionProperties(IntPtr.Zero, ref extensionCount, extensionsPtr);
                if (result != Result.Success)
                {
                    MessageBox.Show($"Failed to retrieve instance extension properties. Error: {result}");
                    return;
                }

                string[] extensionNames = new string[extensionCount];
                for (uint x = 0; x < extensionCount; x++)
                {
                    ExtensionProperties extProps = Marshal.PtrToStructure<ExtensionProperties>(
                        IntPtr.Add(extensionsPtr, (int)(x * Marshal.SizeOf<ExtensionProperties>())));

                    string name = BytePtrToString(extProps.ExtensionName);
                    extensionNames[x] = name;

                    byte[] nameBytes = Encoding.UTF8.GetBytes(name);
                    IntPtr ptr = Marshal.AllocHGlobal(nameBytes.Length + 1);
                    Marshal.Copy(nameBytes, 0, ptr, nameBytes.Length);
                    Marshal.WriteByte(ptr, nameBytes.Length, 0);
                }
                enabledExtensions = extensionNames;
            }
            finally
            {
                Marshal.FreeHGlobal(extensionsPtr);
            }
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

        public static unsafe string BytePtrToString(byte* bytePtr)
        {
            if (bytePtr == null)
            {
                throw new ArgumentNullException(nameof(bytePtr));
            }

            int length = 0;
            while (bytePtr[length] != 0)
            {
                length++;
                if (length > 256)
                    throw new InvalidOperationException("String length exceeds expected limit");
            }

            byte[] bytes = new byte[length];
            for (int i = 0; i < length; i++)
            {
                bytes[i] = bytePtr[i];
            }

            return Encoding.UTF8.GetString(bytes);
        }

        private static unsafe byte** CreateStringPointerArray(string[] strings)
        {
            int count = strings.Length;
            byte** pointerArray = (byte**)Marshal.AllocHGlobal(count * sizeof(byte*));

            for (int i = 0; i < count; i++)
            {
                byte* stringPointer = CreateBytePointerForStrings(new[] { strings[i] });
                pointerArray[i] = stringPointer;
            }
            return pointerArray;
        }

        private static unsafe byte* CreateBytePointerForStrings(string[] strings)
        {
            int totalLength = 0;
            foreach (var str in strings)
            {
                totalLength += Encoding.UTF8.GetByteCount(str) + 1; // +1 for null terminator
            }

            byte* byteArray = (byte*)Marshal.AllocHGlobal(totalLength);
            byte* currentPointer = byteArray;

            foreach (var str in strings)
            {
                byte[] stringBytes = Encoding.UTF8.GetBytes(str);
                Marshal.Copy(stringBytes, 0, new IntPtr(currentPointer), stringBytes.Length);
                currentPointer[stringBytes.Length] = 0; // Null-terminate
                currentPointer += stringBytes.Length + 1; // Move to the next position
            }

            return byteArray;
        }

        private static unsafe void FreeStringPointerArray(byte** pointerArray, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Marshal.FreeHGlobal((IntPtr)pointerArray[i]);
            }
            Marshal.FreeHGlobal((IntPtr)pointerArray);
        }

    }
}
