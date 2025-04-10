using GlmSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineLevelEditor.Components;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.RenderPassEditor;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public class Level2D
    {
        [JsonIgnore]
        public Level2DRenderer LevelRenderer { get; set; } = new Level2DRenderer();

        [JsonIgnore]
        public List<SpriteBatchLayer> SpriteLayerList { get; private set; } = new List<SpriteBatchLayer>();
        
        [JsonIgnore]
        public List<Texture> TextureList { get; private set; } = new List<Texture>();

        [JsonIgnore]
        public Texture RenderedLevelTexture { get; private set; } = new Texture();

        public Guid LevelID { get; private set; } = Guid.NewGuid();
        public string LevelName { get; private set; } = string.Empty;
        public List<GameObject> GameObjectList { get; private set; } = new List<GameObject>();
        public List<Material> MaterialList { get; private set; } = new List<Material>();

        public Level2D()
        {

        }

        public Level2D(string levelJsonPath, ivec2 renderPassResolution)
        {
            LevelName = "TestLevel";
            TextureList.Add(new Texture("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Textures\\MegaMan_diffuse.bmp", VkFormat.VK_FORMAT_R8G8B8A8_SRGB, VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, TextureTypeEnum.kType_DiffuseTextureMap, false));
            MaterialList.Add(new Material("Material1"));
            MaterialList.Last().SetAlbedoMap("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Textures\\MegaMan_diffuse.bmp");

            ivec2 size = new ivec2(32);
            SpriteSheet spriteSheet = new SpriteSheet(MaterialList[0], size, 0);

            AddGameObject("Obj1", new List<ComponentTypeEnum> { ComponentTypeEnum.kTransform2DComponent, ComponentTypeEnum.kSpriteComponent }, spriteSheet, new vec2(960.0f, 540.0f));
            AddGameObject("Obj2", new List<ComponentTypeEnum> { ComponentTypeEnum.kTransform2DComponent, ComponentTypeEnum.kSpriteComponent }, spriteSheet, new vec2(300.0f, 20.0f));
            AddGameObject("Obj3", new List<ComponentTypeEnum> { ComponentTypeEnum.kTransform2DComponent, ComponentTypeEnum.kSpriteComponent }, spriteSheet, new vec2(300.0f, 80.0f));

            GPUImport<Vertex2D> gpuImport = new GPUImport<Vertex2D>
            {
                MeshList = new List<Mesh<Vertex2D>>(GetMeshFromGameObjects()),
                TextureList = new List<Texture>(TextureList),
                MaterialList = new List<Material>(MaterialList)
            };

            LevelRenderer = new Level2DRenderer(gpuImport, renderPassResolution);
            RenderedLevelTexture = LevelRenderer.RenderedColorTextureList.First();
            SpriteLayerList.Add(LevelRenderer.AddSpriteLayer(GameObjectList));
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
            LevelRenderer.Update(deltaTime);
        }

        public VkCommandBuffer Draw(SceneDataBuffer sceneDataBuffer)
        {
           return  LevelRenderer.Draw(SpriteLayerList, sceneDataBuffer);
        }

        public void LoadLevel()
        {
            string jsonContent = File.ReadAllText(@$"{ConstConfig.LevelBasePath}\\TestLevel.json");
            List<GameObjectModel> model = JsonConvert.DeserializeObject<List<GameObjectModel>>(jsonContent);

            foreach (GameObjectModel obj in model)
            {
                GameObjectList.Add(new GameObject(obj));
            }
        }

        public void SaveLevel()
        {
            foreach (var material in MaterialList)
            {
                var materialFilePath = @$"{ConstConfig.MaterialBasePath}\\{material.Name}.json";
                if (File.Exists(materialFilePath))
                {
                    string materialJsonContent = File.ReadAllText(materialFilePath);
                    Material model = JsonConvert.DeserializeObject<Material>(materialJsonContent);

                    if (model.MaterialID == material.MaterialID)
                    {
                        string materialJsonString = JsonConvert.SerializeObject(material, Formatting.Indented);
                        File.WriteAllText(materialFilePath, materialJsonString);
                    }
                    else
                    {
                        var x = 0;
                        while (File.Exists(@$"{ConstConfig.MaterialBasePath}\\{material.Name}{x}.json"))
                        {
                            string materialJsonString = JsonConvert.SerializeObject(material, Formatting.Indented);
                            File.WriteAllText(@$"{ConstConfig.MaterialBasePath}\\{material.Name}{x}.json", materialJsonString);
                            x++;
                        }
                    }
                }
                else
                {
                    string jsonString2 = JsonConvert.SerializeObject(material, Formatting.Indented);
                    File.WriteAllText(@$"{ConstConfig.MaterialBasePath}\\{material.Name}.json", jsonString2);
                }
            }
            {
                var levelFilePath = @$"{ConstConfig.LevelBasePath}\\{LevelName}.json";
                if (File.Exists(levelFilePath))
                {
                    string levelJsonContent = File.ReadAllText(levelFilePath);
                    Level2D model = JsonConvert.DeserializeObject<Level2D>(levelJsonContent);

                    if (model.LevelID == LevelID)
                    {
                        string materialJsonString = JsonConvert.SerializeObject(LevelName, Formatting.Indented);
                        File.WriteAllText(levelFilePath, materialJsonString);
                    }
                    else
                    {
                        var x = 0;
                        while (File.Exists(@$"{ConstConfig.LevelBasePath}\\{LevelName}{x}.json"))
                        {
                            string materialJsonString = JsonConvert.SerializeObject(this, Formatting.Indented);
                            File.WriteAllText(@$"{ConstConfig.LevelBasePath}\\{LevelName}{x}.json", materialJsonString);
                            x++;
                        }
                    }
                }
                else
                {
                    string jsonString2 = JsonConvert.SerializeObject(this, Formatting.Indented);
                    File.WriteAllText(@$"{ConstConfig.LevelBasePath}\\{LevelName}.json", jsonString2);
                }
            }
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

           // frameBufferList.Dispose();
           // commandBufferList.Dispose();
            //descriptorSetLayoutList.Dispose();
            //  descriptorSetList.Dispose();
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
                    case ComponentTypeEnum.kTransform2DComponent: gameObject.AddComponent(new Transform2DComponent(gameObject, objectPosition, name)); break;
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
    }
}
