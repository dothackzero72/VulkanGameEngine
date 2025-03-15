#pragma once
#include "MemoryPool.h"
#include "GameObject.h"
#include "RenderMesh2DComponent.h"
#include <Texture.h>
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

		//static Vector<JsonRenderPass> JsonRenderPassList;
		//static Vector<JsonPipeline> JsonPipelineList;

		//static Vector<SharedPtr<JsonRenderPass>> SharedJsonRenderPassPtrList;
		//static Vector<SharedPtr<JsonPipeline>> SharedJsonPipelinePtrList;

	public:
		static void SetUpMemoryManager(uint32 EstObjectCount);

		//static void Update(float deltaTime);
		//static void UpdateBufferIndex();
		//static void Destroy();

		//static SharedPtr<Texture> AddTexture(Texture texture);
		//static SharedPtr<Material> AddMaterial(Material material);
		//static SharedPtr<JsonRenderPass> AddJsonRenderPass(JsonRenderPass jsonRenderPass);
		//static SharedPtr<JsonPipeline> AddJsonPipeline(JsonPipeline jsonPipeline);
		//static SharedPtr<Mesh> AddMesh(Mesh mesh);

		//static const Vector<VkDescriptorBufferInfo> GetMeshPropertiesBuffer();
		//static const Vector<VkDescriptorBufferInfo> GetMaterialPropertiesBuffer();
		//static const Vector<VkDescriptorImageInfo> GetTexturePropertiesBuffer();

		//static const Vector<SharedPtr<JsonRenderPass>>& GetJsonRenderPassList() { return SharedJsonRenderPassPtrList; }
		//static const Vector<SharedPtr<JsonPipeline>>& GetJsonPipelineList() { return SharedJsonPipelinePtrList; }
		static const SharedPtr<Coral::ManagedAssembly> GetECSassemblyModule() { return ECSassembly; }
};