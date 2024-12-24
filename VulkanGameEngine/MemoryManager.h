#pragma once
#include "MemoryPool.h"
#include "GameObject.h"
#include "RenderMesh2DComponent.h"
#include "Texture.h"
#include "JsonRenderPass.h"
#include "JsonPipeline.h"

void ExceptionCallback(std::string_view InMessage);
class MemoryManager 
{
	private:
		static SharedPtr<Coral::ManagedAssembly> ECSassembly;
		static List<SharedPtr<GameObject>> GameObjectList;
		static List<SharedPtr<RenderMesh2DComponent>> RenderMesh2DComponentList;
		static List<SharedPtr<Texture>> TextureList;
		static List<SharedPtr<JsonRenderPass>> JsonRenderPassList;
		static List<SharedPtr<JsonPipeline>> JsonPipelineList;

		static MemoryPool<GameObject> GameObjectMemoryPool;
		static MemoryPool<Texture> TextureMemoryPool;
		static MemoryPool<RenderMesh2DComponent> RenderMesh2DComponentMemoryPool;
		static MemoryPool<JsonRenderPass> JsonRenderPassMemoryPool;
		static MemoryPool<JsonPipeline> JsonPipelineMemoryPool;

	public:
		static void SetUpMemoryManager(uint32_t EstObjectCount);
		static SharedPtr<GameObject> AllocateNewGameObject();
		static SharedPtr<Texture> AllocateNewTexture();
		static SharedPtr<RenderMesh2DComponent> AllocateRenderMesh2DComponent();
		static SharedPtr<JsonRenderPass> AllocateJsonRenderPass();
		static SharedPtr<JsonPipeline> AllocateJsonPipeline();
		static void Destroy();

		static void ViewGameObjectMemoryMap();
		static void ViewTextureMemoryMap();
		static void ViewRenderMesh2DComponentMemoryMap();
		static void ViewJsonRenderPassMemoryMap();
		static void ViewJsonPipelineMemoryMap();
		static void ViewMemoryMap();

		static std::vector<VkDescriptorBufferInfo> GetGameObjectPropertiesBuffer();
		static std::vector<VkDescriptorImageInfo> GetTexturePropertiesBuffer();
		static const List<SharedPtr<GameObject>>& GetGameObjectList() { return GameObjectList; }
		static const List<SharedPtr<RenderMesh2DComponent>>& GetRenderMesh2DComponentList() { return RenderMesh2DComponentList; }
		static const List<SharedPtr<Texture>>& GetTextureList() { return TextureList; }
		static const List<SharedPtr<JsonRenderPass>>& GetJsonRenderPassList() { return JsonRenderPassList; }
		static const List<SharedPtr<JsonPipeline>>& GetJsonPipelineList() { return JsonPipelineList; }
		static const SharedPtr<Coral::ManagedAssembly> GetECSassemblyModule() { return ECSassembly; }
};