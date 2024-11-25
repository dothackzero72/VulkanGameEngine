//#pragma once
//#include <memory>
//
//using namespace System;
//using namespace VulkanGameEngineGameObjectScripts;
//
//enum ComponentTypeEnum
//{
//	kUndefined,
//	kRenderMesh2DComponent
//};
//
//ref class GameObjectComponent
//{
//private:
//protected:
//	GameObjectComponent(ComponentTypeEnum componentType)
//	{
//		Name = new String("unnamed");
//		ComponentType = componentType;
//	}
//
//	GameObjectComponent(String name, ComponentTypeEnum componentType)
//	{
//		Name = name;
//		ComponentType = componentType;
//	}
//
//public:
//	String Name;
//	size_t MemorySize = 0;
//	ComponentTypeEnum ComponentType;
//
//	GameObjectComponent()
//	{
//		Name = "Unnamed";
//	}
//
//	virtual ~GameObjectComponent()
//	{
//
//	}
//
//
//	virtual void Update(float deltaTime) = 0;
//	virtual void Update(VkCommandBuffer& commandBuffer, float deltaTime) = 0;
//	virtual void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties) = 0;
//	virtual void Destroy() = 0;
//	virtual std::shared_ptr<GameObjectComponent> Clone() const = 0;
//	virtual size_t GetMemorySize() const = 0;
//};
