#include <iostream>
#include <string>
#include <vector>
#include <filesystem>
#include <chrono>
#include <functional>
#include <ranges>

#include <vulkan/vulkan_core.h>
#include <Coral/HostInstance.hpp>
#include <Coral/GC.hpp>
#include <Coral/Array.hpp>
#include <Coral/Attribute.hpp>
#include <TypeDef.h>
#include "SceneDataBuffer.h"
#include <VulkanWindow.h>

void ExceptionCallback(std::string_view InMessage)
{
    std::cout << "Unhandled native exception: " << InMessage << std::endl;
}

struct MyVec3
{
    float X;
    float Y;
    float Z;
};

void VectorAddIcall(MyVec3* InVec0, const MyVec3* InVec1)
{
    std::cout << "VectorAddIcall" << std::endl;
    InVec0->X += InVec1->X;
    InVec0->Y += InVec1->Y;
    InVec0->Z += InVec1->Z;
}

void PrintStringIcall(Coral::String InString)
{
    std::cout << std::string(InString) << std::endl;
}

void NativeArrayIcall(Coral::Array<float> InValues)
{
    std::cout << "NativeArrayIcall" << std::endl;
    for (auto value : InValues)
    {
        std::cout << value << std::endl;
    }
}

Coral::Array<float> ArrayReturnIcall()
{
    std::cout << "ArrayReturnIcall" << std::endl;
    return Coral::Array<float>::New({ 10.0f, 5000.0f, 1000.0f });
}

struct ComponentListContainer
{
    int Count;
    void* ptr;
};

class GameObjectComponent2;
class GameObject2
{
public: 
    String Name;
    std::shared_ptr<List<std::shared_ptr<GameObjectComponent2>>> GameObjectComponentList;

private:
    size_t ObjectComponentMemorySize = 0;

    std::shared_ptr<Coral::Type> CSclass;
    std::shared_ptr<Coral::ManagedObject> CSobject;

public:

    GameObject2()
    {

    }

    GameObject2(Coral::ManagedAssembly* assembly)
    {
        CSclass = std::make_shared<Coral::Type>(assembly->GetType("VulkanGameEngineGameObjectScripts.GameObject"));
        CSobject = std::make_shared<Coral::ManagedObject>(CSclass->CreateInstance());

        GameObjectComponentList = std::make_shared<List<std::shared_ptr<GameObjectComponent2>>>(CSobject->InvokeMethod<List<std::shared_ptr<GameObjectComponent2>>>("GetGameObjectComponentListPtr"));
        int a = 34;
      
    }

    void AddComponent(std::shared_ptr<GameObjectComponent2> component);
};

struct GameObjectComponentBlight
{
    std::shared_ptr<GameObject2> ParentGameObjectPtr;
    std::shared_ptr<int> componentType;
    std::shared_ptr<String> Name;

    GameObjectComponentBlight()
    {
    }
};

 class GameObjectComponent2
 {
 private:
     std::shared_ptr<Coral::Type> CSclass;
     std::shared_ptr<Coral::ManagedObject> CSobject;

     std::shared_ptr<mat4> GameObjectTransform;
     std::shared_ptr<vec2> GameObjectPosition;
     std::shared_ptr<vec2> GameObjectRotation;
     std::shared_ptr<vec2> GameObjectScale;

 public:
     GameObjectComponentBlight blight;

     GameObjectComponent2()
     {

     }

     GameObjectComponent2(Coral::ManagedAssembly* assembly, std::shared_ptr<GameObject2> baseObject)
     {
         blight.Name = std::make_shared<String>("asdfasdf");
         blight.componentType = std::make_shared<int>(kTransform2DComponent);
         blight.ParentGameObjectPtr = baseObject;

         auto a = &blight;
         auto b = &blight.ParentGameObjectPtr;
         auto c = &blight.componentType;
         auto d = &blight.Name;

         CSclass = std::make_shared<Coral::Type>(assembly->GetType("VulkanGameEngineGameObjectScripts.Transform2DComponent"));
         CSobject = std::make_shared<Coral::ManagedObject>(CSclass->CreateInstance());

         GameObjectPosition = std::shared_ptr<vec2>(CSobject->InvokeMethod<vec2*>("GetPositionPtr"));
         GameObjectRotation = std::shared_ptr<vec2>(CSobject->InvokeMethod<vec2*>("GetRotationPtr"));
         GameObjectScale = std::shared_ptr<vec2>(CSobject->InvokeMethod<vec2*>("GetScalePtr"));
         GameObjectTransform = std::shared_ptr<mat4>(CSobject->InvokeMethod<mat4*>("GetTransformMatrixPtr"));
     }

     virtual void Input(InputKey key, KeyState keyState)
     {
         CSobject->InvokeMethod("Input", key, keyState);
     }

     virtual void Update(float deltaTime)
     {
         GameObjectPosition->x += 1.0f;
         CSobject->InvokeMethod("Update", deltaTime);
     }

     virtual void BufferUpdate(VkCommandBuffer& commandBuffer, float deltaTime)
     {
         GameObjectPosition->x += 1.0f;
         CSobject->InvokeMethod("BufferUpdate", commandBuffer, deltaTime);
     }

     virtual void Draw(VkCommandBuffer& commandBuffer, VkPipeline& pipeline, VkPipelineLayout& shaderPipelineLayout, VkDescriptorSet& descriptorSet, SceneDataBuffer& sceneProperties)
     {
     }

     virtual void Destroy()
     {
         CSobject->InvokeMethod("Destroy");
     }
 };

 void GameObject2::AddComponent(std::shared_ptr<GameObjectComponent2> component)
 {
     // Ensure that the component is valid
     if (!component) {
         // Handle the null case (e.g., log an error)
         return;
     }

     // Assuming 'blight' is a pointer that you want to pass to C#
     void* blightPtr = &component->blight;
     component->blight.componentType = ComponentTypeEnum::kTransform2DComponent;

     // Call the C# method using your interop mechanism
     String className = "Transform2DComponent";
     auto a = this;
     CSobject->InvokeMethod("AddComponent", blightPtr, 2);
    
     // Add the component to the list
     GameObjectComponentList->emplace_back(component);
 }

int main(int argc, char** argv)
{
    auto exeDir = std::filesystem::path(argv[0]).parent_path();
    auto coralDir = exeDir.string();
    Coral::HostSettings settings =
    {
        .CoralDirectory = coralDir,
        .ExceptionCallback = ExceptionCallback
    };
    Coral::HostInstance hostInstance;
    hostInstance.Initialize(settings);

    auto loadContext = hostInstance.CreateAssemblyLoadContext("ExampleContext");

    std::string assemblyPath = "C:/Users/dotha/Documents/GitHub/VulkanGameEngine/ClassLibrary1/bin/Debug/ClassLibrary1.dll";
    Coral::ManagedAssembly* assembly = &loadContext.LoadAssembly(assemblyPath);
    
    VkCommandBuffer commandBuffer;

    std::shared_ptr<GameObject2> obj = std::make_shared<GameObject2>(GameObject2(assembly));
    obj->AddComponent(std::make_shared<GameObjectComponent2>(GameObjectComponent2(assembly, obj)));
    obj->AddComponent(std::make_shared<GameObjectComponent2>(GameObjectComponent2(assembly, obj)));
    (*obj->GameObjectComponentList)[0]->Input(InputKey::INPUTKEY_1, KeyState::KS_PRESSED);
    for (int x = 0; x <= 30; x++)
    {
        (*obj->GameObjectComponentList)[0]->Update((float)x);
    }
    (*obj->GameObjectComponentList)[1]->BufferUpdate(commandBuffer, 0.0f);
    (*obj->GameObjectComponentList)[1]->BufferUpdate(commandBuffer, 0.0f);
    (*obj->GameObjectComponentList)[1]->BufferUpdate(commandBuffer, 0.0f);
    (*obj->GameObjectComponentList)[1]->BufferUpdate(commandBuffer, 0.0f);
    (*obj->GameObjectComponentList)[1]->BufferUpdate(commandBuffer, 0.0f);
    (*obj->GameObjectComponentList)[0]->Destroy();

    //assembly.AddInternalCall("VulkanGameEngineGameObjectScripts.GameObject", "EnumIcall", reinterpret_cast<void*>(&EnumIcall));
    //assembly->AddInternalCall("VulkanGameEngineGameObjectScripts.ExampleClass", "VectorAddIcall", reinterpret_cast<void*>(&VectorAddIcall));
    //assembly->AddInternalCall("VulkanGameEngineGameObjectScripts.ExampleClass", "PrintStringIcall", reinterpret_cast<void*>(&PrintStringIcall));
    //assembly->AddInternalCall("VulkanGameEngineGameObjectScripts.ExampleClass", "NativeArrayIcall", reinterpret_cast<void*>(&NativeArrayIcall));
    //assembly->AddInternalCall("VulkanGameEngineGameObjectScripts.ExampleClass", "ArrayReturnIcall", reinterpret_cast<void*>(&ArrayReturnIcall));
    //assembly->UploadInternalCalls();


    auto exampleType = std::make_shared<Coral::Type>(assembly->GetType("VulkanGameEngineGameObjectScripts.ExampleClass"));
    auto exampleInstance = std::make_shared<Coral::ManagedObject>(exampleType->CreateInstance(50));
    //auto GameObjectPosition = std::shared_ptr<vec2>(exampleInstance->InvokeMethod<vec2*>("GetPositionPtr"));
    //auto GameObjectRotation = std::shared_ptr<vec2>(exampleInstance->InvokeMethod<vec2*>("GetRotationPtr"));
    //auto GameObjectScale = std::shared_ptr<vec2>(exampleInstance->InvokeMethod<vec2*>("GetScalePtr"));
    //auto GameObjectTransform = std::shared_ptr<mat4>(exampleInstance->InvokeMethod<mat4*>("GetTransformMatrixPtr"));

    //for (int x = 0; x <= 30; x++)
    //{
    //    GameObjectPosition->x += 1.0f;
    //    exampleInstance->InvokeMethod("Update");
    //}

    // Invoke the method named "MemberMethod" with a MyVec3 argument (doesn't return anything)
    exampleInstance->InvokeMethod("Void MemberMethod(MyVec3)", MyVec3{ 10.0f, 10.0f, 10.0f });

    // Invokes the setter on PublicProp with the value 10 (will be multiplied by 2 in C#)
    exampleInstance->SetPropertyValue("PublicProp", 10);

    // Get the value of PublicProp as an int
    std::cout << exampleInstance->GetPropertyValue<int32_t>("PublicProp") << std::endl;

    // Sets the value of the private field "myPrivateValue" with the value 10 (will NOT be multiplied by 2 in C#)
    exampleInstance->SetFieldValue("myPrivateValue", 10);

    // Get the value of myPrivateValue as an int
    auto asd = exampleInstance->GetFieldValue<int32_t>("myPrivateValue");


    // Sets the value of the private field "myPrivateValue" with the value 10 (will NOT be multiplied by 2 in C#)
    exampleInstance->SetFieldValue("myPrivateValue2", 32);

    // Get the value of myPrivateValue as an int
    auto asd233 = exampleInstance->GetFieldValue<int32_t>("myPrivateValue2");

    // Invokes StringDemo method which will in turn invoke PrintStringIcall with a string parameter
    exampleInstance->InvokeMethod("StringDemo");

    // Invokes ArrayDemo method which will in turn invoke NativeArrayIcall and pass the values we give here
    // and also invoke ArrayReturnIcall
    auto arr = Coral::Array<float>::New({ 5.0f, 0.0f, 10.0f, -50.0f });
    exampleInstance->InvokeMethod("ArrayDemo", arr);
    Coral::Array<float>::Free(arr);

    return 0;
}
