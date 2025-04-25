#include "AssetManager.h"
#include "Material.h"

AssetManager assetManager = AssetManager();

AssetManager::AssetManager()
{

}

AssetManager::~AssetManager()
{

}

void AssetManager::Input(const float& deltaTime)
{

}

void AssetManager::Update(VkCommandBuffer& commandBuffer, const float& deltaTime)
{

}

void AssetManager::Destroy()
{

}

void AssetManager::CreateEntity()
{
	//if (!FreeIds.empty())
	//{
	//	GameObjectList.emplace_back(FreeIds.front());
	//	FreeIds.pop();
	//	return;
	//}
	//GameObjectList.emplace_back(++NextId);
}

void AssetManager::DestroyEntity(uint32_t id)
{
	FreeIds.push(id);
}

UM_SpriteVRAMID AssetManager::AddSpriteVRAM(const String& spritePath)
{
	nlohmann::json json = Json::ReadJson(spritePath);
	UM_SpriteVRAMID vramId = 0;

	SpriteVRAM sprite;
	sprite.VRAMSpriteID = vramId;
	sprite.SpriteLayer = json["SpriteLayer"];
	sprite.SpriteSize = ivec2{ json["SpriteSize"][0], json["SpriteSize"][1] };
	sprite.SpriteColor = vec4{ json["SpriteColor"][0], json["SpriteColor"][1], json["SpriteColor"][2], json["SpriteColor"][3] };

	VRAMSpriteList[vramId] = sprite;
	return vramId;
}

UM_TextureID AssetManager::LoadTexture(const String& texturePath)
{
	if (texturePath.empty())
	{
		return 0;
	}

	nlohmann::json json = Json::ReadJson(texturePath);
	UM_TextureID textureId = Texture::GetNextTextureID();
	String textureFilePath = json["TextureFilePath"];
	VkFormat textureByteFormat = json["TextureByteFormat"];
	VkImageAspectFlags imageType = json["ImageType"];
	TextureTypeEnum textureType = json["TextureType"];
	bool useMipMaps = json["UseMipMaps"];

	TextureList[textureId] = Texture(textureId, textureFilePath, textureByteFormat, imageType, textureType, useMipMaps);
	return textureId;
}

UM_MaterialID AssetManager::LoadMaterial(const String& materialPath)
{
	nlohmann::json json = Json::ReadJson(materialPath);

	UM_MaterialID materialId = Material::GetNextMaterialId();
	String name = json["Name"];
	MaterialList[materialId] = Material(name, materialId);

	MaterialList[materialId].Albedo = vec3(json["Albedo"][0], json["Albedo"][1], json["Albedo"][2]);
	MaterialList[materialId].Metallic = json["Metallic"];
	MaterialList[materialId].Roughness = json["Roughness"];
	MaterialList[materialId].AmbientOcclusion = json["AmbientOcclusion"];
	MaterialList[materialId].Emission = vec3(json["Emission"][0], json["Emission"][1], json["Emission"][2]);
	MaterialList[materialId].Alpha = json["Alpha"];

	MaterialList[materialId].AlbedoMapId = LoadTexture(json["AlbedoMapPath"]);
	MaterialList[materialId].MetallicRoughnessMapId = LoadTexture(json["MetallicRoughnessMapPath"]);
	MaterialList[materialId].MetallicMapId = LoadTexture(json["MetallicMapPath"]);
	MaterialList[materialId].RoughnessMapId = LoadTexture(json["RoughnessMapPath"]);
	MaterialList[materialId].AmbientOcclusionMapId = LoadTexture(json["AmbientOcclusionMapPath"]);
	MaterialList[materialId].NormalMapId = LoadTexture(json["NormalMapPath"]);
	MaterialList[materialId].DepthMapId = LoadTexture(json["DepthMapPath"]);
	MaterialList[materialId].AlphaMapId = LoadTexture(json["AlphaMapPath"]);
	MaterialList[materialId].EmissionMapId = LoadTexture(json["EmissionMapPath"]);
	MaterialList[materialId].HeightMapId = LoadTexture(json["HeightMapPath"]);

	return materialId;
}

GUID AssetManager::GetGUID(nlohmann::json& json)
{
	GUID guid = {};
	std::string guidStr = json.get<String>();

	if (guidStr.front() != '{') 
	{
		guidStr = "{" + guidStr + "}";
	}

	std::wstring wGuidStr(guidStr.begin(), guidStr.end());
	HRESULT result = CLSIDFromString(wGuidStr.c_str(), &guid);

	if (FAILED(result)) 
	{
		std::cerr << "Failed to convert string to GUID. HRESULT: " << std::hex << result << std::endl;
		throw std::runtime_error("Invalid GUID format.");
	}

	return guid;
}