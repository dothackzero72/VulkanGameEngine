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
    public enum DescriptorBindingPropertiesEnum
    {
        kMeshPropertiesDescriptor,
        kTextureDescriptor,
        kMaterialDescriptor,
        kBRDFMapDescriptor,
        kIrradianceMapDescriptor,
        kPrefilterMapDescriptor,
        kCubeMapDescriptor,
        kEnvironmentDescriptor,
        kSunLightDescriptor,
        kDirectionalLightDescriptor,
        kPointLightDescriptor,
        kSpotLightDescriptor,
        kReflectionViewDescriptor,
        kDirectionalShadowDescriptor,
        kPointShadowDescriptor,
        kSpotShadowDescriptor,
        kViewTextureDescriptor,
        kViewDepthTextureDescriptor,
        kCubeMapSamplerDescriptor,
        kRotatingPaletteTextureDescriptor,
        kMathOpperation1Descriptor,
        kMathOpperation2Descriptor,
    };

    public unsafe class JsonPipeline
    {
        Vk vk = Vk.GetApi();
        Device _device { get; set; }
        public DescriptorPool descriptorPool { get; protected set; }
        public List<DescriptorSetLayout> descriptorSetLayoutList { get; protected set; }
        public DescriptorSet descriptorSet { get; protected set; }
        public Pipeline pipeline { get; protected set; }
        public PipelineLayout pipelineLayout { get; protected set; }
        public PipelineCache pipelineCache { get; protected set; }
        
        public JsonPipeline()
        {
          
        }

        public JsonPipeline(String jsonPipelineFilePath, RenderPass renderPass, uint ConstBufferSize)
        {
            _device = VulkanRenderer.device;

            string jsonContent = File.ReadAllText(jsonPipelineFilePath);
            RenderPipelineModel model = JsonConvert.DeserializeObject<RenderPipelineModel>(jsonContent);

            LoadPipeline(model, renderPass, ConstBufferSize);
            LoadDescriptorSets(model);
        }

        private void LoadDescriptorSets(RenderPipelineModel model)
        {
            var meshProperties = MemoryManager.GetGameObjectPropertiesBuffer();
            var textures = MemoryManager.GetTexturePropertiesBuffer();

            //CreateDescriptorPool
            {
                List<DescriptorPoolSize> descriptorPoolSizeList = new List<DescriptorPoolSize>();
                foreach (var binding in model.PipelineDescriptorModelsList)
                {
                    switch (binding.BindingPropertiesList)
                    {
                        case DescriptorBindingPropertiesEnum.kMeshPropertiesDescriptor: descriptorPoolSizeList.Add(new DescriptorPoolSize() { Type = binding.descriptorType, DescriptorCount = meshProperties.UCount() }); break;
                        case DescriptorBindingPropertiesEnum.kTextureDescriptor: descriptorPoolSizeList.Add(new DescriptorPoolSize() { Type = binding.descriptorType, DescriptorCount = textures.UCount() }); break;
                        default:
                            {
                                throw new Exception($"{binding} case hasn't been handled yet");
                            }
                    }
                }

                fixed (DescriptorPoolSize* descriptorPoolSize = descriptorPoolSizeList.ToArray())
                {
                    DescriptorPoolCreateInfo poolCreateInfo = new DescriptorPoolCreateInfo()
                    {
                        SType = StructureType.DescriptorPoolCreateInfo,
                        MaxSets = 500,
                        PPoolSizes = descriptorPoolSize,
                        PoolSizeCount = descriptorPoolSizeList.UCount(),
                        Flags = 0,
                        PNext = null
                    };
                    vk.CreateDescriptorPool(_device, in poolCreateInfo, null, out DescriptorPool descriptorPoolPtr);
                    descriptorPool = descriptorPoolPtr;
                }
            }

            //CreateDescriptorSetLayout
            {
                List<DescriptorSetLayoutBinding> descriptorSetLayoutBindingList = new List<DescriptorSetLayoutBinding>();
                foreach (var binding in model.PipelineDescriptorModelsList)
                {
                    switch (binding.BindingPropertiesList)
                    {
                        case DescriptorBindingPropertiesEnum.kMeshPropertiesDescriptor:
                            {
                                descriptorSetLayoutBindingList.Add(new DescriptorSetLayoutBinding()
                                {
                                    Binding = binding.BindingNumber,
                                    DescriptorCount = meshProperties.UCount(),
                                    DescriptorType = binding.descriptorType,
                                    PImmutableSamplers = null,
                                    StageFlags = ShaderStageFlags.All
                                });
                                break;
                            }
                        case DescriptorBindingPropertiesEnum.kTextureDescriptor:
                            {
                                descriptorSetLayoutBindingList.Add(new DescriptorSetLayoutBinding()
                                {
                                    Binding = binding.BindingNumber,
                                    DescriptorCount = textures.UCount(),
                                    DescriptorType = binding.descriptorType,
                                    PImmutableSamplers = null,
                                    StageFlags = ShaderStageFlags.All
                                });
                                break;
                            }
                        default:
                            {
                                throw new Exception($"{binding} case hasn't been handled yet");
                            }
                    }
                }

                fixed (DescriptorSetLayoutBinding* descriptorSetLayouts = descriptorSetLayoutBindingList.ToArray())
                {
                    DescriptorSetLayoutCreateInfo descriptorSetLayoutCreateInfo = new DescriptorSetLayoutCreateInfo()
                    {
                        SType = StructureType.DescriptorSetLayoutCreateInfo,
                        BindingCount = descriptorSetLayoutBindingList.UCount(),
                        PBindings = descriptorSetLayouts,
                        Flags = 0,
                        PNext = null
                    };
                    vk.CreateDescriptorSetLayout(_device, &descriptorSetLayoutCreateInfo, null, out DescriptorSetLayout descriptorsetLayoutPtr);
                    descriptorSetLayoutList.Add(descriptorsetLayoutPtr);
                }
            }

            //AllocateDescriptorSets
            {
                DescriptorSetLayout* layouts = stackalloc DescriptorSetLayout[VulkanRenderer.MAX_FRAMES_IN_FLIGHT];

                for (int x = 0; x < VulkanRenderer.MAX_FRAMES_IN_FLIGHT; x++)
                {
                    layouts[x] = descriptorSetLayoutList[0];
                }

                DescriptorSetAllocateInfo allocInfo = new
                (
                    descriptorPool: descriptorPool,
                    descriptorSetCount: VulkanRenderer.MAX_FRAMES_IN_FLIGHT,
                    pSetLayouts: layouts
                );
                vk.AllocateDescriptorSets(VulkanRenderer.device, &allocInfo, out DescriptorSet descriptorSetPtr);
                descriptorSet = descriptorSetPtr;
            }

            //UpdateDescriptorSets
            {
                List<WriteDescriptorSet> descriptorSetList = new List<WriteDescriptorSet>();
                for (uint x = 0; x < VulkanRenderer.swapChain.ImageCount; x++)
                {
                    foreach (var binding in model.PipelineDescriptorModelsList)
                    {
                        switch (binding.BindingPropertiesList)
                        {
                            case DescriptorBindingPropertiesEnum.kMeshPropertiesDescriptor:
                                {
                                    fixed (DescriptorBufferInfo* meshInfo = meshProperties.ToArray())
                                    {
                                        descriptorSetList.Add(new WriteDescriptorSet()
                                        {
                                            SType = StructureType.WriteDescriptorSet,
                                            DescriptorCount = meshProperties.UCount(),
                                            DescriptorType = binding.descriptorType,
                                            DstBinding = binding.BindingNumber,
                                            DstArrayElement = 0,
                                            DstSet = descriptorSet,
                                            PBufferInfo = meshInfo,
                                            PImageInfo = null,
                                            PTexelBufferView = null,
                                            PNext = null
                                        });
                                    }
                                    break;
                                }
                            case DescriptorBindingPropertiesEnum.kTextureDescriptor:
                                {
                                    fixed (DescriptorImageInfo* texturesPtr = textures.ToArray())
                                    {
                                        descriptorSetList.Add(new WriteDescriptorSet()
                                        {
                                            SType = StructureType.WriteDescriptorSet,
                                            DescriptorCount = meshProperties.UCount(),
                                            DescriptorType = binding.descriptorType,
                                            DstBinding = binding.BindingNumber,
                                            DstArrayElement = 0,
                                            DstSet = descriptorSet,
                                            PBufferInfo = null,
                                            PImageInfo = texturesPtr,
                                            PTexelBufferView = null,
                                            PNext = null
                                        });
                                    }
                                    break;
                                }
                            default:
                                {
                                    throw new Exception($"{binding} case hasn't been handled yet");
                                }
                        }
                    }
                    fixed (WriteDescriptorSet* writeDescriptorSet = descriptorSetList.ToArray())
                    {
                        vk.UpdateDescriptorSets(_device, descriptorSetList.UCount(), writeDescriptorSet, 0, null);
                    }
                }
            }
        }

        private void LoadPipeline(RenderPipelineModel model, RenderPass renderPass, uint ConstBufferSize)
        {
            List<PushConstantRange> pushConstantRangeList = new List<PushConstantRange>();
            fixed (DescriptorSetLayout* descriptorSet = descriptorSetLayoutList.ToArray())
            {
                pushConstantRangeList = new List<PushConstantRange>()
                {
                    new PushConstantRange()
                    {
                        StageFlags = ShaderStageFlags.VertexBit | ShaderStageFlags.FragmentBit,
                        Offset = 0,
                        Size = ConstBufferSize
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
                Topology = model.PipelineInputAssemblyStateCreateInfo.Topology,
                PrimitiveRestartEnable = model.PipelineInputAssemblyStateCreateInfo.PrimitiveRestartEnable,
                Flags = 0,
                PNext = null,
            };

            PipelineViewportStateCreateInfo pipelineViewportStateCreateInfo = new PipelineViewportStateCreateInfo();
            fixed (Viewport* viewportPtr = model.ViewportList.ToArray())
            fixed (Rect2D* scissorPtr = model.ScissorList.ToArray())
            {
                pipelineViewportStateCreateInfo = new PipelineViewportStateCreateInfo
                {
                    SType = StructureType.PipelineViewportStateCreateInfo,
                    ViewportCount = model.ViewportList.UCount(),
                    PViewports = viewportPtr,
                    ScissorCount = model.ScissorList.UCount(),
                    PScissors = scissorPtr,
                    Flags = 0,
                    PNext = null
                };
            }

            PipelineColorBlendStateCreateInfo pipelineColorBlendStateCreateInfo = new PipelineColorBlendStateCreateInfo();
            fixed (PipelineColorBlendAttachmentState* attachments = model.PipelineColorBlendAttachmentStateList.ToArray())
            {
                pipelineColorBlendStateCreateInfo = new PipelineColorBlendStateCreateInfo()
                {
                    SType = StructureType.PipelineColorBlendStateCreateInfo,
                    LogicOpEnable = model.PipelineColorBlendStateCreateInfoModel.LogicOpEnable,
                    LogicOp = model.PipelineColorBlendStateCreateInfoModel.LogicOp,
                    AttachmentCount = model.PipelineColorBlendAttachmentStateList.UCount(),
                    PAttachments = attachments,
                    Flags = 0,
                    PNext = null
                };
                pipelineColorBlendStateCreateInfo.BlendConstants[0] = model.PipelineColorBlendStateCreateInfoModel.BlendConstants[0];
                pipelineColorBlendStateCreateInfo.BlendConstants[1] = model.PipelineColorBlendStateCreateInfoModel.BlendConstants[1];
                pipelineColorBlendStateCreateInfo.BlendConstants[2] = model.PipelineColorBlendStateCreateInfoModel.BlendConstants[2];
                pipelineColorBlendStateCreateInfo.BlendConstants[3] = model.PipelineColorBlendStateCreateInfoModel.BlendConstants[3];
            }

            List<DynamicState> dynamicStateList = new List<DynamicState>()
            {
                DynamicState.Viewport,
                DynamicState.Scissor
            };

            PipelineDynamicStateCreateInfo pipelineDynamicStateCreateInfo = new PipelineDynamicStateCreateInfo();
            fixed (DynamicState* dynamicState = dynamicStateList.ToArray())
                pipelineDynamicStateCreateInfo = new PipelineDynamicStateCreateInfo()
                {
                    SType = StructureType.PipelineDynamicStateCreateInfo,
                    DynamicStateCount = dynamicStateList.UCount(),
                    PDynamicStates = dynamicState,
                    Flags = 0,
                    PNext = null
                };

            List<PipelineShaderStageCreateInfo> pipelineShaderStageCreateInfoList = new List<PipelineShaderStageCreateInfo>()
            {
                VulkanRenderer.CreateShader(model.VertexShader,  ShaderStageFlags.VertexBit),
                VulkanRenderer.CreateShader(model.FragmentShader, ShaderStageFlags.FragmentBit)
            };

            GraphicsPipelineCreateInfo graphicsPipelineCreateInfo = new GraphicsPipelineCreateInfo();
            PipelineRasterizationStateCreateInfo pipelineRasterizationStateCreateInfoRef = model.PipelineRasterizationStateCreateInfo;
            PipelineMultisampleStateCreateInfo pipelineMultisampleStateCreateInfo = model.PipelineMultisampleStateCreateInfo;
            PipelineDepthStencilStateCreateInfo pipelineDepthStencilStateCreateInfo = model.PipelineDepthStencilStateCreateInfo;
            fixed (PipelineShaderStageCreateInfo* pipelineShaderStageCreateInfo = pipelineShaderStageCreateInfoList.ToArray())
            {
                graphicsPipelineCreateInfo = new GraphicsPipelineCreateInfo()
                {
                    SType = StructureType.GraphicsPipelineCreateInfo,
                    PStages = pipelineShaderStageCreateInfo,
                    PVertexInputState = &vertexInputInfo,
                    PInputAssemblyState = &pipelineInputAssemblyStateCreateInfo,
                    PViewportState = &pipelineViewportStateCreateInfo,
                    PRasterizationState = &pipelineRasterizationStateCreateInfoRef,
                    PMultisampleState = &pipelineMultisampleStateCreateInfo,
                    PDepthStencilState = &pipelineDepthStencilStateCreateInfo,
                    PColorBlendState = &pipelineColorBlendStateCreateInfo,
                    PDynamicState = &pipelineDynamicStateCreateInfo,
                    PTessellationState = null,
                    Layout = pipelineLayout,
                    RenderPass = renderPass,
                    StageCount = pipelineShaderStageCreateInfoList.UCount(),
                    Subpass = 0,
                    // BasePipelineHandle = ,
                    BasePipelineIndex = 0,
                    Flags = 0,
                    PNext = null
                };
            }

            vk.CreateGraphicsPipelines(VulkanRenderer.device, pipelineCache, 1, &graphicsPipelineCreateInfo, null, out Pipeline tempPipelinePtr);
            pipeline = tempPipelinePtr;
        }

        public void SavePipeline(String jsonPipeline)
        {

        }
    }
}
