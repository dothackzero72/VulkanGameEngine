using Silk.NET.Vulkan;

namespace VulkanGameEngineGameObjectScripts
{
    public unsafe class TestScriptComponent : GameObjectComponent
    {
        public int counter { get; set; } = 0;
        public TestScriptComponent()
        {

        }

        public TestScriptComponent(ComponentTypeEnum componentType) : base(componentType)
        {
        }

        public TestScriptComponent(String name, ComponentTypeEnum componentType) : base(name, componentType)
        {
        }

        public override void Destroy()
        {
            Console.WriteLine("C# Destroy");
        }

        public override void Draw(CommandBuffer commandBuffer, Pipeline pipeline, PipelineLayout shaderPipelineLayout, DescriptorSet descriptorSet, SceneDataBuffer sceneProperties)
        {
            Console.WriteLine("C# Draw");
        }

        public override int GetMemorySize()
        {
            Console.WriteLine("C# GetMemorySize");
            return sizeof(TestScriptComponent);
        }

        public override void Update(long startTime)
        {
            counter++;
            Console.WriteLine("C# Update");
        }

        public override void BufferUpdate(CommandBuffer commandBuffer, long startTime)
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