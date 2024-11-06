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
//
#include <nlohmann/json.hpp>
//#include <iostream>
//#include <fstream>
//#include <vector>
//#include <stdexcept>
//#include <string>
//#include <vulkan/vulkan.h>
//
//using json = nlohmann::json;
//
//class RenderPassEditorBaseModel {
//public:
//    std::string _name;
//
//    virtual void LoadJsonComponent(const std::string& jsonPath) {
//        std::ifstream inputFile(jsonPath);
//        if (!inputFile.is_open()) {
//            throw std::runtime_error("Could not open JSON file");
//        }
//        json j;
//        inputFile >> j;
//        from_json(j);
//    }
//
//    virtual void SaveJsonComponent(const std::string& jsonPath) {
//        json j = to_json();
//        std::ofstream outputFile(jsonPath);
//        outputFile << j.dump(4); // Pretty print with 4 spaces
//    }
//
//    virtual json to_json() const {
//        return json{ {"_name", _name} };
//    }
//
//    virtual void from_json(const json& j) {
//        j.at("_name").get_to(_name);
//    }
//};
//
//class ImageCreateInfoModel : public RenderPassEditorBaseModel {
//public:
//    VkImageCreateFlags Flags;
//    VkImageType ImageType;
//    VkFormat Format;
//    VkExtent3D Extent;
//    uint32_t MipLevels;
//    uint32_t ArrayLayers;
//    VkSampleCountFlagBits Samples;
//    VkImageTiling Tiling;
//    VkImageUsageFlags Usage;
//    VkSharingMode SharingMode;
//    uint32_t QueueFamilyIndexCount;
//    std::vector<uint32_t> QueueFamilyIndices; // Use vector for dynamic sized array
//    VkImageLayout InitialLayout;
//
//    json to_json() const override {
//        return json{
//            {"Flags", Flags},
//            {"ImageType", ImageType},
//            {"Format", Format},
//            {"Extent", {Extent.width, Extent.height, Extent.depth}},
//            {"MipLevels", MipLevels},
//            {"ArrayLayers", ArrayLayers},
//            {"Samples", Samples},
//            {"Tiling", Tiling},
//            {"Usage", Usage},
//            {"SharingMode", SharingMode},
//            {"QueueFamilyIndexCount", QueueFamilyIndexCount},
//            {"PQueueFamilyIndices", QueueFamilyIndices},
//            {"InitialLayout", InitialLayout},
//            {"_name", _name}
//        };
//    }
//
//    void from_json(const json& j) override {
//        j.at("Flags").get_to(Flags);
//        j.at("ImageType").get_to(ImageType);
//        j.at("Format").get_to(Format);
//        auto extent = j.at("Extent");
//        Extent.width = extent[0];
//        Extent.height = extent[1];
//        Extent.depth = extent[2];
//        j.at("MipLevels").get_to(MipLevels);
//        j.at("ArrayLayers").get_to(ArrayLayers);
//        j.at("Samples").get_to(Samples);
//        j.at("Tiling").get_to(Tiling);
//        j.at("Usage").get_to(Usage);
//        j.at("SharingMode").get_to(SharingMode);
//        j.at("QueueFamilyIndexCount").get_to(QueueFamilyIndexCount);
//        if (QueueFamilyIndexCount > 0) {
//            QueueFamilyIndices = j.at("PQueueFamilyIndices").get<std::vector<uint32_t>>();
//        }
//        j.at("InitialLayout").get_to(InitialLayout);
//        j.at("_name").get_to(_name);
//    }
//};
//
//class SamplerCreateInfoModel : public RenderPassEditorBaseModel {
//public:
//    VkSamplerCreateFlags Flags;
//    VkFilter MagFilter;
//    VkFilter MinFilter;
//    VkSamplerMipmapMode MipmapMode;
//    VkSamplerAddressMode AddressModeU;
//    VkSamplerAddressMode AddressModeV;
//    VkSamplerAddressMode AddressModeW;
//    float MipLodBias;
//    VkBool32 AnisotropyEnable;
//    float MaxAnisotropy;
//    VkBool32 CompareEnable;
//    VkCompareOp CompareOp;
//    float MinLod;
//    float MaxLod;
//    VkBorderColor BorderColor;
//    VkBool32 UnnormalizedCoordinates;
//
//    json to_json() const override {
//        return json{
//            {"Flags", Flags},
//            {"MagFilter", MagFilter},
//            {"MinFilter", MinFilter},
//            {"MipmapMode", MipmapMode},
//            {"AddressModeU", AddressModeU},
//            {"AddressModeV", AddressModeV},
//            {"AddressModeW", AddressModeW},
//            {"MipLodBias", MipLodBias},
//            {"AnisotropyEnable", AnisotropyEnable},
//            {"MaxAnisotropy", MaxAnisotropy},
//            {"CompareEnable", CompareEnable},
//            {"CompareOp", CompareOp},
//            {"MinLod", MinLod},
//            {"MaxLod", MaxLod},
//            {"BorderColor", BorderColor},
//            {"UnnormalizedCoordinates", UnnormalizedCoordinates},
//            {"_name", _name}
//        };
//    }
//
//    void from_json(const json& j) override {
//        j.at("Flags").get_to(Flags);
//        j.at("MagFilter").get_to(MagFilter);
//        j.at("MinFilter").get_to(MinFilter);
//        j.at("MipmapMode").get_to(MipmapMode);
//        j.at("AddressModeU").get_to(AddressModeU);
//        j.at("AddressModeV").get_to(AddressModeV);
//        j.at("AddressModeW").get_to(AddressModeW);
//        j.at("MipLodBias").get_to(MipLodBias);
//        j.at("AnisotropyEnable").get_to(AnisotropyEnable);
//        j.at("MaxAnisotropy").get_to(MaxAnisotropy);
//        j.at("CompareEnable").get_to(CompareEnable);
//        j.at("CompareOp").get_to(CompareOp);
//        j.at("MinLod").get_to(MinLod);
//        j.at("MaxLod").get_to(MaxLod);
//        j.at("BorderColor").get_to(BorderColor);
//        j.at("UnnormalizedCoordinates").get_to(UnnormalizedCoordinates);
//        j.at("_name").get_to(_name);
//    }
//};
//
//class AttachmentDescriptionModel : public RenderPassEditorBaseModel {
//public:
//    VkAttachmentDescriptionFlags Flags;
//    VkFormat Format;
//    VkSampleCountFlagBits Samples;
//    VkAttachmentLoadOp LoadOp;
//    VkAttachmentStoreOp StoreOp;
//    VkAttachmentLoadOp StencilLoadOp;
//    VkAttachmentStoreOp StencilStoreOp;
//    VkImageLayout InitialLayout;
//    VkImageLayout FinalLayout;
//
//    json to_json() const override {
//        return json{
//            {"Flags", Flags},
//            {"Format", Format},
//            {"Samples", Samples},
//            {"LoadOp", LoadOp},
//            {"StoreOp", StoreOp},
//            {"StencilLoadOp", StencilLoadOp},
//            {"StencilStoreOp", StencilStoreOp},
//            {"InitialLayout", InitialLayout},
//            {"FinalLayout", FinalLayout},
//            {"_name", _name}
//        };
//    }
//
//    void from_json(const json& j) override {
//        j.at("Flags").get_to(Flags);
//        j.at("Format").get_to(Format);
//        j.at("Samples").get_to(Samples);
//        j.at("LoadOp").get_to(LoadOp);
//        j.at("StoreOp").get_to(StoreOp);
//        j.at("StencilLoadOp").get_to(StencilLoadOp);
//        j.at("StencilStoreOp").get_to(StencilStoreOp);
//        j.at("InitialLayout").get_to(InitialLayout);
//        j.at("FinalLayout").get_to(FinalLayout);
//        j.at("_name").get_to(_name);
//    }
//};
//
//class RenderedTextureInfoModel : public RenderPassEditorBaseModel {
//public:
//    bool IsRenderedToSwapchain;
//    ImageCreateInfoModel ImageCreateInfo;
//    SamplerCreateInfoModel SamplerCreateInfo;
//    AttachmentDescriptionModel AttachmentDescription;
//    VkBorderColor TextureType;
//
//    json to_json() const override {
//        return json{
//            {"IsRenderedToSwapchain", IsRenderedToSwapchain},
//            {"ImageCreateInfo", ImageCreateInfo.to_json()},
//            {"SamplerCreateInfo", SamplerCreateInfo.to_json()},
//            {"AttachmentDescription", AttachmentDescription.to_json()},
//            {"TextureType", TextureType},
//            {"_name", _name}
//        };
//    }
//
//    void from_json(const json& j) override {
//        j.at("IsRenderedToSwapchain").get_to(IsRenderedToSwapchain);
//        ImageCreateInfo.from_json(j.at("ImageCreateInfo"));
//        SamplerCreateInfo.from_json(j.at("SamplerCreateInfo"));
//        AttachmentDescription.from_json(j.at("AttachmentDescription"));
//        j.at("TextureType").get_to(TextureType);
//        j.at("_name").get_to(_name);
//    }
//};
//
//class RenderPassBuildInfoModel : public RenderPassEditorBaseModel {
//public:
//    std::vector<RenderedTextureInfoModel> RenderedTextureInfoModelList;
//    int SwapChainResolution[2];
//    std::vector<std::string> RenderPipelineList; 
//    std::vector<std::string> SubpassDependencyList;
//
//    nlohmann::json to_json() const override {
//        nlohmann::json j;
//        j["_name"] = _name;
//        j["SwapChainResolution"] = { SwapChainResolution[0], SwapChainResolution[1] };
//        for (const auto& texture : RenderedTextureInfoModelList) 
//        {
//            j["RenderedTextureInfoModelList"].push_back(texture.to_json());
//        }
//        // Save RenderPipelineList and SubpassDependencyList as needed
//        return j;
//    }
//
//    void from_json(const json& j) override {
//        j.at("_name").get_to(_name);
//        j.at("SwapChainResolution").get_to(SwapChainResolution);
//        for (const auto& texture : j.at("RenderedTextureInfoModelList")) 
//        {
//            RenderedTextureInfoModel textureModel;
//            textureModel.from_json(texture);
//            RenderedTextureInfoModelList.push_back(textureModel);
//        }
//        // Load RenderPipelineList and SubpassDependencyList as needed
//    }
//};