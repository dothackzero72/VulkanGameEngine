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

        public SpriteComponent(GameObject gameObjectId, string name, SpriteSheet spriteSheet)
        {
            SpriteObj = new Sprite(gameObjectId, spriteSheet);
        }

        public override void Input(KeyBoardKeys key, float deltaTime)
        {

        }

        public override void Update(VkCommandBuffer commandBuffer, float deltaTime)
        {

        }

        public override void Draw(VkCommandBuffer commandBuffer, VkPipeline pipeline, VkPipelineLayout pipelineLayout, VkDescriptorSet descriptorSet, SceneDataBuffer sceneProperties)
        {

        }

        public override void Destroy()
        {

        }

        public virtual int GetMemorySize()
        {
            return (int)sizeof(GameObjectComponent);
        }
    }
}
