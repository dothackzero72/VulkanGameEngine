#include <windows.h>
#include <iostream>
#include <vector>
#include <string>
#include "vulkan/vulkan_core.h"
#include "SceneDataBuffer.h"
#include "GameObjectComponent.h"
#include "Typedef.h"

#define GET_CLASS_NAME(classname) std::wstring(L#classname)

extern "C" 
{
    typedef void* (*DLL_CreateComponent)(void* wrapperHandle);
    typedef void (*DLL_ComponentUpdate)(void* wrapperHandle, long startTime);
    typedef void (*DLL_ComponentBufferUpdate)(void* wrapperHandle, VkCommandBuffer commandBuffer, long startTime);
    typedef void (*DLL_ComponentDraw)(void* wrapperHandle, VkCommandBuffer commandBuffer, VkPipeline pipeline, VkPipelineLayout pipelineLayout, VkDescriptorSet descriptorSet, SceneDataBuffer sceneProperties);
    typedef void (*DLL_ComponentDestroy)(void* wrapperHandle);
    typedef int (*DLL_ComponentGetMemorySize)(void* wrapperHandle);
}

class GameObjectComponent2
{
    private:
        HMODULE hModule = nullptr;
        DLL_CreateComponent CreateComponentPtr = nullptr;
        DLL_ComponentUpdate DLLUpdatePtr = nullptr;
        DLL_ComponentBufferUpdate DLLBufferUpdatePtr = nullptr;
        DLL_ComponentDraw DLLDrawPtr = nullptr;
        DLL_ComponentDestroy DLLDestroyPtr = nullptr;

    protected:
        void* componentPtr = nullptr;
        void* wrapperHandle = nullptr;
        DLL_ComponentGetMemorySize DLLGetMemorySizePtr = nullptr;

    public:
        String Name = "Component";
        size_t MemorySize = 0;
        ComponentTypeEnum ComponentType = ComponentTypeEnum::kUndefined;

        GameObjectComponent2(const std::wstring& dllPath, String componentName)
        {
            hModule = LoadLibrary(dllPath.c_str());
            if (!hModule)
            {
                throw std::runtime_error("Could not load the DLL. Error code: " + std::to_string(GetLastError()));
            }

            CreateComponentPtr = (DLL_CreateComponent)GetProcAddress(hModule, ("DLL_Create" + componentName).c_str());
            if (!CreateComponentPtr)
            {
                throw std::runtime_error("Could not find DLL_CreateTestScriptComponent function.");
            }

            DLLUpdatePtr = (DLL_ComponentUpdate)GetProcAddress(hModule, ("DLL_" + componentName + "_Update").c_str());
            if (!DLLUpdatePtr)
            {
                throw std::runtime_error("Could not find DLL_TestScriptComponent_Update function.");
            }

            DLLBufferUpdatePtr = (DLL_ComponentBufferUpdate)GetProcAddress(hModule, ("DLL_" + componentName + "_BufferUpdate").c_str());
            if (!DLLBufferUpdatePtr)
            {
                throw std::runtime_error("Could not find DLL_TestScriptComponent_BufferUpdate function.");
            }

            /*       DLLDrawPtr = (DLL_TestScriptComponent_Draw)GetProcAddress(hModule, ("DLL_" +  componentName + "_Draw").c_str());
                   if (!DLLDrawPtr)
                   {
                       throw std::runtime_error("Could not find DLL_TestScriptComponent_Draw function.");
                   }*/

            DLLDestroyPtr = (DLL_ComponentDestroy)GetProcAddress(hModule, ("DLL_" + componentName + "_Destroy").c_str());
            if (!DLLDestroyPtr)
            {
                throw std::runtime_error("Could not find DLL_TestScriptComponent_Destroy function.");
            }

            DLLGetMemorySizePtr = (DLL_ComponentGetMemorySize)GetProcAddress(hModule, ("DLL_" + componentName + "_GetMemorySize").c_str());
            if (!DLLGetMemorySizePtr)
            {
                throw std::runtime_error("Could not find DLL_TestScriptComponent_GetMemorySize function.");
            }

            wrapperHandle = CreateComponentPtr(componentPtr);
            if (!wrapperHandle)
            {
                throw std::runtime_error("Component creation failed.");
            }
        }

        ~GameObjectComponent2()
        {
        }

        virtual void Update(float deltaTime)
        {
            DLLUpdatePtr(componentPtr, deltaTime);
        }

        virtual void BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime)
        {
            DLLBufferUpdatePtr(componentPtr, commandBuffer, deltaTime);
        }

        virtual void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties)
        {

        }

        virtual void Destroy()
        {
            DLLDestroyPtr(componentPtr);
        }

        virtual size_t GetMemorySize() const = 0;
};


class TestScriptComponent : public GameObjectComponent2
{
private:

public:
    TestScriptComponent(const std::wstring& dllPath) : GameObjectComponent2(dllPath, "TestScriptComponent")
    {
    }

    ~TestScriptComponent()
    {
    }

    virtual void Update(float deltaTime) override
    {
        GameObjectComponent2::Update(deltaTime);
    }

    virtual void BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime) override
    {
        GameObjectComponent2::BufferUpdate(commandBuffer, deltaTime);
    }

    //void Draw(VkCommandBuffer commandBuffer, VkPipeline pipeline, VkPipelineLayout shaderPipelineLayout, VkDescriptorSet descriptorSet, SceneDataBuffer sceneProperties)
    //{
    //}

    virtual void Destroy() override
    {
        GameObjectComponent2::Destroy();
    }

    virtual size_t GetMemorySize() const override
    {
        return DLLGetMemorySizePtr(componentPtr);
    }
};

int main() 
{
    try 
    {
        VkCommandBuffer asdf = VK_NULL_HANDLE;

        TestScriptComponent container(L"C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\x64\\Debug\\VulkanGameEngineGameObjectScriptsCLI.dll");
        container.Update(5);
        container.BufferUpdate(asdf, 5);
         //result = container.Draw(wrapperHandle, 5);
        container.Destroy();
        int result = container.GetMemorySize();
    }
    catch (const std::exception& e) 
    {
        std::cerr << "Error: " << e.what() << std::endl;
        return 1;
    }

    return 0;
}