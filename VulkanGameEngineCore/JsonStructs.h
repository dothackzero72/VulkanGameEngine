#pragma once
#include <string>
#include "includes.h"
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
    kVertexDescsriptor,
    kIndexDescriptor,
    kTransformDescriptor
};

enum VertexTypeEnum
{
    NullVertex = 0,
    SpriteInstanceVertex = 1,
};

struct RenderPassEditorBaseModel
{
    String _name;
    RenderPassEditorBaseModel()
    {
    }
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
    {
    }

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

struct RenderAreaModel
{
    VkRect2D RenderArea;
    bool UseDefaultRenderArea;

    static RenderAreaModel from_json(const nlohmann::json& json, ivec2 renderPassResolution)
    {
        RenderAreaModel model;
        model.RenderArea.offset.x = json["RenderArea"]["offset"]["x"];
        model.RenderArea.offset.y = json["RenderArea"]["offset"]["y"];
        if (json["UseDefaultRenderArea"])
        {
            model.RenderArea.extent.width = renderPassResolution.x;
            model.RenderArea.extent.height = renderPassResolution.y;
        }
        else
        {
            model.RenderArea.extent.width = json["RenderArea"]["extent"]["width"];
            model.RenderArea.extent.height = json["RenderArea"]["extent"]["height"];
        }
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
        model.descriptorType = json["DescriptorType"];
        return model;
    }
};

struct RenderPassBuildInfoModel : public RenderPassEditorBaseModel
{
public:
    bool IsRenderedToSwapchain;
    Vector<String> RenderPipelineList;
    Vector<RenderedTextureInfoModel> RenderedTextureInfoModelList;
    Vector<VkSubpassDependency> SubpassDependencyModelList;
    Vector<VkClearValue> ClearValueList;
    RenderAreaModel RenderArea;

    RenderPassBuildInfoModel()
    {

    }

    static RenderPassBuildInfoModel from_json(const nlohmann::json& json, ivec2 textureResolution)
    {
        RenderPassBuildInfoModel model;
        model._name = json["_name"];
        model.IsRenderedToSwapchain = json["IsRenderedToSwapchain"].get<bool>();
        model.RenderArea = RenderAreaModel::from_json(json["RenderArea"], textureResolution);
      /*  for (int x = 0; x < json["RenderPipelineList"].size(); x++)
        {
            model.RenderPipelineList.emplace_back(json["RenderPipelineList"][x]["Path"]);
        }*/
        for (int x = 0; x < json["RenderedTextureInfoModelList"].size(); x++)
        {
            model.RenderedTextureInfoModelList.emplace_back(RenderedTextureInfoModel::from_json(json["RenderedTextureInfoModelList"][x], textureResolution));
        }
        for (int x = 0; x < json["SubpassDependencyList"].size(); x++)
        {
            model.SubpassDependencyModelList.emplace_back(Json::LoadSubpassDependency(json["SubpassDependencyList"][x]));
        }
        for (int x = 0; x < json["ClearValueList"].size(); x++)
        {
            model.ClearValueList.emplace_back(Json::LoadClearValue(json["ClearValueList"][x]));
        }
        return model;
    }
};

struct RenderPipelineModel : RenderPassEditorBaseModel
{
    String VertexShaderPath;
    String FragmentShaderPath;
    Vector<VkViewport> ViewportList;
    Vector<VkRect2D> ScissorList;
    VertexTypeEnum VertexType;
    uint DescriptorSetCount;
    uint DescriptorSetLayoutCount;
    VkPipelineColorBlendStateCreateInfo PipelineColorBlendStateCreateInfoModel;
    VkPipelineRasterizationStateCreateInfo PipelineRasterizationStateCreateInfo;
    VkPipelineMultisampleStateCreateInfo PipelineMultisampleStateCreateInfo;
    VkPipelineDepthStencilStateCreateInfo PipelineDepthStencilStateCreateInfo;
    VkPipelineInputAssemblyStateCreateInfo PipelineInputAssemblyStateCreateInfo;
    Vector<VkDescriptorSetLayoutBinding> LayoutBindingList;
    Vector<PipelineDescriptorModel> PipelineDescriptorModelsList;
    Vector<VkPipelineColorBlendAttachmentState> PipelineColorBlendAttachmentStateList;
    Vector<VkVertexInputBindingDescription> VertexInputBindingDescriptionList;
    Vector<VkVertexInputAttributeDescription> VertexInputAttributeDescriptionList;

    RenderPipelineModel()
    {

    }

    static RenderPipelineModel from_json(const nlohmann::json& json)
    {
        RenderPipelineModel model;
        model._name = json["_name"];
        model.VertexShaderPath = json["VertexShader"];
        model.FragmentShaderPath = json["FragmentShader"];
        model.VertexType = json["VertexType"];
        model.DescriptorSetCount = json["DescriptorSetCount"];
        model.DescriptorSetLayoutCount = json["DescriptorSetLayoutCount"];
        model.PipelineRasterizationStateCreateInfo = Json::LoadPipelineRasterizationStateCreateInfo(json["PipelineRasterizationStateCreateInfo"]);
        model.PipelineMultisampleStateCreateInfo = Json::LoadPipelineMultisampleStateCreateInfo(json["PipelineMultisampleStateCreateInfo"]);
        model.PipelineDepthStencilStateCreateInfo = Json::LoadPipelineDepthStencilStateCreateInfo(json["PipelineDepthStencilStateCreateInfo"]);
        model.PipelineInputAssemblyStateCreateInfo = Json::LoadPipelineInputAssemblyStateCreateInfo(json["PipelineInputAssemblyStateCreateInfo"]);

        for (int x = 0; x < json["PipelineColorBlendAttachmentStateList"].size(); x++)
        {
            model.PipelineColorBlendAttachmentStateList.emplace_back(Json::LoadPipelineColorBlendAttachmentState(json["PipelineColorBlendAttachmentStateList"][x]));
        }
        model.PipelineColorBlendStateCreateInfoModel = Json::LoadPipelineColorBlendStateCreateInfo(json["PipelineColorBlendStateCreateInfoModel"]);

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
        for (int x = 0; x < json["VertexInputBindingDescriptionList"].size(); x++)
        {
            model.VertexInputBindingDescriptionList.emplace_back(Json::LoadVertexInputBindingDescription(json["VertexInputBindingDescriptionList"][x]));
        }
        for (int x = 0; x < json["VertexInputAttributeDescriptionList"].size(); x++)
        {
            model.VertexInputAttributeDescriptionList.emplace_back(Json::LoadVertexInputAttributeDescription(json["VertexInputAttributeDescriptionList"][x]));
        }
        return model;
    }
};