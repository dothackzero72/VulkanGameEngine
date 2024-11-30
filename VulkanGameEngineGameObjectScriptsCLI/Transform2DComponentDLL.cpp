#include "pch.h"
#include "Transform2DComponentDLL.h"


Transform2DComponentDLL::Transform2DComponentDLL()
{
    std::cout << "do you hear me TestScriptComponentDLL" << std::endl;
    component = gcnew Transform2DComponent_CS();
}

Transform2DComponentDLL::Transform2DComponentDLL(void* gameObjectPtr, std::string name)
{
    std::cout << "do you hear me TestScriptComponentDLL3" << std::endl;
    component = gcnew Transform2DComponent_CS((IntPtr)gameObjectPtr, msclr::interop::marshal_as<String^>(name));
}

void Transform2DComponentDLL::Update(float deltaTime)
{
    Console::WriteLine("Drawing with buffer update");
    std::cout << "Test Object 1" << std::endl;
    component->Update(deltaTime);
}

void Transform2DComponentDLL::BufferUpdate(VkCommandBuffer commandBuffer, float deltaTime)
{
    Console::WriteLine("Drawing with buffer update");
    //component->BufferUpdate(static_cast<void*>(commandBuffer), startTime);
}

void Transform2DComponentDLL::Destroy()
{
    Console::WriteLine("Destroying component: {0}", Name);
    component->Destroy();
}

int Transform2DComponentDLL::GetMemorySize()
{
    return static_cast<int>(MemorySize);
}

extern "C"
{
    DLL_EXPORT_MANAGED void* DLL_CreateTransform2DComponent() {
        try
        {
            std::cout << "do you hear me DLL_CreateGameObjectTransform2DComponent" << std::endl;
            Transform2DComponentDLL^ wrapper = gcnew Transform2DComponentDLL();
            GCHandle handle = GCHandle::Alloc(wrapper);
            return (void*)GCHandle::ToIntPtr(handle).ToPointer();
        }
        catch (Exception^ e) {
            std::cerr << "Exception in managed code: " << msclr::interop::marshal_as<std::string>(e->Message) << std::endl;
            return nullptr;
        }
    }

    DLL_EXPORT_MANAGED void* DLL_CreateTransform2DComponentName(void* wrapperHandle, void* gameObjectPtr, std::string name)
    {
        if (wrapperHandle != nullptr)
        {
            try
            {
                Transform2DComponentDLL^ component = gcnew Transform2DComponentDLL(gameObjectPtr, name);
                GCHandle handle = GCHandle::Alloc(component);
                return (void*)GCHandle::ToIntPtr(handle).ToPointer();
            }
            catch (Exception^ e)
            {
                std::cerr << "Exception in managed code: " << msclr::interop::marshal_as<std::string>(e->Message) << std::endl;
                return nullptr;
            }
        }
    }

    DLL_EXPORT_MANAGED void DLL_Transform2DComponent_Update(void* wrapperHandle, long startTime)
    {
        try
        {
            if (wrapperHandle != nullptr)
            {
                IntPtr handlePtr(wrapperHandle);
                GCHandle handle = GCHandle::FromIntPtr(handlePtr);
                Transform2DComponentDLL^ managedWrapper = static_cast<Transform2DComponentDLL^>(handle.Target);
                if (managedWrapper != nullptr)
                {
                    managedWrapper->Update(startTime);
                }
            }
        }
        catch (Exception^ e)
        {
            std::cerr << "Exception in managed code: " << msclr::interop::marshal_as<std::string>(e->Message) << std::endl;
        }
    }

    DLL_EXPORT_MANAGED void DLL_Transform2DComponent_BufferUpdate(void* wrapperHandle, VkCommandBuffer commandBuffer, float deltaTime)
    {
        try
        {
            if (wrapperHandle != nullptr)
            {
                IntPtr handlePtr(wrapperHandle);
                GCHandle handle = GCHandle::FromIntPtr(handlePtr);
                Transform2DComponentDLL^ managedWrapper = static_cast<Transform2DComponentDLL^>(handle.Target);
                if (managedWrapper != nullptr)
                {
                    managedWrapper->BufferUpdate(commandBuffer, deltaTime);
                }
            }
        }
        catch (Exception^ e)
        {
            std::cerr << "Exception in managed code: " << msclr::interop::marshal_as<std::string>(e->Message) << std::endl;
        }
    }

    DLL_EXPORT_MANAGED void DLL_Transform2DComponent_Destroy(void* wrapperHandle)
    {
        try
        {
            if (wrapperHandle != nullptr)
            {
                IntPtr handlePtr(wrapperHandle);
                GCHandle handle = GCHandle::FromIntPtr(handlePtr);
                Transform2DComponentDLL^ managedWrapper = static_cast<Transform2DComponentDLL^>(handle.Target);
                if (managedWrapper != nullptr)
                {
                    managedWrapper->Destroy();
                }
            }
        }
        catch (Exception^ e)
        {
            std::cerr << "Exception in managed code: " << msclr::interop::marshal_as<std::string>(e->Message) << std::endl;
        }
    }

    DLL_EXPORT_MANAGED int DLL_Transform2DComponent_GetMemorySize(void* wrapperHandle)
    {
        try
        {
            if (wrapperHandle != nullptr)
            {
                IntPtr handlePtr(wrapperHandle);
                GCHandle handle = GCHandle::FromIntPtr(handlePtr);
                Transform2DComponentDLL^ managedWrapper = static_cast<Transform2DComponentDLL^>(handle.Target);
                if (managedWrapper != nullptr)
                {
                    return managedWrapper->GetMemorySize();
                }
            }
            return -1;
        }
        catch (Exception^ e)
        {
            std::cerr << "Exception in managed code: " << msclr::interop::marshal_as<std::string>(e->Message) << std::endl;
            return -1;
        }
    }
}