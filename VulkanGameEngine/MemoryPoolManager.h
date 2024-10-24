#pragma once
#include "MemoryPool.h"
#include "GameObject.h"
#include "RenderMesh2DComponent.h"

class MemoryPoolManager {
private:
	static List<std::shared_ptr<GameObject>> GameObjectList;

	static MemoryPool<GameObject> GameObjectMemoryPool;


public:
	static List<std::shared_ptr<RenderMesh2DComponent>> RenderMesh2DComponentList;
	static MemoryPool<RenderMesh2DComponent> RenderMesh2DComponentMemoryPool;
	static void SetUpMemoryPoolManager(uint32_t EstObjectCount);
	static std::shared_ptr<GameObject> AllocateNewGameObject();
	static std::shared_ptr<RenderMesh2DComponent> AllocateRenderMesh2DComponent();
	static void ViewMemoryPool();

	//static std::vector<VkDescriptorBufferInfo> GetVertexPropertiesBuffer();
	//static std::vector<VkDescriptorBufferInfo> GetIndexPropertiesBuffer();
	static std::vector<VkDescriptorBufferInfo> GetGameObjectPropertiesBuffer();
	static void Destroy();
	//static std::vector<VkDescriptorBufferInfo> GetGameObjectTransformBuffer();

	//static std::vector<VkDescriptorImageInfo> GetTexturePropertiesBuffer()
	//{
	//	std::vector<VkDescriptorImageInfo>	TexturePropertiesBuffer;
	//	if (TextureList.size() == 0)
	//	{
	//		TexturePropertiesBuffer.emplace_back(GetNullDescriptor());
	//	}
	//	else
	//	{
	//		for (auto& texture : TextureList)
	//		{
	//			VkDescriptorImageInfo albedoTextureDescriptor{};
	//			albedoTextureDescriptor.imageLayout = VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL;
	//			albedoTextureDescriptor.imageView = texture->GetView();
	//			albedoTextureDescriptor.sampler = texture->GetSampler();
	//			TexturePropertiesBuffer.emplace_back(albedoTextureDescriptor);
	//		}
	//	}

	//	return TexturePropertiesBuffer;
	//}

	//static std::vector<VkDescriptorBufferInfo> GetMaterialPropertiesBuffer()
	//{
	//	std::vector<VkDescriptorBufferInfo>	MaterialPropertiesBuffer;
	//	if (MaterialList.size() == 0)
	//	{
	//		VkDescriptorBufferInfo nullBuffer;
	//		nullBuffer.buffer = VK_NULL_HANDLE;
	//		nullBuffer.offset = 0;
	//		nullBuffer.range = VK_WHOLE_SIZE;
	//		MaterialPropertiesBuffer.emplace_back(nullBuffer);
	//	}
	//	else
	//	{
	//		for (auto& material : MaterialList)
	//		{
	//			material->GetMaterialPropertiesBuffer(MaterialPropertiesBuffer);
	//		}
	//	}

	//	return MaterialPropertiesBuffer;
	//}
};