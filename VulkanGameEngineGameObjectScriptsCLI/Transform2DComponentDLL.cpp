#include "Transform2DComponentDLL.h"

Transform2DComponentDLL::Transform2DComponentDLL() : GameObjectComponentDLL()
{
    component = gcnew Transform2DComponent();
}

Transform2DComponentDLL::Transform2DComponentDLL(void* wrapperObjectPtrKey, std::string name) : GameObjectComponentDLL(wrapperObjectPtrKey, name, ComponentTypeEnum::kGameObjectTransform2DComponent)
{
    component = gcnew Transform2DComponent((IntPtr)wrapperObjectPtrKey, msclr::interop::marshal_as<String^>(name));
}

void Transform2DComponentDLL::Update(float deltaTime)
{
    component->Update(deltaTime);
}

void Transform2DComponentDLL::BufferUpdate(VkCommandBuffer commandBuffer, float deltaTime)
{
   // component->BufferUpdate(static_cast<void*>(commandBuffer), startTime);
}

void Transform2DComponentDLL::Destroy()
{
    component->Destroy();
}

int Transform2DComponentDLL::GetMemorySize()
{
    return static_cast<int>(MemorySize);
}

extern "C"
{
    DLL_EXPORT_MANAGED void* DLL_CreateTransform2DComponent(void* objectKey)
    {
        GameObjectComponentDLL^ existingComponent = ComponentRegistry::GetComponent((IntPtr)objectKey);
        if (existingComponent)
        {
            return GCHandle::ToIntPtr(GCHandle::Alloc(existingComponent)).ToPointer();
        }

        try
        {
            Transform2DComponentDLL^ wrapper = gcnew Transform2DComponentDLL(objectKey, "adsfa");
            GCHandle handle = GCHandle::Alloc(wrapper);

            auto ptr = GCHandle::ToIntPtr(handle).ToPointer();
            ComponentRegistry::RegisterComponent((IntPtr)ptr, wrapper);

            return ptr;
        }
        catch (Exception^ e)
        {
            std::cerr << "Exception in managed code: " << msclr::interop::marshal_as<std::string>(e->Message) << std::endl;
            return nullptr;
        }
    }

    DLL_EXPORT_MANAGED void DLL_Transform2DComponent_Update(void* objectKey, long startTime)
    {
        if (objectKey == nullptr)
        {
            std::cerr << "Invalid objectKey: nullptr passed." << std::endl;
            return;
        }

        try
        {
            GameObjectComponentDLL^ existingComponent = ComponentRegistry::GetComponent((IntPtr)objectKey);
            if (existingComponent != nullptr)
            {
                Transform2DComponentDLL^ transform = safe_cast<Transform2DComponentDLL^>(existingComponent);
                transform->Update(startTime);
            }
            else
            {
                std::cerr << "Component not found for objectKey." << std::endl;
            }
        }
        catch (Exception^ e)
        {
            std::cerr << "Exception in managed code: " << msclr::interop::marshal_as<std::string>(e->Message) << std::endl;
        }
    }

    DLL_EXPORT_MANAGED void DLL_Transform2DComponent_BufferUpdate(void* objectKey, VkCommandBuffer commandBuffer, float deltaTime)
    {
        if (objectKey == nullptr)
        {
            std::cerr << "Invalid objectKey: nullptr passed." << std::endl;
            return;
        }

        try
        {
            GameObjectComponentDLL^ existingComponent = ComponentRegistry::GetComponent((IntPtr)objectKey);
            if (existingComponent != nullptr)
            {
                Transform2DComponentDLL^ transform = safe_cast<Transform2DComponentDLL^>(existingComponent);
                transform->BufferUpdate(commandBuffer, deltaTime);
            }
            else
            {
                std::cerr << "Component not found for objectKey." << std::endl;
            }
        }
        catch (Exception^ e)
        {
            std::cerr << "Exception in managed code: " << msclr::interop::marshal_as<std::string>(e->Message) << std::endl;
        }
    }

    DLL_EXPORT_MANAGED void DLL_DestroyTransform2DComponent(void* objectKey) 
    {
        if (objectKey == nullptr)
        {
            std::cerr << "Invalid objectKey: nullptr passed." << std::endl;
            return;
        }

        try
        {
            GameObjectComponentDLL^ existingComponent = ComponentRegistry::GetComponent((IntPtr)objectKey);
            if (existingComponent != nullptr)
            {
                Transform2DComponentDLL^ transform = safe_cast<Transform2DComponentDLL^>(existingComponent);
                transform->Destroy();

                ComponentRegistry::DeregisterComponent((IntPtr)objectKey);
            }
            else
            {
                std::cerr << "Component not found for objectKey." << std::endl;
            }
        }
        catch (Exception^ e)
        {
            std::cerr << "Exception in managed code: " << msclr::interop::marshal_as<std::string>(e->Message) << std::endl;
        }
    }

    DLL_EXPORT_MANAGED int DLL_Transform2DComponent_GetMemorySize(void* objectKey)
    {
        if (objectKey == nullptr)
        {
            std::cerr << "Invalid objectKey: nullptr passed." << std::endl;
            return -1;
        }

        try
        {
            GameObjectComponentDLL^ existingComponent = ComponentRegistry::GetComponent((IntPtr)objectKey);
            if (existingComponent != nullptr)
            {
                Transform2DComponentDLL^ transform = safe_cast<Transform2DComponentDLL^>(existingComponent);
                return transform->GetMemorySize();
            }
            else
            {
                std::cerr << "Component not found for objectKey." << std::endl;
            }
        }
        catch (Exception^ e)
        {
            std::cerr << "Exception in managed code: " << msclr::interop::marshal_as<std::string>(e->Message) << std::endl;
        }
        return -1;
    }

    //DLL_EXPORT_MANAGED glm::vec2* DLL_Transform2DComponent_GetPositionPtr(void* objectKey)
    //{
    //    if (objectKey == nullptr)
    //    {
    //        std::cerr << "Invalid objectKey: nullptr passed." << std::endl;
    //        return nullptr;
    //    }

    //    try
    //    {
    //        GameObjectComponentDLL^ existingComponent = ComponentRegistry::GetComponent((IntPtr)objectKey);
    //        if (existingComponent != nullptr)
    //        {
    //            Transform2DComponentDLL^ transform = safe_cast<Transform2DComponentDLL^>(existingComponent);
    //            return transform->GetPositionPtr();
    //        }
    //        else
    //        {
    //            std::cerr << "Component not found for objectKey." << std::endl;
    //        }
    //        return nullptr;
    //    }
    //    catch (Exception^ e)
    //    {
    //        std::cerr << "Exception in managed code: " << msclr::interop::marshal_as<std::string>(e->Message) << std::endl;
    //        return nullptr;
    //    }
    //}

    //DLL_EXPORT_MANAGED glm::vec2* DLL_Transform2DComponent_GetRotationPtr(void* objectKey)
    //{
    //    if (objectKey == nullptr)
    //    {
    //        std::cerr << "Invalid objectKey: nullptr passed." << std::endl;
    //        return nullptr;
    //    }

    //    try
    //    {
    //        GameObjectComponentDLL^ existingComponent = ComponentRegistry::GetComponent((IntPtr)objectKey);
    //        if (existingComponent != nullptr)
    //        {
    //            Transform2DComponentDLL^ transform = safe_cast<Transform2DComponentDLL^>(existingComponent);
    //            return transform->GetRotationPtr();
    //        }
    //        else
    //        {
    //            std::cerr << "Component not found for objectKey." << std::endl;
    //        }
    //        return nullptr;
    //    }
    //    catch (Exception^ e)
    //    {
    //        std::cerr << "Exception in managed code: " << msclr::interop::marshal_as<std::string>(e->Message) << std::endl;
    //        return nullptr;
    //    }
    //}

    //DLL_EXPORT_MANAGED glm::vec2* DLL_Transform2DComponent_GetScalePtr(void* objectKey)
    //{
    //    if (objectKey == nullptr)
    //    {
    //        std::cerr << "Invalid objectKey: nullptr passed." << std::endl;
    //        return nullptr;
    //    }

    //    try
    //    {
    //        GameObjectComponentDLL^ existingComponent = ComponentRegistry::GetComponent((IntPtr)objectKey);
    //        if (existingComponent != nullptr)
    //        {
    //            Transform2DComponentDLL^ transform = safe_cast<Transform2DComponentDLL^>(existingComponent);
    //            return transform->GetScalePtr();
    //        }
    //        else
    //        {
    //            std::cerr << "Component not found for objectKey." << std::endl;
    //        }
    //        return nullptr;
    //    }
    //    catch (Exception^ e)
    //    {
    //        std::cerr << "Exception in managed code: " << msclr::interop::marshal_as<std::string>(e->Message) << std::endl;
    //        return nullptr;
    //    }
    //}

    //DLL_EXPORT_MANAGED glm::mat4* DLL_Transform2DComponent_GetTransformMatrixPtr(void* objectKey)
    //{
    //    if (objectKey == nullptr)
    //    {
    //        std::cerr << "Invalid objectKey: nullptr passed." << std::endl;
    //        return nullptr;
    //    }

    //    try
    //    {
    //        GameObjectComponentDLL^ existingComponent = ComponentRegistry::GetComponent((IntPtr)objectKey);
    //        if (existingComponent != nullptr)
    //        {
    //            Transform2DComponentDLL^ transform = safe_cast<Transform2DComponentDLL^>(existingComponent);
    //            return transform->GetTransformMatrixPtr();
    //        }
    //        else
    //        {
    //            std::cerr << "Component not found for objectKey." << std::endl;
    //        }
    //        return nullptr;
    //    }
    //    catch (Exception^ e)
    //    {
    //        std::cerr << "Exception in managed code: " << msclr::interop::marshal_as<std::string>(e->Message) << std::endl;
    //        return nullptr;
    //    }
    //}
}