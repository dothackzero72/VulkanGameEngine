#include "CoreVulkanRenderer.h"
#include <cstdlib>
#include <iostream>
#include <imgui/imgui.h>
#include <imgui/backends/imgui_impl_glfw.h>
#include "../../../../../../VulkanSDK/1.4.304.0/Include/vulkan/vulkan_win32.h"

RendererState cRenderer = { 0 };

 VkResult Renderer_SetUpSwapChain(void* windowHandle, RendererState& renderState)
{
    int width = 0;
    int height = 0;
    uint32 surfaceFormatCount = 0;
    uint32 presentModeCount = 0;

    VkSurfaceCapabilitiesKHR surfaceCapabilities = SwapChain_GetSurfaceCapabilities(renderState.PhysicalDevice, renderState.Surface);
    Vector<VkSurfaceFormatKHR> compatibleSwapChainFormatList = SwapChain_GetPhysicalDeviceFormats(renderState.PhysicalDevice, renderState.Surface);
    VULKAN_RESULT(SwapChain_GetQueueFamilies(renderState.PhysicalDevice, renderState.Surface, renderState.GraphicsFamily, renderState.PresentFamily));
    Vector<VkPresentModeKHR> compatiblePresentModesList = SwapChain_GetPhysicalDevicePresentModes(renderState.PhysicalDevice, renderState.Surface);
    VkSurfaceFormatKHR swapChainImageFormat = SwapChain_FindSwapSurfaceFormat(compatibleSwapChainFormatList);
    VkPresentModeKHR swapChainPresentMode = SwapChain_FindSwapPresentMode(compatiblePresentModesList);
    vulkanWindow->GetFrameBufferSize(windowHandle, &width, &height);
    renderState.Swapchain = SwapChain_SetUpSwapChain(renderState.Device, renderState.PhysicalDevice, renderState.Surface, renderState.GraphicsFamily, renderState.PresentFamily, width, height, renderState.SwapChainImageCount);
    renderState.SwapChainImages = SwapChain_SetUpSwapChainImages(renderState.Device, renderState.Swapchain, MAX_FRAMES_IN_FLIGHT);
    renderState.SwapChainImageViews = SwapChain_SetUpSwapChainImageViews(renderState.Device, renderState.SwapChainImages, swapChainImageFormat);

    renderState.SwapChainResolution.width = width;
    renderState.SwapChainResolution.height = height;
    return VK_SUCCESS;
}

 RendererState Renderer_RendererSetUp(void* windowHandle)
{
    RendererState renderState;
    renderState.RebuildRendererFlag = false;
    VkDebugUtilsMessengerEXT debugMessenger = VK_NULL_HANDLE;
    renderState.Instance = Renderer_CreateVulkanInstance();
    renderState.DebugMessenger = Renderer_SetupDebugMessenger(renderState.Instance);
    vulkanWindow->CreateSurface(windowHandle, &renderState.Instance, &renderState.Surface);
    renderState.PhysicalDevice = Renderer_SetUpPhysicalDevice(renderState.Instance, renderState.Surface, renderState.GraphicsFamily, renderState.PresentFamily);
    renderState.Device = Renderer_SetUpDevice(renderState.PhysicalDevice, renderState.GraphicsFamily, renderState.PresentFamily);
    VULKAN_RESULT(Renderer_SetUpSwapChain(windowHandle, renderState));
    renderState.CommandPool = Renderer_SetUpCommandPool(renderState.Device, renderState.GraphicsFamily);
    VULKAN_RESULT(Renderer_SetUpSemaphores(renderState.Device, renderState.InFlightFences, renderState.AcquireImageSemaphores, renderState.PresentImageSemaphores));
    VULKAN_RESULT(Renderer_GetDeviceQueue(renderState.Device, renderState.GraphicsFamily, renderState.PresentFamily, renderState.GraphicsQueue, renderState.PresentQueue));

    return renderState;
}

 void Renderer_DestroyRenderer(RendererState& renderer)
 {
     Renderer_DestroySwapChainImageView(renderer.Device, renderer.Surface, &renderer.SwapChainImageViews[0], MAX_FRAMES_IN_FLIGHT);
     Renderer_DestroySwapChain(renderer.Device, &renderer.Swapchain);
     Renderer_DestroyFences(renderer.Device, &renderer.AcquireImageSemaphores[0], &renderer.PresentImageSemaphores[0], &renderer.InFlightFences[0], MAX_FRAMES_IN_FLIGHT);
     Renderer_DestroyCommandPool(renderer.Device, &renderer.CommandPool);
     Renderer_DestroyDevice(renderer.Device);
     Renderer_DestroyDebugger(&renderer.Instance, renderer.DebugMessenger);
     Renderer_DestroySurface(renderer.Instance, &renderer.Surface);
     Renderer_DestroyInstance(&renderer.Instance);
 }

  RendererStateCS Renderer_RendererSetUp_CS(void* windowHandle)
 {
      RendererState renderState;
      renderState.RebuildRendererFlag = false;
      VkDebugUtilsMessengerEXT debugMessenger = VK_NULL_HANDLE;
      renderState.Instance = Renderer_CreateVulkanInstance();
      renderState.DebugMessenger = Renderer_SetupDebugMessenger(renderState.Instance);

      VkWin32SurfaceCreateInfoKHR surfaceCreateInfo = VkWin32SurfaceCreateInfoKHR
      {
          .sType = VK_STRUCTURE_TYPE_WIN32_SURFACE_CREATE_INFO_KHR,
          .pNext = nullptr,
          .hinstance = GetModuleHandle(nullptr),
          .hwnd = (HWND)windowHandle,
      };
      vkCreateWin32SurfaceKHR(renderState.Instance, &surfaceCreateInfo, nullptr, &renderState.Surface);

      renderState.PhysicalDevice = Renderer_SetUpPhysicalDevice(renderState.Instance, renderState.Surface, renderState.GraphicsFamily, renderState.PresentFamily);
      renderState.Device = Renderer_SetUpDevice(renderState.PhysicalDevice, renderState.GraphicsFamily, renderState.PresentFamily);
      VULKAN_RESULT(Renderer_SetUpSwapChainCS(renderState));
      renderState.CommandPool = Renderer_SetUpCommandPool(renderState.Device, renderState.GraphicsFamily);
      VULKAN_RESULT(Renderer_SetUpSemaphores(renderState.Device, renderState.InFlightFences, renderState.AcquireImageSemaphores, renderState.PresentImageSemaphores));
      VULKAN_RESULT(Renderer_GetDeviceQueue(renderState.Device, renderState.GraphicsFamily, renderState.PresentFamily, renderState.GraphicsQueue, renderState.PresentQueue));

      RendererStateCS renderStateCS = Renderer_RendererStateCPPtoCS(renderState);;

          // Print RendererStateCS values
          std::cout << "RendererStateCS (C++):\n";
      std::cout << std::hex << std::showbase; // Show values in hex for clarity
      std::cout << "  Instance: " << renderStateCS.Instance << "\n";
      std::cout << "  Device: " << renderStateCS.Device << "\n";
      std::cout << "  PhysicalDevice: " << renderStateCS.PhysicalDevice << "\n";
      std::cout << "  Surface: " << renderStateCS.Surface << "\n";
      std::cout << "  CommandPool: " << renderStateCS.CommandPool << "\n";
      std::cout << "  DebugMessenger: " << renderStateCS.DebugMessenger << "\n";
      std::cout << "  Swapchain: " << renderStateCS.Swapchain << "\n";
      std::cout << "  GraphicsQueue: " << renderStateCS.GraphicsQueue << "\n";
      std::cout << "  PresentQueue: " << renderStateCS.PresentQueue << "\n";
      std::cout << std::dec; // Switch back to decimal for counts and scalars
      std::cout << "  SwapChainImageCount: " << renderStateCS.SwapChainImageCount << "\n";
      std::cout << "  GraphicsFamily: " << renderStateCS.GraphicsFamily << "\n";
      std::cout << "  PresentFamily: " << renderStateCS.PresentFamily << "\n";
      std::cout << "  ImageIndex: " << renderStateCS.ImageIndex << "\n";
      std::cout << "  CommandIndex: " << renderStateCS.CommandIndex << "\n";
      std::cout << "  RebuildRendererFlag: " << (renderStateCS.RebuildRendererFlag ? "true" : "false") << "\n";
      std::cout << "  SwapChainResolution: (" << renderStateCS.SwapChainResolution.width << ", " << renderStateCS.SwapChainResolution.height << ")\n";

      // Print pointer arrays (first element or "null" if empty)
      std::cout << std::hex;
      std::cout << "  InFlightFences (count=" << std::dec << renderStateCS.InFlightFencesCount << std::hex << "): ";
      if (renderStateCS.InFlightFences && renderStateCS.InFlightFencesCount > 0) {
          std::cout << renderStateCS.InFlightFences[0];
      }
      else {
          std::cout << "null";
      }
      std::cout << "\n";

      std::cout << "  AcquireImageSemaphores (count=" << std::dec << renderStateCS.AcquireImageSemaphoresCount << std::hex << "): ";
      if (renderStateCS.AcquireImageSemaphores && renderStateCS.AcquireImageSemaphoresCount > 0) {
          std::cout << renderStateCS.AcquireImageSemaphores[0];
      }
      else {
          std::cout << "null";
      }
      std::cout << "\n";

      std::cout << "  PresentImageSemaphores (count=" << std::dec << renderStateCS.PresentImageSemaphoresCount << std::hex << "): ";
      if (renderStateCS.PresentImageSemaphores && renderStateCS.PresentImageSemaphoresCount > 0) {
          std::cout << renderStateCS.PresentImageSemaphores[0];
      }
      else {
          std::cout << "null";
      }
      std::cout << "\n";

      std::cout << "  SwapChainImages (count=" << std::dec << renderStateCS.SwapChainImagesCount << std::hex << "): ";
      if (renderStateCS.SwapChainImages && renderStateCS.SwapChainImagesCount > 0) {
          std::cout << renderStateCS.SwapChainImages[0];
      }
      else {
          std::cout << "null";
      }
      std::cout << "\n";

      std::cout << "  SwapChainImageViews (count=" << std::dec << renderStateCS.SwapChainImageViewsCount << std::hex << "): ";
      if (renderStateCS.SwapChainImageViews && renderStateCS.SwapChainImageViewsCount > 0) {
          std::cout << renderStateCS.SwapChainImageViews[0];
      }
      else {
          std::cout << "null";
      }
      std::cout << "\n";
     return renderStateCS;
 }

  RendererState Renderer_RendererStateCStoCPP(const RendererStateCS& renderStateCS)
  {
      return RendererState
      {
          .SwapChainImageCount = renderStateCS.SwapChainImageCount,
          .GraphicsFamily = renderStateCS.GraphicsFamily,
          .PresentFamily = renderStateCS.PresentFamily,
          .GraphicsQueue = renderStateCS.GraphicsQueue,
          .PresentQueue = renderStateCS.PresentQueue,
          .Instance = renderStateCS.Instance,
          .Device = renderStateCS.Device,
          .PhysicalDevice = renderStateCS.PhysicalDevice,
          .Surface = renderStateCS.Surface,
          .CommandPool = renderStateCS.CommandPool,
          .ImageIndex = renderStateCS.ImageIndex,
          .CommandIndex = renderStateCS.CommandIndex,
          .DebugMessenger = renderStateCS.DebugMessenger,
          .InFlightFences = Vector<VkFence>(renderStateCS.InFlightFences, renderStateCS.InFlightFences + renderStateCS.InFlightFencesCount),
          .AcquireImageSemaphores = Vector<VkSemaphore>(renderStateCS.AcquireImageSemaphores, renderStateCS.AcquireImageSemaphores + renderStateCS.AcquireImageSemaphoresCount),
          .PresentImageSemaphores = Vector<VkSemaphore>(renderStateCS.PresentImageSemaphores, renderStateCS.PresentImageSemaphores + renderStateCS.PresentImageSemaphoresCount),
          .SwapChainImages = Vector<VkImage>(renderStateCS.SwapChainImages, renderStateCS.SwapChainImages + renderStateCS.SwapChainImagesCount),
          .SwapChainImageViews = Vector<VkImageView>(renderStateCS.SwapChainImageViews, renderStateCS.SwapChainImageViews + renderStateCS.SwapChainImageViewsCount),
          .SwapChainResolution = renderStateCS.SwapChainResolution,
          .Swapchain = renderStateCS.Swapchain,
          .RebuildRendererFlag = renderStateCS.RebuildRendererFlag,
      };
  }

  RendererStateCS Renderer_RendererStateCPPtoCS( RendererState& renderState)
  {
      RendererStateCS renderStateCS = 
      {
         .Instance = renderState.Instance,
         .Device = renderState.Device,
         .PhysicalDevice = renderState.PhysicalDevice,
         .Surface = renderState.Surface,
         .CommandPool = renderState.CommandPool,
         .DebugMessenger = renderState.DebugMessenger,
         .InFlightFences = new VkFence[renderState.InFlightFences.size()],
         .AcquireImageSemaphores = new VkSemaphore[renderState.AcquireImageSemaphores.size()],
         .PresentImageSemaphores = new VkSemaphore[renderState.PresentImageSemaphores.size()],
         .SwapChainImages = new VkImage[renderState.SwapChainImages.size()],
         .SwapChainImageViews = new VkImageView[renderState.SwapChainImageViews.size()],
         .SwapChainResolution = renderState.SwapChainResolution,
         .Swapchain = renderState.Swapchain,
         .SwapChainImageCount = renderState.SwapChainImageCount,
         .InFlightFencesCount = static_cast<uint32>(renderState.InFlightFences.size()),
         .AcquireImageSemaphoresCount = static_cast<uint32>(renderState.AcquireImageSemaphores.size()),
         .PresentImageSemaphoresCount = static_cast<uint32>(renderState.PresentImageSemaphores.size()),
         .SwapChainImagesCount = static_cast<uint32>(renderState.SwapChainImages.size()),
         .SwapChainImageViewsCount = static_cast<uint32>(renderState.SwapChainImageViews.size()),
         .ImageIndex = renderState.ImageIndex,
         .CommandIndex = renderState.CommandIndex,
         .GraphicsFamily = renderState.GraphicsFamily,
         .PresentFamily = renderState.PresentFamily,
         .GraphicsQueue = renderState.GraphicsQueue,
         .PresentQueue = renderState.PresentQueue,
         .RebuildRendererFlag = renderState.RebuildRendererFlag,
      };

      std::memcpy(renderStateCS.InFlightFences, renderState.InFlightFences.data(), renderState.InFlightFences.size() * sizeof(VkFence));
      std::memcpy(renderStateCS.AcquireImageSemaphores, renderState.AcquireImageSemaphores.data(), renderState.AcquireImageSemaphores.size() * sizeof(VkSemaphore));
      std::memcpy(renderStateCS.PresentImageSemaphores, renderState.PresentImageSemaphores.data(), renderState.PresentImageSemaphores.size() * sizeof(VkSemaphore));
      std::memcpy(renderStateCS.SwapChainImages, renderState.SwapChainImages.data(), renderState.SwapChainImages.size() * sizeof(VkImage));
      std::memcpy(renderStateCS.SwapChainImageViews, renderState.SwapChainImageViews.data(), renderState.SwapChainImageViews.size() * sizeof(VkImageView));
      return renderStateCS;
  }

  VkResult Renderer_SetUpSwapChainCS(RendererState& renderState)
 {
      int width = 0;
      int height = 0;
      uint32 surfaceFormatCount = 0;
      uint32 presentModeCount = 0;

      VkSurfaceCapabilitiesKHR surfaceCapabilities = SwapChain_GetSurfaceCapabilities(renderState.PhysicalDevice, renderState.Surface);
      Vector<VkSurfaceFormatKHR> compatibleSwapChainFormatList = SwapChain_GetPhysicalDeviceFormats(renderState.PhysicalDevice, renderState.Surface);
      VULKAN_RESULT(SwapChain_GetQueueFamilies(renderState.PhysicalDevice, renderState.Surface, renderState.GraphicsFamily, renderState.PresentFamily));
      Vector<VkPresentModeKHR> compatiblePresentModesList = SwapChain_GetPhysicalDevicePresentModes(renderState.PhysicalDevice, renderState.Surface);
      VkSurfaceFormatKHR swapChainImageFormat = SwapChain_FindSwapSurfaceFormat(compatibleSwapChainFormatList);
      VkPresentModeKHR swapChainPresentMode = SwapChain_FindSwapPresentMode(compatiblePresentModesList);
      renderState.Swapchain = SwapChain_SetUpSwapChain(renderState.Device, renderState.PhysicalDevice, renderState.Surface, renderState.GraphicsFamily, renderState.PresentFamily, surfaceCapabilities.currentExtent.width, surfaceCapabilities.currentExtent.height, renderState.SwapChainImageCount);
      renderState.SwapChainImages = SwapChain_SetUpSwapChainImages(renderState.Device, renderState.Swapchain, MAX_FRAMES_IN_FLIGHT);
      renderState.SwapChainImageViews = SwapChain_SetUpSwapChainImageViews(renderState.Device, renderState.SwapChainImages, swapChainImageFormat);

      renderState.SwapChainResolution.width = surfaceCapabilities.currentExtent.width;
      renderState.SwapChainResolution.height = surfaceCapabilities.currentExtent.height;

      return VK_SUCCESS;
 }

Vector<VkExtensionProperties> Renderer_GetDeviceExtensions(VkPhysicalDevice physicalDevice)
{
    Vector<VkExtensionProperties> deviceExtensionList = Vector<VkExtensionProperties>();

    uint32 deviceExtentionCount = 0;
    VkResult result = vkEnumerateDeviceExtensionProperties(physicalDevice, nullptr, &deviceExtentionCount, nullptr);
    if (result != VK_SUCCESS)
    {
        fprintf(stderr, "Failed to enumerate device extension properties. Error: %d\n", result);
        return deviceExtensionList;
    }

    deviceExtensionList = Vector<VkExtensionProperties>(deviceExtentionCount);
    result = vkEnumerateDeviceExtensionProperties(physicalDevice, nullptr, &deviceExtentionCount, deviceExtensionList.data());
    if (result != VK_SUCCESS)
    {
        fprintf(stderr, "Failed to enumerate device extension properties. Error: %d\n", result);
        return deviceExtensionList;
    }

    return deviceExtensionList;
}

Vector<VkSurfaceFormatKHR> Renderer_GetSurfaceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface)
{
    Vector<VkSurfaceFormatKHR> surfaceFormatList = Vector<VkSurfaceFormatKHR>();

    uint32 surfaceFormatCount = 0;
    VkResult result = vkGetPhysicalDeviceSurfaceFormatsKHR(physicalDevice, surface, &surfaceFormatCount, nullptr);
    if (result != VK_SUCCESS)
    {
        fprintf(stderr, "Failed to get the physical device surface formats count. Error: %d\n", result);
        return surfaceFormatList;
    }

    surfaceFormatList = Vector<VkSurfaceFormatKHR>(surfaceFormatCount);
    result = vkGetPhysicalDeviceSurfaceFormatsKHR(physicalDevice, surface, &surfaceFormatCount, surfaceFormatList.data());
    if (result != VK_SUCCESS)
    {
        fprintf(stderr, "Failed to get physical device surface formats. Error: %d\n", result);
        return surfaceFormatList;
    }

    return surfaceFormatList;
}

Vector<VkPresentModeKHR> Renderer_GetSurfacePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface)
{
    Vector<VkPresentModeKHR> presentModeList = Vector<VkPresentModeKHR>();

    uint32_t presentModeCount = 0;
    VkResult result = vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice, surface, &presentModeCount, NULL);
    if (result != VK_SUCCESS)
    {
        fprintf(stderr, "Failed to get the physical device surface present modes count. Error: %d\n", result);
        return presentModeList;
    }

    presentModeList = Vector<VkPresentModeKHR>(presentModeCount);
    result = vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice, surface, &presentModeCount, presentModeList.data());
    if (result != VK_SUCCESS)
    {
        fprintf(stderr, "Failed to get physical device surface present modes. Error: %d\n", result);
        return presentModeList;
    }

    return presentModeList;
}

bool Renderer_GetRayTracingSupport()
{
    /* uint32 deviceExtensionCount = INT32_MAX;
     VkExtensionProperties* deviceExtensions = GetDeviceExtensions(cRenderer.PhysicalDevice, &deviceExtensionCount);
     VkPhysicalDeviceAccelerationStructureFeaturesKHR physicalDeviceAccelerationStructureFeatures =
     {
         .sType = VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_ACCELERATION_STRUCTURE_FEATURES_KHR
     };

     VkPhysicalDeviceRayTracingPipelineFeaturesKHR physicalDeviceRayTracingPipelineFeatures =
     {
         .sType = VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_RAY_TRACING_PIPELINE_FEATURES_KHR,
         .pNext = &physicalDeviceAccelerationStructureFeatures
     };

     VkPhysicalDeviceFeatures2 physicalDeviceFeatures2 =
     {
         .sType = VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_FEATURES_2,
         .pNext = &physicalDeviceRayTracingPipelineFeatures
     };
     vkGetPhysicalDeviceFeatures2(cRenderer.PhysicalDevice, &physicalDeviceFeatures2);

     if (physicalDeviceRayTracingPipelineFeatures.rayTracingPipeline == VK_TRUE &&
         physicalDeviceAccelerationStructureFeatures.accelerationStructure == VK_TRUE)
     {
         if (Array_RendererExtensionPropertiesSearch(deviceExtensions, deviceExtensionCount, VK_KHR_ACCELERATION_STRUCTURE_EXTENSION_NAME) &&
             Array_RendererExtensionPropertiesSearch(deviceExtensions, deviceExtensionCount, VK_KHR_RAY_TRACING_PIPELINE_EXTENSION_NAME))
         {
             uint32 pExtensionCount = 0;
             VkExtensionProperties* extensions = NULL;
             vulkanWindow->GetInstanceExtensions(vulkanWindow, &pExtensionCount, &extensions);
             return true;
         }
         else
         {
             fprintf(stderr, "GPU/Mother Board isn't ray tracing compatible.\n");
             free(deviceExtensions);
             return false;
         }
     }
     else
     {
         fprintf(stderr, "GPU/Mother Board isn't ray tracing compatible.\n");
         free(deviceExtensions);
         return false;
     }*/
    return false;
}

VkInstance Renderer_CreateVulkanInstance()
{
    VkInstance instance = VK_NULL_HANDLE;
    VkDebugUtilsMessengerEXT* debugMessenger = VK_NULL_HANDLE;
    VkDebugUtilsMessengerCreateInfoEXT debugInfo =
    {
        .sType = VK_STRUCTURE_TYPE_DEBUG_UTILS_MESSENGER_CREATE_INFO_EXT,
        .messageSeverity = VK_DEBUG_UTILS_MESSAGE_SEVERITY_WARNING_BIT_EXT |
                           VK_DEBUG_UTILS_MESSAGE_SEVERITY_ERROR_BIT_EXT,
        .messageType = VK_DEBUG_UTILS_MESSAGE_TYPE_GENERAL_BIT_EXT |
                       VK_DEBUG_UTILS_MESSAGE_TYPE_VALIDATION_BIT_EXT |
                       VK_DEBUG_UTILS_MESSAGE_TYPE_PERFORMANCE_BIT_EXT,
        .pfnUserCallback = Vulkan_DebugCallBack
    };

    VkValidationFeaturesEXT ValidationFeatures
    {
        .sType = VK_STRUCTURE_TYPE_VALIDATION_FEATURES_EXT,
        .pNext = (VkDebugUtilsMessengerCreateInfoEXT*)&debugInfo,
        .enabledValidationFeatureCount = static_cast<uint32_t>(disabledList.size()),
        .pEnabledValidationFeatures = enabledList.data(),
        .disabledValidationFeatureCount = static_cast<uint32_t>(enabledList.size()),
        .pDisabledValidationFeatures = disabledList.data(),
    };

    int enableValidationLayers = 1;
#ifdef NDEBUG
    enableValidationLayers = 0;
#endif

    uint32_t extensionCount = 0;
    char** enabledExtensions = nullptr;
    Renderer_GetWin32Extensions(&extensionCount, &enabledExtensions);

    VkApplicationInfo applicationInfo = {
        .sType = VK_STRUCTURE_TYPE_APPLICATION_INFO,
        .pApplicationName = "Vulkan Application",
        .applicationVersion = VK_MAKE_VERSION(1, 0, 0),
        .pEngineName = "No Engine",
        .engineVersion = VK_MAKE_VERSION(1, 0, 0),
        .apiVersion = VK_API_VERSION_1_4
    };

    VkInstanceCreateInfo vulkanCreateInfo =
    {
        .sType = VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO,
        .pNext = nullptr,
        .pApplicationInfo = &applicationInfo,
        .enabledLayerCount = static_cast<uint32>(enableValidationLayers),
        .ppEnabledLayerNames = (enableValidationLayers ? ValidationLayers : nullptr),
        .enabledExtensionCount = extensionCount,
        .ppEnabledExtensionNames = enabledExtensions
    };
    if (enableValidationLayers)
    {
        vulkanCreateInfo.pNext = &ValidationFeatures;
    }

    VkResult result = vkCreateInstance(&vulkanCreateInfo, nullptr, &instance);
    if (result != VK_SUCCESS)
    {
        fprintf(stderr, "Failed to create Vulkan instance\n");
        return VK_NULL_HANDLE;
    }

    return instance;
}

VkDebugUtilsMessengerEXT Renderer_SetupDebugMessenger(VkInstance instance)
{
    VkDebugUtilsMessengerEXT debugMessenger = VK_NULL_HANDLE;

    VkDebugUtilsMessengerCreateInfoEXT debugInfo =
    {
        .sType = VK_STRUCTURE_TYPE_DEBUG_UTILS_MESSENGER_CREATE_INFO_EXT,
        .messageSeverity = VK_DEBUG_UTILS_MESSAGE_SEVERITY_WARNING_BIT_EXT | VK_DEBUG_UTILS_MESSAGE_SEVERITY_ERROR_BIT_EXT,
        .messageType = VK_DEBUG_UTILS_MESSAGE_TYPE_GENERAL_BIT_EXT | VK_DEBUG_UTILS_MESSAGE_TYPE_VALIDATION_BIT_EXT | VK_DEBUG_UTILS_MESSAGE_TYPE_PERFORMANCE_BIT_EXT,
        .pfnUserCallback = Vulkan_DebugCallBack
    };

    if (CreateDebugUtilsMessengerEXT(instance, &debugInfo, NULL, &debugMessenger) != VK_SUCCESS)
    {
        fprintf(stderr, "Failed to set up debug messenger!\n");
        return VK_NULL_HANDLE;
    }

    return debugMessenger;
}

VkPhysicalDeviceFeatures Renderer_GetPhysicalDeviceFeatures(VkPhysicalDevice physicalDevice)
{
    VkPhysicalDeviceFeatures features;
    vkGetPhysicalDeviceFeatures(physicalDevice, &features);
    return features;
}

Vector<VkPhysicalDevice> Renderer_GetPhysicalDeviceList(VkInstance& instance)
{
    Vector<VkPhysicalDevice> physicalDeviceList = Vector<VkPhysicalDevice>();

    uint32 deviceCount = 0;
    VkResult result = vkEnumeratePhysicalDevices(instance, &deviceCount, nullptr);
    if (result != VK_SUCCESS)
    {
        fprintf(stderr, "Failed to enumerate physical devices. Error: %d\n", result);
        return physicalDeviceList;
    }
    if (deviceCount == 0) {
        fprintf(stderr, "No physical devices found.\n");
        return physicalDeviceList;
    }

    physicalDeviceList = Vector<VkPhysicalDevice>(deviceCount);
    result = vkEnumeratePhysicalDevices(instance, &deviceCount, physicalDeviceList.data());
    if (result != VK_SUCCESS)
    {
        fprintf(stderr, "Failed to enumerate physical devices after allocation. Error: %d\n", result);
        return physicalDeviceList;
    }

    return physicalDeviceList;
}

VkPhysicalDevice Renderer_SetUpPhysicalDevice(VkInstance instance, VkSurfaceKHR surface, uint32& graphicsFamily, uint32& presentFamily)
{
    Vector<VkPhysicalDevice> physicalDeviceList = Renderer_GetPhysicalDeviceList(instance);
    for (auto& physicalDevice : physicalDeviceList)
    {
        VkPhysicalDeviceFeatures physicalDeviceFeatures = Renderer_GetPhysicalDeviceFeatures(physicalDevice);
        VkResult result = SwapChain_GetQueueFamilies(physicalDevice, surface, graphicsFamily, presentFamily);
        Vector<VkSurfaceFormatKHR> surfaceFormatList = Renderer_GetSurfaceFormats(physicalDevice, surface);
        Vector<VkPresentModeKHR> presentModeList = Renderer_GetSurfacePresentModes(physicalDevice, surface);

        if (graphicsFamily != UINT32_MAX &&
            presentFamily != UINT32_MAX &&
            surfaceFormatList.size() > 0 &&
            presentModeList.size() > 0 &&
            physicalDeviceFeatures.samplerAnisotropy)
        {
            return physicalDevice;
        }
    }

    return VK_NULL_HANDLE;
}

VkDevice Renderer_SetUpDevice(VkPhysicalDevice physicalDevice, uint32 graphicsFamily, uint32 presentFamily)
{
    VkDevice device = VK_NULL_HANDLE;

    float queuePriority = 1.0f;
    Vector<VkDeviceQueueCreateInfo> queueCreateInfoList;
    if (graphicsFamily != UINT32_MAX)
    {
        queueCreateInfoList.emplace_back(VkDeviceQueueCreateInfo
            {
                .sType = VK_STRUCTURE_TYPE_DEVICE_QUEUE_CREATE_INFO,
                .pNext = nullptr,
                .flags = 0,
                .queueFamilyIndex = graphicsFamily,
                .queueCount = 1,
                .pQueuePriorities = &queuePriority
            });
    }

    if (presentFamily != UINT32_MAX &&
        presentFamily != graphicsFamily)
    {
        queueCreateInfoList.emplace_back(VkDeviceQueueCreateInfo
            {
               .sType = VK_STRUCTURE_TYPE_DEVICE_QUEUE_CREATE_INFO,
               .pNext = nullptr,
               .flags = 0,
               .queueFamilyIndex = presentFamily,
               .queueCount = 1,
               .pQueuePriorities = &queuePriority
            });
    }

    VkPhysicalDeviceBufferDeviceAddressFeatures bufferDeviceAddressFeatures =
    {
        .sType = VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_BUFFER_DEVICE_ADDRESS_FEATURES,
        .pNext = nullptr,
        .bufferDeviceAddress = VK_TRUE,
        .bufferDeviceAddressCaptureReplay = VK_FALSE,
        .bufferDeviceAddressMultiDevice = VK_FALSE,
    };

    if (Renderer_GetRayTracingSupport())
    {
        VkPhysicalDeviceAccelerationStructureFeaturesKHR accelerationStructureFeatures =
        {
            .sType = VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_ACCELERATION_STRUCTURE_FEATURES_KHR,
            .accelerationStructure = VK_TRUE,
            .accelerationStructureCaptureReplay = VK_FALSE,
            .accelerationStructureIndirectBuild = VK_FALSE,
            .accelerationStructureHostCommands = VK_FALSE,
            .descriptorBindingAccelerationStructureUpdateAfterBind = VK_FALSE,
        };

        VkPhysicalDeviceRayTracingPipelineFeaturesKHR rayTracingPipelineFeatures =
        {
            .sType = VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_RAY_TRACING_PIPELINE_FEATURES_KHR,
            .pNext = &accelerationStructureFeatures,
            .rayTracingPipeline = VK_TRUE,
            .rayTracingPipelineShaderGroupHandleCaptureReplay = VK_FALSE,
            .rayTracingPipelineShaderGroupHandleCaptureReplayMixed = VK_FALSE,
            .rayTracingPipelineTraceRaysIndirect = VK_FALSE,
            .rayTraversalPrimitiveCulling = VK_FALSE,
        };

        bufferDeviceAddressFeatures.pNext = &rayTracingPipelineFeatures;
    }
    else
    {
        bufferDeviceAddressFeatures.pNext = NULL;
    }

    VkPhysicalDeviceFeatures deviceFeatures =
    {
        .robustBufferAccess = VK_FALSE,
        .fullDrawIndexUint32 = VK_FALSE,
        .imageCubeArray = VK_FALSE,
        .independentBlend = VK_FALSE,
        .geometryShader = VK_FALSE,
        .tessellationShader = VK_FALSE,
        .sampleRateShading = VK_TRUE,
        .dualSrcBlend = VK_FALSE,
        .logicOp = VK_FALSE,
        .multiDrawIndirect = VK_FALSE,
        .drawIndirectFirstInstance = VK_FALSE,
        .depthClamp = VK_FALSE,
        .depthBiasClamp = VK_FALSE,
        .fillModeNonSolid = VK_TRUE,
        .depthBounds = VK_FALSE,
        .wideLines = VK_FALSE,
        .largePoints = VK_FALSE,
        .alphaToOne = VK_FALSE,
        .multiViewport = VK_FALSE,
        .samplerAnisotropy = VK_TRUE,
        .textureCompressionETC2 = VK_FALSE,
        .textureCompressionASTC_LDR = VK_FALSE,
        .textureCompressionBC = VK_FALSE,
        .occlusionQueryPrecise = VK_FALSE,
        .pipelineStatisticsQuery = VK_FALSE,
        .vertexPipelineStoresAndAtomics = VK_TRUE,
        .fragmentStoresAndAtomics = VK_TRUE,
        .shaderTessellationAndGeometryPointSize = VK_FALSE,
        .shaderImageGatherExtended = VK_FALSE,
        .shaderStorageImageExtendedFormats = VK_FALSE,
        .shaderStorageImageMultisample = VK_FALSE,
        .shaderStorageImageReadWithoutFormat = VK_FALSE,
        .shaderStorageImageWriteWithoutFormat = VK_FALSE,
        .shaderUniformBufferArrayDynamicIndexing = VK_FALSE,
        .shaderSampledImageArrayDynamicIndexing = VK_FALSE,
        .shaderStorageBufferArrayDynamicIndexing = VK_FALSE,
        .shaderStorageImageArrayDynamicIndexing = VK_FALSE,
        .shaderClipDistance = VK_FALSE,
        .shaderCullDistance = VK_FALSE,
        .shaderFloat64 = VK_FALSE,
        .shaderInt64 = VK_TRUE,
        .shaderInt16 = VK_FALSE,
        .shaderResourceResidency = VK_FALSE,
        .shaderResourceMinLod = VK_FALSE,
        .sparseBinding = VK_FALSE,
        .sparseResidencyBuffer = VK_FALSE,
        .sparseResidencyImage2D = VK_FALSE,
        .sparseResidencyImage3D = VK_FALSE,
        .sparseResidency2Samples = VK_FALSE,
        .sparseResidency4Samples = VK_FALSE,
        .sparseResidency8Samples = VK_FALSE,
        .sparseResidency16Samples = VK_FALSE,
        .sparseResidencyAliased = VK_FALSE,
        .variableMultisampleRate = VK_FALSE,
        .inheritedQueries = VK_FALSE,
    };

    VkPhysicalDeviceFeatures2 physicalDeviceFeatures2 =
    {
        .sType = VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_FEATURES_2,
        .pNext = nullptr,
        .features = deviceFeatures
    };

    VkPhysicalDeviceVulkan12Features physicalDeviceVulkan12Features =
    {
        .sType = VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_VULKAN_1_2_FEATURES,
        .pNext = &physicalDeviceFeatures2,
        .samplerMirrorClampToEdge = VK_FALSE,
        .drawIndirectCount = VK_FALSE,
        .storageBuffer8BitAccess = VK_TRUE,
        .uniformAndStorageBuffer8BitAccess = VK_TRUE,
        .storagePushConstant8 = VK_FALSE,
        .shaderBufferInt64Atomics = VK_FALSE,
        .shaderSharedInt64Atomics = VK_FALSE,
        .shaderFloat16 = VK_FALSE,
        .shaderInt8 = VK_FALSE,
        .descriptorIndexing = VK_TRUE,
        .shaderInputAttachmentArrayDynamicIndexing = VK_FALSE,
        .shaderUniformTexelBufferArrayDynamicIndexing = VK_FALSE,
        .shaderStorageTexelBufferArrayDynamicIndexing = VK_FALSE,
        .shaderUniformBufferArrayNonUniformIndexing = VK_FALSE,
        .shaderSampledImageArrayNonUniformIndexing = VK_TRUE,
        .shaderStorageBufferArrayNonUniformIndexing = VK_FALSE,
        .shaderStorageImageArrayNonUniformIndexing = VK_FALSE,
        .shaderInputAttachmentArrayNonUniformIndexing = VK_FALSE,
        .shaderUniformTexelBufferArrayNonUniformIndexing = VK_FALSE,
        .shaderStorageTexelBufferArrayNonUniformIndexing = VK_FALSE,
        .descriptorBindingUniformBufferUpdateAfterBind = VK_FALSE,
        .descriptorBindingSampledImageUpdateAfterBind = VK_FALSE,
        .descriptorBindingStorageImageUpdateAfterBind = VK_FALSE,
        .descriptorBindingStorageBufferUpdateAfterBind = VK_FALSE,
        .descriptorBindingUniformTexelBufferUpdateAfterBind = VK_FALSE,
        .descriptorBindingStorageTexelBufferUpdateAfterBind = VK_FALSE,
        .descriptorBindingUpdateUnusedWhilePending = VK_FALSE,
        .descriptorBindingPartiallyBound = VK_FALSE,
        .descriptorBindingVariableDescriptorCount = VK_TRUE,
        .runtimeDescriptorArray = VK_TRUE,
        .samplerFilterMinmax = VK_FALSE,
        .scalarBlockLayout = VK_FALSE,
        .imagelessFramebuffer = VK_FALSE,
        .uniformBufferStandardLayout = VK_FALSE,
        .shaderSubgroupExtendedTypes = VK_FALSE,
        .separateDepthStencilLayouts = VK_TRUE,
        .hostQueryReset = VK_FALSE,
        .timelineSemaphore = VK_TRUE,
        .bufferDeviceAddress = VK_TRUE,
        .bufferDeviceAddressCaptureReplay = VK_FALSE,
        .bufferDeviceAddressMultiDevice = VK_FALSE,
        .vulkanMemoryModel = VK_TRUE,
        .vulkanMemoryModelDeviceScope = VK_TRUE,
        .vulkanMemoryModelAvailabilityVisibilityChains = VK_FALSE,
        .shaderOutputViewportIndex = VK_FALSE,
        .shaderOutputLayer = VK_FALSE,
        .subgroupBroadcastDynamicId = VK_FALSE,
    };

    VkPhysicalDeviceRobustness2FeaturesEXT physicalDeviceRobustness2Features =
    {
        .sType = VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_ROBUSTNESS_2_FEATURES_EXT,
        .pNext = &physicalDeviceVulkan12Features,
        .robustBufferAccess2 = VK_FALSE,
        .robustImageAccess2 = VK_FALSE,
        .nullDescriptor = VK_TRUE,
    };

    VkPhysicalDeviceVulkan13Features physicalDeviceVulkan13Features =
    {
        .sType = VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_VULKAN_1_3_FEATURES,
        .pNext = &physicalDeviceRobustness2Features,
        .robustImageAccess = VK_FALSE,
        .inlineUniformBlock = VK_FALSE,
        .descriptorBindingInlineUniformBlockUpdateAfterBind = VK_FALSE,
        .pipelineCreationCacheControl = VK_FALSE,
        .privateData = VK_FALSE,
        .shaderDemoteToHelperInvocation = VK_FALSE,
        .shaderTerminateInvocation = VK_FALSE,
        .subgroupSizeControl = VK_FALSE,
        .computeFullSubgroups = VK_FALSE,
        .synchronization2 = VK_FALSE,
        .textureCompressionASTC_HDR = VK_FALSE,
        .shaderZeroInitializeWorkgroupMemory = VK_FALSE,
        .dynamicRendering = VK_FALSE,
        .shaderIntegerDotProduct = VK_FALSE,
        .maintenance4 = VK_FALSE
    };

    VkPhysicalDeviceVertexAttributeRobustnessFeaturesEXT vertexAttributeRobustnessFeatures =
    {
        .sType = VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_VERTEX_ATTRIBUTE_ROBUSTNESS_FEATURES_EXT,
        .pNext = &physicalDeviceVulkan13Features,
        .vertexAttributeRobustness = VK_TRUE
    };

    VkPhysicalDeviceVulkan11Features physicalDeviceVulkan11Features =
    {
        .sType = VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_VULKAN_1_1_FEATURES,
        .pNext = &vertexAttributeRobustnessFeatures,
        .multiview = VK_TRUE
    };

    VkDeviceCreateInfo deviceCreateInfo =
    {
        .sType = VK_STRUCTURE_TYPE_DEVICE_CREATE_INFO,
        .pNext = &physicalDeviceVulkan11Features,
        .flags = 0,
        .queueCreateInfoCount = static_cast<uint32>(queueCreateInfoList.size()),
        .pQueueCreateInfos = queueCreateInfoList.data(),
        .enabledLayerCount = 0,
        .ppEnabledLayerNames = nullptr,
        .enabledExtensionCount = ARRAY_SIZE(DeviceExtensionList),
        .ppEnabledExtensionNames = DeviceExtensionList,
        .pEnabledFeatures = nullptr
    };

#ifdef NDEBUG
    deviceCreateInfo.enabledLayerCount = 0;
#else
    deviceCreateInfo.enabledLayerCount = ARRAY_SIZE(ValidationLayers);
    deviceCreateInfo.ppEnabledLayerNames = ValidationLayers;
#endif

    VULKAN_RESULT(vkCreateDevice(physicalDevice, &deviceCreateInfo, NULL, &device));
    return device;
}

VkCommandPool Renderer_SetUpCommandPool(VkDevice device, uint32 graphicsFamily)
{
    VkCommandPool commandPool = VK_NULL_HANDLE;
    VkCommandPoolCreateInfo CommandPoolCreateInfo =
    {
        .sType = VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO,
        .flags = VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT,
        .queueFamilyIndex = graphicsFamily
    };
    VULKAN_RESULT(vkCreateCommandPool(device, &CommandPoolCreateInfo, NULL, &commandPool));
    return commandPool;
}


VkResult Renderer_GetDeviceQueue(VkDevice device, uint32 graphicsFamily, uint32 presentFamily, VkQueue& graphicsQueue, VkQueue& presentQueue)
{
    vkGetDeviceQueue(device, graphicsFamily, 0, &graphicsQueue);
    vkGetDeviceQueue(device, presentFamily, 0, &presentQueue);
    return VK_SUCCESS;
}

VkSurfaceFormatKHR SwapChain_FindSwapSurfaceFormat(Vector<VkSurfaceFormatKHR>& availableFormats)
{
    for (uint32_t x = 0; x < availableFormats.size(); x++)
    {
        if (availableFormats[x].format == VK_FORMAT_R8G8B8A8_UNORM &&
            availableFormats[x].colorSpace == VK_COLOR_SPACE_SRGB_NONLINEAR_KHR)
        {
            return availableFormats[x];
        }
    }
    fprintf(stderr, "Couldn't find a usable swap surface format.\n");
    return VkSurfaceFormatKHR{ VK_FORMAT_UNDEFINED, VK_COLOR_SPACE_MAX_ENUM_KHR };
}

VkPresentModeKHR SwapChain_FindSwapPresentMode(Vector<VkPresentModeKHR>& availablePresentModes)
{
    for (uint32_t x = 0; x < availablePresentModes.size(); x++)
    {
        if (availablePresentModes[x] == VK_PRESENT_MODE_MAILBOX_KHR)
        {
            return availablePresentModes[x];
        }
    }
    return VK_PRESENT_MODE_FIFO_KHR;
}

VkResult SwapChain_GetQueueFamilies(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint32& graphicsFamily, uint32& presentFamily)
{
    uint32 queueFamilyCount = 0;
    vkGetPhysicalDeviceQueueFamilyProperties(physicalDevice, &queueFamilyCount, NULL);

    Vector<VkQueueFamilyProperties> queueFamilyList(queueFamilyCount);
    vkGetPhysicalDeviceQueueFamilyProperties(physicalDevice, &queueFamilyCount, queueFamilyList.data());

    for (uint32 x = 0; x <= queueFamilyCount; x++)
    {
        VkBool32 presentSupport = false;
        vkGetPhysicalDeviceSurfaceSupportKHR(physicalDevice, x, surface, &presentSupport);

        if (queueFamilyList[x].queueFlags & VK_QUEUE_GRAPHICS_BIT)
        {
            presentFamily = x;
            graphicsFamily = x;
            break;
        }
    }

    return VK_SUCCESS;
}

VkSurfaceCapabilitiesKHR SwapChain_GetSurfaceCapabilities(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface)
{
    VkSurfaceCapabilitiesKHR surfaceCapabilities;
    VULKAN_RESULT(vkGetPhysicalDeviceSurfaceCapabilitiesKHR(physicalDevice, surface, &surfaceCapabilities));
    return surfaceCapabilities;
}

Vector<VkSurfaceFormatKHR> SwapChain_GetPhysicalDeviceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface)
{
    uint32 surfaceFormatCount = 0;
    VULKAN_RESULT(vkGetPhysicalDeviceSurfaceFormatsKHR(physicalDevice, surface, &surfaceFormatCount, nullptr));

    Vector<VkSurfaceFormatKHR> compatibleSwapChainFormatList = Vector<VkSurfaceFormatKHR>(surfaceFormatCount);
    VULKAN_RESULT(vkGetPhysicalDeviceSurfaceFormatsKHR(physicalDevice, surface, &surfaceFormatCount, compatibleSwapChainFormatList.data()));

    return compatibleSwapChainFormatList;
}

Vector<VkPresentModeKHR> SwapChain_GetPhysicalDevicePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface)
{
    uint32 presentModeCount = 0;
    VULKAN_RESULT(vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice, surface, &presentModeCount, nullptr));

    Vector<VkPresentModeKHR> compatiblePresentModesList = Vector<VkPresentModeKHR>(presentModeCount);
    VULKAN_RESULT(vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice, surface, &presentModeCount, compatiblePresentModesList.data()));

    return compatiblePresentModesList;
}

VkSwapchainKHR SwapChain_SetUpSwapChain(VkDevice device, VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, uint32 graphicsFamily, uint32 presentFamily, uint32 width, uint32 height, uint32& swapChainImageCount)
{
    VkSwapchainKHR swapChain = VK_NULL_HANDLE;

    VkExtent2D extent =
    {
        width,
        height
    };

    Vector<VkSurfaceFormatKHR> compatibleSwapChainFormatList = SwapChain_GetPhysicalDeviceFormats(physicalDevice, surface);
    Vector<VkPresentModeKHR> compatiblePresentModesList = SwapChain_GetPhysicalDevicePresentModes(physicalDevice, surface);

    VkSurfaceCapabilitiesKHR surfaceCapabilities = SwapChain_GetSurfaceCapabilities(physicalDevice, surface);
    VkSurfaceFormatKHR swapChainImageFormat = SwapChain_FindSwapSurfaceFormat(compatibleSwapChainFormatList);
    VkPresentModeKHR swapChainPresentMode = SwapChain_FindSwapPresentMode(compatiblePresentModesList);

    swapChainImageCount = surfaceCapabilities.minImageCount + 1;
    if (surfaceCapabilities.maxImageCount > 0 &&
        swapChainImageCount > surfaceCapabilities.maxImageCount)
    {
        swapChainImageCount = surfaceCapabilities.maxImageCount;
    }

    VkSwapchainCreateInfoKHR SwapChainCreateInfo =
    {
        .sType = VK_STRUCTURE_TYPE_SWAPCHAIN_CREATE_INFO_KHR,
        .surface = surface,
        .minImageCount = swapChainImageCount,
        .imageFormat = swapChainImageFormat.format,
        .imageColorSpace = swapChainImageFormat.colorSpace,
        .imageExtent = extent,
        .imageArrayLayers = 1,
        .imageUsage = VK_IMAGE_USAGE_TRANSFER_DST_BIT | VK_IMAGE_USAGE_COLOR_ATTACHMENT_BIT | VK_IMAGE_USAGE_TRANSFER_SRC_BIT,
        .preTransform = surfaceCapabilities.currentTransform,
        .compositeAlpha = VK_COMPOSITE_ALPHA_OPAQUE_BIT_KHR,
        .presentMode = swapChainPresentMode,
        .clipped = VK_TRUE
    };

    if (graphicsFamily != presentFamily)
    {
        Vector<uint32> queueFamilyIndices = { graphicsFamily, presentFamily };

        SwapChainCreateInfo.imageSharingMode = VK_SHARING_MODE_CONCURRENT;
        SwapChainCreateInfo.queueFamilyIndexCount = static_cast<uint32>(queueFamilyIndices.size());
        SwapChainCreateInfo.pQueueFamilyIndices = queueFamilyIndices.data();
    }
    else
    {
        SwapChainCreateInfo.imageSharingMode = VK_SHARING_MODE_EXCLUSIVE;
    }
    VULKAN_RESULT(vkCreateSwapchainKHR(device, &SwapChainCreateInfo, nullptr, &swapChain));
    return swapChain;
}

Vector<VkImage> SwapChain_SetUpSwapChainImages(VkDevice device, VkSwapchainKHR swapChain, uint32 swapChainImageCount)
{
    Vector<VkImage> swapChainImageList;
    VULKAN_RESULT(vkGetSwapchainImagesKHR(device, swapChain, &swapChainImageCount, nullptr));

    swapChainImageList = Vector<VkImage>(swapChainImageCount);
    VULKAN_RESULT(vkGetSwapchainImagesKHR(device, swapChain, &swapChainImageCount, swapChainImageList.data()));

    return swapChainImageList;
}

Vector<VkImageView> SwapChain_SetUpSwapChainImageViews(VkDevice device, Vector<VkImage> swapChainImageList, VkSurfaceFormatKHR swapChainImageFormat)
{
    std::vector<VkImageView> swapChainImageViewList(swapChainImageList.size());
    for (uint32_t x = 0; x < swapChainImageList.size(); x++)
    {
        VkImageViewCreateInfo swapChainViewInfo =
        {
            .sType = VK_STRUCTURE_TYPE_IMAGE_VIEW_CREATE_INFO,
            .image = swapChainImageList[x],
            .viewType = VK_IMAGE_VIEW_TYPE_2D,
            .format = swapChainImageFormat.format,
            .subresourceRange =
            {
                .aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
                .baseMipLevel = 0,
                .levelCount = 1,
                .baseArrayLayer = 0,
                .layerCount = 1
            }
        };
        VULKAN_RESULT(vkCreateImageView(device, &swapChainViewInfo, nullptr, &swapChainImageViewList[x]));
    }

    return swapChainImageViewList;
}

VkResult Renderer_SetUpSemaphores(VkDevice device, Vector<VkFence>& inFlightFences, Vector<VkSemaphore>& acquireImageSemaphores, Vector<VkSemaphore>& presentImageSemaphores)
{
    inFlightFences.resize(MAX_FRAMES_IN_FLIGHT);
    acquireImageSemaphores.resize(MAX_FRAMES_IN_FLIGHT);
    presentImageSemaphores.resize(MAX_FRAMES_IN_FLIGHT);

    return Renderer_CreateSemaphores(device, inFlightFences.data(), acquireImageSemaphores.data(), presentImageSemaphores.data(), MAX_FRAMES_IN_FLIGHT);
}

