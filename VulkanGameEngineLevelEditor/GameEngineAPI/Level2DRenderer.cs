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
using System.Windows.Forms;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class Level2DRenderer
    {
        private Vk vk = Vk.GetApi();
        private Device device => new Device(VulkanRenderer.device);
        private RenderPass renderPass; // Changed to Silk.NET type
        private ListPtr<Framebuffer> FrameBufferList; // Changed to Silk.NET type
        private ListPtr<VkCommandBuffer> commandBufferList; // Changed to Silk.NET type
        public List<SpriteBatchLayer> SpriteLayerList { get; private set; } = new List<SpriteBatchLayer>();
        public List<GameObject> GameObjectList { get; private set; } = new List<GameObject>();
        public List<Texture> TextureList { get; private set; } = new List<Texture>();
        public List<Material> MaterialList { get; private set; } = new List<Material>();
        public RenderedTexture texture { get; private set; }
        public DepthTexture depthTexture { get; private set; }
        private ivec2 RenderPassResolution;

        // Pipeline-related fields
        private DescriptorPool descriptorPool; // Changed to Silk.NET type
        private ListPtr<DescriptorSetLayout> descriptorSetLayoutList = new ListPtr<DescriptorSetLayout>(); // Changed to Silk.NET type
        public ListPtr<DescriptorSet> descriptorSetList = new ListPtr<DescriptorSet>(); // Changed to Silk.NET type
        public Pipeline pipeline; // Changed to Silk.NET type
        public PipelineLayout pipelineLayout; // Changed to Silk.NET type
        private PipelineCache pipelineCache; // Changed to Silk.NET type

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
            commandBufferList = new ListPtr<nint>();
            VulkanRenderer.CreateCommandBuffers(commandBufferList); // Assumes this method is updated for Silk.NET types
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
                    InitialLayout = Silk.NET.Vulkan.ImageLayout.Undefined,
                    FinalLayout = Silk.NET.Vulkan.ImageLayout.PresentSrcKhr
                }
            };

            AttachmentReference[] colorRefsList = new[]
            {
                new AttachmentReference
                {
                    Attachment = 0,
                    Layout = Silk.NET.Vulkan.ImageLayout.ColorAttachmentOptimal
                }
            };

            SubpassDescription[] subpassDescriptionList = new[]
            {
                new SubpassDescription
                {
                    PipelineBindPoint = PipelineBindPoint.Graphics,
                    ColorAttachmentCount = 1,
                    PColorAttachments = null, // Set in fixed block
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
                ImageView[] attachments = new[] { new ImageView((ulong)VulkanRenderer.SwapChain.imageViews[i]) }; // Assumes imageViews is updated to Silk.NET ImageView[]
                GCHandle attachmentsHandle = GCHandle.Alloc(attachments, GCHandleType.Pinned);
                FramebufferCreateInfo fbInfo = new FramebufferCreateInfo
                {
                    SType = StructureType.FramebufferCreateInfo,
                    RenderPass = renderPass,
                    AttachmentCount = 1,
                    PAttachments = (ImageView*)attachmentsHandle.AddrOfPinnedObject(),
                    Width = (uint)RenderPassResolution.x,
                    Height = (uint)RenderPassResolution.y,
                    Layers = 1
                };

                vk.CreateFramebuffer(device, fbInfo, null, out Framebuffer fb);
                FrameBufferList.Add(fb);
                attachmentsHandle.Free();
            }
        }

        public void StartLevelRenderer()
        {
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
            CreateHardcodedPipeline();
        }

        private void CreateHardcodedPipeline()
        {
            // Descriptor Pool
            DescriptorPoolSize[] poolSizes = new[]
            {
                new DescriptorPoolSize { Type = DescriptorType.CombinedImageSampler, DescriptorCount = (uint)TextureList.Count }
            };
            GCHandle poolSizesHandle = GCHandle.Alloc(poolSizes, GCHandleType.Pinned);
            DescriptorPoolCreateInfo poolInfo = new DescriptorPoolCreateInfo
            {
                SType = StructureType.DescriptorPoolCreateInfo,
                MaxSets = 1,
                PoolSizeCount = (uint)poolSizes.Length,
                PPoolSizes = (DescriptorPoolSize*)poolSizesHandle.AddrOfPinnedObject()
            };
            vk.CreateDescriptorPool(device, poolInfo, null, out descriptorPool);
            poolSizesHandle.Free();

            // Descriptor Set Layout
            DescriptorSetLayoutBinding[] bindings = new[]
            {
                new DescriptorSetLayoutBinding
                {
                    Binding = 0,
                    DescriptorType = DescriptorType.CombinedImageSampler,
                    DescriptorCount = 1,
                    StageFlags = ShaderStageFlags.FragmentBit
                }
            };
            GCHandle bindingsHandle = GCHandle.Alloc(bindings, GCHandleType.Pinned);
            DescriptorSetLayoutCreateInfo layoutInfo = new DescriptorSetLayoutCreateInfo
            {
                SType = StructureType.DescriptorSetLayoutCreateInfo,
                BindingCount = (uint)bindings.Length,
                PBindings = (DescriptorSetLayoutBinding*)bindingsHandle.AddrOfPinnedObject()
            };
            vk.CreateDescriptorSetLayout(device, layoutInfo, null, out DescriptorSetLayout layout);
            descriptorSetLayoutList.Add(layout);
            bindingsHandle.Free();

            // Allocate Descriptor Sets
            GCHandle layoutsHandle = GCHandle.Alloc(descriptorSetLayoutList.ToArray(), GCHandleType.Pinned);
            DescriptorSetAllocateInfo allocInfo = new DescriptorSetAllocateInfo
            {
                SType = StructureType.DescriptorSetAllocateInfo,
                DescriptorPool = descriptorPool,
                DescriptorSetCount = 1,
                PSetLayouts = (DescriptorSetLayout*)layoutsHandle.AddrOfPinnedObject()
            };
            DescriptorSet set;
            vk.AllocateDescriptorSets(device, allocInfo, out set);
            descriptorSetList.Add(set);
            layoutsHandle.Free();

            // Update Descriptor Sets
            var textureInfos = GetTexturePropertiesBuffer(TextureList);
            WriteDescriptorSet[] writes = new[]
            {
                new WriteDescriptorSet
                {
                    SType = StructureType.WriteDescriptorSet,
                    DstSet = descriptorSetList[0],
                    DstBinding = 0,
                    DescriptorCount = 1,
                    DescriptorType = DescriptorType.CombinedImageSampler,
                    PImageInfo = (DescriptorImageInfo*)textureInfos.Ptr
                }
            };
            GCHandle writesHandle = GCHandle.Alloc(writes, GCHandleType.Pinned);
            vk.UpdateDescriptorSets(device, (uint)writes.Length, (WriteDescriptorSet*)writesHandle.AddrOfPinnedObject(), 0, null);
            writesHandle.Free();

            // Pipeline Layout
            PipelineLayoutCreateInfo pipelineLayoutInfo = new PipelineLayoutCreateInfo
            {
                SType = StructureType.PipelineLayoutCreateInfo,
                SetLayoutCount = 1,
                PSetLayouts = descriptorSetLayoutList.Ptr
            };
            vk.CreatePipelineLayout(device, pipelineLayoutInfo, null, out pipelineLayout);

            // Graphics Pipeline
            ShaderModule vertShader = CreateShaderModule("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Shaders\\FrameBufferShaderVert.spv");
            ShaderModule fragShader = CreateShaderModule("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Shaders\\FrameBufferShaderFrag.spv");

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

            PipelineVertexInputStateCreateInfo vertexInputInfo = new PipelineVertexInputStateCreateInfo
            {
                SType = StructureType.PipelineVertexInputStateCreateInfo,
                VertexBindingDescriptionCount = 0,
                PVertexBindingDescriptions = null,
                VertexAttributeDescriptionCount = 0,
                PVertexAttributeDescriptions = null
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
                Layout = pipelineLayout,
                RenderPass = renderPass,
                Subpass = 0
            };

            vk.CreateGraphicsPipelines(device, pipelineCache, 1, pipelineInfo, null, out pipeline);
            stagesHandle.Free();
            vertMainHandle.Free();
            fragMainHandle.Free();

            vk.DestroyShaderModule(device, vertShader, null);
            vk.DestroyShaderModule(device, fragShader, null);
        }

        public void Update(float deltaTime)
        {
            DestroyDeadGameObjects();
            VkCommandBuffer commandBuffer = VulkanRenderer.BeginSingleUseCommandBuffer(); // Assumes updated to return Silk.NET type
            foreach (var obj in GameObjectList)
            {
                obj.Update(commandBuffer, deltaTime);
            }
            foreach (var spriteLayer in SpriteLayerList)
            {
                spriteLayer.Update(commandBuffer, deltaTime);
            }
            VulkanRenderer.EndSingleUseCommandBuffer(commandBuffer);
        }

        private void DestroyDeadGameObjects()
        {
            if (!GameObjectList.Any())
            {
                return;
            }

            var deadGameObjectList = GameObjectList.Where(x => x.GameObjectAlive == false).ToList();
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
            foreach (var gameObject in GameObjectList.Where(x => x.GameObjectAlive == false))
            {
                GameObjectList.Remove(gameObject);
            }
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
            CommandBuffer commandBuffer2 = new CommandBuffer(commandBuffer);

            ImageMemoryBarrier barrier = new ImageMemoryBarrier
            {
                SType = StructureType.ImageMemoryBarrier,
                OldLayout = Silk.NET.Vulkan.ImageLayout.Undefined,
                NewLayout = Silk.NET.Vulkan.ImageLayout.ShaderReadOnlyOptimal,
                SrcQueueFamilyIndex = VulkanConst.VK_QUEUE_FAMILY_IGNORED,
                DstQueueFamilyIndex = VulkanConst.VK_QUEUE_FAMILY_IGNORED,
                Image = new Image((ulong)texture.Image), // Assumes Image is updated to Silk.NET type
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
                commandBuffer2,
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

        private ListPtr<DescriptorImageInfo> GetTexturePropertiesBuffer(List<Texture> textureList)
        {
            ListPtr<DescriptorImageInfo> texturePropertiesBuffer = new ListPtr<DescriptorImageInfo>();
            foreach (var texture in textureList)
            {
                var sdf = texture.GetTexturePropertiesBuffer();
                var a = new DescriptorImageInfo
                {
                    Sampler = new Sampler((ulong)texture.Sampler),
                    ImageView = new ImageView((ulong)texture.View),
                    ImageLayout = Silk.NET.Vulkan.ImageLayout.ShaderReadOnlyOptimal
                };
                texturePropertiesBuffer.Add(a); // Assumes updated to Silk.NET type
            }
            return texturePropertiesBuffer;
        }

        private ListPtr<DescriptorBufferInfo> GetMaterialPropertiesBuffer(List<Material> materialList)
        {
            ListPtr<DescriptorBufferInfo> materialPropertiesBuffer = new ListPtr<DescriptorBufferInfo>();
            foreach (var material in materialList)
            {
                var sdf = material.GetMaterialPropertiesBuffer();
                var a = new DescriptorBufferInfo
                {
                    Buffer = new Silk.NET.Vulkan.Buffer((ulong)sdf.buffer),
                    Offset = 0,
                    Range = VulkanConst.VK_WHOLE_SIZE

                };
                materialPropertiesBuffer.Add(a); // Assumes updated to Silk.NET type
            }
            return materialPropertiesBuffer;
        }

        public CommandBuffer Draw(List<GameObject> meshList, SceneDataBuffer sceneDataBuffer)
        {
            var commandIndex = VulkanRenderer.CommandIndex % 3;
            var imageIndex = VulkanRenderer.ImageIndex % 3;
            var commandBuffer = new CommandBuffer(commandBufferList[(int)commandIndex]);

            ClearValue[] clearValues = new[] { new ClearValue { Color = new ClearColorValue(0, 0, 0, 1) } };
            GCHandle clearValuesHandle = GCHandle.Alloc(clearValues, GCHandleType.Pinned);
            RenderPassBeginInfo renderPassInfo = new RenderPassBeginInfo
            {
                SType = StructureType.RenderPassBeginInfo,
                RenderPass = renderPass,
                Framebuffer = FrameBufferList[(int)imageIndex],
                ClearValueCount = (uint)clearValues.Length,
                PClearValues = (ClearValue*)clearValuesHandle.AddrOfPinnedObject(),
                RenderArea = new Rect2D(new Offset2D(0, 0), new Extent2D(VulkanRenderer.SwapChain.SwapChainResolution.width, VulkanRenderer.SwapChain.SwapChainResolution.height)) // Assumes SwapChainResolution is updated
            };

            Viewport viewport = new Viewport
            {
                X = 0.0f,
                Y = 0.0f,
                Width = (float)RenderPassResolution.x,
                Height = (float)RenderPassResolution.y,
                MinDepth = 0.0f,
                MaxDepth = 1.0f
            };

            Rect2D scissor = new Rect2D
            {
                Offset = new Offset2D(0, 0),
                Extent = new Extent2D(VulkanRenderer.SwapChain.SwapChainResolution.width, VulkanRenderer.SwapChain.SwapChainResolution.height) // Assumes updated
            };

            CommandBufferBeginInfo commandInfo = new CommandBufferBeginInfo
            {
                SType = StructureType.CommandBufferBeginInfo,
                Flags = CommandBufferUsageFlags.SimultaneousUseBit
            };

            vk.ResetCommandBuffer(commandBuffer, 0);
            vk.BeginCommandBuffer(commandBuffer, commandInfo);
            vk.CmdBeginRenderPass(commandBuffer, renderPassInfo, SubpassContents.Inline);
            vk.CmdSetViewport(commandBuffer, 0, 1, viewport);
            vk.CmdSetScissor(commandBuffer, 0, 1, scissor);
            vk.CmdBindPipeline(commandBuffer, PipelineBindPoint.Graphics, pipeline);
            vk.CmdBindDescriptorSets(commandBuffer, PipelineBindPoint.Graphics, pipelineLayout, 0, 1, descriptorSetList.Ptr, 0, null);
            vk.CmdDraw(commandBuffer, 6, 1, 0, 0);
            vk.CmdEndRenderPass(commandBuffer);
            vk.EndCommandBuffer(commandBuffer);

            clearValuesHandle.Free();
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
            vk.DestroyPipelineLayout(device, pipelineLayout, null);
            foreach (var layout in descriptorSetLayoutList)
            {
                vk.DestroyDescriptorSetLayout(device, layout, null);
            }
            vk.DestroyDescriptorPool(device, descriptorPool, null);
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