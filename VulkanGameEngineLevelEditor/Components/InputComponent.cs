using System.Linq;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.Components
{
    public unsafe class InputComponent : GameObjectComponent
    {
        Transform2DComponent transform;

        public InputComponent() : base()
        {
            Name = "InputComponent";
            ComponentType = ComponentTypeEnum.kInputComponent;
        }

        public InputComponent(GameObject parentGameObject) : base(parentGameObject, ComponentTypeEnum.kInputComponent)
        {
            Name = "InputComponent";
            transform = ParentGameObject.GameObjectComponentList.Where(x => x.ComponentType == ComponentTypeEnum.kTransform2DComponent).First() as Transform2DComponent;
        }

        public InputComponent(GameObject parentGameObject, string name) : base(parentGameObject, name, ComponentTypeEnum.kInputComponent)
        {
            transform = ParentGameObject.GameObjectComponentList.Where(x => x.ComponentType == ComponentTypeEnum.kTransform2DComponent).First() as Transform2DComponent;
        }

        public override void Destroy()
        {

        }

        public override void Draw(VkCommandBuffer commandBuffer, VkPipeline pipeline, VkPipelineLayout pipelineLayout, ListPtr<VkDescriptorSet> descriptorSetList, SceneDataBuffer sceneProperties)
        {

        }

        public override int GetMemorySize()
        {
            return sizeof(InputComponent);
        }

        //public override void Input(KeyBoardKeys key, float deltaTime)
        //{
        //    if (transform != null)
        //    {
        //        if (key == KeyBoardKeys.W)
        //        {
        //            transform.GameObjectPosition.y += 0.01f;
        //        }
        //        if (key == KeyBoardKeys.A)
        //        {
        //            transform.GameObjectPosition.x -= 0.01f;
        //        }
        //        if (key == KeyBoardKeys.S)
        //        {
        //            transform.GameObjectPosition.y -= 0.01f;
        //        }
        //        if (key == KeyBoardKeys.D)
        //        {
        //            transform.GameObjectPosition.x += 0.01f;
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Transform not found: GameObjectId: 0x{CPPcomponentPtr.ToString("x")} TransformID: 0x{CPPcomponentPtr.ToString("x")}");
        //    }
        //}

        public void Input(int key, float deltaTime)
        {
            if (transform != null)
            {
                if ((KeyBoardKeys)key == KeyBoardKeys.W)
                {
                    //  transform.GameObjectPosition.y += 0.01f;
                }
                if ((KeyBoardKeys)key == KeyBoardKeys.A)
                {
                    //  transform.GameObjectPosition.x -= 0.01f;
                }
                if ((KeyBoardKeys)key == KeyBoardKeys.S)
                {
                    //  transform.GameObjectPosition.y -= 0.01f;
                }
                if ((KeyBoardKeys)key == KeyBoardKeys.D)
                {
                    //  transform.GameObjectPosition.x += 0.01f;
                }
            }
            else
            {
                //  Console.WriteLine($"Transform not found: GameObjectId: 0x{CPPcomponentPtr.ToString("x")} TransformID: 0x{CPPcomponentPtr.ToString("x")}");
            }
        }

        public override void Update(VkCommandBuffer commandBuffer, float deltaTime)
        {

        }

    }
}
