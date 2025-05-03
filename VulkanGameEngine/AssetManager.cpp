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

