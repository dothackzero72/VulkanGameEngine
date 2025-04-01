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
using System.Numerics;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class Level2DRenderer : JsonRenderPass<Vertex2D>
    {
        private Vk vk = Vk.GetApi();
        private VkDevice device => VulkanRenderer.device;
        public List<SpriteBatchLayer> SpriteLayerList { get; private set; } = new List<SpriteBatchLayer>();
        public List<GameObject> GameObjectList { get; private set; } = new List<GameObject>();
        public List<Texture> TextureList { get; private set; } = new List<Texture>();
        public List<Material> MaterialList { get; private set; } = new List<Material>();
        public RenderedTexture texture { get; private set; }

        public Level2DRenderer(string json, ivec2 renderPassResolution)
        {
            RenderPassBuildInfoModel model = new RenderPassBuildInfoModel(json);
            RenderPassBuildInfoDLL modelDLL = new RenderPassBuildInfoModel(json).ToDLL();
            foreach (var item in model.RenderedTextureInfoModelList)
            {
                item.ImageCreateInfo.extent = new VkExtent3DModel
                {
                    width = (uint)renderPassResolution.x,
                    height = (uint)renderPassResolution.y,
                    depth = 1
                };
            }

            RenderPassResolution = renderPassResolution;
            foreach (RenderedTextureInfoModel renderedTextureInfoModel in model.RenderedTextureInfoModelList)
            {
                switch (renderedTextureInfoModel.TextureType)
                {
                    case RenderedTextureType.ColorRenderedTexture: RenderedColorTextureList.Add(new RenderedTexture(VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, renderedTextureInfoModel.ImageCreateInfo.Convert(), renderedTextureInfoModel.SamplerCreateInfo.Convert())); break;
                    case RenderedTextureType.InputAttachmentTexture: RenderedColorTextureList.Add(new RenderedTexture(VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, renderedTextureInfoModel.ImageCreateInfo.Convert(), renderedTextureInfoModel.SamplerCreateInfo.Convert())); break;
                    case RenderedTextureType.ResolveAttachmentTexture: RenderedColorTextureList.Add(new RenderedTexture(VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, renderedTextureInfoModel.ImageCreateInfo.Convert(), renderedTextureInfoModel.SamplerCreateInfo.Convert())); break;
                    case RenderedTextureType.DepthRenderedTexture: depthTexture = new DepthTexture(VkImageAspectFlagBits.VK_IMAGE_ASPECT_DEPTH_BIT, renderedTextureInfoModel.ImageCreateInfo.Convert(), renderedTextureInfoModel.SamplerCreateInfo.Convert()); break;
                    default:
                        {
                            throw new Exception("Case doesn't exist: RenderedTextureType");
                        }
                };
            }

            ListPtr<VkImageView> imageViews = new ListPtr<VkImageView>(RenderedColorTextureList.Select(x => x.View).ToList());
            VkImageView depthView = depthTexture.View;

            VulkanRenderer.CreateCommandBuffers(commandBufferList);
            renderPass = GameEngineImport.DLL_RenderPass_BuildRenderPass(VulkanRenderer.device, modelDLL);
            FrameBufferList = new ListPtr<VkFramebuffer>(GameEngineImport.DLL_RenderPass_BuildFrameBuffer(device, renderPass, modelDLL, imageViews.Ptr, &depthView, VulkanRenderer.SwapChain.imageViews.Ptr, VulkanRenderer.SwapChain.imageViews.UCount, imageViews.UCount, new ivec2((int)VulkanRenderer.SwapChain.SwapChainResolution.width, (int)VulkanRenderer.SwapChain.SwapChainResolution.height)), VulkanRenderer.SwapChain.imageViews.UCount);
            StartLevelRenderer();
        }

        private void CreateHardcodedRenderPass()
        {
            VkAttachmentDescription[] attachmentDescriptionList = new[]
            {
                new VkAttachmentDescription
                {
                    format = (VkFormat)Format.R8G8B8A8Unorm,
                    samples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT,
                    loadOp = (VkAttachmentLoadOp)AttachmentLoadOp.Clear,
                    storeOp = (VkAttachmentStoreOp)AttachmentStoreOp.Store,
                    stencilLoadOp = (VkAttachmentLoadOp)AttachmentLoadOp.DontCare,
                    stencilStoreOp = (VkAttachmentStoreOp)AttachmentStoreOp.DontCare,
                    initialLayout = (VkImageLayout)ImageLayout.Undefined,
                    finalLayout = (VkImageLayout)ImageLayout.PresentSrcKhr
                },
               new VkAttachmentDescription
                {
                    format = (VkFormat)Format.D32Sfloat,
                    samples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT,
                    loadOp = (VkAttachmentLoadOp)AttachmentLoadOp.Clear,
                    storeOp = (VkAttachmentStoreOp)AttachmentStoreOp.DontCare,
                    stencilLoadOp = (VkAttachmentLoadOp)AttachmentLoadOp.DontCare,
                    stencilStoreOp = (VkAttachmentStoreOp)AttachmentStoreOp.DontCare,
                    initialLayout = (VkImageLayout)ImageLayout.Undefined,
                    finalLayout = (VkImageLayout)ImageLayout.DepthStencilAttachmentOptimal
                }
            };

            VkAttachmentReference[] colorRefsList = new[]
            {
                new VkAttachmentReference
                {
                    attachment = 0,
                    layout = (VkImageLayout)ImageLayout.ColorAttachmentOptimal
                },
                new VkAttachmentReference
                {
                    attachment = 1,
                    layout = (VkImageLayout)ImageLayout.DepthAttachmentOptimal
                }
            };

            VkSubpassDescription[] subpassDescriptionList = new[]
            {
                new VkSubpassDescription
                {
                    pipelineBindPoint = (VkPipelineBindPoint)PipelineBindPoint.Graphics,
                    colorAttachmentCount = 1,
                    pColorAttachments = null,
                    pResolveAttachments = null,
                    pDepthStencilAttachment = null
                }
            };

            VkSubpassDependency[] subpassDependencyList = new[]
            {
                new VkSubpassDependency
                {
                    srcSubpass = VulkanConst.VK_SUBPASS_EXTERNAL,
                    dstSubpass = 0,
                    srcStageMask = (VkPipelineStageFlagBits)PipelineStageFlags.ColorAttachmentOutputBit,
                    dstStageMask = (VkPipelineStageFlagBits)PipelineStageFlags.ColorAttachmentOutputBit,
                    srcAccessMask = 0,
                    dstAccessMask = (VkAccessFlagBits)AccessFlags.ColorAttachmentWriteBit
                }
            };

            fixed (VkAttachmentDescription* attachments = attachmentDescriptionList)
            fixed (VkAttachmentReference* colorRefs = colorRefsList)
            fixed (VkSubpassDescription* subpasses = subpassDescriptionList)
            fixed (VkSubpassDependency* dependencies = subpassDependencyList)
            {
                subpasses[0].pColorAttachments = colorRefs;

                VkRenderPassCreateInfo renderPassInfo = new VkRenderPassCreateInfo
                {
                    sType = (VkStructureType)StructureType.RenderPassCreateInfo,
                    pNext = null,
                    flags = 0,
                    attachmentCount = (uint)attachmentDescriptionList.Length,
                    pAttachments = attachments,
                    subpassCount = (uint)subpassDescriptionList.Length,
                    pSubpasses = subpasses,
                    dependencyCount = (uint)subpassDependencyList.Length,
                    pDependencies = dependencies
                };

                VkResult result = VkFunc.vkCreateRenderPass(device, &renderPassInfo, null, out VkRenderPass renderPass2);
                if (result != VkResult.VK_SUCCESS)
                {
                    throw new Exception($"vkCreateRenderPass failed: {result}");
                }

                renderPass = renderPass2;
            }
        }

        private void CreateHardcodedFramebuffers()
        {
            for (int i = 0; i < VulkanRenderer.SwapChain.ImageCount; i++)
            {
                ListPtr<VkImageView> attachments = new ListPtr<VkImageView> { texture.View, depthTexture.View };
                VkFramebufferCreateInfo fbInfo = new VkFramebufferCreateInfo
                {
                    sType = (VkStructureType)StructureType.FramebufferCreateInfo,
                    renderPass = renderPass,
                    attachmentCount = attachments.UCount,
                    pAttachments = attachments.Ptr,
                    width = (uint)RenderPassResolution.x,
                    height = (uint)RenderPassResolution.y,
                    layers = 1
                };

                VkFunc.vkCreateFramebuffer(device, &fbInfo, null, out VkFramebuffer fb);
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

            CreateHardcodedPipeline(model);
            SpriteLayerList.Add(new SpriteBatchLayer(GameObjectList, jsonPipelineList[0]));
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


            var vertexBinding = NullVertex.GetBindingDescriptions();
            foreach (var instanceVar in SpriteInstanceVertex2D.GetBindingDescriptions())
            {
                vertexBinding.Add(instanceVar);
            }

            var vertexAttribute = NullVertex.GetAttributeDescriptions();
            foreach (var instanceVar in SpriteInstanceVertex2D.GetAttributeDescriptions())
            {
                vertexAttribute.Add(instanceVar);
            }

            GPUImport<Vertex2D> gpuImport = new GPUImport<Vertex2D>
            {
                MeshList = new List<Mesh<Vertex2D>>(GetMeshFromGameObjects()),
                TextureList = new List<Texture>(TextureList),
                MaterialList = new List<Material>(MaterialList)
            };

            jsonPipelineList.Add(new JsonPipeline<Vertex2D>("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Pipelines\\Default2DPipeline.json", renderPass, (uint)sizeof(SceneDataBuffer), vertexBinding, vertexAttribute, gpuImport));
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

        private VkShaderModule CreateShaderModule(string filePath)
        {
            VkShaderModule module = new VkShaderModule();
            byte[] code = System.IO.File.ReadAllBytes(filePath);
            GCHandle codeHandle = GCHandle.Alloc(code, GCHandleType.Pinned);
            VkShaderModuleCreateInfo createInfo = new VkShaderModuleCreateInfo
            {
                sType = (VkStructureType)StructureType.ShaderModuleCreateInfo,
                codeSize = (nint)code.Length,
                pCode = (uint*)codeHandle.AddrOfPinnedObject()
            };
            VkFunc.vkCreateShaderModule(device, &createInfo, null, &module);
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
            var commandBuffer = commandBufferList[(int)commandIndex];

            using ListPtr<VkClearValue> clearValues = new ListPtr<VkClearValue>();
            clearValues.Add(new VkClearValue { Color = new VkClearColorValue(0, 0, 0, 1) });
            clearValues.Add(new VkClearValue { DepthStencil = new VkClearDepthStencilValue(0.0f, 1.0f) });

            VkRenderPassBeginInfo renderPassInfo = new VkRenderPassBeginInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
                renderPass = renderPass,
                framebuffer = FrameBufferList[(int)imageIndex],
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
                obj.Draw(commandBuffer, sceneDataBuffer);
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
           // vk.DestroyPipeline(device, pipeline, null);
            //vk.DestroyPipelineLayout(device, new PipelineLayout((ulong?)pipelineLayout), null);
            //foreach (var layout in descriptorSetLayoutList)
            //{
            //    // vk.DestroyDescriptorSetLayout(device, layout, null);
            //}
            //// vk.DestroyDescriptorPool(device, descriptorPool, null);
            //foreach (var fb in FrameBufferList)
            //{
            //    vk.DestroyFramebuffer(device, fb, null);
            //}
            //vk.DestroyRenderPass(device, new RenderPass((ulong?)renderPass), null);

            FrameBufferList.Dispose();
            commandBufferList.Dispose();
            //descriptorSetLayoutList.Dispose();
          //  descriptorSetList.Dispose();
        }
    }
}