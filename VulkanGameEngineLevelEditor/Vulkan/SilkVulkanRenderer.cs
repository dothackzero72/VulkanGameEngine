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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VulkanGameEngineLevelEditor.GameEngineAPI;

namespace VulkanGameEngineLevelEditor
{

    public unsafe static class VulkanRenderer
    {
        public static Vk vulkan = Vk.GetApi();
        public static IWindow window { get; set; }
        public static Instance instance { get; private set; }
        public static DebugUtilsMessengerEXT debugMessenger { get; private set; }
        public static SurfaceKHR surface { get; private set; }
        public static PhysicalDevice physicalDevice { get; private set; }
        public static uint GraphicsFamily { get; private set; }
        public static uint PresentFamily { get; private set; }
        public static Device device { get; private set; }
        public static Queue graphicsQueue { get; private set; }
        public static Queue presentQueue { get; private set; }
        public static VulkanSwapChain swapChain { get; private set; } = new VulkanSwapChain();

        public static KhrSurface khrSurface { get; private set; }

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
            CreateDebugger();
            CreateSurface();
            CreatePhysicalDevice();
            CreateDevice();
            CreateDeviceQueue();
            swapChain.CreateSwapChain();
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

            DebugUtilsMessengerCreateInfoEXT info = VulkanDebug.MakeDebugUtilsMessengerCreateInfoEXT();
            createInfo.PNext = &info;


            Result result = vulkan.CreateInstance(in createInfo, null, out Instance vkInstance);
            instance = vkInstance;
            if (result != Result.Success)
            {
                throw new Exception("Failed to create instance!");
            }

            SilkMarshal.Free((nint)createInfo.PpEnabledLayerNames);
            SilkMarshal.Free((nint)createInfo.PpEnabledExtensionNames);

            Marshal.FreeHGlobal((nint)applicationInfo.PApplicationName);
            Marshal.FreeHGlobal((nint)applicationInfo.PEngineName);
        }

        private static void CreateDebugger()
        {
            DebugUtilsMessengerCreateInfoEXT debugCreateInfo = new DebugUtilsMessengerCreateInfoEXT();
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

        private static void CreateDeviceQueue()
        {
            vulkan.GetDeviceQueue(device, GraphicsFamily, 0, out Queue graphicsQueuePtr);
            vulkan.GetDeviceQueue(device, PresentFamily, 0, out Queue presentQueuePtr);
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

        public static SurfaceFormatKHR[] GetSurfaceFormatsKHR(PhysicalDevice tempPhysicalDevice)
        {
            uint surfaceFormatCount = 0;
            khrSurface.GetPhysicalDeviceSurfaceFormats(tempPhysicalDevice, surface, &surfaceFormatCount, null);
            SurfaceFormatKHR[] surfaceFormats = new SurfaceFormatKHR[surfaceFormatCount];
            khrSurface.GetPhysicalDeviceSurfaceFormats(tempPhysicalDevice, surface, &surfaceFormatCount, surfaceFormats);
            return surfaceFormats;
        }

        public static PresentModeKHR[] GetSurfacePresentModesKHR(PhysicalDevice tempPhysicalDevice)
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
    }
}
