#pragma once
#include <string>
#include <vulkan/vulkan_core.h>
#include <nlohmann/json.hpp>
#include "Typedef.h"
#include "json.h"

enum RenderedTextureType
{
    ColorRenderedTexture,
    DepthRenderedTexture,
    InputAttachmentTexture,
    ResolveAttachmentTexture
};

enum DescriptorBindingPropertiesEnum
{
    kMeshPropertiesDescriptor,
    kTextureDescriptor,
    kMaterialDescriptor,
    kBRDFMapDescriptor,
    kIrradianceMapDescriptor,
    kPrefilterMapDescriptor,
    kCubeMapDescriptor,
    kEnvironmentDescriptor,
    kSunLightDescriptor,
    kDirectionalLightDescriptor,
    kPointLightDescriptor,
    kSpotLightDescriptor,
    kReflectionViewDescriptor,
    kDirectionalShadowDescriptor,
    kPointShadowDescriptor,
    kSpotShadowDescriptor,
    kViewTextureDescriptor,
    kViewDepthTextureDescriptor,
    kCubeMapSamplerDescriptor,
    kRotatingPaletteTextureDescriptor,
    kMathOpperation1Descriptor,
    kMathOpperation2Descriptor,
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

    static RenderedTextureInfoModel from_json(const nlohmann::json& json, ivec2 textureResolution)
    {
        RenderedTextureInfoModel model;
        model.RenderedTextureInfoName = json["RenderedTextureInfoName"];
        model.TextureType = json["TextureType"];
        model.ImageCreateInfo = Json::LoadImageCreateInfo(json["ImageCreateInfo"], textureResolution);
        model.SamplerCreateInfo = Json::LoadVulkanSamplerCreateInfo(json["SamplerCreateInfo"]);
        model.AttachmentDescription = Json::LoadAttachmentDescription(json["AttachmentDescription"]);
        return model;
    }
};

struct PipelineDescriptorModel
{
    uint BindingNumber;
    DescriptorBindingPropertiesEnum BindingPropertiesList;
    VkDescriptorType descriptorType;

    PipelineDescriptorModel()
    {

    }

    static PipelineDescriptorModel from_json(const nlohmann::json& json)
    {
        PipelineDescriptorModel model;
        model.BindingNumber = json["BindingNumber"];
        model.BindingPropertiesList = json["BindingPropertiesList"];
        model.descriptorType  = json["DescriptorType"];
        return model;
    }
};

struct RenderPassBuildInfoModel : public RenderPassEditorBaseModel 
{
public:
    bool IsRenderedToSwapchain;
    std::vector<std::string> RenderPipelineList;
    std::vector<RenderedTextureInfoModel> RenderedTextureInfoModelList;
    List<VkSubpassDependency> SubpassDependencyModelList;

    RenderPassBuildInfoModel()
    {

    }

    static RenderPassBuildInfoModel from_json(const nlohmann::json& json, ivec2 textureResolution)
    {
        RenderPassBuildInfoModel model;
        model._name = json["_name"];
        for (int x = 0; x < json["RenderedTextureInfoModelList"].size(); x++)
        {
            model.RenderedTextureInfoModelList.emplace_back(RenderedTextureInfoModel::from_json(json["RenderedTextureInfoModelList"][x], textureResolution));
        }
        for (int x = 0; x < json["SubpassDependencyList"].size(); x++)
        {
            model.SubpassDependencyModelList.emplace_back(Json::LoadSubpassDependency(json["SubpassDependencyList"][x]));
        }
        return model;
    }
};

struct RenderPipelineModel : RenderPassEditorBaseModel
{
    String VertexShaderPath;
    String FragmentShaderPath;
    List<VkViewport> ViewportList;
    List<VkRect2D> ScissorList;
    VkPipelineColorBlendStateCreateInfo PipelineColorBlendStateCreateInfoModel;
    VkPipelineRasterizationStateCreateInfo PipelineRasterizationStateCreateInfo;
    VkPipelineMultisampleStateCreateInfo PipelineMultisampleStateCreateInfo;
    VkPipelineDepthStencilStateCreateInfo PipelineDepthStencilStateCreateInfo;
    VkPipelineInputAssemblyStateCreateInfo PipelineInputAssemblyStateCreateInfo;
    List<VkDescriptorSetLayoutBinding> LayoutBindingList;
    List<PipelineDescriptorModel> PipelineDescriptorModelsList;

    RenderPipelineModel()
    {

    }

    static RenderPipelineModel from_json(const nlohmann::json& json)
    {
        RenderPipelineModel model;
        model._name = json["_name"];
        model.VertexShaderPath = json["VertexShader"];
        model.FragmentShaderPath = json["FragmentShader"];
        model.PipelineRasterizationStateCreateInfo = Json::LoadPipelineRasterizationStateCreateInfo(json["PipelineRasterizationStateCreateInfo"]);
        model.PipelineMultisampleStateCreateInfo = Json::LoadPipelineMultisampleStateCreateInfo(json["PipelineMultisampleStateCreateInfo"]);
        model.PipelineDepthStencilStateCreateInfo = Json::LoadPipelineDepthStencilStateCreateInfo(json["PipelineDepthStencilStateCreateInfo"]);
        model.PipelineInputAssemblyStateCreateInfo = Json::LoadPipelineInputAssemblyStateCreateInfo(json["PipelineInputAssemblyStateCreateInfo"]);
        model.PipelineMultisampleStateCreateInfo = Json::LoadPipelineMultisampleStateCreateInfo(json["PipelineMultisampleStateCreateInfo"]);


        List<VkPipelineColorBlendAttachmentState> pipelineColorBlendAttachmentStateList;
        for (int x = 0; x < json["PipelineColorBlendAttachmentStateList"].size(); x++)
        {
            pipelineColorBlendAttachmentStateList.emplace_back(Json::LoadPipelineColorBlendAttachmentState(json["PipelineColorBlendAttachmentStateList"][x]));
        }
        model.PipelineColorBlendStateCreateInfoModel = Json::LoadPipelineColorBlendStateCreateInfo(json["PipelineColorBlendStateCreateInfoModel"], pipelineColorBlendAttachmentStateList);

        for (int x = 0; x < json["LayoutBindingList"].size(); x++)
        {
            model.LayoutBindingList.emplace_back(Json::LoadLayoutBinding(json["LayoutBindingList"][x]));
        }
        for (int x = 0; x < json["PipelineDescriptorModelsList"].size(); x++)
        {
            model.PipelineDescriptorModelsList.emplace_back(PipelineDescriptorModel::from_json(json["PipelineDescriptorModelsList"][x]));
        }
        for (int x = 0; x < json["ViewportList"].size(); x++)
        {
            model.ViewportList.emplace_back(Json::LoadViewPort(json["ViewportList"][x]));
        }
        for (int x = 0; x < json["ScissorList"].size(); x++)
        {
            model.ScissorList.emplace_back(Json::LoadRect2D(json["ScissorList"][x]));
        }

        return model;
    }
};