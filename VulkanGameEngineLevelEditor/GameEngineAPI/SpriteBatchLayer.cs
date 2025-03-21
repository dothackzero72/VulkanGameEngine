using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;
using Silk.NET.Vulkan;
using VulkanGameEngineGameObjectScripts;
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

        public List<Sprite> SpriteList { get; private set; }
        public List<SpriteInstanceStruct> SpriteInstanceList { get; private set; }
        public SpriteInstanceBuffer SpriteBuffer { get; private set; }
        public Mesh2D SpriteLayerMesh { get; private set; }
        public string Name;
        public JsonPipeline SpriteRenderPipeline;

        public SpriteBatchLayer()
        {

        }

        public SpriteBatchLayer(List<GameObject> gameObjectList, JsonPipeline spriteRenderPipeline)
        {
            SpriteRenderPipeline = spriteRenderPipeline;
            SpriteLayerMesh = new Mesh2D(SpriteVertexList, SpriteIndexList, null);

            foreach (var gameObject in gameObjectList)
            {
                var sprite = (SpriteComponent)gameObject.GetComponentByComponentType(kSpriteComponent);
                if (sprite->GetSprite())
                {
                    SpriteList.Add(sprite->GetSprite());
                    SpriteInstanceList.Add(*sprite->GetSprite()->GetSpriteInstance().get());
                }
            }

            SpriteBuffer = SpriteInstanceBuffer(SpriteInstanceList, SpriteLayerMesh.GetMeshBufferUsageSettings(), SpriteLayerMesh.GetMeshBufferPropertySettings(), false);
            SortSpritesByLayer(SpriteList);
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
                SpriteBuffer.UpdateBufferMemory(SpriteInstanceList);
            }
        }

        public void Draw(VkCommandBuffer commandBuffer, SceneDataBuffer sceneDataBuffer)
        {
            VkDeviceSize offsets[] = { 0 };
            VkFunc.vkCmdPushConstants(commandBuffer, SpriteRenderPipeline.pipelineLayout, VkShaderStageFlagBits.VK_SHADER_STAGE_VERTEX_BIT | VkShaderStageFlagBits.VK_SHADER_STAGE_FRAGMENT_BIT, 0, (uint)sizeof(SceneDataBuffer), &sceneDataBuffer);
            VkFunc.vkCmdBindPipeline(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, SpriteRenderPipeline.pipeline);
            VkFunc.vkCmdBindDescriptorSets(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, SpriteRenderPipeline.pipelineLayout, 0, 1, &SpriteRenderPipeline.descriptorSetList[0], 0, null);
            VkFunc.vkCmdBindVertexBuffers(commandBuffer, 0, 1, SpriteLayerMesh.VertexBuffer, offsets);
            VkFunc.vkCmdBindVertexBuffers(commandBuffer, 1, 1, &SpriteBuffer.Buffer, offsets);
            VkFunc.vkCmdBindIndexBuffer(commandBuffer, SpriteLayerMesh.IndexBuffer.get(), 0, VkIndexType.VK_INDEX_TYPE_UINT32);
            VkFunc.vkCmdDrawIndexed(commandBuffer, SpriteIndexList.UCount(), SpriteInstanceList.UCount(), 0, 0, 0);
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
