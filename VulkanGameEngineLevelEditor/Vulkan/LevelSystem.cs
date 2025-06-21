using GlmSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Models;

namespace VulkanGameEngineLevelEditor.Vulkan
{
    public class GameObjectLoader
    {
        public string GameObjectPath { get; set; }
        public List<double> GameObjectPositionOverride { get; set; }
    }

    public class LevelLoader
    {
        public string LevelID { get; set; }
        public List<string> LoadTextures { get; set; }
        public List<string> LoadMaterials { get; set; }
        public List<string> LoadSpriteVRAM { get; set; }
        public List<string> LoadTileSetVRAM { get; set; }
        public List<GameObjectLoader> GameObjectList { get; set; }
        public string LoadLevelLayout { get; set; }
    }

    public static unsafe class LevelSystem
    {
        public static Guid levelRenderPass2DId { get; private set; }
        public static Guid spriteRenderPass2DId { get; private set; }
        public static Guid frameBufferId { get; private set; }
        public static OrthographicCamera2D camera { get; set; }

        public static void LoadLevel(String levelPath)
        {
            string levelDirectory = Path.GetDirectoryName(levelPath);
            string jsonContent = File.ReadAllText(levelPath);
            LevelLoader levelLoader = JsonConvert.DeserializeObject<LevelLoader>(jsonContent);

            foreach(var texturePath in levelLoader.LoadTextures)
            {
                string fullTexturePath = Path.GetFullPath(Path.Combine(levelDirectory, texturePath));
                TextureSystem.LoadTexture(fullTexturePath);
            }
            foreach (var materialPath in levelLoader.LoadMaterials)
            {
                string fullMaterialPath = Path.GetFullPath(Path.Combine(levelDirectory, materialPath));
                MaterialSystem.LoadMaterial(fullMaterialPath);
            }

            ListPtr<Texture> textureList = new ListPtr<Texture>(32);
            Texture depthTexture = new Texture();
            ulong count = (ulong)textureList.Count;
            GraphicsRenderer renderSystemStruct = RenderSystem.renderer;
            string jsonText = "C:/Users/dotha/Documents/GitHub/VulkanGameEngine/RenderPass/LevelShader2DRenderPass.json";
            VkExtent2D asdf = RenderSystem.SwapChainResolution;
            Texture sf = textureList[0];

            //string jsonContent = File.ReadAllText("C:/Users/dotha/Documents/GitHub/VulkanGameEngine/Pipelines/Default2DPipeline.json");
            //RenderPipelineDLL pipelineModel = JsonConvert.DeserializeObject<RenderPipelineModel>(jsonContent).ToDLL();
            //ivec2 renderPassRes = new ivec2((int)RenderSystem.renderer.SwapChainResolution.width, (int)RenderSystem.renderer.SwapChainResolution.height);

            //VulkanRenderPass renderPass = GameEngineImport.VulkanRenderPass_CreateVulkanRenderPass(ref renderSystemStruct, jsonText, ref asdf, sizeof(SceneDataBuffer), ref sf, ref count, ref depthTexture);
            //VulkanPipeline pipeline = GameEngineImport.VulkanPipeline_CreateRenderPipeline(RenderSystem.renderer.Device, renderPass.RenderPassId, 0, pipelineModel, renderPass.RenderPass, sizeof(SceneDataBuffer), renderPassRes, includes, )

            // Use renderPass as needed

            //var res = new vec2((float)RenderSystem.SwapChainResolution.width, (float)RenderSystem.SwapChainResolution.height);
            //var pos = new vec2(0.0f, 0.0f);
            //camera = new OrthographicCamera2D(res, pos);

            //string jsonContent = File.ReadAllText(levelPath);
            //LevelLoader levelLoader = JsonConvert.DeserializeObject<LevelLoader>(levelPath);

            //foreach (var texturePath in levelLoader.TextureList)
            //{
            //    TextureSystem.LoadTexture(texturePath);
            //}
            //foreach (var texturePath in levelLoader.TextureList)
            //{
            //    TextureSystem.LoadTexture(texturePath);
            //}
            //foreach (var texturePath in levelLoader.TextureList)
            //{
            //    TextureSystem.LoadTexture(texturePath);
            //}
            //foreach (var texturePath in levelLoader.TextureList)
            //{
            //    TextureSystem.LoadTexture(texturePath);
            //}
        }
    }
}
