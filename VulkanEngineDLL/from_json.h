#pragma once
#include <nlohmann/json.hpp>
#include "Typedef.h"
#include <vulkan/vulkan.h>
#include "JsonStructs.h"
#include "JsonStruct.h"

namespace nlohmann 
{
    void from_json(const nlohmann::json& j, String& s);
    void from_json(const nlohmann::json& j, RenderedTextureType& type);
    void from_json(const nlohmann::json& j, VkImageCreateInfo& info);
    void from_json(const nlohmann::json& j, VkSamplerCreateInfo& info);
    void from_json(const nlohmann::json& j, VkAttachmentDescription& desc);
    void from_json(const nlohmann::json& j, VkSubpassDependency& dep);
    void from_json(const nlohmann::json& j, VkClearValue& clearValue);
    void from_json(const nlohmann::json& j, VkRect2D& rect);
    void from_json(const nlohmann::json& j, RenderAreaModel& area);
    void from_json(const nlohmann::json& j, VkGuid& guid);
    void from_json(const nlohmann::json& j, RenderedTextureInfoModel& model);
}