#include "from_json.h"

namespace nlohmann
{
    void from_json(const nlohmann::json& j, String& s) {
        std::string temp;
        j.get_to(temp);
        s = String(temp.c_str());
    }

    //template<typename T>
    //void from_json(const nlohmann::json& j, Vector<T>& vec) {
    //    vec.clear();
    //    for (const auto& item : j) {
    //        T value;
    //        item.get_to(value);
    //        vec.push_back(value);
    //    }
    //}

    void from_json(const nlohmann::json& j, RenderedTextureType& type) {
        std::string value;
        j.get_to(value);
        if (value == "ColorRenderedTexture") {
            type = ColorRenderedTexture;
        }
        else if (value == "DepthRenderedTexture") {
            type = DepthRenderedTexture;
        }
        else if (value == "InputAttachmentTexture") {
            type = InputAttachmentTexture;
        }
        else if (value == "ResolveAttachmentTexture") {
            type = ResolveAttachmentTexture;
        }
        else {
            throw std::runtime_error("Invalid RenderedTextureType: " + value);
        }
    }

    void from_json(const nlohmann::json& j, VkImageCreateInfo& info) {
        info = {}; // Initialize to zero
        info.sType = VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO;
        j.at("imageType").get_to(info.imageType);
        j.at("format").get_to(info.format);
        j.at("extent").at("width").get_to(info.extent.width);
        j.at("extent").at("height").get_to(info.extent.height);
        j.at("extent").at("depth").get_to(info.extent.depth);
        j.at("mipLevels").get_to(info.mipLevels);
        j.at("arrayLayers").get_to(info.arrayLayers);
        j.at("samples").get_to(info.samples);
        j.at("tiling").get_to(info.tiling);
        j.at("usage").get_to(info.usage);
        j.at("sharingMode").get_to(info.sharingMode);
        j.at("initialLayout").get_to(info.initialLayout);
    }

    void from_json(const nlohmann::json& j, VkSamplerCreateInfo& info) {
        info = Json_LoadVulkanSamplerCreateInfo(j);
    }

    void from_json(const nlohmann::json& j, VkAttachmentDescription& desc) {
        desc = Json_LoadAttachmentDescription(j);
    }

    void from_json(const nlohmann::json& j, VkSubpassDependency& dep) {
        j.at("srcSubpass").get_to(dep.srcSubpass);
        j.at("dstSubpass").get_to(dep.dstSubpass);
        j.at("srcStageMask").get_to(dep.srcStageMask);
        j.at("dstStageMask").get_to(dep.dstStageMask);
        j.at("srcAccessMask").get_to(dep.srcAccessMask);
        j.at("dstAccessMask").get_to(dep.dstAccessMask);
        j.at("dependencyFlags").get_to(dep.dependencyFlags);
    }

    void from_json(const nlohmann::json& j, VkClearValue& clearValue) {
        if (j.contains("color")) {
            auto& color = j.at("color");
            color.at(0).get_to(clearValue.color.float32[0]);
            color.at(1).get_to(clearValue.color.float32[1]);
            color.at(2).get_to(clearValue.color.float32[2]);
            color.at(3).get_to(clearValue.color.float32[3]);
        }
        else if (j.contains("depthStencil")) {
            auto& ds = j.at("depthStencil");
            ds.at("depth").get_to(clearValue.depthStencil.depth);
            ds.at("stencil").get_to(clearValue.depthStencil.stencil);
        }
        else {
            throw std::runtime_error("Invalid VkClearValue: must contain 'color' or 'depthStencil'");
        }
    }

    void from_json(const nlohmann::json& j, VkRect2D& rect) {
        j.at("offset").at("x").get_to(rect.offset.x);
        j.at("offset").at("y").get_to(rect.offset.y);
        j.at("extent").at("width").get_to(rect.extent.width);
        j.at("extent").at("height").get_to(rect.extent.height);
    }

    void from_json(const nlohmann::json& j, RenderAreaModel& area) {
        j.at("RenderArea").get_to(area.RenderArea);
        j.at("UseDefaultRenderArea").get_to(area.UseDefaultRenderArea);
    }

    void from_json(const nlohmann::json& j, VkGuid& guid) {
        std::string temp;
        j.get_to(temp);
        guid = VkGuid(temp.c_str());
    }

    void from_json(const nlohmann::json& j, RenderedTextureInfoModel& model) {
        j.at("RenderedTextureInfoName").get_to(model.RenderedTextureInfoName);
        j.at("TextureType").get_to(model.TextureType);
        j.at("ImageCreateInfo").get_to(model.ImageCreateInfo);
        j.at("SamplerCreateInfo").get_to(model.SamplerCreateInfo);
        j.at("AttachmentDescription").get_to(model.AttachmentDescription);
    }
}