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
     //   public static OrthographicCamera2D OrthographicCamera { get; set; }
        public static Guid TileSetId { get; set; }
        public static Guid LevelRendererId { get; set; }
        public static Guid SpriteRenderPass2DId { get; set; }
        public static Guid FrameBufferId { get; set; }

        /*   public static extern IntPtr VulkanRenderPass_CreateVulkanRenderPassCS(
        ref RendererStateCS renderStateCS,

        [MarshalAs(UnmanagedType.LPStr)] string renderPassLoader,
        VkExtent2D renderPassResolution,
            int ConstBuffer,
            TextureStruct* renderedTextureListPtr,
            ulong* renderedTextureCount,
            TextureStruct* depthTexture
        );*/

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
    }
}
