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
        public List<SpriteBatchLayer> SpriteLayerList { get; private set; } = new List<SpriteBatchLayer>();
        public List<GameObject> GameObjectList { get; private set; } = new List<GameObject>();
        public List<Texture> TextureList { get; private set; } = new List<Texture>();
        public List<Material> MaterialList { get; private set; } = new List<Material>();

        public Level2DRenderer()
        {

        }

        public Level2DRenderer(String jsonPath, ivec2 renderPassResolution, VkSampleCountFlagBits sampleCount = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT)
        {
            RenderPassResolution = renderPassResolution;
            SampleCountFlags = sampleCount;

            string jsonContent = File.ReadAllText(jsonPath);
            RenderPassBuildInfoModel model = JsonConvert.DeserializeObject<RenderPassBuildInfoModel>(jsonContent);
            foreach (var item in model.RenderedTextureInfoModelList)
            {
                item.ImageCreateInfo.extent = new VkExtent3DModel
                {
                    width = (uint)renderPassResolution.x,
                    height = (uint)renderPassResolution.y,
                    depth = 1
                };
            }

            FrameBufferList = new ListPtr<VkFramebuffer>(VulkanRenderer.SwapChain.ImageCount);
            commandBufferList = new ListPtr<VkCommandBuffer>(VulkanRenderer.SwapChain.ImageCount);

            VulkanRenderer.CreateCommandBuffers(commandBufferList);
            renderPass = CreateRenderPass(model);
            CreateFramebuffer();
        }

        public void StartLeveleRenderer()
        {
            TextureList.Add(new Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Textures\\MegaMan_diffuse.bmp", VkFormat.VK_FORMAT_R8G8B8A8_SRGB, VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, TextureTypeEnum.kType_DiffuseTextureMap, false));
            MaterialList.Add(new Material("Material1"));
            MaterialList.Last().SetAlbedoMap(TextureList[0]);

            ivec2 size = new ivec2(32);
            SpriteSheet spriteSheet = new SpriteSheet(MaterialList[0], size, 0);

            AddGameObject("Obj1", new List<ComponentTypeEnum> { ComponentTypeEnum.kTransform2DComponent, ComponentTypeEnum.kSpriteComponent }, spriteSheet, new vec2(300.0f, 40.0f));
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

            List<VkVertexInputBindingDescription> vertexBinding = Vertex2D.GetBindingDescriptions();
            foreach (var instanceVar in SpriteInstanceVertex2D.GetBindingDescriptions())
            {
                vertexBinding.Add(instanceVar);
            }

            List<VkVertexInputAttributeDescription> vertexAttribute = Vertex2D.GetAttributeDescriptions();
            foreach (var instanceVar in SpriteInstanceVertex2D.GetAttributeDescriptions())
            {
                vertexAttribute.Add(instanceVar);
            }

            jsonPipelineList[0] = new JsonPipeline<Vertex2D>("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Pipelines\\Default2DPipeline.json", renderPass, (uint)sizeof(SceneDataBuffer), vertexBinding, vertexAttribute, gpuImport);
            SpriteLayerList[0].SpriteRenderPipeline = jsonPipelineList[0];
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
            clearValues.Add(new VkClearValue { Color = new VkClearColorValue(1, 0, 0, 1) });
            clearValues.Add(new VkClearValue { DepthStencil = new VkClearDepthStencilValue(0.0f, 1.0f) });


            VkRenderPassBeginInfo renderPassInfo = new VkRenderPassBeginInfo
            {
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

            var descSet = jsonPipelineList[0].descriptorSet;
            var commandInfo = new VkCommandBufferBeginInfo { flags = 0 };

            VkFunc.vkBeginCommandBuffer(commandBuffer, &commandInfo);
            VkFunc.vkCmdBeginRenderPass(commandBuffer, &renderPassInfo, VkSubpassContents.VK_SUBPASS_CONTENTS_INLINE);
            VkFunc.vkCmdSetViewport(commandBuffer, 0, 1, &viewport);
            VkFunc.vkCmdSetScissor(commandBuffer, 0, 1, &scissor);
            VkFunc.vkCmdBindPipeline(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, jsonPipelineList[0].pipeline);
            foreach (var obj in GameObjectList)
            {
                obj.Draw(commandBuffer, jsonPipelineList[0].pipeline, jsonPipelineList[0].pipelineLayout, descSet, sceneDataBuffer);
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
