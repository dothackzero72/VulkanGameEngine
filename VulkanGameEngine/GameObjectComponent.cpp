#include "GameObjectComponent.h"
#include "MemoryManager.h"

GameObjectComponent::GameObjectComponent()
{

}

GameObjectComponent::GameObjectComponent(std::shared_ptr<GameObject> parentGameObjectPtr, ComponentTypeEnum componentType)
{
    Name = "component";
    ComponentType = componentType;
    ParentGameObjectPtr = parentGameObjectPtr;
}

GameObjectComponent::GameObjectComponent(std::shared_ptr<GameObject> parentGameObjectPtr, String name, ComponentTypeEnum componentType)
{
    Name = name;
    ComponentType = componentType;
    ParentGameObjectPtr = parentGameObjectPtr;
}

GameObjectComponent::GameObjectComponent(std::shared_ptr<GameObject> parentGameObjectPtr, String name, String componentName, ComponentTypeEnum componentType)
{
    Name = name;
    ComponentType = componentType;
    hModuleRef = MemoryManager::GetEntityComponentSytemModule();
    ParentGameObjectPtr = parentGameObjectPtr;

    StartUp(parentGameObjectPtr, componentName);
}

GameObjectComponent::~GameObjectComponent()
{
}

void GameObjectComponent::StartUp(std::shared_ptr<GameObject> gameObjectPtr, String& componentName)
{
    ParentGameObjectPtr = gameObjectPtr;

    CreateComponentPtr = (DLL_CreateComponent)GetProcAddress(hModuleRef, ("DLL_Create" + componentName).c_str());
    if (!CreateComponentPtr)
    {
        throw std::runtime_error("Could not find DLL_CreateTestScriptComponent function.");
    }

    DLLUpdatePtr = (DLL_ComponentUpdate)GetProcAddress(hModuleRef, ("DLL_" + componentName + "_Update").c_str());
    if (!DLLUpdatePtr)
    {
        throw std::runtime_error("Could not find DLL_TestScriptComponent_Update function.");
    }

    DLLBufferUpdatePtr = (DLL_ComponentBufferUpdate)GetProcAddress(hModuleRef, ("DLL_" + componentName + "_BufferUpdate").c_str());
    if (!DLLBufferUpdatePtr)
    {
        throw std::runtime_error("Could not find DLL_TestScriptComponent_BufferUpdate function.");
    }

   /* DLLDestroyPtr = (DLL_ComponentDestroy)GetProcAddress(hModuleRef, ("DLL_" + componentName + "_Destroy").c_str());
    if (!DLLDestroyPtr)
    {
        throw std::runtime_error("Could not find DLL_TestScriptComponent_Destroy function.");
    }*/

    DLLGetMemorySizePtr = (DLL_ComponentGetMemorySize)GetProcAddress(hModuleRef, ("DLL_" + componentName + "_GetMemorySize").c_str());
    if (!DLLGetMemorySizePtr)
    {
        throw std::runtime_error("Could not find DLL_TestScriptComponent_GetMemorySize function.");
    }

    ComponentPtr = CreateComponentPtr(this);
    if (!ComponentPtr)
    {
        throw std::runtime_error("Could not create the component.");
    }
}

void GameObjectComponent::Input()
{
    //if (vulkanWindow->keyboard.KeyPressed[KeyCode::KEY_W] == KS_PRESSED)
    //{
    //    const auto conponent = gameObjectList.front()->GetComponentByComponentType(kRenderMesh2DComponent);
    //    const auto meshRenderer = dynamic_cast<RenderMesh2DComponent*>(conponent.get());
    //    meshRenderer->GetMesh2D()->MeshPosition.x += 1.0f * deltaTime;

    //    //orthographicCamera->Position.y -= 1.0f * deltaTime;
    //}
    //if (vulkanWindow->keyboard.KeyPressed[KeyCode::KEY_A] == KS_PRESSED)
    //{
    //    const auto conponent = gameObjectList.front()->GetComponentByComponentType(kRenderMesh2DComponent);
    //    const auto meshRenderer = dynamic_cast<RenderMesh2DComponent*>(conponent.get());
    //    meshRenderer->GetMesh2D()->MeshPosition.x -= 1.0f * deltaTime;

    //    //orthographicCamera->Position.x += 1.0f * deltaTime;
    //}
    //if (vulkanWindow->keyboard.KeyPressed[KeyCode::KEY_S] == KS_PRESSED)
    //{
    //    const auto conponent = gameObjectList.front()->GetComponentByComponentType(kRenderMesh2DComponent);
    //    const auto meshRenderer = dynamic_cast<RenderMesh2DComponent*>(conponent.get());
    //    meshRenderer->GetMesh2D()->MeshPosition.y -= 1.0f * deltaTime;

    //    //orthographicCamera->Position.y += 1.0f * deltaTime;
    //}
    //if (vulkanWindow->keyboard.KeyPressed[KeyCode::KEY_D] == KS_PRESSED)
    //{
    //    const auto conponent = gameObjectList.front()->GetComponentByComponentType(kRenderMesh2DComponent);
    //    const auto meshRenderer = dynamic_cast<RenderMesh2DComponent*>(conponent.get());
    //    meshRenderer->GetMesh2D()->MeshPosition.y += 1.0f * deltaTime;

    //    //orthographicCamera->Position.x -= 1.0f * deltaTime;
    //}
    //if (vulkanWindow->keyboard.KeyPressed[KeyCode::KEY_D] == KS_PRESSED)
    //{
    //    orthographicCamera->Zoom -= 1.0f * deltaTime;
    //}
    //if (vulkanWindow->keyboard.KeyPressed[KeyCode::KEY_D] == KS_PRESSED)
    //{
    //    orthographicCamera->Zoom += 1.0f * deltaTime;
    //}
}

void GameObjectComponent::Update(float deltaTime)
{
    DLLUpdatePtr(ComponentPtr, deltaTime);
}

void GameObjectComponent::BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime)
{
    DLLBufferUpdatePtr(ComponentPtr, commandBuffer, deltaTime);
}

void GameObjectComponent::Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties)
{

}

void GameObjectComponent::Destroy()
{
    if (ComponentPtr) 
    {
        DLLDestroyPtr(ComponentPtr);
        ComponentPtr = nullptr;
    }
}

std::shared_ptr<GameObjectComponent> GameObjectComponent::Clone() const
{
    return std::shared_ptr<GameObjectComponent>();
}

size_t GameObjectComponent::GetMemorySize() const
{
    return size_t();
}
