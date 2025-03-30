using AutoMapper;
using CSScripting;
using GlmSharp;
using Newtonsoft.Json;
using Silk.NET.SDL;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineGameObjectScripts.Interface;
using VulkanGameEngineGameObjectScripts.Vulkan;
using VulkanGameEngineLevelEditor.Components;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.RenderPassEditor;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class Level2DRenderer : JsonRenderPass<Vertex2D>
    {
        //private readonly IMapper _mapper;
        public List<SpriteBatchLayer> SpriteLayerList { get; private set; } = new List<SpriteBatchLayer>();
        public List<GameObject> GameObjectList { get; private set; } = new List<GameObject>();
        public List<Texture> TextureList { get; private set; } = new List<Texture>();
        public List<Material> MaterialList { get; private set; } = new List<Material>();

        public Level2DRenderer()
        {

        }

        public Level2DRenderer(String jsonPath, ivec2 renderPassResolution, VkSampleCountFlagBits sampleCount)
        {
            //_mapper = mapper;
            RenderPassResolution = renderPassResolution;
            SampleCountFlags = sampleCount;

            string jsonContent = File.ReadAllText(jsonPath);
            RenderPassBuildInfoModel model = JsonConvert.DeserializeObject<RenderPassBuildInfoModel>(jsonContent);
            RenderPassBuildInfoDLL modelDLL = JsonConvert.DeserializeObject<RenderPassBuildInfoModel>(jsonContent).ToDLL();
            foreach (var item in model.RenderedTextureInfoModelList)
            {
                item.ImageCreateInfo.extent = new VkExtent3DModel
                {
                    width = (uint)renderPassResolution.x,
                    height = (uint)renderPassResolution.y,
                    depth = 1
                };
            }

            RenderedColorTextureList.Add(new RenderedTexture(VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, model.RenderedTextureInfoModelList[0].ImageCreateInfo.Convert(), model.RenderedTextureInfoModelList[0].SamplerCreateInfo.Convert()));
            depthTexture = new DepthTexture(VkImageAspectFlagBits.VK_IMAGE_ASPECT_DEPTH_BIT, model.RenderedTextureInfoModelList[1].ImageCreateInfo.Convert(), model.RenderedTextureInfoModelList[1].SamplerCreateInfo.Convert());


            //FrameBufferList = new ListPtr<nint>(3);
            //var depthView = depthTexture.View;

            CreateHardcodedRenderPass();
            CreateHardcodedFramebuffers();
            VulkanRenderer.CreateCommandBuffers(commandBufferList);

        }

        public void StartLeveleRenderer()
        {
            TextureList.Add(new Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Textures\\MegaMan_diffuse.bmp", VkFormat.VK_FORMAT_R8G8B8A8_SRGB, VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, TextureTypeEnum.kType_DiffuseTextureMap, false));
            MaterialList.Add(new Material("Material1"));
            MaterialList.Last().SetAlbedoMap(TextureList[0]);

            ivec2 size = new ivec2(32);
            SpriteSheet spriteSheet = new SpriteSheet(MaterialList[0], size, 0);

            AddGameObject("Obj1", new List<ComponentTypeEnum> { ComponentTypeEnum.kTransform2DComponent, ComponentTypeEnum.kSpriteComponent }, spriteSheet, new vec2(960.0f, 540.0f));
            AddGameObject("Obj2", new List<ComponentTypeEnum> { ComponentTypeEnum.kTransform2DComponent, ComponentTypeEnum.kSpriteComponent }, spriteSheet, new vec2(300.0f, 20.0f));
            AddGameObject("Obj3", new List<ComponentTypeEnum> { ComponentTypeEnum.kTransform2DComponent, ComponentTypeEnum.kSpriteComponent }, spriteSheet, new vec2(300.0f, 80.0f));

            jsonPipelineList.Add(new JsonPipeline<Vertex2D>());
            SpriteLayerList.Add(new SpriteBatchLayer(GameObjectList, jsonPipelineList[0]));
            GPUImport<Vertex2D> gpuImport = new GPUImport<Vertex2D>
            {
                MeshList = new List<Mesh<Vertex2D>>(GetMeshFromGameObjects()),
                TextureList = new List<Texture>(TextureList),
                MaterialList = new List<Material>(MaterialList)
            };

            ListPtr<VkVertexInputBindingDescription> vertexBinding = NullVertex.GetBindingDescriptions();
            foreach (var instanceVar in SpriteInstanceVertex2D.GetBindingDescriptions())
            {
                vertexBinding.Add(instanceVar);
            }

            ListPtr<VkVertexInputAttributeDescription> vertexAttribute = NullVertex.GetAttributeDescriptions();
            foreach (var instanceVar in SpriteInstanceVertex2D.GetAttributeDescriptions())
            {
                vertexAttribute.Add(instanceVar);
            }

            jsonPipelineList[0] = new JsonPipeline<Vertex2D>("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Pipelines\\Default2DPipeline.json", renderPass, (uint)sizeof(SceneDataBuffer), vertexBinding, vertexAttribute, gpuImport);
            SpriteLayerList[0].SpriteRenderPipeline = jsonPipelineList[0];
        }

        private void CreateHardcodedRenderPass()
        {
            VkAttachmentDescription colorAttachment = new VkAttachmentDescription
            {
                format = VkFormat.VK_FORMAT_R8G8B8A8_UNORM,
                samples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT,
                loadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_CLEAR,
                storeOp = VkAttachmentStoreOp.VK_ATTACHMENT_STORE_OP_STORE,
                stencilLoadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_DONT_CARE,
                stencilStoreOp = VkAttachmentStoreOp.VK_ATTACHMENT_STORE_OP_DONT_CARE,
                initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED,
                finalLayout = VkImageLayout.VK_IMAGE_LAYOUT_PRESENT_SRC_KHR
            };

            VkAttachmentDescription depthAttachment = new VkAttachmentDescription
            {
                format = VkFormat.VK_FORMAT_D32_SFLOAT,
                samples = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT,
                loadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_CLEAR,
                storeOp = VkAttachmentStoreOp.VK_ATTACHMENT_STORE_OP_DONT_CARE,
                stencilLoadOp = VkAttachmentLoadOp.VK_ATTACHMENT_LOAD_OP_DONT_CARE,
                stencilStoreOp = VkAttachmentStoreOp.VK_ATTACHMENT_STORE_OP_DONT_CARE,
                initialLayout = VkImageLayout.VK_IMAGE_LAYOUT_UNDEFINED,
                finalLayout = VkImageLayout.VK_IMAGE_LAYOUT_DEPTH_STENCIL_ATTACHMENT_OPTIMAL
            };

            VkAttachmentReference colorAttachmentRef = new VkAttachmentReference
            {
                attachment = 0,
                layout = VkImageLayout.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL
            };

            VkAttachmentReference depthAttachmentRef = new VkAttachmentReference
            {
                attachment = 1,
                layout = VkImageLayout.VK_IMAGE_LAYOUT_DEPTH_STENCIL_ATTACHMENT_OPTIMAL
            };

            VkSubpassDescription subpass = new VkSubpassDescription
            {
                pipelineBindPoint = VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS,
                colorAttachmentCount = 1,
                pColorAttachments = &colorAttachmentRef,
                pDepthStencilAttachment = &depthAttachmentRef
            };

            VkSubpassDependency dependency = new VkSubpassDependency
            {
                srcSubpass = VulkanConst.VK_SUBPASS_EXTERNAL,
                dstSubpass = 0,
                srcStageMask = VkPipelineStageFlagBits.COLOR_ATTACHMENT_OUTPUT_BIT,
                dstStageMask = VkPipelineStageFlagBits.COLOR_ATTACHMENT_OUTPUT_BIT,
                srcAccessMask = 0,
                dstAccessMask = VkAccessFlagBits.VK_ACCESS_COLOR_ATTACHMENT_WRITE_BIT
            };

            VkAttachmentDescription[] attachments = new[] { colorAttachment, depthAttachment };
            GCHandle attachmentsHandle = GCHandle.Alloc(attachments, GCHandleType.Pinned);
            VkRenderPassCreateInfo renderPassInfo = new VkRenderPassCreateInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_RENDER_PASS_CREATE_INFO,
                attachmentCount = 2,
                pAttachments = (VkAttachmentDescription*)attachmentsHandle.AddrOfPinnedObject(),
                subpassCount = 1,
                pSubpasses = &subpass,
                dependencyCount = 1,
                pDependencies = &dependency
            };

            VkFunc.vkCreateRenderPass(VulkanRenderer.device, &renderPassInfo, null, out VkRenderPass renderPass2);
            renderPass = renderPass2;
            attachmentsHandle.Free();
        }

        private void CreateHardcodedFramebuffers()
        {
            for (int i = 0; i < VulkanRenderer.SwapChain.ImageCount; i++) // 3 images
            {
                nint[] attachments = new nint[]
                {
                    RenderedColorTextureList[0].View,
                    depthTexture.View
                };

                GCHandle attachmentsHandle = GCHandle.Alloc(attachments, GCHandleType.Pinned);
                VkFramebufferCreateInfo fbInfo = new VkFramebufferCreateInfo
                {
                    sType = VkStructureType.VK_STRUCTURE_TYPE_FRAMEBUFFER_CREATE_INFO,
                    renderPass = renderPass,
                    attachmentCount = 2,
                    pAttachments = (nint*)attachmentsHandle.AddrOfPinnedObject(),
                    width = (uint)RenderPassResolution.x,
                    height = (uint)RenderPassResolution.y,
                    layers = 1
                };

                VkFunc.vkCreateFramebuffer(VulkanRenderer.device, &fbInfo, null, out VkFramebuffer fb);
                FrameBufferList.Add(fb);
                attachmentsHandle.Free();
            }
        }

        public virtual void Input(float deltaTime)
        {
        }

        public override void Update(float deltaTime)
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

        public void UpdateBufferIndex()
        {
            for (int x = 0; x < TextureList.Count(); x++)
            {
                TextureList[x].UpdateTextureBufferIndex((uint)x);
            }
            for (int x = 0; x < MaterialList.Count(); x++)
            {
                MaterialList[x].UpdateMaterialBufferIndex((uint)x);
            }
        }

        public override VkCommandBuffer Draw(List<GameObject> meshList, SceneDataBuffer sceneDataBuffer)
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
            // Remove this: VkFunc.vkCmdBindPipeline(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, jsonPipelineList[0].pipeline);
            foreach (var obj in SpriteLayerList)
            {
                obj.Draw(commandBuffer, sceneDataBuffer);
            }
            VkFunc.vkCmdEndRenderPass(commandBuffer);
            VkFunc.vkEndCommandBuffer(commandBuffer);

            return commandBuffer;
        }

        public override void Destroy()
        {
            foreach (var gameObject in GameObjectList)
            {
                gameObject.Destroy();
            }
            foreach (var spriteLayer in SpriteLayerList)
            {
                spriteLayer.Destroy();
            }
            foreach (var texture in TextureList)
            {
                texture.Destroy();
            }
            foreach (var material in MaterialList)
            {
                material.Destroy();
            }
            base.Destroy();
        }

        private void AddGameObject(string name, List<ComponentTypeEnum> gameObjectComponentTypeList, SpriteSheet spriteSheet, vec2 objectPosition)
        { 
            GameObjectList.Add(new GameObject(name, new List<ComponentTypeEnum>
                                             {
                                                ComponentTypeEnum.kTransform2DComponent,
                                                ComponentTypeEnum.kSpriteComponent
                                             }, spriteSheet));
            var gameObject = GameObjectList.Last();

            List<GameObjectComponent> gameObjectComponentList = gameObject.GameObjectComponentList;
            foreach (var component in gameObjectComponentTypeList)
            {
                switch (component)
                {
                    case ComponentTypeEnum.kTransform2DComponent: gameObject.AddComponent(new Transform2DComponent(gameObject.GameObjectId, objectPosition, name)); break;
                    case ComponentTypeEnum.kInputComponent: gameObject.AddComponent(new InputComponent(gameObject.GameObjectId, name)); break;
                    case ComponentTypeEnum.kSpriteComponent: gameObject.AddComponent(new SpriteComponent(gameObject, name, spriteSheet)); break;
                }
            }
        }

        private void AddTexture()
        {

        }

        private void AddMaterial()
        {

        }

        private void RemoveGameObject(GameObject gameObject)
        {

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

        private List<Mesh2D> GetMeshFromGameObjects()
        {
            var meshList = new List<Mesh2D>();
            foreach (SpriteBatchLayer spriteLayer in SpriteLayerList)
            {
                meshList.Add(spriteLayer.SpriteLayerMesh);
            }
            return meshList;
        }

        private GameObject SearchGameObjectsById(uint id)
        {
            return GameObjectList.Where(x => x.GameObjectId == id).First();
        }
    }
}
