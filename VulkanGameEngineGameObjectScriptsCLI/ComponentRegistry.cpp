#include "pch.h"
#include "ComponentRegistry.h"

void ComponentRegistry::RegisterComponent(IntPtr wrapperObjectPtrKey, GameObjectComponentDLL^ component)
{
    GCHandle handle = GCHandle::Alloc(component);

    instanceRegistry->Add(wrapperObjectPtrKey, component);
    handleRegistry->Add(wrapperObjectPtrKey, handle);
    ComponentRegistry::DisplayRegistryDebug();
}

GameObjectComponentDLL^ ComponentRegistry::GetComponent(IntPtr wrapperObjectPtrKey)
{
    GameObjectComponentDLL^ component = nullptr;
    if (instanceRegistry->TryGetValue(wrapperObjectPtrKey, component))
    {
        return component;
    }
    return nullptr;
}

void ComponentRegistry::DeregisterComponent(IntPtr wrapperObjectPtrKey)
{
    GCHandle handle;
    if (handleRegistry->TryGetValue(wrapperObjectPtrKey, handle))
    {
        handle.Free();
        handleRegistry->Remove(wrapperObjectPtrKey);
    }
    instanceRegistry->Remove(wrapperObjectPtrKey);
}

void ComponentRegistry::DisplayRegistryDebug()
{
    std::cout << "Current Component Registry:" << std::endl;
    for each (auto instance in instanceRegistry)
    {
        IntPtr wrapperObjectPtrKey = instance.Key;
        GameObjectComponentDLL^ component = instance.Value;

        String^ name = component->Name;
        uint64_t memorySize = component->GetMemorySize(); 

        std::cout << "Key: 0x" << std::hex << wrapperObjectPtrKey.ToInt64() << ", Name: " << msclr::interop::marshal_as<std::string>(name) << std::dec
            << ", Memory Size: " << memorySize << std::endl;
    }
}
