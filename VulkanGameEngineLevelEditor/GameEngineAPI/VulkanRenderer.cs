using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.GameEngineAPI;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public static unsafe class VulkanRenderer
    {


        public const int MAX_FRAMES_IN_FLIGHT = 3;
        public static bool RebuildRendererFlag { get; set; } = false;
        public static IntPtr WindowHandleCopy = IntPtr.Zero;
        public static VkInstance Instance { get; private set; } = new VkInstance();
        public static VkDevice Device { get; private set; } = new VkDevice();
        public static VkPhysicalDevice PhysicalDevice { get; private set; } = new VkPhysicalDevice();
        public static VkSurfaceKHR Surface { get; private set; } = new VkSurfaceKHR();
        public static VkCommandPool CommandPool { get; private set; } = new VkCommandPool();
        public static UInt32 ImageIndex { get; private set; } = new UInt32();
        public static UInt32 CommandIndex { get; private set; } = new UInt32();
        public static VkDebugUtilsMessengerEXT DebugMessenger { get; private set; } = new VkDebugUtilsMessengerEXT();
        public static VkPhysicalDeviceFeatures PhysicalDeviceFeatures { get; private set; } = new VkPhysicalDeviceFeatures();
        public static List<VkFence> InFlightFences { get; private set; } = new List<VkFence>();
        public static List<VkSemaphore> AcquireImageSemaphores { get; private set; } = new List<VkSemaphore>();
        public static List<VkSemaphore> PresentImageSemaphores { get; private set; } = new List<VkSemaphore>();
        public static UInt32 SwapChainImageCount { get; private set; } = new UInt32();
        public static UInt32 GraphicsFamily { get; private set; } = new UInt32();
        public static UInt32 PresentFamily { get; private set; } = new UInt32();
        public static VkQueue GraphicsQueue { get; private set; } = new VkQueue();
        public static VkQueue PresentQueue { get; private set; } = new VkQueue();
        public static VkFormat Format { get; private set; } = new VkFormat();
        public static VkColorSpaceKHR ColorSpace { get; private set; } = new VkColorSpaceKHR();
        public static VkPresentModeKHR PresentMode { get; private set; } = new VkPresentModeKHR();
        public static List<VkImage> SwapChainImages { get; private set; } = new List<VkImage> { };
        public static List<VkImageView> SwapChainImageViews { get; private set; } = new List<VkImageView> { };
        public static VkExtent2D SwapChainResolution { get; private set; } = new VkExtent2D();
        public static VkSwapchainKHR Swapchain { get; private set; } = new VkSwapchainKHR();


        public static void SetUpRenderer(IntPtr handle, PictureBox pictureBox)
        {
            WindowHandleCopy = handle;
            CreateVulkanInstance();
            SetupDebugMessenger();
            CreateVulkanSurface(pictureBox);
            SetUpPhysicalDevice();
            SetUpDevice();
            VkSurfaceCapabilitiesKHR surfaceCapabilities = GetSurfaceCapabilities();
            List<VkSurfaceFormatKHR> surfaceFormatList = GetPhysicalDeviceFormats();
            GetQueueFamilies();
            List<VkPresentModeKHR> presentModeList = GetPhysicalDevicePresentModes();
            VkSurfaceFormatKHR swapChainImageFormat = FindSwapSurfaceFormat(surfaceFormatList);
            VkPresentModeKHR presentMode = FindSwapPresentMode(presentModeList);
            GetFrameBufferSize(pictureBox);
            SetUpSwapChain(surfaceCapabilities, swapChainImageFormat, presentMode);
            SetUpSwapChainImages();
            SetUpSwapChainImageViews(swapChainImageFormat);
            SetUpCommandPool();
            SetUpSemaphores();
            GetDeviceQueue();
        }

        public static void CreateVulkanInstance()
        {
            try
            {
                Instance = GameEngineDLL.DLL_Renderer_CreateVulkanInstance();
                if (Instance == IntPtr.Zero)
                {
                    MessageBox.Show("Failed to create Vulkan instance.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred: {ex.Message}");
            }
        }

        private static void SetupDebugMessenger()
        {
            try
            {
                DebugMessenger = GameEngineDLL.DLL_Renderer_SetupDebugMessenger(Instance);
                if (DebugMessenger == IntPtr.Zero)
                {
                    MessageBox.Show("Failed to set up debug messenger.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred: {ex.Message}");
            }
        }

        public static void CreateVulkanSurface(PictureBox pictureBox)
        {
            var surfaceCreateInfo = new VkWin32SurfaceCreateInfoKHR
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_WIN32_SURFACE_CREATE_INFO_KHR,
                hwnd = pictureBox.Handle,
                hinstance = Marshal.GetHINSTANCE(typeof(Form1).Module)
            };

            VkSurfaceKHR surface = IntPtr.Zero;
            if (VulkanAPI.vkCreateWin32SurfaceKHR(Instance, ref surfaceCreateInfo, IntPtr.Zero, out surface) != 0)
            {
                MessageBox.Show("Failed to create Vulkan surface.");
                return;
            }
            Surface = surface;
        }

        private static void SetUpPhysicalDevice()
        {
            try
            {
                VkPhysicalDevice physicalDevice = VkPhysicalDevice.Zero;
                VkPhysicalDeviceFeatures physicalDeviceFeatures = new VkPhysicalDeviceFeatures();
                UInt32 graphicsFamily = new UInt32();
                UInt32 presentFamily = new UInt32();

                if (GameEngineDLL.DLL_Renderer_SetUpPhysicalDevice(Instance, ref physicalDevice, Surface, ref physicalDeviceFeatures, out graphicsFamily, out presentFamily) != VkResult.VK_SUCCESS)
                {
                    MessageBox.Show("Physical Device setup failed.");
                    return;
                }

                PhysicalDevice = physicalDevice;
                PhysicalDeviceFeatures = physicalDeviceFeatures;
                GraphicsFamily = graphicsFamily;
                PresentFamily = presentFamily;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred: {ex.Message}");
            }
        }

        private static void SetUpDevice()
        {
            try
            {
                Device = GameEngineDLL.DLL_Renderer_SetUpDevice(PhysicalDevice, GraphicsFamily, PresentFamily);
                if (Device == IntPtr.Zero)
                {
                    MessageBox.Show("Failed to set up Vulkan device.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred: {ex.Message}");
            }
        }

        private static void SetUpCommandPool()
        {
            try
            {
                CommandPool = GameEngineDLL.DLL_Renderer_SetUpCommandPool(Device, GraphicsFamily);
                if (CommandPool == IntPtr.Zero)
                {
                    MessageBox.Show("Failed to set up command pool.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred: {ex.Message}");
            }
        }

        private static void SetUpSemaphores()
        {
            try
            {
                IntPtr inFlightFencesPtr = IntPtr.Zero;
                IntPtr acquireImageSemaphoresPtr = IntPtr.Zero;
                IntPtr presentImageSemaphoresPtr = IntPtr.Zero;

                if (GameEngineDLL.DLL_Renderer_SetUpSemaphores(Device, out inFlightFencesPtr, out acquireImageSemaphoresPtr, out presentImageSemaphoresPtr, MAX_FRAMES_IN_FLIGHT) != VkResult.VK_SUCCESS)
                {
                    MessageBox.Show("Failed to set up semaphores.");
                    return;
                }

                VkFence[] inFlightFences = new VkFence[MAX_FRAMES_IN_FLIGHT];
                Marshal.Copy(inFlightFencesPtr, inFlightFences, 0, MAX_FRAMES_IN_FLIGHT);
                InFlightFences = inFlightFences.ToList();

                VkSemaphore[] acquireImageSemaphores = new VkSemaphore[MAX_FRAMES_IN_FLIGHT];
                Marshal.Copy(acquireImageSemaphoresPtr, acquireImageSemaphores, 0, MAX_FRAMES_IN_FLIGHT);
                AcquireImageSemaphores = acquireImageSemaphores.ToList();

                VkSemaphore[] presentImageSemaphores = new VkSemaphore[MAX_FRAMES_IN_FLIGHT];
                Marshal.Copy(presentImageSemaphoresPtr, presentImageSemaphores, 0, MAX_FRAMES_IN_FLIGHT);
                PresentImageSemaphores = presentImageSemaphores.ToList();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred: {ex.Message}");
            }
        }

        private static void GetDeviceQueue()
        {
            try
            {
                VkQueue graphicsQueue = new VkQueue();
                VkQueue presentQueue = new VkQueue();
                if (GameEngineDLL.DLL_Renderer_GetDeviceQueue(Device, GraphicsFamily, PresentFamily, out graphicsQueue, out presentQueue) != VkResult.VK_SUCCESS)
                {
                    MessageBox.Show("Failed to get device queues.");
                    return;
                }
                GraphicsQueue = graphicsQueue;
                PresentQueue = presentQueue;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred: {ex.Message}");
            }
        }

        private static  List<VkSurfaceFormatKHR> GetSurfaceFormats()
        {
            try
            {
                uint surfaceFormatCount = 0;
                VkResult result = GameEngineDLL.DLL_Renderer_GetSurfaceFormats(PhysicalDevice, Surface, null, ref surfaceFormatCount);
                if (result != VkResult.VK_SUCCESS || surfaceFormatCount == 0)
                {
                    MessageBox.Show($"Failed to get surface format count or no formats available: {result}");
                    return new List<VkSurfaceFormatKHR>();
                }

                VkSurfaceFormatKHR[] surfaceFormat = new VkSurfaceFormatKHR[surfaceFormatCount];
                result = GameEngineDLL.DLL_Renderer_GetSurfaceFormats(PhysicalDevice, Surface, surfaceFormat, ref surfaceFormatCount);
                if (result != VkResult.VK_SUCCESS || surfaceFormatCount == 0)
                {
                    MessageBox.Show($"Failed to get surface formats: {result}");
                    return new List<VkSurfaceFormatKHR>();
                }

                return surfaceFormat.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred: {ex.Message}");
                return new List<VkSurfaceFormatKHR>();
            }
        }

        private static List<VkPresentModeKHR> GetPresentModes()
        {
            try
            {
                uint presentModeCount = 0;
                VkResult result = GameEngineDLL.DLL_Renderer_GetPresentModes(PhysicalDevice, Surface, null, ref presentModeCount);
                if (result != VkResult.VK_SUCCESS || presentModeCount == 0)
                {
                    MessageBox.Show($"Failed to get surface format count or no formats available: {result}");
                    return new List<VkPresentModeKHR>();
                }

                VkPresentModeKHR[] presentFormat = new VkPresentModeKHR[presentModeCount];
                result = GameEngineDLL.DLL_Renderer_GetPresentModes(PhysicalDevice, Surface, presentFormat, ref presentModeCount);
                if (result != VkResult.VK_SUCCESS || presentModeCount == 0)
                {
                    MessageBox.Show($"Failed to get surface formats: {result}");
                    return new List<VkPresentModeKHR>();
                }

                return presentFormat.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred: {ex.Message}");
            }
            return new List<VkPresentModeKHR>();
        }

        private static VkSurfaceFormatKHR FindSwapSurfaceFormat(List<VkSurfaceFormatKHR> surfaceFormatList)
        {
            try
            {
                VkSurfaceFormatKHR surfaceFormat = GameEngineDLL.DLL_SwapChain_FindSwapSurfaceFormat(surfaceFormatList.ToArray(), ((uint)surfaceFormatList.Count));
                return surfaceFormat;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred: {ex.Message}");
                return new VkSurfaceFormatKHR
                {
                    colorSpace = VkColorSpaceKHR.VK_COLOR_SPACE_MAX_ENUM_KHR,
                    format = VkFormat.VK_FORMAT_UNDEFINED
                };
            }
        }

        private static VkPresentModeKHR FindSwapPresentMode(List<VkPresentModeKHR> presentModeList)
        {
            try
            {
                return GameEngineDLL.DLL_SwapChain_FindSwapPresentMode(presentModeList.ToArray(), (uint)presentModeList.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred: {ex.Message}");
                return VkPresentModeKHR.VK_PRESENT_MODE_FIFO_KHR;
            }
        }

        private static void GetQueueFamilies()
        {
            try
            {
                uint graphicsFamily;
                uint presentFamily;
                VkResult result = GameEngineDLL.DLL_SwapChain_GetQueueFamilies(PhysicalDevice, Surface, out graphicsFamily, out presentFamily);
                GraphicsFamily = graphicsFamily;
                PresentFamily = presentFamily;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred: {ex.Message}");
            }
        }

        private static VkSurfaceCapabilitiesKHR GetSurfaceCapabilities()
        {
            try
            {
                VkSurfaceCapabilitiesKHR surfaceFormats = new VkSurfaceCapabilitiesKHR();
                VkResult result = GameEngineDLL.DLL_SwapChain_GetSurfaceCapabilities(PhysicalDevice, Surface, out surfaceFormats);
                if (result != VkResult.VK_SUCCESS)
                {
                    MessageBox.Show($"Failed to get surface format count or no formats available: {result}");
                    return new VkSurfaceCapabilitiesKHR();
                }
                return surfaceFormats;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred: {ex.Message}");
                return new VkSurfaceCapabilitiesKHR();
            }
        }

        private static List<VkSurfaceFormatKHR> GetPhysicalDeviceFormats()
        {
            uint surfaceFormatCount = 0;
            IntPtr compatibleFormatsPtr = IntPtr.Zero;

            IntPtr compatibleFormatsPointer = Marshal.AllocHGlobal(IntPtr.Size);
            VkResult result = GameEngineDLL.DLL_SwapChain_GetPhysicalDeviceFormats(PhysicalDevice, Surface, compatibleFormatsPointer, ref surfaceFormatCount);
            if (result != VkResult.VK_SUCCESS)
            {
                Console.WriteLine("Failed to get the surface format count.");
                return new List<VkSurfaceFormatKHR>();
            }

            compatibleFormatsPtr = Marshal.ReadIntPtr(compatibleFormatsPointer);
            if (surfaceFormatCount == 0 || compatibleFormatsPtr == IntPtr.Zero)
            {
                Console.WriteLine("No compatible swap chain formats.");
                return new List<VkSurfaceFormatKHR>();
            }

            VkSurfaceFormatKHR[] compatibleFormats = new VkSurfaceFormatKHR[surfaceFormatCount];
            for (uint x = 0; x < surfaceFormatCount; x++)
            {
                IntPtr currentFormatPtr = IntPtr.Add(compatibleFormatsPtr, (int)(x * Marshal.SizeOf<VkSurfaceFormatKHR>()));
                compatibleFormats[x] = Marshal.PtrToStructure<VkSurfaceFormatKHR>(currentFormatPtr);
            }
            return compatibleFormats.ToList();
        }

        private static List<VkPresentModeKHR> GetPhysicalDevicePresentModes()
        {
            uint presentModeCount = 0;
            IntPtr presentModesPointer = IntPtr.Zero;
            IntPtr presentModesPtr = Marshal.AllocHGlobal(IntPtr.Size);

            VkResult result = GameEngineDLL.DLL_SwapChain_GetPhysicalDevicePresentModes(PhysicalDevice, Surface, presentModesPtr, out presentModeCount);
            if (result != VkResult.VK_SUCCESS || presentModeCount == 0)
            {
                MessageBox.Show($"Failed to get present mode count or no present modes available: {result}");
                return new List<VkPresentModeKHR>();
            }

            presentModesPointer = Marshal.ReadIntPtr(presentModesPtr);
            if (presentModesPointer == IntPtr.Zero)
            {
                MessageBox.Show("No compatible present modes found.");
                return new List<VkPresentModeKHR>();
            }

            List<VkPresentModeKHR> presentModes = new List<VkPresentModeKHR>();
            for (uint x = 0; x < presentModeCount; x++)
            {
                IntPtr currentPresentModePtr = IntPtr.Add(presentModesPointer, (int)(x * sizeof(int)));
                VkPresentModeKHR presentMode = (VkPresentModeKHR)Marshal.ReadInt32(currentPresentModePtr);
                presentModes.Add(presentMode);
            }
            return presentModes;
        }

        private static void GetFrameBufferSize(PictureBox pictureBox)
        {
            SwapChainResolution = new VkExtent2D
            {   
                Width = (uint)pictureBox.Width,
                Height = (uint)pictureBox.Height
            };
        }
        private static void SetUpSwapChain(VkSurfaceCapabilitiesKHR surfaceCapabilities, VkSurfaceFormatKHR surfaceFormat, VkPresentModeKHR presentMode)
        {
            try
            {
                uint swapChainImageCount = 0;
                Swapchain = GameEngineDLL.DLL_SwapChain_SetUpSwapChain(Device, Surface, surfaceCapabilities, surfaceFormat, presentMode, GraphicsFamily, PresentFamily, SwapChainResolution.Width, SwapChainResolution.Height, out swapChainImageCount);
                SwapChainImageCount = swapChainImageCount;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred: {ex.Message}");
            }
        }

        private static void SetUpSwapChainImages()
        {
            try
            {
                uint swapChainImageCount = SwapChainImageCount;
                IntPtr swapChainImagesPtr = GameEngineDLL.DLL_SwapChain_SetUpSwapChainImages(Device, Swapchain, swapChainImageCount);
                if (swapChainImagesPtr == IntPtr.Zero)
                {
                    MessageBox.Show("Failed to retrieve swap chain images.");
                    return;
                }

                List<VkImage> swapChainImages = new List<VkImage>();
                for (uint x = 0; x < swapChainImageCount; x++)
                {
                    IntPtr currentImagePtr = IntPtr.Add(swapChainImagesPtr, (int)(x * IntPtr.Size));
                    VkImage image = Marshal.PtrToStructure<VkImage>(currentImagePtr); 
                    swapChainImages.Add(image);
                }
                SwapChainImages = swapChainImages;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred: {ex.Message}");
            }
        }

        private static void SetUpSwapChainImageViews(VkSurfaceFormatKHR surfaceFormat)
        {
            try
            {
                uint swapChainImageCount = SwapChainImageCount;
                IntPtr swapChainViewPtr = GameEngineDLL.DLL_SwapChain_SetUpSwapChainImages(Device, Swapchain, swapChainImageCount);
                if (swapChainViewPtr == IntPtr.Zero)
                {
                    MessageBox.Show("Failed to retrieve swap chain images.");
                    return;
                }

                List<VkImageView> swapChainView = new List<VkImageView>();
                for (uint x = 0; x < swapChainImageCount; x++)
                {
                    IntPtr currentImagePtr = IntPtr.Add(swapChainViewPtr, (int)(x * IntPtr.Size));
                    VkImageView image = Marshal.PtrToStructure<VkImageView>(currentImagePtr);
                    swapChainView.Add(image);
                }
                SwapChainImageViews = swapChainView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred: {ex.Message}");
            }
        }
    }
}

