using GlmSharp;
using SDL2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using static VulkanGameEngineLevelEditor.VulkanAPI;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public static unsafe class VulkanRenderer
    {
        private static string[] _deviceExtensionNames =
         {
        "VK_KHR_surface",
    "VK_KHR_win32_surface",
    "VK_EXT_debug_utils"
    };

        private static string[] _validationLayers =
         {
        "VK_LAYER_KHRONOS_validation"
    };

        private static VkValidationFeatureEnableEXT[] _enabledList =
        {
        VkValidationFeatureEnableEXT.VK_VALIDATION_FEATURE_ENABLE_DEBUG_PRINTF_EXT,
        VkValidationFeatureEnableEXT.VK_VALIDATION_FEATURE_ENABLE_GPU_ASSISTED_RESERVE_BINDING_SLOT_EXT
    };

        private static VkValidationFeatureDisableEXT[] _disabledList =
          {
        VkValidationFeatureDisableEXT.VK_VALIDATION_FEATURE_DISABLE_THREAD_SAFETY_EXT,
        VkValidationFeatureDisableEXT.VK_VALIDATION_FEATURE_DISABLE_API_PARAMETERS_EXT,
        VkValidationFeatureDisableEXT.VK_VALIDATION_FEATURE_DISABLE_OBJECT_LIFETIMES_EXT,
        VkValidationFeatureDisableEXT.VK_VALIDATION_FEATURE_DISABLE_CORE_CHECKS_EXT
    };

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

        public static void SetUpRenderer(IntPtr handle)
        {
            WindowHandleCopy = handle;
            CreateVulkanInstance();
          //  SetupDebugMessenger();
            CreateVulkanSurface();
            SetUpPhysicalDevice();
            SetUpDevice();
            VkSurfaceCapabilitiesKHR surfaceCapabilities = GetSurfaceCapabilities();
            List<VkSurfaceFormatKHR> surfaceFormatList = GetPhysicalDeviceFormats();
            GetQueueFamilies();
            List<VkPresentModeKHR> presentModeList = GetPhysicalDevicePresentModes();
            VkSurfaceFormatKHR swapChainImageFormat = FindSwapSurfaceFormat(surfaceFormatList);
            VkPresentModeKHR presentMode = FindSwapPresentMode(presentModeList);
            GetFrameBufferSize();
            SetUpSwapChain(surfaceCapabilities, swapChainImageFormat, presentMode);
            SetUpSwapChainImages();
            SetUpSwapChainImageViews(swapChainImageFormat);
            SetUpCommandPool();
            SetUpSemaphores();
            GetDeviceQueue();
        }

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

        public const uint VkApiVersion10 = 1;
        public const uint VkApiVersion11 = 2;
        public const uint VkApiVersion12 = 3;
        public const uint VkApiVersion13 = 4;  // Replace with actual numeric value if different

        // To create a version from major, minor, and patch
        public static uint MakeVersion(uint major, uint minor, uint patch)
        {
            return (major << 22) | (minor << 12) | patch;
        }

        // Vulkan versioning macros as constants
        public static uint VK_API_VERSION_1_0 = MakeVersion(1, 0, 0);
        public static uint VK_API_VERSION_1_1 = MakeVersion(1, 1, 0);
        public static uint VK_API_VERSION_1_2 = MakeVersion(1, 2, 0);
        public static uint VK_API_VERSION_1_3 = MakeVersion(1, 3, 0);

        public static VkResult CreateCommandBuffers(List<VkCommandBuffer> commandBufferList)
        {
            fixed (VkCommandBuffer* cmdPtr = commandBufferList.ToArray())
            {
                VkResult result = GameEngineDLL.DLL_Renderer_CreateCommandBuffers(Device, CommandPool, cmdPtr, commandBufferList.UCount());
                commandBufferList.PtrToList(cmdPtr);
                return VkResult.VK_SUCCESS;
            }
        }

        public static VkResult CreateRenderPass(RenderPassCreateInfoStruct renderPassCreateInfo)
        {
            return GameEngineDLL.DLL_Renderer_CreateRenderPass(Device, Helper.GetObjectPtr(renderPassCreateInfo));
        }

        public static VkDescriptorPool CreateDescriptorPool(VkDescriptorPoolCreateInfo descriptorPoolCreateInfo)
        {
            var tempDescriptorPool = new VkDescriptorPool();
            VkResult result = GameEngineDLL.DLL_Renderer_CreateDescriptorPool(Device, &tempDescriptorPool, &descriptorPoolCreateInfo);
            return tempDescriptorPool;
        }

        public static VkDescriptorSetLayout CreateDescriptorSetLayout(VkDescriptorSetLayoutCreateInfo descriptorSetLayoutCreateInfo)
        {
            var descriptorSetLayout = new VkDescriptorSetLayout();
            VkResult result = GameEngineDLL.DLL_Renderer_CreateDescriptorSetLayout(Device, &descriptorSetLayout, &descriptorSetLayoutCreateInfo);
            return descriptorSetLayout;
        }

        public static VkResult CreatePipelineLayout(VkPipelineLayout pipelineLayout, VkPipelineLayoutCreateInfo pipelineLayoutCreateInfo)
        {
            return GameEngineDLL.DLL_Renderer_CreatePipelineLayout(Device, &pipelineLayout, &pipelineLayoutCreateInfo);
        }

        public static VkDescriptorSet AllocateDescriptorSets(VkDescriptorSet descriptorSet, VkDescriptorSetAllocateInfo descriptorSetAllocateInfo)
        {
            IntPtr ptr = Helper.GetObjectPtr(descriptorSet);
            VkResult result = GameEngineDLL.DLL_Renderer_AllocateDescriptorSets(Device, &ptr, &descriptorSetAllocateInfo);
            return descriptorSet;
        }

        public static VkResult AllocateCommandBuffers(VkCommandBuffer commandBuffer, VkCommandBufferAllocateInfo commandBufferAllocateInfo)
        {
            IntPtr ptr = Helper.GetObjectPtr(commandBuffer);
            return GameEngineDLL.DLL_Renderer_AllocateCommandBuffers(Device, out ptr, Helper.GetObjectPtr(commandBufferAllocateInfo));
        }

        public static VkPipeline CreateGraphicsPipelines(VkPipeline graphicPipeline, List<VkGraphicsPipelineCreateInfo> createGraphicPipelines)
        {
            var ptr = &graphicPipeline;
            fixed (VkGraphicsPipelineCreateInfo* cmdPtr = createGraphicPipelines.ToArray())
            {
                VulkanAPI.vkCreateGraphicsPipelines(Device, IntPtr.Zero, createGraphicPipelines.UCount(), cmdPtr, null, &graphicPipeline);
                return graphicPipeline;
            }

        }

        public static void UpdateDescriptorSet(List<VkWriteDescriptorSet> writeDescriptorSet)
        {
            fixed (VkWriteDescriptorSet* ptr = writeDescriptorSet.ToArray())
            {
                GameEngineDLL.DLL_Renderer_UpdateDescriptorSet(Device, ptr, writeDescriptorSet.UCount());
            }
        }

        public static VkResult CreateFrameBuffer(VkFramebuffer* frameBuffer, VkFramebufferCreateInfo frameBufferCreateInfo)
        {
            VkFramebufferCreateInfo createInfo = frameBufferCreateInfo;
            return GameEngineDLL.DLL_Renderer_CreateFrameBuffer(Device, frameBuffer, &createInfo);
        }

        public static VkPipelineShaderStageCreateInfo CreateShader(string path, VkShaderStageFlagBits shaderStage)
        {
            byte[] shaderCode = File.ReadAllBytes(path);
            VkShaderModule shaderModule = CreateShaderModule(shaderCode);

            IntPtr pName = Marshal.StringToHGlobalAnsi("main");
            try
            {
                VkPipelineShaderStageCreateInfo shaderStageInfo = new VkPipelineShaderStageCreateInfo
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_SHADER_STAGE_CREATE_INFO,
                    stage = shaderStage,
                    module = shaderModule,
                    pName = pName,
                    pNext = IntPtr.Zero,
                    flags = 0
                };

                return shaderStageInfo;
            }
            finally
            {
                Marshal.FreeHGlobal(pName);
            }
        }

        public static VkCommandBuffer BeginCommandBuffer()
        {
            return GameEngineDLL.DLL_Renderer_BeginSingleUseCommandBuffer(Device, CommandPool);
        }

        public static VkResult BeginCommandBuffer(VkCommandBuffer* pCommandBufferList, VkCommandBufferBeginInfo* commandBufferBeginInfo)
        {
            return GameEngineDLL.DLL_Renderer_BeginCommandBuffer(pCommandBufferList, commandBufferBeginInfo);
        }

        public static VkResult EndCommandBuffer(VkCommandBuffer* pCommandBufferList)
        {
            return GameEngineDLL.DLL_Renderer_EndCommandBuffer(pCommandBufferList);
        }

        public static VkResult EndCommandBuffer(VkCommandBuffer commandBuffer)
        {
            return GameEngineDLL.DLL_Renderer_EndSingleUseCommandBuffer(Device, CommandPool, GraphicsQueue, commandBuffer);
        }

        private static VkShaderModule CreateShaderModule(byte[] code)
        {
            VkShaderModule shaderModule = new VkShaderModule();
            fixed (byte* codePtr = code)
            {
                var createInfo = new VkShaderModuleCreateInfo
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_SHADER_MODULE_CREATE_INFO,
                    codeSize = (nuint)code.Length,
                    pCode = codePtr
                };

                VkResult result = VulkanAPI.vkCreateShaderModule(Device, &createInfo, null, &shaderModule);
                if (result != VkResult.VK_SUCCESS)
                {
                    Console.WriteLine($"Failed to create shader module: {result}");
                }
            }
            return shaderModule;
        }

        public static void Renderer_GetWin32Extensions(ref uint extensionCount, out IntPtr enabledExtensions)
        {
            VkResult result = vkEnumerateInstanceExtensionProperties(IntPtr.Zero, ref extensionCount, IntPtr.Zero);
            if (result != VkResult.VK_SUCCESS)
            {
                MessageBox.Show($"Failed to enumerate instance extension properties. Error: {result}");
                enabledExtensions = IntPtr.Zero;
            }

            IntPtr extensionsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<VkExtensionProperties>() * (int)extensionCount);
            try
            {
                result = vkEnumerateInstanceExtensionProperties(IntPtr.Zero, ref extensionCount, extensionsPtr);
                if (result != VkResult.VK_SUCCESS)
                {
                    MessageBox.Show($"Failed to retrieve instance extension properties. Error: {result}");
                    enabledExtensions = IntPtr.Zero;
                }

                IntPtr[] extensionNames = new IntPtr[extensionCount];
                for (uint x = 0; x < extensionCount; x++)
                {
                    VkExtensionProperties extProps = Marshal.PtrToStructure<VkExtensionProperties>(
                        IntPtr.Add(extensionsPtr, (int)(x * Marshal.SizeOf<VkExtensionProperties>())));
                    extensionNames[x] = Marshal.StringToHGlobalAnsi(extProps.extensionName);
                }

                IntPtr managedEnabledExtensionsPtr = Marshal.AllocHGlobal((int)(extensionCount * IntPtr.Size));
                Marshal.Copy(extensionNames, 0, managedEnabledExtensionsPtr, (int)extensionCount);
                enabledExtensions = managedEnabledExtensionsPtr;
            }
            finally
            {
                Marshal.FreeHGlobal(extensionsPtr);
            }
        }

        public static VkInstance CreateVulkanInstance()
        {
            VkInstance instance = VulkanConsts.VK_NULL_HANDLE;

            uint extensionCount = 0;
            IntPtr extensions = IntPtr.Zero;
           Renderer_GetWin32Extensions(ref extensionCount, out extensions);

            VkApplicationInfo applicationInfo = new VkApplicationInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_APPLICATION_INFO,
                pApplicationName = Marshal.StringToHGlobalAnsi("Vulkan Application"),
                applicationVersion = MakeVersion(1, 0, 0),
                pEngineName = Marshal.StringToHGlobalAnsi("No Engine"),
                engineVersion = MakeVersion(1, 0, 0),
                apiVersion = VK_API_VERSION_1_3
            };

            VkInstanceCreateInfo vulkanCreateInfo = new VkInstanceCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO,
                pApplicationInfo = &applicationInfo,
                enabledLayerCount = 0,
                ppEnabledLayerNames = IntPtr.Zero,
                enabledExtensionCount = extensionCount,
                ppEnabledExtensionNames = extensions
            };

            VkResult result = vkCreateInstance(ref vulkanCreateInfo, null, out instance);
            if (result != VkResult.VK_SUCCESS)
            {
                Console.Error.WriteLine("Failed to create Vulkan instance");
                FreeResources(applicationInfo, extensions);
                return VulkanConsts.VK_NULL_HANDLE;
            }

            // Clean up
            Marshal.FreeHGlobal(applicationInfo.pApplicationName);
            Marshal.FreeHGlobal(applicationInfo.pEngineName);
            Marshal.FreeHGlobal(extensions);

            return instance;
        }

        private static void FreeResources(VkApplicationInfo appInfo, IntPtr extensions)
        {
            Marshal.FreeHGlobal(appInfo.pApplicationName);
            Marshal.FreeHGlobal(appInfo.pEngineName);
            if (extensions != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(extensions);
            }
        }

        public static VkDebugUtilsMessengerEXT SetupDebugMessenger()
        {
            return VulkanDebugMessenger.SetupDebugMessenger();
        }
    
    public static void CreateVulkanSurface()
        {

            var surface = new VkSurfaceKHR();
            var surfaceCreateInfo = new VkWin32SurfaceCreateInfoKHR
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_WIN32_SURFACE_CREATE_INFO_KHR,
                hwnd = GetWindowHandle(),
                hinstance = Marshal.GetHINSTANCE(typeof(Program).Module)
            };

            if (VulkanAPI.vkCreateWin32SurfaceKHR(Instance, ref surfaceCreateInfo, null, out surface) != 0)
            {
                MessageBox.Show("Failed to create Vulkan surface.");
                return;
            }
            Surface = surface;
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

            if (VulkanAPI.vkCreateWin32SurfaceKHR(Instance, ref surfaceCreateInfo, null, out surface) != 0)
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
                uint deviceCount = 0;

                // First call to obtain the number of physical devices
                VkResult result = vkEnumeratePhysicalDevices(Instance, ref deviceCount, IntPtr.Zero);
                if (result != VkResult.VK_SUCCESS)
                {
                    throw new Exception($"vkEnumeratePhysicalDevices failed with result: {result}");
                }

                IntPtr[] physicalDevices = new IntPtr[deviceCount];

                // Pinning the array to prevent the garbage collector from moving it
                GCHandle handle = GCHandle.Alloc(physicalDevices, GCHandleType.Pinned);

                try
                {
                    // Getting the pointers of the physical devices
                    IntPtr physicalDevicePtr = handle.AddrOfPinnedObject();
                    result = vkEnumeratePhysicalDevices(Instance, ref deviceCount, physicalDevicePtr);
                    if (result != VkResult.VK_SUCCESS)
                    {
                        throw new Exception($"vkEnumeratePhysicalDevices failed with result: {result}");
                    }

                    VkPhysicalDeviceFeatures physicalDeviceFeatures;

                    for (uint x = 0; x < deviceCount; x++)
                    {
                        // Ensure to use 'out' properly
                       vkGetPhysicalDeviceFeatures(physicalDevices[x], &physicalDeviceFeatures);
                        if (result != VkResult.VK_SUCCESS)
                        {
                            throw new Exception($"vkGetPhysicalDeviceFeatures failed with result: {result}");
                        }

                        // Correctly check queue families and formats
                        result = GameEngineDLL.DLL_SwapChain_GetQueueFamilies(physicalDevices[x], Surface, out uint graphicsFamily, out uint presentFamily);
                        if (result != VkResult.VK_SUCCESS)
                        {
                            throw new Exception($"DLL_SwapChain_GetQueueFamilies failed with result: {result}");
                        }

                        VkSurfaceFormatKHR surfaceFormat;
                        uint surfaceFormatCount = 0;

                        result = GameEngineDLL.DLL_Renderer_GetSurfaceFormats(physicalDevices[x], Surface, out surfaceFormat, out surfaceFormatCount);
                        if (result != VkResult.VK_SUCCESS)
                        {
                            throw new Exception($"DLL_Renderer_GetSurfaceFormats failed with result: {result}");
                        }

                        VkPresentModeKHR presentMode;
                        uint presentModeCount = 0;

                        result = GameEngineDLL.DLL_Renderer_GetSurfacePresentModes(physicalDevices[x], Surface, out presentMode, out presentModeCount);
                        if (result != VkResult.VK_SUCCESS)
                        {
                            throw new Exception($"DLL_Renderer_GetSurfacePresentModes failed with result: {result}");
                        }

                        // Additional checks for valid families and formats
                        if (graphicsFamily != VulkanConsts.UINT32_MAX &&
                            presentFamily != VulkanConsts.UINT32_MAX &&
                            surfaceFormatCount > 0 &&
                            presentModeCount > 0)
                        {
                            PhysicalDevice = physicalDevices[x];
                            GraphicsFamily = graphicsFamily;
                            PresentFamily = presentFamily;
                        }
                    }
                }
                finally
                {
                    handle.Free(); // Always free pinned handles
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        public static VkResult StartFrame()
        {
            CommandIndex = (CommandIndex + 1) % MAX_FRAMES_IN_FLIGHT;

            var fence = InFlightFences[(int)CommandIndex];
            var imageSemaphore = AcquireImageSemaphores[(int)CommandIndex];
            var imageIndex = ImageIndex;

            // Wait for the current frame to finish
            vkWaitForFences(Device, 1, &fence, VulkanConsts.VK_TRUE, VulkanConsts.UINT64_MAX);
            // Reset the fence to be reusable
            vkResetFences(Device, 1, &fence);

            // Acquire the next image
            VkResult result = vkAcquireNextImageKHR(Device, Swapchain, VulkanConsts.UINT64_MAX, imageSemaphore, fence, &imageIndex);
            if (result == VkResult.VK_ERROR_OUT_OF_DATE_KHR)
            {
                RebuildRendererFlag = true;
                return result;
            }

            return result;
        }

        public static unsafe VkResult EndFrame(List<VkCommandBuffer> commandBufferSubmitList)
        {
            var imageIdx = ImageIndex; // Use the acquired image index
            var fence = InFlightFences[(int)CommandIndex]; // Use the corresponding fence
            var presentSemaphore = PresentImageSemaphores[(int)CommandIndex];
            var imageSemaphore = AcquireImageSemaphores[(int)CommandIndex]; // Corrected index
            var swapChain = Swapchain;

            // Make sure to wait for the fence before we start submission
            vkWaitForFences(Device, 1, &fence, VulkanConsts.VK_TRUE, VulkanConsts.UINT64_MAX);

            VkPipelineStageFlags[] waitStages = new VkPipelineStageFlags[]
            {
        VkPipelineStageFlags.VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT
            };

            fixed (VkPipelineStageFlags* pWaitStages = waitStages)
            {
                // Allocate command buffers array
                var commandBufferCount = commandBufferSubmitList.Count;
                var commandBuffersPtr = (VkCommandBuffer*)Marshal.AllocHGlobal(commandBufferCount * sizeof(VkCommandBuffer));

                try
                {
                    for (int i = 0; i < commandBufferCount; i++)
                    {
                        commandBuffersPtr[i] = commandBufferSubmitList[i];
                    }

                    VkSubmitInfo submitInfo = new VkSubmitInfo()
                    {
                        sType = VkStructureType.VK_STRUCTURE_TYPE_SUBMIT_INFO,
                        waitSemaphoreCount = 1,
                        pWaitSemaphores = &imageSemaphore,
                        pWaitDstStageMask = pWaitStages,
                        commandBufferCount = (uint)commandBufferSubmitList.Count,
                        pCommandBuffers = commandBuffersPtr,
                        signalSemaphoreCount = 1,
                        pSignalSemaphores = &presentSemaphore
                    };

                    // Make sure to wait for the fence before we start submission
                    vkWaitForFences(Device, 1, &fence, VulkanConsts.VK_TRUE, VulkanConsts.UINT64_MAX);

                    // Reset the fence for reuse
                    vkResetFences(Device, 1, &fence);
                    VkResult submitResult = vkQueueSubmit(GraphicsQueue, 1, &submitInfo, fence);
                    if (submitResult != VkResult.VK_SUCCESS)
                    {
                        // Handle submission error if necessary
                        return submitResult;
                    }

                    // Use the correct image index
                    VkPresentInfoKHR presentInfo = new VkPresentInfoKHR()
                    {
                        sType = VkStructureType.VK_STRUCTURE_TYPE_PRESENT_INFO_KHR,
                        waitSemaphoreCount = 1,
                        pWaitSemaphores = &presentSemaphore,
                        swapchainCount = 1,
                        pSwapchains = &swapChain,
                        pImageIndices = &imageIdx // Use the correct image index
                    };

                    VkResult result = vkQueuePresentKHR(PresentQueue, &presentInfo);
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
            return new List<VkSurfaceFormatKHR>();
            //try
            //{
            //    uint surfaceFormatCount = 0;
            //    VkResult result = GameEngineDLL.DLL_Renderer_GetSurfaceFormats(PhysicalDevice, Surface, IntPtr.Zero, ref surfaceFormatCount);
            //    if (result != VkResult.VK_SUCCESS || surfaceFormatCount == 0)
            //    {
            //        MessageBox.Show($"Failed to get surface format count or no formats available: {result}");
            //        return new List<VkSurfaceFormatKHR>();
            //    }

            //    IntPtr surfaceFormatsPtr = Marshal.AllocHGlobal((int)(surfaceFormatCount * Marshal.SizeOf<VkSurfaceFormatKHR>()));
            //    try
            //    {
            //        result = GameEngineDLL.DLL_Renderer_GetSurfaceFormats(PhysicalDevice, Surface, surfaceFormatsPtr, ref surfaceFormatCount);
            //        if (result != VkResult.VK_SUCCESS)
            //        {
            //            MessageBox.Show($"Failed to get surface formats: {result}");
            //            return new List<VkSurfaceFormatKHR>();
            //        }

            //        VkSurfaceFormatKHR[] surfaceFormats = new VkSurfaceFormatKHR[surfaceFormatCount];
            //        for (uint i = 0; i < surfaceFormatCount; i++)
            //        {
            //            IntPtr formatPtr = IntPtr.Add(surfaceFormatsPtr, (int)(i * Marshal.SizeOf<VkSurfaceFormatKHR>()));
            //            surfaceFormats[i] = Marshal.PtrToStructure<VkSurfaceFormatKHR>(formatPtr);
            //        }

            //        return surfaceFormats.ToList();
            //    }
            //    finally
            //    {
            //        Marshal.FreeHGlobal(surfaceFormatsPtr);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Exception occurred: {ex.Message}");
            //    return new List<VkSurfaceFormatKHR>();
            //}
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

        private static VkSurfaceCapabilitiesKHR GetSurfaceCapabilities(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface)
        {
            try
            {
                VkResult result = vkGetPhysicalDeviceSurfaceCapabilitiesKHR(physicalDevice, surface, out VkSurfaceCapabilitiesKHR surfaceCapabilities);

                if (result == VkResult.VK_ERROR_SURFACE_LOST_KHR)
                {
                    // Handle surface loss
                    MessageBox.Show("Surface is lost, recreating surface and swapchain...");
                    // HandleSurfaceLost();
                    return new VkSurfaceCapabilitiesKHR(); // or handle retry logic
                }

                if (result != VkResult.VK_SUCCESS)
                {
                    MessageBox.Show($"Failed to get surface capabilities: {result}");
                    return new VkSurfaceCapabilitiesKHR(); // or handle error logic
                }

                return surfaceCapabilities;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred: {ex.Message}");
                return new VkSurfaceCapabilitiesKHR(); // or handle further
            }
        }

        private static VkSurfaceCapabilitiesKHR GetSurfaceCapabilities()
        {
            try
            {
                VkResult result = vkGetPhysicalDeviceSurfaceCapabilitiesKHR(PhysicalDevice, Surface, out VkSurfaceCapabilitiesKHR surfaceCapabilities);

                if (result == VkResult.VK_ERROR_SURFACE_LOST_KHR)
                {
                    // Handle surface loss
                    MessageBox.Show("Surface is lost, recreating surface and swapchain...");
                   // HandleSurfaceLost();
                    return new VkSurfaceCapabilitiesKHR(); // or handle retry logic
                }

                if (result != VkResult.VK_SUCCESS)
                {
                    MessageBox.Show($"Failed to get surface capabilities: {result}");
                    return new VkSurfaceCapabilitiesKHR(); // or handle error logic
                }

                return surfaceCapabilities;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception occurred: {ex.Message}");
                return new VkSurfaceCapabilitiesKHR(); // or handle further
            }
        }

        private static List<VkSurfaceFormatKHR> GetPhysicalDeviceFormats()
        {
            uint surfaceFormatCount = 0;
            IntPtr compatibleFormatsPtr = IntPtr.Zero;

            try
            {
                VkResult result = GameEngineDLL.DLL_SwapChain_GetPhysicalDeviceFormats(PhysicalDevice, Surface, out compatibleFormatsPtr, out surfaceFormatCount);
                if (result != VkResult.VK_SUCCESS)
                {
                    Console.WriteLine("Failed to get the surface format count.");
                    return new List<VkSurfaceFormatKHR>();
                }

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

            List<VkPresentModeKHR> presentModes = new List<VkPresentModeKHR>();
            for (uint x = 0; x < presentModeCount; x++)
            {
                IntPtr currentPresentModePtr = IntPtr.Add(presentModesPointer, (int)(x * sizeof(int)));
                VkPresentModeKHR presentMode = (VkPresentModeKHR)Marshal.ReadInt32(currentPresentModePtr);
                presentModes.Add(presentMode);
            }

            return presentModes;
        }

        public static uint GetMemoryType(uint typeFilter, VkMemoryPropertyFlagBits memoryType)
        {
            return GameEngineDLL.DLL_Renderer_GetMemoryType(PhysicalDevice, typeFilter, memoryType);
        }

        private static void GetFrameBufferSize()
        {
          
            SDL.SDL_GetWindowSize(WindowHandleCopy, out int width, out int height);
            SwapChainResolution = new VkExtent2D
            {
                Width = (uint)width,
                Height = (uint)height
            };
        }

        private static void GetFrameBufferSize(PictureBox pictureBox)
        {
            SwapChainResolution = new VkExtent2D
            {
                Width = (uint)pictureBox.Size.Width,
                Height = (uint)pictureBox.Size.Height
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

        private static IntPtr MarshalStringToUtf8(string str)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(str + "\0");
            IntPtr ptr = Marshal.AllocHGlobal(utf8Bytes.Length);
            Marshal.Copy(utf8Bytes, 0, ptr, utf8Bytes.Length);
            return ptr;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        public static IntPtr GetWindowHandle()
        {
            return GetForegroundWindow(); // This will get the active window, not the specific SDL window.
        }
    }
}