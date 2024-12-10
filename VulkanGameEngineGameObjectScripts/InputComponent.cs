//using Silk.NET.Vulkan;
//using System.Runtime.InteropServices;

//namespace VulkanGameEngineGameObjectScripts
//{
//    public unsafe class InputComponent : GameObjectComponent
//    {
//        private GameObject ParentGameObject { get; set; }

//        public string Name;
//        public ulong MemorySize;
//        public ComponentTypeEnum ComponentType { get; set; } = ComponentTypeEnum.kGameObjectTransform2DComponent;

//        public InputComponent()
//        {
//            ParentGameObjectPtr = IntPtr.Zero;
//            Name = "GameObjectTransform2DComponent";
//        }

//        public InputComponent(IntPtr parentGameObject)
//        {
//            ParentGameObjectPtr = parentGameObject;

//            GCHandle handle = GCHandle.FromIntPtr(ParentGameObjectPtr);
//            ParentGameObject = handle.Target as GameObject;

//            Name = "GameObjectTransform2DComponent";
//        }

//        public override void Input(InputKey key, KeyState keyState)
//        {
//            //var parentGameObject = IGameObjectComponent;

//            if (Keyboard.keyboardState.KeyPressed[(int)InputKey.INPUTKEY_E] == KeyState.KS_PRESSED)
//            {

//            }

//            if (Keyboard.keyboardState.KeyPressed[(int)InputKey.INPUTKEY_A] == KeyState.KS_PRESSED)
//            {

//            }

//            if (Keyboard.keyboardState.KeyPressed[(int)InputKey.INPUTKEY_S] == KeyState.KS_PRESSED)
//            {

//            }

//            if (Keyboard.keyboardState.KeyPressed[(int)InputKey.INPUTKEY_D] == KeyState.KS_PRESSED)
//            {

//            }
//        }

//        public override void Update(float deltaTime)
//        {
//        }

//        public override void BufferUpdate(CommandBuffer commandBuffer, float deltaTime)
//        {
//        }

//        public override void Draw(CommandBuffer commandBuffer, Pipeline pipeline, PipelineLayout shaderPipelineLayout, DescriptorSet descriptorSet, SceneDataBuffer sceneProperties)
//        {
//        }

//        public override void Destroy()
//        {
//        }

//        public override int GetMemorySize()
//        {
//            return (int)sizeof(InputComponent);
//        }
//    }
//}
