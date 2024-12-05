using GlmSharp;
using Silk.NET.Core.Attributes;
using Silk.NET.Vulkan;
using System.Runtime.InteropServices;

namespace VulkanGameEngineGameObjectScripts
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SceneDataBuffer
    {
        public uint MeshBufferIndex;
        public ulong buffer;
        public mat4 Projection;
        public mat4 View;
        public vec3 CameraPosition;

        public SceneDataBuffer()
        {
            MeshBufferIndex = uint.MaxValue;
            Projection = new mat4();
            View = new mat4();
            CameraPosition = new vec3(0.0f);
            buffer = 0;
        }
    };

    public enum ComponentTypeEnum
    {
        kUndefined,
        kRenderMesh2DComponent,
        kGameObjectTransform2DComponent,
        kInputComponent
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct GameObjectStruct
    {

        public String Name { get; set; }
        public List<GameObjectComponent> GameObjectComponentList { get; set; } = new List<GameObjectComponent>();

        public GameObjectStruct()
        {
        }
    }

    public abstract class GameObjectBase
    {
        public String Name { get; protected set; }
        public List<GameObjectComponent> GameObjectComponentList { get; protected set; } = new List<GameObjectComponent>();

        public abstract void Input(InputKey key, KeyState keyState);
        public abstract void Update(float deltaTime);
        public abstract void BufferUpdate(CommandBuffer commandBuffer, float deltaTime);
        public abstract void Draw(CommandBuffer commandBuffer, Pipeline pipeline, PipelineLayout shaderPipelineLayout, DescriptorSet descriptorSet, SceneDataBuffer sceneProperties);
        public abstract void Destroy();
        public abstract int GetMemorySize();

        public void AddComponent(GameObjectComponent newComponent)
        {
            GameObjectComponentList.Add(newComponent);
        }

        public void RemoveComponent(GameObjectComponent gameObjectComponent)
        {
            GameObjectComponentList.Remove(gameObjectComponent);
        }

        public GameObjectComponent GetComponentByName(String name)
        {
            return GameObjectComponentList.Where(x => x.Name == name).First();
        }

        public List<GameObjectComponent> GetComponentByComponentType(ComponentTypeEnum type)
        {
            return GameObjectComponentList.Where(x => x.ComponentType == type).ToList();
        }
    }
}
