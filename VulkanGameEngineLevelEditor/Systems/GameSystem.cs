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


namespace VulkanGameEngineLevelEditor.Systems
{
    public static class GameSystem
    {
        public static Guid TileSetId { get; set; }
        public static Guid LevelRendererId { get; set; }
        public static Guid SpriteRenderPass2DId { get; set; }
        public static Guid FrameBufferId { get; set; }

        public static unsafe void StartUp(VkQueue window, VkQueue renderAreaHandle)
        {
            RenderSystem.CreateVulkanRenderer(window, renderAreaHandle);
            LevelSystem.LoadLevel("C:/Users/dotha/Documents/GitHub/VulkanGameEngine/Levels/TestLevel.json");
        }

        public static void LoadLevel(string levelPath)
        {
            var res = new vec2(RenderSystem.SwapChainResolution.width, RenderSystem.SwapChainResolution.height);
            var pos = new vec2(0.0f, 0.0f);
            //   OrthographicCamera = new OrthographicCamera2D(res, pos);

            string jsonContent = File.ReadAllText(levelPath);
            LevelLoader levelLoader = JsonConvert.DeserializeObject<LevelLoader>(levelPath);

        }

        public static void Update(float deltaTime)
        {
            LevelSystem.Update(deltaTime);
            TextureSystem.Update(deltaTime);
            MaterialSystem.Update(deltaTime);
            RenderSystem.Update(deltaTime);

            //VkCommandBuffer commandBuffer = RenderSystem.BeginSingleTimeCommands();
            //MeshSystem.Update(deltaTime);
            //RenderSystem.EndSingleTimeCommands(commandBuffer);
        }


        public static void Draw(float deltaTime)
        {
            //RenderSystem.StartFrame();
            //LevelSystem.Draw(CommandBufferSubmitList, deltaTime);
            //CommandBufferSubmitList.emplace_back(ImGui_Draw(renderSystem.renderer, renderSystem.imGuiRenderer));
            //RenderSystem.EndFrame(CommandBufferSubmitList);
            //CommandBufferSubmitList.clear();
        }

        public static void Destroy()
        {
            ////gameObjectSystem.DestroyGameObjects();
            ////meshSystem.DestroyAllGameObjects();
            //TextureSystem.DestroyAllTextures();
            //MaterialSystem.DestroyAllMaterials();
            //LevelSystem.DestoryLevel();
            //MeshSystem.DestroyAllGameObjects();
            //BufferSystem.DestroyAllBuffers();
            //RenderSystem.Destroy();
            //MemorySystem.ReportLeaks();
        }
    }
}
