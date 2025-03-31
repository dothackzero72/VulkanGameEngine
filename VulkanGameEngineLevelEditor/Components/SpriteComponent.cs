using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineGameObjectScripts.Input;
using VulkanGameEngineLevelEditor.GameEngineAPI;

namespace VulkanGameEngineLevelEditor.Components
{
    public unsafe class SpriteComponent : GameObjectComponent
    {
        public Sprite SpriteObj { get; protected set; }
        public SpriteComponent()
        {

        }

        public SpriteComponent(GameObject gameObjectId, string name, SpriteSheet spriteSheet) : base(gameObjectId.GameObjectId, ComponentTypeEnum.kSpriteComponent)
        {
            SpriteObj = new Sprite(gameObjectId, spriteSheet);
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
