using Coral.Managed.Interop;
using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineGameObjectScripts.Import;
using VulkanGameEngineGameObjectScripts.Input;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using Newtonsoft.Json;

namespace VulkanGameEngineLevelEditor.Components
{
    public unsafe class Transform2DComponent : GameObjectComponent
    {
        [JsonIgnore]
        public mat4 GameObjectTransform { get; private set; }
        public vec2 GameObjectPosition { get; set; }
        public vec2 GameObjectRotation { get; set; }
        public vec2 GameObjectScale { get; set; }

        public Transform2DComponent() : base()
        {
            Name = "GameObjectTransform2DComponent";

            GameObjectTransform = mat4.Identity;
            GameObjectPosition = new vec2(0.0f, 0.0f);
            GameObjectRotation = new vec2(0.0f, 0.0f);
            GameObjectScale = new vec2(1.0f, 1.0f);
        }

        public Transform2DComponent(uint gameObjectId, vec2 position) : base(gameObjectId, ComponentTypeEnum.kGameObjectTransform2DComponent)
        {
            Name = "GameObjectTransform2DComponent";

            GameObjectTransform = mat4.Identity;
            GameObjectPosition = position;
            GameObjectRotation = new vec2(0.0f, 0.0f);
            GameObjectScale = new vec2(1.0f, 1.0f);
        }

        public Transform2DComponent(uint gameObjectId, vec2 position, string name) : base(gameObjectId, name, ComponentTypeEnum.kGameObjectTransform2DComponent)
        {
            GameObjectTransform = mat4.Identity;
            GameObjectPosition = position;
            GameObjectRotation = new vec2(0.0f, 0.0f);
            GameObjectScale = new vec2(1.0f, 1.0f);
        }

        public override void Input(KeyBoardKeys key, float deltaTime)
        {
        }

        public override void Update(VkCommandBuffer commandBuffer, float deltaTime)
        {
            GameObjectTransform = mat4.Identity;
            GameObjectTransform = mat4.Scale(new vec3(GameObjectScale, 0.0f));
            GameObjectTransform = mat4.Rotate(CLIMath.DegreesToRadians(GameObjectRotation.x), new vec3(1.0f, 0.0f, 0.0f));
            GameObjectTransform = mat4.Rotate(CLIMath.DegreesToRadians(GameObjectRotation.y), new vec3(0.0f, 1.0f, 0.0f));
            GameObjectTransform = mat4.Translate(new vec3(GameObjectPosition, 0.0f));
        }

        public override void Draw(VkCommandBuffer commandBuffer, VkPipeline pipeline, VkPipelineLayout pipelineLayout, ListPtr<VkDescriptorSet> descriptorSet, SceneDataBuffer sceneProperties)
        {

        }

        public override void Destroy()
        {

        }

        public override int GetMemorySize()
        {
            return (int)sizeof(Transform2DComponent);
        }

    }
}
