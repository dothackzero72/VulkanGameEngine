using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor
{
    // Vulkan Structs
    [StructLayout(LayoutKind.Sequential)]
    public struct VkExtent2D
    {
        public uint Width;
        public uint Height;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkExtent3D
    {
        public uint Width;
        public uint Height;
        public uint Depth;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkOffset2D
    {
        public int X;
        public int Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkOffset3D
    {
        public int X;
        public int Y;
        public int Z;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkRect2D
    {
        public VkOffset2D Offset;
        public VkExtent2D Extent;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkBaseInStructure
    {
        public VkStructureType sType;
        public IntPtr pNext; // Pointer for extension structures
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkBaseOutStructure
    {
        public VkStructureType sType;
        public IntPtr pNext; // Pointer for extension structures
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkBufferMemoryBarrier
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkAccessFlags srcAccessMask;
        public VkAccessFlags dstAccessMask;
        public uint srcQueueFamilyIndex;
        public uint dstQueueFamilyIndex;
        public VkBuffer buffer;
        public VkDeviceSize offset;
        public VkDeviceSize size;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkDispatchIndirectCommand
    {
        public uint X;
        public uint Y;
        public uint Z;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkDrawIndexedIndirectCommand
    {
        public uint indexCount;
        public uint instanceCount;
        public uint firstIndex;
        public int vertexOffset;
        public uint firstInstance;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkDrawIndirectCommand
    {
        public uint vertexCount;
        public uint instanceCount;
        public uint firstVertex;
        public uint firstInstance;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkImageSubresourceRange
    {
        public VkImageAspectFlags aspectMask;
        public uint baseMipLevel;
        public uint levelCount;
        public uint baseArrayLayer;
        public uint layerCount;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkImageMemoryBarrier
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkAccessFlags srcAccessMask;
        public VkAccessFlags dstAccessMask;
        public VkImageLayout oldLayout;
        public VkImageLayout newLayout;
        public uint srcQueueFamilyIndex;
        public uint dstQueueFamilyIndex;
        public VkImage image;
        public VkImageSubresourceRange subresourceRange;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkMemoryBarrier
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkAccessFlags srcAccessMask;
        public VkAccessFlags dstAccessMask;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkPipelineCacheHeaderVersionOne
    {
        public uint headerSize;
        public VkPipelineCacheHeaderVersion headerVersion;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = VulkanConsts.VK_UUID_SIZE)]
        public byte[] pipelineCacheUUID;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkAllocationCallbacks
    {
        public IntPtr pUserData;
        public IntPtr pfnAllocation;
        public IntPtr pfnReallocation;
        public IntPtr pfnFree;
        public IntPtr pfnInternalAllocation;
        public IntPtr pfnInternalFree;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkApplicationInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string pApplicationName;
        public uint applicationVersion;
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string pEngineName;
        public uint engineVersion;
        public uint apiVersion;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkFormatProperties
    {
        public VkFormatFeatureFlags linearTilingFeatures;
        public VkFormatFeatureFlags optimalTilingFeatures;
        public VkFormatFeatureFlags bufferFeatures;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkImageFormatProperties
    {
        public VkExtent3D maxExtent;
        public uint maxMipLevels;
        public uint maxArrayLayers;
        public VkSampleCountFlags sampleCounts;
        public VkDeviceSize maxResourceSize;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkInstanceCreateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkInstanceCreateFlags flags;
        public IntPtr pApplicationInfo; // Pointer to VkApplicationInfo
        public uint enabledLayerCount;
        public IntPtr pEnabledLayerNames; // Pointer to array of layer names
        public uint enabledExtensionCount;
        public IntPtr pEnabledExtensionNames; // Pointer to array of extension names
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkMemoryHeap
    {
        public VkDeviceSize size;
        public VkMemoryHeapFlags flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkMemoryType
    {
        public VkMemoryPropertyFlagBits propertyFlags;
        public uint heapIndex;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkPhysicalDeviceFeatures 
    {
        public VkBool32 robustBufferAccess;
        public VkBool32 fullDrawIndexUint32;
        public VkBool32 imageCubeArray;
        public VkBool32 independentBlend;
        public VkBool32 geometryShader;
        public VkBool32 tessellationShader;
        public VkBool32 sampleRateShading;
        public VkBool32 dualSrcBlend;
        public VkBool32 logicOp;
        public VkBool32 multiDrawIndirect;
        public VkBool32 drawIndirectFirstInstance;
        public VkBool32 depthClamp;
        public VkBool32 depthBiasClamp;
        public VkBool32 fillModeNonSolid;
        public VkBool32 depthBounds;
        public VkBool32 wideLines;
        public VkBool32 largePoints;
        public VkBool32 alphaToOne;
        public VkBool32 multiViewport;
        public VkBool32 samplerAnisotropy;
        public VkBool32 textureCompressionETC2;
        public VkBool32 textureCompressionASTC_LDR;
        public VkBool32 textureCompressionBC;
        public VkBool32 occlusionQueryPrecise;
        public VkBool32 pipelineStatisticsQuery;
        public VkBool32 vertexPipelineStoresAndAtomics;
        public VkBool32 fragmentStoresAndAtomics;
        public VkBool32 shaderTessellationAndGeometryPointSize;
        public VkBool32 shaderImageGatherExtended;
        public VkBool32 shaderStorageImageExtendedFormats;
        public VkBool32 shaderStorageImageMultisample;
        public VkBool32 shaderStorageImageReadWithoutFormat;
        public VkBool32 shaderStorageImageWriteWithoutFormat;
        public VkBool32 shaderUniformBufferArrayDynamicIndexing;
        public VkBool32 shaderSampledImageArrayDynamicIndexing;
        public VkBool32 shaderStorageBufferArrayDynamicIndexing;
        public VkBool32 shaderStorageImageArrayDynamicIndexing;
        public VkBool32 shaderClipDistance;
        public VkBool32 shaderCullDistance;
        public VkBool32 shaderFloat64;
        public VkBool32 shaderInt64;
        public VkBool32 shaderInt16;
        public VkBool32 shaderResourceResidency;
        public VkBool32 shaderResourceMinLod;
        public VkBool32 sparseBinding;
        public VkBool32 sparseResidencyBuffer;
        public VkBool32 sparseResidencyImage2D;
        public VkBool32 sparseResidencyImage3D;
        public VkBool32 sparseResidency2Samples;
        public VkBool32 sparseResidency4Samples;
        public VkBool32 sparseResidency8Samples;
        public VkBool32 sparseResidency16Samples;
        public VkBool32 sparseResidencyAliased;
        public VkBool32 variableMultisampleRate;
        public VkBool32 inheritedQueries;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkPhysicalDeviceLimits
    {
        public uint maxImageDimension1D;
        public uint maxImageDimension2D;
        public uint maxImageDimension3D;
        public uint maxImageDimensionCube;
        public uint maxImageArrayLayers;
        public uint maxTexelBufferElements;
        public uint maxUniformBufferRange;
        public uint maxStorageBufferRange;
        public uint maxPushConstantsSize;
        public uint maxMemoryAllocationCount;
        public uint maxSamplerAllocationCount;
        public VkDeviceSize bufferImageGranularity;
        public VkDeviceSize sparseAddressSpaceSize;
        public uint maxBoundDescriptorSets;
        public uint maxPerStageDescriptorSamplers;
        public uint maxPerStageDescriptorUniformBuffers;
        public uint maxPerStageDescriptorStorageBuffers;
        public uint maxPerStageDescriptorSampledImages;
        public uint maxPerStageDescriptorStorageImages;
        public uint maxPerStageDescriptorInputAttachments;
        public uint maxPerStageResources;
        public uint maxDescriptorSetSamplers;
        public uint maxDescriptorSetUniformBuffers;
        public uint maxDescriptorSetUniformBuffersDynamic;
        public uint maxDescriptorSetStorageBuffers;
        public uint maxDescriptorSetStorageBuffersDynamic;
        public uint maxDescriptorSetSampledImages;
        public uint maxDescriptorSetStorageImages;
        public uint maxDescriptorSetInputAttachments;
        public uint maxVertexInputAttributes;
        public uint maxVertexInputBindings;
        public uint maxVertexInputAttributeOffset;
        public uint maxVertexInputBindingStride;
        public uint maxVertexOutputComponents;
        public uint maxTessellationGenerationLevel;
        public uint maxTessellationPatchSize;
        public uint maxTessellationControlPerVertexInputComponents;
        public uint maxTessellationControlPerVertexOutputComponents;
        public uint maxTessellationControlPerPatchOutputComponents;
        public uint maxTessellationControlTotalOutputComponents;
        public uint maxTessellationEvaluationInputComponents;
        public uint maxTessellationEvaluationOutputComponents;
        public uint maxGeometryShaderInvocations;
        public uint maxGeometryInputComponents;
        public uint maxGeometryOutputComponents;
        public uint maxGeometryOutputVertices;
        public uint maxGeometryTotalOutputComponents;
        public uint maxFragmentInputComponents;
        public uint maxFragmentOutputAttachments;
        public uint maxFragmentDualSrcAttachments;
        public uint maxFragmentCombinedOutputResources;
        public uint maxComputeSharedMemorySize;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public uint[] maxComputeWorkGroupCount;
        public uint maxComputeWorkGroupInvocations;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public uint[] maxComputeWorkGroupSize;
        public uint subPixelPrecisionBits;
        public uint subTexelPrecisionBits;
        public uint mipmapPrecisionBits;
        public uint maxDrawIndexedIndexValue;
        public uint maxDrawIndirectCount;
        public float maxSamplerLodBias;
        public float maxSamplerAnisotropy;
        public uint maxViewports;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public uint[] maxViewportDimensions;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public float[] viewportBoundsRange;
        public uint viewportSubPixelBits;
        public IntPtr minMemoryMapAlignment;
        public VkDeviceSize minTexelBufferOffsetAlignment;
        public VkDeviceSize minUniformBufferOffsetAlignment;
        public VkDeviceSize minStorageBufferOffsetAlignment;
        public int minTexelOffset;
        public uint maxTexelOffset;
        public int minTexelGatherOffset;
        public uint maxTexelGatherOffset;
        public float minInterpolationOffset;
        public float maxInterpolationOffset;
        public uint subPixelInterpolationOffsetBits;
        public uint maxFramebufferWidth;
        public uint maxFramebufferHeight;
        public uint maxFramebufferLayers;
        public VkSampleCountFlags framebufferColorSampleCounts;
        public VkSampleCountFlags framebufferDepthSampleCounts;
        public VkSampleCountFlags framebufferStencilSampleCounts;
        public VkSampleCountFlags framebufferNoAttachmentsSampleCounts;
        public uint maxColorAttachments;
        public VkSampleCountFlags sampledImageColorSampleCounts;
        public VkSampleCountFlags sampledImageIntegerSampleCounts;
        public VkSampleCountFlags sampledImageDepthSampleCounts;
        public VkSampleCountFlags sampledImageStencilSampleCounts;
        public VkSampleCountFlags storageImageSampleCounts;
        public uint maxSampleMaskWords;
        public VkBool32 timestampComputeAndGraphics;
        public float timestampPeriod;
        public uint maxClipDistances;
        public uint maxCullDistances;
        public uint maxCombinedClipAndCullDistances;
        public uint discreteQueuePriorities;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public float[] pointSizeRange;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public float[] lineWidthRange;
        public float pointSizeGranularity;
        public float lineWidthGranularity;
        public VkBool32 strictLines;
        public VkBool32 standardSampleLocations;
        public VkDeviceSize optimalBufferCopyOffsetAlignment;
        public VkDeviceSize optimalBufferCopyRowPitchAlignment;
        public VkDeviceSize nonCoherentAtomSize;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkPhysicalDeviceMemoryProperties
    {
        public uint memoryTypeCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = VulkanConsts.VK_MAX_MEMORY_TYPES)]
        public VkMemoryType[] memoryTypes;
        public uint memoryHeapCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = VulkanConsts.VK_MAX_MEMORY_HEAPS)]
        public VkMemoryHeap[] memoryHeaps;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkPhysicalDeviceSparseProperties
    {
        public VkBool32 residencyStandard2DBlockShape;
        public VkBool32 residencyStandard2DMultisampleBlockShape;
        public VkBool32 residencyStandard3DBlockShape;
        public VkBool32 residencyAlignedMipSize;
        public VkBool32 residencyNonResidentStrict;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkPhysicalDeviceProperties
    {
        public uint apiVersion;
        public uint driverVersion;
        public uint vendorID;
        public uint deviceID;
        public VkPhysicalDeviceType deviceType;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = VulkanConsts.VK_MAX_PHYSICAL_DEVICE_NAME_SIZE)]
        public string deviceName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = VulkanConsts.VK_UUID_SIZE)]
        public byte[] pipelineCacheUUID;
        public VkPhysicalDeviceLimits limits;
        public VkPhysicalDeviceSparseProperties sparseProperties;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkQueueFamilyProperties
    {
        public VkQueueFlagBits queueFlags;
        public uint queueCount;
        public uint timestampValidBits;
        public VkExtent3D minImageTransferGranularity;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkDeviceQueueCreateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkDeviceQueueCreateFlags flags;
        public uint queueFamilyIndex;
        public uint queueCount;
        public IntPtr pQueuePriorities; // Pointer to float array
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkDeviceCreateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkDeviceCreateFlags flags;
        public uint queueCreateInfoCount;
        public IntPtr pQueueCreateInfos; // Pointer to VkDeviceQueueCreateInfo array
        public uint enabledLayerCount;
        public IntPtr pEnabledLayerNames; // Pointer to array of layer names
        public uint enabledExtensionCount;
        public IntPtr pEnabledExtensionNames; // Pointer to array of extension names
        public IntPtr pEnabledFeatures; // Pointer to VkPhysicalDeviceFeatures
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkExtensionProperties
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = VulkanConsts.VK_MAX_EXTENSION_NAME_SIZE)]
        public string extensionName;
        public uint specVersion;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkLayerProperties
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = VulkanConsts.VK_MAX_EXTENSION_NAME_SIZE)]
        public string layerName;
        public uint specVersion;
        public uint implementationVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = VulkanConsts.VK_MAX_DESCRIPTION_SIZE)]
        public string description;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkSubmitInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public uint waitSemaphoreCount;
        public IntPtr pWaitSemaphores; // Pointer to array of VkSemaphore
        public IntPtr pWaitDstStageMask; // Pointer to array of VkPipelineStageFlags
        public uint commandBufferCount;
        public IntPtr pCommandBuffers; // Pointer to array of VkCommandBuffer
        public uint signalSemaphoreCount;
        public IntPtr pSignalSemaphores; // Pointer to array of VkSemaphore
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkMappedMemoryRange
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkDeviceMemory memory;
        public VkDeviceSize offset;
        public VkDeviceSize size;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkMemoryAllocateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkDeviceSize allocationSize;
        public uint memoryTypeIndex;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkMemoryRequirements
    {
        public VkDeviceSize size;
        public VkDeviceSize alignment;
        public uint memoryTypeBits;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkSparseMemoryBind
    {
        public VkDeviceSize resourceOffset;
        public VkDeviceSize size;
        public VkDeviceMemory memory;
        public VkDeviceSize memoryOffset;
        public VkSparseMemoryBindFlags flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkSparseBufferMemoryBindInfo
    {
        public VkBuffer buffer;
        public uint bindCount;
        public IntPtr pBinds; // Pointer to array of VkSparseMemoryBind
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkSparseImageOpaqueMemoryBindInfo
    {
        public VkImage image;
        public uint bindCount;
        public IntPtr pBinds; // Pointer to array of VkSparseMemoryBind
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkImageSubresource
    {
        public VkImageAspectFlags aspectMask;
        public uint mipLevel;
        public uint arrayLayer;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkSparseImageMemoryBind
    {
        public VkImageSubresource subresource;
        public VkOffset3D offset;
        public VkExtent3D extent;
        public VkDeviceMemory memory;
        public VkDeviceSize memoryOffset;
        public VkSparseMemoryBindFlags flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkSparseImageMemoryBindInfo
    {
        public VkImage image;
        public uint bindCount;
        public IntPtr pBinds; // Pointer to array of VkSparseImageMemoryBind
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkBindSparseInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public uint waitSemaphoreCount;
        public IntPtr pWaitSemaphores; // Pointer to VkSemaphore array
        public uint bufferBindCount;
        public IntPtr pBufferBinds; // Pointer to VkSparseBufferMemoryBindInfo array
        public uint imageOpaqueBindCount;
        public IntPtr pImageOpaqueBinds; // Pointer to VkSparseImageOpaqueMemoryBindInfo array
        public uint imageBindCount;
        public IntPtr pImageBinds; // Pointer to VkSparseImageMemoryBindInfo array
        public uint signalSemaphoreCount;
        public IntPtr pSignalSemaphores; // Pointer to VkSemaphore array
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkSparseImageFormatProperties
    {
        public VkImageAspectFlags aspectMask;
        public VkExtent3D imageGranularity;
        public VkSparseImageFormatFlags flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkSparseImageMemoryRequirements
    {
        public VkSparseImageFormatProperties formatProperties;
        public uint imageMipTailFirstLod;
        public VkDeviceSize imageMipTailSize;
        public VkDeviceSize imageMipTailOffset;
        public VkDeviceSize imageMipTailStride;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkFenceCreateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkFenceCreateFlags flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkSemaphoreCreateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkSemaphoreCreateFlags flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkEventCreateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkEventCreateFlags flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkQueryPoolCreateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkQueryPoolCreateFlags flags;
        public VkQueryType queryType;
        public uint queryCount;
        public VkQueryPipelineStatisticFlags pipelineStatistics;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkBufferCreateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkBufferCreateFlags flags;
        public VkDeviceSize size;
        public VkBufferUsageFlags usage;
        public VkSharingMode sharingMode;
        public uint queueFamilyIndexCount;
        public IntPtr pQueueFamilyIndices; // Pointer to array of uint
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkBufferViewCreateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkBufferViewCreateFlags flags;
        public VkBuffer buffer;
        public VkFormat format;
        public VkDeviceSize offset;
        public VkDeviceSize range;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkImageCreateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkImageCreateFlags flags;
        public VkImageType imageType;
        public VkFormat format;
        public VkExtent3D extent;
        public uint mipLevels;
        public uint arrayLayers;
        public VkSampleCountFlagBits samples;
        public VkImageTiling tiling;
        public VkImageUsageFlags usage;
        public VkSharingMode sharingMode;
        public uint queueFamilyIndexCount;
        public IntPtr pQueueFamilyIndices; // Pointer to array of uint
        public VkImageLayout initialLayout;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkSubresourceLayout
    {
        public VkDeviceSize offset;
        public VkDeviceSize size;
        public VkDeviceSize rowPitch;
        public VkDeviceSize arrayPitch;
        public VkDeviceSize depthPitch;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkComponentMapping
    {
        public VkComponentSwizzle r;
        public VkComponentSwizzle g;
        public VkComponentSwizzle b;
        public VkComponentSwizzle a;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkImageViewCreateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkImageViewCreateFlags flags;
        public VkImage image;
        public VkImageViewType viewType;
        public VkFormat format;
        public VkComponentMapping components;
        public VkImageSubresourceRange subresourceRange;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct VkShaderModuleCreateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkShaderModuleCreateFlags flags;
        public nuint codeSize; // Use UIntPtr for size_t
        public byte* pCode; // Pointer to uint array
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkPipelineCacheCreateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkPipelineCacheCreateFlags flags;
        public nuint initialDataSize; // size_t in C
        public IntPtr pInitialData; // Pointer to void
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkSpecializationMapEntry
    {
        public uint constantID;
        public uint offset;
        public nuint size; // size_t in C
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkSpecializationInfo
    {
        public uint mapEntryCount;
        public IntPtr pMapEntries; // Pointer to VkSpecializationMapEntry array
        public nuint dataSize; // size_t in C
        public IntPtr pData; // Pointer to void
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct VkPipelineShaderStageCreateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkPipelineShaderStageCreateFlags flags;
        public VkShaderStageFlagBits stage;
        public VkShaderModule module;
        public IntPtr pName; // Pointer to the name of the entry point, encoded as UTF-8
        public VkSpecializationInfo* pSpecializationInfo; // Pointer to VkSpecializationInfo or null
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkComputePipelineCreateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkPipelineCreateFlags flags;
        public VkPipelineShaderStageCreateInfo stage;
        public VkPipelineLayout layout;
        public VkPipeline basePipelineHandle;
        public int basePipelineIndex;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkVertexInputBindingDescription
    {
        public uint binding;
        public uint stride;
        public VkVertexInputRate inputRate;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkVertexInputAttributeDescription
    {
        public uint location;
        public uint binding;
        public VkFormat format;
        public uint offset;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkPipelineVertexInputStateCreateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkPipelineVertexInputStateCreateFlags flags;
        public uint vertexBindingDescriptionCount;
        public IntPtr pVertexBindingDescriptions; // Pointer to array of VkVertexInputBindingDescription
        public uint vertexAttributeDescriptionCount;
        public IntPtr pVertexAttributeDescriptions; // Pointer to array of VkVertexInputAttributeDescription
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkPipelineInputAssemblyStateCreateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkPipelineInputAssemblyStateCreateFlags flags;
        public VkPrimitiveTopology topology;
        public VkBool32 primitiveRestartEnable;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkPipelineTessellationStateCreateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkPipelineTessellationStateCreateFlags flags;
        public uint patchControlPoints;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkViewport
    {
        public float x;
        public float y;
        public float width;
        public float height;
        public float minDepth;
        public float maxDepth;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct VkPipelineViewportStateCreateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkPipelineViewportStateCreateFlags flags;
        public uint viewportCount;
        public VkViewport* pViewports; // Pointer to VkViewport array
        public uint scissorCount;
        public VkRect2D* pScissors; // Pointer to VkRect2D array
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkPipelineRasterizationStateCreateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkPipelineRasterizationStateCreateFlags flags;
        public VkBool32 depthClampEnable;
        public VkBool32 rasterizerDiscardEnable;
        public VkPolygonMode polygonMode;
        public VkCullModeFlags cullMode;
        public VkFrontFace frontFace;
        public VkBool32 depthBiasEnable;
        public float depthBiasConstantFactor;
        public float depthBiasClamp;
        public float depthBiasSlopeFactor;
        public float lineWidth;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkPipelineMultisampleStateCreateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkPipelineMultisampleStateCreateFlags flags;
        public VkSampleCountFlagBits rasterizationSamples;
        public VkBool32 sampleShadingEnable;
        public float minSampleShading;
        public IntPtr pSampleMask; // Pointer to VkSampleMask array
        public VkBool32 alphaToCoverageEnable;
        public VkBool32 alphaToOneEnable;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkStencilOpState
    {
        public VkStencilOp failOp;
        public VkStencilOp passOp;
        public VkStencilOp depthFailOp;
        public VkCompareOp compareOp;
        public uint compareMask;
        public uint writeMask;
        public uint reference;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkPipelineDepthStencilStateCreateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkPipelineDepthStencilStateCreateFlags flags;
        public VkBool32 depthTestEnable;
        public VkBool32 depthWriteEnable;
        public VkCompareOp depthCompareOp;
        public VkBool32 depthBoundsTestEnable;
        public VkBool32 stencilTestEnable;
        public VkStencilOpState front;
        public VkStencilOpState back;
        public float minDepthBounds;
        public float maxDepthBounds;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkPipelineColorBlendAttachmentState
    {
        public VkBool32 blendEnable;
        public VkBlendFactor srcColorBlendFactor;
        public VkBlendFactor dstColorBlendFactor;
        public VkBlendOp colorBlendOp;
        public VkBlendFactor srcAlphaBlendFactor;
        public VkBlendFactor dstAlphaBlendFactor;
        public VkBlendOp alphaBlendOp;
        public VkColorComponentFlagBits colorWriteMask;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct VkPipelineColorBlendStateCreateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkPipelineColorBlendStateCreateFlags flags;
        public VkBool32 logicOpEnable;
        public VkLogicOp logicOp;
        public uint attachmentCount;
        public VkPipelineColorBlendAttachmentState* pAttachments; // Pointer to VkPipelineColorBlendAttachmentState array

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public fixed float blendConstants[4]; // Fixed size array
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkPipelineDynamicStateCreateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkPipelineDynamicStateCreateFlags flags;
        public uint dynamicStateCount;
        public IntPtr pDynamicStates; // Pointer to VkDynamicState array
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct VkGraphicsPipelineCreateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkPipelineCreateFlags flags;
        public uint stageCount;
        public VkPipelineShaderStageCreateInfo* pStages; // Pointer to VkPipelineShaderStageCreateInfo array
        public VkPipelineVertexInputStateCreateInfo* pVertexInputState; // Pointer to VkPipelineVertexInputStateCreateInfo
        public VkPipelineInputAssemblyStateCreateInfo* pInputAssemblyState; // Pointer to VkPipelineInputAssemblyStateCreateInfo
        public VkPipelineTessellationStateCreateInfo* pTessellationState; // Pointer to VkPipelineTessellationStateCreateInfo
        public VkPipelineViewportStateCreateInfo* pViewportState; // Pointer to VkPipelineViewportStateCreateInfo
        public VkPipelineRasterizationStateCreateInfo* pRasterizationState; // Pointer to VkPipelineRasterizationStateCreateInfo
        public VkPipelineMultisampleStateCreateInfo* pMultisampleState; // Pointer to VkPipelineMultisampleStateCreateInfo
        public VkPipelineDepthStencilStateCreateInfo* pDepthStencilState; // Pointer to VkPipelineDepthStencilStateCreateInfo
        public VkPipelineColorBlendStateCreateInfo* pColorBlendState; // Pointer to VkPipelineColorBlendStateCreateInfo
        public VkPipelineDynamicStateCreateInfo* pDynamicState; // Pointer to VkPipelineDynamicStateCreateInfo
        public VkPipeline layout;
        public VkRenderPass renderPass;
        public uint subpass;
        public VkPipeline basePipelineHandle;
        public int basePipelineIndex;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkPushConstantRange
    {
        public VkShaderStageFlags stageFlags;
        public uint offset;
        public uint size;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct VkPipelineLayoutCreateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkPipelineLayoutCreateFlags flags;
        public uint setLayoutCount;
        public VkDescriptorSetLayout* pSetLayouts; // Pointer to VkDescriptorSetLayout array
        public uint pushConstantRangeCount;
        public VkPushConstantRange* pPushConstantRanges; // Pointer to VkPushConstantRange array
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkSamplerCreateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkSamplerCreateFlags flags;
        public VkFilter magFilter;
        public VkFilter minFilter;
        public VkSamplerMipmapMode mipmapMode;
        public VkSamplerAddressMode addressModeU;
        public VkSamplerAddressMode addressModeV;
        public VkSamplerAddressMode addressModeW;
        public float mipLodBias;
        public VkBool32 anisotropyEnable;
        public float maxAnisotropy;
        public VkBool32 compareEnable;
        public VkCompareOp compareOp;
        public float minLod;
        public float maxLod;
        public VkBorderColor borderColor;
        public VkBool32 unnormalizedCoordinates;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkCopyDescriptorSet
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkDescriptorSet srcSet;
        public uint srcBinding;
        public uint srcArrayElement;
        public VkDescriptorSet dstSet;
        public uint dstBinding;
        public uint dstArrayElement;
        public uint descriptorCount;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkDescriptorBufferInfo
    {
        public VkBuffer buffer;
        public VkDeviceSize offset;
        public ulong range;

        public VkDescriptorBufferInfo()
        {
            range = ulong.MaxValue;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkDescriptorImageInfo
    {
        public VkSampler sampler;
        public VkImageView imageView;
        public VkImageLayout imageLayout;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkDescriptorPoolSize
    {
        public VkDescriptorType type;
        public uint descriptorCount;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct VkDescriptorPoolCreateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkDescriptorPoolCreateFlags flags;
        public uint maxSets;
        public uint poolSizeCount;
        public VkDescriptorPoolSize* pPoolSizes; // Pointer to VkDescriptorPoolSize array
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct VkDescriptorSetAllocateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public VkDescriptorPool descriptorPool;
        public uint descriptorSetCount;
        public VkDescriptorSetLayout* pSetLayouts; // Pointer to VkDescriptorSetLayout array
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkDescriptorSetLayoutBinding
    {
        public uint binding;
        public VkDescriptorType descriptorType;
        public uint descriptorCount;
        public VkShaderStageFlags stageFlags;
        public VkSampler pImmutableSamplers; // Pointer to VkSampler array
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct VkDescriptorSetLayoutCreateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public VkDescriptorSetLayoutCreateFlags flags;
        public uint bindingCount;
        public VkDescriptorSetLayoutBinding* pBindings; // Pointer to VkDescriptorSetLayoutBinding array
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct VkWriteDescriptorSet
    {
        public VkStructureType sType;
        public void* pNext;
        public IntPtr dstSet;
        public uint dstBinding;
        public uint dstArrayElement;
        public uint descriptorCount;
        public VkDescriptorType descriptorType;
        public VkDescriptorImageInfo* pImageInfo; // Pointer to VkDescriptorImageInfo array
        public VkDescriptorBufferInfo* pBufferInfo; // Pointer to VkDescriptorBufferInfo array
        public VkBufferView* pTexelBufferView; // Pointer to VkBufferView array
    }

    [Flags]
    public enum VkAttachmentDescriptionFlags
    {
        VK_ATTACHMENT_DESCRIPTION_MAY_ALIAS_BIT = 0x00000001, // Indicates that this attachment may alias another attachment
        VK_ATTACHMENT_DESCRIPTION_FLAG_BITS_MAX_ENUM = 0x7FFFFFFF
    }

    // Define the VkFormat Enum (you already have this, but included for context)
    public enum VkFormat2
    {
        VK_FORMAT_UNDEFINED = 0,
        // ... Additional formats ...
        VK_FORMAT_B8G8R8A8_UNORM = 44,
        VK_FORMAT_R8G8B8A8_UNORM = 37,
        // ... More formats ...
        VK_FORMAT_MAX_ENUM = 0x7FFFFFFF
    }

    // Define the VkSampleCountFlagBits Enum
    [Flags]
    public enum VkSampleCountFlagBits2
    {
        VK_SAMPLE_COUNT_1_BIT = 0x00000001,
        VK_SAMPLE_COUNT_2_BIT = 0x00000002,
        VK_SAMPLE_COUNT_4_BIT = 0x00000004,
        VK_SAMPLE_COUNT_8_BIT = 0x00000008,
        VK_SAMPLE_COUNT_16_BIT = 0x00000010,
        VK_SAMPLE_COUNT_32_BIT = 0x00000020,
        VK_SAMPLE_COUNT_64_BIT = 0x00000040,
        VK_SAMPLE_COUNT_MAX_ENUM = 0x7FFFFFFF
    }

    // Define the VkAttachmentLoadOp Enum
    public enum VkAttachmentLoadOp2
    {
        VK_ATTACHMENT_LOAD_OP_LOAD = 0,
        VK_ATTACHMENT_LOAD_OP_CLEAR = 1,
        VK_ATTACHMENT_LOAD_OP_DONT_CARE = 2,
        VK_ATTACHMENT_LOAD_OP_MAX_ENUM = 0x7FFFFFFF
    }

    // Define the VkAttachmentStoreOp Enum
    public enum VkAttachmentStoreOp2
    {
        VK_ATTACHMENT_STORE_OP_STORE = 0,
        VK_ATTACHMENT_STORE_OP_DONT_CARE = 1,
        VK_ATTACHMENT_STORE_OP_MAX_ENUM = 0x7FFFFFFF
    }

    // Define the VkImageLayout Enum
    public enum VkImageLayout2
    {
        VK_IMAGE_LAYOUT_UNDEFINED = 0,
        VK_IMAGE_LAYOUT_GENERAL = 1,
        VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL = 2,
        VK_IMAGE_LAYOUT_DEPTH_STENCIL_ATTACHMENT_OPTIMAL = 3,
        VK_IMAGE_LAYOUT_DEPTH_STENCIL_READ_ONLY_OPTIMAL = 4,
        VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL = 5,
        VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL = 6,
        VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL = 7,
        VK_IMAGE_LAYOUT_PREINITIALIZED = 8,
        VK_IMAGE_LAYOUT_PRESENT_SRC_KHR = 1000001002,
        VK_IMAGE_LAYOUT_MAX_ENUM = 0x7FFFFFFF
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct VkAttachmentDescription
    {
        public VkAttachmentDescriptionFlags flags;
        public VkFormat2 format;
        public VkSampleCountFlagBits2 samples;
        public VkAttachmentLoadOp2 loadOp;
        public VkAttachmentStoreOp2 storeOp;
        public VkAttachmentLoadOp2 stencilLoadOp;
        public VkAttachmentStoreOp2 stencilStoreOp;
        public VkImageLayout2 initialLayout;
        public VkImageLayout2 finalLayout;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkAttachmentReference
    {
        public uint attachment;
        public VkImageLayout layout;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct VkFramebufferCreateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public VkFramebufferCreateFlags flags;
        public VkRenderPass renderPass;
        public UInt32 attachmentCount;
        public VkImageView* pAttachments;
        public UInt32 width;
        public UInt32 height;
        public UInt32 layers;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct VkSubpassDescription
    {
        public VkSubpassDescriptionFlags flags;
        public VkPipelineBindPoint pipelineBindPoint;
        public uint inputAttachmentCount;
        public VkAttachmentReference* pInputAttachments; // Pointer to VkAttachmentReference array
        public uint colorAttachmentCount;
        public VkAttachmentReference* pColorAttachments; // Pointer to VkAttachmentReference array
        public VkAttachmentReference* pResolveAttachments; // Pointer to VkAttachmentReference array
        public VkAttachmentReference* pDepthStencilAttachment; // Pointer to VkAttachmentReference
        public uint preserveAttachmentCount;
        public uint* pPreserveAttachments; // Pointer to array of uint
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkSubpassDependency
    {
        public uint srcSubpass;
        public uint dstSubpass;
        public VkPipelineStageFlags srcStageMask;
        public VkPipelineStageFlags dstStageMask;
        public VkAccessFlags srcAccessMask;
        public VkAccessFlags dstAccessMask;
        public VkDependencyFlags dependencyFlags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct VkRenderPassCreateInfo
    {
        public VkStructureType sType;
        public void* pNext;
        public VkRenderPassCreateFlags flags;
        public uint attachmentCount;
        public VkAttachmentDescription* pAttachments; // Pointer to VkAttachmentDescription array
        public uint subpassCount;
        public VkSubpassDescription* pSubpasses; // Pointer to VkSubpassDescription array
        public uint dependencyCount;
        public VkSubpassDependency* pDependencies; // Pointer to VkSubpassDependency array
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkCommandPoolCreateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkCommandPoolCreateFlags flags;
        public uint queueFamilyIndex;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkCommandBufferAllocateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkCommandPool commandPool;
        public VkCommandBufferLevel level;
        public uint commandBufferCount;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkCommandBufferInheritanceInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkRenderPass renderPass;
        public uint subpass;
        public VkFramebuffer framebuffer;
        public VkBool32 occlusionQueryEnable;
        public VkQueryControlFlags queryFlags;
        public VkQueryPipelineStatisticFlags pipelineStatistics;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkCommandBufferBeginInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkCommandBufferUsageFlags flags;
        public IntPtr pInheritanceInfo; // Pointer to VkCommandBufferInheritanceInfo
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkBufferCopy
    {
        public VkDeviceSize srcOffset;
        public VkDeviceSize dstOffset;
        public VkDeviceSize size;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkImageSubresourceLayers
    {
        public VkImageAspectFlags aspectMask;
        public uint mipLevel;
        public uint baseArrayLayer;
        public uint layerCount;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkBufferImageCopy
    {
        public VkDeviceSize bufferOffset;
        public uint bufferRowLength;
        public uint bufferImageHeight;
        public VkImageSubresourceLayers imageSubresource;
        public VkOffset3D imageOffset;
        public VkExtent3D imageExtent;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct VkClearColorValue
    {
        [FieldOffset(0)] // Define the starting offset for this field
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] float32;

        [FieldOffset(0)] // Same offset, creating a union-like effect
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public int[] int32;

        [FieldOffset(0)] // Same offset for uint as well
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public uint[] uint32;

        public VkClearColorValue(float[] values)
        {
            // Initialize float32 array
            float32 = new float[4];
            Array.Copy(values, float32, Math.Min(values.Length, 4));
            int32 = null; // Clear other references
            uint32 = null; // Clear other references
        }

        public VkClearColorValue(int[] values)
        {
            // Initialize int32 array
            int32 = new int[4];
            Array.Copy(values, int32, Math.Min(values.Length, 4));
            float32 = null; // Clear other references
            uint32 = null; // Clear other references
        }

        public VkClearColorValue(uint[] values)
        {
            // Initialize uint32 array
            uint32 = new uint[4];
            Array.Copy(values, uint32, Math.Min(values.Length, 4));
            float32 = null; // Clear other references
            int32 = null; // Clear other references
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkClearDepthStencilValue
    {
        public float depth;
        public uint stencil;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkClearValue
    {
        public VkClearColorValue color;
        public VkClearDepthStencilValue depthStencil;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkClearAttachment
    {
        public VkImageAspectFlags aspectMask;
        public uint colorAttachment;
        public VkClearValue clearValue;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkClearRect
    {
        public VkRect2D rect;
        public uint baseArrayLayer;
        public uint layerCount;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkImageBlit
    {
        public VkImageSubresourceLayers srcSubresource;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public VkOffset3D[] srcOffsets; // Size is 2
        public VkImageSubresourceLayers dstSubresource;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public VkOffset3D[] dstOffsets; // Size is 2
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkImageCopy
    {
        public VkImageSubresourceLayers srcSubresource;
        public VkOffset3D srcOffset;
        public VkImageSubresourceLayers dstSubresource;
        public VkOffset3D dstOffset;
        public VkExtent3D extent;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkImageResolve
    {
        public VkImageSubresourceLayers srcSubresource;
        public VkOffset3D srcOffset;
        public VkImageSubresourceLayers dstSubresource;
        public VkOffset3D dstOffset;
        public VkExtent3D extent;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkRenderPassBeginInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkRenderPass renderPass;
        public VkFramebuffer framebuffer;
        public VkRect2D renderArea;
        public uint clearValueCount;
        public IntPtr pClearValues; // Pointer to VkClearValue array
    }

    public struct VkPhysicalDeviceShaderDrawParametersFeatures
    {
        public VkStructureType sType; // Enum representing the structure type
        public IntPtr pNext; // Pointer to the next structure
        public Bool32 shaderDrawParameters; // Similar to above
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Bool32
    {
        public uint value; // 0 for false, 1 for true
        public static implicit operator Bool32(bool b) => new Bool32 { value = b ? 1u : 0u };
        public static implicit operator bool(Bool32 b) => b.value != 0;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkSamplerReductionModeCreateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkSamplerReductionMode reductionMode;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkPhysicalDeviceSamplerFilterMinmaxProperties
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkBool32 filterMinmaxSingleComponentFormats;
        public VkBool32 filterMinmaxImageComponentMapping;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkTransformMatrixKHR
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[,] matrix;

        public VkTransformMatrixKHR()
        {
            matrix = new float[3, 4];
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkMemoryRequirements2
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkMemoryRequirements memoryRequirements;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkImageMemoryRequirementsInfo2
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkImage image;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkPhysicalDeviceProperties2
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkPhysicalDeviceProperties properties;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkFormatProperties2
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkFormatProperties formatProperties;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkImageFormatProperties2
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkImageFormatProperties imageFormatProperties;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkPhysicalDeviceMemoryProperties2
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkPhysicalDeviceMemoryProperties memoryProperties;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkSamplerYcbcrConversionCreateInfo
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkFormat format;
        public VkSamplerYcbcrModelConversion ycbcrModel;
        public VkSamplerYcbcrRange ycbcrRange;
        public VkComponentMapping components;
        public VkChromaLocation xChromaOffset;
        public VkChromaLocation yChromaOffset;
        public VkFilter chromaFilter;
        public VkBool32 forceExplicitReconstruction;
    }

    public struct VkSamplerYcbcrConversion { }
    public struct VkDescriptorUpdateTemplate { }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkExternalBufferProperties
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkExternalMemoryProperties externalMemoryProperties;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkExternalFenceProperties
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkExternalFenceHandleTypeFlags exportFromImportedHandleTypes;
        public VkExternalFenceHandleTypeFlags compatibleHandleTypes;
        public VkExternalFenceFeatureFlags externalFenceFeatures;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkExternalSemaphoreProperties
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkExternalSemaphoreHandleTypeFlags exportFromImportedHandleTypes;
        public VkExternalSemaphoreHandleTypeFlags compatibleHandleTypes;
        public VkExternalSemaphoreFeatureFlags externalSemaphoreFeatures;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkDescriptorSetLayoutSupport
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkBool32 supported;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkExternalMemoryProperties
    {
        public VkExternalMemoryFeatureFlags externalMemoryFeatures;
        public VkExternalMemoryHandleTypeFlags exportFromImportedHandleTypes;
        public VkExternalMemoryHandleTypeFlags compatibleHandleTypes;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkPhysicalDeviceFeatures2
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public VkPhysicalDeviceFeatures features;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkDebugUtilsMessengerCreateInfoEXT
    {
        public VkStructureType sType;
        public IntPtr pNext; // Pointer to next structure, can be null
        public VkDebugUtilsMessengerCreateFlagsEXT flags;
        public VkDebugUtilsMessageSeverityFlagsEXT messageSeverity;
        public VkDebugUtilsMessageTypeFlagsEXT messageType;
        public VkDebugUtilsMessengerCallbackEXT pfnUserCallback;
        public IntPtr pUserData; // Pointer to user data
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkDebugUtilsMessengerCallbackDataEXT
    {
        public VkStructureType sType;     // The type of this structure
        public IntPtr pNext;              // Pointer to the next structure or null
        public string pMessageIdName;     // Name of the message ID
        public uint messageIdNumber;      // Numeric message ID
        public string pMessage;           // The message itself
        public uint queueLabelCount;      // Number of queue labels
        public IntPtr pQueueLabels;       // Pointer to queue labels
        public uint cmdBufLabelCount;     // Number of command buffer labels
        public IntPtr pCmdBufLabels;      // Pointer to command buffer labels
        public IntPtr pUserData;          // Pointer to user data
    }

    public delegate VkResult VkSetDebugUtilsObjectNameEXT(
    IntPtr device, // Use IntPtr for VkDevice
    ref VkDebugUtilsObjectNameInfoEXT pNameInfo
);

    // PFN_vkSetDebugUtilsObjectTagEXT
    public delegate VkResult VkSetDebugUtilsObjectTagEXT(
        IntPtr device, // Use IntPtr for VkDevice
        ref VkDebugUtilsObjectTagInfoEXT pTagInfo
    );

    // PFN_vkQueueBeginDebugUtilsLabelEXT
    public delegate void VkQueueBeginDebugUtilsLabelEXT(
        IntPtr queue, // Use IntPtr for VkQueue
        ref VkDebugUtilsLabelEXT pLabelInfo
    );

    // PFN_vkQueueEndDebugUtilsLabelEXT
    public delegate void VkQueueEndDebugUtilsLabelEXT(
        IntPtr queue  // Use IntPtr for VkQueue
    );

    // PFN_vkQueueInsertDebugUtilsLabelEXT
    public delegate void VkQueueInsertDebugUtilsLabelEXT(
        IntPtr queue, // Use IntPtr for VkQueue
        ref VkDebugUtilsLabelEXT pLabelInfo
    );

    // PFN_vkCmdBeginDebugUtilsLabelEXT
    public delegate void VkCmdBeginDebugUtilsLabelEXT(
        IntPtr commandBuffer, // Use IntPtr for VkCommandBuffer
        ref VkDebugUtilsLabelEXT pLabelInfo
    );

    // PFN_vkCmdEndDebugUtilsLabelEXT
    public delegate void VkCmdEndDebugUtilsLabelEXT(
        IntPtr commandBuffer // Use IntPtr for VkCommandBuffer
    );

    // PFN_vkCmdInsertDebugUtilsLabelEXT
    public delegate void VkCmdInsertDebugUtilsLabelEXT(
        IntPtr commandBuffer, // Use IntPtr for VkCommandBuffer
        ref VkDebugUtilsLabelEXT pLabelInfo
    );

    // PFN_vkCreateDebugUtilsMessengerEXT
    public delegate VkResult VkCreateDebugUtilsMessengerEXT(
        IntPtr instance, // Use IntPtr for VkInstance
        ref VkDebugUtilsMessengerCreateInfoEXT pCreateInfo,
        IntPtr pAllocator, // Use IntPtr for VkAllocationCallbacks
        out IntPtr pMessenger // Use `out` to return the messenger handle
    );

    // PFN_vkDestroyDebugUtilsMessengerEXT
    public delegate void VkDestroyDebugUtilsMessengerEXT(
        IntPtr instance, // Use IntPtr for VkInstance
        IntPtr messenger, // Use IntPtr for VkDebugUtilsMessengerEXT
        IntPtr pAllocator // Use IntPtr for VkAllocationCallbacks
    );

    // PFN_vkSubmitDebugUtilsMessageEXT
    public delegate void VkSubmitDebugUtilsMessageEXT(
        IntPtr instance, // Use IntPtr for VkInstance
        VkDebugUtilsMessageSeverityFlagBitsEXT messageSeverity,
        VkDebugUtilsMessageTypeFlagsEXT messageTypes,
        ref VkDebugUtilsMessengerCallbackDataEXT pCallbackData
    );

    public delegate VkBool32 VkDebugUtilsMessengerCallbackEXT(
        VkDebugUtilsMessageSeverityFlagBitsEXT messageSeverity,
        VkDebugUtilsMessageTypeFlagsEXT messageTypes,
        ref VkDebugUtilsMessengerCallbackDataEXT pCallbackData,  // Use ref to allow modifying the struct's contents in C#
        IntPtr pUserData   // Pointer to user data
    );

    [StructLayout(LayoutKind.Sequential)]
    public struct VkDebugUtilsObjectNameInfoEXT
    {
        public VkStructureType sType;            // Structure type: should be `VK_STRUCTURE_TYPE_DEBUG_UTILS_OBJECT_NAME_INFO_EXT`
        public IntPtr pNext;                      // Pointer to extension-specific structure, or null
        public VkObjectType objectType;           // Type of the object being named
        public ulong objectHandle;                 // Object handle (for example, a VkDevice or VkBuffer handle)
        [MarshalAs(UnmanagedType.LPStr)]         // Marshals as a null-terminated ANSI string
        public string pObjectName;                // Name to give to object
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkDebugUtilsObjectTagInfoEXT
    {
        public VkStructureType sType;            // Structure type: should be `VK_STRUCTURE_TYPE_DEBUG_UTILS_OBJECT_TAG_INFO_EXT`
        public IntPtr pNext;                      // Pointer to extension-specific structure, or null
        public VkObjectType objectType;           // Type of the object being tagged
        public ulong objectHandle;                 // Object handle (for example, a VkDevice or VkBuffer handle)
        public ulong tagName;                     // Unique identifier for the tag
        public IntPtr tagSize;                    // Size of the tag data
        public IntPtr pTag;                       // Pointer to tag data
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkDebugUtilsLabelEXT
    {
        public VkStructureType sType;            // Structure type: should be `VK_STRUCTURE_TYPE_DEBUG_UTILS_LABEL_EXT`
        public IntPtr pNext;                      // Pointer to extension-specific structure, or null
        [MarshalAs(UnmanagedType.LPStr)]         // Marshals as a null-terminated ANSI string
        public string pLabelName;                 // Label name
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)] // Fixed-size array of 4 floats
        public float[] color;                     // RGBA color for the label (r, g, b, a)
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkWin32SurfaceCreateInfoKHR
    {
        public VkStructureType sType;
        public IntPtr pNext;
        public uint flags;
        public IntPtr hinstance;
        public IntPtr hwnd;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkSurfaceCapabilitiesKHR
    {
        public uint minImageCount;                      // Minimum number of images
        public uint maxImageCount;                      // Maximum number of images
        public VkExtent2D currentExtent;                // Current extent of the surface
        public VkExtent2D minImageExtent;               // Minimum extent for images
        public VkExtent2D maxImageExtent;               // Maximum extent for images
        public uint maxImageArrayLayers;                // Maximum layers for images
        public VkSurfaceTransformFlagsKHR supportedTransforms; // Supported transforms
        public VkCompositeAlphaFlagsKHR supportedCompositeAlpha; // Supported composite alpha modes
        public VkPresentModeKHR supportedPresentModes;  // Supported present modes
        public bool clipped;                             // Clipping behavior
        public IntPtr display;                           // Handle to the display
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VkSurfaceFormatKHR
    {
        public VkFormat format;
        public VkColorSpaceKHR colorSpace;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct VulkanBufferInfo
    {
        public VkBuffer Buffer;
        public VkBuffer StagingBuffer;
        public VkDeviceMemory BufferMemory;
        public VkDeviceMemory StagingBufferMemory;
        public VkDeviceSize BufferSize;
        public VkBufferUsageFlags BufferUsage;
        public VkMemoryPropertyFlagBits BufferProperties;
        public IntPtr BufferDeviceAddress;
        public IntPtr BufferHandle;
        public IntPtr BufferData;
        public bool IsMapped;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RenderPassCreateInfoStruct
    {
        public VkStructureType sType; // Required for Vulkan to identify the structure
        public IntPtr pNext;           // We can usually set this to IntPtr.Zero
        public uint flags;             // Vulkan flags for the render pass
        public uint attachmentCount;    // Number of attachments
        public IntPtr pAttachmentList; // Pointer to an array of VkAttachmentDescription
        public uint subpassCount;       // Number of subpasses
        public IntPtr pSubpassDescriptionList; // Pointer to an array of VkSubpassDescription
        public uint dependencyCount;    // Number of subpass dependencies
        public IntPtr pSubpassDependencyList; // Pointer to an array of VkSubpassDependency
    };

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct VkPhysicalDeviceVulkan11Features
    {
        public VkStructureType sType;
        public void* pNext;
        public VkBool32 storageBuffer16BitAccess;
        public VkBool32 uniformAndStorageBuffer16BitAccess;
        public VkBool32 storagePushConstant16;
        public VkBool32 storageInputOutput16;
        public VkBool32 multiview;
        public VkBool32 multiviewGeometryShader;
        public VkBool32 multiviewTessellationShader;
        public VkBool32 variablePointersStorageBuffer;
        public VkBool32 variablePointers;
        public VkBool32 protectedMemory;
        public VkBool32 samplerYcbcrConversion;
        public VkBool32 shaderDrawParameters;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct MeshProperitiesStruct
    {
        public UInt32 MeshIndex;
        public UInt32 MaterialIndex;
        public mat4 MeshTransform;

        public MeshProperitiesStruct()
        {
             MeshIndex = 0;
             MaterialIndex = 0;
             MeshTransform = new mat4();
        }
    };
}
