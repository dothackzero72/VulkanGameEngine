using Coral.Managed.Interop;
using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineGameObjectScripts.Import;
using VulkanGameEngineGameObjectScripts.Vulkan;

namespace VulkanGameEngineGameObjectScripts
{
    public unsafe class Mesh2D;
    public unsafe class MeshRenderer2DComponent : GameObjectComponent
    {
        public MeshRenderer2DComponent() : base()
        {
            Name = "GameObjectTransform2DComponent";
            ComponentType = ComponentTypeEnum.kRenderMesh2DComponent;
        }

        public MeshRenderer2DComponent(IntPtr parentGameObject) : base(parentGameObject, ComponentTypeEnum.kGameObjectTransform2DComponent)
        {
            Name = "GameObjectTransform2DComponent";
        }

        public MeshRenderer2DComponent(IntPtr parentGameObject, NativeString name) : base(parentGameObject, name, ComponentTypeEnum.kGameObjectTransform2DComponent)
        {

        }

        public override void Input(InputKey key, KeyState keyState)
        {
        }

        public override void Update(float deltaTime)
        {
        }

        public override void BufferUpdate(VkCommandBuffer commandBuffer, float deltaTime)
        {
        }

        public override void Draw(VkCommandBuffer commandBuffer, VkPipeline pipeline, VkPipelineLayout pipelineLayout, VkDescriptorSet descriptorSet, SceneDataBuffer sceneProperties)
        {
            Console.WriteLine("Draw called");
        }

        public override void Destroy()
        {
            Console.WriteLine("Destroy called");
        }

        public override int GetMemorySize()
        {
            return sizeof(MeshRenderer2DComponent);
        }
    }
}
