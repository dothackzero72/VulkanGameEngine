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

class MyClass {
public:
	MyClass() {
		std::cout << "MyClass Constructor called!" << std::endl;
	}
	~MyClass() {
		std::cout << "MyClass Destructor called!" << std::endl;
	}
	void Destroy()
	{
		std::cout << "MyClass Destroy called!" << std::endl;
	}
};

void ExceptionCallback(std::string_view InMessage);
class MemoryManager 
{
	private:
		static Coral::HostInstance hostInstance;
		static SharedPtr<Coral::ManagedAssembly> ECSassembly;
		static List<SharedPtr<GameObject>> GameObjectList;
		//static List<SharedPtr<RenderMesh2DComponent>> RenderMesh2DComponentList;
		static List<SharedPtr<Texture>> TextureList;
		static List<SharedPtr<Material>> MaterialList;
		static List<SharedPtr<JsonRenderPass>> JsonRenderPassList;
		static List<SharedPtr<JsonPipeline>> JsonPipelineList;
		static List<SharedPtr<Mesh>> MeshList;
		static List<SharedPtr<Mesh2D>> Mesh2DList;
		static List<SharedPtr<SpriteBatchLayer>> SpriteBatchLayerList;

		static MemoryPool<GameObject> GameObjectMemoryPool;
		static MemoryPool<Texture> TextureMemoryPool;
		static MemoryPool<Material> MaterialMemoryPool;
		//static MemoryPool<RenderMesh2DComponent> RenderMesh2DComponentMemoryPool;
		static MemoryPool<JsonRenderPass> JsonRenderPassMemoryPool;
		static MemoryPool<JsonPipeline> JsonPipelineMemoryPool;
		static MemoryPool<Mesh2D> Mesh2DMemoryPool;
		static MemoryPool<SpriteBatchLayer> SpriteBatchLayerMemeryPool;
		static MemoryPool<MyClass> myclassMemoryPool;

	public:
		static void SetUpMemoryManager(uint32 EstObjectCount);
		static SharedPtr<GameObject> AllocateNewGameObject();
		static SharedPtr<Mesh2D> AllocateMesh2D();
		static SharedPtr<Texture> AllocateNewTexture();
		//static SharedPtr<RenderMesh2DComponent> AllocateRenderMesh2DComponent();
		static SharedPtr<JsonRenderPass> AllocateJsonRenderPass();
		static SharedPtr<JsonPipeline> AllocateJsonPipeline();
		static SharedPtr<SpriteBatchLayer> AllocateSpriteBatchLayer();
		static SharedPtr<Material> AllocateMaterial();

		static void Update(float deltaTime);
		static void UpdateBufferIndex();
		static void Destroy();

		static void ViewMemoryMap();

		static const List<VkDescriptorBufferInfo> GetMeshPropertiesBuffer();
		static const List<VkDescriptorBufferInfo> GetMaterialPropertiesBuffer();
		static const List<VkDescriptorImageInfo> GetTexturePropertiesBuffer();

		static const List<SharedPtr<GameObject>>& GetGameObjectList() { return GameObjectList; }
		//static const List<SharedPtr<RenderMesh2DComponent>>& GetRenderMesh2DComponentList() { return RenderMesh2DComponentList; }
		static const List<SharedPtr<Texture>>& GetTextureList() { return TextureList; }
		static const List<SharedPtr<JsonRenderPass>>& GetJsonRenderPassList() { return JsonRenderPassList; }
		static const List<SharedPtr<JsonPipeline>>& GetJsonPipelineList() { return JsonPipelineList; }
		static const List<SharedPtr<Mesh>>& GetMeshListList() { return MeshList; }
		static const List<SharedPtr<Mesh2D>>& GetMesh2DList() { return Mesh2DList; }
		static const List<SharedPtr<Material>>& GetMaterialist() { return MaterialList; }
		static const SharedPtr<Coral::ManagedAssembly> GetECSassemblyModule() { return ECSassembly; }
};