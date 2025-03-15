using GlmSharp;
using Newtonsoft.Json;
using Silk.NET.SDL;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineGameObjectScripts.Component;
using VulkanGameEngineGameObjectScripts.Vulkan;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.RenderPassEditor;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class FrameBufferRenderPass
    {
        Vk vk = Vk.GetApi();

        ivec2 RenderPassResolution;
        SampleCountFlags SampleCount;

        VkRenderPass renderPass;
        VkCommandBuffer[] CommandBufferList;
        VkFramebuffer[] FrameBufferList;

        VkDescriptorPool descriptorPool;
        List<VkDescriptorSetLayout> descriptorSetLayoutList = new List<VkDescriptorSetLayout>();
        List<VkDescriptorSet> descriptorSetLists = new List<VkDescriptorSet>();
        VkPipeline pipeline;
        VkPipelineLayout pipelineLayout;
        VkPipelineCache pipelineCache;

        public FrameBufferRenderPass()
        {

        }

        public void BuildRenderPass(Texture texture)
        {
        //    SaveRenderPass();
          //  SavePipeline();

            RenderPassResolution = new ivec2((int)VulkanRenderer.SwapChain.SwapChainResolution.width, (int)VulkanRenderer.SwapChain.SwapChainResolution.height);
            SampleCount = SampleCountFlags.Count1Bit;

            CommandBufferList = new VkCommandBuffer[(int)VulkanRenderer.SwapChain.ImageCount];
            FrameBufferList = new VkFramebuffer[(int)VulkanRenderer.SwapChain.ImageCount];

            renderPass = CreateRenderPass();
            FrameBufferList = CreateFramebuffer();
            BuildRenderPipeline(texture);
            VulkanRenderer.CreateCommandBuffers(CommandBufferList);
        }

        public VkRenderPass CreateRenderPass()
        {
            VkRenderPass tempRenderPass;
            List<VkAttachmentDescription> attachmentDescriptionList = new List<VkAttachmentDescription>()
            {
                new VkAttachmentDescription
                {
                    format = VkFormat.VK_FORMAT_R8G8B8A8_UNORM,
                    samples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT,
                    loadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_CLEAR,
                    storeOp = VkAttachmentStoreOp.VK_ATTACHMENT_STORE_OP_STORE,
                    stencilLoadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_DONT_CARE,
                    stencilStoreOp = VkAttachmentStoreOp.VK_ATTACHMENT_STORE_OP_DONT_CARE,
                    initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED,
                    finalLayout = VkImageLayout.VK_IMAGE_LAYOUT_PRESENT_SRC_KHR,
                }
            };

            VkImageLayout initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED;
            VkImageLayout finalLayout = VkImageLayout.VK_IMAGE_LAYOUT_PRESENT_SRC_KHR;
            uint sdf = (uint)VkImageLayout.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL;
            int wer = (int)VkImageLayout.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL;


            List<VkAttachmentReference> colorRefsList = new List<VkAttachmentReference>()
            {
                new VkAttachmentReference
                {
                    attachment = 0,
                    layout = VkImageLayout.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL
                }
            };

            List<VkAttachmentReference> multiSampleReferenceList = new List<VkAttachmentReference>();
            List<VkAttachmentReference> depthReference = new List<VkAttachmentReference>();

            List<VkSubpassDescription> subpassDescriptionList = new List<VkSubpassDescription>();
            fixed (VkAttachmentReference* colorRefs = colorRefsList.ToArray())
            {
                subpassDescriptionList.Add(
                    new VkSubpassDescription
                    {
                        pipelineBindPoint = VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS,
                        colorAttachmentCount = (uint)colorRefsList.Count,
                        pColorAttachments = colorRefs,
                        pResolveAttachments = null,
                        pDepthStencilAttachment = null
                    });
            }

            List<VkSubpassDependency> subpassDependencyList = new List<VkSubpassDependency>()
            {
                new VkSubpassDependency
                {
                    srcSubpass = uint.MaxValue,
                    dstSubpass = 0,
                    srcStageMask = VkPipelineStageFlagBits.COLOR_ATTACHMENT_OUTPUT_BIT,
                    dstStageMask = VkPipelineStageFlagBits.COLOR_ATTACHMENT_OUTPUT_BIT,
                    srcAccessMask = 0,
                    dstAccessMask = VkAccessFlagBits.VK_ACCESS_COLOR_ATTACHMENT_WRITE_BIT,
                }
            };

            fixed (VkAttachmentDescription* attachments = attachmentDescriptionList.ToArray())
            fixed (VkSubpassDescription* description = subpassDescriptionList.ToArray())
            fixed (VkSubpassDependency* dependency = subpassDependencyList.ToArray())
            {
                var renderPassCreateInfo = new VkRenderPassCreateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_RENDER_PASS_CREATE_INFO,
                    pNext = null,
                    flags = 0,
                    attachmentCount = (uint)attachmentDescriptionList.Count(),
                    pAttachments = attachments,
                    subpassCount = (uint)subpassDescriptionList.Count(),
                    pSubpasses = description,
                    dependencyCount = (uint)subpassDependencyList.Count(),
                    pDependencies = dependency
                };

                VkFunc.vkCreateRenderPass(VulkanRenderer.device, &renderPassCreateInfo, null, &tempRenderPass);
                renderPass = tempRenderPass;
            }

            return renderPass;
        }

        public VkFramebuffer[] CreateFramebuffer()
        {
            VkFramebuffer[] frameBufferList = new VkFramebuffer[(int)VulkanRenderer.SwapChain.ImageCount];
            for (int x = 0; x < (int)VulkanRenderer.SwapChain.ImageCount; x++)
            {
                List<VkImageView> TextureAttachmentList = new List<VkImageView>();
                TextureAttachmentList.Add(VulkanRenderer.SwapChain.imageViews[x]);

                fixed (VkImageView* imageViewPtr = TextureAttachmentList.ToArray())
                {
                    VkFramebufferCreateInfo framebufferInfo = new VkFramebufferCreateInfo()
                    {
                        sType = VkStructureType.VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO,
                        renderPass = renderPass,
                        attachmentCount = TextureAttachmentList.UCount(),
                        pAttachments = imageViewPtr,
                        width = VulkanRenderer.SwapChain.SwapChainResolution.width,
                        height = VulkanRenderer.SwapChain.SwapChainResolution.height,
                        layers = 1
                    };

                    VkFramebuffer frameBuffer = FrameBufferList[x];
                    VkFunc.vkCreateFramebuffer(VulkanRenderer.device, &framebufferInfo, null, &frameBuffer);
                    frameBufferList[x] = frameBuffer;
                }
            }

            FrameBufferList = frameBufferList;
            return frameBufferList;
        }

        public VkDescriptorPool CreateDescriptorPoolBinding()
        {
            VkDescriptorPool tempDescriptorPool = new VkDescriptorPool();
            List<VkDescriptorPoolSize> DescriptorPoolBinding = new List<VkDescriptorPoolSize>();
            {
                new VkDescriptorPoolSize
                {
                    type = VkDescriptorType.VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER,
                    descriptorCount = VulkanRenderer.SwapChain.ImageCount
                };
            };

            fixed (VkDescriptorPoolSize* ptr = DescriptorPoolBinding.ToArray())
            {
                VkDescriptorPoolCreateInfo poolInfo = new VkDescriptorPoolCreateInfo()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_DESCRIPTOR_POOL_CREATE_INFO,
                    maxSets = VulkanRenderer.SwapChain.ImageCount,
                    poolSizeCount = (uint)DescriptorPoolBinding.Count,
                    pPoolSizes = ptr
                };
                VkFunc.vkCreateDescriptorPool(VulkanRenderer.device, in poolInfo, null, out tempDescriptorPool);
            }

            descriptorPool = tempDescriptorPool;
            return descriptorPool;
        }

        public unsafe void BuildRenderPipeline(Texture texture)
        {
            string jsonContent = File.ReadAllText("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Pipelines\\FrameBufferPipeline.json");
            RenderPipelineModel model = JsonConvert.DeserializeObject<RenderPipelineModel>(jsonContent);

            RenderPipelineDLL nativeModel = model.ToDLL();

            var meshProperties = MemoryManager.GetGameObjectPropertiesBuffer().ToArray();
            var texturePropertiesList = new List<VkDescriptorImageInfo> { texture.GetTextureBuffer() }.ToArray();
            var materialProperties = MemoryManager.GetMaterialPropertiesBuffer().ToArray();
            var vertexProperties = new VkDescriptorBufferInfo[0];
            var indexProperties = new VkDescriptorBufferInfo[0];
            var transformProperties = new VkDescriptorBufferInfo[0];

            descriptorSetLayoutList.Add(new nint());
            SceneDataBuffer dataBuffer = new SceneDataBuffer();

            fixed (VkDescriptorBufferInfo* vertexPtr = vertexProperties)
            fixed (VkDescriptorBufferInfo* indexPtr = indexProperties)
            fixed (VkDescriptorBufferInfo* transformPtr = transformProperties)
            fixed (VkDescriptorBufferInfo* meshPtr = meshProperties)
            fixed (VkDescriptorImageInfo* texturePtr = texturePropertiesList)
            fixed (VkDescriptorBufferInfo* materialPtr = materialProperties)
            fixed (VkDescriptorSetLayout* descriptorSetLayout = descriptorSetLayoutList.ToArray())
            fixed (VkDescriptorSet* descriptorSetList = descriptorSetLists.ToArray())
            fixed (VkPipelineLayout* pipelineLayoutPtr = &pipelineLayout)
            {
                GPUIncludes includes = new GPUIncludes
                {
                    vertexProperties = vertexPtr,
                    indexProperties = indexPtr,
                    transformProperties = transformPtr,
                    meshProperties = meshPtr,
                    texturePropertiesList = texturePtr,
                    materialProperties = materialPtr,
                    vertexPropertiesCount = (uint)vertexProperties.Length,
                    indexPropertiesCount = (uint)indexProperties.Length,
                    transformPropertiesCount = (uint)transformProperties.Length,
                    meshPropertiesCount = (uint)meshProperties.Length,
                    texturePropertiesListCount = (uint)texturePropertiesList.Length,
                    materialPropertiesCount = (uint)materialProperties.Length
                };

                VkVertexInputBindingDescription* vertexBindingDescription = null;
                VkVertexInputAttributeDescription* vertexAttributeDescription = null;
                uint size = Convert.ToUInt32(sizeof(SceneDataBuffer));

                descriptorPool = GameEngineImport.DLL_Pipeline_CreateDescriptorPool(VulkanRenderer.device, &nativeModel, &includes);
                GameEngineImport.DLL_Pipeline_CreateDescriptorSetLayout(VulkanRenderer.device, model, includes, descriptorSetLayout, (uint)descriptorSetLayoutList.Count);
                VkDescriptorSet* descriptorSetPtrArray = GameEngineImport.DLL_Pipeline_AllocateDescriptorSets(VulkanRenderer.device, descriptorPool, descriptorSetLayout, out size_t count);
                GameEngineImport.DLL_Pipeline_UpdateDescriptorSets(VulkanRenderer.device, descriptorSetList, model, includes);
                GameEngineImport.DLL_Pipeline_CreatePipelineLayout(VulkanRenderer.device, descriptorSetLayout, size, pipelineLayoutPtr);
                GameEngineImport.DLL_Pipeline_CreatePipeline(VulkanRenderer.device, renderPass, pipelineLayout, pipelineCache, model, vertexBindingDescription, vertexAttributeDescription, pipeline);
            }
        }

        public VkCommandBuffer Draw()
        {
            var commandIndex = VulkanRenderer.CommandIndex;
            var imageIndex = VulkanRenderer.ImageIndex;
            var commandBuffer = CommandBufferList[(int)commandIndex];
            VkClearValue[] clearValues = new VkClearValue[]
            {
                new VkClearValue
                {
                    color = new VkClearColorValue(1, 1, 0, 1)
                }
            };

            var viewport = new VkViewport
            {
                x = 0.0f,
                y = 0.0f,
                width = VulkanRenderer.SwapChain.SwapChainResolution.width,
                height = VulkanRenderer.SwapChain.SwapChainResolution.height,
                minDepth = 0.0f,
                maxDepth = 1.0f
            };
            var scissor = new VkRect2D(new VkOffset2D(0, 0), VulkanRenderer.SwapChain.SwapChainResolution);

            fixed (VkClearValue* pClearValue = clearValues.ToArray())
            {
                VkRenderPassBeginInfo renderPassInfo = new VkRenderPassBeginInfo
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_DEVICE_GROUP_RENDER_PASS_BEGIN_INFO,
                    renderPass = renderPass,
                    renderArea = new VkRect2D(new VkOffset2D(0, 0), VulkanRenderer.SwapChain.SwapChainResolution),
                    clearValueCount = 1,
                    framebuffer = FrameBufferList[imageIndex],
                    pClearValues = pClearValue,
                    pNext = null
                };


                var descriptorSet =  descriptorSetLists[0];
                var commandInfo = new VkCommandBufferBeginInfo { flags = 0 };
                VkFunc.vkBeginCommandBuffer(commandBuffer, &commandInfo);
                VkFunc.vkCmdBeginRenderPass(commandBuffer, &renderPassInfo, VkSubpassContents.VK_SUBPASS_CONTENTS_INLINE);
                VkFunc.vkCmdSetViewport(commandBuffer, 0, 1, &viewport);
                VkFunc.vkCmdSetScissor(commandBuffer, 0, 1, &scissor);
                VkFunc.vkCmdBindPipeline(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, pipeline);
                VkFunc.vkCmdBindDescriptorSets(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, pipelineLayout, 0, 1, ref descriptorSet, 0, null);
                VkFunc.vkCmdDraw(commandBuffer, 6, 1, 0, 0);
                VkFunc.vkCmdEndRenderPass(commandBuffer);
                VkFunc.vkEndCommandBuffer(commandBuffer);

                return commandBuffer;
            }
        }

        private void SaveRenderPass()
        {
            RenderPassBuildInfoModel modelInfo = new RenderPassBuildInfoModel()
            {
                SubpassDependencyList = new List<VkSubpassDependencyModel>()
                {
                      new VkSubpassDependencyModel
                    {
                        srcSubpass = uint.MaxValue,
                        dstSubpass = 0,
                        srcStageMask = VkPipelineStageFlagBits.COLOR_ATTACHMENT_OUTPUT_BIT,
                        dstStageMask = VkPipelineStageFlagBits.COLOR_ATTACHMENT_OUTPUT_BIT,
                        srcAccessMask = 0,
                        dstAccessMask = VkAccessFlagBits.VK_ACCESS_COLOR_ATTACHMENT_WRITE_BIT,
                    },
                },
                _name = "Default2DRenderPass"
            };

            string jsonString = JsonConvert.SerializeObject(modelInfo, Formatting.Indented);

            string finalfilePath = @"C:\Users\dotha\Documents\GitHub\VulkanGameEngine\RenderPass\FrameBufferRenderPass.json";
            File.WriteAllText(finalfilePath, jsonString);
        }

        public void SavePipeline()
        {
            var jsonObj = new RenderPipelineModel
            {
                _name = "DefaultPipeline",
                VertexShader = "C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Shaders\\Shader2DVert.spv",
                FragmentShader = "C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Shaders\\Shader2DFrag.spv",
                PipelineColorBlendAttachmentStateList = new List<VkPipelineColorBlendAttachmentState>()
                {
                     new VkPipelineColorBlendAttachmentState
                            {
                                blendEnable = true,
                                srcColorBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_SRC_ALPHA,
                                dstColorBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_ONE_MINUS_SRC_ALPHA,
                                colorBlendOp = VkBlendOp.VK_BLEND_OP_ADD,
                                srcAlphaBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_ONE,
                                dstAlphaBlendFactor = VkBlendFactor.VK_BLEND_FACTOR_ZERO,
                                alphaBlendOp = VkBlendOp.VK_BLEND_OP_ADD,
                                colorWriteMask = VkColorComponentFlagBits.VK_COLOR_COMPONENT_R_BIT | VkColorComponentFlagBits.VK_COLOR_COMPONENT_G_BIT | VkColorComponentFlagBits.VK_COLOR_COMPONENT_B_BIT | VkColorComponentFlagBits.VK_COLOR_COMPONENT_A_BIT
                            }
                },
                PipelineDepthStencilStateCreateInfo = new VkPipelineDepthStencilStateCreateInfoModel()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_DEPTH_STENCIL_STATE_CREATE_INFO,
                    depthTestEnable = Vk.True,
                    depthWriteEnable = Vk.True,
                    depthCompareOp = VkCompareOp.VK_COMPARE_OP_LESS_OR_EQUAL,
                    depthBoundsTestEnable = Vk.False,
                    stencilTestEnable = Vk.False
                },
                PipelineMultisampleStateCreateInfo = new VkPipelineMultisampleStateCreateInfoModel()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_MULTISAMPLE_STATE_CREATE_INFO,
                    rasterizationSamples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT
                },
                PipelineRasterizationStateCreateInfo = new VkPipelineRasterizationStateCreateInfoModel()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_RASTERIZATION_STATE_CREATE_INFO,
                    depthClampEnable = Vk.False,
                    rasterizerDiscardEnable = Vk.False,
                    polygonMode = VkPolygonMode.VK_POLYGON_MODE_FILL,
                    cullMode = VkCullModeFlagBits.VK_CULL_MODE_NONE,
                    frontFace = VkFrontFace.VK_FRONT_FACE_COUNTER_CLOCKWISE,
                    depthBiasEnable = Vk.False,
                    depthBiasConstantFactor = 0.0f,
                    depthBiasClamp = 0.0f,
                    depthBiasSlopeFactor = 0.0f,
                    lineWidth = 1.0f
                },
                ScissorList = new List<VkRect2D>(),
                ViewportList = new List<VkViewport>(),
                PipelineColorBlendStateCreateInfoModel = new VkPipelineColorBlendStateCreateInfoModel()
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_PIPELINE_COLOR_BLEND_STATE_CREATE_INFO,
                    logicOpEnable = Vk.False,
                    logicOp = VkLogicOp.VK_LOGIC_OP_NO_OP,
                    attachmentCount = 1,
                    pNext = null
                },
                PipelineInputAssemblyStateCreateInfo = new VkPipelineInputAssemblyStateCreateInfoModel()
                {
                    topology = VkPrimitiveTopology.VK_PRIMITIVE_TOPOLOGY_TRIANGLE_LIST
                },
                PipelineDescriptorModelsList = new List<PipelineDescriptorModel>()
                {

                },
                LayoutBindingList = new List<VkDescriptorSetLayoutBindingModel>()
                {
 
                }
            };

            string finalfilePath = @"C:\Users\dotha\Documents\GitHub\VulkanGameEngine\Pipelines\FrameBufferPipeline.json";
            try
            {
                string jsonString = JsonConvert.SerializeObject(jsonObj, Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                Directory.CreateDirectory(Path.GetDirectoryName(finalfilePath)); // Ensure directory exists
                File.WriteAllText(finalfilePath, jsonString);
                Console.WriteLine("Pipeline saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save pipeline: {ex.Message}");
                throw;
            }
        }

        public static List<VkDescriptorBufferInfo> GetGameObjectPropertiesBuffer(List<Mesh> meshList)
        {
            List<VkDescriptorBufferInfo> MeshPropertiesBuffer = new List<VkDescriptorBufferInfo>();
            if (meshList.Count == 0)
            {
                VkDescriptorBufferInfo nullBuffer = new VkDescriptorBufferInfo();
                nullBuffer.buffer = new VkBuffer();
                nullBuffer.offset = 0;
                nullBuffer.range = Vk.WholeSize;
                MeshPropertiesBuffer.Add(nullBuffer);
            }
            else
            {
                foreach (var mesh in meshList)
                {
                    if (mesh != null)
                    {
                        VkDescriptorBufferInfo MeshProperitesBufferInfo = new VkDescriptorBufferInfo();
                        MeshProperitesBufferInfo.buffer = mesh.GetMeshPropertiesBuffer().Buffer;
                        MeshProperitesBufferInfo.offset = 0;
                        MeshProperitesBufferInfo.range = Vk.WholeSize;
                        MeshPropertiesBuffer.Add(MeshProperitesBufferInfo);
                    }
                }
            }

            return MeshPropertiesBuffer;
        }

        public static List<VkDescriptorImageInfo> GetTexturePropertiesBuffer(List<Texture> textureList)
        {
            List<VkDescriptorImageInfo> TexturePropertiesBuffer = new List<VkDescriptorImageInfo>();
            if (textureList.Count == 0)
            {
                VkSamplerCreateInfo NullSamplerInfo = new VkSamplerCreateInfo
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_SAMPLER_CREATE_INFO,
                    magFilter = VkFilter.VK_FILTER_NEAREST,
                    minFilter = VkFilter.VK_FILTER_NEAREST,
                    addressModeU = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_REPEAT,
                    addressModeV = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_REPEAT,
                    addressModeW = VkSamplerAddressMode.VK_SAMPLER_ADDRESS_MODE_REPEAT,
                    anisotropyEnable = true,
                    maxAnisotropy = 16.0f,
                    borderColor = VkBorderColor.VK_BORDER_COLOR_FLOAT_OPAQUE_BLACK,
                    unnormalizedCoordinates = false,
                    compareEnable = false,
                    compareOp = VkCompareOp.VK_COMPARE_OP_ALWAYS,
                    mipmapMode = VkSamplerMipmapMode.VK_SAMPLER_MIPMAP_MODE_LINEAR,
                    minLod = 0,
                    maxLod = 0,
                    mipLodBias = 0
                };
                var result = VkFunc.vkCreateSampler(VulkanRenderer.device, &NullSamplerInfo, null, out VkSampler nullSampler);


                VkDescriptorImageInfo nullBuffer = new VkDescriptorImageInfo
                {
                    imageLayout = VkImageLayout.VK_IMAGE_LAYOUT_DEPTH_READ_ONLY_OPTIMAL,
                    imageView = new VkImageView(),
                    sampler = nullSampler
                };
                TexturePropertiesBuffer.Add(nullBuffer);
            }
            else
            {
                foreach (var texture in textureList)
                {
                    if (texture != null)
                    {
                        VkDescriptorImageInfo textureDescriptor = new VkDescriptorImageInfo
                        {
                            imageLayout = VkImageLayout.VK_IMAGE_LAYOUT_DEPTH_READ_ONLY_OPTIMAL,
                            imageView = texture.View,
                            sampler = texture.Sampler
                        };
                        TexturePropertiesBuffer.Add(textureDescriptor);
                    }
                }
            }

            return TexturePropertiesBuffer;
        }

        public static List<VkDescriptorBufferInfo> GetMaterialPropertiesBuffer(List<Material> materialList)
        {
            List<VkDescriptorBufferInfo> MeshPropertiesBuffer = new List<VkDescriptorBufferInfo>();
            if (materialList.Count == 0)
            {
                VkDescriptorBufferInfo nullBuffer = new VkDescriptorBufferInfo();
                nullBuffer.buffer = new VkBuffer();
                nullBuffer.offset = 0;
                nullBuffer.range = Vk.WholeSize;
                MeshPropertiesBuffer.Add(nullBuffer);
            }
            else
            {
                foreach (var mesh in materialList)
                {
                    mesh.GetMaterialPropertiesBuffer(ref MeshPropertiesBuffer);
                }
            }
            return MeshPropertiesBuffer;
        }
    }
}