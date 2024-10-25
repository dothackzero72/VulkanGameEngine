#pragma once
#include "MemoryPool.h"
#include "GameObject.h"
#include "RenderMesh2DComponent.h"
#include "Texture.h"

class MemoryManager {
private:
	static List<std::shared_ptr<GameObject>> GameObjectList;
	static MemoryPool<GameObject> GameObjectMemoryPool;


public:
	/*static List<std::shared_ptr<Texture>> TextureList;
	static MemoryPool<Texture> TextureMemoryPool;*/

	static List<std::shared_ptr<RenderMesh2DComponent>> RenderMesh2DComponentList;
	static MemoryPool<RenderMesh2DComponent> RenderMesh2DComponentMemoryPool;

	static void SetUpMemoryManager(uint32_t EstObjectCount);
	static std::shared_ptr<GameObject> AllocateNewGameObject();
	//static std::shared_ptr<Texture> AllocateNewTexture();
	static std::shared_ptr<RenderMesh2DComponent> AllocateRenderMesh2DComponent();
	static void ViewMemoryMap();

	//static std::vector<VkDescriptorBufferInfo> GetVertexPropertiesBuffer();
	//static std::vector<VkDescriptorBufferInfo> GetIndexPropertiesBuffer();
	static std::vector<VkDescriptorBufferInfo> GetGameObjectPropertiesBuffer();
	static void Destroy();
	//static std::vector<VkDescriptorBufferInfo> GetGameObjectTransformBuffer();

	//static std::vector<VkDescriptorImageInfo> GetTexturePropertiesBuffer();
};