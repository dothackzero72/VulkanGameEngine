#include "MaterialSystem.h"
#include "TextureSystem.h"

MaterialSystem materialSystem = MaterialSystem();

MaterialSystem::MaterialSystem()
{
}

MaterialSystem::~MaterialSystem()
{
}

void MaterialSystem::Update(const float& deltaTime)
{
    int x = 0;
    for (auto& [id, material] : MaterialMap)
    {
        material.UpdateMaterialBufferIndex(x);
        material.UpdateBuffer();
        ++x;
    }
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

    auto it = MaterialMap.find(materialId);
    if (it != MaterialMap.end())
    {
        return materialId;
    }

    String name = json["Name"];
    MaterialMap[materialId] = Material(name, materialId);
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

    return materialId;
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
        for (auto& [id, material] : MaterialMap)
        {
            material.GetMaterialPropertiesBuffer(materialPropertiesBuffer);
        }
    }
    return materialPropertiesBuffer;
}
