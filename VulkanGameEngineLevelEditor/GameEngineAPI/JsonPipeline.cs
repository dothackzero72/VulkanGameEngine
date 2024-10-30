using Newtonsoft.Json;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{

    public unsafe class JsonPipeline
    {
        Vk vk = Vk.GetApi();
        public DescriptorPool descriptorPool { get; protected set; }
        public List<DescriptorSetLayout> descriptorSetLayoutList { get; protected set; }
        public DescriptorSet descriptorSet { get; protected set; }
        public Pipeline pipeline { get; protected set; }
        public PipelineLayout pipelineLayout { get; protected set; }
        public PipelineCache pipelineCache { get; protected set; }
        public JsonPipeline()
        {
        }

        public JsonPipeline(String jsonPipelineFilePath)
        {

            string jsonContent = File.ReadAllText(jsonPipelineFilePath);
            RenderPipeline obj = JsonConvert.DeserializeObject<RenderPipeline>(jsonContent);

            PipelineShaderStageCreateInfo* shadermoduleList = stackalloc[]
            {
                VulkanRenderer.CreateShader(obj.VertexShader,  ShaderStageFlags.VertexBit),
                VulkanRenderer.CreateShader(obj.FragmentShader, ShaderStageFlags.FragmentBit)
            };

            List<PushConstantRange> pushConstantRangeList = new List<PushConstantRange>();
            fixed (DescriptorSetLayout* descriptorSet = descriptorSetLayoutList.ToArray())
            {
                pushConstantRangeList = new List<PushConstantRange>()
                {
                    new PushConstantRange()
                    {
                        StageFlags = ShaderStageFlags.VertexBit | ShaderStageFlags.FragmentBit,
                        Offset = 0,
                        Size = (uint)sizeof(SceneDataBuffer)
                    }
                };


                fixed (PushConstantRange* pushConstantRange = pushConstantRangeList.ToArray())
                {
                    PipelineLayoutCreateInfo pipelineLayoutInfo = new PipelineLayoutCreateInfo
                    {
                        SType = StructureType.PipelineLayoutCreateInfo,
                        SetLayoutCount = descriptorSetLayoutList.UCount(),
                        PSetLayouts = descriptorSet,
                        PushConstantRangeCount = pushConstantRangeList.UCount(),
                        PPushConstantRanges = pushConstantRange,
                        Flags = PipelineLayoutCreateFlags.None,
                        PNext = null,
                    };
                    vk.CreatePipelineLayout(VulkanRenderer.device, &pipelineLayoutInfo, null, out PipelineLayout pipelinelayout);
                }
            }

            PipelineVertexInputStateCreateInfo vertexInputInfo = new PipelineVertexInputStateCreateInfo();
            fixed (VertexInputBindingDescription* vertexBindings = Vertex3D.GetBindingDescriptions().ToArray())
            fixed (VertexInputAttributeDescription* attributeBindings = Vertex3D.GetAttributeDescriptions().ToArray())
            {
                vertexInputInfo = new PipelineVertexInputStateCreateInfo()
                {
                    SType = StructureType.PipelineVertexInputStateCreateInfo,
                    PVertexAttributeDescriptions = attributeBindings,
                    PVertexBindingDescriptions = vertexBindings,
                    VertexAttributeDescriptionCount = Vertex3D.GetAttributeDescriptions().UCount(),
                    VertexBindingDescriptionCount = Vertex3D.GetBindingDescriptions().UCount(),
                    Flags = 0,
                    PNext = null
                };
            }

            PipelineInputAssemblyStateCreateInfo pipelineInputAssemblyStateCreateInfo = new PipelineInputAssemblyStateCreateInfo()
            {
                SType = StructureType.PipelineInputAssemblyStateCreateInfo,
                Topology = obj.PipelineInputAssemblyStateCreateInfo.Topology,
                PrimitiveRestartEnable = obj.PipelineInputAssemblyStateCreateInfo.PrimitiveRestartEnable,
                Flags = 0,
                PNext = null,
            };

            PipelineViewportStateCreateInfo pipelineViewportStateCreateInfo = new PipelineViewportStateCreateInfo();
            fixed (Viewport* viewportPtr = obj.viewportList.ToArray())
            fixed (Rect2D* scissorPtr = obj.scissorList.ToArray())
            {
                pipelineViewportStateCreateInfo = new PipelineViewportStateCreateInfo
                {
                    SType = StructureType.PipelineViewportStateCreateInfo,
                    ViewportCount = obj.viewportList.UCount(),
                    PViewports = viewportPtr,
                    ScissorCount = obj.scissorList.UCount(),
                    PScissors = scissorPtr,
                    Flags = 0,
                    PNext = null
                };
            }
        }

        public void CreatePipeline()
        {
           
        }
    }
}
