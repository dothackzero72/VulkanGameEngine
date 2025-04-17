using GlmSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public enum DescriptorBindingPropertiesEnum : UInt32
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

    public struct GPUImport<T>
    {
        public List<Mesh<T>> MeshList { get; set; }
        public List<Texture> TextureList { get; set; }
        public List<Material> MaterialList { get; set; }

        public GPUImport(List<Mesh<T>> meshList, List<Texture> textureList, List<Material> materialList)
        {
            MeshList = meshList;
            TextureList = textureList;
            MaterialList = materialList;
        }
    };

    public unsafe class JsonPipeline<T>
    {
        private VkDevice _device { get; set; } = VulkanRenderer.device;
        public GPUImport<T> _GPUImport { get; protected set; } 
        public VkDescriptorPool descriptorPool { get; protected set; }
        public ListPtr<VkDescriptorSetLayout> descriptorSetLayoutList { get; protected set; } = new ListPtr<VkDescriptorSetLayout>();
        public ListPtr<VkDescriptorSet> descriptorSetList { get; protected set; } = new ListPtr<VkDescriptorSet>();
        public VkPipeline pipeline { get; protected set; }
        public VkPipelineLayout pipelineLayout { get; protected set; }
        public VkPipelineCache pipelineCache { get; protected set; }

        public JsonPipeline()
        {

        }

        public JsonPipeline(String jsonPipelineFilePath, VkRenderPass renderPass, uint ConstBufferSize, GPUImport<T> gpuImport, ivec2 renderPassResolution)
        {
            _GPUImport = gpuImport;

            string jsonContent = File.ReadAllText(jsonPipelineFilePath);
            RenderPipelineDLL model = JsonConvert.DeserializeObject<RenderPipelineModel>(jsonContent).ToDLL();

            var meshProperties = GetMeshPropertiesBuffer<T>(gpuImport.MeshList);
            var textureProperties = GetTexturePropertiesBuffer(gpuImport.TextureList);
            var materialProperties = GetMaterialPropertiesBuffer(gpuImport.MaterialList);
            var vertexProperties = new ListPtr<VkDescriptorBufferInfo>();
            var indexProperties = new ListPtr<VkDescriptorBufferInfo>();
            var transformProperties = new ListPtr<VkDescriptorBufferInfo>();

            GPUIncludes includes = new GPUIncludes
            {
                vertexProperties = vertexProperties.Ptr,
                indexProperties = indexProperties.Ptr,
                transformProperties = transformProperties.Ptr,
                meshProperties = meshProperties.Ptr,
                texturePropertiesList = textureProperties.Ptr,
                materialProperties = materialProperties.Ptr,
                vertexPropertiesCount = vertexProperties.UCount,
                indexPropertiesCount = indexProperties.UCount,
                transformPropertiesCount = transformProperties.UCount,
                meshPropertiesCount = meshProperties.UCount,
                texturePropertiesListCount = textureProperties.UCount,
                materialPropertiesCount = materialProperties.UCount
            };

            descriptorPool = GameEngineImport.DLL_Pipeline_CreateDescriptorPool(VulkanRenderer.device, model, &includes);
            descriptorSetLayoutList = new ListPtr<VkDescriptorSetLayout>(GameEngineImport.DLL_Pipeline_CreateDescriptorSetLayout(VulkanRenderer.device, model, includes), model.DescriptorSetLayoutCount);
            descriptorSetList = new ListPtr<VkDescriptorSet>(GameEngineImport.DLL_Pipeline_AllocateDescriptorSets(VulkanRenderer.device, descriptorPool, model, descriptorSetLayoutList.Ptr), model.DescriptorSetCount);
            GameEngineImport.DLL_Pipeline_UpdateDescriptorSets(_device, model, includes, descriptorSetList.Ptr);
            pipelineLayout = GameEngineImport.DLL_Pipeline_CreatePipelineLayout(_device, model, ConstBufferSize, descriptorSetLayoutList.Ptr);
            pipeline = GameEngineImport.DLL_Pipeline_CreatePipeline(_device, renderPass, pipelineLayout, pipelineCache, model, renderPassResolution);
        }

        public VkCommandBuffer Draw(VkCommandBuffer[] commandBufferList, VkRenderPass renderPass, VkFramebuffer[] frameBufferList, ivec2 renderPassResolution, List<GameObject> gameObjectList, SceneDataBuffer sceneDataBuffer)
        {
            var commandIndex = (int)VulkanRenderer.CommandIndex;
            var imageIndex = (int)VulkanRenderer.ImageIndex;
            var commandBuffer = commandBufferList[commandIndex];

            List<VkClearValue> clearValueList = new List<VkClearValue>()
            {
                new VkClearValue
                {
                    Color = new VkClearColorValue(0, 0, 0, 1),
                },
                new VkClearValue
                {
                    DepthStencil = new VkClearDepthStencilValue(1.0f, 0)
                }
            };

            VkRenderPassBeginInfo renderPassInfo = new VkRenderPassBeginInfo();
            fixed (VkClearValue* clearValuePtr = clearValueList.ToArray())
            {
                renderPassInfo = new VkRenderPassBeginInfo()
                {
                    renderPass = renderPass,
                    framebuffer = frameBufferList[imageIndex],
                    clearValueCount = clearValueList.UCount(),
                    pClearValues = clearValuePtr,
                    renderArea = new VkRect2D
                    {
                        offset = new VkOffset2D(0, 0),
                        extent = new VkExtent2D()
                        {
                            width = (uint)renderPassResolution.x,
                            height = (uint)renderPassResolution.y
                        }
                    }
                };
            }

            var commandInfo = new VkCommandBufferBeginInfo
            {
                flags = 0
            };

            VkFunc.vkBeginCommandBuffer(commandBuffer, &commandInfo);
            VkFunc.vkCmdBeginRenderPass(commandBuffer, &renderPassInfo, VkSubpassContents.VK_SUBPASS_CONTENTS_INLINE);
            VkFunc.vkCmdBindPipeline(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, pipeline);
            foreach (var obj in gameObjectList)
            {
          //      obj.Draw(commandBuffer, pipeline, pipelineLayout, descriptorSetList, sceneDataBuffer);
            }
            VkFunc.vkCmdEndRenderPass(commandBuffer);
            VkFunc.vkEndCommandBuffer(commandBuffer);

            return commandBuffer;
        }

        public static ListPtr<VkDescriptorBufferInfo> GetVertexPropertiesBuffer<T>(List<Mesh<T>> meshList)
        {
            ListPtr<VkDescriptorBufferInfo> vertexPropertiesBuffer = new ListPtr<VkDescriptorBufferInfo>();
            if (meshList.Count() == 0)
            {
                vertexPropertiesBuffer.Add(new VkDescriptorBufferInfo
                {
                    buffer = VulkanConst.VK_NULL_HANDLE,
                    offset = 0,
                    range = VulkanConst.VK_WHOLE_SIZE
                });
            }
            else
            {
                foreach (var mesh in meshList)
                {
                    // mesh->GetVertexBuffer(vertexPropertiesBuffer);
                }
            }

            return vertexPropertiesBuffer;
        }

        public static ListPtr<VkDescriptorBufferInfo> GetIndexPropertiesBuffer<T>(List<Mesh<T>> meshList)
        {
            ListPtr<VkDescriptorBufferInfo> indexPropertiesBuffer = new ListPtr<VkDescriptorBufferInfo>();
            if (meshList.Count() == 0)
            {
                indexPropertiesBuffer.Add(new VkDescriptorBufferInfo
                {
                    buffer = VulkanConst.VK_NULL_HANDLE,
                    offset = 0,
                    range = VulkanConst.VK_WHOLE_SIZE
                });
            }
            else
            {
                foreach (var mesh in meshList)
                {
                    //   mesh->GetIndexBuffer(indexPropertiesBuffer);
                }
            }
            return indexPropertiesBuffer;
        }

        public static ListPtr<VkDescriptorBufferInfo> GetGameObjectTransformBuffer<T>(List<Mesh<T>> meshList)
        {
            ListPtr<VkDescriptorBufferInfo> transformPropertiesBuffer = new ListPtr<VkDescriptorBufferInfo>();
            if (meshList.Count() == 0)
            {
                transformPropertiesBuffer.Add(new VkDescriptorBufferInfo
                {
                    buffer = VulkanConst.VK_NULL_HANDLE,
                    offset = 0,
                    range = VulkanConst.VK_WHOLE_SIZE
                });
            }
            else
            {
                foreach (var mesh in meshList)
                {
                    mesh.GetTransformBuffer();
                }
            }

            return transformPropertiesBuffer;
        }

        public static ListPtr<VkDescriptorBufferInfo> GetMeshPropertiesBuffer<T>(List<Mesh<T>> meshList)
        {
            ListPtr<VkDescriptorBufferInfo> meshPropertiesBuffer = new ListPtr<VkDescriptorBufferInfo>();
            if (meshList.Count() == 0)
            {
                meshPropertiesBuffer.Add(new VkDescriptorBufferInfo
                {
                    buffer = VulkanConst.VK_NULL_HANDLE,
                    offset = 0,
                    range = VulkanConst.VK_WHOLE_SIZE
                });
            }
            else
            {
                foreach (var mesh in meshList)
                {
                    meshPropertiesBuffer.Add(mesh.GetMeshPropertiesBuffer());
                }
            }

            return meshPropertiesBuffer;
        }

        public static ListPtr<VkDescriptorImageInfo> GetTexturePropertiesBuffer(List<Texture> textureList)
        {
            ListPtr<VkDescriptorImageInfo> texturePropertiesBuffer = new ListPtr<VkDescriptorImageInfo>();
            if (textureList.Count() == 0)
            {
                VkSamplerCreateInfo NullSamplerInfo = new VkSamplerCreateInfo
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO,
                    magFilter = VkFilter.VK_FILTER_NEAREST,
                    minFilter = VkFilter.VK_FILTER_NEAREST,
                    mipmapMode = VkSamplerMipmapMode.VK_SAMPLER_MIPMAP_MODE_LINEAR,
                    addressModeU = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_REPEAT,
                    addressModeV = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_REPEAT,
                    addressModeW = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_REPEAT,
                    mipLodBias = 0,
                    anisotropyEnable = true,
                    maxAnisotropy = 16.0f,
                    compareEnable = false,
                    compareOp = VkCompareOp.VK_COMPARE_OP_ALWAYS,
                    minLod = 0,
                    maxLod = 0,
                    borderColor = VkBorderColor.VK_BORDER_COLOR_INT_OPAQUE_BLACK,
                    unnormalizedCoordinates = false,
                };
                VkFunc.vkCreateSampler(VulkanRenderer.device, &NullSamplerInfo, null, out VkSampler nullSampler);

                VkDescriptorImageInfo nullBuffer = new VkDescriptorImageInfo
                {
                    sampler = nullSampler,
                    imageView = VulkanConst.VK_NULL_HANDLE,
                    imageLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED,
                };
                texturePropertiesBuffer.Add(nullBuffer);
            }
            else
            {
                foreach (var texture in textureList)
                {
                    texturePropertiesBuffer.Add(texture.GetTexturePropertiesBuffer());
                }
            }

            return texturePropertiesBuffer;
        }

        public static ListPtr<VkDescriptorBufferInfo> GetMaterialPropertiesBuffer(List<Material> materialList)
        {
            ListPtr<VkDescriptorBufferInfo> materialPropertiesBuffer = new ListPtr<VkDescriptorBufferInfo>();
            if (materialList.Count() == 0)
            {
                materialPropertiesBuffer.Add(new VkDescriptorBufferInfo
                {
                    buffer = VulkanConst.VK_NULL_HANDLE,
                    offset = 0,
                    range = VulkanConst.VK_WHOLE_SIZE
                });
            }
            else
            {
                foreach (var material in materialList)
                {
                    materialPropertiesBuffer.Add(material.GetMaterialPropertiesBuffer());
                }
            }
            return materialPropertiesBuffer;
        }
    }
}
