#include "AssetManager.h"
#include "Material.h"
#include "json.h"
#include "RenderSystem.h"
#include "TextureSystem.h"

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

void AssetManager::CreateGameObject(const String& name, const Vector<ComponentTypeEnum>& gameObjectComponentTypeList, VkGuid vramId, vec2 objectPosition)
{
	GameObjectID id;
	id.id = assetManager.GameObjectList.size() + 1;
	assetManager.GameObjectList[id] = GameObject(id);

	for (auto component : gameObjectComponentTypeList)
	{
		switch (component)
		{
			case kTransform2DComponent: assetManager.TransformComponentList[id] = Transform2DComponent(objectPosition); break;
			case kInputComponent: assetManager.InputComponentList[id] = InputComponent(); break;
			case kSpriteComponent: assetManager.SpriteList[id] = Sprite(id, vramId); break;
		}
	}
}

void AssetManager::CreateGameObject(const String& gameObjectPath, const vec2& gameObjectPosition)
{
	GameObjectID id;
	id.id = assetManager.GameObjectList.size() + 1;
	assetManager.GameObjectList[id] = GameObject(id);

	nlohmann::json json = Json::ReadJson(gameObjectPath);
	for (int x = 0; x < json["GameObjectComponentList"].size(); x++)
	{
		const int componentType = json["GameObjectComponentList"][x]["ComponentType"];
		switch (componentType)
		{
			case kTransform2DComponent:  AddTransformComponent(json["GameObjectComponentList"][x], id, gameObjectPosition); break;
			case kInputComponent: AddInputComponent(json["GameObjectComponentList"][x], id); break;
			case kSpriteComponent: AddSpriteComponent(json["GameObjectComponentList"][x], id); break;
		}
	}
}

void AssetManager::AddTransformComponent(const nlohmann::json& json, GameObjectID id, const vec2& gameObjectPosition)
{
	Transform2DComponent transform2D;
	transform2D.GameObjectPosition = gameObjectPosition;
	transform2D.GameObjectRotation = vec2{ json["GameObjectRotation"][0], json["GameObjectRotation"][1] };
	transform2D.GameObjectScale = vec2{ json["GameObjectScale"][0], json["GameObjectScale"][1] };
	assetManager.TransformComponentList[id] = transform2D;
}

void AssetManager::AddInputComponent(const nlohmann::json& json, GameObjectID id)
{
	assetManager.InputComponentList[id] = InputComponent();
}

void AssetManager::AddSpriteComponent(const nlohmann::json& json, GameObjectID id)
{
	VkGuid vramId = VkGuid(json["VramId"].get<String>().c_str());
	assetManager.SpriteList[id] = Sprite(id, vramId);
}

VkGuid AssetManager::AddSpriteVRAM(const String& spritePath)
{
    nlohmann::json json = Json::ReadJson(spritePath);
    VkGuid vramId = VkGuid(json["VramSpriteId"].get<String>().c_str());
    VkGuid materialId = VkGuid(json["MaterialId"].get<String>().c_str());

    auto it = VramSpriteList.find(vramId);
    if (it != VramSpriteList.end())
    {
        return vramId;
    }

    const Material& material = MaterialList.at(materialId);
    const Texture& texture = textureSystem.TextureList.at(material.AlbedoMapId);

    SpriteVram sprite = SpriteVram
    {
        .VramSpriteID = vramId,
        .SpriteMaterialID = materialId,
        .SpriteLayer = json["SpriteLayer"],
        .SpriteColor = vec4{ json["SpriteColor"][0], json["SpriteColor"][1], json["SpriteColor"][2], json["SpriteColor"][3] },
        .SpritePixelSize = ivec2{ json["SpritePixelSize"][0], json["SpritePixelSize"][1] },
        .SpriteScale = ivec2{ json["SpriteScale"][0], json["SpriteScale"][1] },
        .SpriteCells = ivec2(texture.width / sprite.SpritePixelSize.x, texture.height / sprite.SpritePixelSize.y),
        .SpriteUVSize = vec2(1.0f / (float)sprite.SpriteCells.x, 1.0f / (float)sprite.SpriteCells.y),
        .SpriteSize = vec2(sprite.SpritePixelSize.x * sprite.SpriteScale.x, sprite.SpritePixelSize.y * sprite.SpriteScale.y),
    };

    for (int x = 0; x < json["AnimationList"].size(); x++)
    {
        Animation2D animation = Animation2D
        {
            .AnimationId = json["AnimationList"][x]["AnimationId"],
            .FrameHoldTime = json["AnimationList"][x]["FrameHoldTime"]
        };

        Vector<ivec2> FrameList;
        for (int y = 0; y < json["AnimationList"][x]["FrameList"].size(); y++)
        {
            FrameList.emplace_back(ivec2{ json["AnimationList"][x]["FrameList"][y][0], json["AnimationList"][x]["FrameList"][y][1] });
        }

        assetManager.AnimationList[animation.AnimationId] = animation;
        assetManager.AnimationFrameList[vramId].emplace_back(FrameList);
    }

    VramSpriteList[vramId] = sprite;
    return vramId;
}

VkGuid AssetManager::AddTileSetVRAM(const String& tileSetPath)
{
    if (tileSetPath.empty() ||
        tileSetPath == "")
    {
        return VkGuid();
    }

    nlohmann::json json = Json::ReadJson(tileSetPath);
    VkGuid tileSetId = VkGuid(json["TileSetId"].get<String>().c_str());
    VkGuid materialId = VkGuid(json["MaterialId"].get<String>().c_str());

    auto it = LevelTileSetList.find(tileSetId);
    if (it != LevelTileSetList.end())
    {
        return tileSetId;
    }

    const Material& material = MaterialList[materialId];
    const Texture& tileSetTexture = textureSystem.TextureList[material.AlbedoMapId];

    LevelTileSet tileSet = LevelTileSet();
    tileSet.TileSetId = VkGuid(json["TileSetId"].get<String>().c_str());
    tileSet.MaterialId = materialId;
    tileSet.TilePixelSize = ivec2{ json["TilePixelSize"][0], json["TilePixelSize"][1] };
    tileSet.TileSetBounds = ivec2{ tileSetTexture.width / tileSet.TilePixelSize.x,  tileSetTexture.height / tileSet.TilePixelSize.y };
    tileSet.TileUVSize = vec2(1.0f / (float)tileSet.TileSetBounds.x, 1.0f / (float)tileSet.TileSetBounds.y);
    for (int x = 0; x < json["TileList"].size(); x++)
    {
        Tile tile;
        tile.TileId = json["TileList"][x]["TileId"];
        tile.TileUVCellOffset = ivec2(json["TileList"][x]["TileUVCellOffset"][0], json["TileList"][x]["TileUVCellOffset"][1]);
        tile.TileLayer = json["TileList"][x]["TileLayer"];
        tile.IsAnimatedTile = json["TileList"][x]["IsAnimatedTile"];
        tile.TileUVOffset = vec2(tile.TileUVCellOffset.x * tileSet.TileUVSize.x, tile.TileUVCellOffset.y * tileSet.TileUVSize.y);
        tileSet.LevelTileList.emplace_back(tile);
    }
    LevelTileSetList[tileSetId] = tileSet;
    return tileSetId;
}

VkGuid AssetManager::LoadMaterial(const String& materialPath)
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

VkGuid AssetManager::LoadLevelLayout(const String& levelLayoutPath)
{
    if (levelLayoutPath.empty() ||
        levelLayoutPath == "")
    {
        return VkGuid();
    }

    nlohmann::json json = Json::ReadJson(levelLayoutPath);
    VkGuid levelLayoutId = VkGuid(json["LevelLayoutId"].get<String>().c_str());

    levelLayout.LevelLayoutId = VkGuid(json["LevelLayoutId"].get<String>().c_str());
    levelLayout.LevelBounds = ivec2(json["LevelBounds"][0], json["LevelBounds"][1]);
    levelLayout.TileSizeinPixels = ivec2(json["TileSizeInPixels"][0], json["TileSizeInPixels"][1]);
    for (int x = 0; x < json["LevelLayouts"].size(); x++)
    {
        Vector<uint> levelLayer;
        for (int y = 0; y < json["LevelLayouts"][x].size(); y++)
        {
            for (int z = 0; z < json["LevelLayouts"][x][y].size(); z++)
            {
                levelLayer.emplace_back(json["LevelLayouts"][x][y][z]);
            }
        }
        levelLayout.LevelMapList.emplace_back(levelLayer);
    }
}

void AssetManager::DestroyEntity(RenderPassID id)
{
	//FreeIds.push(id);
}

void AssetManager::DestroyGameObject(GameObjectID id)
{
	//MeshList[id].Destroy();

	//MeshList.erase(id);
	//SpriteList.erase(id);
	//TransformComponentList.erase(id);
	//GameObjectList.erase(id);
}

void AssetManager::DestroyGameObjects()
{
	//for (auto& gameObject : GameObjectList)
	//{
	//	const UM_GameObjectID id = gameObject.second.GameObjectId;
	//	MeshList[id].Destroy();
	//}
	//MeshList.clear();
	//SpriteList.clear();
	//TransformComponentList.clear();
	//GameObjectList.clear();
}

void AssetManager::DestoryTextures()
{
	//for (auto& texture : TextureList)
	//{
	//	const VkGuid id = texture.second.TextureId;
	//	TextureList[id].Destroy();
	//}
	//TextureList.clear();
}

void AssetManager::DestoryMaterials()
{
	//for (auto& material : MaterialList)
	//{
	//	const VkGuid id = material.second.MaterialId;
	//	MaterialList[id].Destroy();
	//}
	//MaterialList.clear();
}

void AssetManager::DestoryVRAMSprites()
{
	//for (auto& spriteVram : VramSpriteList)
	//{
	//	/*const uint id = spriteVRAM.second.VRAMSpriteID;
	//	VRAMSpriteList.erase(id);*/
	//}
	//VramSpriteList.clear();
}

void AssetManager::Destroy()
{
	DestroyGameObjects();
	DestoryTextures();
	DestoryMaterials();
	DestoryVRAMSprites();
}

