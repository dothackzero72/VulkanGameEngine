#include "CVulkanRenderer.h"
#include "VulkanWindow.h"
#include "GLFWWindow.h"
#include "CTypedef.h"

RendererState renderer = { 0 };
//RayTracingFunctions RTX = { 0 };

static const char* DeviceExtensionList[] = {
    VK_KHR_SWAPCHAIN_EXTENSION_NAME,
    VK_KHR_MAINTENANCE3_EXTENSION_NAME,
    VK_KHR_BUFFER_DEVICE_ADDRESS_EXTENSION_NAME,
    VK_EXT_DESCRIPTOR_INDEXING_EXTENSION_NAME,
    VK_KHR_SPIRV_1_4_EXTENSION_NAME,
    VK_KHR_SHADER_FLOAT_CONTROLS_EXTENSION_NAME,
    VK_KHR_SHADER_NON_SEMANTIC_INFO_EXTENSION_NAME,
    VK_KHR_RAY_TRACING_PIPELINE_EXTENSION_NAME,
    VK_KHR_ACCELERATION_STRUCTURE_EXTENSION_NAME,
    VK_KHR_DEFERRED_HOST_OPERATIONS_EXTENSION_NAME,
    VK_EXT_ROBUSTNESS_2_EXTENSION_NAME
};

static const char* ValidationLayers[] = { "VK_LAYER_KHRONOS_validation" };

static VkValidationFeatureEnableEXT enabledList[] = { VK_VALIDATION_FEATURE_ENABLE_DEBUG_PRINTF_EXT,
                                                      VK_VALIDATION_FEATURE_ENABLE_GPU_ASSISTED_RESERVE_BINDING_SLOT_EXT };

static VkValidationFeatureDisableEXT disabledList[] = { VK_VALIDATION_FEATURE_DISABLE_THREAD_SAFETY_EXT,
                                                        VK_VALIDATION_FEATURE_DISABLE_API_PARAMETERS_EXT,
                                                        VK_VALIDATION_FEATURE_DISABLE_OBJECT_LIFETIMES_EXT,
                                                        VK_VALIDATION_FEATURE_DISABLE_CORE_CHECKS_EXT };

static bool Array_RendererExtensionPropertiesSearch(VkExtensionProperties* array, uint32 arrayCount, const char* target)
{
    for (uint32 x = 0; x < arrayCount; x++)
    {
        if (strcmp(array[x].extensionName, target) == 0)
        {
            return true;
        }
    }
    return false;
}

VkResult Renderer_RendererSetUp()
{
    renderer.RebuildRendererFlag = false;
    VkDebugUtilsMessengerEXT debugMessenger = VK_NULL_HANDLE;
    renderer.Instance = Renderer_CreateVulkanInstance(&debugMessenger);
    renderer.DebugMessenger = Renderer_SetupDebugMessenger(renderer.Instance);
    vulkanWindow->CreateSurface(vulkanWindow, &renderer.Instance, &renderer.Surface);
    VkResult deviceSetupResult = Renderer_SetUpPhysicalDevice(renderer.Instance, &renderer.PhysicalDevice, renderer.Surface, &renderer.PhysicalDeviceFeatures, &renderer.SwapChain.GraphicsFamily, &renderer.SwapChain.PresentFamily);
    renderer.Device = Renderer_SetUpDevice(renderer.PhysicalDevice, renderer.SwapChain.GraphicsFamily, renderer.SwapChain.PresentFamily);
    VULKAN_RESULT(Vulkan_SetUpSwapChain(renderer.Device, renderer.PhysicalDevice, renderer.Surface));
    renderer.CommandPool = Renderer_SetUpCommandPool(renderer.Device, renderer.SwapChain.GraphicsFamily);
    VULKAN_RESULT(Renderer_SetUpSemaphores(renderer.Device, &renderer.InFlightFences, &renderer.AcquireImageSemaphores, &renderer.PresentImageSemaphores, MAX_FRAMES_IN_FLIGHT));
    VULKAN_RESULT(Renderer_GetDeviceQueue(renderer.Device, renderer.SwapChain.GraphicsFamily, renderer.SwapChain.PresentFamily, &renderer.SwapChain.GraphicsQueue, &renderer.SwapChain.PresentQueue));
    return VK_SUCCESS;
}

 VkResult CreateDebugUtilsMessengerEXT(VkInstance instance, const VkDebugUtilsMessengerCreateInfoEXT* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkDebugUtilsMessengerEXT* pDebugMessenger)
{
    PFN_vkCreateDebugUtilsMessengerEXT func = (PFN_vkCreateDebugUtilsMessengerEXT)vkGetInstanceProcAddr(instance, "vkCreateDebugUtilsMessengerEXT");
    if (func != NULL)
    {
        return func(instance, pCreateInfo, pAllocator, pDebugMessenger);
    }
    else
    {
        return VK_ERROR_EXTENSION_NOT_PRESENT;
    }
}

 void DestroyDebugUtilsMessengerEXT(VkInstance instance, const VkAllocationCallbacks* pAllocator)
{
    PFN_vkDestroyDebugUtilsMessengerEXT func = (PFN_vkDestroyDebugUtilsMessengerEXT)vkGetInstanceProcAddr(instance, "vkDestroyDebugUtilsMessengerEXT");
    if (func != NULL)
    {
        func(instance, renderer.DebugMessenger, pAllocator);
    }
    else
    {
        fprintf(stderr, "Failed to load vkDestroyDebugUtilsMessengerEXT function\n");
    }
}

VKAPI_ATTR VkBool32 VKAPI_CALL Vulkan_DebugCallBack(VkDebugUtilsMessageSeverityFlagBitsEXT MessageSeverity, VkDebugUtilsMessageTypeFlagsEXT MessageType, const VkDebugUtilsMessengerCallbackDataEXT* CallBackData, void* UserData)
{
    RichTextBoxCallback callback = (RichTextBoxCallback)UserData;
    if (callback)
    {
        fprintf(stderr, "Validation Layer: %s\n", CallBackData->pMessage);
    }
    else
    {
        char message[4096];
        snprintf(message, sizeof(message), "Vulkan Message [Severity: %d, Type: %d]: %s",
            MessageSeverity, MessageType, CallBackData->pMessage);

        switch (MessageSeverity)
        {
        case VK_DEBUG_UTILS_MESSAGE_SEVERITY_VERBOSE_BIT_EXT:
            fprintf(stdout, "VERBOSE: %s\n", message);
            break;
        case VK_DEBUG_UTILS_MESSAGE_SEVERITY_INFO_BIT_EXT:
            fprintf(stdout, "INFO: %s\n", message);
            break;
        case VK_DEBUG_UTILS_MESSAGE_SEVERITY_WARNING_BIT_EXT:
            fprintf(stderr, "WARNING: %s\n", message);
            break;
        case VK_DEBUG_UTILS_MESSAGE_SEVERITY_ERROR_BIT_EXT:
            fprintf(stderr, "ERROR: %s\n", message);
            break;
        default:
            fprintf(stderr, "UNKNOWN SEVERITY: %s\n", message);
            break;
        }
        if (callback)
        callback(message);
    }
    return VK_FALSE;
}

 VkExtensionProperties* Renderer_GetDeviceExtensions(VkPhysicalDevice physicalDevice, uint32* deviceExtensionCountPtr)
{
    uint32 deviceExtensionCount = 0;
    VULKAN_RESULT(vkEnumerateDeviceExtensionProperties(physicalDevice, NULL, &deviceExtensionCount, NULL));

    VkExtensionProperties* deviceExtensions = malloc(sizeof(VkExtensionProperties) * deviceExtensionCount);
    if (!deviceExtensions)
    {
        fprintf(stderr, "Failed to allocate memory for Vulkan.\n");
        *deviceExtensionCountPtr = 0;
        free(deviceExtensions);
        return NULL;
    }

    VULKAN_RESULT(vkEnumerateDeviceExtensionProperties(physicalDevice, NULL, &deviceExtensionCount, deviceExtensions));
    *deviceExtensionCountPtr = deviceExtensionCount;
    free(deviceExtensions);
    return deviceExtensions;
}

 VkResult Renderer_GetSurfaceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkSurfaceFormatKHR* surfaceFormat, uint32* surfaceFormatCount)
{
    VULKAN_RESULT(vkGetPhysicalDeviceSurfaceFormatsKHR(physicalDevice, surface, surfaceFormatCount, NULL));
    if (surfaceFormatCount > 0)
    {
        VkSurfaceFormatKHR* surfaceFormats = malloc(sizeof(VkSurfaceFormatKHR) * (*surfaceFormatCount));
        if (!surfaceFormats)
        {
            fprintf(stderr, "Failed to allocate memory for Vulkan.\n");
        }
        VkResult result = vkGetPhysicalDeviceSurfaceFormatsKHR(physicalDevice, surface, surfaceFormatCount, surfaceFormats);
        free(surfaceFormats);
        return result;
    }
}

 VkResult Renderer_GetPresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkPresentModeKHR* presentMode, int32* presentModeCount)
{
    VULKAN_RESULT(vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice, surface, presentModeCount, NULL));
    if (presentModeCount > 0)
    {
        VkPresentModeKHR* presentModes = malloc(sizeof(VkPresentModeKHR) * *presentModeCount);
        if (!presentModes)
        {
            fprintf(stderr, "Failed to allocate memory for Vulkan.\n");
        }
        VkResult result = vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice, surface, presentModeCount, presentModes);
        free(presentModes);
        return result;
    }
}

 bool Renderer_GetRayTracingSupport()
{
   /* uint32 deviceExtensionCount = INT32_MAX;
    VkExtensionProperties* deviceExtensions = GetDeviceExtensions(Renderer.PhysicalDevice, &deviceExtensionCount);
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
    vkGetPhysicalDeviceFeatures2(Renderer.PhysicalDevice, &physicalDeviceFeatures2);

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

 void Renderer_GetRendererFeatures(VkPhysicalDeviceVulkan11Features* physicalDeviceVulkan11Features)
{
    VkPhysicalDeviceBufferDeviceAddressFeatures bufferDeviceAddressFeatures =
    {
        .sType = VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_BUFFER_DEVICE_ADDRESS_FEATURES,
        .bufferDeviceAddress = VK_TRUE,
    };

    VkPhysicalDeviceDescriptorIndexingFeatures physicalDeviceDescriptorIndexingFeatures =
    {
        .sType = VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_DESCRIPTOR_INDEXING_FEATURES,
        .runtimeDescriptorArray = VK_TRUE,
        .shaderSampledImageArrayNonUniformIndexing = VK_TRUE,
        .descriptorBindingVariableDescriptorCount = VK_TRUE,
    };

    VkPhysicalDeviceRobustness2FeaturesEXT PhysicalDeviceRobustness2Features =
    {
        .sType = VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_ROBUSTNESS_2_FEATURES_EXT,
        .nullDescriptor = VK_TRUE,
        .pNext = &physicalDeviceDescriptorIndexingFeatures,
    };

    if (Renderer_GetRayTracingSupport())
    {
        VkPhysicalDeviceAccelerationStructureFeaturesKHR accelerationStructureFeatures =
        {
            .sType = VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_ACCELERATION_STRUCTURE_FEATURES_KHR,
            .accelerationStructure = VK_TRUE,
        };

        VkPhysicalDeviceRayTracingPipelineFeaturesKHR rayTracingPipelineFeatures =
        {
            .sType = VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_RAY_TRACING_PIPELINE_FEATURES_KHR,
            .rayTracingPipeline = VK_TRUE,
            .pNext = &accelerationStructureFeatures,
        };

        bufferDeviceAddressFeatures.pNext = &rayTracingPipelineFeatures;
    }
    PhysicalDeviceRobustness2Features.pNext = &bufferDeviceAddressFeatures;

    VkPhysicalDeviceFeatures deviceFeatures =
    {
        .samplerAnisotropy = VK_TRUE,
        .fillModeNonSolid = VK_TRUE,
    };

    VkPhysicalDeviceFeatures2 deviceFeatures2 =
    {
        .sType = VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_FEATURES_2,
        .features = deviceFeatures,
        .pNext = &PhysicalDeviceRobustness2Features,
    };

    physicalDeviceVulkan11Features->sType = VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_VULKAN_1_1_FEATURES;
    physicalDeviceVulkan11Features->multiview = VK_TRUE;
    physicalDeviceVulkan11Features->pNext = &deviceFeatures2;
}

void Renderer_GetWin32Extensions(uint32_t* extensionCount, const char*** enabledExtensions) 
{
    vkEnumerateInstanceExtensionProperties(NULL, extensionCount, NULL);
    VkExtensionProperties* extensions = (VkExtensionProperties*)malloc(sizeof(VkExtensionProperties) * (*extensionCount));
    vkEnumerateInstanceExtensionProperties(NULL, extensionCount, extensions);

    const char** extensionNames = (const char**)malloc(sizeof(const char*) * (*extensionCount));
    for (uint32_t x = 0; x < *extensionCount; x++)
    {
        extensionNames[x] = (const char*)malloc(256);
        strncpy((char*)extensionNames[x], extensions[x].extensionName, 256);
        printf("Extension: %s, Spec Version: %d\n", extensions[x].extensionName, extensions[x].specVersion);
    }

    free(extensions);
    *enabledExtensions = extensionNames;
}

VkInstance Renderer_CreateVulkanInstance()
{
    VkInstance instance = VK_NULL_HANDLE;
    VkDebugUtilsMessengerEXT* debugMessenger = VK_NULL_HANDLE;
    VkDebugUtilsMessengerCreateInfoEXT debugInfo =
    {
        .sType = VK_STRUCTURE_TYPE_DEBUG_UTILS_MESSENGER_CREATE_INFO_EXT,
        .messageSeverity = VK_DEBUG_UTILS_MESSAGE_SEVERITY_INFO_BIT_EXT |
                          VK_DEBUG_UTILS_MESSAGE_SEVERITY_VERBOSE_BIT_EXT |
                          VK_DEBUG_UTILS_MESSAGE_SEVERITY_WARNING_BIT_EXT |
                          VK_DEBUG_UTILS_MESSAGE_SEVERITY_ERROR_BIT_EXT,
        .messageType = VK_DEBUG_UTILS_MESSAGE_TYPE_GENERAL_BIT_EXT |
                       VK_DEBUG_UTILS_MESSAGE_TYPE_VALIDATION_BIT_EXT |
                       VK_DEBUG_UTILS_MESSAGE_TYPE_PERFORMANCE_BIT_EXT,
        .pfnUserCallback = Vulkan_DebugCallBack
    };

    int enableValidationLayers = 1;
#ifdef NDEBUG
    enableValidationLayers = 0;
#endif

    uint32_t extensionCount = 0;
    const VkExtensionProperties* extensions = NULL;
    Renderer_GetWin32Extensions(&extensionCount, &extensions);

    VkApplicationInfo applicationInfo = {
        .sType = VK_STRUCTURE_TYPE_APPLICATION_INFO,
        .pApplicationName = "Vulkan Application",
        .applicationVersion = VK_MAKE_VERSION(1, 0, 0),
        .pEngineName = "No Engine",
        .engineVersion = VK_MAKE_VERSION(1, 0, 0),
        .apiVersion = VK_API_VERSION_1_3
    };

    VkInstanceCreateInfo vulkanCreateInfo = {
        .sType = VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO,
        .pApplicationInfo = &applicationInfo,
        .enabledLayerCount = (enableValidationLayers),
        .ppEnabledLayerNames = (enableValidationLayers ? ValidationLayers : NULL),
        .enabledExtensionCount = extensionCount,
        .ppEnabledExtensionNames = extensions,
        .pNext = (enableValidationLayers ? &debugInfo : NULL)
    };

    VkResult result = vkCreateInstance(&vulkanCreateInfo, NULL, &instance);
    if (result != VK_SUCCESS) {
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
        .messageSeverity = VK_DEBUG_UTILS_MESSAGE_SEVERITY_INFO_BIT_EXT | VK_DEBUG_UTILS_MESSAGE_SEVERITY_VERBOSE_BIT_EXT | VK_DEBUG_UTILS_MESSAGE_SEVERITY_WARNING_BIT_EXT | VK_DEBUG_UTILS_MESSAGE_SEVERITY_ERROR_BIT_EXT,
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

 VkResult Renderer_SetUpPhysicalDevice(VkInstance instance, VkPhysicalDevice* physicalDevice, VkSurfaceKHR surface, VkPhysicalDeviceFeatures* physicalDeviceFeatures, uint32* graphicsFamily, uint32* presentFamily)
 {
     fprintf(stderr, "Entered DLL_Renderer_SetUpPhysicalDevice\n");
     uint32 deviceCount = 0;
     VkResult result = vkEnumeratePhysicalDevices(instance, &deviceCount, NULL);
     fprintf(stderr, "Device count: %d\n", deviceCount);

     VULKAN_RESULT(vkEnumeratePhysicalDevices(instance, &deviceCount, NULL));
     VkPhysicalDevice* physicalDeviceList = malloc(sizeof(VkPhysicalDevice) * deviceCount);
     VULKAN_RESULT(vkEnumeratePhysicalDevices(instance, &deviceCount, physicalDeviceList));

     VkPresentModeKHR* presentMode = NULL;
     for (uint32 x = 0; x < deviceCount; x++)
     {
         VkSurfaceFormatKHR surfaceFormat;
         VkPresentModeKHR presentMode;
         uint32 surfaceFormatCount = 0;
         uint32 presentModeCount = 0;

         vkGetPhysicalDeviceFeatures(physicalDeviceList[x], physicalDeviceFeatures);
         VULKAN_RESULT(SwapChain_GetQueueFamilies(physicalDeviceList[x], surface, graphicsFamily, presentFamily));
         VULKAN_RESULT(Renderer_GetSurfaceFormats(physicalDeviceList[x], surface, &surfaceFormat, &surfaceFormatCount));
         VULKAN_RESULT(Renderer_GetPresentModes(physicalDeviceList[x], surface, &presentMode, &presentModeCount));

         if (*graphicsFamily != -1 &&
             *presentFamily != -1 &&
             surfaceFormatCount > 0 &&
             presentModeCount != 0 &&
             physicalDeviceFeatures->samplerAnisotropy)
         {
             *physicalDevice = physicalDeviceList[x];
             break;
         }
         else
         {
             free(physicalDeviceList);
             return VK_ERROR_DEVICE_LOST;
         }
     }

     free(physicalDeviceList);
     return VK_SUCCESS;
 }

 VkDevice Renderer_SetUpDevice(VkPhysicalDevice physicalDevice, uint32 graphicsFamily, uint32 presentFamily)
 {
     VkDevice device = VK_NULL_HANDLE;

     float queuePriority = 1.0f;
     VkDeviceQueueCreateInfo queueCreateInfo[2];
     uint32 queueCreateInfoCount = 0;
     if (graphicsFamily != UINT32_MAX)
     {
         queueCreateInfo[queueCreateInfoCount++] = (VkDeviceQueueCreateInfo){
             .sType = VK_STRUCTURE_TYPE_DEVICE_QUEUE_CREATE_INFO,
             .queueFamilyIndex = graphicsFamily,
             .queueCount = 1,
             .pQueuePriorities = &queuePriority
         };
     }

     if (presentFamily != UINT32_MAX &&
         presentFamily != graphicsFamily)
     {
         queueCreateInfo[queueCreateInfoCount++] = (VkDeviceQueueCreateInfo)
         {
             .sType = VK_STRUCTURE_TYPE_DEVICE_QUEUE_CREATE_INFO,
             .queueFamilyIndex = presentFamily,
             .queueCount = 1,
             .pQueuePriorities = &queuePriority
         };
     }

     VkPhysicalDeviceBufferDeviceAddressFeatures bufferDeviceAddressFeatures =
     {
         .sType = VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_BUFFER_DEVICE_ADDRESS_FEATURES,
         .bufferDeviceAddress = VK_TRUE,
     };

     VkPhysicalDeviceDescriptorIndexingFeatures physicalDeviceDescriptorIndexingFeatures =
     {
         .sType = VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_DESCRIPTOR_INDEXING_FEATURES,
         .runtimeDescriptorArray = VK_TRUE,
         .shaderSampledImageArrayNonUniformIndexing = VK_TRUE,
         .descriptorBindingVariableDescriptorCount = VK_TRUE,
     };

     VkPhysicalDeviceRobustness2FeaturesEXT PhysicalDeviceRobustness2Features =
     {
         .sType = VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_ROBUSTNESS_2_FEATURES_EXT,
         .nullDescriptor = VK_TRUE,
         .pNext = &physicalDeviceDescriptorIndexingFeatures,
     };

     if (Renderer_GetRayTracingSupport())
     {
         VkPhysicalDeviceAccelerationStructureFeaturesKHR accelerationStructureFeatures =
         {
             .sType = VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_ACCELERATION_STRUCTURE_FEATURES_KHR,
             .accelerationStructure = VK_TRUE,
         };

         VkPhysicalDeviceRayTracingPipelineFeaturesKHR rayTracingPipelineFeatures =
         {
             .sType = VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_RAY_TRACING_PIPELINE_FEATURES_KHR,
             .rayTracingPipeline = VK_TRUE,
             .pNext = &accelerationStructureFeatures,
         };

         bufferDeviceAddressFeatures.pNext = &rayTracingPipelineFeatures;
     }
     PhysicalDeviceRobustness2Features.pNext = &bufferDeviceAddressFeatures;

     VkPhysicalDeviceFeatures deviceFeatures =
     {
         .samplerAnisotropy = VK_TRUE,
         .fillModeNonSolid = VK_TRUE,
     };

     VkPhysicalDeviceFeatures2 deviceFeatures2 =
     {
         .sType = VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_FEATURES_2,
         .features = deviceFeatures,
         .pNext = &PhysicalDeviceRobustness2Features,
     };

     VkPhysicalDeviceVulkan11Features physicalDeviceVulkan11Features = { 0 };
     physicalDeviceVulkan11Features.sType = VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_VULKAN_1_1_FEATURES;
     physicalDeviceVulkan11Features.multiview = VK_TRUE;
     physicalDeviceVulkan11Features.pNext = &deviceFeatures2;

     VkDeviceCreateInfo deviceCreateInfo =
     {
         .sType = VK_STRUCTURE_TYPE_DEVICE_CREATE_INFO,
         .queueCreateInfoCount = queueCreateInfoCount,
         .pQueueCreateInfos = queueCreateInfo,
         .pEnabledFeatures = NULL,
         .enabledExtensionCount = ARRAY_SIZE(DeviceExtensionList),
         .ppEnabledExtensionNames = DeviceExtensionList,
         .pNext = &physicalDeviceVulkan11Features
     };

#ifdef NDEBUG
     deviceCreateInfo.enabledLayerCount = 0;
#else
     deviceCreateInfo.enabledLayerCount = ARRAY_SIZE(ValidationLayers);
     deviceCreateInfo.ppEnabledLayerNames = ValidationLayers;
#endif

      vkCreateDevice(physicalDevice, &deviceCreateInfo, NULL, &device);
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

 VkResult Renderer_SetUpSemaphores(VkDevice device, VkFence** inFlightFences, VkSemaphore** acquireImageSemaphores, VkSemaphore** presentImageSemaphores, int maxFramesInFlight)
 {
     *inFlightFences = malloc(sizeof(VkFence) * maxFramesInFlight);
     if (*inFlightFences == NULL)
     {
         return VK_ERROR_OUT_OF_HOST_MEMORY; 
     }

     *acquireImageSemaphores = malloc(sizeof(VkSemaphore) * maxFramesInFlight);
     if (*acquireImageSemaphores == NULL)
     {
         free(*inFlightFences);
         return VK_ERROR_OUT_OF_HOST_MEMORY; 
     }

     *presentImageSemaphores = malloc(sizeof(VkSemaphore) * maxFramesInFlight);
     if (*presentImageSemaphores == NULL)
     {
         free(*inFlightFences); 
         free(*acquireImageSemaphores);
         return VK_ERROR_OUT_OF_HOST_MEMORY; 
     }

     VkSemaphoreTypeCreateInfo semaphoreTypeCreateInfo =
     {
         .sType = VK_STRUCTURE_TYPE_SEMAPHORE_TYPE_CREATE_INFO,
         .semaphoreType = VK_SEMAPHORE_TYPE_BINARY,
         .initialValue = 0,
         .pNext = NULL
     };

     VkSemaphoreCreateInfo semaphoreCreateInfo =
     {
         .sType = VK_STRUCTURE_TYPE_SEMAPHORE_CREATE_INFO,
         .pNext = &semaphoreTypeCreateInfo
     };

     VkFenceCreateInfo fenceInfo =
     {
         .sType = VK_STRUCTURE_TYPE_FENCE_CREATE_INFO,
         .flags = VK_FENCE_CREATE_SIGNALED_BIT
     };

     for (size_t x = 0; x < maxFramesInFlight; x++)
     {
         VULKAN_RESULT(vkCreateSemaphore(device, &semaphoreCreateInfo, NULL, &(*acquireImageSemaphores)[x]));
         VULKAN_RESULT(vkCreateSemaphore(device, &semaphoreCreateInfo, NULL, &(*presentImageSemaphores)[x]));
         VULKAN_RESULT(vkCreateFence(device, &fenceInfo, NULL, &(*inFlightFences)[x]));
     }

     return VK_SUCCESS;
 }

 VkResult Renderer_GetDeviceQueue(VkDevice device, uint32 graphicsFamily, uint32 presentFamily, VkQueue* graphicsQueue, VkQueue* presentQueue)
 {
     vkGetDeviceQueue(device, graphicsFamily, 0, graphicsQueue);
     vkGetDeviceQueue(device, presentFamily, 0, presentQueue);
     return VK_SUCCESS;
 }


VkResult Renderer_CreateCommandBuffers(VkCommandBuffer* commandBufferList)
{
    for (size_t x = 0; x < renderer.SwapChain.SwapChainImageCount; x++)
    {
        VkCommandBufferAllocateInfo commandBufferAllocateInfo =
        {
            .sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO,
            .commandPool = renderer.CommandPool,
            .level = VK_COMMAND_BUFFER_LEVEL_PRIMARY,
            .commandBufferCount = 1
        };

        VULKAN_RESULT(vkAllocateCommandBuffers(renderer.Device, &commandBufferAllocateInfo, &commandBufferList[x]));
    }
    return VK_SUCCESS;
}

VkResult Renderer_CreateFrameBuffer(VkFramebuffer* pFrameBuffer, VkFramebufferCreateInfo* frameBufferCreateInfo)
{
    return vkCreateFramebuffer(renderer.Device, *&frameBufferCreateInfo, NULL, pFrameBuffer);
}

VkResult Renderer_CreateRenderPass(Renderer_RenderPassCreateInfoStruct* renderPassCreateInfo)
{
    VkRenderPassCreateInfo renderPassInfo =
    {
        .sType = VK_STRUCTURE_TYPE_RENDER_PASS_CREATE_INFO,
        .attachmentCount = renderPassCreateInfo->AttachmentCount,
        .pAttachments = renderPassCreateInfo->pAttachmentList,
        .subpassCount = renderPassCreateInfo->SubpassCount,
        .pSubpasses = renderPassCreateInfo->pSubpassDescriptionList,
        .dependencyCount = renderPassCreateInfo->DependencyCount,
        .pDependencies = renderPassCreateInfo->pSubpassDependencyList
    };
    return vkCreateRenderPass(renderer.Device, &renderPassInfo, NULL, renderPassCreateInfo->pRenderPass);
}

VkResult Renderer_CreateDescriptorPool(VkDescriptorPool* descriptorPool, VkDescriptorPoolCreateInfo* descriptorPoolCreateInfo)
{
    return vkCreateDescriptorPool(renderer.Device, descriptorPoolCreateInfo, NULL, descriptorPool);
}

VkResult Renderer_CreateDescriptorSetLayout(VkDescriptorSetLayout* descriptorSetLayout, VkDescriptorSetLayoutCreateInfo* descriptorSetLayoutCreateInfo)
{
    return vkCreateDescriptorSetLayout(renderer.Device, descriptorSetLayoutCreateInfo, NULL, descriptorSetLayout);
}

VkResult Renderer_CreatePipelineLayout(VkPipelineLayout* pipelineLayout, VkPipelineLayoutCreateInfo* pipelineLayoutCreateInfo)
{
    return vkCreatePipelineLayout(renderer.Device, pipelineLayoutCreateInfo, NULL, pipelineLayout);
}

VkResult Renderer_AllocateDescriptorSets(VkDescriptorSet* descriptorSet, VkDescriptorSetAllocateInfo* descriptorSetAllocateInfo)
{
    return vkAllocateDescriptorSets(renderer.Device, descriptorSetAllocateInfo, descriptorSet);
}

VkResult Renderer_AllocateCommandBuffers(VkCommandBuffer* commandBuffer, VkCommandBufferAllocateInfo* ImGuiCommandBuffers)
{
    return vkAllocateCommandBuffers(renderer.Device, ImGuiCommandBuffers, commandBuffer);
}

VkResult Renderer_CreateGraphicsPipelines(VkPipeline* graphicPipeline, VkGraphicsPipelineCreateInfo* createGraphicPipelines, uint32 createGraphicPipelinesCount)
{
    return vkCreateGraphicsPipelines(renderer.Device, VK_NULL_HANDLE, createGraphicPipelinesCount, createGraphicPipelines, NULL, graphicPipeline);
}

VkResult Renderer_CreateCommandPool(VkCommandPool* commandPool, VkCommandPoolCreateInfo* commandPoolInfo)
{
    return vkCreateCommandPool(renderer.Device, commandPoolInfo, NULL, commandPool);
}

void Renderer_UpdateDescriptorSet(VkWriteDescriptorSet* writeDescriptorSet, uint32 count)
{
    vkUpdateDescriptorSets(renderer.Device, count, writeDescriptorSet, 0, NULL);
}

VkResult Renderer_RebuildSwapChain()
{
    renderer.RebuildRendererFlag = true;

    VULKAN_RESULT(vkDeviceWaitIdle(renderer.Device));
    Vulkan_DestroyImageView();
    vkDestroySwapchainKHR(renderer.Device, renderer.SwapChain.Swapchain, NULL);
    return Vulkan_RebuildSwapChain(renderer.Device, renderer.PhysicalDevice, renderer.Surface);
}

VkResult Renderer_StartFrame(VkDevice device, VkSwapchainKHR swapChain, VkFence* fenceList, VkSemaphore* acquireImageSemaphoreList, uint32* pImageIndex, uint32* pCommandIndex, bool* pRebuildRendererFlag)
{
    *pCommandIndex = (*pCommandIndex + 1) % MAX_FRAMES_IN_FLIGHT;

    VULKAN_RESULT(vkWaitForFences(device, 1, &fenceList[*pCommandIndex], VK_TRUE, UINT64_MAX));
    VULKAN_RESULT(vkResetFences(device, 1, &fenceList[*pCommandIndex]));

    VkResult result = vkAcquireNextImageKHR(device, swapChain, UINT64_MAX, acquireImageSemaphoreList[*pCommandIndex], VK_NULL_HANDLE, pImageIndex);
    if (result == VK_ERROR_OUT_OF_DATE_KHR)
    {
        *pRebuildRendererFlag = true;
        return result;
    }

    return result;
}

VkResult Renderer_EndFrame(VkSwapchainKHR swapChain, VkSemaphore* acquireImageSemaphoreList, VkSemaphore* presentImageSemaphoreList, VkFence* fenceList, VkQueue graphicsQueue, VkQueue presentQueue, uint32 commandIndex, uint32 imageIndex, VkCommandBuffer* pCommandBufferSubmitList, uint32 commandBufferCount, bool* rebuildRendererFlag)
{
    VkPipelineStageFlags waitStages[] =
    {
        VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT
    };

    VkSubmitInfo submitInfo =
    {
        .sType = VK_STRUCTURE_TYPE_SUBMIT_INFO,
        .waitSemaphoreCount = 1,
        .pWaitSemaphores = &acquireImageSemaphoreList[commandIndex],
        .pWaitDstStageMask = waitStages,
        .commandBufferCount = commandBufferCount,
        .pCommandBuffers = pCommandBufferSubmitList,
        .signalSemaphoreCount = 1,
        .pSignalSemaphores = &presentImageSemaphoreList[imageIndex]
    };
    VULKAN_RESULT(vkQueueSubmit(graphicsQueue, 1, &submitInfo, fenceList[commandIndex]));

    VkPresentInfoKHR presentInfo =
    {
        .sType = VK_STRUCTURE_TYPE_PRESENT_INFO_KHR,
        .waitSemaphoreCount = 1,
        .pWaitSemaphores = &presentImageSemaphoreList[imageIndex],
        .swapchainCount = 1,
        .pSwapchains = &swapChain,
        .pImageIndices = &imageIndex
    };
    VkResult result = vkQueuePresentKHR(presentQueue, &presentInfo);
    if (result == VK_ERROR_OUT_OF_DATE_KHR || result == VK_SUBOPTIMAL_KHR)
    {
        *rebuildRendererFlag = true;
    }

    return result;
}

VkResult Renderer_SubmitDraw(VkSemaphore* acquireImageSemaphoreList, VkSemaphore* presentImageSemaphoreList, VkFence* fenceList, VkQueue graphicsQueue, VkQueue presentQueue, uint32 commandIndex, uint32 imageIndex, VkCommandBuffer* pCommandBufferSubmitList, uint32 commandBufferCount, bool* rebuildRendererFlag)
{
    VkPipelineStageFlags waitStages[] = { VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT };

    VkSubmitInfo submitInfo =
    {
        .sType = VK_STRUCTURE_TYPE_SUBMIT_INFO,
        .waitSemaphoreCount = 1,
        .pWaitSemaphores = &acquireImageSemaphoreList[commandIndex],
        .pWaitDstStageMask = waitStages,
        .commandBufferCount = commandBufferCount,
        .pCommandBuffers = pCommandBufferSubmitList,
        .signalSemaphoreCount = 1,
        .pSignalSemaphores = &presentImageSemaphoreList[imageIndex]
    };
    VULKAN_RESULT(vkQueueSubmit(graphicsQueue, 1, &submitInfo, fenceList[commandIndex]));

    VkPresentInfoKHR presentInfo =
    {
        .sType = VK_STRUCTURE_TYPE_PRESENT_INFO_KHR,
        .waitSemaphoreCount = 1,
        .pWaitSemaphores = &presentImageSemaphoreList[imageIndex],
        .swapchainCount = 1,
        .pSwapchains = &renderer.SwapChain.Swapchain,
        .pImageIndices = &imageIndex
    };
    VkResult result = vkQueuePresentKHR(presentQueue, &presentInfo);
    if (result == VK_ERROR_OUT_OF_DATE_KHR)
    {
        *rebuildRendererFlag = true;
    }
    return result;
}

VkResult Renderer_BeginCommandBuffer(VkCommandBuffer* pCommandBufferList, VkCommandBufferBeginInfo* commandBufferBeginInfo)
{
    return vkBeginCommandBuffer(*pCommandBufferList, commandBufferBeginInfo);
}

VkResult Renderer_EndCommandBuffer(VkCommandBuffer* pCommandBufferList)
{
    return vkEndCommandBuffer(*pCommandBufferList);
}

uint32_t Renderer_GetMemoryType(VkPhysicalDevice physicalDevice, uint32_t typeFilter, VkMemoryPropertyFlags properties) 
{
    VkPhysicalDeviceMemoryProperties memProperties;
    vkGetPhysicalDeviceMemoryProperties(physicalDevice, &memProperties);

    for (uint32_t x = 0; x < memProperties.memoryTypeCount; x++) 
    {
        if ((typeFilter & (1 << x)) &&
            (memProperties.memoryTypes[x].propertyFlags & properties) == properties) 
        {
            return x;
        }
    }

    Renderer_DestroyRenderer();
    vulkanWindow->DestroyWindow(vulkanWindow);
    return UINT32_MAX; 
}

VkCommandBuffer Renderer_BeginSingleUseCommandBuffer(VkDevice device, VkCommandPool commandPool)
{
    VkCommandBuffer commandBuffer = VK_NULL_HANDLE;
    VkCommandBufferAllocateInfo allocInfo =
    {
        .sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO,
        .level = VK_COMMAND_BUFFER_LEVEL_PRIMARY,
        .commandPool = commandPool,
        .commandBufferCount = 1
    };
    VULKAN_RESULT(vkAllocateCommandBuffers(device, &allocInfo, &commandBuffer));

    VkCommandBufferBeginInfo beginInfo =
    {
        .sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
        .flags = VK_COMMAND_BUFFER_USAGE_ONE_TIME_SUBMIT_BIT
    };
    VULKAN_RESULT(vkBeginCommandBuffer(commandBuffer, &beginInfo));
    return commandBuffer;
}

VkResult Renderer_EndSingleUseCommandBuffer(VkDevice device, VkCommandPool commandPool, VkQueue graphicsQueue, VkCommandBuffer commandBuffer)
{
    VkSubmitInfo submitInfo =
    {
        .sType = VK_STRUCTURE_TYPE_SUBMIT_INFO,
        .commandBufferCount = 1,
        .pCommandBuffers = &commandBuffer
    };

    VULKAN_RESULT(vkEndCommandBuffer(commandBuffer));
    VULKAN_RESULT(vkQueueSubmit(graphicsQueue, 1, &submitInfo, VK_NULL_HANDLE));
    VULKAN_RESULT(vkQueueWaitIdle(graphicsQueue));
    vkFreeCommandBuffers(device, commandPool, 1, &commandBuffer);
    return VK_SUCCESS;
}

void Renderer_DestroyRenderer()
{

    Vulkan_DestroyImageView();
    Vulkan_DestroySwapChain();
    Renderer_DestroyFences();
    Renderer_DestroyCommandPool();
    Renderer_DestroyFences();
    Renderer_DestroyDevice();
    Renderer_DestroyDebugger();
    Renderer_DestroySurface();
    Renderer_DestroyInstance();
}

void Renderer_DestroyCommandPool()
{
    Renderer_BaseDestroyCommandPool(&renderer.Device, &renderer.CommandPool);
}

void Renderer_DestroyFences()
{
    Renderer_BaseDestroyFences(renderer.Device, renderer.AcquireImageSemaphores, renderer.PresentImageSemaphores, renderer.InFlightFences, MAX_FRAMES_IN_FLIGHT);
}

void Renderer_DestroyDevice()
{
    Renderer_BaseDestroyDevice(&renderer.Device);
}

void Renderer_DestroySurface()
{
    Renderer_BaseDestroySurface(&renderer.Instance, &renderer.Surface);
}

void Renderer_DestroyDebugger()
{
    Renderer_BaseDestroyDebugger(&renderer.Instance);
}

void Renderer_DestroyInstance()
{
    Renderer_BaseDestroyInstance(&renderer.Instance);
}

void Renderer_BaseDestroyFences(VkDevice* device, VkSemaphore* acquireImageSemaphores, VkSemaphore* presentImageSemaphores, VkFence* fences, size_t semaphoreCount)
{
    for (size_t x = 0; x < semaphoreCount; x++)
    {
        if (acquireImageSemaphores[x] != VK_NULL_HANDLE)
        {
            vkDestroySemaphore(device, acquireImageSemaphores[x], NULL);
            acquireImageSemaphores[x] = VK_NULL_HANDLE;
        }
        if (presentImageSemaphores[x] != VK_NULL_HANDLE)
        {
            vkDestroySemaphore(device, presentImageSemaphores[x], NULL);
            presentImageSemaphores[x] = VK_NULL_HANDLE;
        }
        if (fences[x] != VK_NULL_HANDLE)
        {
            vkDestroyFence(device, fences[x], NULL);
            fences[x] = VK_NULL_HANDLE;
        }
    }
}

void Renderer_BaseDestroyCommandPool(VkDevice* device, VkCommandBuffer* commandPool)
{
    if (*commandPool != VK_NULL_HANDLE)
    {
        vkDestroyCommandPool(*device, *commandPool, NULL);
        *commandPool = VK_NULL_HANDLE;
    }
}

void Renderer_BaseDestroyDevice(VkDevice* device)
{
    if (*device != VK_NULL_HANDLE)
    {
        vkDestroyDevice(*device, NULL);
        *device = VK_NULL_HANDLE;
    }
}

void Renderer_BaseDestroySurface(VkInstance* instance, VkSurfaceKHR* surface)
{
    if (*surface != VK_NULL_HANDLE)
    {
        vkDestroySurfaceKHR(*instance, *surface, NULL);
        *surface = VK_NULL_HANDLE;
    }
}

void Renderer_BaseDestroyDebugger(VkInstance* instance)
{
    DestroyDebugUtilsMessengerEXT(*instance, NULL);
}

void Renderer_BaseDestroyInstance(VkInstance* instance)
{
    if (*instance != VK_NULL_HANDLE)
    {
        vkDestroyInstance(*instance, NULL);
        *instance = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyRenderPass(VkRenderPass* renderPass)
{
    if (*renderPass != VK_NULL_HANDLE)
    {
        vkDestroyRenderPass(renderer.Device, *renderPass, NULL);
        *renderPass = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyFrameBuffers(VkFramebuffer* frameBufferList)
{
    for (size_t x = 0; x < renderer.SwapChain.SwapChainImageCount; x++)
    {
        if (frameBufferList[x] != VK_NULL_HANDLE)
        {
            vkDestroyFramebuffer(renderer.Device, frameBufferList[x], NULL);
            frameBufferList[x] = VK_NULL_HANDLE;
        }
    }
}

void Renderer_DestroyDescriptorPool(VkDescriptorPool* descriptorPool)
{
    if (*descriptorPool != VK_NULL_HANDLE)
    {
        vkDestroyDescriptorPool(renderer.Device, *descriptorPool, NULL);
        *descriptorPool = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyDescriptorSetLayout(VkDescriptorSetLayout* descriptorSetLayout)
{
    if (*descriptorSetLayout != VK_NULL_HANDLE)
    {
        vkDestroyDescriptorSetLayout(renderer.Device, *descriptorSetLayout, NULL);
        *descriptorSetLayout = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyCommandBuffers(VkCommandPool* commandPool, VkCommandBuffer* commandBufferList)
{
    if (*commandBufferList != VK_NULL_HANDLE)
    {
        vkFreeCommandBuffers(renderer.Device, *commandPool, renderer.SwapChain.SwapChainImageCount, &*commandBufferList);
        for (size_t x = 0; x < renderer.SwapChain.SwapChainImageCount; x++)
        {
            commandBufferList[x] = VK_NULL_HANDLE;
        }
    }
}

void Renderer_DestroyCommnadPool(VkCommandPool* commandPool)
{
    if (*commandPool != VK_NULL_HANDLE)
    {
        vkDestroyCommandPool(renderer.Device, *commandPool, NULL);
        *commandPool = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyBuffer(VkBuffer* buffer)
{
    if (*buffer != VK_NULL_HANDLE)
    {
        vkDestroyBuffer(renderer.Device, *buffer, NULL);
        *buffer = VK_NULL_HANDLE;
    }
}

void Renderer_FreeMemory(VkDeviceMemory* memory)
{
    if (*memory != VK_NULL_HANDLE)
    {
        vkFreeMemory(renderer.Device, *memory, NULL);
        *memory = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyImageView(VkImageView* imageView)
{
    if (*imageView != VK_NULL_HANDLE)
    {
        vkDestroyImageView(renderer.Device, *imageView, NULL);
        *imageView = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyImage(VkImage* image)
{
    if (*image != VK_NULL_HANDLE)
    {
        vkDestroyImage(renderer.Device, *image, NULL);
        *image = VK_NULL_HANDLE;
    }
}

void Renderer_DestroySampler(VkSampler* sampler)
{
    if (*sampler != VK_NULL_HANDLE)
    {
        vkDestroySampler(renderer.Device, *sampler, NULL);
        *sampler = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyPipeline(VkPipeline* pipeline)
{
    if (*pipeline != VK_NULL_HANDLE)
    {
        vkDestroyPipeline(renderer.Device, *pipeline, NULL);
        *pipeline = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyPipelineLayout(VkPipelineLayout* pipelineLayout)
{
    if (*pipelineLayout != VK_NULL_HANDLE)
    {
        vkDestroyPipelineLayout(renderer.Device, *pipelineLayout, NULL);
        *pipelineLayout = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyPipelineCache(VkPipelineCache* pipelineCache)
{
    if (*pipelineCache != VK_NULL_HANDLE)
    {
        vkDestroyPipelineCache(renderer.Device, *pipelineCache, NULL);
        *pipelineCache = VK_NULL_HANDLE;
    }
}

int Renderer_SimpleTestLIB()
{
    return 43;
}