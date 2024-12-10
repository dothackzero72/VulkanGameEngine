//using Silk.NET.Vulkan;

//namespace VulkanGameEngineGameObjectScripts
//{
//    public unsafe class GameObject : GameObjectBase
//    {
//        public GameObject()
//        {
//        }

//        private GameObject(String name)
//        {
//            Name = name;
//        }

//        private GameObject(String name, List<GameObjectComponent> gameObjectComponentList)
//        {
//            Name = name;
//            GameObjectComponentList = gameObjectComponentList;
//        }

//        public void Initialize(String name)
//        {
//            Name = name;
//        }

//        public void Initialize(String name, List<GameObjectComponent> componentTypeList)
//        {
//            Name = name;
//            GameObjectComponentList = componentTypeList;
//        }

//        public override void Input(InputKey key, KeyState keyState)
//        {
//        }

//        public override void Update(float deltaTime)
//        {
//            foreach (GameObjectComponent component in GameObjectComponentList)
//            {
//                component.Update(deltaTime);
//            }
//        }

//        public override void BufferUpdate(CommandBuffer commandBuffer, float deltaTime)
//        {
//            foreach (GameObjectComponent component in GameObjectComponentList)
//            {
//                component.BufferUpdate(commandBuffer, deltaTime);
//            }
//        }

//        public override void Draw(CommandBuffer commandBuffer, Pipeline pipeline, PipelineLayout shaderPipelineLayout, DescriptorSet descriptorSet, SceneDataBuffer sceneProperties)
//        {
//            foreach (GameObjectComponent component in GameObjectComponentList)
//            {
//                component.Draw(commandBuffer, pipeline, shaderPipelineLayout, descriptorSet, sceneProperties);
//            }
//        }

//        public override void Destroy()
//        {
//            foreach (GameObjectComponent component in GameObjectComponentList)
//            {
//                component.Destroy();
//            }
//        }

//        public override int GetMemorySize()
//        {
//            return sizeof(GameObject);
//        }
//    }
//}
