#include "MemoryManager.h"
#include <CBuffer.h>

Coral::HostInstance MemoryManager::hostInstance;
SharedPtr<Coral::ManagedAssembly> MemoryManager::ECSassembly = nullptr;
List<SharedPtr<GameObject>> MemoryManager::GameObjectList;
List<SharedPtr<SpriteComponent>> MemoryManager::SpriteComponentList;
List<SharedPtr<Texture>> MemoryManager::TextureList;
List<SharedPtr<Material>> MemoryManager::MaterialList;
List<SharedPtr<JsonRenderPass>> MemoryManager::JsonRenderPassList;
List<SharedPtr<JsonPipeline>> MemoryManager::JsonPipelineList;
List<SharedPtr<Mesh>> MemoryManager::MeshList;
List<SharedPtr<Mesh2D>> MemoryManager::Mesh2DList;
List<SharedPtr<SpriteBatchLayer>> MemoryManager::SpriteBatchLayerList;

MemoryPool<GameObject> MemoryManager::GameObjectMemoryPool;
MemoryPool<SpriteComponent> MemoryManager::SpriteComponentMemoryPool;
MemoryPool<Texture> MemoryManager::TextureMemoryPool;
MemoryPool<Material> MemoryManager::MaterialMemoryPool;
MemoryPool<Mesh2D> MemoryManager::Mesh2DMemoryPool;
MemoryPool<SpriteBatchLayer> MemoryManager::SpriteBatchLayerMemeryPool;
MemoryPool<JsonRenderPass> MemoryManager::JsonRenderPassMemoryPool;
MemoryPool<JsonPipeline> MemoryManager::JsonPipelineMemoryPool;

void ExceptionCallback(std::string_view InMessage)
{
	std::cout << "Unhandled native exception: " << InMessage << std::endl;
}

void MemoryManager::SetUpMemoryManager(uint32 EstObjectCount)
{
	auto exeDir = std::filesystem::path(L"C:/Users/dotha/Documents/GitHub/VulkanGameEngine/x64/Debug");
	String coralDir = exeDir.string();
	Coral::HostSettings settings =
	{
		.CoralDirectory = coralDir,
		.ExceptionCallback = ExceptionCallback
	};
	hostInstance.Initialize(settings);

	auto loadContext = hostInstance.CreateAssemblyLoadContext("ExampleContext");

	std::string assemblyPath = "C:/Users/dotha/Documents/GitHub/VulkanGameEngine/VulkanGameEngineGameObjectScripts/bin/Debug/VulkanGameEngineGameObjectScripts.dll";
	Coral::ManagedAssembly* assembly = &loadContext.LoadAssembly(assemblyPath);
	ECSassembly = SharedPtr<Coral::ManagedAssembly>(assembly);

	

	GameObjectMemoryPool.CreateMemoryPool(EstObjectCount);
	//RenderMesh2DComponentMemoryPool.CreateMemoryPool(EstObjectCount);
	TextureMemoryPool.CreateMemoryPool(EstObjectCount);
	MaterialMemoryPool.CreateMemoryPool(EstObjectCount);
	JsonRenderPassMemoryPool.CreateMemoryPool(EstObjectCount);
	JsonPipelineMemoryPool.CreateMemoryPool(EstObjectCount);
	SpriteBatchLayerMemeryPool.CreateMemoryPool(EstObjectCount);
	Mesh2DMemoryPool.CreateMemoryPool(EstObjectCount);
}

SharedPtr<GameObject> MemoryManager::AllocateNewGameObject()
{
	return GameObjectList.emplace_back(GameObjectMemoryPool.AllocateMemoryLocation());
}

SharedPtr<Mesh2D> MemoryManager::AllocateMesh2D()
{
	SharedPtr<Mesh2D> mesh2D = Mesh2DList.emplace_back(Mesh2DMemoryPool.AllocateMemoryLocation());
	MeshList.emplace_back(mesh2D);
	return mesh2D;
}

SharedPtr<Texture> MemoryManager::AllocateNewTexture()
{
	SharedPtr<Texture> texture = TextureList.emplace_back(TextureMemoryPool.AllocateMemoryLocation());
	UpdateBufferIndex();
	return texture;
}

//SharedPtr<RenderMesh2DComponent> MemoryManager::AllocateRenderMesh2DComponent()
//{
//	return RenderMesh2DComponentList.emplace_back(RenderMesh2DComponentMemoryPool.AllocateMemoryLocation());
//}

SharedPtr<JsonRenderPass> MemoryManager::AllocateJsonRenderPass()
{
	return JsonRenderPassList.emplace_back(JsonRenderPassMemoryPool.AllocateMemoryLocation());
}

SharedPtr<JsonPipeline> MemoryManager::AllocateJsonPipeline()
{
	return JsonPipelineList.emplace_back(JsonPipelineMemoryPool.AllocateMemoryLocation());
}

SharedPtr<SpriteBatchLayer> MemoryManager::AllocateSpriteBatchLayer()
{
	SharedPtr<SpriteBatchLayer> spriteLayer = SpriteBatchLayerList.emplace_back(SpriteBatchLayerMemeryPool.AllocateMemoryLocation());
	MeshList.emplace_back(spriteLayer->GetSpriteLayerMesh());
	return spriteLayer;
}

SharedPtr<Material> MemoryManager::AllocateMaterial()
{
	SharedPtr<Material> material = MaterialList.emplace_back(MaterialMemoryPool.AllocateMemoryLocation());
	UpdateBufferIndex();
	return material;
}

void MemoryManager::ViewMemoryMap()
{
	GameObjectMemoryPool.ViewMemoryMap();
	TextureMemoryPool.ViewMemoryMap();
	//RenderMesh2DComponentMemoryPool.ViewMemoryMap();
	JsonRenderPassMemoryPool.ViewMemoryMap();
	JsonPipelineMemoryPool.ViewMemoryMap();
	SpriteBatchLayerMemeryPool.ViewMemoryMap();
	MaterialMemoryPool.ViewMemoryMap();
}

// std::vector<VkDescriptorBufferInfo>  MemoryManager::GetVertexPropertiesBuffer()
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

// std::vector<VkDescriptorBufferInfo>  MemoryManager::GetIndexPropertiesBuffer()
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

void MemoryManager::Update(float deltaTime)
{
	VkCommandBuffer commandBuffer = renderer.BeginSingleTimeCommands();
	for (auto drawLayer : Mesh2DList)
	{
		drawLayer->BufferUpdate(commandBuffer, deltaTime);
	}
	renderer.EndSingleTimeCommands(commandBuffer);
}

void MemoryManager::UpdateBufferIndex()
{
	for (int x = 0; x < TextureList.size(); x++)
	{
		TextureList[x]->UpdateTextureBufferIndex(x);
	}
	for (int x = 0; x < MaterialList.size(); x++)
	{
		MaterialList[x]->UpdateMaterialBufferIndex(x);
	}
}

void MemoryManager::Destroy()
{
	//GameObjectList.clear();
	////RenderMesh2DComponentList.clear();
	//TextureList.clear();
	//MaterialList.clear();
	//JsonRenderPassList.clear();
	//JsonPipelineList.clear();
	//MeshList.clear();
	//Mesh2DList.clear();
	//SpriteBatchLayerList.clear();

	GameObjectMemoryPool.Destroy();
	//RenderMesh2DComponentMemoryPool.Destroy();
	SpriteBatchLayerMemeryPool.Destroy();
	MaterialMemoryPool.Destroy();
	TextureMemoryPool.Destroy();
	JsonPipelineMemoryPool.Destroy();
	JsonRenderPassMemoryPool.Destroy();

	hostInstance.Shutdown();
}

const List<VkDescriptorBufferInfo> MemoryManager::GetMeshPropertiesBuffer()
{
	List<VkDescriptorBufferInfo> meshPropertiesBuffer;
	if (MeshList.size() == 0)
	{
		VkDescriptorBufferInfo nullBuffer;
		nullBuffer.buffer = VK_NULL_HANDLE;
		nullBuffer.offset = 0;
		nullBuffer.range = VK_WHOLE_SIZE;
		meshPropertiesBuffer.emplace_back(nullBuffer);
	}
	else
	{
		for (auto& mesh : MeshList)
		{
			mesh->GetMeshPropertiesBuffer(meshPropertiesBuffer);
		}
	}

	return meshPropertiesBuffer;
}

const List<VkDescriptorImageInfo> MemoryManager::GetTexturePropertiesBuffer()
{
	List<VkDescriptorImageInfo>	texturePropertiesBuffer;
	if (TextureList.size() == 0)
	{
		VkSamplerCreateInfo NullSamplerInfo = {};
		NullSamplerInfo.sType = VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO;
		NullSamplerInfo.magFilter = VK_FILTER_NEAREST;
		NullSamplerInfo.minFilter = VK_FILTER_NEAREST;
		NullSamplerInfo.addressModeU = VK_SAMPLER_ADDRESS_MODE_REPEAT;
		NullSamplerInfo.addressModeV = VK_SAMPLER_ADDRESS_MODE_REPEAT;
		NullSamplerInfo.addressModeW = VK_SAMPLER_ADDRESS_MODE_REPEAT;
		NullSamplerInfo.anisotropyEnable = VK_TRUE;
		NullSamplerInfo.maxAnisotropy = 16.0f;
		NullSamplerInfo.borderColor = VK_BORDER_COLOR_INT_OPAQUE_BLACK;
		NullSamplerInfo.unnormalizedCoordinates = VK_FALSE;
		NullSamplerInfo.compareEnable = VK_FALSE;
		NullSamplerInfo.compareOp = VK_COMPARE_OP_ALWAYS;
		NullSamplerInfo.mipmapMode = VK_SAMPLER_MIPMAP_MODE_LINEAR;
		NullSamplerInfo.minLod = 0;
		NullSamplerInfo.maxLod = 0;
		NullSamplerInfo.mipLodBias = 0;

		VkSampler nullSampler = VK_NULL_HANDLE;
		if (vkCreateSampler(cRenderer.Device, &NullSamplerInfo, nullptr, &nullSampler))
		{
			throw std::runtime_error("Failed to create Sampler.");
		}

		VkDescriptorImageInfo nullBuffer;
		nullBuffer.imageLayout = VK_IMAGE_LAYOUT_UNDEFINED;
		nullBuffer.imageView = VK_NULL_HANDLE;
		nullBuffer.sampler = nullSampler;
		texturePropertiesBuffer.emplace_back(nullBuffer);
	}
	else
	{
		for (auto& texture : TextureList)
		{
			texture->GetTexturePropertiesBuffer(texturePropertiesBuffer);
		}
	}

	return texturePropertiesBuffer;
}

const List<VkDescriptorBufferInfo> MemoryManager::GetMaterialPropertiesBuffer()
{
	std::vector<VkDescriptorBufferInfo>	materialPropertiesBuffer;
		for (auto& material : MaterialList)
		{
			material->GetMaterialPropertiesBuffer(materialPropertiesBuffer);
		}
	return materialPropertiesBuffer;
}


// std::vector<VkDescriptorBufferInfo>  MemoryManager::GetGameObjectTransformBuffer()
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