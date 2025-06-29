#include "SpriteBatchLayer.h"
#include "GameObjectSystem.h"
#include "BufferSystem.h"
#include "Typedef.h"
#include "MeshSystem.h"
#include "LevelSystem.h"
#include "SpriteSystem.h"

uint32 SpriteBatchLayer::NextSpriteBatchLayerID = 0;

SpriteBatchLayer::SpriteBatchLayer()
{

}

SpriteBatchLayer::SpriteBatchLayer(VkGuid& renderPassId)
{                            
	SpriteBatchLayerID = ++NextSpriteBatchLayerID;

	for (int x = 0; x < spriteSystem.SpriteListRef().size(); x++)
	{
		spriteSystem.AddSpriteBatchObjectList(SpriteBatchLayerID, GameObjectID(x + 1));
	}

	SpriteLayerMeshId = meshSystem.CreateSpriteLayerMesh(gameObjectSystem.SpriteVertexList, gameObjectSystem.SpriteIndexList);
	Vector<SpriteInstanceStruct> af = Vector<SpriteInstanceStruct>(spriteSystem.FindSpriteBatchObjectListMap(SpriteBatchLayerID).size());
	spriteSystem.AddSpriteInstanceLayerList(SpriteBatchLayerID, af);
	spriteSystem.AddSpriteInstanceBufferId(SpriteBatchLayerID, bufferSystem.CreateVulkanBuffer<SpriteInstanceStruct>(renderSystem.renderer, spriteSystem.FindSpriteInstanceList(SpriteBatchLayerID), MeshBufferUsageSettings, MeshBufferPropertySettings, false));
}

SpriteBatchLayer::~SpriteBatchLayer()
{
}

void SpriteBatchLayer::Update(VkCommandBuffer& commandBuffer, const float& deltaTime)
{
	spriteSystem.FindSpriteInstanceList(SpriteBatchLayerID).clear();
	spriteSystem.FindSpriteInstanceList(SpriteBatchLayerID).reserve(spriteSystem.FindSpriteBatchObjectListMap(SpriteBatchLayerID).size());
	for (auto& gameObjectID : spriteSystem.FindSpriteBatchObjectListMap(SpriteBatchLayerID))
	{
		spriteSystem.FindSpriteInstanceList(SpriteBatchLayerID).emplace_back(*spriteSystem.FindSpriteInstance(gameObjectID));
	}

	if (spriteSystem.FindSpriteBatchObjectListMap(SpriteBatchLayerID).size())
	{
		bufferSystem.UpdateBufferMemory(renderSystem.renderer, spriteSystem.FindSpriteInstanceBufferId(SpriteBatchLayerID), spriteSystem.FindSpriteInstanceList(SpriteBatchLayerID));
	}
}