#include "AssetManager.h"
#include "Material.h"
#include <json.h>

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

VkGuid AssetManager::AddSpriteVRAM(const String& spritePath)
{
	const Material& material = MaterialList.at(0);
	const Texture& texture = TextureList.at(material.AlbedoMapId);

	nlohmann::json json = Json::ReadJson(spritePath);
	VkGuid vramId = VkGuid(json["VramSpriteId"].get<String>().c_str());

	//VkGuid				VramSpriteID;
	//uint				SpriteMaterialID = 0;
	//uint				SpriteLayer = 0;
	//vec2				SpriteSize = vec2(50.0f);
	//vec4				SpriteColor = vec4(0.0f, 0.0f, 0.0f, 1.0f);
	//ivec2				SpritePixelSize;
	//ivec2				SpriteCells;
	//vec2				SpriteUVSize;
	//vec2				SpriteScale;
	//uint				AnimationListID;

	SpriteVram sprite = SpriteVram
	{
		.VramSpriteID = vramId,
		.SpriteMaterialID = 0,
		.SpriteLayer = json["SpriteLayer"],
		.SpriteColor = vec4{ json["SpriteColor"][0], json["SpriteColor"][1], json["SpriteColor"][2], json["SpriteColor"][3] },
		.SpritePixelSize = ivec2{ json["SpritePixelSize"][0], json["SpritePixelSize"][1] },
		.SpriteScale = vec2(5.0f),
		.SpriteCells = ivec2(texture.Width / sprite.SpritePixelSize.x, texture.Height / sprite.SpritePixelSize.y),
		.SpriteUVSize = vec2(1.0f / (float)sprite.SpriteCells.x, 1.0f / (float)sprite.SpriteCells.y),
		.SpriteSize = vec2(sprite.SpritePixelSize.x * sprite.SpriteScale.x, sprite.SpritePixelSize.y * sprite.SpriteScale.y),
	};

	VramSpriteList[vramId] = sprite;
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

void AssetManager::DestroyGameObject(UM_GameObjectID id)
{
	MeshList[id].Destroy();

	MeshList.erase(id);
	SpriteList.erase(id);
	TransformComponentList.erase(id);
	GameObjectList.erase(id);
}

void AssetManager::DestroyGameObjects()
{
	for (auto& gameObject : GameObjectList)
	{
		const UM_GameObjectID id = gameObject.second.GameObjectId;
		MeshList[id].Destroy();
	}
	MeshList.clear();
	SpriteList.clear();
	TransformComponentList.clear();
	GameObjectList.clear();
}

void AssetManager::DestoryTextures()
{
	for (auto& texture : TextureList)
	{
		const uint id = texture.second.TextureId;
		TextureList[id].Destroy();
	}
	TextureList.clear();
}

void AssetManager::DestoryMaterials()
{
	for (auto& material : MaterialList)
	{
		const uint id = material.second.MaterialID;
		MaterialList[id].Destroy();
	}
	MaterialList.clear();
}

void AssetManager::DestoryVRAMSprites()
{
	for (auto& spriteVram : VramSpriteList)
	{
		/*const uint id = spriteVRAM.second.VRAMSpriteID;
		VRAMSpriteList.erase(id);*/
	}
	VramSpriteList.clear();
}

void AssetManager::Destroy()
{
	DestroyGameObjects();
	DestoryTextures();
	DestoryMaterials();
	DestoryVRAMSprites();
}

