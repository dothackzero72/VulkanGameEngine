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
        public List<SpriteBatchLayer> SpriteLayerList { get; private set; } = new List<SpriteBatchLayer>();
        public List<GameObject> GameObjectList { get; private set; } = new List<GameObject>();
        public List<Texture> TextureList { get; private set; } = new List<Texture>();
        public List<Material> MaterialList { get; private set; } = new List<Material>();

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

            RenderPassResolution = renderPassResolution;

            VulkanRenderer.CreateCommandBuffers(commandBufferList);
            base.CreateJsonRenderPass(json, renderPassResolution);
            StartLevelRenderer();
        }

        public void StartLevelRenderer()
        {
            string jsonContent = File.ReadAllText(ConstConfig.Default2DPipeline);
            RenderPipelineDLL model = JsonConvert.DeserializeObject<RenderPipelineModel>(jsonContent).ToDLL();

            TextureList.Add(new Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Textures\\MegaMan_diffuse.bmp", VkFormat.VK_FORMAT_R8G8B8A8_SRGB, VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, TextureTypeEnum.kType_DiffuseTextureMap, false));
            MaterialList.Add(new Material("Material1"));
            MaterialList.Last().SetAlbedoMap(TextureList[0]);

            ivec2 size = new ivec2(32);
            SpriteSheet spriteSheet = new SpriteSheet(MaterialList[0], size, 0);

            AddGameObject("Obj1", new List<ComponentTypeEnum> { ComponentTypeEnum.kTransform2DComponent, ComponentTypeEnum.kSpriteComponent }, spriteSheet, new vec2(960.0f, 540.0f));
            AddGameObject("Obj2", new List<ComponentTypeEnum> { ComponentTypeEnum.kTransform2DComponent, ComponentTypeEnum.kSpriteComponent }, spriteSheet, new vec2(300.0f, 20.0f));
            AddGameObject("Obj3", new List<ComponentTypeEnum> { ComponentTypeEnum.kTransform2DComponent, ComponentTypeEnum.kSpriteComponent }, spriteSheet, new vec2(300.0f, 80.0f));

            CreatePipeline(model);
            SpriteLayerList.Add(new SpriteBatchLayer(GameObjectList, jsonPipelineList[0]));

        }

        private void CreatePipeline(RenderPipelineDLL model)
        {
            ListPtr<VkDescriptorBufferInfo> meshPropertiesBuffer = new ListPtr<VkDescriptorBufferInfo>();
            GPUImport<Vertex2D> gpuImport = new GPUImport<Vertex2D>
            {
                MeshList = new List<Mesh<Vertex2D>>(GetMeshFromGameObjects()),
                TextureList = new List<Texture>(TextureList),
                MaterialList = new List<Material>(MaterialList)
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

            jsonPipelineList.Add(new JsonPipeline<Vertex2D>("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Pipelines\\Default2DPipeline.json", renderPass, (uint)sizeof(SceneDataBuffer), vertexBinding, vertexAttribute, gpuImport, RenderPassResolution));
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

        public override VkCommandBuffer Draw(List<GameObject> meshList, SceneDataBuffer sceneDataBuffer)
        {
            var commandIndex = VulkanRenderer.CommandIndex;
            var commandBuffer = commandBufferList[(int)commandIndex];

            using ListPtr<VkClearValue> clearValues = new ListPtr<VkClearValue>();
            clearValues.Add(new VkClearValue { Color = new VkClearColorValue(0, 0, 0, 1) }); 
            clearValues.Add(new VkClearValue { DepthStencil = new VkClearDepthStencilValue(1.0f, 0) });

            VkRenderPassBeginInfo renderPassInfo = new VkRenderPassBeginInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
                renderPass = renderPass,
                framebuffer = frameBufferList[0],
                clearValueCount = clearValues.UCount,
                pClearValues = clearValues.Ptr,
                renderArea = new(new VkOffset2D(0, 0), VulkanRenderer.SwapChain.SwapChainResolution)
            };

            VkCommandBufferBeginInfo commandInfo = new VkCommandBufferBeginInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
                flags = VkCommandBufferUsageFlagBits.VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT
            };

            VkFunc.vkResetCommandBuffer(commandBuffer, 0);
            VkFunc.vkBeginCommandBuffer(commandBuffer, &commandInfo);
            VkFunc.vkCmdBeginRenderPass(commandBuffer, &renderPassInfo, VkSubpassContents.VK_SUBPASS_CONTENTS_INLINE);
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

        public void SaveLevel()
        {
          
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

            frameBufferList.Dispose();
            commandBufferList.Dispose();
            //descriptorSetLayoutList.Dispose();
            //  descriptorSetList.Dispose();
        }
    }
}