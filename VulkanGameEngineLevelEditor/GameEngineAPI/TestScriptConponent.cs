using GlmSharp;
using RGiesecke.DllExport;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineLevelEditor.RenderPassEditor;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{


    public class TestScriptConponent : GameObjectComponent
    {
        private TestScriptComponentDLL _testScriptConponentDLL { get; set;}
        public TestScriptConponent()
        {

        }

        private void Initialize()
        {
            _testScriptConponentDLL = new TestScriptComponentDLL(0);
        }

        public static TestScriptConponent CreateTestScriptConponent()
        {
            TestScriptConponent gameObject = MemoryManager.AllocateTestScriptConponent();
            gameObject.Initialize();
            return gameObject;
        }

        public override void Update(long startTime)
        {
            _testScriptConponentDLL.Update(startTime);
        }

        public override void Update(CommandBuffer commandBuffer, long startTime)
        {
            _testScriptConponentDLL?.Update(startTime);
        }

        public override void Draw(CommandBuffer commandBuffer, Pipeline pipeline, PipelineLayout shaderPipelineLayout, DescriptorSet descriptorSet, SceneDataBuffer sceneProperties)
        {
            
        }

        public override void Destroy()
        {
           _testScriptConponentDLL.Destroy();
        }

        public override int GetMemorySize()
        {
            return 32;
            //throw new NotImplementedException();
        }
    }

}
