#include "pch.h"
#include "GameObjectComponentDLL.h"
#include <msclr/marshal.h>
#include <msclr/marshal_cppstd.h>
#include "VulkanGameEngineGameObjectScriptsCLI.h"
#include <iostream>

using namespace System;
using namespace System::Runtime::InteropServices;

GameObjectComponentDLL::GameObjectComponentDLL()
{

}

GameObjectComponentDLL::GameObjectComponentDLL(void* GameObjectPtr, ComponentTypeEnum componentType)
{
    Name = "unnamed";
    ComponentType = componentType;
}

GameObjectComponentDLL::GameObjectComponentDLL(void* GameObjectPtr, String^ name, ComponentTypeEnum componentType)
{
    Name = name;
    ComponentType = componentType;
}

//std::string GameObjectComponentDLL::GetName()
//{
//    auto name = Name;
//    return msclr::interop::marshal_as<std::string>(name);
//}
//
//uint32_t GameObjectComponentDLL::GetComponentType()
//{
//    return static_cast<uint32_t>(ComponentType);
//}

extern "C"
{
   /* DLL_EXPORT_MANAGED std::string DLL_TestScriptComponent_GetName(void* wrapperHandle)
    {
        try
        {
            if (wrapperHandle != nullptr)
            {
                IntPtr handlePtr(wrapperHandle);
                GCHandle handle = GCHandle::FromIntPtr(handlePtr);
                GameObjectComponentDLL^ managedWrapper = static_cast<GameObjectComponentDLL^>(handle.Target);
                if (managedWrapper != nullptr)
                {
                    return msclr::interop::marshal_as<std::string>(managedWrapper->GetName());
                }
            }
            return "";
        }
        catch (Exception^ e)
        {
            std::cerr << "Exception in managed code: " << msclr::interop::marshal_as<std::string>(e->Message) << std::endl;
            return "";
        }
    }

    DLL_EXPORT_MANAGED uint32_t DLL_TestScriptComponent_ComponentTypeEnum(void* wrapperHandle)
    {
        try
        {
            if (wrapperHandle != nullptr)
            {
                IntPtr handlePtr(wrapperHandle);
                GCHandle handle = GCHandle::FromIntPtr(handlePtr);
                GameObjectComponentDLL^ managedWrapper = static_cast<GameObjectComponentDLL^>(handle.Target);
                if (managedWrapper != nullptr)
                {
                    return static_cast<uint32_t>(managedWrapper->GetComponentType());
                }
            }
            return static_cast<uint32_t>(-1);
        }
        catch (Exception^ e)
        {
            std::cerr << "Exception in managed code: " << msclr::interop::marshal_as<std::string>(e->Message) << std::endl;
            return static_cast<uint32_t>(-1);
        }
    }*/
}