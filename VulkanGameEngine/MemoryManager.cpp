#include "MemoryManager.h"
#include <CBuffer.h>

Coral::HostInstance MemoryManager::hostInstance;
SharedPtr<Coral::ManagedAssembly> MemoryManager::ECSassembly = nullptr;

Vector<JsonRenderPass> MemoryManager::JsonRenderPassList;
Vector<JsonPipeline> MemoryManager::JsonPipelineList;
Vector<SharedPtr<JsonRenderPass>> MemoryManager::SharedJsonRenderPassPtrList;
Vector<SharedPtr<JsonPipeline>> MemoryManager::SharedJsonPipelinePtrList;

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
}

//SharedPtr<GameObject> MemoryManager::AllocateNewGameObject()
//{
//	return GameObjectList.emplace_back(GameObjectMemoryPool.AllocateMemoryLocation());
//}
//
//SharedPtr<Mesh2D> MemoryManager::AllocateMesh2D()
//{
//	SharedPtr<Mesh2D> mesh2D = Mesh2DList.emplace_back(Mesh2DMemoryPool.AllocateMemoryLocation());
//	MeshList.emplace_back(mesh2D);
//	return mesh2D;
//}
//
//SharedPtr<Texture> MemoryManager::AllocateNewTexture()
//{
//	SharedPtr<Texture> texture = TextureList.emplace_back(TextureMemoryPool.AllocateMemoryLocation());
//	UpdateBufferIndex();
//	return texture;
//}
//
//SharedPtr<SpriteComponent> MemoryManager::AllocateSpriteComponent()
//{
//	return SpriteComponentList.emplace_back(SpriteComponentMemoryPool.AllocateMemoryLocation());
//}
//
//SharedPtr<JsonRenderPass> MemoryManager::AllocateJsonRenderPass()
//{
//	return JsonRenderPassList.emplace_back(JsonRenderPassMemoryPool.AllocateMemoryLocation());
//}
//
//SharedPtr<JsonPipeline> MemoryManager::AllocateJsonPipeline()
//{
//	return JsonPipelineList.emplace_back(JsonPipelineMemoryPool.AllocateMemoryLocation());
//}
//
//SharedPtr<SpriteBatchLayer> MemoryManager::AllocateSpriteBatchLayer()
//{
//	SharedPtr<SpriteBatchLayer> spriteLayer = SpriteBatchLayerList.emplace_back(SpriteBatchLayerMemeryPool.AllocateMemoryLocation());
//	MeshList.emplace_back(spriteLayer->GetSpriteLayerMesh());
//	return spriteLayer;
//}
//
//SharedPtr<Material> MemoryManager::AllocateMaterial()
//{
//	SharedPtr<Material> material = MaterialList.emplace_back(MaterialMemoryPool.AllocateMemoryLocation());
//	UpdateBufferIndex();
//	return material;
//}
//
//void MemoryManager::ViewMemoryMap()
//{
//	GameObjectMemoryPool.ViewMemoryMap();
//	TextureMemoryPool.ViewMemoryMap();
//	//SpriteComponentMemoryPool.ViewMemoryMap();
//	JsonRenderPassMemoryPool.ViewMemoryMap();
//	JsonPipelineMemoryPool.ViewMemoryMap();
//	SpriteBatchLayerMemeryPool.ViewMemoryMap();
//	MaterialMemoryPool.ViewMemoryMap();
//}

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

//void MemoryManager::Update(float deltaTime)
//{
//	VkCommandBuffer commandBuffer = renderer.BeginSingleTimeCommands();
//	for (auto drawLayer : MeshList)
//	{
//		drawLayer.BufferUpdate(commandBuffer, deltaTime);
//	}
//	renderer.EndSingleTimeCommands(commandBuffer);
//}
//
//void MemoryManager::UpdateBufferIndex()
//{
//	for (int x = 0; x < TextureList.size(); x++)
//	{
//		TextureList[x].UpdateTextureBufferIndex(x);
//	}
//	for (int x = 0; x < MaterialList.size(); x++)
//	{
//		MaterialList[x].UpdateMaterialBufferIndex(x);
//	}
//}

void MemoryManager::Destroy()
{
	//GameObjectList.clear();
	//RenderMesh2DComponentList.clear();
	//TextureList.clear();
	//MaterialList.clear();
	//JsonRenderPassList.clear();
	//JsonPipelineList.clear();
	//MeshList.clear();
	//Mesh2DList.clear();
	//SpriteBatchLayerList.clear();

	//GameObjectMemoryPool.Destroy();
	//SpriteComponentMemoryPool.Destroy();
	//SpriteBatchLayerMemeryPool.Destroy();
	//MaterialMemoryPool.Destroy();
	//TextureMemoryPool.Destroy();
	//JsonPipelineMemoryPool.Destroy();
	//JsonRenderPassMemoryPool.Destroy();

	hostInstance.Shutdown();
}

//const Vector<VkDescriptorBufferInfo> MemoryManager::GetMeshPropertiesBuffer()
//{
//	Vector<VkDescriptorBufferInfo> meshPropertiesBuffer;
//	if (MeshList.size() == 0)
//	{
//		VkDescriptorBufferInfo nullBuffer;
//		nullBuffer.buffer = VK_NULL_HANDLE;
//		nullBuffer.offset = 0;
//		nullBuffer.range = VK_WHOLE_SIZE;
//		meshPropertiesBuffer.emplace_back(nullBuffer);
//	}
//	else
//	{
//		for (auto& mesh : MeshList)
//		{
//			mesh.GetMeshPropertiesBuffer(meshPropertiesBuffer);
//		}
//	}
//
//	return meshPropertiesBuffer;
//}

//SharedPtr<GameObject> MemoryManager::AddGameObject(GameObject gameObject)
//{
//	GameObjectList.emplace_back(gameObject);
//	SharedGameObjectPtrList.emplace_back(std::make_shared<GameObject>(GameObject(gameObject)));
//	return SharedGameObjectPtrList.back();
//}
//
// SharedPtr<GameObjectComponent> MemoryManager::AddGameObjectComponent(SharedPtr<GameObjectComponent> gameObjectComponent)
// {
//	 return GameObjectComponentList.emplace_back(gameObjectComponent);
// }

 //SharedPtr<Texture> MemoryManager::AddTexture(Texture texture)
 //{
	// TextureList.emplace_back(texture);
	// SharedTexturePtrList.emplace_back(std::make_shared<Texture>(Texture(texture)));
	// return SharedTexturePtrList.back();
 //}

 //SharedPtr<Material> MemoryManager::AddMaterial(Material material)
 //{
	// MaterialList.emplace_back(material);
	// SharedMaterialPtrList.emplace_back(std::make_shared<Material>(Material(material)));
	// return SharedMaterialPtrList.back();
 //}

 SharedPtr<JsonRenderPass> MemoryManager::AddJsonRenderPass(JsonRenderPass jsonRenderPass)
 {
	 JsonRenderPassList.emplace_back(jsonRenderPass);
	 SharedJsonRenderPassPtrList.emplace_back(std::make_shared<JsonRenderPass>(JsonRenderPass(jsonRenderPass)));
	 return SharedJsonRenderPassPtrList.back();
 }

 SharedPtr<JsonPipeline> MemoryManager::AddJsonPipeline(JsonPipeline jsonPipeline)
 {
	 JsonPipelineList.emplace_back(jsonPipeline);
	 SharedJsonPipelinePtrList.emplace_back(std::make_shared<JsonPipeline>(JsonPipeline(jsonPipeline)));
	 return SharedJsonPipelinePtrList.back();
 }

 //SharedPtr<Mesh> MemoryManager::AddMesh(Mesh mesh)
 //{
	// MeshList.emplace_back(mesh);
	// SharedMeshPtrList.emplace_back(std::make_shared<Mesh>(Mesh(mesh)));
	// return SharedMeshPtrList.back();
 //}

 //SharedPtr<SpriteBatchLayer> MemoryManager::AddSpriteBatchLayer(SpriteBatchLayer spriteBatchLayer)
 //{
	// SpriteBatchLayerList.emplace_back(spriteBatchLayer);
	// SharedSpriteBatchLayerPtrList.emplace_back(std::make_shared<SpriteBatchLayer>(SpriteBatchLayer(spriteBatchLayer)));
	// return SharedSpriteBatchLayerPtrList.back();
 //}

 //SharedPtr<Sprite> MemoryManager::AddSprite(Sprite sprite)
 //{
	// SpriteList.emplace_back(sprite);
	// SharedSpritePtrList.emplace_back(std::make_shared<Sprite>(Sprite(sprite)));
	// return SharedSpritePtrList.back();
 //}

//const Vector<VkDescriptorImageInfo> MemoryManager::GetTexturePropertiesBuffer()
//{
//	Vector<VkDescriptorImageInfo>	texturePropertiesBuffer;
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
//		texturePropertiesBuffer.emplace_back(nullBuffer);
//	}
//	else
//	{
//		for (auto& texture : TextureList)
//		{
//			texture.GetTexturePropertiesBuffer(texturePropertiesBuffer);
//		}
//	}
//
//	return texturePropertiesBuffer;
//}
//
//const Vector<VkDescriptorBufferInfo> MemoryManager::GetMaterialPropertiesBuffer()
//{
//	std::vector<VkDescriptorBufferInfo>	materialPropertiesBuffer;
//		for (auto& material : MaterialList)
//		{
//			material.GetMaterialPropertiesBuffer(materialPropertiesBuffer);
//		}
//	return materialPropertiesBuffer;
//}


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