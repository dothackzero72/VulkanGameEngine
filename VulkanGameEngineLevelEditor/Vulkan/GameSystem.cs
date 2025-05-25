using GlmSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.GameEngineAPI;

namespace VulkanGameEngineLevelEditor.Vulkan
{
    public struct LevelLoader
    {
        public Guid LevelId;
        public List<string> TextureList;
    }

    public static class GameSystem
    {
        public static OrthographicCamera2D OrthographicCamera { get; set; }
        public static Guid TileSetId { get; set; }
        public static Guid LevelRendererId { get; set; }
        public static Guid SpriteRenderPass2DId { get; set; }
        public static Guid FrameBufferId { get; set; }


        public static void StartUp(IntPtr window, IntPtr renderAreaHandle)
        {
            RenderSystem.CreateVulkanRenderer(window, renderAreaHandle);
            LoadLevel("C:/Users/dotha/Documents/GitHub/VulkanGameEngine/Levels/TestLevel.json");
        }
        public static void LoadLevel(String levelPath)
        {
            var res = new vec2((float)RenderSystem.SwapChainResolution.width, (float)RenderSystem.SwapChainResolution.height);
            var pos = new vec2(0.0f, 0.0f);
            OrthographicCamera = new OrthographicCamera2D(res, pos);

            //string jsonContent = File.ReadAllText(levelPath);
            //LevelLoader levelLoader = JsonConvert.DeserializeObject<LevelLoader>(levelPath);

            //foreach(var texture in levelLoader.TextureList)
            //{
                TextureSystem.LoadTexture("C:/Users/dotha/Documents/GitHub/VulkanGameEngine/Textures/TestTexture.json");
            //}
        }
    }
}
