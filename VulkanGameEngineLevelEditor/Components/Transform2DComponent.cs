using GlmSharp;
using Newtonsoft.Json;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.Vulkan;

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

        public Transform2DComponent(GameObject parentGameObject, GameObjectComponentModel model) : base(parentGameObject, ComponentTypeEnum.kTransform2DComponent)
        {
            var transform = model as Transform2DComponentModel;

            ParentGameObject = parentGameObject;

            Name = model.Name;
            GameObjectTransform = mat4.Identity;
            GameObjectPosition = transform.GameObjectPosition;
            GameObjectRotation = transform.GameObjectRotation;
            GameObjectScale = transform.GameObjectScale;
            ComponentType = transform.ComponentType;
        }

        public Transform2DComponent(GameObject parentGameObject, vec2 position) : base(parentGameObject, ComponentTypeEnum.kTransform2DComponent)
        {
            Name = "GameObjectTransform2DComponent";

            GameObjectTransform = mat4.Identity;
            GameObjectPosition = position;
            GameObjectRotation = new vec2(0.0f, 0.0f);
            GameObjectScale = new vec2(1.0f, 1.0f);
        }

        public Transform2DComponent(GameObject parentGameObject, vec2 position, string name) : base(parentGameObject, name, ComponentTypeEnum.kTransform2DComponent)
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
            GameObjectTransform = mat4.Rotate(VMath.DegreesToRadians(GameObjectRotation.x), new vec3(1.0f, 0.0f, 0.0f));
            GameObjectTransform = mat4.Rotate(VMath.DegreesToRadians(GameObjectRotation.y), new vec3(0.0f, 1.0f, 0.0f));
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
