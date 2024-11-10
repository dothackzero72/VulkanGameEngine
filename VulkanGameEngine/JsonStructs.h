#pragma once
#include <string>
#include <vulkan/vulkan_core.h>
#include <nlohmann/json.hpp>

enum RenderedTextureType
{
    ColorRenderedTexture,
    DepthRenderedTexture,
    InputAttachmentTexture,
    ResolveAttachmentTexture
};

struct RenderPassEditorBaseModel
{
    String _name;
    RenderPassEditorBaseModel()
    {}
};

struct RenderedTextureInfoModel : RenderPassEditorBaseModel
{
public:
    String RenderedTextureInfoName;
    VkImageCreateInfo ImageCreateInfo;
    VkSamplerCreateInfo SamplerCreateInfo;
    VkAttachmentDescription AttachmentDescription;
    RenderedTextureType TextureType;

    RenderedTextureInfoModel()
    {}

    static RenderedTextureInfoModel from_json(const nlohmann::json& json)
    {
        RenderedTextureInfoModel model;
        model.RenderedTextureInfoName = json["RenderedTextureInfoName"];
        model.ImageCreateInfo = Json::LoadImageCreateInfo(json["ImageCreateInfo"]);
        model.SamplerCreateInfo = Json::LoadVulkanSamplerCreateInfo(json["SamplerCreateInfo"]);
        model.AttachmentDescription = Json::LoadAttachmentDescription(json["AttachmentDescription"]);
        return model;
    }
};

struct SubpassDependencyModel : public RenderPassEditorBaseModel
{
public:
    uint32 _srcSubpass;
    uint32 _dstSubpass;
    VkPipelineStageFlags _srcStageMask;
    VkPipelineStageFlags _dstStageMask;
    VkAccessFlags _srcAccessMask;
    VkAccessFlags _dstAccessMask;
    VkDependencyFlags _dependencyFlags;

    SubpassDependencyModel()
    {

    }

    static SubpassDependencyModel from_json(const nlohmann::json& json)
    {
        SubpassDependencyModel model;
        model._srcSubpass = json["_srcSubpass"];
        model._dstSubpass = json["_dstSubpass"];
        model._srcStageMask = json["_srcStageMask"];
        model._dstStageMask = json["_dstStageMask"];
        model._srcAccessMask = json["_srcAccessMask"];
        model._dstAccessMask = json["_dstAccessMask"];
        model._dependencyFlags = json["_dependencyFlags"];
        model._name = json["_name"];

        return model;
    }
};

struct RenderPassBuildInfoModel : public RenderPassEditorBaseModel 
{
public:
    ivec2 SwapChainResolution;
    std::vector<std::string> RenderPipelineList;
    std::vector<RenderedTextureInfoModel> RenderedTextureInfoModelList;
    List<SubpassDependencyModel> SubpassDependencyModelList;

    RenderPassBuildInfoModel()
    {

    }

    static RenderPassBuildInfoModel from_json(const nlohmann::json& json)
    {
        RenderPassBuildInfoModel model;
        model._name = json["_name"];
        model.SwapChainResolution.x = json["SwapChainResuloution"][0];
        model.SwapChainResolution.y  = json["SwapChainResuloution"][1];
        for (int x = 0; x < json.at("RenderedTextureInfoModelList"); x++)
        {
            model.RenderedTextureInfoModelList.emplace_back(RenderedTextureInfoModel::from_json(json["RenderedTextureInfoModelList"][x]));
        }
        //for (int x = 0; x < json.at("RenderedTextureInfoModelList"); x++)
        //{
        //    nlohmann::json b = json["RenderedTextureInfoModelList"][x];
        //    auto a = Json::LoadImageCreateInfo(b);
        //    model.RenderedTextureInfoModelList.emplace_back(a);
        //}
        //for (int x = 0; x < json.at("SubpassDependencyModelList"); x++)
        //{
        //    nlohmann::json b = json["SubpassDependencyModelList"][x];
        //    auto a = Json::LoadImageCreateInfo(b);
        //    model.RenderedTextureInfoModelList.emplace_back(a);
        //}
        return model;
    }
};