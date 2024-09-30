//using System;
//using System.Runtime.InteropServices;
//using static VulkanGameEngineLevelEditor.VulkanAPI;
//using static VulkanGameEngineLevelEditor.GameEngineAPI.VulkanRenderer;

//namespace VulkanGameEngineLevelEditor.GameEngineAPI
//{
//    public unsafe class VulkanDebugMessenger
//    {
//        private static VkDebugUtilsMessengerEXT debugMessenger;

//        // Delegate for the Vulkan callback
//        public delegate VkBool32 VkDebugUtilsMessengerCallbackEXT(
//            VkDebugUtilsMessageSeverityFlagBitsEXT messageSeverity,
//            VkDebugUtilsMessageTypeFlagsEXT messageTypes,
//            ref VkDebugUtilsMessengerCallbackDataEXT pCallbackData,
//            IntPtr pUserData);

//        [StructLayout(LayoutKind.Sequential)]
//        public struct VkDebugUtilsMessengerCreateInfoEXT
//        {
//            public VkStructureType sType;
//            public IntPtr pNext;  // Pointer to the next structure in a chain (can be null)
//            public VkDebugUtilsMessageTypeFlagsEXT messageType;
//            public VkDebugUtilsMessageSeverityFlagBitsEXT messageSeverity;
//            public VkDebugUtilsMessengerCallbackEXT pfnUserCallback;  // Callback function
//            public IntPtr pUserData;  // Pointer to user data
//        }

//        [StructLayout(LayoutKind.Sequential)]
//        public struct VkDebugUtilsMessengerCallbackDataEXT
//        {
//            public VkStructureType sType;
//            public IntPtr pNext;
//            public uint messageIdNumber;
//            public IntPtr pMessageIdName;
//            public IntPtr pMessage;
//            public uint queueLabelCount;
//            public IntPtr pQueueLabels;
//            public uint cmdBufLabelCount;
//            public IntPtr pCmdBufLabels;
//            public uint objectCount;
//            public IntPtr pObjects;
//        }


//        private static VkBool32 VulkanDebugCallbackFunction(
//    VkDebugUtilsMessageSeverityFlagBitsEXT messageSeverity,
//    VkDebugUtilsMessageTypeFlagsEXT messageType,
//    ref VkDebugUtilsMessengerCallbackDataEXT pCallbackData,
//    IntPtr pUserData)
//        {
//            // Access the message using pCallbackData structure
//            var message = Marshal.PtrToStringAnsi(pCallbackData.pMessage);

//            // Log the Vulkan message based on its severity
//            Console.WriteLine($"[{messageSeverity}]: {message}");

//            return VulkanConsts.VK_FALSE; // Indicate the message was processed
//        }

//        public static VulkanGameEngineLevelEditor.GameEngineAPI.VulkanDebugMessenger.VkDebugUtilsMessengerCreateInfoEXT GetDebugInfo()
//        {
//            return new VulkanGameEngineLevelEditor.GameEngineAPI.VulkanDebugMessenger.VkDebugUtilsMessengerCreateInfoEXT // Fully qualify if necessary
//            {
//                sType = VkStructureType.VK_STRUCTURE_TYPE_DEBUG_UTILS_MESSENGER_CREATE_INFO_EXT,
//                messageSeverity =
//        VkDebugUtilsMessageSeverityFlagBitsEXT.VK_DEBUG_UTILS_MESSAGE_SEVERITY_INFO_BIT_EXT |
//        VkDebugUtilsMessageSeverityFlagBitsEXT.VK_DEBUG_UTILS_MESSAGE_SEVERITY_WARNING_BIT_EXT |
//        VkDebugUtilsMessageSeverityFlagBitsEXT.VK_DEBUG_UTILS_MESSAGE_SEVERITY_ERROR_BIT_EXT,
//                messageType =
//        VkDebugUtilsMessageTypeFlagsEXT.VK_DEBUG_UTILS_MESSAGE_TYPE_GENERAL_BIT_EXT |
//        VkDebugUtilsMessageTypeFlagsEXT.VK_DEBUG_UTILS_MESSAGE_TYPE_VALIDATION_BIT_EXT |
//        VkDebugUtilsMessageTypeFlagsEXT.VK_DEBUG_UTILS_MESSAGE_TYPE_PERFORMANCE_BIT_EXT,
//                pfnUserCallback = VulkanDebugCallbackFunction,
//                pUserData = IntPtr.Zero
//            };

//        }
//        private static bool IsDebugUtilsMessengerSupported()
//        {
//            uint extensionCount = 0;

//            // First, get the number of available extensions
//            var result = vkEnumerateInstanceExtensionProperties(IntPtr.Zero, ref extensionCount, IntPtr.Zero);
//            if (result != VkResult.VK_SUCCESS)
//            {
//                // Handle error if needed
//                return false;
//            }

//            // Create an array to hold the extension properties
//            VkExtensionProperties[] extensions = new VkExtensionProperties[extensionCount];

//            // Get the extension properties
//            fixed (VkExtensionProperties* pExtensions = extensions)
//            {
//                result = vkEnumerateInstanceExtensionProperties(IntPtr.Zero, ref extensionCount, (IntPtr)pExtensions);
//            }
//            // Check if the debug utility extension is available
//            foreach (var extension in extensions)
//            {
                
//                if (extension.extensionName == "VK_EXT_debug_utils")
//                {
//                    return true; // Extension is supported
//                }
//            }

//            return false; // Extension not supported
//        }

//        //public static VkDebugUtilsMessengerEXT SetupDebugMessenger()
//        //{
//        //  //  var a = IsDebugUtilsMessengerSupported();
//        //    var debug = GetDebugInfo();
//        //    var debugMessager = new VkDebugUtilsMessengerEXT();
//        //    if (vkCreateDebugUtilsMessengerEXT(Instance, &debug, null, &debugMessager) != VkResult.VK_SUCCESS)
//        //    {
//        //        Console.Error.WriteLine("Failed to set up debug messenger!");
//        //        return default; // Return default on failure
//        //    }

//        //    return debugMessenger;
//        //}
//    }
//}