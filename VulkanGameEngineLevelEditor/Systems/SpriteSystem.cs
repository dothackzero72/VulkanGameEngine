using GlmSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using VulkanGameEngineLevelEditor.GameEngineAPI;

namespace VulkanGameEngineLevelEditor.Systems
{
    public struct SpriteBatchLayer
    {
        public Guid RenderPassId { get; set; }
        public uint SpriteBatchLayerId { get; set; }
        public uint SpriteLayerMeshId { get; set; }
        public SpriteBatchLayer() { }
        public SpriteBatchLayer(Guid renderPassId)
        {
            RenderPassId = renderPassId;
        }
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
        public static ListPtr<Sprite> SpriteList = new ListPtr<Sprite>();
        public static ListPtr<SpriteInstanceStruct> SpriteInstanceList = new ListPtr<SpriteInstanceStruct>();
        public static ListPtr<SpriteBatchLayer> SpriteBatchLayerList = new ListPtr<SpriteBatchLayer>();
        public static ListPtr<SpriteVram> SpriteVramList = new ListPtr<SpriteVram>();
        public static Dictionary<Guid, SpriteVram> SpriteVramMap = new Dictionary<Guid, SpriteVram>();
        public static Dictionary<int, size_t> SpriteIdToListIndexMap = new Dictionary<int, size_t>();
        public static Dictionary<int, int> SpriteInstanceBufferIdMap = new Dictionary<int, int>();
        public static Dictionary<uint, Animation2D> SpriteAnimationMap = new Dictionary<uint, Animation2D>();
        public static Dictionary<Guid, List<aniVec2[]>> SpriteAnimationFrameListMap = new Dictionary<Guid, List<aniVec2[]>>();
        public static Dictionary<int, ListPtr<SpriteInstanceStruct>> SpriteInstanceListMap = new Dictionary<int, ListPtr<SpriteInstanceStruct>>();
        public static Dictionary<int, ListPtr<int>> SpriteBatchObjectListMap = new Dictionary<int, ListPtr<int>>();

        public static void Update(float deltaTime)
        {
            if (SpriteList.Count > 100)
            {
                UpdateBatchSprites(deltaTime);
            }
            else
            {
                UpdateSprites(deltaTime);
            }

            VkCommandBuffer commandBuffer = RenderSystem.BeginSingleTimeCommands();
            UpdateSpriteBatchLayers(deltaTime);
            RenderSystem.EndSingleTimeCommands(commandBuffer);
        }

        private static void UpdateSpriteBatchLayers(float deltaTime)
        {
            foreach (var spriteBatchLayer in SpriteBatchLayerList)
            {
                ListPtr<SpriteInstanceStruct> spriteInstanceStructList = FindSpriteInstanceList((int)spriteBatchLayer.SpriteBatchLayerId);
                ListPtr<int> spriteBatchObjectList = FindSpriteBatchObjectListMap((int)spriteBatchLayer.SpriteBatchLayerId);

                spriteInstanceStructList.Clear();
                spriteInstanceStructList = new ListPtr<SpriteInstanceStruct>(spriteBatchObjectList.Count);
                foreach (var gameObjectID in spriteBatchObjectList)
                {
                    SpriteInstanceStruct spriteInstanceStruct = FindSpriteInstance(gameObjectID);
                    spriteInstanceStructList.Add(spriteInstanceStruct);
                }

                if (spriteBatchObjectList.Any())
                {
                    uint bufferId = (uint)FindSpriteInstanceBufferId((int)spriteBatchLayer.SpriteBatchLayerId);
                    BufferSystem.UpdateBufferMemory(RenderSystem.renderer, bufferId, spriteInstanceStructList);
                }
            }
        }

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
                Sprite_UpdateBatchSprites(SpriteInstanceList.Ptr, SpriteList.Ptr, transform2D.Ptr, vram.Ptr, animation.Ptr, frameList.Ptr, material.Ptr, (nint)SpriteInstanceList.Count, deltaTime);
            }
            finally
            {
                foreach (var handle in pinnedHandles)
                {
                    handle.Free();
                }
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

        public static void AddSpriteBatchLayer(Guid renderPassId)
        {
            SpriteBatchLayerList.Add(new SpriteBatchLayer(renderPassId));
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

        public static Sprite FindSprite(int gameObjectId)
        {
            return SpriteList.Where(x => x.GameObjectId == gameObjectId).First();
        }

        public static SpriteVram FindVramSprite(Guid vramSpriteId)
        {
            return SpriteVramList.Where(x => x.VramSpriteId == vramSpriteId).First();
        }

        public static Animation2D FindSpriteAnimation(uint animationId)
        {
            return SpriteAnimationMap.Where(x => x.Key == animationId).First().Value;
        }

        public static List<aniVec2[]> FindSpriteAnimationFrames(Guid vramSpriteId)
        {
            return SpriteAnimationFrameListMap.Where(x => x.Key == vramSpriteId).First().Value;
        }

        public static SpriteInstanceStruct FindSpriteInstance(int gameObjectId)
        {
            if (SpriteInstanceList.Count() <= 200)
            {
                size_t spriteInstanceIndex = FindSpriteIndex(gameObjectId);
                return SpriteInstanceList[(int)spriteInstanceIndex];
            }
            else
            {
                var spriteInstanceIndex = SpriteIdToListIndexMap.Where(x => x.Key == gameObjectId).First().Value;
                return SpriteInstanceList[(int)spriteInstanceIndex];
            }
        }

        public static int FindSpriteInstanceBufferId(int spriteInstanceBufferId)
        {
            return SpriteInstanceBufferIdMap.Where(x => x.Key == spriteInstanceBufferId).First().Value;
        }

        public static ListPtr<SpriteInstanceStruct> FindSpriteInstanceList(int spriteBatchId)
        {
            return SpriteInstanceListMap.Where(x => x.Key == spriteBatchId).First().Value;
        }

        public static ListPtr<int> FindSpriteBatchObjectListMap(int spriteBatchObjectListId)
        {
            return SpriteBatchObjectListMap.Where(x => x.Key == spriteBatchObjectListId).First().Value;
        }

        public static ListPtr<SpriteBatchLayer> FindSpriteBatchLayer(Guid renderPassId)
        {
            return new ListPtr<SpriteBatchLayer>(SpriteBatchLayerList.Where(x => x.RenderPassId == renderPassId).ToList());
        }

        public static int FindSpriteIndex(int gameObjectId)
        {
            var sprite = SpriteList.Where(x => x.GameObjectId == gameObjectId).First();
            return SpriteList.ToList().IndexOf(sprite);
        }

        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] private static extern SpriteVram VRAM_LoadSpriteVRAM([MarshalAs(UnmanagedType.LPStr)] string spritePath, ref Material material, ref Texture texture);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] private static extern Animation2D* VRAM_LoadSpriteAnimations([MarshalAs(UnmanagedType.LPStr)] string spritePath, out size_t animationListCount);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] private static extern vec2* VRAM_LoadSpriteAnimationFrames([MarshalAs(UnmanagedType.LPStr)] string spritePath, out size_t animationFrameCount);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern void Sprite_UpdateBatchSprites(SpriteInstanceStruct* spriteInstanceList, Sprite* spriteList, Transform2DComponent* transform2DList, SpriteVram* vramList, Animation2D* animationList, AnimationFrames* frameList, Material* materialList, size_t spriteCount, float deltaTime);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern SpriteInstanceStruct Sprite_UpdateSprites(Transform2DComponent transform2D, SpriteVram vram, Animation2D animation, AnimationFrames frameList, Material material, ivec2 currentFrame, ref Sprite sprite, float deltaTime);
        [DllImport(GameEngineImport.DLLPath, CallingConvention = CallingConvention.StdCall)] public static extern void Sprite_SetSpriteAnimation(Sprite* sprite, Sprite.SpriteAnimationEnum spriteAnimation);
    }
}
