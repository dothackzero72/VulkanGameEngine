using Silk.NET.Vulkan;

namespace VulkanGameEngineGameObjectScripts
{
    public unsafe class TestScriptComponent_CS : IGameObjectComponent
    {
        
        public int counter { get; set; }
        public string Name { get; set; }

        public ulong MemorySize { get; set; }

        public ComponentTypeEnum ComponentType { get; set; } = ComponentTypeEnum.kTestScriptComponent;
        public nint ParentGameObject { get; set; }

        public TestScriptComponent_CS()
        {

        }

        public TestScriptComponent_CS(IntPtr parentGameObject)
        {
            ParentGameObject = parentGameObject;
        }

        public TestScriptComponent_CS(IntPtr parentGameObject, String name)
        {
            ParentGameObject = parentGameObject;
        }

        public void Destroy()
        {
            Console.WriteLine("C# Destroy");
        }

        public void Draw(nint commandBuffer, nint pipeline, nint shaderPipelineLayout, nint descriptorSet, SceneDataBuffer sceneProperties)
        {
            CommandBuffer commandBuffer1 = new CommandBuffer { Handle = commandBuffer };
            Pipeline pipeline1 = new Pipeline { Handle = (ulong)pipeline };
            PipelineLayout shaderPipelineLayout1 = new PipelineLayout { Handle = (ulong)shaderPipelineLayout };
            DescriptorSet descriptorSet1 = new DescriptorSet { Handle = (ulong)descriptorSet };

            Draw(commandBuffer1, pipeline1, shaderPipelineLayout1, descriptorSet1, sceneProperties);
        }

        public void Draw(CommandBuffer commandBuffer, Pipeline pipeline, PipelineLayout shaderPipelineLayout, DescriptorSet descriptorSet, SceneDataBuffer sceneProperties)
        {
            Console.WriteLine("C# Draw");
        }

        public int GetMemorySize()
        {
            Console.WriteLine("C# GetMemorySize");
            return sizeof(TestScriptComponent_CS);
        }

        public void Update(float deltaTime)
        {
            counter++;
            Console.WriteLine("C# Update");
        }

        public void BufferUpdate(nint commandBuffer, float deltaTime)
        {
            CommandBuffer commandBuffer1 = new CommandBuffer { Handle = commandBuffer };
            BufferUpdate(commandBuffer1, deltaTime);
        }

        public void BufferUpdate(CommandBuffer commandBuffer, float deltaTime)
        {
            counter++;
            Console.WriteLine("C# Buffer Update");
        }
    }

    public class SimpleTest
    {
        public int counter = 0;

        public SimpleTest()
        {
            counter = 0;
        }

        public int SimpleFunction(int input)
        {
            counter++;
            return counter;
        }

        public void SimpleDestroy()
        {
            Console.WriteLine("Simple function called.");
        }
    }
}