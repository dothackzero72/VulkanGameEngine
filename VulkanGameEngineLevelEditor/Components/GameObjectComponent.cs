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

namespace VulkanGameEngineLevelEditor.Components
{
    public unsafe class GameObjectComponent
    {
        public GameObject CPPgameObjectPtr;
        public GameObjectComponent CPPcomponentPtr;

        public GameObject ParentGameObject;
        public ComponentTypeEnum ComponentType { get; set; }
        public string Name { get; set; }

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
