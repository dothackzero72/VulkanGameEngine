#include "MemoryPoolManager.h"

List<std::shared_ptr<GameObject>> MemoryPoolManager::GameObjectList;
List<std::shared_ptr<RenderMesh2DComponent>> MemoryPoolManager::RenderMesh2DComponentList;
MemoryPool<GameObject> MemoryPoolManager::GameObjectMemoryPool;
MemoryPool<RenderMesh2DComponent> MemoryPoolManager::RenderMesh2DComponentMemoryPool;

void MemoryPoolManager::SetUpMemoryPoolManager(uint32_t EstObjectCount)
{
	GameObjectMemoryPool.CreateMemoryPool(EstObjectCount);
	RenderMesh2DComponentMemoryPool.CreateMemoryPool(EstObjectCount);
}

std::shared_ptr<GameObject> MemoryPoolManager::AllocateNewGameObject()
{
	GameObjectList.emplace_back(GameObjectMemoryPool.AllocateMemoryLocation());
	return GameObjectList.back();
}

std::shared_ptr<RenderMesh2DComponent> MemoryPoolManager::AllocateRenderMesh2DComponent()
{
	RenderMesh2DComponentList.emplace_back(RenderMesh2DComponentMemoryPool.AllocateMemoryLocation());
	return RenderMesh2DComponentList.back();
}

void MemoryPoolManager::ViewMemoryPool()
{
	const auto gameObjectMemoryPool = GameObjectMemoryPool.ViewMemoryPool();
	const auto renderMesh2DComponentMemoryPool = RenderMesh2DComponentMemoryPool.ViewMemoryPool();
	

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

std::vector<VkDescriptorBufferInfo> MemoryPoolManager::GetGameObjectPropertiesBuffer()
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

void MemoryPoolManager::Destroy()
{
	GameObjectMemoryPool.Destroy();
	RenderMesh2DComponentMemoryPool.Destroy();

	GameObjectList.clear();
	RenderMesh2DComponentList.clear();
}

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