using CSScripting;
using GlmSharp;
using Newtonsoft.Json;
using Silk.NET.SDL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.Systems
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
      //  public static OrthographicCamera2D camera { get; set; }
     //   public static Dictionary<uint, Sprite> SpriteMap { get; private set; } = new Dictionary<uint, Sprite>();
        public static Dictionary<Guid, LevelTileSet> LevelTileSetMap { get; private set; } = new Dictionary<Guid, LevelTileSet>();
        public static Dictionary<Guid, SpriteVram> VramSpriteMap { get; private set; } = new Dictionary<Guid, SpriteVram>();
       // public static Dictionary<Guid, List<SpriteBatchLayer>> SpriteBatchLayerListMap { get; private set; } = new Dictionary<Guid, List<SpriteBatchLayer>>();
        public static Dictionary<uint, List<SpriteInstanceStruct>> SpriteInstanceListMap { get; private set; } = new Dictionary<uint, List<SpriteInstanceStruct>>();
        public static Dictionary<uint, int> SpriteInstanceBufferMap { get; private set; } = new Dictionary<uint, int>();
        public static Dictionary<uint, List<uint>> SpriteBatchLayerObjectListMap { get; private set; } = new Dictionary<uint, List<uint>>();
        public static Dictionary<uint, Animation2D> AnimationMap { get; private set; } = new Dictionary<uint, Animation2D>();
        public static Dictionary<Guid, ListPtr<vec2>> AnimationFrameListMap { get; private set; } = new Dictionary<Guid, ListPtr<vec2>>();


        public static void LoadLevel(string levelPath)
        {
            string levelDirectory = Path.GetDirectoryName(levelPath);
            string jsonContent = File.ReadAllText(levelPath);
            LevelLoader levelLoader = JsonConvert.DeserializeObject<LevelLoader>(jsonContent);

            foreach (var texturePath in levelLoader.LoadTextures)
            {
                string fullTexturePath = Path.GetFullPath(Path.Combine(levelDirectory, texturePath));
                TextureSystem.LoadTexture(fullTexturePath);
            }
            foreach (var materialPath in levelLoader.LoadMaterials)
            {
                string fullMaterialPath = Path.GetFullPath(Path.Combine(levelDirectory, materialPath));
                MaterialSystem.LoadMaterial(fullMaterialPath);
            }
            foreach (var spriteVRAMPath in levelLoader.LoadSpriteVRAM)
            {
                string fullSpriteVRAMPath = Path.GetFullPath(Path.Combine(levelDirectory, spriteVRAMPath));
                LoadSpriteVRAM(fullSpriteVRAMPath);
            }
            foreach (var levelLayoutPath in levelLoader.LoadLevelLayout)
            {
                // string fullMaterialPath = Path.GetFullPath(Path.Combine(levelDirectory, spriteVRAMPath));
                //MaterialSystem.LoadMaterial(fullMaterialPath);
            }
            {

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

        }

        private static Guid LoadSpriteVRAM(string spriteVramPath)
        {
            if (spriteVramPath.IsEmpty())
            {
                return new Guid();
            }

            string jsonContent = File.ReadAllText(spriteVramPath);
            SpriteVram spriteVramJson = JsonConvert.DeserializeObject<SpriteVram>(jsonContent);

            if (VramSpriteMap.ContainsKey(spriteVramJson.VramSpriteId))
            {
                return spriteVramJson.VramSpriteId;
            }

            var spriteMaterial = MaterialSystem.MaterialMap[spriteVramJson.MaterialId];
            var spriteTexture = TextureSystem.TextureList[spriteMaterial.AlbedoMapId];

            Animation2D* animationListPtr = null;
            vec2* animationFrameListPtr = null;

            VramSpriteMap[spriteVramJson.VramSpriteId] = VRAM_LoadSpriteVRAM(spriteVramPath, ref spriteMaterial, ref spriteTexture);
            VRAM_LoadSpriteAnimation(spriteVramPath, animationListPtr, animationFrameListPtr, out size_t animationListCount, out size_t animationFrameCount);
            ListPtr<Animation2D> animationList = new ListPtr<Animation2D>(animationListPtr, animationListCount);

            return spriteVramJson.VramSpriteId;
        }
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)]
        private static extern SpriteVram VRAM_LoadSpriteVRAM([MarshalAs(UnmanagedType.LPStr)] string spritePath, ref Material material, ref Texture texture);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)]
        private static extern void VRAM_LoadSpriteAnimation([MarshalAs(UnmanagedType.LPStr)] string spritePath, Animation2D* animationListPtr, vec2* animationFrameListPtr, out size_t animationListCount, out size_t animationFrameCount);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)]
        private static extern void VRAM_DeleteSpriteAnimation(Animation2D* animationListPtr, vec2* animationFrameListPtr);
    }
}
