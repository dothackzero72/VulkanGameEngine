#include "CVulkanRenderer.h"
#include "VulkanWindow.h"
#include "GLFWWindow.h"
#include "CTypedef.h"

RendererState cRenderer = { 0 };
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
        func(instance, cRenderer.DebugMessenger, pAllocator);
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

VkExtensionProperties* Renderer_GetDeviceExtensions(VkPhysicalDevice physicalDevice, uint32_t* deviceExtensionCountPtr)
{
    uint32_t deviceExtensionCount = 0;
    VkResult result = vkEnumerateDeviceExtensionProperties(physicalDevice, NULL, &deviceExtensionCount, NULL);
    if (result != VK_SUCCESS)
    {
        fprintf(stderr, "Failed to enumerate device extension properties. Error: %d\n", result);
        *deviceExtensionCountPtr = 0;
        return NULL;
    }

    VkExtensionProperties* deviceExtensions = malloc(sizeof(VkExtensionProperties) * deviceExtensionCount);
    if (!deviceExtensions) 
    {
        fprintf(stderr, "Failed to allocate memory for device extensions.\n");
        *deviceExtensionCountPtr = 0;
        return NULL;
    }

    result = vkEnumerateDeviceExtensionProperties(physicalDevice, NULL, &deviceExtensionCount, deviceExtensions);
    if (result != VK_SUCCESS)
    {
        fprintf(stderr, "Failed to enumerate device extension properties. Error: %d\n", result);
        free(deviceExtensions);
        *deviceExtensionCountPtr = 0;
        return NULL;
    }

    *deviceExtensionCountPtr = deviceExtensionCount;
    return deviceExtensions;
}

VkResult Renderer_GetSurfaceFormats(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkSurfaceFormatKHR** surfaceFormats, uint32_t* surfaceFormatCount)
{
    VkResult result = vkGetPhysicalDeviceSurfaceFormatsKHR(physicalDevice, surface, surfaceFormatCount, NULL);
    if (result != VK_SUCCESS) 
    {
        fprintf(stderr, "Failed to get the physical device surface formats count. Error: %d\n", result);
        return result;
    }

    if (*surfaceFormatCount > 0) 
    {
        *surfaceFormats = malloc(sizeof(VkSurfaceFormatKHR) * (*surfaceFormatCount));
        if (!(*surfaceFormats)) 
        {
            fprintf(stderr, "Failed to allocate memory for surface formats.\n");
            return VK_ERROR_OUT_OF_HOST_MEMORY;
        }

        result = vkGetPhysicalDeviceSurfaceFormatsKHR(physicalDevice, surface, surfaceFormatCount, *surfaceFormats);
        if (result != VK_SUCCESS) 
        {
            free(*surfaceFormats);
            *surfaceFormats = NULL;
            fprintf(stderr, "Failed to get physical device surface formats. Error: %d\n", result);
            return result;
        }
    }
    else 
    {
        *surfaceFormats = NULL;
    }
    return VK_SUCCESS;
}

VkResult Renderer_GetSurfacePresentModes(VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkPresentModeKHR** presentModes, uint32_t* presentModeCount)
{
    VkResult result = vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice, surface, presentModeCount, NULL);
    if (result != VK_SUCCESS) 
    {
        fprintf(stderr, "Failed to get the physical device surface present modes count. Error: %d\n", result);
        return result;
    }

    if (*presentModeCount > 0) 
    {
        *presentModes = malloc(sizeof(VkPresentModeKHR) * (*presentModeCount));
        if (!(*presentModes)) 
        {
            fprintf(stderr, "Failed to allocate memory for present modes.\n");
            return VK_ERROR_OUT_OF_HOST_MEMORY;
        }

        result = vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice, surface, presentModeCount, *presentModes);
        if (result != VK_SUCCESS) 
        {
            free(*presentModes);
            *presentModes = NULL;
            fprintf(stderr, "Failed to get physical device surface present modes. Error: %d\n", result);
            return result;
        }
    }
    else
    {
        *presentModes = NULL;
    }
    return VK_SUCCESS;
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

 VkResult Renderer_GetWin32Extensions(uint32_t* extensionCount, char*** enabledExtensions)
 {
     if (extensionCount == NULL || enabledExtensions == NULL) 
     {
         fprintf(stderr, "Invalid arguments: extensionCount and enabledExtensions cannot be NULL.\n");
         return VK_ERROR_INITIALIZATION_FAILED;
     }

     VkResult result = vkEnumerateInstanceExtensionProperties(NULL, extensionCount, NULL);
     if (result != VK_SUCCESS) 
     {
         fprintf(stderr, "Failed to enumerate instance extension properties. Error: %d\n", result);
         *extensionCount = 0;
         return result;
     }

     VkExtensionProperties* extensions = malloc(sizeof(VkExtensionProperties) * (*extensionCount));
     if (!extensions) 
     {
         fprintf(stderr, "Failed to allocate memory for extension properties.\n");
         return VK_ERROR_OUT_OF_HOST_MEMORY;
     }

     result = vkEnumerateInstanceExtensionProperties(NULL, extensionCount, extensions);
     if (result != VK_SUCCESS) 
     {
         fprintf(stderr, "Failed to enumerate instance extension properties after allocation. Error: %d\n", result);
         free(extensions);
         return result;
     }

     char** extensionNames = malloc(sizeof(char*) * (*extensionCount));
     if (!extensionNames) 
     {
         fprintf(stderr, "Failed to allocate memory for extension names.\n");
         free(extensions);
         return VK_ERROR_OUT_OF_HOST_MEMORY;
     }

     for (uint32_t x = 0; x < *extensionCount; x++) 
     {
         extensionNames[x] = malloc(256);
         if (!extensionNames[x]) 
         {
             fprintf(stderr, "Failed to allocate memory for extension name at index %d.\n", x);
             for (uint32_t y = 0; y < x; y++) {
                 free(extensionNames[y]);
             }
             free(extensionNames);
             free(extensions);
             return VK_ERROR_OUT_OF_HOST_MEMORY;
         }
         strncpy(extensionNames[x], extensions[x].extensionName, 256);
         extensionNames[x][256 - 1] = '\0'; 
         printf("Extension: %s, Spec Version: %d\n", extensionNames[x], extensions[x].specVersion);
     }

     free(extensions);
     *enabledExtensions = extensionNames;

     return VK_SUCCESS;
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
    const char** extensions = NULL;
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

 VkResult Renderer_SetUpPhysicalDevice(VkInstance instance, VkPhysicalDevice* physicalDevice, VkSurfaceKHR surface, VkPhysicalDeviceFeatures* physicalDeviceFeatures, uint32_t* graphicsFamily, uint32_t* presentFamily) 
 {
     uint32_t deviceCount = 0;
     VkResult result = vkEnumeratePhysicalDevices(instance, &deviceCount, NULL);
     if (result != VK_SUCCESS) 
     {
         fprintf(stderr, "Failed to enumerate physical devices. Error: %d\n", result);
         return result;
     }
     if (deviceCount == 0) {
         fprintf(stderr, "No physical devices found.\n");
         return VK_ERROR_INITIALIZATION_FAILED;
     }

     VkPhysicalDevice* physicalDeviceList = malloc(sizeof(VkPhysicalDevice) * deviceCount);
     if (!physicalDeviceList) 
     {
         fprintf(stderr, "Failed to allocate memory for physical devices.\n");
         return VK_ERROR_OUT_OF_HOST_MEMORY;
     }

     result = vkEnumeratePhysicalDevices(instance, &deviceCount, physicalDeviceList);
     if (result != VK_SUCCESS) 
     {
         fprintf(stderr, "Failed to enumerate physical devices after allocation. Error: %d\n", result);
         free(physicalDeviceList);
         return result;
     }

     VkSurfaceFormatKHR surfaceFormat; 
     uint32_t surfaceFormatCount = 0;
     VkPresentModeKHR* presentMode = NULL;
     uint32_t presentModeCount = 0;

     for (uint32_t x = 0; x < deviceCount; x++) 
     {
         vkGetPhysicalDeviceFeatures(physicalDeviceList[x], physicalDeviceFeatures);
         result = SwapChain_GetQueueFamilies(physicalDeviceList[x], surface, graphicsFamily, presentFamily);
         if (result != VK_SUCCESS) 
         {
             fprintf(stderr, "Failed to get queue families. Error: %d\n", result);
             continue;
         }

         result = Renderer_GetSurfaceFormats(physicalDeviceList[x], surface, &surfaceFormat, &surfaceFormatCount);
         if (result != VK_SUCCESS) 
         {
             fprintf(stderr, "Failed to get surface formats. Error: %d\n", result);
             continue;
         }

         result = Renderer_GetSurfacePresentModes(physicalDeviceList[x], surface, &presentMode, &presentModeCount);
         if (result != VK_SUCCESS) 
         {
             fprintf(stderr, "Failed to get surface present modes. Error: %d\n", result);
             continue; 
         }

         if (*graphicsFamily != UINT32_MAX &&
             *presentFamily != UINT32_MAX && 
             surfaceFormatCount > 0 &&
             presentModeCount > 0 &&
             physicalDeviceFeatures->samplerAnisotropy) 
         {
             *physicalDevice = physicalDeviceList[x];
             free(presentMode);
             free(physicalDeviceList);
             return VK_SUCCESS;
         }
     }

     free(presentMode);
     free(physicalDeviceList); 
     return VK_ERROR_DEVICE_LOST;
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


VkResult Renderer_CreateCommandBuffers(VkDevice device, VkCommandPool commandPool, VkCommandBuffer* commandBufferList, uint32 commandBufferCount)
{
    for (size_t x = 0; x < commandBufferCount; x++)
    {
        VkCommandBufferAllocateInfo commandBufferAllocateInfo =
        {
            .sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO,
            .commandPool = commandPool,
            .level = VK_COMMAND_BUFFER_LEVEL_PRIMARY,
            .commandBufferCount = 1
        };

        VULKAN_RESULT(vkAllocateCommandBuffers(device, &commandBufferAllocateInfo, &commandBufferList[x]));
    }
    return VK_SUCCESS;
}

VkResult Renderer_CreateFrameBuffer(VkDevice device, VkFramebuffer* pFrameBuffer, VkFramebufferCreateInfo* frameBufferCreateInfo)
{
    return vkCreateFramebuffer(device, *&frameBufferCreateInfo, NULL, pFrameBuffer);
}

VkResult Renderer_CreateRenderPass(VkDevice device, RenderPassCreateInfoStruct* renderPassCreateInfo)
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
    return vkCreateRenderPass(device, &renderPassInfo, NULL, renderPassCreateInfo->pRenderPass);
}

VkResult Renderer_CreateDescriptorPool(VkDevice device, VkDescriptorPool* descriptorPool, VkDescriptorPoolCreateInfo* descriptorPoolCreateInfo)
{
    return vkCreateDescriptorPool(device, descriptorPoolCreateInfo, NULL, descriptorPool);
}

VkResult Renderer_CreateDescriptorSetLayout(VkDevice device, VkDescriptorSetLayout* descriptorSetLayout, VkDescriptorSetLayoutCreateInfo* descriptorSetLayoutCreateInfo)
{
    return vkCreateDescriptorSetLayout(device, descriptorSetLayoutCreateInfo, NULL, descriptorSetLayout);
}

VkResult Renderer_CreatePipelineLayout(VkDevice device, VkPipelineLayout* pipelineLayout, VkPipelineLayoutCreateInfo* pipelineLayoutCreateInfo)
{
    return vkCreatePipelineLayout(device, pipelineLayoutCreateInfo, NULL, pipelineLayout);
}

VkResult Renderer_AllocateDescriptorSets(VkDevice device, VkDescriptorSet* descriptorSet, VkDescriptorSetAllocateInfo* descriptorSetAllocateInfo)
{
    return vkAllocateDescriptorSets(device, descriptorSetAllocateInfo, descriptorSet);
}

VkResult Renderer_AllocateCommandBuffers(VkDevice device, VkCommandBuffer* commandBuffer, VkCommandBufferAllocateInfo* ImGuiCommandBuffers)
{
    return vkAllocateCommandBuffers(device, ImGuiCommandBuffers, commandBuffer);
}

VkResult Renderer_CreateGraphicsPipelines(VkDevice device, VkPipeline* graphicPipeline, VkGraphicsPipelineCreateInfo* createGraphicPipelines, uint32 createGraphicPipelinesCount)
{
    return vkCreateGraphicsPipelines(device, VK_NULL_HANDLE, createGraphicPipelinesCount, createGraphicPipelines, NULL, graphicPipeline);
}

VkResult Renderer_CreateCommandPool(VkDevice device, VkCommandPool* commandPool, VkCommandPoolCreateInfo* commandPoolInfo)
{
    return vkCreateCommandPool(device, commandPoolInfo, NULL, commandPool);
}

void Renderer_UpdateDescriptorSet(VkDevice device, VkWriteDescriptorSet* writeDescriptorSet, uint32 count)
{
    vkUpdateDescriptorSets(device, count, writeDescriptorSet, 0, NULL);
}

VkResult Renderer_StartFrame(VkDevice device, VkSwapchainKHR swapChain, VkFence* fenceList, VkSemaphore* acquireImageSemaphoreList, uint32_t* pImageIndex, uint32_t* pCommandIndex, bool* pRebuildRendererFlag)
{
    if (!pImageIndex || 
        !pCommandIndex || 
        !pRebuildRendererFlag)
    {
        fprintf(stderr, "Error: Null pointer passed to Renderer_StartFrame.\n");
        return VK_ERROR_INITIALIZATION_FAILED; 
    }

    *pCommandIndex = (*pCommandIndex + 1) % MAX_FRAMES_IN_FLIGHT;

    VkResult waitResult = vkWaitForFences(device, 1, &fenceList[*pCommandIndex], VK_TRUE, UINT64_MAX);
    if (waitResult != VK_SUCCESS) 
    {
        fprintf(stderr, "Error: vkWaitForFences failed with error code: %s\n", Renderer_GetError(waitResult));
        return waitResult;
    }

    VkResult resetResult = vkResetFences(device, 1, &fenceList[*pCommandIndex]);
    if (resetResult != VK_SUCCESS) 
    {
        fprintf(stderr, "Error: vkResetFences failed with error code: %s\n", Renderer_GetError(resetResult));
        return resetResult;
    }

    VkResult result = vkAcquireNextImageKHR(device, swapChain, UINT64_MAX, acquireImageSemaphoreList[*pCommandIndex], VK_NULL_HANDLE, pImageIndex);
    if (result == VK_ERROR_OUT_OF_DATE_KHR) 
    {
        *pRebuildRendererFlag = true;
        return result;
    }
    else if (result != VK_SUCCESS) 
    {
        fprintf(stderr, "Error: vkAcquireNextImageKHR failed with error code: %s\n", Renderer_GetError(result));
        return result;
    }

    return result;
}

VkResult Renderer_EndFrame(VkSwapchainKHR swapChain, VkSemaphore* acquireImageSemaphoreList, VkSemaphore* presentImageSemaphoreList, VkFence* fenceList, VkQueue graphicsQueue, VkQueue presentQueue, uint32_t commandIndex, uint32_t imageIndex, VkCommandBuffer* pCommandBufferSubmitList, uint32_t commandBufferCount, bool* rebuildRendererFlag)
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

    VkResult submitResult = vkQueueSubmit(graphicsQueue, 1, &submitInfo, fenceList[commandIndex]);
    if (submitResult != VK_SUCCESS) 
    {
        fprintf(stderr, "Error: vkQueueSubmit failed with error code: %s\n", Renderer_GetError(submitResult));
        return submitResult; 
    }

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
    if (result == VK_ERROR_OUT_OF_DATE_KHR || 
        result == VK_SUBOPTIMAL_KHR) 
    {
        *rebuildRendererFlag = true;
    }
    else if (result != VK_SUCCESS) {
        fprintf(stderr, "Error: vkQueuePresentKHR failed with error code: %s\n", Renderer_GetError(result));
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

void Renderer_DestroyFences(VkDevice device, VkSemaphore* acquireImageSemaphores, VkSemaphore* presentImageSemaphores, VkFence* fences, size_t semaphoreCount)
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
 
void Renderer_DestroyCommandPool(VkDevice device, VkCommandPool* commandPool)
{
    if (*commandPool != VK_NULL_HANDLE)
    {
        vkDestroyCommandPool(device, *commandPool, NULL);
        *commandPool = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyDevice(VkDevice device)
{
    if (device != VK_NULL_HANDLE)
    {
        vkDestroyDevice(device, NULL);
        device = VK_NULL_HANDLE;
    }
}

void Renderer_DestroySurface(VkInstance instance, VkSurfaceKHR* surface)
{
    if (*surface != VK_NULL_HANDLE)
    {
        vkDestroySurfaceKHR(instance, *surface, NULL);
        *surface = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyDebugger(VkInstance* instance)
{
    DestroyDebugUtilsMessengerEXT(*instance, NULL);
}

void Renderer_DestroyInstance(VkInstance* instance)
{
    if (*instance != VK_NULL_HANDLE)
    {
        vkDestroyInstance(*instance, NULL);
        *instance = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyRenderPass(VkDevice device, VkRenderPass* renderPass)
{
    if (*renderPass != VK_NULL_HANDLE)
    {
        vkDestroyRenderPass(device, *renderPass, NULL);
        *renderPass = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyFrameBuffers(VkDevice device, VkFramebuffer* frameBufferList, uint32 count)
{
    for (size_t x = 0; x < count; x++)
    {
        if (frameBufferList[x] != VK_NULL_HANDLE)
        {
            vkDestroyFramebuffer(device, frameBufferList[x], NULL);
            frameBufferList[x] = VK_NULL_HANDLE;
        }
    }
}

void Renderer_DestroyDescriptorPool(VkDevice device, VkDescriptorPool* descriptorPool)
{
    if (*descriptorPool != VK_NULL_HANDLE)
    {
        vkDestroyDescriptorPool(device, *descriptorPool, NULL);
        *descriptorPool = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyDescriptorSetLayout(VkDevice device, VkDescriptorSetLayout* descriptorSetLayout)
{
    if (*descriptorSetLayout != VK_NULL_HANDLE)
    {
        vkDestroyDescriptorSetLayout(device, *descriptorSetLayout, NULL);
        *descriptorSetLayout = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyCommandBuffers(VkDevice device, VkCommandPool* commandPool, VkCommandBuffer* commandBufferList, uint32 count)
{
    if (*commandBufferList != VK_NULL_HANDLE)
    {
        vkFreeCommandBuffers(device, *commandPool, count, &*commandBufferList);
        for (size_t x = 0; x < count; x++)
        {
            commandBufferList[x] = VK_NULL_HANDLE;
        }
    }
}

void Renderer_DestroyBuffer(VkDevice device, VkBuffer* buffer)
{
    if (*buffer != VK_NULL_HANDLE)
    {
        vkDestroyBuffer(device, *buffer, NULL);
        *buffer = VK_NULL_HANDLE;
    }
}

void Renderer_FreeDeviceMemory(VkDevice device, VkDeviceMemory* memory)
{
    if (*memory != VK_NULL_HANDLE)
    {
        vkFreeMemory(device, *memory, NULL);
        *memory = VK_NULL_HANDLE;
    }
}

void Renderer_DestroySwapChainImageView(VkDevice device, VkImageView* pSwapChainImageViewList, uint32 count)
{
    for (uint32 x = 0; x < count; x++)
    {
        if (cRenderer.Surface != VK_NULL_HANDLE)
        {
            vkDestroyImageView(device, pSwapChainImageViewList[x], NULL);
            pSwapChainImageViewList[x] = VK_NULL_HANDLE;
        }
    }
}

void Renderer_DestroySwapChain(VkDevice device, VkSwapchainKHR* swapChain)
{
    vkDestroySwapchainKHR(device, *swapChain, NULL);
    *swapChain = VK_NULL_HANDLE;
}

void Renderer_DestroyImageView(VkDevice device, VkImageView* imageView)
{
    if (*imageView != VK_NULL_HANDLE)
    {
        vkDestroyImageView(device, *imageView, NULL);
        *imageView = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyImage(VkDevice device, VkImage* image)
{
    if (*image != VK_NULL_HANDLE)
    {
        vkDestroyImage(device, *image, NULL);
        *image = VK_NULL_HANDLE;
    }
}

void Renderer_DestroySampler(VkDevice device, VkSampler* sampler)
{
    if (*sampler != VK_NULL_HANDLE)
    {
        vkDestroySampler(device, *sampler, NULL);
        *sampler = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyPipeline(VkDevice device, VkPipeline* pipeline)
{
    if (*pipeline != VK_NULL_HANDLE)
    {
        vkDestroyPipeline(device, *pipeline, NULL);
        *pipeline = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyPipelineLayout(VkDevice device, VkPipelineLayout* pipelineLayout)
{
    if (*pipelineLayout != VK_NULL_HANDLE)
    {
        vkDestroyPipelineLayout(device, *pipelineLayout, NULL);
        *pipelineLayout = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyPipelineCache(VkDevice device, VkPipelineCache* pipelineCache)
{
    if (*pipelineCache != VK_NULL_HANDLE)
    {
        vkDestroyPipelineCache(device, *pipelineCache, NULL);
        *pipelineCache = VK_NULL_HANDLE;
    }
}

int Renderer_SimpleTestLIB()
{
    return 43;
}