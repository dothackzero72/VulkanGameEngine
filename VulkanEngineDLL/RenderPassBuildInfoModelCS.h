//#pragma once
//#include "TypeDef.h"
//#include "VulkanRendererDLL.h"
//
//enum DescriptorBindingPropertiesEnum
//{
//    kMeshPropertiesDescriptor,
//    kTextureDescriptor,
//    kMaterialDescriptor,
//    kBRDFMapDescriptor,
//    kIrradianceMapDescriptor,
//    kPrefilterMapDescriptor,
//    kCubeMapDescriptor,
//    kEnvironmentDescriptor,
//    kSunLightDescriptor,
//    kDirectionalLightDescriptor,
//    kPointLightDescriptor,
//    kSpotLightDescriptor,
//    kReflectionViewDescriptor,
//    kDirectionalShadowDescriptor,
//    kPointShadowDescriptor,
//    kSpotShadowDescriptor,
//    kViewTextureDescriptor,
//    kViewDepthTextureDescriptor,
//    kCubeMapSamplerDescriptor,
//    kRotatingPaletteTextureDescriptor,
//    kMathOpperation1Descriptor,
//    kMathOpperation2Descriptor,
//};
//
//enum RenderedTextureType
//{
//    ColorRenderedTexture,
//    DepthRenderedTexture,
//    InputAttachmentTexture,
//    ResolveAttachmentTexture
//};
//
//
//struct RenderPassEditorBaseModel
//{
//	String name;
//};
//
//struct Blender
//{
//    float R;
//    float G;
//    float B;
//    float A;
//};
//
//struct VkPipelineColorBlendStateCreateInfoModel
//{
//    VkStructureType sType;
//    void* pNext;
//    VkPipelineColorBlendStateCreateFlagBits flags;
//    bool logicOpEnable;
//    VkLogicOp logicOp;
//    uint attachmentCount;
//    VkPipelineColorBlendAttachmentState* pAttachments;
//    Blender blendConstants;
//};
//
//struct VkPipelineRasterizationStateCreateInfoModel
//{
//    VkStructureType sType;
//    bool depthClampEnable;
//    bool rasterizerDiscardEnable;
//    VkPolygonMode polygonMode;
//    VkCullModeFlagBits cullMode;
//    VkFrontFace frontFace;
//    bool depthBiasEnable;
//    float depthBiasConstantFactor;
//    float depthBiasClamp;
//    float depthBiasSlopeFactor;
//    float lineWidth;
//    uint flags;
//
//    void* pNext;
//};
//
//struct VkPipelineMultisampleStateCreateInfoModel
//{
//    VkStructureType sType;
//    VkSampleCountFlagBits rasterizationSamples;
//    bool sampleShadingEnable;
//    float minSampleShading;
//    uint* pSampleMask;
//    bool alphaToCoverageEnable;
//    bool alphaToOneEnable;
//    uint flags;
//    void* pNext;
//};
//
//struct VkStencilOpStateModel
//{
//    VkStencilOp failOp;
//    VkStencilOp passOp;
//    VkStencilOp depthFailOp;
//    VkCompareOp compareOp;
//    uint compareMask;
//    uint writeMask;
//    uint reference;
//};
//
//struct VkPipelineDepthStencilStateCreateInfoModel
//{
//    VkStructureType sType;
//    bool depthTestEnable;
//    bool depthWriteEnable;
//    VkCompareOp depthCompareOp;
//    bool depthBoundsTestEnable;
//    bool stencilTestEnable;
//    VkStencilOpStateModel front;
//    VkStencilOpStateModel back;
//    float minDepthBounds;
//    float maxDepthBounds;
//    uint flags;
//    void* pNext;
//};
//
//struct VkPipelineInputAssemblyStateCreateInfoModel
//{
//    VkStructureType sType;
//    VkPrimitiveTopology topology;
//    bool primitiveRestartEnable;
//    uint flags;
//    void* pNext;
//};
//
//struct VkDescriptorSetLayoutBindingModel
//{
//    uint binding;
//    VkDescriptorType descriptorType;
//    uint descriptorCount;
//    VkShaderStageFlagBits stageFlags;
//    VkSampler* pImmutableSamplers;
//};
//
//struct PipelineDescriptorModel
//{
//    uint BindingNumber;
//    DescriptorBindingPropertiesEnum BindingPropertiesList;
//    VkDescriptorType DescriptorType;
//};
//
//struct VkExtent3DModel : RenderPassEditorBaseModel
//{
//    uint _width;
//    uint _height;
//    uint _depth;
//};
//
//struct RenderPipelineModel
//{
//    String VertexShader;
//    String FragmentShader;
//    Vector<VkViewport> ViewportList;
//    Vector<VkRect2D> ScissorList;
//    Vector<VkPipelineColorBlendAttachmentState> PipelineColorBlendAttachmentStateList;
//    VkPipelineColorBlendStateCreateInfoModel PipelineColorBlendStateCreateInfoModel;
//    VkPipelineRasterizationStateCreateInfoModel PipelineRasterizationStateCreateInfo;
//    VkPipelineMultisampleStateCreateInfoModel PipelineMultisampleStateCreateInfo;
//    VkPipelineDepthStencilStateCreateInfoModel PipelineDepthStencilStateCreateInfo;
//    VkPipelineInputAssemblyStateCreateInfoModel PipelineInputAssemblyStateCreateInfo;
//    Vector<VkDescriptorSetLayoutBindingModel> LayoutBindingList;
//    Vector<PipelineDescriptorModel> PipelineDescriptorModelsList;
//};
//
//struct VkImageCreateInfoModel : RenderPassEditorBaseModel
//{
//    VkStructureType _sType;
//    VkImageCreateFlagBits _flags;
//    void* _pNext;
//    VkImageType _imageType;
//    VkFormat _format;
//    VkExtent3DModel _extent;
//    uint _mipLevels;
//    uint _arrayLayers;
//    VkSampleCountFlagBits _samples;
//    VkImageTiling _tiling;
//    VkImageUsageFlagBits _usage;
//    VkSharingMode _sharingMode;
//    uint _queueFamilyIndexCount;
//    uint* _pQueueFamilyIndices;
//    VkImageLayout _initialLayout;
//};
//
//struct VkSamplerCreateInfoModel : RenderPassEditorBaseModel
//{
//    VkStructureType _sType;
//    VkSamplerCreateFlagBits _flags;
//    void* _pNext;
//    VkFilter _magFilter;
//    VkFilter _minFilter;
//    VkSamplerMipmapMode _mipmapMode;
//    VkSamplerAddressMode _addressModeU;
//    VkSamplerAddressMode _addressModeV;
//    VkSamplerAddressMode _addressModeW;
//    float _mipLodBias;
//    bool _anisotropyEnable;
//    float _maxAnisotropy;
//    bool _compareEnable;
//    VkCompareOp _compareOp;
//    float _minLod;
//    float _maxLod;
//    VkBorderColor _borderColor;
//    bool _unnormalizedCoordinates;
//};
//
//struct VkAttachmentDescriptionModel : RenderPassEditorBaseModel
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
//
//struct RenderedTextureInfoModel : RenderPassEditorBaseModel
//{
//    bool IsRenderedToSwapchain;
//    String _renderedTextureInfoName;
//    VkImageCreateInfoModel _imageCreateInfo;
//    VkSamplerCreateInfoModel _samplerCreateInfo;
//    VkAttachmentDescriptionModel _attachmentDescription;
//    RenderedTextureType _textureType;
//};
//
//struct VkSubpassDependencyModel : RenderPassEditorBaseModel
//{
//    uint _srcSubpass;
//    uint _dstSubpass;
//    VkPipelineStageFlagBits _srcStageMask;
//    VkPipelineStageFlagBits _dstStageMask;
//    VkAccessFlagBits _srcAccessMask;
//    VkAccessFlagBits _dstAccessMask;
//    VkDependencyFlagBits _dependencyFlags;
//};
//
//struct RenderPassBuildInfoModel : RenderPassEditorBaseModel
//{
//	Vector<RenderPipelineModel> RenderPipelineList;
//	Vector<RenderedTextureInfoModel> RenderedTextureInfoModelList;
//	Vector<VkSubpassDependencyModel> SubpassDependencyList;
//};