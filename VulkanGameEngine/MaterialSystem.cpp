#include "MaterialSystem.h"
#include "TextureSystem.h"

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

    auto it = MaterialList.find(materialId);
    if (it != MaterialList.end())
    {
        return materialId;
    }

    String name = json["Name"];
    MaterialList[materialId] = Material(name, materialId);
    MaterialList[materialId].Albedo = vec3(json["Albedo"][0], json["Albedo"][1], json["Albedo"][2]);
    MaterialList[materialId].Metallic = json["Metallic"];
    MaterialList[materialId].Roughness = json["Roughness"];
    MaterialList[materialId].AmbientOcclusion = json["AmbientOcclusion"];
    MaterialList[materialId].Emission = vec3(json["Emission"][0], json["Emission"][1], json["Emission"][2]);
    MaterialList[materialId].Alpha = json["Alpha"];

    MaterialList[materialId].AlbedoMapId = textureSystem.LoadTexture(json["AlbedoMapPath"]);
    MaterialList[materialId].MetallicRoughnessMapId = textureSystem.LoadTexture(json["MetallicRoughnessMapPath"]);
    MaterialList[materialId].MetallicMapId = textureSystem.LoadTexture(json["MetallicMapPath"]);
    MaterialList[materialId].RoughnessMapId = textureSystem.LoadTexture(json["RoughnessMapPath"]);
    MaterialList[materialId].AmbientOcclusionMapId = textureSystem.LoadTexture(json["AmbientOcclusionMapPath"]);
    MaterialList[materialId].NormalMapId = textureSystem.LoadTexture(json["NormalMapPath"]);
    MaterialList[materialId].DepthMapId = textureSystem.LoadTexture(json["DepthMapPath"]);
    MaterialList[materialId].AlphaMapId = textureSystem.LoadTexture(json["AlphaMapPath"]);
    MaterialList[materialId].EmissionMapId = textureSystem.LoadTexture(json["EmissionMapPath"]);
    MaterialList[materialId].HeightMapId = textureSystem.LoadTexture(json["HeightMapPath"]);

    return materialId;
}
