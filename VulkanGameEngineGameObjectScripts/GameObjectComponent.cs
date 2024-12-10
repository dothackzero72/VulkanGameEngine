//using GlmSharp;
//using Silk.NET.Vulkan;
//using System.Runtime.InteropServices;

//namespace VulkanGameEngineGameObjectScripts
//{

//    public unsafe abstract class GameObjectComponent
//    {
//        public IntPtr ParentGameObjectPtr = IntPtr.Zero;
//        public String Name { get; protected set; } = string.Empty;
//        public ComponentTypeEnum ComponentType { get; set; }
//        public abstract void Input(InputKey key, KeyState keyState);
//        public abstract void Update(float deltaTime);
//        public abstract void BufferUpdate(CommandBuffer commandBuffer, float deltaTime);
//        public abstract void Draw(CommandBuffer commandBuffer, Pipeline pipeline, PipelineLayout shaderPipelineLayout, DescriptorSet descriptorSet, SceneDataBuffer sceneProperties);
//        public abstract void Destroy();
//        public abstract int GetMemorySize();

//        protected IntPtr AllocateAndMarshal<T>(T structure) where T : unmanaged
//        {
//            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf<T>());
//            Marshal.StructureToPtr(structure, ptr, false);
//            return ptr;
//        }

//        public static object PointerToObject(IntPtr pMapping, Type type)
//        {
//            return Marshal.PtrToStructure(pMapping, type);
//        }
//    }
//}
