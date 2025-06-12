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
    public static class LevelSystem
    {
        public static Guid levelRenderPass2DId { get; private set; }
        public static Guid spriteRenderPass2DId { get; private set; }
        public static Guid frameBufferId { get; private set; }
        public static OrthographicCamera2D camera { get; set; }

        public static void LoadLevel(String levelPath)
        {
            var res = new vec2((float)RenderSystem.SwapChainResolution.width, (float)RenderSystem.SwapChainResolution.height);
            var pos = new vec2(0.0f, 0.0f);
            camera = new OrthographicCamera2D(res, pos);

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
