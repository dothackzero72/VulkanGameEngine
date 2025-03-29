using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using VulkanGameEngineGameObjectScripts.Vulkan;
using VulkanGameEngineLevelEditor.Components;
using VulkanGameEngineLevelEditor.Models;
using Silk.NET.Vulkan;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineLevelEditor.Vulkan;
using static VulkanGameEngineLevelEditor.GameEngineAPI.SpriteInstanceVertex2D;
using Newtonsoft.Json;
using VulkanGameEngineLevelEditor.RenderPassEditor;
using System.IO;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class Level2DRenderer
    {
        private Vk vk = Vk.GetApi();
        private Device device => new Device(VulkanRenderer.device);
        private RenderPass renderPass;
        private ListPtr<Framebuffer> FrameBufferList;
        private ListPtr<CommandBuffer> commandBufferList; // Updated to Silk.NET type
        public List<SpriteBatchLayer> SpriteLayerList { get; private set; } = new List<SpriteBatchLayer>();
        public List<GameObject> GameObjectList { get; private set; } = new List<GameObject>();
        public List<Texture> TextureList { get; private set; } = new List<Texture>();
        public List<Material> MaterialList { get; private set; } = new List<Material>();
        public RenderedTexture texture { get; private set; }
        public DepthTexture depthTexture { get; private set; }
        private ivec2 RenderPassResolution;

        // Pipeline-related fields
        private VkDescriptorPool descriptorPool;
        private ListPtr<VkDescriptorSetLayout> descriptorSetLayoutList = new ListPtr<VkDescriptorSetLayout>();
        public ListPtr<VkDescriptorSet> descriptorSetList = new ListPtr<VkDescriptorSet>();
        public Pipeline pipeline;
        public VkPipelineLayout pipelineLayout;
        private PipelineCache pipelineCache;

        public Level2DRenderer(string json, ivec2 renderPassResolution)
        {
            RenderPassBuildInfoModel model = new RenderPassBuildInfoModel(json);
            foreach (var item in model.RenderedTextureInfoModelList)
            {
                item.ImageCreateInfo.extent = new VkExtent3DModel
                {
                    width = (uint)renderPassResolution.x,
                    height = (uint)renderPassResolution.y,
                    depth = 1
                };
            }

            texture = new RenderedTexture(VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, model.RenderedTextureInfoModelList[0].ImageCreateInfo.Convert(), model.RenderedTextureInfoModelList[0].SamplerCreateInfo.Convert());
            depthTexture = new DepthTexture(VkImageAspectFlagBits.VK_IMAGE_ASPECT_DEPTH_BIT, model.RenderedTextureInfoModelList[1].ImageCreateInfo.Convert(), model.RenderedTextureInfoModelList[1].SamplerCreateInfo.Convert());

            RenderPassResolution = renderPassResolution;
            FrameBufferList = new ListPtr<Framebuffer>();
            commandBufferList = new ListPtr<CommandBuffer>();
            VulkanRenderer.CreateCommandBuffers(commandBufferList); // Assumes updated for Silk.NET
            CreateHardcodedRenderPass();
            CreateHardcodedFramebuffers();
            StartLevelRenderer();
        }

        private void CreateHardcodedRenderPass()
        {
            AttachmentDescription[] attachmentDescriptionList = new[]
            {
                new AttachmentDescription
                {
                    Format = Format.R8G8B8A8Unorm,
                    Samples = SampleCountFlags.Count1Bit,
                    LoadOp = AttachmentLoadOp.Clear,
                    StoreOp = AttachmentStoreOp.Store,
                    StencilLoadOp = AttachmentLoadOp.DontCare,
                    StencilStoreOp = AttachmentStoreOp.DontCare,
                    InitialLayout = ImageLayout.Undefined,
                    FinalLayout = ImageLayout.PresentSrcKhr
                },
               new AttachmentDescription
                {
                    Format = Format.D32Sfloat,
                    Samples = SampleCountFlags.Count1Bit,
                    LoadOp = AttachmentLoadOp.Clear,
                    StoreOp = AttachmentStoreOp.DontCare,
                    StencilLoadOp = AttachmentLoadOp.DontCare,
                    StencilStoreOp = AttachmentStoreOp.DontCare,
                    InitialLayout = ImageLayout.Undefined,
                    FinalLayout = ImageLayout.DepthStencilAttachmentOptimal
                }
            };

            AttachmentReference[] colorRefsList = new[]
            {
                new AttachmentReference
                {
                    Attachment = 0,
                    Layout = ImageLayout.ColorAttachmentOptimal
                },
                new AttachmentReference
                {
                    Attachment = 1,
                    Layout = ImageLayout.DepthAttachmentOptimal
                }
            };

            SubpassDescription[] subpassDescriptionList = new[]
            {
                new SubpassDescription
                {
                    PipelineBindPoint = PipelineBindPoint.Graphics,
                    ColorAttachmentCount = 1,
                    PColorAttachments = null,
                    PResolveAttachments = null,
                    PDepthStencilAttachment = null
                }
            };

            SubpassDependency[] subpassDependencyList = new[]
            {
                new SubpassDependency
                {
                    SrcSubpass = VulkanConst.VK_SUBPASS_EXTERNAL,
                    DstSubpass = 0,
                    SrcStageMask = PipelineStageFlags.ColorAttachmentOutputBit,
                    DstStageMask = PipelineStageFlags.ColorAttachmentOutputBit,
                    SrcAccessMask = 0,
                    DstAccessMask = AccessFlags.ColorAttachmentWriteBit
                }
            };

            fixed (AttachmentDescription* attachments = attachmentDescriptionList)
            fixed (AttachmentReference* colorRefs = colorRefsList)
            fixed (SubpassDescription* subpasses = subpassDescriptionList)
            fixed (SubpassDependency* dependencies = subpassDependencyList)
            {
                subpasses[0].PColorAttachments = colorRefs;

                RenderPassCreateInfo renderPassInfo = new RenderPassCreateInfo
                {
                    SType = StructureType.RenderPassCreateInfo,
                    PNext = null,
                    Flags = 0,
                    AttachmentCount = (uint)attachmentDescriptionList.Length,
                    PAttachments = attachments,
                    SubpassCount = (uint)subpassDescriptionList.Length,
                    PSubpasses = subpasses,
                    DependencyCount = (uint)subpassDependencyList.Length,
                    PDependencies = dependencies
                };

                Result result = vk.CreateRenderPass(device, renderPassInfo, null, out renderPass);
                if (result != Result.Success)
                {
                    throw new Exception($"vkCreateRenderPass failed: {result}");
                }
            }
        }

        private void CreateHardcodedFramebuffers()
        {
            for (int i = 0; i < VulkanRenderer.SwapChain.ImageCount; i++)
            {
                ListPtr<ImageView> attachments = new ListPtr<ImageView> { new ImageView((ulong?)texture.View), new ImageView((ulong?)depthTexture.View) };
                FramebufferCreateInfo fbInfo = new FramebufferCreateInfo
                {
                    SType = StructureType.FramebufferCreateInfo,
                    RenderPass = renderPass,
                    AttachmentCount = attachments.UCount,
                    PAttachments = attachments.Ptr,
                    Width = (uint)RenderPassResolution.x,
                    Height = (uint)RenderPassResolution.y,
                    Layers = 1
                };

                vk.CreateFramebuffer(device, fbInfo, null, out Framebuffer fb);
                FrameBufferList.Add(fb);
            }
        }

        public void StartLevelRenderer()
        {
            string jsonContent = File.ReadAllText(ConstConfig.Default2DPipeline);
            RenderPipelineDLL model = JsonConvert.DeserializeObject<RenderPipelineModel>(jsonContent).ToDLL();

            TextureList.Add(new Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Textures\\MegaMan_diffuse.bmp", VkFormat.VK_FORMAT_R8G8B8A8_SRGB, VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, TextureTypeEnum.kType_DiffuseTextureMap, false));
            TransitionTextureLayout(TextureList[0]);
            MaterialList.Add(new Material("Material1"));
            MaterialList.Last().SetAlbedoMap(TextureList[0]);

            ivec2 size = new ivec2(32);
            SpriteSheet spriteSheet = new SpriteSheet(MaterialList[0], size, 0);

            AddGameObject("Obj1", new List<ComponentTypeEnum> { ComponentTypeEnum.kTransform2DComponent, ComponentTypeEnum.kSpriteComponent }, spriteSheet, new vec2(960.0f, 540.0f));
            AddGameObject("Obj2", new List<ComponentTypeEnum> { ComponentTypeEnum.kTransform2DComponent, ComponentTypeEnum.kSpriteComponent }, spriteSheet, new vec2(300.0f, 20.0f));
            AddGameObject("Obj3", new List<ComponentTypeEnum> { ComponentTypeEnum.kTransform2DComponent, ComponentTypeEnum.kSpriteComponent }, spriteSheet, new vec2(300.0f, 80.0f));

            SpriteLayerList.Add(new SpriteBatchLayer(GameObjectList));
            CreateHardcodedPipeline(model);
        }

        private void CreateHardcodedPipeline(RenderPipelineDLL model)
        {
            ListPtr<VkDescriptorBufferInfo> meshPropertiesBuffer = new ListPtr<VkDescriptorBufferInfo>();
            foreach (var mesh in GetMeshFromGameObjects())
            {
                var sdf = mesh.GetMeshPropertiesBuffer();
                var a = new VkDescriptorBufferInfo
                {
                    buffer = new VkBuffer(sdf.buffer),
                    offset = 0,
                    range = VulkanConst.VK_WHOLE_SIZE

                };
                meshPropertiesBuffer.Add(a); 
            }

            var meshProperties = meshPropertiesBuffer;
            var textureProperties = GetTexturePropertiesBuffer(TextureList);
            var materialProperties = GetMaterialPropertiesBuffer(MaterialList);
            var vertexProperties = new ListPtr<VkDescriptorBufferInfo>();
            var indexProperties = new ListPtr<VkDescriptorBufferInfo>();
            var transformProperties = new ListPtr<VkDescriptorBufferInfo>();

            GPUIncludes includes = new GPUIncludes
            {
                vertexProperties = vertexProperties.Ptr,
                indexProperties = indexProperties.Ptr,
                transformProperties = transformProperties.Ptr,
                meshProperties = meshPropertiesBuffer.Ptr,
                texturePropertiesList = (VkDescriptorImageInfo*)textureProperties.Ptr,
                materialProperties = (VkDescriptorBufferInfo*)materialProperties.Ptr,
                vertexPropertiesCount = vertexProperties.UCount,
                indexPropertiesCount = indexProperties.UCount,
                transformPropertiesCount = transformProperties.UCount,
                meshPropertiesCount = meshProperties.UCount,
                texturePropertiesListCount = textureProperties.UCount,
                materialPropertiesCount = materialProperties.UCount
            };

            descriptorSetList = new ListPtr<nint>(1);
            descriptorSetLayoutList = new ListPtr<nint>(1);
            descriptorPool = GameEngineImport.DLL_Pipeline_CreateDescriptorPool(VulkanRenderer.device, model, &includes);
            GameEngineImport.DLL_Pipeline_CreateDescriptorSetLayout(VulkanRenderer.device, model, includes, descriptorSetLayoutList.Ptr, model.LayoutBindingListCount);
            GameEngineImport.DLL_Pipeline_AllocateDescriptorSets(VulkanRenderer.device, descriptorPool, descriptorSetLayoutList.Ptr, descriptorSetList.Ptr, descriptorSetLayoutList.UCount);
            // GameEngineImport.DLL_Pipeline_CreateDescriptorSetLayout(VulkanRenderer.device, model, includes, descriptorSetLayoutList.Ptr, model.LayoutBindingListCount);

            // Descriptor Set Layout
            //ListPtr<VkDescriptorSetLayoutBinding> bindings = new ListPtr<VkDescriptorSetLayoutBinding>
            //     {
            //    new VkDescriptorSetLayoutBinding
            //    {
            //        binding = 0,
            //        descriptorType = (VkDescriptorType)DescriptorType.StorageBuffer, // Instance data
            //        descriptorCount = 1,
            //        stageFlags = (VkShaderStageFlagBits)ShaderStageFlags.VertexBit
            //    },
            //    new VkDescriptorSetLayoutBinding
            //    {
            //        binding = 1,
            //        descriptorType = (VkDescriptorType)DescriptorType.CombinedImageSampler,
            //        descriptorCount = 1,
            //        stageFlags = (VkShaderStageFlagBits)ShaderStageFlags.FragmentBit
            //    },
            //    new VkDescriptorSetLayoutBinding
            //    {
            //        binding = 2,
            //        descriptorType = (VkDescriptorType)DescriptorType.StorageBuffer, // Instance data
            //        descriptorCount = 1,
            //        stageFlags = (VkShaderStageFlagBits)ShaderStageFlags.VertexBit | (VkShaderStageFlagBits)ShaderStageFlags.FragmentBit
            //    },
            //};
            //VkDescriptorSetLayoutCreateInfo layoutInfo = new VkDescriptorSetLayoutCreateInfo
            //{
            //    sType = (VkStructureType)StructureType.DescriptorSetLayoutCreateInfo,
            //    bindingCount = bindings.UCount,
            //    pBindings = bindings.Ptr
            //};
            //VkFunc.vkCreateDescriptorSetLayout(VulkanRenderer.device, &layoutInfo, null, out VkDescriptorSetLayout layout);
            //descriptorSetLayoutList.Add(layout);


            // Allocate Descriptor Sets
            //descriptorSetList.Add(new nint());
            //GameEngineImport.DLL_Pipeline_AllocateDescriptorSets(VulkanRenderer.device, descriptorPool, descriptorSetLayoutList.Ptr, descriptorSetList.Ptr, model.LayoutBindingListCount);

            // Update Descriptor Sets
            var instanceBufferInfo = new VkDescriptorBufferInfo
            {
                buffer = SpriteLayerList[0].SpriteBuffer.Buffer, // Assumes VulkanBuffer<T> uses Silk.NET Buffer
                offset = 0,
                range = (ulong)(sizeof(SpriteInstanceStruct) * SpriteLayerList[0].SpriteInstanceList.Count)
            };

            var textureInfos = GetTexturePropertiesBuffer(TextureList);
            var materialInfo = GetMaterialPropertiesBuffer(MaterialList);
            ListPtr<VkWriteDescriptorSet> writes = new ListPtr<VkWriteDescriptorSet>()
            {
                new VkWriteDescriptorSet
                {
                    sType = (VkStructureType)StructureType.WriteDescriptorSet,
                    dstSet = descriptorSetList[0],
                    dstBinding = 0,
                    descriptorCount = 1,
                    descriptorType = (VkDescriptorType)DescriptorType.StorageBuffer,
                    pBufferInfo = &instanceBufferInfo
                },
                new VkWriteDescriptorSet
                {
                    sType = (VkStructureType)StructureType.WriteDescriptorSet,
                    dstSet = descriptorSetList[0],
                    dstBinding = 1,
                    descriptorCount = 1,
                    descriptorType = (VkDescriptorType)DescriptorType.CombinedImageSampler,
                    pImageInfo = textureInfos.Ptr
                },
                 new VkWriteDescriptorSet
                {
                    sType = (VkStructureType)StructureType.WriteDescriptorSet,
                    dstSet = descriptorSetList[0],
                    dstBinding = 0,
                    descriptorCount = 1,
                    descriptorType = (VkDescriptorType)DescriptorType.StorageBuffer,
                    pBufferInfo = &instanceBufferInfo
                }
            };
            VkFunc.vkUpdateDescriptorSets(VulkanRenderer.device, writes.UCount, writes.Ptr, 0, null);


            // Pipeline Layout with Push Constants
            GameEngineImport.DLL_Pipeline_CreatePipelineLayout(VulkanRenderer.device, descriptorSetLayoutList.Ptr, (uint)sizeof(SceneDataBuffer), out VkPipelineLayout pipelineLayoutPtr, model.LayoutBindingListCount);
            pipelineLayout = pipelineLayoutPtr;

            // Graphics Pipeline with Vertex Input
            ShaderModule vertShader = CreateShaderModule("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Shaders\\Shader2DVert.spv");
            ShaderModule fragShader = CreateShaderModule("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Shaders\\Shader2DFrag.spv");

            byte[] mainBytes = Encoding.UTF8.GetBytes("main\0");
            GCHandle vertMainHandle = GCHandle.Alloc(mainBytes, GCHandleType.Pinned);
            GCHandle fragMainHandle = GCHandle.Alloc(mainBytes, GCHandleType.Pinned);

            PipelineShaderStageCreateInfo[] stages = new[]
            {
                new PipelineShaderStageCreateInfo
                {
                    SType = StructureType.PipelineShaderStageCreateInfo,
                    Stage = ShaderStageFlags.VertexBit,
                    Module = vertShader,
                    PName = (byte*)vertMainHandle.AddrOfPinnedObject()
                },
                new PipelineShaderStageCreateInfo
                {
                    SType = StructureType.PipelineShaderStageCreateInfo,
                    Stage = ShaderStageFlags.FragmentBit,
                    Module = fragShader,
                    PName = (byte*)fragMainHandle.AddrOfPinnedObject()
                }
            };

            var vertexBindings = SilkSpriteInstanceVertex2D.GetSilkBindingDescriptions(); // Assumes updated to Silk.NET VertexInputBindingDescription
            var vertexAttributes = SilkSpriteInstanceVertex2D.GetSilkAttributeDescriptions(); // Assumes updated to Silk.NET VertexInputAttributeDescription
            PipelineVertexInputStateCreateInfo vertexInputInfo = new PipelineVertexInputStateCreateInfo
            {
                SType = StructureType.PipelineVertexInputStateCreateInfo,
                VertexBindingDescriptionCount = vertexBindings.UCount,
                PVertexBindingDescriptions = vertexBindings.Ptr,
                VertexAttributeDescriptionCount = vertexAttributes.UCount,
                PVertexAttributeDescriptions = vertexAttributes.Ptr
            };

            PipelineInputAssemblyStateCreateInfo inputAssembly = new PipelineInputAssemblyStateCreateInfo
            {
                SType = StructureType.PipelineInputAssemblyStateCreateInfo,
                Topology = PrimitiveTopology.TriangleList,
                PrimitiveRestartEnable = false
            };

            Viewport viewport = new Viewport { X = 0, Y = 0, Width = (float)RenderPassResolution.x, Height = (float)RenderPassResolution.y, MinDepth = 0, MaxDepth = 1 };
            Rect2D scissor = new Rect2D { Offset = new Offset2D(0, 0), Extent = new Extent2D { Width = (uint)RenderPassResolution.x, Height = (uint)RenderPassResolution.y } };
            PipelineViewportStateCreateInfo viewportState = new PipelineViewportStateCreateInfo
            {
                SType = StructureType.PipelineViewportStateCreateInfo,
                ViewportCount = 1,
                PViewports = &viewport,
                ScissorCount = 1,
                PScissors = &scissor
            };

            PipelineRasterizationStateCreateInfo rasterizer = new PipelineRasterizationStateCreateInfo
            {
                SType = StructureType.PipelineRasterizationStateCreateInfo,
                DepthClampEnable = false,
                RasterizerDiscardEnable = false,
                PolygonMode = PolygonMode.Fill,
                CullMode = CullModeFlags.None,
                FrontFace = FrontFace.Clockwise,
                LineWidth = 1.0f
            };

            PipelineMultisampleStateCreateInfo multisampling = new PipelineMultisampleStateCreateInfo
            {
                SType = StructureType.PipelineMultisampleStateCreateInfo,
                RasterizationSamples = SampleCountFlags.Count1Bit,
                SampleShadingEnable = false
            };

            PipelineColorBlendAttachmentState colorBlendAttachment = new PipelineColorBlendAttachmentState
            {
                BlendEnable = true,
                SrcColorBlendFactor = BlendFactor.SrcAlpha,
                DstColorBlendFactor = BlendFactor.OneMinusSrcAlpha,
                ColorBlendOp = BlendOp.Add,
                SrcAlphaBlendFactor = BlendFactor.One,
                DstAlphaBlendFactor = BlendFactor.Zero,
                AlphaBlendOp = BlendOp.Add,
                ColorWriteMask = ColorComponentFlags.RBit | ColorComponentFlags.GBit | ColorComponentFlags.BBit | ColorComponentFlags.ABit
            };

            PipelineColorBlendStateCreateInfo colorBlending = new PipelineColorBlendStateCreateInfo
            {
                SType = StructureType.PipelineColorBlendStateCreateInfo,
                LogicOpEnable = false,
                AttachmentCount = 1,
                PAttachments = &colorBlendAttachment
            };

            GCHandle stagesHandle = GCHandle.Alloc(stages, GCHandleType.Pinned);
            GraphicsPipelineCreateInfo pipelineInfo = new GraphicsPipelineCreateInfo
            {
                SType = StructureType.GraphicsPipelineCreateInfo,
                StageCount = 2,
                PStages = (PipelineShaderStageCreateInfo*)stagesHandle.AddrOfPinnedObject(),
                PVertexInputState = &vertexInputInfo,
                PInputAssemblyState = &inputAssembly,
                PViewportState = &viewportState,
                PRasterizationState = &rasterizer,
                PMultisampleState = &multisampling,
                PDepthStencilState = null,
                PColorBlendState = &colorBlending,
                Layout = new PipelineLayout((ulong?)pipelineLayout),
                RenderPass = renderPass,
                Subpass = 0
            };

            Result result = vk.CreateGraphicsPipelines(device, pipelineCache, 1, pipelineInfo, null, out pipeline);
            if (result != Result.Success)
            {
                throw new Exception($"vkCreateGraphicsPipelines failed: {result}");
            }
            stagesHandle.Free();
            vertMainHandle.Free();
            fragMainHandle.Free();

            vk.DestroyShaderModule(device, vertShader, null);
            vk.DestroyShaderModule(device, fragShader, null);
        }

        public void Update(float deltaTime)
        {
            DestroyDeadGameObjects();
            VkCommandBuffer commandBuffer = VulkanRenderer.BeginSingleUseCommandBuffer();
            foreach (var obj in GameObjectList)
            {
                obj.Update(commandBuffer, deltaTime);
            }
            foreach (var spriteLayer in SpriteLayerList)
            {
                spriteLayer.Update(commandBuffer, deltaTime);
            }
            VulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);

            // Update descriptor set with latest instance buffer
            var instanceBufferInfo = new DescriptorBufferInfo
            {
                Buffer = new Silk.NET.Vulkan.Buffer((ulong?)SpriteLayerList[0].SpriteBuffer.Buffer),
                Offset = 0,
                Range = (ulong)(sizeof(SpriteInstanceStruct) * SpriteLayerList[0].SpriteInstanceList.Count)
            };
            WriteDescriptorSet write = new WriteDescriptorSet
            {
                SType = StructureType.WriteDescriptorSet,
                DstSet = new DescriptorSet((ulong?)descriptorSetList[0]),
                DstBinding = 0,
                DescriptorCount = 1,
                DescriptorType = DescriptorType.StorageBuffer,
                PBufferInfo = &instanceBufferInfo
            };
            vk.UpdateDescriptorSets(device, 1, &write, 0, null);
        }

        private void DestroyDeadGameObjects()
        {
            if (!GameObjectList.Any()) return;

            var deadGameObjectList = GameObjectList.Where(x => !x.GameObjectAlive).ToList();
            if (deadGameObjectList.Any())
            {
                foreach (var gameObject in deadGameObjectList)
                {
                    var spriteComponent = gameObject.GetComponentByComponentType(ComponentTypeEnum.kSpriteComponent);
                    if (spriteComponent != null)
                    {
                        var sprite = (spriteComponent as SpriteComponent).SpriteObj;
                        gameObject.RemoveComponent(spriteComponent);
                    }
                    gameObject.Destroy();
                }
            }
            GameObjectList.RemoveAll(x => !x.GameObjectAlive);
        }

        private ShaderModule CreateShaderModule(string filePath)
        {
            byte[] code = System.IO.File.ReadAllBytes(filePath);
            GCHandle codeHandle = GCHandle.Alloc(code, GCHandleType.Pinned);
            ShaderModuleCreateInfo createInfo = new ShaderModuleCreateInfo
            {
                SType = StructureType.ShaderModuleCreateInfo,
                CodeSize = (nuint)code.Length,
                PCode = (uint*)codeHandle.AddrOfPinnedObject()
            };
            vk.CreateShaderModule(device, createInfo, null, out ShaderModule module);
            codeHandle.Free();
            return module;
        }

        private void TransitionTextureLayout(Texture texture)
        {
            VkCommandBuffer commandBuffer = VulkanRenderer.BeginSingleUseCommandBuffer();

            ImageMemoryBarrier barrier = new ImageMemoryBarrier
            {
                SType = StructureType.ImageMemoryBarrier,
                OldLayout = ImageLayout.Undefined,
                NewLayout = ImageLayout.ShaderReadOnlyOptimal,
                SrcQueueFamilyIndex = VulkanConst.VK_QUEUE_FAMILY_IGNORED,
                DstQueueFamilyIndex = VulkanConst.VK_QUEUE_FAMILY_IGNORED,
                Image = new Image((ulong?)texture.Image),
                SubresourceRange = new ImageSubresourceRange
                {
                    AspectMask = ImageAspectFlags.ColorBit,
                    BaseMipLevel = 0,
                    LevelCount = 1,
                    BaseArrayLayer = 0,
                    LayerCount = 1
                },
                SrcAccessMask = 0,
                DstAccessMask = AccessFlags.ShaderReadBit
            };

            vk.CmdPipelineBarrier(
                new CommandBuffer(commandBuffer),
                PipelineStageFlags.TopOfPipeBit,
                PipelineStageFlags.FragmentShaderBit,
                0,
                0, null,
                0, null,
                1, ref barrier
            );

            VulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);
        }

        private ListPtr<DescriptorBufferInfo> GetMeshPropertiesBuffer(List<Mesh<Vertex2D>> meshList)
        {
            ListPtr<DescriptorBufferInfo> meshPropertiesBuffer = new ListPtr<DescriptorBufferInfo>();
            foreach (var mesh in meshList)
            {
                var sdf = mesh.GetMeshPropertiesBuffer();
                var a = new DescriptorBufferInfo
                {
                    Buffer = new Silk.NET.Vulkan.Buffer((ulong)sdf.buffer),
                    Offset = 0,
                    Range = VulkanConst.VK_WHOLE_SIZE

                };
                meshPropertiesBuffer.Add(a); // Assumes updated to Silk.NET type
            }
            return meshPropertiesBuffer;
        }

        private ListPtr<VkDescriptorImageInfo> GetTexturePropertiesBuffer(List<Texture> textureList)
        {
            ListPtr<VkDescriptorImageInfo> texturePropertiesBuffer = new ListPtr<VkDescriptorImageInfo>();
            foreach (var texture in textureList)
            {
                texturePropertiesBuffer.Add(texture.GetTexturePropertiesBuffer());
            }
            return texturePropertiesBuffer;
        }

        private ListPtr<VkDescriptorBufferInfo> GetMaterialPropertiesBuffer(List<Material> materialList)
        {
            ListPtr<VkDescriptorBufferInfo> materialPropertiesBuffer = new ListPtr<VkDescriptorBufferInfo>();
            foreach (var material in materialList)
            {
                materialPropertiesBuffer.Add(material.GetMaterialPropertiesBuffer());
            }
            return materialPropertiesBuffer;
        }

        public VkCommandBuffer Draw(List<GameObject> meshList, SceneDataBuffer sceneDataBuffer)
        {
            var commandIndex = VulkanRenderer.CommandIndex;
            var imageIndex = VulkanRenderer.ImageIndex;
            var commandBuffer = commandBufferList[(int)commandIndex].Handle;

            using ListPtr<VkClearValue> clearValues = new ListPtr<VkClearValue>();
            clearValues.Add(new VkClearValue { Color = new VkClearColorValue(0, 0, 0, 1) });
            clearValues.Add(new VkClearValue { DepthStencil = new VkClearDepthStencilValue(0.0f, 1.0f) });

            VkRenderPassBeginInfo renderPassInfo = new VkRenderPassBeginInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
                renderPass = (nint)renderPass.Handle,
                framebuffer = (nint)FrameBufferList[(int)imageIndex].Handle,
                clearValueCount = clearValues.UCount,
                pClearValues = clearValues.Ptr,
                renderArea = new(new VkOffset2D(0, 0), VulkanRenderer.SwapChain.SwapChainResolution)
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

            var scissor = new VkRect2D
            {
                offset = new VkOffset2D(0, 0),
                extent = VulkanRenderer.SwapChain.SwapChainResolution,
            };

            VkCommandBufferBeginInfo commandInfo = new VkCommandBufferBeginInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
                flags = VkCommandBufferUsageFlagBits.VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT
            };

            VkFunc.vkResetCommandBuffer(commandBuffer, 0); // Add this
            VkFunc.vkBeginCommandBuffer(commandBuffer, &commandInfo);
            VkFunc.vkCmdBeginRenderPass(commandBuffer, &renderPassInfo, VkSubpassContents.VK_SUBPASS_CONTENTS_INLINE);
            VkFunc.vkCmdSetViewport(commandBuffer, 0, 1, &viewport);
            VkFunc.vkCmdSetScissor(commandBuffer, 0, 1, &scissor);
            foreach (var obj in SpriteLayerList)
            {
                GCHandle vertexHandle = GCHandle.Alloc(obj.SpriteLayerMesh.MeshVertexBuffer.Buffer, GCHandleType.Pinned);
                GCHandle indexHandle = GCHandle.Alloc(obj.SpriteLayerMesh.MeshIndexBuffer.Buffer, GCHandleType.Pinned);
                GCHandle instanceHandle = GCHandle.Alloc(obj.SpriteBuffer.Buffer, GCHandleType.Pinned);
                
                ulong offsets = 0;
                VkDescriptorSet descriptorSet = descriptorSetList[0];
                VkFunc.vkCmdPushConstants(commandBuffer, pipelineLayout, VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT | VkShaderStageFlagBits.VK_SHADER_STAGE_FRAGMENT_BIT, 0, (uint)sizeof(SceneDataBuffer), &sceneDataBuffer);
                VkFunc.vkCmdBindPipeline(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, (nint)pipeline.Handle);
                VkFunc.vkCmdBindDescriptorSets(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, pipelineLayout, 0, 1, &descriptorSet, 0, null);
                VkFunc.vkCmdBindVertexBuffers(commandBuffer, 0, 1, (nint*)vertexHandle.AddrOfPinnedObject(), &offsets);
                VkFunc.vkCmdBindVertexBuffers(commandBuffer, 1, 1, (nint*)instanceHandle.AddrOfPinnedObject(), &offsets);
                VkFunc.vkCmdBindIndexBuffer(commandBuffer, *(nint*)indexHandle.AddrOfPinnedObject(), 0, VkIndexType.VK_INDEX_TYPE_UINT32);
                VkFunc.vkCmdDrawIndexed(commandBuffer, obj.SpriteIndexList.UCount(), obj.SpriteInstanceList.UCount(), 0, 0, 0);

                vertexHandle.Free();
                indexHandle.Free();
                instanceHandle.Free();
            }
            VkFunc.vkCmdEndRenderPass(commandBuffer);
            VkFunc.vkEndCommandBuffer(commandBuffer);

            return commandBuffer;
        }

        private void AddGameObject(string name, List<ComponentTypeEnum> gameObjectComponentTypeList, SpriteSheet spriteSheet, vec2 objectPosition)
        {
            GameObjectList.Add(new GameObject(name, new List<ComponentTypeEnum>
            {
                ComponentTypeEnum.kTransform2DComponent,
                ComponentTypeEnum.kSpriteComponent
            }, spriteSheet));
            var gameObject = GameObjectList.Last();

            foreach (var component in gameObjectComponentTypeList)
            {
                switch (component)
                {
                    case ComponentTypeEnum.kTransform2DComponent: gameObject.AddComponent(new Transform2DComponent(gameObject.GameObjectId, objectPosition, name)); break;
                    case ComponentTypeEnum.kSpriteComponent: gameObject.AddComponent(new SpriteComponent(gameObject, name, spriteSheet)); break;
                }
            }
        }

        private List<Mesh2D> GetMeshFromGameObjects()
        {
            var meshList = new List<Mesh2D>();
            foreach (SpriteBatchLayer spriteLayer in SpriteLayerList)
            {
                meshList.Add(spriteLayer.SpriteLayerMesh);
            }
            return meshList;
        }

        public void Destroy()
        {
            vk.DestroyPipeline(device, pipeline, null);
            vk.DestroyPipelineLayout(device, new PipelineLayout((ulong?)pipelineLayout), null);
            foreach (var layout in descriptorSetLayoutList)
            {
               // vk.DestroyDescriptorSetLayout(device, layout, null);
            }
           // vk.DestroyDescriptorPool(device, descriptorPool, null);
            foreach (var fb in FrameBufferList)
            {
                vk.DestroyFramebuffer(device, fb, null);
            }
            vk.DestroyRenderPass(device, renderPass, null);

            FrameBufferList.Dispose();
            commandBufferList.Dispose();
            descriptorSetLayoutList.Dispose();
            descriptorSetList.Dispose();
        }
    }
}