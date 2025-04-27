using GlmSharp;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using VulkanGameEngineLevelEditor.Components;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class SpriteBatchLayer
    {
        public List<Vertex2D> SpriteVertexList = new List<Vertex2D>
        {
            new Vertex2D(new vec2(0.0f, 1.0f), new vec2(0.0f, 0.0f)),
            new Vertex2D(new vec2(1.0f, 1.0f), new vec2(1.0f, 0.0f)),
            new Vertex2D(new vec2(1.0f, 0.0f), new vec2(1.0f, 1.0f)),
            new Vertex2D(new vec2(0.0f, 0.0f), new vec2(0.0f, 1.0f)),
        };

        public List<uint> SpriteIndexList = new List<uint>
        {
            0, 3, 1,
            1, 3, 2
        };

        public List<Sprite> SpriteList = new List<Sprite>();
        public List<SpriteInstanceStruct> SpriteInstanceList = new List<SpriteInstanceStruct>();
        public VulkanBuffer<SpriteInstanceStruct> SpriteBuffer;
        public Mesh2D SpriteLayerMesh { get; private set; }
        public JsonPipeline<Vertex2D> SpriteRenderPipeline { get; set; }

        public SpriteBatchLayer()
        {

        }

        public SpriteBatchLayer(List<GameObject> gameObjectList, JsonPipeline<Vertex2D> spriteRenderPipeline)
        {
            SpriteLayerMesh = new Mesh2D(SpriteVertexList, SpriteIndexList, null);
            SpriteRenderPipeline = spriteRenderPipeline;
            foreach (var gameObject in gameObjectList)
            {
                var sprite = gameObject.GetComponentByComponentType(ComponentTypeEnum.kSpriteComponent) as SpriteComponent;
                if (sprite?.SpriteObj != null)
                {
                    SpriteList.Add(sprite.SpriteObj);
                    SpriteInstanceList.Add(sprite.SpriteObj.SpriteInstance);
                }
            }

            var spriteInstanceArray = SpriteInstanceList.ToArray();
            GCHandle spriteInstanceListHandle = GCHandle.Alloc(spriteInstanceArray, GCHandleType.Pinned);
            SpriteBuffer = new VulkanBuffer<SpriteInstanceStruct>((void*)spriteInstanceListHandle.AddrOfPinnedObject(), SpriteInstanceList.UCount(), VkBufferUsageFlagBits.VK_BUFFER_USAGE_VERTEX_BUFFER_BIT | VkBufferUsageFlagBits.VK_BUFFER_USAGE_STORAGE_BUFFER_BIT, VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT | VkMemoryPropertyFlagBits.VK_MEMORY_PROPERTY_HOST_COHERENT_BIT, false);
            spriteInstanceListHandle.Free();
            SortSpritesByLayer(SpriteList);
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

            ulong[] offsets = new ulong[] { 0, 0 };
            GCHandle offsetsHandle = GCHandle.Alloc(offsets, GCHandleType.Pinned);
            VkFunc.vkCmdPushConstants(commandBuffer, SpriteRenderPipeline.pipelineLayout, VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT | VkShaderStageFlagBits.VK_SHADER_STAGE_FRAGMENT_BIT, 0, (uint)sizeof(SceneDataBuffer), &sceneDataBuffer);
            VkFunc.vkCmdBindPipeline(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, SpriteRenderPipeline.pipeline);
            VkFunc.vkCmdBindDescriptorSets(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, SpriteRenderPipeline.pipelineLayout, 0, SpriteRenderPipeline.descriptorSetList.UCount, SpriteRenderPipeline.descriptorSetList.Ptr, 0, null);
            VkFunc.vkCmdBindVertexBuffers(commandBuffer, 0, 1, (nint*)vertexHandle.AddrOfPinnedObject(), (ulong*)offsetsHandle.AddrOfPinnedObject());
            VkFunc.vkCmdBindVertexBuffers(commandBuffer, 1, 1, (nint*)instanceHandle.AddrOfPinnedObject(), (ulong*)offsetsHandle.AddrOfPinnedObject() + 1);
            VkFunc.vkCmdBindIndexBuffer(commandBuffer, *(nint*)indexHandle.AddrOfPinnedObject(), 0, VkIndexType.VK_INDEX_TYPE_UINT32);
            VkFunc.vkCmdDrawIndexed(commandBuffer, SpriteIndexList.UCount(), SpriteInstanceList.UCount(), 0, 0, 0);

            vertexHandle.Free();
            indexHandle.Free();
            instanceHandle.Free();
            offsetsHandle.Free();
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