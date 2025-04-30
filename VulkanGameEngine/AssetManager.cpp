#include "AssetManager.h"
#include "Material.h"
#include <json.h>
#include "RenderSystem.h"

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

void AssetManager::CreateGameObject(uint renderPassId, const String& name, const Vector<ComponentTypeEnum>& gameObjectComponentTypeList, VkGuid vramId, vec2 objectPosition)
{
	GameObject gameObject = GameObject(name, Vector<ComponentTypeEnum> { kTransform2DComponent, kSpriteComponent }, 0);
	assetManager.GameObjectList[gameObject.GameObjectId] = gameObject;
	//renderSystem.SpriteBatchLayerObjectList[renderPassId].emplace_back(gameObject.GameObjectId);

	Vector<GameObjectComponent> gameObjectComponentList;
	for (auto component : gameObjectComponentTypeList)
	{
		switch (component)
		{
		case kTransform2DComponent: assetManager.TransformComponentList[gameObject.GameObjectId] = Transform2DComponent(gameObject.GetId(), objectPosition, name); break;
			// case kInputComponent: gameObject->AddComponent(std::make_shared<InputComponent>(InputComponent(gameObject->GetId(), name))); break;
		case kSpriteComponent: assetManager.SpriteList[gameObject.GameObjectId] = Sprite(gameObject.GetId(), vramId); break;
		}
	}
}

void AssetManager::DestroyEntity(uint32_t id)
{
	FreeIds.push(id);
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

