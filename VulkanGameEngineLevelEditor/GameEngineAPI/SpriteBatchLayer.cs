using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;
using Silk.NET.Vulkan;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineLevelEditor.Components;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class SpriteBatchLayer
    {
        private readonly List<Vertex2D> SpriteVertexList = new List<Vertex2D>
        {
            new Vertex2D(new vec2(0.0f, 1.0f), new vec2(0.0f, 0.0f)),
            new Vertex2D(new vec2(1.0f, 0.0f), new vec2(1.0f, 0.0f)),
            new Vertex2D(new vec2(1.0f, 0.0f), new vec2(1.0f, 1.0f)),
            new Vertex2D(new vec2(0.0f, 0.0f), new vec2(0.0f, 1.0f)),
        };

        private readonly List<uint> SpriteIndexList = new List<uint>
        {
          0, 3, 1,
          1, 3, 2
        };

        public uint MaxSpritesPerSheet { get; private set; }
        public uint SpriteLayerIndex { get; private set; }

        public List<Sprite> SpriteList { get; private set; } = new List<Sprite>();
        public ListPtr<SpriteInstanceStruct> SpriteInstanceList { get; private set; } = new ListPtr<SpriteInstanceStruct>();
        public VulkanBuffer<SpriteInstanceStruct> SpriteBuffer { get; private set; } = new VulkanBuffer<SpriteInstanceStruct>();
        public Mesh2D SpriteLayerMesh { get; private set; }
        public string Name { get; private set; }
        public JsonPipeline<Vertex2D> SpriteRenderPipeline { get; set; }

        public SpriteBatchLayer()
        {

        }

        public SpriteBatchLayer(List<GameObject> gameObjectList, JsonPipeline<Vertex2D> spriteRenderPipeline)
        {
            SpriteRenderPipeline = spriteRenderPipeline;
            SpriteLayerMesh = new Mesh2D(SpriteVertexList, SpriteIndexList, null);

            foreach (var gameObject in gameObjectList)
            {
                var sprite = gameObject.GetComponentByComponentType(ComponentTypeEnum.kSpriteComponent) as SpriteComponent;
                if (sprite.SpriteObj != null)
                {
                    SpriteList.Add(sprite.SpriteObj);
                    SpriteInstanceList.Add(sprite.SpriteObj.SpriteInstance);
                }
            }

            var spriteInstanceArray = SpriteInstanceList.ToArray();
            GCHandle spriteInstanceListHandle = GCHandle.Alloc(spriteInstanceArray, GCHandleType.Pinned);
            SpriteBuffer = new VulkanBuffer<SpriteInstanceStruct>(spriteInstanceListHandle.AddrOfPinnedObject(), SpriteInstanceList.UCount, VkBufferUsageFlagBits.VK_BUFFER_USAGE_VERTEX_BUFFER_BIT |
                                                                                                                                            VkBufferUsageFlagBits.VK_BUFFER_USAGE_INDEX_BUFFER_BIT |
                                                                                                                                            VkBufferUsageFlagBits.VK_BUFFER_USAGE_STORAGE_BUFFER_BIT |
                                                                                                                                            VkBufferUsageFlagBits.VK_BUFFER_USAGE_TRANSFER_DST_BIT, 
                                                                                                                                            VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT |
                                                                                                                                            VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT, false);
            SortSpritesByLayer(SpriteList);
            spriteInstanceListHandle.Free();
        }

        public void LoadSprites()
        {

        }

        public void Update(VkCommandBuffer commandBuffer, float deltaTime)
        {
            SpriteInstanceList.Clear();
            foreach (var sprite in SpriteList)
            {
                sprite.Update(commandBuffer, deltaTime);
                SpriteInstanceList.Add(sprite.SpriteInstance);
            }

            if (SpriteList.Any())
            {
                var spriteInstanceArray = SpriteInstanceList.ToArray();
                GCHandle spriteInstanceListHandle = GCHandle.Alloc(spriteInstanceArray, GCHandleType.Pinned);
                SpriteBuffer.UpdateBufferMemory(spriteInstanceListHandle.AddrOfPinnedObject());
                spriteInstanceListHandle.Free();
            }
        }

        public void Draw(VkCommandBuffer commandBuffer, SceneDataBuffer sceneDataBuffer)
        {
            GCHandle vertexHandle = GCHandle.Alloc(SpriteLayerMesh.MeshVertexBuffer.Buffer, GCHandleType.Pinned);
            GCHandle indexHandle = GCHandle.Alloc(SpriteLayerMesh.MeshIndexBuffer.Buffer, GCHandleType.Pinned);
            GCHandle instanceHandle = GCHandle.Alloc(SpriteBuffer.Buffer, GCHandleType.Pinned);

            ulong offsets = 0;
            VkDescriptorSet descriptorSet = SpriteRenderPipeline.descriptorSetList[0];
            VkFunc.vkCmdPushConstants(commandBuffer, SpriteRenderPipeline.pipelineLayout, VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT | VkShaderStageFlagBits.VK_SHADER_STAGE_FRAGMENT_BIT, 0, (uint)sizeof(SceneDataBuffer), &sceneDataBuffer);
            VkFunc.vkCmdBindPipeline(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, SpriteRenderPipeline.pipeline);
            VkFunc.vkCmdBindDescriptorSets(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, SpriteRenderPipeline.pipelineLayout, 0, 1, &descriptorSet, 0, null);
            VkFunc.vkCmdBindVertexBuffers(commandBuffer, 0, 1, (nint*)vertexHandle.AddrOfPinnedObject(), &offsets);
            VkFunc.vkCmdBindVertexBuffers(commandBuffer, 1, 1, (nint*)instanceHandle.AddrOfPinnedObject(), &offsets);
            VkFunc.vkCmdBindIndexBuffer(commandBuffer, *(nint*)indexHandle.AddrOfPinnedObject(), 0, VkIndexType.VK_INDEX_TYPE_UINT32);
            VkFunc.vkCmdDrawIndexed(commandBuffer, SpriteIndexList.UCount(), SpriteInstanceList.UCount, 0, 0, 0);

            vertexHandle.Free();
            indexHandle.Free();
            instanceHandle.Free();
        }

        public void Destroy()
        {

        }

        public void AddSprite(Sprite sprite)
        {
            SpriteList.Add(sprite);
            SortSpritesByLayer(SpriteList);
        }

        public void RemoveSprite(Sprite sprite)
        {
            sprite.Destroy();
            SpriteList.Remove(sprite);
		}

        private void SortSpritesByLayer(List<Sprite> sprites)
        {
            sprites.Sort((x, y) => x.SpriteLayer.CompareTo(y.SpriteLayer));
        }
    }
}
