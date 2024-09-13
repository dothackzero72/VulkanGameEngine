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
    public unsafe class VulkanRenderer
    {
        public const int MAX_FRAMES_IN_FLIGHT = 3;
        public IntPtr WindowHandleCopy = IntPtr.Zero;
        public VkInstance Instance { get; private set; } = new VkInstance();
        public VkDevice Device { get; private set; } = new VkDevice();
        public VkPhysicalDevice PhysicalDevice { get; private set; } = new VkPhysicalDevice();
        public VkSurfaceKHR Surface { get; private set; } = new VkSurfaceKHR();
        public VkCommandPool CommandPool { get; private set; } = new VkCommandPool();
        public UInt32 ImageIndex { get; private set; } = new UInt32();
        public UInt32 CommandIndex { get; private set; } = new UInt32();
        public VkDebugUtilsMessengerEXT DebugMessenger { get; private set; } = new VkDebugUtilsMessengerEXT();
        // SwapChainState SwapChain;
        public VkPhysicalDeviceFeatures PhysicalDeviceFeatures { get; private set; } = new VkPhysicalDeviceFeatures();

        public List<VkFence> InFlightFences { get; private set; } = new List<VkFence>();
        public List<VkSemaphore> AcquireImageSemaphores { get; private set; } = new List<VkSemaphore>();
        public List<VkSemaphore> PresentImageSemaphores { get; private set; } = new List<VkSemaphore>();
        bool RebuildRendererFlag;

        public UInt32 SwapChainImageCount { get; private set; } = new UInt32();
        public UInt32 GraphicsFamily { get; private set; } = new UInt32();
        public UInt32 PresentFamily { get; private set; } = new UInt32();
        public VkQueue GraphicsQueue { get; private set; } = new VkQueue();
        public VkQueue PresentQueue { get; private set; } = new VkQueue();
        public VkFormat Format { get; private set; } = new VkFormat();
        public VkColorSpaceKHR ColorSpace { get; private set; } = new VkColorSpaceKHR();
        public VkPresentModeKHR PresentMode { get; private set; } = new VkPresentModeKHR();
        public List<VkImage> SwapChainImages { get; private set; } = new List<VkImage> { };
        public List<VkImageView> SwapChainImageViews { get; private set; } = new List<VkImageView> { };
        public VkExtent2D SwapChainResolution { get; private set; } = new VkExtent2D();
        public VkSwapchainKHR Swapchain { get; private set; } = new VkSwapchainKHR();

        public VulkanRenderer()
        {

        }

        public void SetUpRenderer(IntPtr handle)
        {
            WindowHandleCopy = handle;
            CreateVulkanInstance();
            SetupDebugMessenger();
            CreateVulkanSurface();
            SetUpPhysicalDevice();
            SetUpDevice();
        }

        private void CreateVulkanInstance()
        {
            try
            {
                Instance = GameEngineDLL.DLL_Renderer_CreateVulkanInstance();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred: {ex.Message}");
            }
        }

        private void SetupDebugMessenger()
        {
            try
            {
                DebugMessenger = GameEngineDLL.DLL_Renderer_SetupDebugMessenger(Instance);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred: {ex.Message}");
            }
        }

        public void CreateVulkanSurface()
        {
            var surfaceCreateInfo = new VkWin32SurfaceCreateInfoKHR
            {
                sType = 0,
                hwnd = WindowHandleCopy,
                hinstance = System.Diagnostics.Process.GetCurrentProcess().MainModule.BaseAddress
            };

            VkSurfaceKHR surface = UIntPtr.Zero;
            if (VulkanAPI.vkCreateWin32SurfaceKHR(VulkanInstance.Instance, ref surfaceCreateInfo, IntPtr.Zero, out surface) != 0)
            {
                MessageBox.Show("Failed to create Vulkan surface.");
                return;
            }
            VulkanInstance.Surface = surface;
        }

        private void SetUpPhysicalDevice()
        {
            try
            {
                VkPhysicalDevice physicalDevice = VkPhysicalDevice.Zero;
                VkPhysicalDeviceFeatures physicalDeviceFeatures = new VkPhysicalDeviceFeatures();
                UInt32 graphicsFamily = new UInt32();
                UInt32 presentFamily = new UInt32();

                if(GameEngineDLL.DLL_Renderer_SetUpPhysicalDevice(Instance, ref physicalDevice, Surface, ref physicalDeviceFeatures, out graphicsFamily, out presentFamily) != VkResult.VK_SUCCESS)
                {
                    MessageBox.Show($"Physical Device set up failed");
                }

                PhysicalDevice = PhysicalDevice;
                PhysicalDeviceFeatures = physicalDeviceFeatures;
                GraphicsFamily = graphicsFamily;
                PresentFamily = presentFamily;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred: {ex.Message}");
            }
        }

        private void SetUpDevice()
        {
            try
            {
                Device = GameEngineDLL.DLL_Renderer_SetUpDevice(PhysicalDevice, GraphicsFamily, PresentFamily);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred: {ex.Message}");
            }
        }

        private void SetUpCommandPool()
        {
            try
            {
                CommandPool = GameEngineDLL.DLL_Renderer_SetUpCommandPool(Device, GraphicsFamily);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred: {ex.Message}");
            }
        }

        private void SetUpSemaphores()
        {
            try
            {
                VkFence[] inFlightFences;
                VkSemaphore[] acquireImageSemaphores;
                VkSemaphore[] presentImageSemaphores;
                if(GameEngineDLL.DLL_Renderer_SetUpSemaphores(Device, out inFlightFences, out acquireImageSemaphores, out presentImageSemaphores, MAX_FRAMES_IN_FLIGHT) != VkResult.VK_SUCCESS)
                {
                    MessageBox.Show($"Semaphores set up failed");
                }
                InFlightFences =  InFlightFences.ToList();
                AcquireImageSemaphores = AcquireImageSemaphores.ToList();
                PresentImageSemaphores = PresentImageSemaphores.ToList(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred: {ex.Message}");
            }
        }

        private void GetDeviceQueue()
        {
            try
            {
                VkQueue graphicsQueue = new VkQueue();
                VkQueue presentQueue = new VkQueue();
                if(GameEngineDLL.DLL_Renderer_GetDeviceQueue(Device, GraphicsFamily, PresentFamily, out graphicsQueue, out presentQueue) != VkResult.VK_SUCCESS)
                {
                    MessageBox.Show($"Semaphores set up failed");
                }
                GraphicsQueue = graphicsQueue;
                PresentQueue = presentQueue;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred: {ex.Message}");
            }
        }

        private void GetSurfaceFormats()
        {
            try
            {
                //UInt32 surfaceImageCount = 0;
                //VkSurfaceFormatKHR[] surfaceFormatKHR;
                //Instance = GameEngineDLL.DLL_Renderer_GetSurfaceFormats(PhysicalDevice, Surface, surfaceFormatKHR, ref surfaceImageCount);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred: {ex.Message}");
            }
        }

        private void GetPresentModes()
        {
            try
            {
                //Instance = GameEngineDLL.DLL_Renderer_GetPresentModes(PhysicalDevice, Surface, );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred: {ex.Message}");
            }
        }

        //private UInt32 SwapChain_FindSwapSurfaceFormat()
        //{
        //    try
        //    {
        //        UInt32 availableFormatsCount = 0;
        //        VkSurfaceFormatKHR[] availableFormats;
        //        VkResult result = GameEngineDLL.DLL_Renderer_GetSurfaceFormats(PhysicalDevice, Surface, availableFormats, ref availableFormatsCount);

        //        return availableFormatsCount;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Exception occurred: {ex.Message}");
        //    }

        //    return UInt32.MaxValue;
        //}

        //private UInt32 SwapChain_FindSwapPresentMode()
        //{
        //    try
        //    {
        //        VkPresentModeKHR[] availablePresentModes = new VkPresentModeKHR[1];
        //        uint availablePresentModesCount = 1;
        //        VkResult result = DLL_SwapChain_GetPresentModes(PhysicalDevice, Surface, availablePresentModes, ref availablePresentModesCount);
        //        PresentMode = DLL_SwapChain_FindSwapPresentMode(availablePresentModes, availablePresentModesCount);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Exception occurred: {ex.Message}");
        //    }

        //    return UInt32.MaxValue;
        //}

        //private void SwapChain_GetQueueFamilies()
        //{
        //    try
        //    {
        //        uint graphicsFamily;
        //        uint presentFamily;
        //        VkResult result = GameEngineDLL.DLL_SwapChain_GetQueueFamilies(PhysicalDevice, Surface, out graphicsFamily, out presentFamily);
        //        GraphicsFamily = graphicsFamily;
        //        PresentFamily = presentFamily;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Exception occurred: {ex.Message}");
        //    }
        //}

        //private void SwapChain_GetSurfaceCapabilities()
        //{
        //    try
        //    {
        //        VkSurfaceCapabilitiesKHR surfaceCapabilities = new VkSurfaceCapabilitiesKHR();
        //        VkResult result = GameEngineDLL.DLL_SwapChain_GetSurfaceCapabilities(PhysicalDevice, Surface, surfaceCapabilities);
        //        // Use the surface capabilities as needed
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Exception occurred: {ex.Message}");
        //    }
        //}

        //private void SwapChain_GetPhysicalDeviceFormats()
        //{
        //    try
        //    {
        //        VkSurfaceFormatKHR[] compatibleSwapChainFormatList = new VkSurfaceFormatKHR[1];
        //        uint surfaceFormatCount = 1;
        //        VkResult result = GameEngineDLL.DLL_SwapChain_GetPhysicalDeviceFormats(PhysicalDevice, Surface, out compatibleSwapChainFormatList, out surfaceFormatCount);
        //        // Use the compatible swap chain format list as needed
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Exception occurred: {ex.Message}");
        //    }
        //}

        //private void SwapChain_GetPhysicalDevicePresentModes()
        //{
        //    try
        //    {
        //        VkPresentModeKHR[] compatiblePresentModesList = new VkPresentModeKHR[1];
        //        uint presentModeCount = 1;
        //        VkResult result = GameEngineDLL.DLL_SwapChain_GetPhysicalDevicePresentModes(PhysicalDevice, Surface, out compatiblePresentModesList, out presentModeCount);
        //        // Use the compatible present modes list as needed
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Exception occurred: {ex.Message}");
        //    }
        //}


        //private void SwapChain_SetUpSwapChain()
        //{
        //    try
        //    {
        //        Instance = GameEngineDLL.DLL_Renderer_CreateVulkanInstance();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Exception occurred: {ex.Message}");
        //    }
        //}

        //private void SwapChain_SetUpSwapChainImages()
        //{
        //    try
        //    {
        //        Instance = GameEngineDLL.DLL_Renderer_CreateVulkanInstance();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Exception occurred: {ex.Message}");
        //    }
        //}

        //private void SwapChain_SetUpSwapChainImageViews()
        //{
        //    try
        //    {
        //        Instance = GameEngineDLL.DLL_Renderer_CreateVulkanInstance();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Exception occurred: {ex.Message}");
        //    }
        //}
    }
}

