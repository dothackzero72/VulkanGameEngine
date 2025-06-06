#include "MaterialSystem.h"
#include "TextureSystem.h"
#include <Material.h>
#include "BufferSystem.h"

MaterialSystem materialSystem = MaterialSystem();

MaterialSystem::MaterialSystem()
{
}

MaterialSystem::~MaterialSystem()
{
}

VkGuid MaterialSystem::LoadMaterial(const String& materialPath)
{
    if (materialPath.empty() ||
        materialPath == "")
    {
        return VkGuid();
    }

    nlohmann::json json = Json::ReadJson(materialPath);
    VkGuid materialId = VkGuid(json["MaterialId"].get<String>().c_str());

    if (MaterialMapExists(materialId))
    {
        return materialId;
    }

    String name = json["Name"];
    MaterialMap[materialId] = Material(materialId);
    MaterialMap[materialId].Albedo = vec3(json["Albedo"][0], json["Albedo"][1], json["Albedo"][2]);
    MaterialMap[materialId].Metallic = json["Metallic"];
    MaterialMap[materialId].Roughness = json["Roughness"];
    MaterialMap[materialId].AmbientOcclusion = json["AmbientOcclusion"];
    MaterialMap[materialId].Emission = vec3(json["Emission"][0], json["Emission"][1], json["Emission"][2]);
    MaterialMap[materialId].Alpha = json["Alpha"];

    MaterialMap[materialId].AlbedoMapId = textureSystem.LoadTexture(json["AlbedoMapPath"]);
    MaterialMap[materialId].MetallicRoughnessMapId = textureSystem.LoadTexture(json["MetallicRoughnessMapPath"]);
    MaterialMap[materialId].MetallicMapId = textureSystem.LoadTexture(json["MetallicMapPath"]);
    MaterialMap[materialId].RoughnessMapId = textureSystem.LoadTexture(json["RoughnessMapPath"]);
    MaterialMap[materialId].AmbientOcclusionMapId = textureSystem.LoadTexture(json["AmbientOcclusionMapPath"]);
    MaterialMap[materialId].NormalMapId = textureSystem.LoadTexture(json["NormalMapPath"]);
    MaterialMap[materialId].DepthMapId = textureSystem.LoadTexture(json["DepthMapPath"]);
    MaterialMap[materialId].AlphaMapId = textureSystem.LoadTexture(json["AlphaMapPath"]);
    MaterialMap[materialId].EmissionMapId = textureSystem.LoadTexture(json["EmissionMapPath"]);
    MaterialMap[materialId].HeightMapId = textureSystem.LoadTexture(json["HeightMapPath"]);

    MaterialProperitiesBuffer buffer = MaterialProperitiesBuffer();
    MaterialMap[materialId].MaterialBufferId = bufferSystem.CreateVulkanBuffer<MaterialProperitiesBuffer>(cRenderer, buffer, VK_BUFFER_USAGE_STORAGE_BUFFER_BIT,
                                                                                                                             VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT |
                                                                                                                             VK_MEMORY_PROPERTY_HOST_COHERENT_BIT |
                                                                                                                             VK_MEMORY_ALLOCATE_DEVICE_ADDRESS_BIT, false);

    return materialId;
}

bool MaterialSystem::MaterialMapExists(const VkGuid& renderPassId)
{
    auto it = MaterialMap.find(renderPassId);
    if (it != MaterialMap.end())
    {
        return true;
    }
    return false;
}

const Material& MaterialSystem::FindMaterial(const RenderPassGuid& guid)
{
    auto it = MaterialMap.find(guid);
    if (it != MaterialMap.end())
    {
        return it->second;
    }
    throw std::out_of_range("Material not found for given GUID");
}

const Vector<Material>& MaterialSystem::MaterialList()
{
    Vector<Material> materialList;
    for (const auto& material : MaterialMap)
    {
        materialList.emplace_back(material.second);
    }
    return materialList;
}

const Vector<VkDescriptorBufferInfo> MaterialSystem::GetMaterialPropertiesBuffer()
{
    std::vector<VkDescriptorBufferInfo>	materialPropertiesBuffer;
    if (MaterialMap.empty())
    {
        materialPropertiesBuffer.emplace_back(VkDescriptorBufferInfo
            {
                .buffer = VK_NULL_HANDLE,
                .offset = 0,
                .range = VK_WHOLE_SIZE
            });
    }
    else
    {
        for (auto& material : MaterialMap)
        {
            VkDescriptorBufferInfo meshBufferInfo =
            {
                .buffer = bufferSystem.FindVulkanBuffer(material.second.MaterialBufferId).Buffer,
                .offset = 0,
                .range = VK_WHOLE_SIZE
            };
            materialPropertiesBuffer.emplace_back(meshBufferInfo);
        }
    }
    return materialPropertiesBuffer;
}

void MaterialSystem::Update(const float& deltaTime)
{
    uint x = 0;
    for (auto& materialValue : MaterialMap)
    {
        materialValue.second.ShaderMaterialBufferIndex = x;

        const Material material = materialValue.second;
        MaterialProperitiesBuffer materialBuffer = MaterialProperitiesBuffer
        {
            .AlbedoMapId = material.AlbedoMapId != VkGuid() ? textureSystem.FindTexture(material.AlbedoMapId).textureBufferIndex : 0,
            .MetallicRoughnessMapId = material.MetallicRoughnessMapId != VkGuid() ? textureSystem.FindTexture(material.MetallicRoughnessMapId).textureBufferIndex : 0,
            .MetallicMapId = material.MetallicMapId != VkGuid() ? textureSystem.FindTexture(material.MetallicMapId).textureBufferIndex : 0,
            .RoughnessMapId = material.RoughnessMapId != VkGuid() ? textureSystem.FindTexture(material.RoughnessMapId).textureBufferIndex : 0,
            .AmbientOcclusionMapId = material.AmbientOcclusionMapId != VkGuid() ? textureSystem.FindTexture(material.AmbientOcclusionMapId).textureBufferIndex : 0,
            .NormalMapId = material.NormalMapId != VkGuid() ? textureSystem.FindTexture(material.NormalMapId).textureBufferIndex : 0,
            .DepthMapId = material.DepthMapId != VkGuid() ? textureSystem.FindTexture(material.DepthMapId).textureBufferIndex : 0,
            .AlphaMapId = material.AlphaMapId != VkGuid() ? textureSystem.FindTexture(material.AlphaMapId).textureBufferIndex : 0,
            .EmissionMapId = material.EmissionMapId != VkGuid() ? textureSystem.FindTexture(material.EmissionMapId).textureBufferIndex : 0,
            .HeightMapId = material.HeightMapId != VkGuid() ? textureSystem.FindTexture(material.HeightMapId).textureBufferIndex : 0
        };
        bufferSystem.UpdateBufferMemory<MaterialProperitiesBuffer>(cRenderer, material.MaterialBufferId, materialBuffer);
        x++;
    }
}

void MaterialSystem::DestroyAllMaterials()
{
    //for (auto& material : MaterialMap)
    //{
    //    material.second.Destroy();
    //}
}
