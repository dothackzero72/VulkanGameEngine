#include "MemoryManager.h"
#include <CBuffer.h>

List<std::shared_ptr<GameObject>> MemoryManager::GameObjectList;
List<std::shared_ptr<RenderMesh2DComponent>> MemoryManager::RenderMesh2DComponentList;
//List<std::shared_ptr<Texture>> MemoryManager::TextureList;

MemoryPool<GameObject> MemoryManager::GameObjectMemoryPool;
MemoryPool<RenderMesh2DComponent> MemoryManager::RenderMesh2DComponentMemoryPool;
//MemoryPool<Texture> MemoryManager::TextureMemoryPool;

void MemoryManager::SetUpMemoryManager(uint32_t EstObjectCount)
{
	GameObjectMemoryPool.CreateMemoryPool(EstObjectCount);
	RenderMesh2DComponentMemoryPool.CreateMemoryPool(EstObjectCount);
	//TextureMemoryPool.CreateMemoryPool(EstObjectCount);
}

std::shared_ptr<GameObject> MemoryManager::AllocateNewGameObject()
{
	GameObjectList.emplace_back(GameObjectMemoryPool.AllocateMemoryLocation());
	return GameObjectList.back();
}

//std::shared_ptr<Texture> MemoryManager::AllocateNewTexture()
//{
//	TextureList.emplace_back(TextureMemoryPool.AllocateMemoryLocation());
//	return TextureList.back();
//}

std::shared_ptr<RenderMesh2DComponent> MemoryManager::AllocateRenderMesh2DComponent()
{
	RenderMesh2DComponentList.emplace_back(RenderMesh2DComponentMemoryPool.AllocateMemoryLocation());
	return RenderMesh2DComponentList.back();
}

void MemoryManager::ViewMemoryMap()
{
	const auto gameObjectMemoryPool = GameObjectMemoryPool.ViewMemoryPool();
	const auto renderMesh2DComponentMemoryPool = RenderMesh2DComponentMemoryPool.ViewMemoryPool();
	//const auto textureMemoryPool = TextureMemoryPool.ViewMemoryPool();
	

	std::cout << "Memory Map of the Game Object Pool(" << sizeof(GameObject) << " bytes each," << std::to_string(sizeof(GameObject) * renderMesh2DComponentMemoryPool.size()) << " bytes total): " << std::endl;
	std::cout << std::setw(20) << "Address" << std::setw(15) << "Value" << std::endl;
	for (size_t x = 0; x < gameObjectMemoryPool.size(); x++)
	{
		if (GameObjectMemoryPool.ViewMemoryBlockUsage()[x] == 1)
		{
			std::cout << std::setw(10) << std::hex << "0x" << &gameObjectMemoryPool[x] << ": " << std::setw(15) << gameObjectMemoryPool[x]->Name << std::endl;
		}
		else
		{
			std::cout << std::hex << "0x" << &gameObjectMemoryPool[x] << ": " << "nullptr" << std::endl;
		}
	}
	std::cout << "" << std::endl << std::endl;

	/*std::cout << "Memory Map of the Texture Memory Pool(" << sizeof(Texture) << " bytes each," << std::to_string(sizeof(Texture) * TextureList.size()) << " bytes total): " << std::endl;
	std::cout << std::setw(20) << "Address" << std::setw(15) << "Value" << std::endl;
	for (size_t x = 0; x < textureMemoryPool.size(); x++)
	{
		if (TextureMemoryPool.ViewMemoryBlockUsage()[x] == 1)
		{
			std::cout << std::setw(10) << std::hex << "0x" << &textureMemoryPool[x] << ": " << std::setw(15) << textureMemoryPool[x]->Name << std::endl;
		}
		else
		{
			std::cout << std::hex << "0x" << &textureMemoryPool[x] << ": " << "nullptr" << std::endl;
		}
	}
	std::cout << "" << std::endl << std::endl;*/

	std::cout << "Memory Map of the RenderComponent Memory Pool(" << sizeof(RenderMesh2DComponent) << " bytes each," << std::to_string(sizeof(RenderMesh2DComponent) * renderMesh2DComponentMemoryPool.size()) << " bytes total): " << std::endl;
	std::cout << std::setw(20) << "Address" << std::setw(15) << "Value" << std::endl;
	for (size_t x = 0; x < renderMesh2DComponentMemoryPool.size(); x++)
	{
		if (RenderMesh2DComponentMemoryPool.ViewMemoryBlockUsage()[x] == 1)
		{
			std::cout << std::setw(10) << std::hex << "0x" << &renderMesh2DComponentMemoryPool[x] << ": " << std::setw(15) << renderMesh2DComponentMemoryPool[x]->Name << std::endl;
		}
		else
		{
			std::cout << std::hex << "0x" << &renderMesh2DComponentMemoryPool[x] << ": " << "nullptr" << std::endl;
		}
	}
	std::cout << "" << std::endl << std::endl;
}

// std::vector<VkDescriptorBufferInfo>  MemoryPoolManager::GetVertexPropertiesBuffer()
//{
//	std::vector<VkDescriptorBufferInfo>	VertexPropertiesBuffer;
//	if (GameObjectList.size() == 0)
//	{
//		std::vector<VkDescriptorBufferInfo>	VertexPropertiesBuffer;
//		VkDescriptorBufferInfo nullBuffer;
//		nullBuffer.buffer = VK_NULL_HANDLE;
//		nullBuffer.offset = 0;
//		nullBuffer.range = VK_WHOLE_SIZE;
//		VertexPropertiesBuffer.emplace_back(nullBuffer);
//	}
//	else
//	{
//		
//		for (auto& mesh : RenderMesh2DComponentList)
//		{
//			VkDescriptorBufferInfo MeshProperitesBufferInfo = {};
//			MeshProperitesBufferInfo.buffer = mesh->GetVertexPropertiesBuffer().buffer;
//			MeshProperitesBufferInfo.offset = 0;
//			MeshProperitesBufferInfo.range = VK_WHOLE_SIZE;
//			VertexPropertiesBuffer.emplace_back(MeshProperitesBufferInfo);
//		}
//	}
//	return VertexPropertiesBuffer;
//}

// std::vector<VkDescriptorBufferInfo>  MemoryPoolManager::GetIndexPropertiesBuffer()
//{
//	std::vector<VkDescriptorBufferInfo>	IndexPropertiesBuffer;
//	if (GameObjectList.size() == 0)
//	{
//		VkDescriptorBufferInfo nullBuffer;
//		nullBuffer.buffer = VK_NULL_HANDLE;
//		nullBuffer.offset = 0;
//		nullBuffer.range = VK_WHOLE_SIZE;
//		IndexPropertiesBuffer.emplace_back(nullBuffer);
//	}
//	else
//	{
//		for (auto& gameObject : GameObjectList)
//		{
//			VkDescriptorBufferInfo MeshProperitesBufferInfo = {};
//			MeshProperitesBufferInfo.buffer = gameObject->GetIndexPropertiesBuffer().buffer;
//			MeshProperitesBufferInfo.offset = 0;
//			MeshProperitesBufferInfo.range = VK_WHOLE_SIZE;
//			IndexPropertiesBuffer.emplace_back(MeshProperitesBufferInfo);
//		}
//	}
//	return IndexPropertiesBuffer;
//}

std::vector<VkDescriptorBufferInfo> MemoryManager::GetGameObjectPropertiesBuffer()
{
	std::vector<VkDescriptorBufferInfo>	MeshPropertiesBuffer;
	if (GameObjectList.size() == 0)
	{
		VkDescriptorBufferInfo nullBuffer;
		nullBuffer.buffer = VK_NULL_HANDLE;
		nullBuffer.offset = 0;
		nullBuffer.range = VK_WHOLE_SIZE;
		MeshPropertiesBuffer.emplace_back(nullBuffer);
	}
	else
	{
		for (auto& mesh : RenderMesh2DComponentList)
		{
			auto asdf = mesh->GetMeshPropertiesBuffer()->CheckBufferContents();
			VkDescriptorBufferInfo MeshProperitesBufferInfo = {};
			MeshProperitesBufferInfo.buffer = mesh->GetMeshPropertiesBuffer()->Buffer;
			MeshProperitesBufferInfo.offset = 0;
			MeshProperitesBufferInfo.range = mesh->GetMeshPropertiesBuffer()->GetBufferSize();
			MeshPropertiesBuffer.emplace_back(MeshProperitesBufferInfo);
		}
	}

	return MeshPropertiesBuffer;
}

void MemoryManager::Destroy()
{
	GameObjectMemoryPool.Destroy();
	RenderMesh2DComponentMemoryPool.Destroy();

	GameObjectList.clear();
	RenderMesh2DComponentList.clear();
}

//std::vector<VkDescriptorImageInfo> MemoryManager::GetTexturePropertiesBuffer()
//{
//	std::vector<VkDescriptorImageInfo>	TexturePropertiesBuffer = List<VkDescriptorImageInfo>();
//	if (TextureList.size() == 0)
//	{
//		VkSamplerCreateInfo NullSamplerInfo = {};
//		NullSamplerInfo.sType = VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO;
//		NullSamplerInfo.magFilter = VK_FILTER_NEAREST;
//		NullSamplerInfo.minFilter = VK_FILTER_NEAREST;
//		NullSamplerInfo.addressModeU = VK_SAMPLER_ADDRESS_MODE_REPEAT;
//		NullSamplerInfo.addressModeV = VK_SAMPLER_ADDRESS_MODE_REPEAT;
//		NullSamplerInfo.addressModeW = VK_SAMPLER_ADDRESS_MODE_REPEAT;
//		NullSamplerInfo.anisotropyEnable = VK_TRUE;
//		NullSamplerInfo.maxAnisotropy = 16.0f;
//		NullSamplerInfo.borderColor = VK_BORDER_COLOR_INT_OPAQUE_BLACK;
//		NullSamplerInfo.unnormalizedCoordinates = VK_FALSE;
//		NullSamplerInfo.compareEnable = VK_FALSE;
//		NullSamplerInfo.compareOp = VK_COMPARE_OP_ALWAYS;
//		NullSamplerInfo.mipmapMode = VK_SAMPLER_MIPMAP_MODE_LINEAR;
//		NullSamplerInfo.minLod = 0;
//		NullSamplerInfo.maxLod = 0;
//		NullSamplerInfo.mipLodBias = 0;
//
//		VkSampler nullSampler = VK_NULL_HANDLE;
//		if (vkCreateSampler(cRenderer.Device, &NullSamplerInfo, nullptr, &nullSampler))
//		{
//			throw std::runtime_error("Failed to create Sampler.");
//		}
//
//		VkDescriptorImageInfo nullBuffer;
//		nullBuffer.imageLayout = VK_IMAGE_LAYOUT_UNDEFINED;
//		nullBuffer.imageView = VK_NULL_HANDLE;
//		nullBuffer.sampler = nullSampler;
//		TexturePropertiesBuffer.emplace_back(nullBuffer);
//	}
//	else
//	{
//		for (auto& texture : TextureList)
//		{
//			VkDescriptorImageInfo textureDescriptor;
//			textureDescriptor.imageLayout = VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL;
//			textureDescriptor.imageView = texture->View;
//			textureDescriptor.sampler = texture->Sampler;
//			TexturePropertiesBuffer.emplace_back(textureDescriptor);
//		}
//	}
//
//	return TexturePropertiesBuffer;
//}

// std::vector<VkDescriptorBufferInfo>  MemoryPoolManager::GetGameObjectTransformBuffer()
//{
//	std::vector<VkDescriptorBufferInfo>	TransformPropertiesBuffer;
//	if (GameObjectList.size() == 0)
//	{
//		VkDescriptorBufferInfo nullBuffer;
//		nullBuffer.buffer = VK_NULL_HANDLE;
//		nullBuffer.offset = 0;
//		nullBuffer.range = VK_WHOLE_SIZE;
//		TransformPropertiesBuffer.emplace_back(nullBuffer);
//	}
//	else
//	{
//		for (auto& gameObject : GameObjectList)
//		{
//			for (int x = 0; x < gameObject->GetGameObjectTransformMatrixBuffer().size(); x++)
//			{
//				VkDescriptorBufferInfo TransformBufferInfo = {};
//				TransformBufferInfo.buffer = gameObject->GetGameObjectTransformMatrixBuffer()[x].buffer;
//				TransformBufferInfo.offset = 0;
//				TransformBufferInfo.range = VK_WHOLE_SIZE;
//				TransformPropertiesBuffer.emplace_back(TransformBufferInfo);
//			}
//		}
//	}
//
//	return TransformPropertiesBuffer;
//}