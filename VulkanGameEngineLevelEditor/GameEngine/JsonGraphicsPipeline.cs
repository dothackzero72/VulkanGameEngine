using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class JsonGraphicsPipeline
    {
        private Vk vk = Vk.GetApi();
        public DescriptorPool descriptorpool { get; protected set; }
        public DescriptorSetLayout descriptorSetLayout { get; protected set; }
        public DescriptorSet descriptorset { get; protected set; }
        public Pipeline shaderpipeline { get; protected set; }
        public PipelineLayout shaderpipelineLayout { get; protected set; }
        public PipelineCache pipelineCache { get; protected set; }

        public JsonGraphicsPipeline()
        {

        }

        public void CreateGraphicsPipeline(PipelineLayoutModel model)
        {
            CreateDescriptorSetLayout(model);
        }

        private void CreateDescriptorSetLayout(PipelineLayoutModel model)
        {
            fixed (DescriptorSetLayoutBinding* ptr = model.LayoutBindingList.ToArray())
            {
                DescriptorSetLayoutCreateInfo layoutInfo = new DescriptorSetLayoutCreateInfo()
                {
                    SType = StructureType.DescriptorSetLayoutCreateInfo,
                    BindingCount = (uint)model.LayoutBindingList.Count,
                    PBindings = ptr,
                };
                vk.CreateDescriptorSetLayout(VulkanRenderer.device, &layoutInfo, null, out DescriptorSetLayout descriptorsetLayout);
                descriptorSetLayout = descriptorsetLayout;
            }
        }

        private void CreateDescriptorSets()
        {
            DescriptorSetLayout* layouts = stackalloc DescriptorSetLayout[VulkanRenderer.MAX_FRAMES_IN_FLIGHT];

            for (int x = 0; x < VulkanRenderer.MAX_FRAMES_IN_FLIGHT; x++)
            {
                layouts[x] = descriptorSetLayout;
            }

            DescriptorSetAllocateInfo allocInfo = new
            (
                descriptorPool: descriptorpool,
                descriptorSetCount: VulkanRenderer.MAX_FRAMES_IN_FLIGHT,
                pSetLayouts: layouts
            );
            vk.AllocateDescriptorSets(VulkanRenderer.device, &allocInfo, out DescriptorSet descriptorSet);
            descriptorset = descriptorSet;
        }

        private void CreateDescriptorPool(PipelineLayoutModel model)
        {
            for (int x = 0; x < model.BindingList.Count(); x++)
            {
                switch (model.BindingList[x])
                {
                    case DescriptorBindingPropertiesEnum.kMeshPropertiesDescriptor:
                        {
                           // model.descriptorPoolSizeList.Add(new DescriptorPoolSize { model.DescriptorList[x], (uint32_t)GLTFSceneManager::GetGameObjectPropertiesBuffer().size() });
                            break;
                        }
                    case DescriptorBindingPropertiesEnum.kModelTransformDescriptor:
                        {
                           // model.descriptorPoolSizeList.Add(new DescriptorPoolSize { model.DescriptorList[x], (uint32_t)GLTFSceneManager::GetGameObjectTransformBuffer().size() });
                            break;
                        }
                    case DescriptorBindingPropertiesEnum.kMaterialDescriptor:
                        {
                         //   model.descriptorPoolSizeList.Add(new DescriptorPoolSize { model.DescriptorList[x], (uint32_t)GLTFSceneManager::GetMaterialPropertiesBuffer().size() });
                            break;
                        }
                    case DescriptorBindingPropertiesEnum.kTextureDescriptor:
                        {
                           // model.descriptorPoolSizeList.Add(new DescriptorPoolSize { model.DescriptorList[x], (uint32_t)GLTFSceneManager::GetTexturePropertiesBuffer().size() });
                            break;
                        }
                    case DescriptorBindingPropertiesEnum.kBRDFMapDescriptor:
                        {
                            model.DescriptorPoolList.Add(new DescriptorPoolSize()
                            {
                                Type = model.DescriptorList[x],
                                DescriptorCount = 1
                            });
                            break;
                        }
                    case DescriptorBindingPropertiesEnum.kIrradianceMapDescriptor:
                        {
                            model.DescriptorPoolList.Add(new DescriptorPoolSize()
                            {
                                Type = model.DescriptorList[x],
                                DescriptorCount = 1
                            });
                            break;
                        }
                    case DescriptorBindingPropertiesEnum.kPrefilterMapDescriptor:
                        {
                            model.DescriptorPoolList.Add(new DescriptorPoolSize()
                            {
                                Type = model.DescriptorList[x],
                                DescriptorCount = 1
                            });
                            break;
                        }
                    case DescriptorBindingPropertiesEnum.kCubeMapDescriptor:
                        {
                            model.DescriptorPoolList.Add(new DescriptorPoolSize()
                            {
                                Type = model.DescriptorList[x],
                                DescriptorCount = 1
                            });
                            break;
                        }
                    case DescriptorBindingPropertiesEnum.kEnvironmentDescriptor:
                        {
                            model.DescriptorPoolList.Add(new DescriptorPoolSize()
                            {
                                Type = model.DescriptorList[x],
                                DescriptorCount = 1
                            });
                            break;
                        }
                    case DescriptorBindingPropertiesEnum.kSunLightDescriptor:
                        {
                           // model.descriptorPoolSizeList.Add(new DescriptorPoolSize { model.DescriptorList[x], (uint32_t)GLTFSceneManager::GetSunLightPropertiesBuffer().size() });
                            break;
                        }
                    case DescriptorBindingPropertiesEnum.kDirectionalLightDescriptor:
                        {
                          //  model.descriptorPoolSizeList.Add(new DescriptorPoolSize { model.DescriptorList[x], (uint32_t)GLTFSceneManager::GetDirectionalLightPropertiesBuffer().size() });
                            break;
                        }
                    case DescriptorBindingPropertiesEnum.kPointLightDescriptor:
                        {
                          //  model.descriptorPoolSizeList.Add(new DescriptorPoolSize { model.DescriptorList[x], (uint32_t)GLTFSceneManager::GetPointLightPropertiesBuffer().size() });
                            break;
                        }
                    case DescriptorBindingPropertiesEnum.kSpotLightDescriptor:
                        {
                           // model.descriptorPoolSizeList.Add(new DescriptorPoolSize { model.DescriptorList[x], (uint32_t)GLTFSceneManager::GetSpotLightPropertiesBuffer().size() });
                            break;
                        }
                    case DescriptorBindingPropertiesEnum.kReflectionViewDescriptor:
                        {
                            model.DescriptorPoolList.Add(new DescriptorPoolSize()
                            {
                                Type = model.DescriptorList[x],
                                DescriptorCount = 1
                            });
                            break;
                        }
                    case DescriptorBindingPropertiesEnum.kDirectionalShadowDescriptor:
                        {
                            model.DescriptorPoolList.Add(new DescriptorPoolSize()
                            {
                                Type = model.DescriptorList[x],
                                DescriptorCount = 1
                            });
                            break;
                        }
                    case DescriptorBindingPropertiesEnum.kPointShadowDescriptor:
                        {
                            //model.descriptorPoolSizeList.Add(new DescriptorPoolSize { model.DescriptorList[x], (uint32_t)GLTFSceneManager::GetPointLightPropertiesBuffer().size() });
                            break;
                        }
                    case DescriptorBindingPropertiesEnum.kSpotShadowDescriptor: 
                        {
                            model.DescriptorPoolList.Add(new DescriptorPoolSize()
                            {
                                Type = model.DescriptorList[x],
                                DescriptorCount = 1
                            });
                            break;
                        }
                    case DescriptorBindingPropertiesEnum.kViewTextureDescriptor:
                        {
                            model.DescriptorPoolList.Add(new DescriptorPoolSize()
                            {
                                Type = model.DescriptorList[x],
                                DescriptorCount = 1
                            });
                            break;
                        }
                    case DescriptorBindingPropertiesEnum.kViewDepthTextureDescriptor: 
                        {
                            model.DescriptorPoolList.Add(new DescriptorPoolSize()
                            {
                                Type = model.DescriptorList[x],
                                DescriptorCount = 1
                            });
                            break;
                        }
                    case DescriptorBindingPropertiesEnum.kCubeMapSamplerDescriptor:
                        {
                           // model.descriptorPoolSizeList.Add(new DescriptorPoolSize { model.DescriptorList[x], (uint32_t)GLTFSceneManager::GetCubeMapSamplerBuffer().size() }); 
                            break;
                        }
                    case DescriptorBindingPropertiesEnum.kMathOpperation1Descriptor:
                        {

                            model.DescriptorPoolList.Add(new DescriptorPoolSize()
                            {
                                Type = model.DescriptorList[x],
                                DescriptorCount = 1
                            });
                            break;
                        };
                    case DescriptorBindingPropertiesEnum.kMathOpperation2Descriptor:
                        {

                            model.DescriptorPoolList.Add(new DescriptorPoolSize()
                            {
                                Type = model.DescriptorList[x],
                                DescriptorCount = 1
                            });
                            break;
                        }
                    default: throw new NotImplementedException();
                }
            }

            fixed (DescriptorPoolSize* poolSizes = model.DescriptorPoolList.ToArray())
            {
                DescriptorPoolCreateInfo poolInfo = new DescriptorPoolCreateInfo()
                {
                    SType = StructureType.DescriptorPoolCreateInfo,
                    PoolSizeCount = model.DescriptorPoolList.UCount(),
                    PPoolSizes = poolSizes,
                    MaxSets = Vk.MaxDescriptionSize
                };

                var descriptorPool = descriptorpool;
                var allocation = new AllocationCallbacks();
                vk.CreateDescriptorPool(VulkanRenderer.device, &poolInfo, &allocation, &descriptorPool);
            }
        }

        private void UpdateDescriptorSets()
        {

        }
    }
}

