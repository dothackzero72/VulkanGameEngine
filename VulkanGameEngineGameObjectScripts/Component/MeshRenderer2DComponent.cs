using Coral.Managed.Interop;
using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineGameObjectScripts.Import;
using VulkanGameEngineGameObjectScripts.Input;

namespace VulkanGameEngineGameObjectScripts.Component
{
    //RenderMesh2DComponent is basically just a skeletion container for linking from C++ to C#. 
    //Mesh and Draw calls are too diffrent to cleanly run them on C#.
    public unsafe class RenderMesh2DComponent : GameObjectComponent
    {
        public RenderMesh2DComponent() : base()
        {
            Name = "RenderMesh2DComponent";
        }

        public RenderMesh2DComponent(IntPtr cppComponentPtr, IntPtr cppGameObjectPtr, IntPtr csParentGameObject) :
            base(cppComponentPtr, cppGameObjectPtr, csParentGameObject, ComponentTypeEnum.kRenderMesh2DComponent)
        {
            Name = "RenderMesh2DComponent";
        }

        public RenderMesh2DComponent(IntPtr cppComponentPtr, IntPtr cppGameObjectPtr, IntPtr csParentGameObject, NativeString name) :
            base(cppComponentPtr, cppGameObjectPtr, csParentGameObject, name, ComponentTypeEnum.kRenderMesh2DComponent)
        {

        }

        public override void Input(KeyBoardKeys key, float deltaTime)
        {
        }

        public override void Update(float deltaTime)
        {

        }

        public override void BufferUpdate(IntPtr commandBuffer, float deltaTime)
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
