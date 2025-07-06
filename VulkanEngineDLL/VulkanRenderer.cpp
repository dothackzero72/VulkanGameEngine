#include "VulkanRenderer.h"

VkInstance Renderer_CreateVulkanInstance2() {
    VkInstance instance = VK_NULL_HANDLE;

    VkDebugUtilsMessengerCreateInfoEXT debugInfo = {};
    debugInfo.sType = VK_STRUCTURE_TYPE_DEBUG_UTILS_MESSENGER_CREATE_INFO_EXT;
    debugInfo.messageSeverity = VK_DEBUG_UTILS_MESSAGE_SEVERITY_WARNING_BIT_EXT |
        VK_DEBUG_UTILS_MESSAGE_SEVERITY_ERROR_BIT_EXT;
    debugInfo.messageType = VK_DEBUG_UTILS_MESSAGE_TYPE_GENERAL_BIT_EXT |
        VK_DEBUG_UTILS_MESSAGE_TYPE_VALIDATION_BIT_EXT |
        VK_DEBUG_UTILS_MESSAGE_TYPE_PERFORMANCE_BIT_EXT;
    debugInfo.pfnUserCallback = Vulkan_DebugCallBack2;

    VkValidationFeaturesEXT validationFeatures = {};
    validationFeatures.sType = VK_STRUCTURE_TYPE_VALIDATION_FEATURES_EXT;
    validationFeatures.pNext = &debugInfo;
    validationFeatures.enabledValidationFeatureCount = static_cast<uint32_t>(enabledList.size());
    validationFeatures.pEnabledValidationFeatures = enabledList.data();
    validationFeatures.disabledValidationFeatureCount = static_cast<uint32_t>(disabledList.size());
    validationFeatures.pDisabledValidationFeatures = disabledList.data();

    int enableValidationLayers = 1;
#ifdef NDEBUG
    enableValidationLayers = 0;
#endif

    uint32_t extensionCount = 0;
    std::vector<std::string> extensionNames;
    std::vector<const char*> extensionNamesPointers;

    VkResult result = Renderer_GetWin32Extensions2(&extensionCount, extensionNames);
    if (result != VK_SUCCESS) {
        fprintf(stderr, "Failed to get extensions: %d\n", result);
        return VK_NULL_HANDLE;
    }

    extensionNamesPointers.reserve(extensionCount);
    for (const auto& name : extensionNames) {
        extensionNamesPointers.push_back(name.c_str());
    }

    VkApplicationInfo applicationInfo = {};
    applicationInfo.sType = VK_STRUCTURE_TYPE_APPLICATION_INFO;
    applicationInfo.pApplicationName = "Vulkan Application";
    applicationInfo.applicationVersion = VK_MAKE_VERSION(1, 0, 0);
    applicationInfo.pEngineName = "No Engine";
    applicationInfo.engineVersion = VK_MAKE_VERSION(1, 0, 0);
    applicationInfo.apiVersion = VK_API_VERSION_1_4;

    VkInstanceCreateInfo createInfo = {};
    createInfo.sType = VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO;
    createInfo.pNext = enableValidationLayers ? &validationFeatures : nullptr;
    createInfo.pApplicationInfo = &applicationInfo;
    createInfo.enabledLayerCount = enableValidationLayers ? 1 : 0;
    createInfo.ppEnabledLayerNames = enableValidationLayers ? ValidationLayers : nullptr;
    createInfo.enabledExtensionCount = static_cast<uint32_t>(extensionNamesPointers.size());
    createInfo.ppEnabledExtensionNames = extensionNamesPointers.data();

    result = vkCreateInstance(&createInfo, nullptr, &instance);
    if (result != VK_SUCCESS) {
        fprintf(stderr, "Failed to create Vulkan instance: %d\n", result);
        return VK_NULL_HANDLE;
    }

    return instance;
}

VkSurfaceKHR Renderer_CreateVulkanSurface2(void* windowHandle, VkInstance instance) 
{
    if (!windowHandle || !instance) {
        fprintf(stderr, "Invalid window handle (%p) or instance (%p)\n", windowHandle, instance);
        return VK_NULL_HANDLE;
    }

    VkWin32SurfaceCreateInfoKHR surfaceCreateInfo = {};
    surfaceCreateInfo.sType = VK_STRUCTURE_TYPE_WIN32_SURFACE_CREATE_INFO_KHR;
    surfaceCreateInfo.pNext = nullptr;
    surfaceCreateInfo.hinstance = GetModuleHandle(nullptr);
    surfaceCreateInfo.hwnd = (HWND)windowHandle;

    fprintf(stderr, "Creating surface with hwnd=%p, hinstance=%p\n", windowHandle, surfaceCreateInfo.hinstance);

    VkSurfaceKHR surface;
    VkResult result = vkCreateWin32SurfaceKHR(instance, &surfaceCreateInfo, nullptr, &surface);
    if (result != VK_SUCCESS) {
        fprintf(stderr, "Failed to create Vulkan surface: %d\n", result);
        return VK_NULL_HANDLE;
    }

    return surface;
}

VkResult Renderer_GetWin32Extensions2(uint32_t* extensionCount, std::vector<std::string>& enabledExtensions) {
    if (!extensionCount) {
        fprintf(stderr, "Invalid argument: extensionCount cannot be NULL.\n");
        return VK_ERROR_INITIALIZATION_FAILED;
    }

    VkResult result = vkEnumerateInstanceExtensionProperties(nullptr, extensionCount, nullptr);
    if (result != VK_SUCCESS) {
        fprintf(stderr, "Failed to enumerate instance extension properties: %d\n", result);
        *extensionCount = 0;
        return result;
    }

    std::vector<VkExtensionProperties> extensions(*extensionCount);
    result = vkEnumerateInstanceExtensionProperties(nullptr, extensionCount, extensions.data());
    if (result != VK_SUCCESS) {
        fprintf(stderr, "Failed to enumerate instance extension properties after allocation: %d\n", result);
        *extensionCount = 0;
        return result;
    }

    enabledExtensions.clear();
    for (const char* requiredExt : InstanceExtensionList) {
        bool found = false;
        for (const auto& ext : extensions) {
            if (strcmp(ext.extensionName, requiredExt) == 0) {
                enabledExtensions.emplace_back(ext.extensionName);
                found = true;
                break;
            }
        }
        if (!found) {
            fprintf(stderr, "Required extension %s not supported\n", requiredExt);
            return VK_ERROR_EXTENSION_NOT_PRESENT;
        }
    }

    *extensionCount = static_cast<uint32_t>(enabledExtensions.size());
    return VK_SUCCESS;
}

VkBool32 VKAPI_CALL Vulkan_DebugCallBack2(
    VkDebugUtilsMessageSeverityFlagBitsEXT MessageSeverity,
    VkDebugUtilsMessageTypeFlagsEXT MessageType,
    const VkDebugUtilsMessengerCallbackDataEXT* CallBackData,
    void* UserData)
{
    HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
    if (hConsole == INVALID_HANDLE_VALUE) {
        printf("Failed to get console handle.\n");
        return 1;
    }

    CONSOLE_SCREEN_BUFFER_INFO consoleInfo;
    GetConsoleScreenBufferInfo(hConsole, &consoleInfo);
    WORD originalAttributes = consoleInfo.wAttributes;

    char message[4096];
    snprintf(message, sizeof(message), "%s", CallBackData->pMessage);

    switch (MessageSeverity)
    {
    case VK_DEBUG_UTILS_MESSAGE_SEVERITY_VERBOSE_BIT_EXT:
        SetConsoleTextAttribute(hConsole, FOREGROUND_BLUE);
        fprintf(stdout, "VERBOSE: ");
        SetConsoleTextAttribute(hConsole, originalAttributes);
        fprintf(stdout, "%s\n", message);
        break;
    case VK_DEBUG_UTILS_MESSAGE_SEVERITY_INFO_BIT_EXT:
        SetConsoleTextAttribute(hConsole, FOREGROUND_GREEN);
        fprintf(stdout, "INFO: ");
        SetConsoleTextAttribute(hConsole, originalAttributes);
        fprintf(stdout, "%s\n", message);
        break;
    case VK_DEBUG_UTILS_MESSAGE_SEVERITY_WARNING_BIT_EXT:
        SetConsoleTextAttribute(hConsole, FOREGROUND_RED | FOREGROUND_GREEN);
        fprintf(stdout, "WARNING: ");
        SetConsoleTextAttribute(hConsole, originalAttributes);
        fprintf(stdout, "%s\n", message);
        break;
    case VK_DEBUG_UTILS_MESSAGE_SEVERITY_ERROR_BIT_EXT:
        SetConsoleTextAttribute(hConsole, FOREGROUND_RED);
        fprintf(stdout, "ERROR: ");
        SetConsoleTextAttribute(hConsole, originalAttributes);
        fprintf(stdout, "%s\n", message);
        break;
    default:
        fprintf(stderr, "UNKNOWN SEVERITY: %s\n", message);
        break;
    }

    return VK_FALSE;
}