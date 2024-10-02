using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.Core;
using Silk.NET.Core.Native;
using Silk.NET.Maths;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.EXT;
using Silk.NET.Vulkan.Extensions.KHR;
using Silk.NET.Windowing;
using VulkanGameEngineLevelEditor.GameEngineAPI;

namespace VulkanGameEngineLevelEditor.Tests
{
    unsafe class SU
    {
        public static Vk vk = Vk.GetApi();
        public static string[] extensions;
        public static VkDeviceMemory AllocateDeviceMemory(Device device,
                                           PhysicalDeviceMemoryProperties memoryProperties,
                                           MemoryRequirements memoryRequirements,
                                           MemoryPropertyFlags memoryPropertyFlags)
        {
            uint memoryTypeIndex = FindMemoryType(memoryProperties, memoryRequirements.MemoryTypeBits, memoryPropertyFlags);

            MemoryAllocateInfo allocInfo = new
            (
                allocationSize: memoryRequirements.Size,
                memoryTypeIndex: memoryTypeIndex
            );

            return new VkDeviceMemory(device, allocInfo);
        }


        public static DebugUtilsMessengerCreateInfoEXT MakeDebugUtilsMessengerCreateInfoEXT()
        {
            DebugUtilsMessengerCreateInfoEXT createInfo = new
            (
                messageSeverity:
                    DebugUtilsMessageSeverityFlagsEXT.DebugUtilsMessageSeverityVerboseBitExt |
                    DebugUtilsMessageSeverityFlagsEXT.DebugUtilsMessageSeverityInfoBitExt |
                    DebugUtilsMessageSeverityFlagsEXT.DebugUtilsMessageSeverityWarningBitExt |
                    DebugUtilsMessageSeverityFlagsEXT.DebugUtilsMessageSeverityErrorBitExt,
                messageType:
                    DebugUtilsMessageTypeFlagsEXT.DebugUtilsMessageTypeGeneralBitExt |
                    DebugUtilsMessageTypeFlagsEXT.DebugUtilsMessageTypeValidationBitExt |
                    DebugUtilsMessageTypeFlagsEXT.DebugUtilsMessageTypePerformanceBitExt,
                pfnUserCallback: new PfnDebugUtilsMessengerCallbackEXT(MessageCallback)
            );

            return createInfo;
        }

        public static uint MessageCallback(DebugUtilsMessageSeverityFlagsEXT severity, DebugUtilsMessageTypeFlagsEXT messageType, DebugUtilsMessengerCallbackDataEXT* callbackData, void* userData)
        {
            string messageSeverity;

            if (severity < DebugUtilsMessageSeverityFlagsEXT.DebugUtilsMessageSeverityWarningBitExt) return Vk.False;

            if (severity.HasFlag(DebugUtilsMessageSeverityFlagsEXT.DebugUtilsMessageSeverityErrorBitExt))
            {
                messageSeverity = "error";
            }
            else if (severity.HasFlag(DebugUtilsMessageSeverityFlagsEXT.DebugUtilsMessageSeverityInfoBitExt))
            {
                messageSeverity = "info";
            }
            else if (severity.HasFlag(DebugUtilsMessageSeverityFlagsEXT.DebugUtilsMessageSeverityVerboseBitExt))
            {
                messageSeverity = "verbose";
            }
            else
            {
                messageSeverity = "warning";
            }

            string messageTypeStr;

            if (messageType.HasFlag(DebugUtilsMessageTypeFlagsEXT.DebugUtilsMessageTypeGeneralBitExt))
            {
                messageTypeStr = "";
            }
            else if (messageType.HasFlag(DebugUtilsMessageTypeFlagsEXT.DebugUtilsMessageTypePerformanceBitExt))
            {
                messageTypeStr = "performance ";
            }
            else
            {
                messageTypeStr = "validation ";
            }

            string messageIdName = Marshal.PtrToStringAnsi((nint)callbackData->PMessageIdName);
            int messageIdNumber = callbackData->MessageIdNumber;
            string message = Marshal.PtrToStringAnsi((nint)callbackData->PMessage);

            if (callbackData->QueueLabelCount > 0)
            {
                message += "\n\tQueue Labels:\n";
                for (uint i = 0; i < callbackData->QueueLabelCount; i++)
                {
                    string labelName = Marshal.PtrToStringAnsi((nint)callbackData->PQueueLabels[i].PLabelName);

                    message += $"\t\tlabelName = <${labelName}>";
                }
            }

            if (callbackData->CmdBufLabelCount > 0)
            {
                message += "\n\tCommandBuffer Labels:\n";
                for (uint i = 0; i < callbackData->CmdBufLabelCount; i++)
                {
                    string labelName = Marshal.PtrToStringAnsi((nint)callbackData->PCmdBufLabels[i].PLabelName);

                    message += $"\t\tlabelName = <${labelName}>";
                }
            }

            if (callbackData->ObjectCount > 0)
            {
                for (uint i = 0; i < callbackData->ObjectCount; i++)
                {
                    message += $"\n\tObject {i}\n";
                    message += $"\t\tobjectType = {callbackData->PObjects[i].ObjectType}\n";
                    message += $"\t\tobjectHandle = {callbackData->PObjects[i].ObjectHandle}";
                    if (callbackData->PObjects[i].PObjectName != null)
                    {
                        string objectName = Marshal.PtrToStringAnsi((nint)callbackData->PObjects[i].PObjectName);

                        message += $"\n\t\tobjectName = <{objectName}>";
                    }
                }
            }

            Debug.WriteLine($"VK ({messageTypeStr}{messageSeverity}) {messageIdName}: {message}");

            return Vk.False;
        }

        public static void GetQueueFamilyProperties(PhysicalDevice physicalDevice, uint* queueFamilyPropertyCount, Span<QueueFamilyProperties> queueFamilyProperties)
        {
            vk.GetPhysicalDeviceQueueFamilyProperties(physicalDevice, queueFamilyPropertyCount, queueFamilyProperties);
        }

        public static bool GetSurfaceSupportKHR(PhysicalDevice physicalDevice, SurfaceKHR surface, KhrSurface khrSurface, uint queueFamilyIndex)
        {
            Result result = khrSurface.GetPhysicalDeviceSurfaceSupport(physicalDevice, queueFamilyIndex, surface, out Bool32 presentSupport);
            if (result != Result.Success)
            {
                //ResultException.Throw(result, "Error getting supported surfaces by physical device");
            }

            return presentSupport;
        }

        public static QueueFamilyIndices FindGraphicsAndPresentQueueFamilyIndex(PhysicalDevice physicalDevice, SurfaceKHR surface, KhrSurface khrSurface)
        {
            QueueFamilyIndices queueFamilyIndices = new();

            uint queueFamilyCount = 0;

            GetQueueFamilyProperties(physicalDevice, &queueFamilyCount, null);

            QueueFamilyProperties[] queueFamilies = new QueueFamilyProperties[queueFamilyCount];
            GetQueueFamilyProperties(physicalDevice, &queueFamilyCount, queueFamilies);

            uint i = 0;
            foreach (var queueFamily in queueFamilies)
            {
                if (queueFamily.QueueFlags.HasFlag(QueueFlags.QueueGraphicsBit))
                {
                    queueFamilyIndices.GraphicsFamily = i;
                }

                bool isPresentSupport = GetSurfaceSupportKHR(physicalDevice, surface, khrSurface, i);

                if (isPresentSupport)
                {
                    queueFamilyIndices.PresentFamily = i;
                }

                if (queueFamilyIndices.IsComplete)
                {
                    break;
                }

                i++;
            }

            return queueFamilyIndices;
        }

        public static FormatProperties GetFormatProperties(PhysicalDevice physicalDevice, Format format)
        {
            vk.GetPhysicalDeviceFormatProperties(physicalDevice, format,
                out FormatProperties props);

            return props;
        }

        public static SurfaceFormatKHR[] GetSurfaceFormatsKHR(PhysicalDevice physicalDevice, SurfaceKHR surface, KhrSurface khrSurface)
        {
            uint surfaceFormatCount;

            Result result = khrSurface.GetPhysicalDeviceSurfaceFormats(physicalDevice, surface, &surfaceFormatCount, null);
            if (result != Result.Success)
            {
                //ResultException.Throw(result, "Error getting physical device supported surface formats");
            }

            SurfaceFormatKHR[] surfaceFormats = new SurfaceFormatKHR[surfaceFormatCount];

            result = khrSurface.GetPhysicalDeviceSurfaceFormats(physicalDevice, surface, &surfaceFormatCount, surfaceFormats);
            if (result != Result.Success)
            {
                //ResultException.Throw(result, "Error getting physical device supported surface formats");
            }

            return surfaceFormats;
        }

        public static SurfaceCapabilitiesKHR GetSurfaceCapabilitiesKHR(PhysicalDevice physicalDevice, SurfaceKHR surface, KhrSurface khrSurface)
        {
            Result result = khrSurface.GetPhysicalDeviceSurfaceCapabilities(physicalDevice, surface, out SurfaceCapabilitiesKHR surfaceCapabilities);
            if (result != Result.Success)
            {
                //ResultException.Throw(result, "Error getting physical device supported surface capabilities");
            }

            return surfaceCapabilities;
        }

        public static PresentModeKHR[] GetSurfacePresentModesKHR(PhysicalDevice physicalDevice, SurfaceKHR surface, KhrSurface khrSurface)
        {
            uint presentModesCount;

            Result result = khrSurface.GetPhysicalDeviceSurfacePresentModes(physicalDevice, surface, &presentModesCount, null);
            if (result != Result.Success)
            {
                ///ResultException.Throw(result, "Error getting physical device supported surface present modes");
            }

            PresentModeKHR[] presentModes = new PresentModeKHR[presentModesCount];

            result = khrSurface.GetPhysicalDeviceSurfacePresentModes(physicalDevice, surface, &presentModesCount, presentModes);
            if (result != Result.Success)
            {
                //ResultException.Throw(result, "Error getting physical device supported surface present modes");
            }

            return presentModes;
        }

        public static PhysicalDeviceFeatures GetSupportedFeatures(PhysicalDevice physicalDevice)
        {
            vk.GetPhysicalDeviceFeatures(physicalDevice, out PhysicalDeviceFeatures supportedFeatures);

            return supportedFeatures;
        }

        public static PhysicalDeviceProperties GetProperties(PhysicalDevice physicalDevice)
        {
            vk.GetPhysicalDeviceProperties(physicalDevice, out PhysicalDeviceProperties properties);

            return properties;
        }

        public static PhysicalDeviceMemoryProperties GetMemoryProperties(PhysicalDevice physicalDevice)
        {
            vk.GetPhysicalDeviceMemoryProperties(physicalDevice, out PhysicalDeviceMemoryProperties memProperties);

            return memProperties;
        }

        public static string[] EnumerateExtensionProperties(PhysicalDevice physicalDevice)
        {
            uint extensionCount;
            vk.EnumerateDeviceExtensionProperties(physicalDevice, (byte*)null, &extensionCount, null);

            ExtensionProperties[] availableExtensions = new ExtensionProperties[extensionCount];
            vk.EnumerateDeviceExtensionProperties(physicalDevice, (byte*)null, &extensionCount, availableExtensions);

            extensions = new string[extensionCount];

            for (int i = 0; i < extensionCount; i++)
            {
                var extension = availableExtensions[i];

                extensions[i] = Marshal.PtrToStringAnsi((nint)extension.ExtensionName);
            }

            return extensions;
        }

        public static PhysicalDevice[] EnumerateDevices(Instance instance)
        {
            uint deviceCount = 0;
            vk.EnumeratePhysicalDevices(instance, &deviceCount, null);

            if (deviceCount == 0)
            {
                throw new Exception("failed to find GPUs with Vulkan support!");
            }

            PhysicalDevice[] devices = new PhysicalDevice[deviceCount];

            vk.EnumeratePhysicalDevices(instance, &deviceCount, devices);


            return devices;
        }

        public static PhysicalDevice PickPhysicalDevice(Instance instance, SurfaceKHR surface, KhrSurface khrSurface, string[] deviceExtensions)
        {
            PhysicalDevice pickedDevice = EnumerateDevices(instance).FirstOrDefault();

            foreach (var device in EnumerateDevices(instance))
            {
                string[] availableExtensions = EnumerateExtensionProperties(pickedDevice);

                bool isExtensionsSupported = deviceExtensions.All(e => availableExtensions.Contains(e));

                var indices = FindGraphicsAndPresentQueueFamilyIndex(device, surface, khrSurface);

                bool isSwapChainAdequate = false;
                if (isExtensionsSupported)
                {
                    var formats = GetSurfaceFormatsKHR(pickedDevice, surface, khrSurface);
                    var presentModes = GetSurfacePresentModesKHR(pickedDevice, surface, khrSurface);

                    isSwapChainAdequate = (formats != null) && (presentModes != null);
                }

                if (indices.IsComplete && isExtensionsSupported && isSwapChainAdequate
                    && GetSupportedFeatures(pickedDevice).SamplerAnisotropy)
                {
                    pickedDevice = device;
                }
            }

            return pickedDevice;
        }


        public static Device CreateDevice(
            Instance instance,
            PhysicalDevice physicalDevice,
            QueueFamilyIndices indices,
            PhysicalDeviceFeatures deviceFeatures,
            string[] validationLayers = null,
            string[] deviceExtensions = null)
        {
            float queuePriority = 1.0f;
            Vk vk = Vk.GetApi();

            HashSet<uint> uniqueQueueFamilies = new() { indices.GraphicsFamily.Value, indices.PresentFamily.Value };

            using var mem = GlobalMemory.Allocate(uniqueQueueFamilies.Count * sizeof(DeviceQueueCreateInfo));
            var queueCreateInfos = (DeviceQueueCreateInfo*)Unsafe.AsPointer(ref mem.GetPinnableReference());

            int i = 0;

            foreach (var queueId in uniqueQueueFamilies)
            {
                DeviceQueueCreateInfo queueCreateInfo = new
                (
                    queueFamilyIndex: queueId,
                    queueCount: 1,
                    pQueuePriorities: &queuePriority
                );

                queueCreateInfos[i++] = queueCreateInfo;
            };

            DeviceCreateInfo createInfo = new
            (
                pQueueCreateInfos: queueCreateInfos,
                queueCreateInfoCount: (uint)uniqueQueueFamilies.Count,
                pEnabledFeatures: &deviceFeatures
            );

            if (deviceExtensions != null)
            {
                createInfo.EnabledExtensionCount = (uint)deviceExtensions.Length;
                createInfo.PpEnabledExtensionNames = (byte**)SilkMarshal.StringArrayToPtr(deviceExtensions);
            }

            if (validationLayers != null)
            {
                createInfo.EnabledLayerCount = (uint)validationLayers.Length;
                createInfo.PpEnabledLayerNames = (byte**)SilkMarshal.StringArrayToPtr(validationLayers);
            }

            var result = vk.CreateDevice(physicalDevice, in createInfo, null, out Device device);

            if (result != Result.Success)
            {
                //ResultException.Throw(result, "Error creating logical device");
            }

            return device;
        }

        public static SurfaceFormatKHR PickSurfaceFormat(SurfaceFormatKHR[] formats)
        {
            SurfaceFormatKHR pickedFormat = formats[0];
            if (formats.Length == 1)
            {
                if (formats[0].Format == Format.Undefined)
                {
                    pickedFormat.Format = Format.B8G8R8A8Unorm;
                    pickedFormat.ColorSpace = ColorSpaceKHR.ColorspaceSrgbNonlinearKhr;
                }
            }
            else
            {
                // request several formats, the first found will be used
                Format[] requestedFormats = new Format[] { Format.B8G8R8A8Srgb, Format.R8G8B8A8Srgb, Format.B8G8R8Unorm, Format.R8G8B8Unorm };
                ColorSpaceKHR requestedColorSpace = ColorSpaceKHR.ColorspaceSrgbNonlinearKhr;
                for (int i = 0; i < requestedFormats.Length; i++)
                {
                    Format requestedFormat = requestedFormats[i];

                    if (formats.Any(f => f.Format == requestedFormat && f.ColorSpace == requestedColorSpace))
                    {
                        pickedFormat.Format = requestedFormat;
                        pickedFormat.ColorSpace = requestedColorSpace;
                        break;
                    }

                }
            }

            return pickedFormat;
        }

        static uint FindMemoryType(PhysicalDeviceMemoryProperties memoryProperties, uint typeBits, MemoryPropertyFlags requirementsMask)
        {
            for (int i = 0; i < memoryProperties.MemoryTypeCount; i++)
            {
                if ((typeBits & (i << 1)) != 0 && memoryProperties.MemoryTypes[i].PropertyFlags.HasFlag(requirementsMask))
                {
                    return (uint)i;

                }
            }

            throw new Exception("failed to find suitable memory type!");
        }


        public static void Submit(Queue queue, SubmitInfo submit, void* fence)
        {
            var result = vk.QueueSubmit(queue, 1, submit, new Fence(null));

            if (result != Result.Success)
            {
                //ResultException.Throw(result, "Error submitting queue");
            }
        }
        public static void WaitIdle(Queue queue)
        {
            vk.QueueWaitIdle(queue);
        }

        public static void OneTimeSubmit(VkCommandBuffer commandBuffer, Queue queue, Action<VkCommandBuffer> func)
        {
            commandBuffer.Begin(new(flags: CommandBufferUsageFlags.CommandBufferUsageOneTimeSubmitBit));
            func(commandBuffer);
            commandBuffer.End();

            CommandBuffer buffer = commandBuffer;

            SubmitInfo submitInfo = new(commandBufferCount: 1, pCommandBuffers: &buffer);
            Submit(queue, submitInfo, null);
            WaitIdle(queue);
        }

        public static void OneTimeSubmit(Device device, CommandPool commandPool, Queue queue, Action<VkCommandBuffer> func)
        {
            VkCommandBuffers commandBuffers = new(device,
                new(commandPool: commandPool, level: CommandBufferLevel.Primary, commandBufferCount: 1));
            var commandBuffer = commandBuffers.FirstOrDefault();

            OneTimeSubmit(commandBuffer, queue, func);

            commandBuffer.Dispose();
        }

        public static void CopyToDevice<T>(VkDeviceMemory deviceMemory, Span<T> data, uint stride = 0) where T : struct
        {
            Debug.Assert(!data.IsEmpty);

            uint elemSize = (uint)Marshal.SizeOf(data[0]);

            stride = stride > 0 ? stride : elemSize;
            Debug.Assert(elemSize <= stride);

            byte* dataPtr = (byte*)Unsafe.AsPointer(ref data.GetPinnableReference());

            byte* deviceData = (byte*)deviceMemory.MapMemory(0, (ulong)data.Length * stride);
            if (stride == elemSize)
            {
                ulong sizeBytes = (ulong)data.Length * stride;

                System.Buffer.MemoryCopy(dataPtr, deviceData, sizeBytes, sizeBytes);
            }
            else
            {
                for (int i = 0; i < data.Length; i++)
                {
                    System.Buffer.MemoryCopy(dataPtr, deviceData, elemSize, elemSize);
                    dataPtr += elemSize;
                    deviceData += stride;
                }
            }

            deviceMemory.UnmapMemory();
        }


        public static void SetImageLayout(VkCommandBuffer commandBuffer, Image image, Format format, ImageLayout oldImageLayout, ImageLayout newImageLayout)
        {
            AccessFlags sourceAccessMask = 0;

            switch (oldImageLayout)
            {
                case ImageLayout.TransferDstOptimal:
                    {
                        sourceAccessMask = AccessFlags.AccessTransferWriteBit;
                        break;
                    }
                case ImageLayout.Preinitialized:
                    {
                        sourceAccessMask = AccessFlags.AccessHostWriteBit;
                        break;
                    }
                case ImageLayout.ColorAttachmentOptimal:
                    {
                        sourceAccessMask = AccessFlags.AccessColorAttachmentWriteBit;
                        break;
                    }
                case ImageLayout.General:  // sourceAccessMask is empty
                case ImageLayout.Undefined:
                    {
                        break;
                    }
                default:
                    {
                        Debug.Assert(false);
                        break;
                    }
            }

            PipelineStageFlags sourceStage = 0;

            switch (oldImageLayout)
            {
                case ImageLayout.General:
                case ImageLayout.Preinitialized:
                    {
                        sourceStage = PipelineStageFlags.PipelineStageHostBit;
                        break;
                    }
                case ImageLayout.TransferDstOptimal:
                    {
                        sourceStage = PipelineStageFlags.PipelineStageTransferBit;
                        break;
                    }
                case ImageLayout.Undefined:
                    {
                        sourceStage = PipelineStageFlags.PipelineStageTopOfPipeBit;
                        break;
                    }
                default:
                    {
                        Debug.Assert(false);
                        break;
                    }
            }

            AccessFlags destinationAccessMask = 0;

            switch (newImageLayout)
            {
                case ImageLayout.ColorAttachmentOptimal:
                    {
                        destinationAccessMask = AccessFlags.AccessColorAttachmentWriteBit;
                        break;
                    }
                case ImageLayout.DepthStencilAttachmentOptimal:
                    {
                        destinationAccessMask = AccessFlags.AccessDepthStencilAttachmentReadBit | AccessFlags.AccessDepthStencilAttachmentWriteBit;
                        break;
                    }
                case ImageLayout.General:  // empty destinationAccessMask
                case ImageLayout.PresentSrcKhr:
                    {
                        break;
                    }
                case ImageLayout.ShaderReadOnlyOptimal:
                    {
                        destinationAccessMask = AccessFlags.AccessShaderReadBit;
                        break;
                    }
                case ImageLayout.TransferSrcOptimal:
                    {
                        destinationAccessMask = AccessFlags.AccessTransferReadBit;
                        break;
                    }
                case ImageLayout.TransferDstOptimal:
                    {
                        destinationAccessMask = AccessFlags.AccessTransferWriteBit;
                        break;
                    }
                default:
                    {
                        Debug.Assert(false);
                        break;
                    }
            }

            PipelineStageFlags destinationStage = 0;

            switch (newImageLayout)
            {
                case ImageLayout.ColorAttachmentOptimal:
                    {
                        destinationStage = PipelineStageFlags.PipelineStageColorAttachmentOutputBit;
                        break;
                    }
                case ImageLayout.DepthStencilAttachmentOptimal:
                    {
                        destinationStage = PipelineStageFlags.PipelineStageEarlyFragmentTestsBit;
                        break;
                    }
                case ImageLayout.General:
                    {
                        destinationStage = PipelineStageFlags.PipelineStageHostBit;
                        break;
                    }
                case ImageLayout.PresentSrcKhr:
                    {
                        destinationStage = PipelineStageFlags.PipelineStageBottomOfPipeBit;
                        break;
                    }
                case ImageLayout.ShaderReadOnlyOptimal:
                    {
                        destinationStage = PipelineStageFlags.PipelineStageFragmentShaderBit;
                        break;
                    }
                case ImageLayout.TransferDstOptimal:
                case ImageLayout.TransferSrcOptimal:
                    {
                        destinationStage = PipelineStageFlags.PipelineStageTransferBit;
                        break;
                    }
                default:
                    {
                        Debug.Assert(false);
                        break;
                    }
            }

            ImageAspectFlags aspectMask = ImageAspectFlags.ImageAspectColorBit;

            if (newImageLayout == ImageLayout.DepthStencilAttachmentOptimal)
            {
                aspectMask = ImageAspectFlags.ImageAspectDepthBit;

                if (format == Format.D32SfloatS8Uint || format == Format.D24UnormS8Uint)
                {
                    aspectMask |= ImageAspectFlags.ImageAspectStencilBit;
                }
            }

            ImageSubresourceRange imageSubresourceRange = new(aspectMask, 0, 1, 0, 1);
            ImageMemoryBarrier imageMemoryBarrier = new
            (
                srcAccessMask: sourceAccessMask,
                dstAccessMask: destinationAccessMask,
                oldLayout: oldImageLayout,
                newLayout: newImageLayout,
                srcQueueFamilyIndex: Vk.QueueFamilyIgnored,
                dstQueueFamilyIndex: Vk.QueueFamilyIgnored,
                image: image,
                subresourceRange: imageSubresourceRange
            );

            var imageMemoryBarriers = new ReadOnlySpan<ImageMemoryBarrier>(new[] { imageMemoryBarrier });

            commandBuffer.PipelineBarrier(sourceStage, destinationStage, 0, null, null, imageMemoryBarriers);
        }

        public static Pipeline MakeGraphicsPipeline(Device device,
                                        PipelineCache pipelineCache,
                                        ShaderModule vertexShaderModule,
                                        SpecializationInfo vertexShaderSpecializationInfo,
                                        ShaderModule fragmentShaderModule,
                                        SpecializationInfo fragmentShaderSpecializationInfo,
                                        uint vertexStride,
                                        (Format, uint)[] vertexInputAttributeFormatOffset,
                                        FrontFace frontFace,
                                        bool depthBuffered,
                                        PipelineLayout pipelineLayout,
                                        RenderPass renderPass)
        {


            PipelineShaderStageCreateInfo* pipelineShaderStageCreateInfos = stackalloc[]
            {
                new PipelineShaderStageCreateInfo(
                    stage: ShaderStageFlags.ShaderStageVertexBit,
                    module: vertexShaderModule,
                    pName: (byte*)SilkMarshal.StringToPtr("main"),
                    pSpecializationInfo: &vertexShaderSpecializationInfo),

                new PipelineShaderStageCreateInfo(
                    stage: ShaderStageFlags.ShaderStageFragmentBit,
                    module: fragmentShaderModule,
                    pName: (byte*)SilkMarshal.StringToPtr("main"),
                    pSpecializationInfo: &fragmentShaderSpecializationInfo)
            };

            PipelineVertexInputStateCreateInfo pipelineVertexInputStateCreateInfo = new(flags: 0);
            VertexInputBindingDescription vertexInputBindingDescription = new(0, vertexStride);

            VertexInputAttributeDescription* vertexInputAttributeDescriptions = null;

            if (vertexStride > 0)
            {
                vertexInputAttributeDescriptions = (VertexInputAttributeDescription*)Mem.AllocArray<VertexInputAttributeDescription>(vertexInputAttributeFormatOffset.Length);
                for (uint i = 0; i < vertexInputAttributeFormatOffset.Length; i++)
                {
                    vertexInputAttributeDescriptions[i] = new
                    (
                        location: i,
                        binding: 0,
                        format: vertexInputAttributeFormatOffset[i].Item1,
                        offset: vertexInputAttributeFormatOffset[i].Item2
                    );
                }

                pipelineVertexInputStateCreateInfo.VertexBindingDescriptionCount = 1;
                pipelineVertexInputStateCreateInfo.PVertexBindingDescriptions = &vertexInputBindingDescription;

                pipelineVertexInputStateCreateInfo.VertexAttributeDescriptionCount = (uint)vertexInputAttributeFormatOffset.Length;
                pipelineVertexInputStateCreateInfo.PVertexAttributeDescriptions = vertexInputAttributeDescriptions;
            }

            PipelineInputAssemblyStateCreateInfo pipelineInputAssemblyStateCreateInfo = new(topology: PrimitiveTopology.TriangleList);

            PipelineViewportStateCreateInfo pipelineViewportStateCreateInfo = new
            (
                viewportCount: 1,
                pViewports: null,
                scissorCount: 1,
                pScissors: null
            );

            PipelineRasterizationStateCreateInfo pipelineRasterizationStateCreateInfo = new
            (
                depthClampEnable: false,
                rasterizerDiscardEnable: false,
                polygonMode: PolygonMode.Fill,
                cullMode: CullModeFlags.CullModeBackBit,
                frontFace: frontFace,
                depthBiasEnable: false,
                depthBiasConstantFactor: 0.0f,
                depthBiasClamp: 0.0f,
                depthBiasSlopeFactor: 0.0f,
                lineWidth: 1.0f
            );

            PipelineMultisampleStateCreateInfo pipelineMultisampleStateCreateInfo = new(rasterizationSamples: SampleCountFlags.SampleCount1Bit);

            StencilOpState stencilOpState = new(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep, CompareOp.Always);

            PipelineDepthStencilStateCreateInfo pipelineDepthStencilStateCreateInfo = new
            (
                depthTestEnable: depthBuffered,
                depthWriteEnable: depthBuffered,
                depthCompareOp: CompareOp.LessOrEqual,
                depthBoundsTestEnable: false,
                stencilTestEnable: false,
                front: stencilOpState,
                back: stencilOpState
            );

            ColorComponentFlags colorComponentFlags =
                ColorComponentFlags.ColorComponentRBit |
                ColorComponentFlags.ColorComponentGBit |
                ColorComponentFlags.ColorComponentBBit |
                ColorComponentFlags.ColorComponentABit;

            PipelineColorBlendAttachmentState pipelineColorBlendAttachmentState = new(
                false,
                BlendFactor.Zero,
                BlendFactor.Zero,
                BlendOp.Add,
                BlendFactor.Zero,
                BlendFactor.Zero,
                BlendOp.Add,
                colorComponentFlags);

            PipelineColorBlendStateCreateInfo pipelineColorBlendStateCreateInfo = new
            (
                logicOpEnable: false,
                logicOp: LogicOp.NoOp,
                attachmentCount: 1,
                pAttachments: &pipelineColorBlendAttachmentState
            );

            pipelineColorBlendStateCreateInfo.BlendConstants[0] = 0.0f;
            pipelineColorBlendStateCreateInfo.BlendConstants[1] = 0.0f;
            pipelineColorBlendStateCreateInfo.BlendConstants[2] = 0.0f;
            pipelineColorBlendStateCreateInfo.BlendConstants[3] = 0.0f;

            DynamicState* dynamicStates = stackalloc[] { DynamicState.Viewport, DynamicState.Scissor };

            PipelineDynamicStateCreateInfo pipelineDynamicStateCreateInfo = new
            (
                dynamicStateCount: 2,
                pDynamicStates: dynamicStates
            );

            GraphicsPipelineCreateInfo graphicsPipelineCreateInfo = new
            (
                stageCount: 2,
                pStages: pipelineShaderStageCreateInfos,
                pVertexInputState: &pipelineVertexInputStateCreateInfo,
                pInputAssemblyState: &pipelineInputAssemblyStateCreateInfo,
                pViewportState: &pipelineViewportStateCreateInfo,
                pRasterizationState: &pipelineRasterizationStateCreateInfo,
                pMultisampleState: &pipelineMultisampleStateCreateInfo,
                pDepthStencilState: &pipelineDepthStencilStateCreateInfo,
                pColorBlendState: &pipelineColorBlendStateCreateInfo,
                pDynamicState: &pipelineDynamicStateCreateInfo,
                layout: pipelineLayout,
                renderPass: renderPass
            );

            vk.CreateGraphicsPipelines(device, new PipelineCache(null), 1, &graphicsPipelineCreateInfo, null, out Pipeline pipeLine);


            SilkMarshal.Free((nint)pipelineShaderStageCreateInfos[0].PName);
            SilkMarshal.Free((nint)pipelineShaderStageCreateInfos[1].PName);

            Mem.FreeArray(vertexInputAttributeDescriptions);

            return pipeLine;
        }

        public static DescriptorSetLayout MakeDescriptorSetLayout(Device device,
                                              (DescriptorType descriptorType, int descriptorCount, ShaderStageFlags shaderStageFlags)[] bindingData,
                                              DescriptorSetLayoutCreateFlags flags = 0)
        {
            DescriptorSetLayoutBinding* bindings = (DescriptorSetLayoutBinding*)Mem.AllocArray<DescriptorSetLayoutBinding>(bindingData.Length);

            for (uint i = 0; i < bindingData.Length; i++)
            {
                bindings[i] = new DescriptorSetLayoutBinding
                (
                    i,
                    bindingData[i].descriptorType,
                    (uint)bindingData[i].descriptorCount,
                    bindingData[i].shaderStageFlags
                );
            }

            DescriptorSetLayoutCreateInfo descriptorSetLayoutCreateInfo = new
            (
                flags: flags,
                bindingCount: (uint)bindingData.Length,
                pBindings: bindings
            );

            vk.CreateDescriptorSetLayout(device, &descriptorSetLayoutCreateInfo, null, out DescriptorSetLayout descriptorSetLayout);

            Mem.FreeArray(bindings);

            return descriptorSetLayout;
        }

        public static Format PickDepthFormat(PhysicalDevice physicalDevice)
        {
            Format[] candidates = new[] { Format.D32Sfloat, Format.D32SfloatS8Uint, Format.D24UnormS8Uint };
            foreach (Format format in candidates)
            {
                FormatProperties props = GetFormatProperties(physicalDevice, format);

                if (props.OptimalTilingFeatures.HasFlag(FormatFeatureFlags.FormatFeatureDepthStencilAttachmentBit))
                {
                    return format;
                }
            }
            throw new Exception("failed to find supported format!");
        }

        public static RenderPass MakeRenderPass(Device device,
                                    Format colorFormat,
                                    Format depthFormat,
                                    AttachmentLoadOp loadOp = AttachmentLoadOp.Clear,
                                    ImageLayout colorFinalLayout = ImageLayout.PresentSrcKhr)
        {
            List<AttachmentDescription> attachmentDescriptions = new();

            Debug.Assert(colorFormat != Format.Undefined);

            attachmentDescriptions.Add(new()
            {
                Format = colorFormat,
                Samples = SampleCountFlags.SampleCount1Bit,
                LoadOp = loadOp,
                StoreOp = AttachmentStoreOp.Store,
                StencilLoadOp = AttachmentLoadOp.DontCare,
                StencilStoreOp = AttachmentStoreOp.DontCare,
                InitialLayout = ImageLayout.Undefined,
                FinalLayout = colorFinalLayout,
            });

            if (depthFormat != Format.Undefined)
            {
                attachmentDescriptions.Add(new()
                {
                    Format = depthFormat,
                    Samples = SampleCountFlags.SampleCount1Bit,
                    LoadOp = loadOp,
                    StoreOp = AttachmentStoreOp.DontCare,
                    StencilLoadOp = AttachmentLoadOp.DontCare,
                    StencilStoreOp = AttachmentStoreOp.DontCare,
                    InitialLayout = ImageLayout.Undefined,
                    FinalLayout = ImageLayout.DepthStencilAttachmentOptimal,
                });

            }
            AttachmentReference colorAttachment = new(0, ImageLayout.ColorAttachmentOptimal);
            AttachmentReference depthAttachment = new(1, ImageLayout.DepthStencilAttachmentOptimal);

            SubpassDescription subpassDescription = new()
            {
                PipelineBindPoint = PipelineBindPoint.Graphics,
                ColorAttachmentCount = 1,
                PColorAttachments = &colorAttachment,
                PDepthStencilAttachment = (depthFormat != Format.Undefined) ? &depthAttachment : null
            };

            AttachmentDescription* attachments = (AttachmentDescription*)Mem.AllocArray<AttachmentDescription>(attachmentDescriptions.Count);

            for (int i = 0; i < attachmentDescriptions.Count; i++)
            {
                attachments[i] = attachmentDescriptions[i];
            }

            RenderPassCreateInfo renderPassCreateInfo = new
            (
                attachmentCount: (uint)attachmentDescriptions.Count,
                pAttachments: attachments,
                subpassCount: 1,
                pSubpasses: &subpassDescription
            );


            vk.CreateRenderPass(device, renderPassCreateInfo, null, out RenderPass renderPass);

            Mem.FreeArray(attachments);

            return renderPass;
        }

        public static Framebuffer[] MakeFramebuffers(Device device,
                                                  RenderPass renderPass,
                                                  ImageView[] imageViews,
                                                  ImageView? depthImageView,
                                                  Extent2D extent)
        {
            ImageView* attachments = stackalloc ImageView[2];
            attachments[1] = depthImageView.Value;

            FramebufferCreateInfo framebufferInfo = new
            (
                 renderPass: renderPass,
                 attachmentCount: depthImageView != null ? (uint)2 : (uint)1,
                 pAttachments: attachments,
                 width: extent.Width,
                 height: extent.Height,
                 layers: 1
            );

            Framebuffer[] framebuffers = new Framebuffer[imageViews.Length];

            for (int i = 0; i < imageViews.Length; i++)
            {
                var FrameBuffer = new Framebuffer();
                attachments[0] = imageViews[i];
                vk.CreateFramebuffer(device, framebufferInfo, null, out FrameBuffer);
                framebuffers[i] = FrameBuffer;
            }

            return framebuffers;
        }

        /// <summary>
        /// Shader resources are accessable in descriptor sets.
        /// Descriptor sets are created from a descriptor pool which contains maxSets Descriptor sets.
        /// Each resourcetype required in a set and the total amount of the resource used over all sets (DescriptorCount)
        /// are specified when creating the descriptor pool.
        /// Each descriptor set requires a descriptor layout which defines how the resources are ordered in the set
        /// </summary>
        /// <param name="device"></param>
        /// <param name="poolSizes"></param>
        /// <returns></returns>
        public static DescriptorPool MakeDescriptorPool(Device device, DescriptorPoolSize[] poolSizes)
        {
            Debug.Assert(poolSizes.Length > 0);

            // maxSets parameter specifies how many descriptor sets can be allocated from a given pool
            uint maxSets = poolSizes.Aggregate<DescriptorPoolSize, uint>(0, (sum, dps) => sum + dps.DescriptorCount);

            Debug.Assert(maxSets > 0);

            fixed (DescriptorPoolSize* poolSizesPtr = &poolSizes[0])
            {
                DescriptorPoolCreateInfo descriptorPoolCreateInfo = new
                (
                    poolSizeCount: (uint)poolSizes.Length,
                    pPoolSizes: poolSizesPtr,
                    maxSets: maxSets
                );

                vk.CreateDescriptorPool(device, descriptorPoolCreateInfo, null, out DescriptorPool descriptorPool);
                return descriptorPool;
            }
        }

        public static void UpdateDescriptorSets(Device device, ReadOnlySpan<WriteDescriptorSet> descriptorWrites, ReadOnlySpan<CopyDescriptorSet> descriptorCopies)
        {
            vk.UpdateDescriptorSets(device, descriptorWrites, descriptorCopies);
        }
        public static void UpdateDescriptorSets(Device device,
        DescriptorSet descriptorSet,
            (DescriptorType descriptorType, VkBuffer buffer, ulong range, VkBufferView bufferView)[] bufferData,
            Texture texture,
            uint bindingOffset = 0)
        {

            WriteDescriptorSet[] writeDescriptorSets = new WriteDescriptorSet[bufferData.Length + 1];
            DescriptorBufferInfo* bufferInfos = (DescriptorBufferInfo*)Mem.AllocArray<DescriptorBufferInfo>(bufferData.Length);
            BufferView* bufferViews = (BufferView*)Mem.AllocArray<BufferView>(bufferData.Length);

            uint dstBinding = bindingOffset;

            int i = 0;

            for (; i < bufferData.Length; i++)
            {
                bufferInfos[i] = new(bufferData[i].buffer, 0, bufferData[i].range);
                bufferViews[i] = bufferData[i].bufferView ?? new BufferView(null);

                writeDescriptorSets[i] = new
                (
                    dstSet: descriptorSet,
                    dstBinding: dstBinding++,
                    dstArrayElement: 0,
                    descriptorCount: 1,
                    descriptorType: bufferData[i].descriptorType,
                    pBufferInfo: &bufferInfos[i],
                    pTexelBufferView: &bufferViews[i]
                );
            }

            DescriptorImageInfo imageInfo = new(texture.Sampler, texture.View, ImageLayout.ShaderReadOnlyOptimal);

            writeDescriptorSets[i] = new
            (
                dstSet: descriptorSet,
                dstBinding: dstBinding,
                dstArrayElement: 0,
                descriptorCount: 1,
                descriptorType: DescriptorType.CombinedImageSampler,
                pImageInfo: &imageInfo
            );

            UpdateDescriptorSets(device, writeDescriptorSets, null);
            Mem.FreeArray(bufferInfos);
            Mem.FreeArray(bufferViews);
        }
        public static void UpdateDescriptorSets(Device device,
        DescriptorSet descriptorSet,
            (DescriptorType descriptorType, VkBuffer buffer, ulong range, VkBufferView bufferView)[] bufferData,
            TextureData[] textureData,
            uint bindingOffset = 0)
        {
            WriteDescriptorSet[] writeDescriptorSets = new WriteDescriptorSet[bufferData.Length + textureData.Length];

            DescriptorBufferInfo* bufferInfos = (DescriptorBufferInfo*)Mem.AllocArray<DescriptorBufferInfo>(bufferData.Length);
            BufferView* bufferViews = (BufferView*)Mem.AllocArray<BufferView>(bufferData.Length);
            DescriptorImageInfo* imageInfos = (DescriptorImageInfo*)Mem.AllocArray<DescriptorImageInfo>(textureData.Length);

            uint dstBinding = bindingOffset;

            int i = 0;

            for (; i < bufferData.Length; i++)
            {
                bufferInfos[i] = new(bufferData[i].buffer, 0, bufferData[i].range);
                bufferViews[i] = bufferData[i].bufferView ?? new BufferView(null);

                writeDescriptorSets[i] = new
                (
                    dstSet: descriptorSet,
                    dstBinding: dstBinding++,
                    dstArrayElement: 0,
                    descriptorCount: 1,
                    descriptorType: bufferData[i].descriptorType,
                    pBufferInfo: &bufferInfos[i],
                    pTexelBufferView: &bufferViews[i]
                );

            }

            for (int j = 0; i < writeDescriptorSets.Length; i++, j++)
            {
                imageInfos[j] = new(textureData[j].sampler,
                    textureData[j].imageData.imageView,
                    ImageLayout.ShaderReadOnlyOptimal);

                writeDescriptorSets[i] = new
                (
                    dstSet: descriptorSet,
                    dstBinding: dstBinding++,
                    dstArrayElement: 0,
                    descriptorCount: 1,
                    descriptorType: DescriptorType.CombinedImageSampler,
                    pImageInfo: &imageInfos[j]
                );
            }

            UpdateDescriptorSets(device, writeDescriptorSets, null);

            Mem.FreeArray(bufferInfos);
            Mem.FreeArray(bufferViews);
            Mem.FreeArray(imageInfos);
        }

        public static ShaderModule CreateShaderModule(Device device, string filename)
        {
            ShaderModule shader;
            byte[] code = File.ReadAllBytes(filename);

            fixed (byte* codePtr = code)
            {
                ShaderModuleCreateInfo createInfo = new
                (
                    codeSize: (nuint)code.Length,
                    pCode: (uint*)codePtr
                );

                Result result = vk.CreateShaderModule(device, createInfo, null, out shader);

                if (result != Result.Success)
                {
                    //ResultException.Throw(result, "Error creating shader module");
                }

                return shader;
            }
        }
    }
}
