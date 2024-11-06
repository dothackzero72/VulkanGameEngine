#pragma once
#include "MemoryPool.h"
#include "GameObject.h"
#include "RenderMesh2DComponent.h"
#include "Texture.h"
#include "JsonRenderPass.h"
#include "JsonPipeline.h"

class MemoryManager {
private:
	static List<std::shared_ptr<GameObject>> GameObjectList;
	static List<std::shared_ptr<RenderMesh2DComponent>> RenderMesh2DComponentList;
	static List<std::shared_ptr<Texture>> TextureList;
	static List<std::shared_ptr<JsonRenderPass>> JsonRenderPassList;
	static List<std::shared_ptr<JsonPipeline>> JsonPipelineList;

	static MemoryPool<GameObject> GameObjectMemoryPool;
	static MemoryPool<Texture> TextureMemoryPool;
	static MemoryPool<RenderMesh2DComponent> RenderMesh2DComponentMemoryPool;
	static MemoryPool<JsonRenderPass> JsonRenderPassMemoryPool;
	static MemoryPool<JsonPipeline> JsonPipelineMemoryPool;

public:

	static void SetUpMemoryManager(uint32_t EstObjectCount);
	static std::shared_ptr<GameObject> AllocateNewGameObject();
	static std::shared_ptr<Texture> AllocateNewTexture();
	static std::shared_ptr<RenderMesh2DComponent> AllocateRenderMesh2DComponent();
	static std::shared_ptr<JsonRenderPass> AllocateJsonRenderPass();
	static std::shared_ptr<JsonPipeline> AllocateJsonPipeline();
	static void Destroy();

	static void ViewGameObjectMemoryMap();
	static void ViewTextureMemoryMap();
	static void ViewRenderMesh2DComponentMemoryMap();
	static void ViewJsonRenderPassMemoryMap();
	static void ViewJsonPipelineMemoryMap();
	static void ViewMemoryMap();

	static std::vector<VkDescriptorBufferInfo> GetGameObjectPropertiesBuffer();
	static std::vector<VkDescriptorImageInfo> GetTexturePropertiesBuffer();
	static const List<std::shared_ptr<GameObject>>& GetGameObjectList() { return GameObjectList; }
	static const List<std::shared_ptr<RenderMesh2DComponent>>& GetRenderMesh2DComponentList() { return RenderMesh2DComponentList; }
	static const List<std::shared_ptr<Texture>>& GetTextureList() { return TextureList; }
	static const List<std::shared_ptr<JsonRenderPass>>& GetJsonRenderPassList() { return JsonRenderPassList; }
	static const List<std::shared_ptr<JsonPipeline>>& GetJsonPipelineList() { return JsonPipelineList; }
};