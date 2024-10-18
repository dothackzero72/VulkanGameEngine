#pragma once
#include <string>
#include <vulkan/vulkan_core.h>

enum RenderedTextureType
{
    ColorRenderedTexture,
    DepthRenderedTexture,
    InputAttachmentTexture,
    ResolveAttachmentTexture
};

struct RenderPassEditorBaseModel
{
    String Name;
};

struct RenderedTextureInfoModel : RenderPassEditorBaseModel
{
     bool IsRenderedToSwapchain = false;
     String RenderedTextureInfoName;
     VkImageCreateInfo ImageCreateInfo;
     VkSamplerCreateInfo SamplerCreateInfo;
     VkAttachmentDescription AttachmentDescription;
     RenderedTextureType TextureType;
};

struct RenderPassBuildInfoModel : RenderPassEditorBaseModel
{
    ivec2 SwapChainResuloution = ivec2();
    List<VkPipeline> RenderPipelineList = List<VkPipeline>();
    List<RenderedTextureInfoModel> RenderedTextureInfoModelList = List<RenderedTextureInfoModel>();
    List<VkSubpassDependency> SubpassDependencyList = List<VkSubpassDependency>();
};
