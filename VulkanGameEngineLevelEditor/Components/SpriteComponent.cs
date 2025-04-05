using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineGameObjectScripts.Input;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using Newtonsoft.Json;
using VulkanGameEngineLevelEditor.Models;

namespace VulkanGameEngineLevelEditor.Components
{
    public unsafe class SpriteComponent : GameObjectComponent
    {
        public Sprite SpriteObj { get; protected set; }
        public SpriteComponent()
        {

        }

        public SpriteComponent(GameObject parentGameObject, GameObjectComponentModel model) : base(parentGameObject, ComponentTypeEnum.kSpriteComponent)
        {
            var spriteModel = model as SpriteComponentModel;

            ParentGameObject = parentGameObject;
            SpriteObj = new Sprite(parentGameObject, spriteModel.SpriteObj);
        }

        public SpriteComponent(GameObject parentGameObject, string name, SpriteSheet spriteSheet) : base(parentGameObject, name, ComponentTypeEnum.kSpriteComponent)
        {
            SpriteObj = new Sprite(parentGameObject, spriteSheet);
        }

        public override void Input(KeyBoardKeys key, float deltaTime)
        {
            SpriteObj.Input(deltaTime);
        }

        public override void Update(VkCommandBuffer commandBuffer, float deltaTime)
        {
            SpriteObj.Update(commandBuffer, deltaTime);
        }

        public override void Draw(VkCommandBuffer commandBuffer, VkPipeline pipeline, VkPipelineLayout pipelineLayout, ListPtr<VkDescriptorSet> descriptorSetList, SceneDataBuffer sceneProperties)
        {

        }

        public override void Destroy()
        {
            SpriteObj.Destroy();
        }

        public virtual int GetMemorySize()
        {
            return (int)sizeof(SpriteComponent);
        }
    }
}
