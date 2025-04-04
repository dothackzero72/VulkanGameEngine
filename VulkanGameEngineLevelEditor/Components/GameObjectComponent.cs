using Coral.Managed.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineGameObjectScripts.Input;
using VulkanGameEngineGameObjectScripts.Interface;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using Newtonsoft.Json;
using VulkanGameEngineLevelEditor.Models;

namespace VulkanGameEngineLevelEditor.Components
{
    public unsafe class GameObjectComponent
    {
        [JsonIgnore]
        public GameObject CPPgameObjectPtr { get; set; }
        [JsonIgnore]
        public GameObjectComponent CPPcomponentPtr { get; set; }
        [JsonIgnore]
        public GameObject ParentGameObject { get; set; }

        public string Name { get; set; }
        public ComponentTypeEnum ComponentType { get; set; }

        public GameObjectComponent()
        {

        }

        public GameObjectComponent(uint gameObjectId, ComponentTypeEnum componentType)
        {
            //CPPcomponentPtr = cppComponentPtr;
            //CPPgameObjectPtr = cppGameObjectPtr;

            //GCHandle handle = GCHandle.FromIntPtr(csParentGameObject);
            //ParentGameObject = (GameObject)handle.Target;

            Name = "GameObjectComponent";
            ComponentType = componentType;
        }

        public GameObjectComponent(uint gameObjectId,  string name, ComponentTypeEnum componentType)
        {
            //CPPcomponentPtr = cppComponentPtr;
            //CPPgameObjectPtr = cppGameObjectPtr;

        //GCHandle handle = GCHandle.FromIntPtr(csParentGameObject);
        //ParentGameObject = (GameObject)handle.Target;

            Name = name;
            ComponentType = componentType;
        }

        public GameObjectComponent(GameObject parentGameObject, GameObjectComponentModel model)
        {
            ParentGameObject = parentGameObject;
            Name = model.Name;
            ComponentType = model.ComponentType;
        }

        public virtual void Input(KeyBoardKeys key, float deltaTime)
        {
        }

        public virtual void Update(VkCommandBuffer commandBuffer, float deltaTime)
        {

        }

        public virtual void Draw(VkCommandBuffer commandBuffer, VkPipeline pipeline, VkPipelineLayout pipelineLayout, ListPtr<VkDescriptorSet> descriptorSetList, SceneDataBuffer sceneProperties)
        {

        }

        public virtual void Destroy()
        {

        }

        public virtual int GetMemorySize()
        {
            return sizeof(GameObjectComponent);
        }

        //public IntPtr GetCPPgameObjectPtr()
        //{
        //    return CPPgameObjectPtr;
        //}

        //public IntPtr GetCPPComponentPtr()
        //{
        //    return CPPcomponentPtr;
        //}

        //public NativeString* GetPositionPtr()
        //{
        //    fixed (NativeString* positionPointer = &Name)
        //    {
        //        return positionPointer;
        //    }
        //}

        //public unsafe ComponentTypeEnum* GetComponentTypePtr()
        //{
        //    // Get the address of the field: `ComponentType`
        //    fixed (ComponentTypeEnum* ptr = &ComponentType)
        //    {
        //        return ptr;
        //    }
        //}
    }
}
