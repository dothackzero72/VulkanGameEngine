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

		static Vector<GameObject> GameObjectList;
		static Vector<SharedPtr<GameObjectComponent>> GameObjectComponentList;
		static Vector<Texture> TextureList;
		static Vector<Material> MaterialList;
		static Vector<JsonRenderPass> JsonRenderPassList;
		static Vector<JsonPipeline> JsonPipelineList;
		static Vector<Mesh> MeshList;
		static Vector<SpriteBatchLayer> SpriteBatchLayerList;
		static Vector<Sprite> SpriteList;

		static Vector<SharedPtr<GameObject>> SharedGameObjectPtrList;
		static Vector<SharedPtr<Texture>> SharedTexturePtrList;
		static Vector<SharedPtr<Material>> SharedMaterialPtrList;
		static Vector<SharedPtr<JsonRenderPass>> SharedJsonRenderPassPtrList;
		static Vector<SharedPtr<JsonPipeline>> SharedJsonPipelinePtrList;
		static Vector<SharedPtr<Mesh>> SharedMeshPtrList;
		static Vector<SharedPtr<Mesh2D>> SharedMesh2DPtrList;
		static Vector<SharedPtr<SpriteBatchLayer>> SharedSpriteBatchLayerPtrList;
		static Vector<SharedPtr<Sprite>> SharedSpritePtrList;

	public:
		static void SetUpMemoryManager(uint32 EstObjectCount);

		static void Update(float deltaTime);
		static void UpdateBufferIndex();
		static void Destroy();

		static SharedPtr<GameObject> AddGameObject(GameObject gameObject);
		static SharedPtr<GameObjectComponent> AddGameObjectComponent(SharedPtr<GameObjectComponent> gameObjectComponent);
		static SharedPtr<Texture> AddTexture(Texture texture);
		static SharedPtr<Material> AddMaterial(Material material);
		static SharedPtr<JsonRenderPass> AddJsonRenderPass(JsonRenderPass jsonRenderPass);
		static SharedPtr<JsonPipeline> AddJsonPipeline(JsonPipeline jsonPipeline);
		static SharedPtr<Mesh> AddMesh(Mesh mesh);
		static SharedPtr<SpriteBatchLayer> AddSpriteBatchLayer(SpriteBatchLayer spriteBatchLayer);
		static SharedPtr<Sprite> AddSprite(Sprite sprite);

		static const Vector<VkDescriptorBufferInfo> GetMeshPropertiesBuffer();
		static const Vector<VkDescriptorBufferInfo> GetMaterialPropertiesBuffer();
		static const Vector<VkDescriptorImageInfo> GetTexturePropertiesBuffer();

		static const Vector<SharedPtr<GameObject>>& GetGameObjectList() { return SharedGameObjectPtrList; }
		static const Vector<SharedPtr<GameObjectComponent>>& GetGameObjectComponentList() { return GameObjectComponentList; }
		static const Vector<SharedPtr<Texture>>& GetTextureList() { return SharedTexturePtrList; }
		static const Vector<SharedPtr<JsonRenderPass>>& GetJsonRenderPassList() { return SharedJsonRenderPassPtrList; }
		static const Vector<SharedPtr<JsonPipeline>>& GetJsonPipelineList() { return SharedJsonPipelinePtrList; }
		static const Vector<SharedPtr<Mesh>>& GetMeshListList() { return SharedMeshPtrList; }
		static const Vector<SharedPtr<Mesh2D>>& GetMesh2DList() { return SharedMesh2DPtrList; }
		static const Vector<SharedPtr<Material>>& GetMaterialist() { return SharedMaterialPtrList; }
		static const Vector<SharedPtr<Sprite>>& GetSpriteList() { return SharedSpritePtrList; }
		static const SharedPtr<Coral::ManagedAssembly> GetECSassemblyModule() { return ECSassembly; }
};