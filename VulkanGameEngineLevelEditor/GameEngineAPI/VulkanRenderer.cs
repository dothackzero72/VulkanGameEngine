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

         public static VkResult CreateRenderPass(RenderPassCreateInfoStruct renderPassCreateInfo)
        {
            return GameEngineDLL.DLL_Renderer_CreateRenderPass(Device, Helper.GetObjectPtr(renderPassCreateInfo));
        }

        public static VkResult CreateDescriptorPool(VkDescriptorPool descriptorPool, VkDescriptorPoolCreateInfo descriptorPoolCreateInfo)
        {
            IntPtr ptr = Helper.GetObjectPtr(descriptorPoolCreateInfo);
            return GameEngineDLL.DLL_Renderer_CreateDescriptorPool(Device, out ptr, Helper.GetObjectPtr(descriptorPoolCreateInfo));
        }

        public static VkResult CreateDescriptorSetLayout(VkDescriptorSetLayout descriptorSetLayout, VkDescriptorSetLayoutCreateInfo descriptorSetLayoutCreateInfo)
        {
            IntPtr ptr = Helper.GetObjectPtr(descriptorSetLayout);
            return GameEngineDLL.DLL_Renderer_CreateDescriptorSetLayout(Device, out ptr, Helper.GetObjectPtr(descriptorSetLayoutCreateInfo));
        }

        public static VkResult CreatePipelineLayout(VkPipelineLayout pipelineLayout, VkPipelineLayoutCreateInfo pipelineLayoutCreateInfo)
        {
            IntPtr ptr = Helper.GetObjectPtr(pipelineLayout);
            return GameEngineDLL.DLL_Renderer_CreatePipelineLayout(Device, out ptr, Helper.GetObjectPtr(pipelineLayoutCreateInfo));
        }

        public static VkResult AllocateDescriptorSets(VkDescriptorSet descriptorSet, VkDescriptorSetAllocateInfo descriptorSetAllocateInfo)
        {
            IntPtr ptr = Helper.GetObjectPtr(descriptorSet);
            return GameEngineDLL.DLL_Renderer_AllocateDescriptorSets(Device, out ptr, Helper.GetObjectPtr(descriptorSetAllocateInfo));
        }

        public static VkResult AllocateCommandBuffers(VkCommandBuffer commandBuffer, VkCommandBufferAllocateInfo commandBufferAllocateInfo)
        {
            IntPtr ptr = Helper.GetObjectPtr(commandBuffer);
            return GameEngineDLL.DLL_Renderer_AllocateCommandBuffers(Device, out ptr, Helper.GetObjectPtr(commandBufferAllocateInfo));
        }

        public static VkResult CreateGraphicsPipelines(VkPipeline graphicPipeline, List<VkGraphicsPipelineCreateInfo> createGraphicPipelines)
        {
            IntPtr ptr = Helper.GetObjectPtr(graphicPipeline);
            return GameEngineDLL.DLL_Renderer_CreateGraphicsPipelines(Device, out ptr, Helper.GetObjectPtr(createGraphicPipelines), (uint)createGraphicPipelines.Count());
        }

        public static void UpdateDescriptorSet(List<VkWriteDescriptorSet> writeDescriptorSet)
        {
            GameEngineDLL.DLL_Renderer_UpdateDescriptorSet(Device, writeDescriptorSet, (uint)writeDescriptorSet.Count());
        }

        public static VkResult CreateFrameBuffer(VkFramebuffer frameBuffer, VkFramebufferCreateInfo frameBufferCreateInfo)
        {
            return GameEngineDLL.DLL_Renderer_CreateFrameBuffer(Device, &frameBuffer, &frameBufferCreateInfo);
        }

        public static VkPipelineShaderStageCreateInfo CreateShader(string shaderModule, VkShaderStageFlagBits shaderStages)
        {
            return GameEngineDLL.DLL_Shader_CreateShader(shaderModule, shaderStages);
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
            var surface = new VkSurfaceKHR();
            var surfaceCreateInfo = new VkWin32SurfaceCreateInfoKHR
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_WIN32_SURFACE_CREATE_INFO_KHR,
                hwnd = pictureBox.Handle,
                hinstance = Marshal.GetHINSTANCE(typeof(Form1).Module)
            };

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
                UInt32 graphicsFamily, presentFamily;
                var physicalDeviceFeatures = PhysicalDeviceFeatures;
                if (GameEngineDLL.DLL_Renderer_SetUpPhysicalDevice(Instance, ref physicalDevice, Surface, ref physicalDeviceFeatures, out graphicsFamily, out presentFamily) != VkResult.VK_SUCCESS)
                {
                    MessageBox.Show("Physical Device setup failed.");
                    return;
                }

                PhysicalDevice = physicalDevice;
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
                IntPtr inFlightFencesPtr;
                IntPtr acquireImageSemaphoresPtr;
                IntPtr presentImageSemaphoresPtr;

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
                var graphicsQueue = new VkQueue();
                var presentQueue = new VkQueue();
                if (GameEngineDLL.DLL_Renderer_GetDeviceQueue(Device, GraphicsFamily, PresentFamily, out graphicsQueue, out presentQueue) != VkResult.VK_SUCCESS)
                {
                    MessageBox.Show("Failed to get device queues.");
                }
                GraphicsQueue = graphicsQueue;
                PresentQueue = presentQueue;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred: {ex.Message}");
            }
        }

        private static List<VkSurfaceFormatKHR> GetSurfaceFormats()
        {
            try
            {
                uint surfaceFormatCount = 0;
                VkResult result = GameEngineDLL.DLL_Renderer_GetSurfaceFormats(PhysicalDevice, Surface, IntPtr.Zero, ref surfaceFormatCount);
                if (result != VkResult.VK_SUCCESS || surfaceFormatCount == 0)
                {
                    MessageBox.Show($"Failed to get surface format count or no formats available: {result}");
                    return new List<VkSurfaceFormatKHR>();
                }

                IntPtr surfaceFormatsPtr = Marshal.AllocHGlobal((int)(surfaceFormatCount * Marshal.SizeOf<VkSurfaceFormatKHR>()));
                try
                {
                    result = GameEngineDLL.DLL_Renderer_GetSurfaceFormats(PhysicalDevice, Surface, surfaceFormatsPtr, ref surfaceFormatCount);
                    if (result != VkResult.VK_SUCCESS)
                    {
                        MessageBox.Show($"Failed to get surface formats: {result}");
                        return new List<VkSurfaceFormatKHR>();
                    }

                    VkSurfaceFormatKHR[] surfaceFormats = new VkSurfaceFormatKHR[surfaceFormatCount];
                    for (uint i = 0; i < surfaceFormatCount; i++)
                    {
                        IntPtr formatPtr = IntPtr.Add(surfaceFormatsPtr, (int)(i * Marshal.SizeOf<VkSurfaceFormatKHR>()));
                        surfaceFormats[i] = Marshal.PtrToStructure<VkSurfaceFormatKHR>(formatPtr);
                    }

                    return surfaceFormats.ToList();
                }
                finally
                {
                    Marshal.FreeHGlobal(surfaceFormatsPtr);
                }
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
                VkResult result = GameEngineDLL.DLL_Renderer_GetPresentModes(PhysicalDevice, Surface, null, out presentModeCount);
                if (result != VkResult.VK_SUCCESS || presentModeCount == 0)
                {
                    MessageBox.Show($"Failed to get present mode count or no present modes available: {result}");
                    return new List<VkPresentModeKHR>();
                }

                VkPresentModeKHR[] presentFormat = new VkPresentModeKHR[presentModeCount];
                result = GameEngineDLL.DLL_Renderer_GetPresentModes(PhysicalDevice, Surface, presentFormat, out presentModeCount);
                if (result != VkResult.VK_SUCCESS || presentModeCount == 0)
                {
                    MessageBox.Show($"Failed to get present modes: {result}");
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
                return GameEngineDLL.DLL_SwapChain_FindSwapSurfaceFormat(surfaceFormatList.ToArray(), (uint)surfaceFormatList.Count);
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
                if (result == VkResult.VK_SUCCESS)
                {
                    GraphicsFamily = graphicsFamily;
                    PresentFamily = presentFamily;
                }
                else
                {
                    MessageBox.Show($"Failed to get queue families: {result}");
                }
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
                VkSurfaceCapabilitiesKHR surfaceCapabilities;
                VkResult result = GameEngineDLL.DLL_SwapChain_GetSurfaceCapabilities(PhysicalDevice, Surface, out surfaceCapabilities);
                if (result != VkResult.VK_SUCCESS)
                {
                    MessageBox.Show($"Failed to get surface capabilities: {result}");
                    return new VkSurfaceCapabilitiesKHR();
                }
                return surfaceCapabilities;
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

            try
            {
                // Step 1: Get the count of surface formats
                VkResult result = GameEngineDLL.DLL_SwapChain_GetPhysicalDeviceFormats(PhysicalDevice, Surface, out compatibleFormatsPtr, out surfaceFormatCount);
                if (result != VkResult.VK_SUCCESS)
                {
                    Console.WriteLine("Failed to get the surface format count.");
                    return new List<VkSurfaceFormatKHR>();
                }

                // Step 2: Check for no compatible formats
                if (surfaceFormatCount == 0 || compatibleFormatsPtr == IntPtr.Zero)
                {
                    Console.WriteLine("No compatible swap chain formats.");
                    return new List<VkSurfaceFormatKHR>();
                }

                // Step 3: Allocate an array to hold the compatible formats
                VkSurfaceFormatKHR[] compatibleFormats = new VkSurfaceFormatKHR[surfaceFormatCount];
                for (uint x = 0; x < surfaceFormatCount; x++)
                {
                    IntPtr currentFormatPtr = IntPtr.Add(compatibleFormatsPtr, (int)(x * Marshal.SizeOf<VkSurfaceFormatKHR>()));
                    compatibleFormats[x] = Marshal.PtrToStructure<VkSurfaceFormatKHR>(currentFormatPtr);
                }

                return compatibleFormats.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return new List<VkSurfaceFormatKHR>();
            }
        }

        private static List<VkPresentModeKHR> GetPhysicalDevicePresentModes()
        {
            uint presentModeCount = 0;
            IntPtr presentModesPointer = IntPtr.Zero;

            VkResult result = GameEngineDLL.DLL_SwapChain_GetPhysicalDevicePresentModes(PhysicalDevice, Surface, out presentModesPointer, out presentModeCount);

            if (result != VkResult.VK_SUCCESS || presentModeCount == 0)
            {
                MessageBox.Show($"Failed to get present mode count or no present modes available: {result}");
                return new List<VkPresentModeKHR>();
            }

            // No need to dereference presentModesPointer again
            // Now read the present modes directly
            List<VkPresentModeKHR> presentModes = new List<VkPresentModeKHR>();
            for (uint x = 0; x < presentModeCount; x++)
            {
                IntPtr currentPresentModePtr = IntPtr.Add(presentModesPointer, (int)(x * sizeof(int)));
                VkPresentModeKHR presentMode = (VkPresentModeKHR)Marshal.ReadInt32(currentPresentModePtr);
                presentModes.Add(presentMode);
            }

            // If Vulkan allocated the memory, ensure to free it here if applicable
            // This line depends on how memory is managed by Vulkan
            // Marshal.FreeHGlobal(presentModesPointer); // Uncomment this line if Vulkan does not manage its memory

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
                uint swapChainImageCount;
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
                IntPtr swapChainViewPtr = GameEngineDLL.DLL_SwapChain_SetUpSwapChainImageViews(Device, SwapChainImages.ToArray(), ref surfaceFormat, swapChainImageCount);
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