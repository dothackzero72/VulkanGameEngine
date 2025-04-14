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
	if (!FreeIds.empty())
	{
		GameObjectList.emplace_back(FreeIds.front());
		FreeIds.pop();
		return;
	}
	GameObjectList.emplace_back(++NextId);
}

void AssetManager::DestroyEntity(uint32_t id)
{
	FreeIds.push(id);
}