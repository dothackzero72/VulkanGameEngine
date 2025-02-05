#pragma once
#include "MemoryPool.h"
#include "GameObject.h"
#include "RenderMesh2DComponent.h"
#include "Texture.h"
#include "JsonRenderPass.h"
#include "JsonPipeline.h"
#include "Material.h"
#include "Mesh2D.h"
#include "SpriteBatchLayer.h"
#include "SpriteComponent.h"

void ExceptionCallback(std::string_view InMessage);
class MemoryManager 
{
	private:
		static Coral::HostInstance hostInstance;
		static SharedPtr<Coral::ManagedAssembly> ECSassembly;
		static Vector<SharedPtr<GameObject>> GameObjectList;
		static Vector<SharedPtr<SpriteComponent>> SpriteComponentList;
		static Vector<SharedPtr<Texture>> TextureList;
		static Vector<SharedPtr<Material>> MaterialList;
		static Vector<SharedPtr<JsonRenderPass>> JsonRenderPassList;
		static Vector<SharedPtr<JsonPipeline>> JsonPipelineList;
		static Vector<SharedPtr<Mesh>> MeshList;
		static Vector<SharedPtr<Mesh2D>> Mesh2DList;
		static Vector<SharedPtr<SpriteBatchLayer>> SpriteBatchLayerList;

		//static MemoryPool<GameObject> GameObjectMemoryPool;
		//static MemoryPool<Texture> TextureMemoryPool;
		//static MemoryPool<Material> MaterialMemoryPool;
		//static MemoryPool<SpriteComponent> SpriteComponentMemoryPool;
		//static MemoryPool<JsonRenderPass> JsonRenderPassMemoryPool;
		//static MemoryPool<JsonPipeline> JsonPipelineMemoryPool;
		//static MemoryPool<Mesh2D> Mesh2DMemoryPool;
		//static MemoryPool<SpriteBatchLayer> SpriteBatchLayerMemeryPool;

	public:
		static void SetUpMemoryManager(uint32 EstObjectCount);
		//static SharedPtr<GameObject> AllocateNewGameObject();
		//static SharedPtr<Mesh2D> AllocateMesh2D();
		//static SharedPtr<Texture> AllocateNewTexture();
		//static SharedPtr<SpriteComponent> AllocateSpriteComponent();
		//static SharedPtr<JsonRenderPass> AllocateJsonRenderPass();
		//static SharedPtr<JsonPipeline> AllocateJsonPipeline();
		//static SharedPtr<SpriteBatchLayer> AllocateSpriteBatchLayer();
		//static SharedPtr<Material> AllocateMaterial();

		static void Update(float deltaTime);
		static void UpdateBufferIndex();
		static void Destroy();

		static void ViewMemoryMap();

		static const Vector<VkDescriptorBufferInfo> GetMeshPropertiesBuffer();
		static const Vector<VkDescriptorBufferInfo> GetMaterialPropertiesBuffer();
		static const Vector<VkDescriptorImageInfo> GetTexturePropertiesBuffer();

		static const Vector<SharedPtr<GameObject>>& GetGameObjectList() { return GameObjectList; }
		static const Vector<SharedPtr<SpriteComponent>>& GetSpriteComponentList() { return SpriteComponentList; }
		static const Vector<SharedPtr<Texture>>& GetTextureList() { return TextureList; }
		static const Vector<SharedPtr<JsonRenderPass>>& GetJsonRenderPassList() { return JsonRenderPassList; }
		static const Vector<SharedPtr<JsonPipeline>>& GetJsonPipelineList() { return JsonPipelineList; }
		static const Vector<SharedPtr<Mesh>>& GetMeshListList() { return MeshList; }
		static const Vector<SharedPtr<Mesh2D>>& GetMesh2DList() { return Mesh2DList; }
		static const Vector<SharedPtr<Material>>& GetMaterialist() { return MaterialList; }
		static const SharedPtr<Coral::ManagedAssembly> GetECSassemblyModule() { return ECSassembly; }
};