using Coral.Managed.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineGameObjectScripts.Input;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineLevelEditor.GameEngineAPI;

namespace VulkanGameEngineLevelEditor.Components
{
    public unsafe class RenderMesh2DComponent : GameObjectComponent
    {
        public RenderMesh2DComponent() : base()
        {
            Name = "RenderMesh2DComponent";
        }

        public RenderMesh2DComponent(GameObjectComponent cppComponentPtr, GameObject cppGameObjectPtr, GameObject csParentGameObject) :
            base(cppComponentPtr, cppGameObjectPtr, csParentGameObject, ComponentTypeEnum.kRenderMesh2DComponent)
        {
            Name = "RenderMesh2DComponent";
        }

        public RenderMesh2DComponent(GameObjectComponent cppComponentPtr, GameObject cppGameObjectPtr, GameObject csParentGameObject, string name) :
            base(cppComponentPtr, cppGameObjectPtr, csParentGameObject, name, ComponentTypeEnum.kRenderMesh2DComponent)
        {

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

        public override int GetMemorySize()
        {
            return (int)sizeof(RenderMesh2DComponent);
        }
    }
}
