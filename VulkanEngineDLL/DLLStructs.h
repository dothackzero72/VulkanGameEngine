#pragma once
#include <VulkanPipeline.h>
#include "DLL.h"
#include <CoreVulkanRenderer.h>
#include <JsonStructs.h>

struct RenderPassEditorBaseDLL
{
    String name;
};

struct GPUIncludesDLL
{
	VkDescriptorBufferInfo* vertexProperties = nullptr;
	VkDescriptorBufferInfo* indexProperties = nullptr;
	VkDescriptorBufferInfo* transformProperties = nullptr;
	VkDescriptorBufferInfo* meshProperties = nullptr;
	VkDescriptorImageInfo* texturePropertiesList = nullptr;
	VkDescriptorBufferInfo* materialProperties = nullptr;
	uint vertexPropertiesCount = 0;
	uint indexPropertiesCount = 0;
	uint transformPropertiesCount = 0;
	uint meshPropertiesCount = 0;
	uint texturePropertiesListCount = 0;
	uint materialPropertiesCount = 0;

    GPUIncludes Convert()
    {
        GPUIncludes includes;
        for (int x = 0; x < vertexPropertiesCount; x++)
        {
            includes.vertexProperties.emplace_back(vertexProperties[x]);
        }
        for (int x = 0; x < indexPropertiesCount; x++)
        {
            includes.indexProperties.emplace_back(indexProperties[x]);
        }
        for (int x = 0; x < transformPropertiesCount; x++)
        {
            includes.transformProperties.emplace_back(transformProperties[x]);
        }
        for (int x = 0; x < meshPropertiesCount; x++)
        {
            includes.meshProperties.emplace_back(meshProperties[x]);
        }
        for (int x = 0; x < texturePropertiesListCount; x++)
        {
            includes.texturePropertiesList.emplace_back(texturePropertiesList[x]);
        }
        for (int x = 0; x < materialPropertiesCount; x++)
        {
            includes.materialProperties.emplace_back(materialProperties[x]);
        }
        return includes;
    }
};

struct Blender
{
    float R;
    float G;
    float B;
    float A;
};

struct VkPipelineColorBlendStateCreateInfoDLL
{
    VkStructureType sType;
    void* pNext;
    VkPipelineColorBlendStateCreateFlags flags;
    VkBool32 logicOpEnable;
    VkLogicOp logicOp;
    uint attachmentCount;
    VkPipelineColorBlendAttachmentState* pAttachments;
    float blendConstants[4];

    VkPipelineColorBlendStateCreateInfo Convert()
    {
        VkPipelineColorBlendStateCreateInfo info =
        {
            .sType = sType,
            .pNext = pNext,
            .flags = flags,
            .logicOpEnable = logicOpEnable,
            .logicOp = logicOp,
            .attachmentCount = attachmentCount,
            .pAttachments = pAttachments,
            .blendConstants = { blendConstants[0], blendConstants[1], blendConstants[2], blendConstants[3] }
        };
        return info;
    }
};

struct VkPipelineRasterizationStateCreateInfoDLL
{
    VkStructureType sType;
    VkBool32 depthClampEnable;
    VkBool32 rasterizerDiscardEnable;
    VkPolygonMode polygonMode;
    VkCullModeFlags cullMode;
    VkFrontFace frontFace;
    VkBool32 depthBiasEnable;
    float depthBiasConstantFactor;
    float depthBiasClamp;
    float depthBiasSlopeFactor;
    float lineWidth;
    uint flags;
    void* pNext;

    VkPipelineRasterizationStateCreateInfo Convert()
    {
        return VkPipelineRasterizationStateCreateInfo
        {
            .sType = sType,
            .pNext = pNext,
            .flags = flags,
            .depthClampEnable = depthClampEnable,
            .rasterizerDiscardEnable = rasterizerDiscardEnable,
            .polygonMode = polygonMode,
            .cullMode = cullMode,
            .frontFace = frontFace,
            .depthBiasEnable = depthBiasEnable,
            .depthBiasConstantFactor = depthBiasConstantFactor,
            .depthBiasClamp = depthBiasClamp,
            .depthBiasSlopeFactor = depthBiasSlopeFactor,
            .lineWidth = lineWidth
        };
    }
};

struct VkPipelineMultisampleStateCreateInfoDLL
{
    VkStructureType sType;
    VkSampleCountFlagBits rasterizationSamples;
    VkBool32 sampleShadingEnable;
    float minSampleShading;
    const VkSampleMask* pSampleMask;
    VkBool32 alphaToCoverageEnable;
    VkBool32 alphaToOneEnable;
    uint flags;
    void* pNext;

    VkPipelineMultisampleStateCreateInfo Convert()
    {
        return VkPipelineMultisampleStateCreateInfo
        {
            .sType = sType,
            .pNext = pNext,
            .flags = flags,
            .rasterizationSamples = rasterizationSamples,
            .sampleShadingEnable = sampleShadingEnable,
            .minSampleShading = minSampleShading,
            .pSampleMask = pSampleMask,
            .alphaToCoverageEnable = alphaToCoverageEnable,
            .alphaToOneEnable = alphaToOneEnable
        };
    }
};

struct VkStencilOpStateDLL
{
    VkStencilOp failOp;
    VkStencilOp passOp;
    VkStencilOp depthFailOp;
    VkCompareOp compareOp;
    uint compareMask;
    uint writeMask;
    uint reference;

    VkStencilOpState Convert()
    {
        return VkStencilOpState
        {
            .failOp = failOp,
            .passOp = passOp,
            .depthFailOp = depthFailOp,
            .compareOp = compareOp,
            .compareMask = compareMask,
            .writeMask = writeMask,
            .reference = reference
        };
    }
};

struct VkPipelineDepthStencilStateCreateInfoDLL
{
    const char* Name;
    VkStructureType sType;
    VkBool32 depthTestEnable;
    VkBool32 depthWriteEnable;
    VkCompareOp depthCompareOp;
    VkBool32 depthBoundsTestEnable;
    VkBool32 stencilTestEnable;
    VkStencilOpStateDLL front;
    VkStencilOpStateDLL back;
    float minDepthBounds;
    float maxDepthBounds; 
    uint flags;
    void* pNext;

    VkPipelineDepthStencilStateCreateInfo Convert()
    {
        return VkPipelineDepthStencilStateCreateInfo
        {
            .sType = sType,
            .pNext = pNext,
            .flags = flags,
            .depthTestEnable = depthTestEnable,
            .depthWriteEnable = depthWriteEnable,
            .depthCompareOp = depthCompareOp,
            .depthBoundsTestEnable = depthBoundsTestEnable,
            .stencilTestEnable = stencilTestEnable,
            .front = front.Convert(),
            .back = back.Convert(),
            .minDepthBounds = minDepthBounds,
            .maxDepthBounds = maxDepthBounds
        };
    }
};

struct VkPipelineInputAssemblyStateCreateInfoDLL
{
    VkStructureType sType;
    VkPrimitiveTopology topology;
    bool primitiveRestartEnable;
    uint flags;
    void* pNext;

    VkPipelineInputAssemblyStateCreateInfo Convert()
    {
        return VkPipelineInputAssemblyStateCreateInfo
        {
            .sType = sType,
            .pNext = pNext,
            .flags = flags,
            .topology = topology,
            .primitiveRestartEnable = primitiveRestartEnable ? VK_TRUE : VK_FALSE
        };
    }
};

struct VkDescriptorSetLayoutBindingDLL
{
    uint binding;
    VkDescriptorType descriptorType;
    uint descriptorCount;
    VkShaderStageFlags stageFlags;
    VkSampler* pImmutableSamplers;

    VkDescriptorSetLayoutBinding Convert()
    {
        return VkDescriptorSetLayoutBinding
        {
            .binding = binding,
            .descriptorType = descriptorType,
            .descriptorCount = descriptorCount,
            .stageFlags = stageFlags,
            .pImmutableSamplers = pImmutableSamplers
        };
    }
};

struct PipelineDescriptorDLL
{
    uint BindingNumber;
    DescriptorBindingPropertiesEnum BindingPropertiesList;
    VkDescriptorType DescriptorType;
};

struct VkExtent3DDLL : RenderPassEditorBaseDLL
{
    uint _width;
    uint _height;
    uint _depth;

    VkExtent3D Convert()
    {
        return VkExtent3D
        {
            .width = _width,
            .height = _height,
            .depth = _depth
        };
    }
};

struct VkPipelineColorBlendAttachmentStateDLL
{
    VkBool32 blendEnable;
    VkBlendFactor srcColorBlendFactor;
    VkBlendFactor dstColorBlendFactor;
    VkBlendOp colorBlendOp;
    VkBlendFactor srcAlphaBlendFactor;
    VkBlendFactor dstAlphaBlendFactor;
    VkBlendOp alphaBlendOp;
    VkColorComponentFlags colorWriteMask;

    VkPipelineColorBlendAttachmentState Convert()
    {
        return VkPipelineColorBlendAttachmentState
        {
            .blendEnable = blendEnable,
            .srcColorBlendFactor = srcColorBlendFactor,
            .dstColorBlendFactor = dstColorBlendFactor,
            .colorBlendOp = colorBlendOp,
            .srcAlphaBlendFactor = srcAlphaBlendFactor,
            .dstAlphaBlendFactor = dstAlphaBlendFactor,
            .alphaBlendOp = alphaBlendOp,
            .colorWriteMask = colorWriteMask
        };
    }
};

struct VkImageCreateInfoDLL : RenderPassEditorBaseDLL
{
    VkStructureType _sType;
    VkImageCreateFlags _flags;
    void* _pNext;
    VkImageType _imageType;
    VkFormat _format;
    VkExtent3DDLL _extent;
    uint _mipLevels;
    uint _arrayLayers;
    VkSampleCountFlagBits _samples;
    VkImageTiling _tiling;
    VkImageUsageFlags _usage;
    VkSharingMode _sharingMode;
    uint _queueFamilyIndexCount;
    uint* _pQueueFamilyIndices;
    VkImageLayout _initialLayout;

    VkImageCreateInfo Convert()
    {
        return VkImageCreateInfo
        {
            .sType = _sType,
            .pNext = _pNext,
            .flags = _flags,
            .imageType = _imageType,
            .format = _format,
            .extent = _extent.Convert(),
            .mipLevels = _mipLevels,
            .arrayLayers = _arrayLayers,
            .samples = _samples,
            .tiling = _tiling,
            .usage = _usage,
            .sharingMode = _sharingMode,
            .queueFamilyIndexCount = _queueFamilyIndexCount,
            .pQueueFamilyIndices = _pQueueFamilyIndices,
            .initialLayout = _initialLayout
        };
    }
};

struct VkSamplerCreateInfoDLL : RenderPassEditorBaseDLL
{
    VkStructureType _sType;
    VkSamplerCreateFlags _flags;
    void* _pNext;
    VkFilter _magFilter;
    VkFilter _minFilter;
    VkSamplerMipmapMode _mipmapMode;
    VkSamplerAddressMode _addressModeU;
    VkSamplerAddressMode _addressModeV;
    VkSamplerAddressMode _addressModeW;
    float _mipLodBias;
    VkBool32 _anisotropyEnable;
    float _maxAnisotropy;
    VkBool32 _compareEnable;
    VkCompareOp _compareOp;
    float _minLod;
    float _maxLod;
    VkBorderColor _borderColor;
    VkBool32 _unnormalizedCoordinates;

    VkSamplerCreateInfo Convert()
    {
        return VkSamplerCreateInfo
        {
            .sType = _sType,
            .pNext = _pNext,
            .flags = _flags,
            .magFilter = _magFilter,
            .minFilter = _minFilter,
            .mipmapMode = _mipmapMode,
            .addressModeU = _addressModeU,
            .addressModeV = _addressModeV,
            .addressModeW = _addressModeW,
            .mipLodBias = _mipLodBias,
            .anisotropyEnable = _anisotropyEnable,
            .maxAnisotropy = _maxAnisotropy,
            .compareEnable = _compareEnable,
            .compareOp = _compareOp,
            .minLod = _minLod,
            .maxLod = _maxLod,
            .borderColor = _borderColor,
            .unnormalizedCoordinates = _unnormalizedCoordinates
        };
    }
};

struct VkAttachmentDescriptionDLL : RenderPassEditorBaseDLL
{
    VkStructureType _structureType;
    VkAttachmentDescriptionFlags _flags;
    void* _pNext;
    VkFormat _format;
    VkSampleCountFlagBits _samples;
    VkAttachmentLoadOp _loadOp;
    VkAttachmentStoreOp _storeOp;
    VkAttachmentLoadOp _stencilLoadOp;
    VkAttachmentStoreOp _stencilStoreOp;
    VkImageLayout _initialLayout;
    VkImageLayout _finalLayout;

    VkAttachmentDescription Convert()
    {
        return VkAttachmentDescription
        {
            .flags = _flags,
            .format = _format,
            .samples = _samples,
            .loadOp = _loadOp,
            .storeOp = _storeOp,
            .stencilLoadOp = _stencilLoadOp,
            .stencilStoreOp = _stencilStoreOp,
            .initialLayout = _initialLayout,
            .finalLayout = _finalLayout
        };
    }
};

//struct RenderedTextureInfoDLL : RenderPassEditorBaseDLL
//{
//    bool IsRenderedToSwapchain;
//    String _renderedTextureInfoName;
//    VkImageCreateInfo _imageCreateInfo;
//    VkSamplerCreateInfo _samplerCreateInfo;
//    VkAttachmentDescription _attachmentDescription;
//    RenderedTextureType _textureType;
//};

#pragma pack(push, 8)
struct RenderPipelineDLL
{
    const char* Name;
    const char* VertexShader;
    const char* FragmentShader;
    VertexTypeEnum VertexType;

    uint DescriptorSetCount;
    uint DescriptorSetLayoutCount;
    uint ViewportListCount;
    uint ScissorListCount;
    uint PipelineColorBlendAttachmentStateListCount;
    uint LayoutBindingListCount;
    uint PipelineDescriptorListCount;
    uint VertexInputBindingDescriptionCount;
    uint VertexInputAttributeDescriptionCount;

    VkViewport* ViewportList;
    VkRect2D* ScissorList;
    VkDescriptorSetLayoutBinding* LayoutBindingList;
    PipelineDescriptorModel* PipelineDescriptorList;
    VkPipelineColorBlendAttachmentState* PipelineColorBlendAttachmentStateList;
    VkPipelineColorBlendStateCreateInfo PipelineColorBlendStateCreateInfo;
    VkPipelineRasterizationStateCreateInfo PipelineRasterizationStateCreateInfo;
    VkPipelineMultisampleStateCreateInfo PipelineMultisampleStateCreateInfo;
    VkPipelineDepthStencilStateCreateInfo PipelineDepthStencilStateCreateInfo;
    VkPipelineInputAssemblyStateCreateInfo PipelineInputAssemblyStateCreateInfo;
    VkVertexInputBindingDescription* VertexInputBindingDescription;
    VkVertexInputAttributeDescription* VertexInputAttributeDescription;

    RenderPipelineModel Convert()
    {
        RenderPipelineModel model;
        model._name = Name;
        model.VertexShaderPath = VertexShader;
        model.FragmentShaderPath = FragmentShader;
        model.DescriptorSetCount = DescriptorSetCount;
        model.DescriptorSetLayoutCount = DescriptorSetLayoutCount;
        model.PipelineColorBlendStateCreateInfoModel = PipelineColorBlendStateCreateInfo;
        model.PipelineRasterizationStateCreateInfo = PipelineRasterizationStateCreateInfo;
        model.PipelineMultisampleStateCreateInfo = PipelineMultisampleStateCreateInfo;
        model.PipelineDepthStencilStateCreateInfo = PipelineDepthStencilStateCreateInfo;
        model.PipelineInputAssemblyStateCreateInfo = PipelineInputAssemblyStateCreateInfo;

        for (uint x = 0; x < ViewportListCount; x++)
        {
            model.ViewportList.emplace_back(ViewportList[x]);
        }
        for (uint x = 0; x < ScissorListCount; x++)
        {
            model.ScissorList.emplace_back(ScissorList[x]);
        }
        for (uint x = 0; x < LayoutBindingListCount; x++)
        {
            model.LayoutBindingList.emplace_back(LayoutBindingList[x]);
        }
        for (uint x = 0; x < PipelineDescriptorListCount; x++)
        {
            model.PipelineDescriptorModelsList.emplace_back(PipelineDescriptorList[x]);
        }
        for (uint x = 0; x < PipelineColorBlendAttachmentStateListCount; x++)
        {
            model.PipelineColorBlendAttachmentStateList.emplace_back(PipelineColorBlendAttachmentStateList[x]);
        }
        for (uint x = 0; x < PipelineColorBlendAttachmentStateListCount; x++)
        {
            model.PipelineColorBlendAttachmentStateList.emplace_back(PipelineColorBlendAttachmentStateList[x]);
        }
        for (uint x = 0; x < VertexInputBindingDescriptionCount; x++)
        {
            model.VertexInputBindingDescriptionList.emplace_back(VertexInputBindingDescription[x]);
        }
        for (uint x = 0; x < VertexInputAttributeDescriptionCount; x++)
        {
            model.VertexInputAttributeDescriptionList.emplace_back(VertexInputAttributeDescription[x]);
        }
        return model;
    }
};
#pragma pack(pop)

//struct VkExtent3DDLL : RenderPassEditorBaseDLL
//{
//    uint _width;
//    uint _height;
//    uint _depth;
//};

//struct VkAttachmentDescriptionDLL : RenderPassEditorBaseDLL
//{
//    VkStructureType _structureType;
//    VkAttachmentDescriptionFlagBits _flags;
//    void* _pNext;
//    VkFormat _format;
//    VkSampleCountFlagBits _samples;
//    VkAttachmentLoadOp _loadOp;
//    VkAttachmentStoreOp _storeOp;
//    VkAttachmentLoadOp _stencilLoadOp;
//    VkAttachmentStoreOp _stencilStoreOp;
//    VkImageLayout _initialLayout;
//    VkImageLayout _finalLayout;
//};

struct RenderedTextureInfoDLL
{
    //const char* renderedTextureInfoName;
    VkImageCreateInfo imageCreateInfo;
    VkSamplerCreateInfo samplerCreateInfo;
    VkAttachmentDescription attachmentDescription;
    RenderedTextureType textureType;

    RenderedTextureInfoModel Convert()
    {
        RenderedTextureInfoModel model;
      //  model.RenderedTextureInfoName = String(renderedTextureInfoName);
       // model.RenderedTextureInfoName = renderedTextureInfoName;
        model.ImageCreateInfo = imageCreateInfo;
        model.SamplerCreateInfo = samplerCreateInfo;
        model.AttachmentDescription = attachmentDescription;
        model.TextureType = textureType;
        return model;
    }
};

struct TextureMemoryDLL
{
    uint TextureId;
    uint TextureBufferIndex;
    String Name;
    int Width;
    int Height;
    int Depth;
    ColorChannelUsed ColorChannels;
    uint MipMapLevels;
    TextureUsageEnum TextureUsage;
    TextureTypeEnum TextureType;
    VkFormat TextureByteFormat;
    VkImageLayout TextureImageLayout;
    VkSampleCountFlagBits SampleCount;
    VkImage Image;
    VkDeviceMemory Memory;
    VkImageView View;
    VkSampler Sampler;
};

struct RenderAreaModelDLL
{
    VkRect2D RenderArea;
    bool UseDefaultRenderArea;
};

struct RenderPassBuildInfoDLL
{
   // const char* name;
//const char** RenderPipelineList;
    RenderedTextureInfoDLL* RenderedTextureInfoModelList;
    VkSubpassDependency* SubpassDependencyList;
    VkClearValue* ClearValueList;
    RenderAreaModelDLL RenderArea;

    bool IsRenderedToSwapchain;

    uint RenderPipelineCount;
    uint RenderedTextureInfoModeCount;
    uint SubpassDependencyCount;
    uint ClearValueCount;

    RenderPassBuildInfoModel Convert()
    {
        RenderPassBuildInfoModel model;
       // model._name = String(name);
        model.IsRenderedToSwapchain = IsRenderedToSwapchain;
        model.RenderArea = RenderAreaModel
        {
            .RenderArea = RenderArea.RenderArea,
            .UseDefaultRenderArea = RenderArea.UseDefaultRenderArea
        };

        for (uint32_t x = 0; x < RenderedTextureInfoModeCount; x++)
        {
            model.RenderedTextureInfoModelList.emplace_back(RenderedTextureInfoModelList[x].Convert());
        }

        for (uint32_t x = 0; x < SubpassDependencyCount; x++)
        {
            model.SubpassDependencyModelList.emplace_back(SubpassDependencyList[x]);
        }

      /*  for (uint32_t x = 0; x < RenderPipelineCount; x++)
        {
            model.RenderPipelineList.emplace_back(String(RenderPipelineList[x]));
        }*/
        for (uint x = 0; x < ClearValueCount; x++)
        {
            model.ClearValueList.emplace_back(ClearValueList[x]);
        }
        return model;
    }
};