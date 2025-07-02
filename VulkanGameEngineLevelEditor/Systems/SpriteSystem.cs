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
using System.Security.Cryptography.Xml;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace VulkanGameEngineLevelEditor.Systems
{
    public class SpriteBatchLayer
    {
        public Guid RenderPassId { get; set; }
        public uint SpriteBatchLayerId { get; set; }
        public uint SpriteLayerMeshId { get; set; }
    }

    public struct Sprite
    {
        public enum SpriteAnimationEnum
        {
            kStanding,
            kWalking
        };

        public int GameObjectId { get; set; }
        public uint SpriteID { get; set; } = 0;
        public uint CurrentAnimationID { get; set; } = 0;
        public uint CurrentFrame { get; set; } = 0;
        public Guid SpriteVramId { get; set; }
        public float CurrentFrameTime { get; set; } = 0.0f;
        public bool SpriteAlive { get; set; } = true;
        public ivec2 FlipSprite { get; set; } = new ivec2(0);
        public vec2 LastSpritePosition { get; set; } = new vec2(0.0f);
        public vec2 LastSpriteRotation { get; set; } = new vec2(0.0f);
        public vec2 LastSpriteScale { get; set; } = new vec2(1.0f);
        public vec2 SpritePosition { get; set; } = new vec2(0.0f);
        public vec2 SpriteRotation { get; set; } = new vec2(0.0f);
        public vec2 SpriteScale { get; set; } = new vec2(1.0f);

        public Sprite()
        {
        }
    };

    public unsafe struct aniVec2
    {
        public float x, y;
    };
    
    public unsafe struct AnimationFrames
    {
        public IntPtr frames;
        public size_t count;
    };

    public static unsafe class SpriteSystem
    {
        public static List<Sprite> SpriteList = new List<Sprite>();
        public static List<SpriteInstanceStruct> SpriteInstanceList = new List<SpriteInstanceStruct>();
        public static List<SpriteBatchLayer> SpriteBatchLayerList = new List<SpriteBatchLayer>();
        public static List<SpriteVram> SpriteVramList = new List<SpriteVram>();
        public static Dictionary<Guid, SpriteVram> SpriteVramMap = new Dictionary<Guid, SpriteVram>();
        public static Dictionary<int, size_t> SpriteIdToListIndexMap = new Dictionary<int, size_t>();
        public static Dictionary<int, int> SpriteInstanceBufferIdMap = new Dictionary<int, int>();
        public static Dictionary<uint, Animation2D> SpriteAnimationMap = new Dictionary<uint, Animation2D>();
        public static Dictionary<Guid, List<aniVec2[]>> SpriteAnimationFrameListMap = new Dictionary<Guid, List<aniVec2[]>>();
        public static Dictionary<int, Vector<SpriteInstanceStruct>> SpriteInstanceListMap = new Dictionary<int, Vector<SpriteInstanceStruct>>();
        public static Dictionary<int, Vector<int>> SpriteBatchObjectListMap = new Dictionary<int, Vector<int>>();

        private static void UpdateBatchSprites(float deltaTime)
        {
            using ListPtr<Transform2DComponent> transform2D = new ListPtr<Transform2DComponent>((size_t)SpriteInstanceList.Count);
            using ListPtr<SpriteVram> vram = new ListPtr<SpriteVram>((size_t)SpriteInstanceList.Count);
            using ListPtr<Animation2D> animation = new ListPtr<Animation2D>((size_t)SpriteInstanceList.Count);
            using ListPtr<AnimationFrames> frameList = new ListPtr<AnimationFrames>((size_t)SpriteInstanceList.Count);
            using ListPtr<Material> material = new ListPtr<Material>((size_t)SpriteInstanceList.Count);
            List<GCHandle> pinnedHandles = new List<GCHandle>();

            try
            {
                for (int x = 0; x < SpriteInstanceList.Count; ++x)
                {
                    var sprite = SpriteList[x];
                    transform2D[x] = GameObjectSystem.Transform2DComponentMap[sprite.GameObjectId];
                    vram[x] = SpriteVramMap[sprite.SpriteVramId];
                    animation[x] = SpriteAnimationMap[sprite.CurrentAnimationID];
                    material[x] = MaterialSystem.MaterialMap[vram[x].MaterialId];
                    aniVec2[] frameData = SpriteAnimationFrameListMap[vram[x].VramSpriteId][(int)sprite.CurrentAnimationID];

                    aniVec2[] frameArray = frameData.ToArray();
                    GCHandle handle = GCHandle.Alloc(frameArray, GCHandleType.Pinned);
                    pinnedHandles.Add(handle);
                    frameList[x] = new AnimationFrames
                    {
                        frames = handle.AddrOfPinnedObject(),
                        count = (nint)frameArray.Length
                    };
                }

                Sprite_UpdateBatchSprites(
                    SpriteInstanceList.ToListPtr(),
                    SpriteList.ToListPtr(),
                    transform2D.Ptr,
                    vram.Ptr,
                    animation.Ptr,
                    frameList.Ptr,
                    material.Ptr,
                    (nint)SpriteInstanceList.Count,
                    deltaTime);
            }
            finally
            {
                foreach (var handle in pinnedHandles)
                    handle.Free();
            }
        }

        private static void UpdateSprites(float deltaTime)
        {
            for (int x = 0; x < SpriteInstanceList.Count; x++)
            {
                var sprite = SpriteList[x];
                Transform2DComponent transform2D = GameObjectSystem.Transform2DComponentMap[sprite.GameObjectId];
                SpriteVram vram = SpriteVramMap[sprite.SpriteVramId];
                Animation2D animation = SpriteAnimationMap[sprite.CurrentAnimationID];
                Material material = MaterialSystem.MaterialMap[vram.MaterialId];
                var frameData = SpriteAnimationFrameListMap[vram.VramSpriteId][(int)sprite.CurrentAnimationID];
                aniVec2[] frameArray = frameData.ToArray();
                GCHandle handle = GCHandle.Alloc(frameArray, GCHandleType.Pinned);
                try
                {
                    AnimationFrames frameList = new AnimationFrames
                    {
                        frames = handle.AddrOfPinnedObject(),
                        count = (nint)frameArray.Length
                    };
                    ivec2 currentFrame = new ivec2 { x = 0, y = 0 };
                    SpriteInstanceList[x] = Sprite_UpdateSprites(transform2D, vram, animation, frameList, material, currentFrame, ref sprite, deltaTime);
                }
                finally
                {
                    handle.Free();
                }
            }
        }

        public static void AddSprite(int gameObjectId, Guid spriteVramId)
        {
            Sprite sprite = new Sprite();
            sprite.GameObjectId = gameObjectId;
            sprite.SpriteVramId = spriteVramId;
            SpriteList.Add(sprite);
            SpriteInstanceList.Add(new SpriteInstanceStruct());
            SpriteIdToListIndexMap[gameObjectId] = SpriteList.Count();
        }

        public static Guid LoadSpriteVRAM(string spriteVramPath)
        {
            if (string.IsNullOrEmpty(spriteVramPath))
            {
                return Guid.Empty;
            }

            string jsonContent = File.ReadAllText(spriteVramPath);
            SpriteVram spriteVramJson = JsonConvert.DeserializeObject<SpriteVram>(jsonContent);
            if (SpriteVramMap.ContainsKey(spriteVramJson.VramSpriteId))
            {
                return spriteVramJson.VramSpriteId;
            }

            if (!MaterialSystem.MaterialMap.TryGetValue(spriteVramJson.MaterialId, out var spriteMaterial))
            {
                throw new KeyNotFoundException($"Material ID {spriteVramJson.MaterialId} not found.");
            }
            if (!TextureSystem.TextureList.TryGetValue(spriteMaterial.AlbedoMapId, out var spriteTexture))
            {
                throw new KeyNotFoundException($"Texture ID {spriteMaterial.AlbedoMapId} not found.");
            }

            SpriteVram spriteVram = new SpriteVram
            {
                VramSpriteId = spriteVramJson.VramSpriteId,
                MaterialId = spriteVramJson.MaterialId
            };
            SpriteVramMap[spriteVramJson.VramSpriteId] = spriteVram;
            SpriteVramList.Add(spriteVram);

            //Animation2D[] animations = spriteVramJson.Animations.Select(a => new Animation2D
            //{
            //    AnimationId = a.AnimationId,
            //    FrameHoldTime = a.Duration
            //}).ToArray();

            //List<List<vec2>> animationFrames = spriteVramJson.AnimationFrames.Select(frameList =>
            //    frameList.Select(f => new vec2 { x = f.x, y = f.y }).ToList()
            //).ToList();

            //SpriteAnimationFrameListMap[spriteVramJson.VramSpriteId] = animationFrames;
            //for (int x = 0; x < animations.Length; x++)
            //{
            //    SpriteAnimationMap[animations[x].AnimationId] = animations[x];
            //}

            return spriteVramJson.VramSpriteId;
        }

        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] private static extern SpriteVram VRAM_LoadSpriteVRAM([MarshalAs(UnmanagedType.LPStr)] string spritePath, ref Material material, ref Texture texture);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] private static extern Animation2D* VRAM_LoadSpriteAnimations([MarshalAs(UnmanagedType.LPStr)] string spritePath, out size_t animationListCount);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] private static extern vec2* VRAM_LoadSpriteAnimationFrames([MarshalAs(UnmanagedType.LPStr)] string spritePath, out size_t animationFrameCount);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern void Sprite_UpdateBatchSprites(SpriteInstanceStruct* spriteInstanceList, Sprite* spriteList, Transform2DComponent* transform2DList, SpriteVram* vramList, Animation2D* animationList, AnimationFrames* frameList, Material* materialList, size_t spriteCount, float deltaTime);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern SpriteInstanceStruct Sprite_UpdateSprites(Transform2DComponent transform2D, SpriteVram vram, Animation2D animation, AnimationFrames frameList, Material material, ivec2 currentFrame, ref Sprite sprite, float deltaTime);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern void Sprite_SetSpriteAnimation(Sprite* sprite, Sprite.SpriteAnimationEnum spriteAnimation);
    }
}
