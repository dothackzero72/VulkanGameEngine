#include "AssetManager.h"

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
	UM_SpriteVRAMID vramId = json["VRAMSpriteID"];

	SpriteVRAM sprite;
	sprite.VRAMSpriteID = vramId;
	sprite.VRAMSpriteName = json["VRAMSpriteName"];
	sprite.SpriteLayer = json["SpriteLayer"];
	sprite.SpriteSize = ivec2{ json["SpriteSize"][0], json["SpriteSize"][1] };
	sprite.SpriteColor = vec4{ json["SpriteColor"][0], json["SpriteColor"][1], json["SpriteColor"][2], json["SpriteColor"][3] };

	VRAMSpriteList[vramId] = sprite;
	return vramId;
}

UM_TextureID AssetManager::LoadTexture(const String& texturePath)
{
	nlohmann::json json = Json::ReadJson(texturePath);
	UM_TextureID textureId = Texture::GetNextTextureID();
	GUID assetID = GetGUID(json["AssetID"]);
	String textureFilePath = json["TextureFilePath"];
	VkFormat textureByteFormat = json["TextureByteFormat"];
	VkImageAspectFlags imageType = json["ImageType"];
	TextureTypeEnum textureType = json["TextureType"];
	bool useMipMaps = json["UseMipMaps"];

	TextureList[textureId] = Texture(textureId, assetID, textureFilePath, textureByteFormat, imageType, textureType, useMipMaps);
	return textureId;
}

UM_MaterialID AssetManager::LoadMaterial(const String& materialPath)
{
	return 0;
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