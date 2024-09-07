#include "VulkanRenderer.h"
#include "VulkanWindow.h"

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

//PFN_vkGetBufferDeviceAddressKHR vkGetBufferDeviceAddressKHR;
//PFN_vkCreateAccelerationStructureKHR vkCreateAccelerationStructureKHR;
//PFN_vkDestroyAccelerationStructureKHR vkDestroyAccelerationStructureKHR;
//PFN_vkGetAccelerationStructureBuildSizesKHR vkGetAccelerationStructureBuildSizesKHR;
//PFN_vkGetAccelerationStructureDeviceAddressKHR vkGetAccelerationStructureDeviceAddressKHR;
//PFN_vkCmdBuildAccelerationStructuresKHR vkCmdBuildAccelerationStructuresKHR;
//PFN_vkBuildAccelerationStructuresKHR vkBuildAccelerationStructuresKHR;
//PFN_vkCmdTraceRaysKHR vkCmdTraceRaysKHR;
//PFN_vkGetRayTracingShaderGroupHandlesKHR vkGetRayTracingShaderGroupHandlesKHR;
//PFN_vkCreateRayTracingPipelinesKHR vkCreateRayTracingPipelinesKHR;

RendererState Renderer = { 0 };

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

static VkResult CreateDebugUtilsMessengerEXT(VkInstance instance, const VkDebugUtilsMessengerCreateInfoEXT* pCreateInfo, const VkAllocationCallbacks* pAllocator, VkDebugUtilsMessengerEXT* pDebugMessenger)
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

static void DestroyDebugUtilsMessengerEXT(VkInstance instance, const VkAllocationCallbacks* pAllocator)
{
    PFN_vkDestroyDebugUtilsMessengerEXT func = (PFN_vkDestroyDebugUtilsMessengerEXT)vkGetInstanceProcAddr(instance, "vkDestroyDebugUtilsMessengerEXT");
    if (func != NULL)
    {
        func(instance, Renderer.DebugMessenger, pAllocator);
    }
    else
    {
        fprintf(stderr, "Failed to load vkDestroyDebugUtilsMessengerEXT function\n");
    }
}

VKAPI_ATTR VkBool32 VKAPI_CALL Vulkan_DebugCallBack(VkDebugUtilsMessageSeverityFlagBitsEXT MessageSeverity, VkDebugUtilsMessageTypeFlagsEXT MessageType, const VkDebugUtilsMessengerCallbackDataEXT* CallBackData, void* UserData)
{
    fprintf(stderr, "Validation Layer: %s\n", CallBackData->pMessage);
    return VK_FALSE;
}

static VkExtensionProperties* GetDeviceExtensions(VkPhysicalDevice physicalDevice, uint32* deviceExtensionCountPtr)
{
    uint32 deviceExtensionCount = 0;
    VULKAN_RESULT(vkEnumerateDeviceExtensionProperties(physicalDevice, NULL, &deviceExtensionCount, NULL));

    VkExtensionProperties* deviceExtensions = malloc(sizeof(VkExtensionProperties) * deviceExtensionCount);
    if (!deviceExtensions)
    {
        fprintf(stderr, "Failed to allocate memory for Vulkan.\n");
        *deviceExtensionCountPtr = 0;
        return NULL;
    }

    VULKAN_RESULT(vkEnumerateDeviceExtensionProperties(physicalDevice, NULL, &deviceExtensionCount, deviceExtensions));
    *deviceExtensionCountPtr = deviceExtensionCount;
    return deviceExtensions;
}

static void GetSurfaceFormats(VkPhysicalDevice physicalDevice, VkSurfaceFormatKHR* surfaceFormat, uint32* surfaceFormatCount)
{
    VULKAN_RESULT(vkGetPhysicalDeviceSurfaceFormatsKHR(physicalDevice, Renderer.Surface, surfaceFormatCount, NULL));
    if (surfaceFormatCount > 0)
    {
        VkSurfaceFormatKHR* surfaceFormats = malloc(sizeof(VkSurfaceFormatKHR) * *surfaceFormatCount);
        if (!surfaceFormats)
        {
            fprintf(stderr, "Failed to allocate memory for Vulkan.\n");
        }

        VULKAN_RESULT(vkGetPhysicalDeviceSurfaceFormatsKHR(physicalDevice, Renderer.Surface, surfaceFormatCount, surfaceFormats));
    }
}

static void GetPresentModes(VkPhysicalDevice physicalDevice, VkPresentModeKHR* presentMode, int32* presentModeCount)
{
    VULKAN_RESULT(vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice, Renderer.Surface, presentModeCount, NULL));
    if (presentModeCount > 0)
    {
        VkPresentModeKHR* presentModes = malloc(sizeof(VkPresentModeKHR) * *presentModeCount);
        if (!presentModes)
        {
            fprintf(stderr, "Failed to allocate memory for Vulkan.\n");
        }

        VULKAN_RESULT(vkGetPhysicalDeviceSurfacePresentModesKHR(physicalDevice, Renderer.Surface, presentModeCount, presentModes));
    }
}

static bool GetRayTracingSupport()
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

static void GetRendererFeatures(VkPhysicalDeviceVulkan11Features* physicalDeviceVulkan11Features)
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

    if (GetRayTracingSupport())
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

VkInstance Renderer_CreateVulkanInstance(VkInstanceCreateInfo instanceInfo)
{
    VkInstance Instance = VK_NULL_HANDLE;
    vkCreateInstance(&instanceInfo, NULL, &Instance);
    return Instance;
}

void Renderer_Windows_Renderer(uint32* pExtensionCount, VkExtensionProperties** extensionProperties)
{
    vkEnumerateInstanceExtensionProperties(NULL, &pExtensionCount, NULL);
    VkExtensionProperties* instanceExtensions = (VkExtensionProperties*)malloc(sizeof(VkExtensionProperties) * (*pExtensionCount));
    vkEnumerateInstanceExtensionProperties(NULL, &pExtensionCount, instanceExtensions);
    (*pExtensionCount)++;
    *extensionProperties = (VkExtensionProperties*)extensionProperties;
}

VkResult Renderer_RendererSetUp()
{
    Renderer.RebuildRendererFlag = false;
    uint32 pExtensionCount = 0;
    VkExtensionProperties* extensions = NULL;


    vulkanWindow->GetInstanceExtensions(vulkanWindow, &pExtensionCount, &extensions);
    VkDebugUtilsMessengerCreateInfoEXT debugInfo =
    {
        .sType = VK_STRUCTURE_TYPE_DEBUG_UTILS_MESSENGER_CREATE_INFO_EXT,
        .messageSeverity = VK_DEBUG_UTILS_MESSAGE_SEVERITY_INFO_BIT_EXT | VK_DEBUG_UTILS_MESSAGE_SEVERITY_VERBOSE_BIT_EXT | VK_DEBUG_UTILS_MESSAGE_SEVERITY_WARNING_BIT_EXT | VK_DEBUG_UTILS_MESSAGE_SEVERITY_ERROR_BIT_EXT,
        .messageType = VK_DEBUG_UTILS_MESSAGE_TYPE_GENERAL_BIT_EXT | VK_DEBUG_UTILS_MESSAGE_TYPE_VALIDATION_BIT_EXT | VK_DEBUG_UTILS_MESSAGE_TYPE_PERFORMANCE_BIT_EXT,
        .pfnUserCallback = Vulkan_DebugCallBack
    };

    VkApplicationInfo applicationInfo =
    {
        .sType = VK_STRUCTURE_TYPE_APPLICATION_INFO,
        .pApplicationName = "Vulkan Application",
        .applicationVersion = VK_MAKE_VERSION(1, 0, 0),
        .pEngineName = "No Engine",
        .engineVersion = VK_MAKE_VERSION(1, 0, 0),
        .apiVersion = VK_API_VERSION_1_3
    };

    VkInstanceCreateInfo vulkanCreateInfo =
    {
        .sType = VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO,
        .pApplicationInfo = &applicationInfo,
        .enabledExtensionCount = pExtensionCount,
        .ppEnabledExtensionNames = extensions
    };
#ifdef NDEBUG
    vulkanCreateInfo.enabledLayerCount = 0;
#else
    vulkanCreateInfo.enabledLayerCount = ARRAY_SIZE(ValidationLayers);
    vulkanCreateInfo.ppEnabledLayerNames = ValidationLayers;
    vulkanCreateInfo.pNext = &debugInfo;
#endif

    VULKAN_RESULT(vkCreateInstance(&vulkanCreateInfo, NULL, &Renderer.Instance));

#ifdef NDEBUG
#else
    VULKAN_RESULT(CreateDebugUtilsMessengerEXT(Renderer.Instance, &debugInfo, NULL, &Renderer.DebugMessenger));
#endif

    vulkanWindow->CreateSurface(vulkanWindow, &Renderer.Instance, &Renderer.Surface);

    uint32 deviceCount = UINT32_MAX;
    VULKAN_RESULT(vkEnumeratePhysicalDevices(Renderer.Instance, &deviceCount, NULL));

    VkPhysicalDevice* physicalDeviceList = malloc(sizeof(VkPhysicalDevice) * deviceCount);
    VULKAN_RESULT(vkEnumeratePhysicalDevices(Renderer.Instance, &deviceCount, physicalDeviceList));

    VkPresentModeKHR* presentMode = NULL;
    for (uint32 x = 0; x < deviceCount; x++)
    {
        vkGetPhysicalDeviceFeatures(physicalDeviceList[x], &Renderer.PhysicalDeviceFeatures);
        SwapChain_GetQueueFamilies(physicalDeviceList[x], &Renderer.SwapChain.GraphicsFamily, &Renderer.SwapChain.PresentFamily);

        VkSurfaceFormatKHR surfaceFormat;
        VkPresentModeKHR presentMode;
        uint32 surfaceFormatCount = 0;
        uint32 presentModeCount = 0;
        GetSurfaceFormats(physicalDeviceList[x], &surfaceFormat, &surfaceFormatCount);
        GetPresentModes(physicalDeviceList[x], &presentMode, &presentModeCount);

        if (Renderer.SwapChain.GraphicsFamily != -1 &&
            Renderer.SwapChain.PresentFamily != -1 &&
            surfaceFormatCount > 0 &&
            presentModeCount != 0 &&
            Renderer.PhysicalDeviceFeatures.samplerAnisotropy)
        {
            Renderer.PhysicalDevice = physicalDeviceList[x];
            break;
        }
        else
        {
            Renderer_DestroyRenderer();
            SDL_Quit();
        }
    }

    //VkPhysicalDeviceProperties deviceProperties;
    //vkGetPhysicalDeviceProperties(physicalDeviceList[0], &deviceProperties);

    //VkPhysicalDeviceVulkan11Features physicalDeviceVulkan11Features;
    //GetRendererFeatures(&physicalDeviceVulkan11Features);

    float queuePriority = 1.0f;
    VkDeviceQueueCreateInfo queueCreateInfo[2];
    uint32 queueCreateInfoCount = 0;
    if (Renderer.SwapChain.GraphicsFamily != UINT32_MAX)
    {
        queueCreateInfo[queueCreateInfoCount++] = (VkDeviceQueueCreateInfo){
            .sType = VK_STRUCTURE_TYPE_DEVICE_QUEUE_CREATE_INFO,
            .queueFamilyIndex = Renderer.SwapChain.GraphicsFamily,
            .queueCount = 1,
            .pQueuePriorities = &queuePriority
        };
    }

    if (Renderer.SwapChain.PresentFamily != UINT32_MAX &&
        Renderer.SwapChain.PresentFamily != Renderer.SwapChain.GraphicsFamily)
    {
        queueCreateInfo[queueCreateInfoCount++] = (VkDeviceQueueCreateInfo)
        {
            .sType = VK_STRUCTURE_TYPE_DEVICE_QUEUE_CREATE_INFO,
            .queueFamilyIndex = Renderer.SwapChain.PresentFamily,
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

    if (GetRayTracingSupport())
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

    VULKAN_RESULT(vkCreateDevice(Renderer.PhysicalDevice, &deviceCreateInfo, NULL, &Renderer.Device));
    Vulkan_SetUpSwapChain();

    VkCommandPoolCreateInfo CommandPoolCreateInfo =
    {
        .sType = VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO,
        .flags = VK_COMMAND_POOL_CREATE_RESET_COMMAND_BUFFER_BIT,
        .queueFamilyIndex = Renderer.SwapChain.GraphicsFamily
    };

    VULKAN_RESULT(vkCreateCommandPool(Renderer.Device, &CommandPoolCreateInfo, NULL, &Renderer.CommandPool));

    Renderer.InFlightFences = malloc(sizeof(VkFence) * MAX_FRAMES_IN_FLIGHT);
    VkSemaphoreTypeCreateInfo semaphoreTypeCreateInfo =
    {
        .sType = VK_STRUCTURE_TYPE_SEMAPHORE_TYPE_CREATE_INFO,
        .semaphoreType = VK_SEMAPHORE_TYPE_BINARY,
        .initialValue = 0,
        .pNext = NULL
    };

    Renderer.AcquireImageSemaphores = malloc(sizeof(VkSemaphore) * MAX_FRAMES_IN_FLIGHT);
    VkSemaphoreCreateInfo semaphoreCreateInfo =
    {
        .sType = VK_STRUCTURE_TYPE_SEMAPHORE_CREATE_INFO,
        .pNext = &semaphoreTypeCreateInfo
    };

    Renderer.PresentImageSemaphores = malloc(sizeof(VkSemaphore) * MAX_FRAMES_IN_FLIGHT);
    VkFenceCreateInfo fenceInfo =
    {
        .sType = VK_STRUCTURE_TYPE_FENCE_CREATE_INFO,
        .flags = VK_FENCE_CREATE_SIGNALED_BIT
    };

    if (Renderer.InFlightFences &&
        Renderer.AcquireImageSemaphores &&
        Renderer.PresentImageSemaphores)
    {
        for (size_t x = 0; x < MAX_FRAMES_IN_FLIGHT; x++)
        {
            VULKAN_RESULT(vkCreateSemaphore(Renderer.Device, &semaphoreCreateInfo, NULL, &Renderer.AcquireImageSemaphores[x]));
            VULKAN_RESULT(vkCreateSemaphore(Renderer.Device, &semaphoreCreateInfo, NULL, &Renderer.PresentImageSemaphores[x]));
            VULKAN_RESULT(vkCreateFence(Renderer.Device, &fenceInfo, NULL, &Renderer.InFlightFences[x]));
        }
    }
    else
    {
        Renderer_DestroyRenderer();
        SDL_Quit();
    }

    vkGetDeviceQueue(Renderer.Device, Renderer.SwapChain.GraphicsFamily, 0, &Renderer.SwapChain.GraphicsQueue);
    vkGetDeviceQueue(Renderer.Device, Renderer.SwapChain.PresentFamily, 0, &Renderer.SwapChain.PresentQueue);
    free(extensions);

    //PFN_vkGetBufferDeviceAddressKHR vkGetBufferDeviceAddressKHR = (PFN_vkGetBufferDeviceAddressKHR)(vkGetDeviceProcAddr(Renderer.Device, "vkGetBufferDeviceAddressKHR"));
    //PFN_vkCreateAccelerationStructureKHR vkCreateAccelerationStructureKHR = (PFN_vkCreateAccelerationStructureKHR)(vkGetDeviceProcAddr(Renderer.Device, "vkCreateAccelerationStructureKHR"));
    //PFN_vkDestroyAccelerationStructureKHR vkDestroyAccelerationStructureKHR = (PFN_vkDestroyAccelerationStructureKHR)(vkGetDeviceProcAddr(Renderer.Device, "vkDestroyAccelerationStructureKHR"));
    //PFN_vkGetAccelerationStructureBuildSizesKHR vkGetAccelerationStructureBuildSizesKHR = (PFN_vkGetAccelerationStructureBuildSizesKHR)(vkGetDeviceProcAddr(Renderer.Device, "vkGetAccelerationStructureBuildSizesKHR"));
    //PFN_vkGetAccelerationStructureDeviceAddressKHR vkGetAccelerationStructureDeviceAddressKHR = (PFN_vkGetAccelerationStructureDeviceAddressKHR)(vkGetDeviceProcAddr(Renderer.Device, "vkGetAccelerationStructureDeviceAddressKHR"));
    //PFN_vkCmdBuildAccelerationStructuresKHR vkCmdBuildAccelerationStructuresKHR = (PFN_vkCmdBuildAccelerationStructuresKHR)(vkGetDeviceProcAddr(Renderer.Device, "vkCmdBuildAccelerationStructuresKHR"));
    //PFN_vkBuildAccelerationStructuresKHR vkBuildAccelerationStructuresKHR = (PFN_vkBuildAccelerationStructuresKHR)(vkGetDeviceProcAddr(Renderer.Device, "vkBuildAccelerationStructuresKHR"));
    //PFN_vkCmdTraceRaysKHR vkCmdTraceRaysKHR = (PFN_vkCmdTraceRaysKHR)(vkGetDeviceProcAddr(Renderer.Device, "vkCmdTraceRaysKHR"));
    //PFN_vkGetRayTracingShaderGroupHandlesKHR vkGetRayTracingShaderGroupHandlesKHR = (PFN_vkGetRayTracingShaderGroupHandlesKHR)(vkGetDeviceProcAddr(Renderer.Device, "vkGetRayTracingShaderGroupHandlesKHR"));
    //PFN_vkCreateRayTracingPipelinesKHR vkCreateRayTracingPipelinesKHR = (PFN_vkCreateRayTracingPipelinesKHR)(vkGetDeviceProcAddr(Renderer.Device, "vkCreateRayTracingPipelinesKHR"));

    return VK_SUCCESS;
}

VkResult Renderer_CreateCommandBuffers(VkCommandBuffer* commandBufferList)
{
    for (size_t x = 0; x < Renderer.SwapChain.SwapChainImageCount; x++)
    {
        VkCommandBufferAllocateInfo commandBufferAllocateInfo =
        {
            .sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO,
            .commandPool = Renderer.CommandPool,
            .level = VK_COMMAND_BUFFER_LEVEL_PRIMARY,
            .commandBufferCount = 1
        };

        VULKAN_RESULT(vkAllocateCommandBuffers(Renderer.Device, &commandBufferAllocateInfo, &commandBufferList[x]));
    }
    return VK_SUCCESS;
}

VkResult Renderer_CreateFrameBuffer(VkFramebuffer* pFrameBuffer, VkFramebufferCreateInfo* frameBufferCreateInfo)
{
    return vkCreateFramebuffer(Renderer.Device, *&frameBufferCreateInfo, NULL, pFrameBuffer);
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
    return vkCreateRenderPass(Renderer.Device, &renderPassInfo, NULL, renderPassCreateInfo->pRenderPass);
}

VkResult Renderer_CreateDescriptorPool(VkDescriptorPool* descriptorPool, VkDescriptorPoolCreateInfo* descriptorPoolCreateInfo)
{
    return vkCreateDescriptorPool(Renderer.Device, descriptorPoolCreateInfo, NULL, descriptorPool);
}

VkResult Renderer_CreateDescriptorSetLayout(VkDescriptorSetLayout* descriptorSetLayout, VkDescriptorSetLayoutCreateInfo* descriptorSetLayoutCreateInfo)
{
    return vkCreateDescriptorSetLayout(Renderer.Device, descriptorSetLayoutCreateInfo, NULL, descriptorSetLayout);
}

VkResult Renderer_CreatePipelineLayout(VkPipelineLayout* pipelineLayout, VkPipelineLayoutCreateInfo* pipelineLayoutCreateInfo)
{
    return vkCreatePipelineLayout(Renderer.Device, pipelineLayoutCreateInfo, NULL, pipelineLayout);
}

VkResult Renderer_AllocateDescriptorSets(VkDescriptorSet* descriptorSet, VkDescriptorSetAllocateInfo* descriptorSetAllocateInfo)
{
    return vkAllocateDescriptorSets(Renderer.Device, descriptorSetAllocateInfo, descriptorSet);
}

VkResult Renderer_AllocateCommandBuffers(VkCommandBuffer* commandBuffer, VkCommandBufferAllocateInfo* ImGuiCommandBuffers)
{
    return vkAllocateCommandBuffers(Renderer.Device, ImGuiCommandBuffers, commandBuffer);
}

VkResult Renderer_CreateGraphicsPipelines(VkPipeline* graphicPipeline, VkGraphicsPipelineCreateInfo* createGraphicPipelines, uint32 createGraphicPipelinesCount)
{
    return vkCreateGraphicsPipelines(Renderer.Device, VK_NULL_HANDLE, createGraphicPipelinesCount, createGraphicPipelines, NULL, graphicPipeline);
}

VkResult Renderer_CreateCommandPool(VkCommandPool* commandPool, VkCommandPoolCreateInfo* commandPoolInfo)
{
    return vkCreateCommandPool(Renderer.Device, commandPoolInfo, NULL, commandPool);
}

void Renderer_UpdateDescriptorSet(VkWriteDescriptorSet* writeDescriptorSet, uint32 count)
{
    vkUpdateDescriptorSets(Renderer.Device, count, writeDescriptorSet, 0, NULL);
}

VkResult Renderer_RebuildSwapChain()
{
    Renderer.RebuildRendererFlag = true;

    VULKAN_RESULT(vkDeviceWaitIdle(Renderer.Device));
    Vulkan_DestroyImageView();
    vkDestroySwapchainKHR(Renderer.Device, Renderer.SwapChain.Swapchain, NULL);
    return Vulkan_RebuildSwapChain();
}

VkResult Renderer_StartFrame()
{
    Renderer.CommandIndex = (Renderer.CommandIndex + 1) % MAX_FRAMES_IN_FLIGHT;

    vkWaitForFences(Renderer.Device, 1, &Renderer.InFlightFences[Renderer.CommandIndex], VK_TRUE, UINT64_MAX);
    vkResetFences(Renderer.Device, 1, &Renderer.InFlightFences[Renderer.CommandIndex]);

    VkSemaphore imageAvailableSemaphore = Renderer.AcquireImageSemaphores[Renderer.CommandIndex];
    VkSemaphore renderFinishedSemaphore = Renderer.PresentImageSemaphores[Renderer.CommandIndex];

    VkResult result = vkAcquireNextImageKHR(Renderer.Device, Renderer.SwapChain.Swapchain, UINT64_MAX, imageAvailableSemaphore, VK_NULL_HANDLE, &Renderer.ImageIndex);
    if (result == VK_ERROR_OUT_OF_DATE_KHR)
    {
        Renderer.RebuildRendererFlag = true;
        return result;
    }

    return result;
}

VkResult Renderer_EndFrame(VkCommandBuffer* pCommandBufferSubmitList, uint32 commandBufferCount)
{
    VkPipelineStageFlags waitStages[] =
    {
        VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT
    };

    VkSubmitInfo submitInfo =
    {
        .sType = VK_STRUCTURE_TYPE_SUBMIT_INFO,
        .waitSemaphoreCount = 1,
        .pWaitSemaphores = &Renderer.AcquireImageSemaphores[Renderer.CommandIndex],
        .pWaitDstStageMask = waitStages,
        .commandBufferCount = commandBufferCount,
        .pCommandBuffers = pCommandBufferSubmitList,
        .signalSemaphoreCount = 1,
        .pSignalSemaphores = &Renderer.PresentImageSemaphores[Renderer.ImageIndex]
    };
    VULKAN_RESULT(vkQueueSubmit(Renderer.SwapChain.GraphicsQueue, 1, &submitInfo, Renderer.InFlightFences[Renderer.CommandIndex]));

    VkPresentInfoKHR presentInfo =
    {
        .sType = VK_STRUCTURE_TYPE_PRESENT_INFO_KHR,
        .waitSemaphoreCount = 1,
        .pWaitSemaphores = &Renderer.PresentImageSemaphores[Renderer.ImageIndex],
        .swapchainCount = 1,
        .pSwapchains = &Renderer.SwapChain.Swapchain,
        .pImageIndices = &Renderer.ImageIndex,
    };
    VkResult result = vkQueuePresentKHR(Renderer.SwapChain.PresentQueue, &presentInfo);
    if (result == VK_ERROR_OUT_OF_DATE_KHR || result == VK_SUBOPTIMAL_KHR)
    {
        Renderer.RebuildRendererFlag = true;
    }

    return result;
}

VkResult Renderer_BeginCommandBuffer(VkCommandBuffer* pCommandBuffer, VkCommandBufferBeginInfo* commandBufferBeginInfo)
{
    return vkBeginCommandBuffer(*pCommandBuffer, commandBufferBeginInfo);
}

VkResult Renderer_EndCommandBuffer(VkCommandBuffer* pCommandBuffer)
{
    return vkEndCommandBuffer(*pCommandBuffer);
}

VkResult Renderer_SubmitDraw(VkCommandBuffer* pCommandBufferSubmitList)
{
    VkPipelineStageFlags waitStages[] = { VK_PIPELINE_STAGE_COLOR_ATTACHMENT_OUTPUT_BIT };

    VkSubmitInfo SubmitInfo =
    {
        .sType = VK_STRUCTURE_TYPE_SUBMIT_INFO,
        .waitSemaphoreCount = 1,
        .pWaitSemaphores = &Renderer.AcquireImageSemaphores[Renderer.CommandIndex],
        .pWaitDstStageMask = waitStages,
        .commandBufferCount = 3,
        .pCommandBuffers = pCommandBufferSubmitList,
        .signalSemaphoreCount = 1,
        .pSignalSemaphores = &Renderer.PresentImageSemaphores[Renderer.ImageIndex],
    };
    VULKAN_RESULT(vkQueueSubmit(Renderer.SwapChain.GraphicsQueue, 1, &SubmitInfo, Renderer.InFlightFences[Renderer.CommandIndex]));

    VkPresentInfoKHR PresentInfoKHR =
    {
        .sType = VK_STRUCTURE_TYPE_PRESENT_INFO_KHR,
        .waitSemaphoreCount = 1,
        .pWaitSemaphores = &Renderer.PresentImageSemaphores[Renderer.ImageIndex],
        .swapchainCount = 1,
        .pSwapchains = &Renderer.SwapChain.Swapchain,
        .pImageIndices = &Renderer.ImageIndex
    };
    VkResult result = vkQueuePresentKHR(Renderer.SwapChain.PresentQueue, &PresentInfoKHR);
    if (result == VK_ERROR_OUT_OF_DATE_KHR)
    {
        Renderer.RebuildRendererFlag = true;
        return result;
    }
    return result;
}

uint32 Renderer_GetMemoryType(uint32 typeFilter, VkMemoryPropertyFlags properties)
{
    VkPhysicalDeviceMemoryProperties memProperties;
    vkGetPhysicalDeviceMemoryProperties(Renderer.PhysicalDevice, &memProperties);

    for (uint32 x = 0; x < memProperties.memoryTypeCount; x++)
    {
        if ((typeFilter & (1 << x)) &&
            (memProperties.memoryTypes[x].propertyFlags & properties) == properties)
        {
            return x;
        }
    }

    fprintf(stderr, "Couldn't find suitable memory type.\n");
    Renderer_DestroyRenderer();
    vulkanWindow->DestroyWindow(vulkanWindow);
    return -1;
}

VkCommandBuffer Renderer_BeginSingleUseCommandBuffer()
{
    VkCommandBuffer commandBuffer = VK_NULL_HANDLE;
    VkCommandBufferAllocateInfo allocInfo =
    {
        .sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO,
        .level = VK_COMMAND_BUFFER_LEVEL_PRIMARY,
        .commandPool = Renderer.CommandPool,
        .commandBufferCount = 1
    };
    VULKAN_RESULT(vkAllocateCommandBuffers(Renderer.Device, &allocInfo, &commandBuffer));

    VkCommandBufferBeginInfo beginInfo =
    {
        .sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
        .flags = VK_COMMAND_BUFFER_USAGE_ONE_TIME_SUBMIT_BIT
    };
    VULKAN_RESULT(vkBeginCommandBuffer(commandBuffer, &beginInfo));
    return commandBuffer;
}

VkResult Renderer_EndSingleUseCommandBuffer(VkCommandBuffer commandBuffer)
{
    VULKAN_RESULT(vkEndCommandBuffer(commandBuffer));

    VkSubmitInfo submitInfo =
    {
        .sType = VK_STRUCTURE_TYPE_SUBMIT_INFO,
        .commandBufferCount = 1,
        .pCommandBuffers = &commandBuffer
    };
    VULKAN_RESULT(vkQueueSubmit(Renderer.SwapChain.GraphicsQueue, 1, &submitInfo, VK_NULL_HANDLE));
    VULKAN_RESULT(vkQueueWaitIdle(Renderer.SwapChain.GraphicsQueue));
    vkFreeCommandBuffers(Renderer.Device, Renderer.CommandPool, 1, &commandBuffer);
    return VK_SUCCESS;
}

void Renderer_DestroyRenderer()
{
    Vulkan_DestroyImageView();
    Vulkan_DestroySwapChain();
    Renderer_DestroyCommandPool();
    Renderer_DestroyFences();
    Renderer_DestroyDevice();
    Renderer_DestroyDebugger();
    Renderer_DestroySurface();
    Renderer_DestroyInstance();
}

void Renderer_DestroyCommandPool()
{
    if (Renderer.CommandPool != VK_NULL_HANDLE)
    {
        vkDestroyCommandPool(Renderer.Device, Renderer.CommandPool, NULL);
        Renderer.CommandPool = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyFences()
{
    for (size_t x = 0; x < MAX_FRAMES_IN_FLIGHT; x++)
    {
        if (Renderer.AcquireImageSemaphores[x] != VK_NULL_HANDLE)
        {
            vkDestroySemaphore(Renderer.Device, Renderer.AcquireImageSemaphores[x], NULL);
            Renderer.AcquireImageSemaphores[x] = VK_NULL_HANDLE;
        }
        if (Renderer.PresentImageSemaphores[x] != VK_NULL_HANDLE)
        {
            vkDestroySemaphore(Renderer.Device, Renderer.PresentImageSemaphores[x], NULL);
            Renderer.PresentImageSemaphores[x] = VK_NULL_HANDLE;
        }
        if (Renderer.InFlightFences[x] != VK_NULL_HANDLE)
        {
            vkDestroyFence(Renderer.Device, Renderer.InFlightFences[x], NULL);
            Renderer.InFlightFences[x] = VK_NULL_HANDLE;
        }
    }
}

void Renderer_DestroyDevice()
{
    if (Renderer.Device != VK_NULL_HANDLE)
    {
        vkDestroyDevice(Renderer.Device, NULL);
        Renderer.Device = VK_NULL_HANDLE;
    }
}

void Renderer_DestroySurface()
{
    if (Renderer.Surface != VK_NULL_HANDLE)
    {
        vkDestroySurfaceKHR(Renderer.Instance, Renderer.Surface, NULL);
        Renderer.Surface = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyDebugger()
{
    DestroyDebugUtilsMessengerEXT(Renderer.Instance, NULL);
}

void Renderer_DestroyInstance()
{
    if (Renderer.Instance != VK_NULL_HANDLE)
    {
        vkDestroyInstance(Renderer.Instance, NULL);
        Renderer.Instance = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyRenderPass(VkRenderPass* renderPass)
{
    if (*renderPass != VK_NULL_HANDLE)
    {
        vkDestroyRenderPass(Renderer.Device, *renderPass, NULL);
        *renderPass = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyFrameBuffers(VkFramebuffer* frameBufferList)
{
    for (size_t x = 0; x < Renderer.SwapChain.SwapChainImageCount; x++)
    {
        if (frameBufferList[x] != VK_NULL_HANDLE)
        {
            vkDestroyFramebuffer(Renderer.Device, frameBufferList[x], NULL);
            frameBufferList[x] = VK_NULL_HANDLE;
        }
    }
}

void Renderer_DestroyDescriptorPool(VkDescriptorPool* descriptorPool)
{
    if (*descriptorPool != VK_NULL_HANDLE)
    {
        vkDestroyDescriptorPool(Renderer.Device, *descriptorPool, NULL);
        *descriptorPool = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyDescriptorSetLayout(VkDescriptorSetLayout* descriptorSetLayout)
{
    if (*descriptorSetLayout != VK_NULL_HANDLE)
    {
        vkDestroyDescriptorSetLayout(Renderer.Device, *descriptorSetLayout, NULL);
        *descriptorSetLayout = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyCommandBuffers(VkCommandPool* commandPool, VkCommandBuffer* commandBufferList)
{
    if (*commandBufferList != VK_NULL_HANDLE)
    {
        vkFreeCommandBuffers(Renderer.Device, *commandPool, Renderer.SwapChain.SwapChainImageCount, &*commandBufferList);
        for (size_t x = 0; x < Renderer.SwapChain.SwapChainImageCount; x++)
        {
            commandBufferList[x] = VK_NULL_HANDLE;
        }
    }
}

void Renderer_DestroyCommnadPool(VkCommandPool* commandPool)
{
    if (*commandPool != VK_NULL_HANDLE)
    {
        vkDestroyCommandPool(Renderer.Device, *commandPool, NULL);
        *commandPool = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyBuffer(VkBuffer* buffer)
{
    if (*buffer != VK_NULL_HANDLE)
    {
        vkDestroyBuffer(Renderer.Device, *buffer, NULL);
        *buffer = VK_NULL_HANDLE;
    }
}

void Renderer_FreeMemory(VkDeviceMemory* memory)
{
    if (*memory != VK_NULL_HANDLE)
    {
        vkFreeMemory(Renderer.Device, *memory, NULL);
        *memory = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyImageView(VkImageView* imageView)
{
    if (*imageView != VK_NULL_HANDLE)
    {
        vkDestroyImageView(Renderer.Device, *imageView, NULL);
        *imageView = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyImage(VkImage* image)
{
    if (*image != VK_NULL_HANDLE)
    {
        vkDestroyImage(Renderer.Device, *image, NULL);
        *image = VK_NULL_HANDLE;
    }
}

void Renderer_DestroySampler(VkSampler* sampler)
{
    if (*sampler != VK_NULL_HANDLE)
    {
        vkDestroySampler(Renderer.Device, *sampler, NULL);
        *sampler = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyPipeline(VkPipeline* pipeline)
{
    if (*pipeline != VK_NULL_HANDLE)
    {
        vkDestroyPipeline(Renderer.Device, *pipeline, NULL);
        *pipeline = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyPipelineLayout(VkPipelineLayout* pipelineLayout)
{
    if (*pipelineLayout != VK_NULL_HANDLE)
    {
        vkDestroyPipelineLayout(Renderer.Device, *pipelineLayout, NULL);
        *pipelineLayout = VK_NULL_HANDLE;
    }
}

void Renderer_DestroyPipelineCache(VkPipelineCache* pipelineCache)
{
    if (*pipelineCache != VK_NULL_HANDLE)
    {
        vkDestroyPipelineCache(Renderer.Device, *pipelineCache, NULL);
        *pipelineCache = VK_NULL_HANDLE;
    }
}

int SimpleTest()
{
    return 42;
}