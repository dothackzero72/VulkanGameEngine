#pragma once
#include "GameObjectComponentDLL.h"

public ref class ComponentRegistry
{
private:
     static Dictionary<IntPtr, GameObjectComponentDLL^>^ instanceRegistry = gcnew Dictionary<IntPtr, GameObjectComponentDLL^>(90);
     static Dictionary<IntPtr, GCHandle>^ handleRegistry = gcnew Dictionary<IntPtr, GCHandle>(90);

public:
     static void RegisterComponent(IntPtr wrapperObjectPtrKey, GameObjectComponentDLL^ component);
     static GameObjectComponentDLL^ GetComponent(IntPtr wrapperObjectPtrKey);
     static void DeregisterComponent(IntPtr wrapperObjectPtrKey);
     static void DisplayRegistryDebug();
};