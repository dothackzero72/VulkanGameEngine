using Newtonsoft.Json;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.Components
{
    public unsafe class GameObjectComponent
    {
        [JsonIgnore]
        public GameObject ParentGameObject { get; set; }

        public string Name { get; set; }
        public ComponentTypeEnum ComponentType { get; set; }

        public GameObjectComponent()
        {

        }

        public GameObjectComponent(GameObject parentGameObject, ComponentTypeEnum componentType)
        {
            ParentGameObject = parentGameObject;
            Name = "GameObjectComponent";
            ComponentType = componentType;
        }

        public GameObjectComponent(GameObject parentGameObject, string name, ComponentTypeEnum componentType)
        {
            ParentGameObject = parentGameObject;
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
