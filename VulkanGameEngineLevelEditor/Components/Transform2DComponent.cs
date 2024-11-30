using GlmSharp;
using Silk.NET.Vulkan;
using System;
using System.Runtime.InteropServices;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineLevelEditor.GameEngineAPI;

namespace VulkanGameEngineLevelEditor.Components
{
    public class Transform2DComponent : GameObjectComponent
    {
        private Transform2DComponent_CS _transform2D_CS;

        public Transform2DComponent_CS Transform2D_CS
        {
            get => _transform2D_CS;
            set
            {
                _transform2D_CS = value;
                ComponentType = _transform2D_CS.ComponentType;
            }
        }
        public mat4 GameObjectTransform
        {
            get => _transform2D_CS.GameObjectTransform;
            set => _transform2D_CS.GameObjectTransform = value;
        }

        public vec2 GameObjectPosition
        {
            get => _transform2D_CS.GameObjectPosition;
            set => _transform2D_CS.GameObjectPosition = value;
        }

        public vec2 GameObjectRotation
        {
            get => _transform2D_CS.GameObjectRotation;
            set => _transform2D_CS.GameObjectRotation = value;
        }

        public vec2 GameObjectScale
        {
            get => _transform2D_CS.GameObjectScale;
            set => _transform2D_CS.GameObjectScale = value;
        }

        public Transform2DComponent()
            : this(IntPtr.Zero, "unnamed") { }

        public Transform2DComponent(IntPtr parentGameObject, string name)
        {
            Transform2D_CS = new Transform2DComponent_CS(parentGameObject, name);
        }

        private IntPtr AllocateAndMarshal<T>(T structure) where T : unmanaged
        {
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf<T>());
            Marshal.StructureToPtr(structure, ptr, false);
            return ptr;
        }

        public override void BufferUpdate(CommandBuffer commandBuffer, float deltaTime)
        {
            IntPtr commandBufferPtr = AllocateAndMarshal(commandBuffer);
            Transform2D_CS.BufferUpdate(commandBufferPtr, deltaTime);
            Marshal.FreeHGlobal(commandBufferPtr);
        }

        public override void Destroy()
        {
            Transform2D_CS.Destroy();
        }

        public override void Draw(CommandBuffer commandBuffer, Pipeline pipeline, PipelineLayout shaderPipelineLayout, DescriptorSet descriptorSet, SceneDataBuffer sceneProperties)
        {
            IntPtr commandBufferPtr = AllocateAndMarshal(commandBuffer);
            IntPtr pipelinePtr = AllocateAndMarshal(pipeline);
            IntPtr shaderPipelineLayoutPtr = AllocateAndMarshal(shaderPipelineLayout);
            IntPtr descriptorSetPtr = AllocateAndMarshal(descriptorSet);

            Transform2D_CS.Draw(commandBufferPtr, pipelinePtr, shaderPipelineLayoutPtr, descriptorSetPtr, sceneProperties);

            Marshal.FreeHGlobal(commandBufferPtr);
            Marshal.FreeHGlobal(pipelinePtr);
            Marshal.FreeHGlobal(shaderPipelineLayoutPtr);
            Marshal.FreeHGlobal(descriptorSetPtr);
        }

        public override int GetMemorySize()
        {
            return Transform2D_CS.GetMemorySize();
        }

        public override void Update(float deltaTime)
        {
            Transform2D_CS.Update(deltaTime);
        }
    }
}