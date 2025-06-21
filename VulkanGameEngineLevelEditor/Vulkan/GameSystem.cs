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

namespace VulkanGameEngineLevelEditor.Vulkan
{

    public unsafe struct VulkanRenderPass
    {
        public Guid RenderPassId;
        public VkSampleCountFlagBits SampleCount;
        public VkRect2D RenderArea;
        public VkRenderPass RenderPass;
        public VkFramebuffer* FrameBufferList;
        public VkClearValue* ClearValueList;
        public size_t FrameBufferCount;
        public size_t ClearValueCount;
        public VkCommandBuffer CommandBuffer;
        public bool UseFrameBufferResolution;
    };

    public unsafe struct VulkanPipeline
    {
        public uint RenderPipelineId;
        public size_t DescriptorSetLayoutCount;
        public size_t DescriptorSetCount;
        public VkDescriptorPool DescriptorPool;
        public VkDescriptorSetLayout* DescriptorSetLayoutList;
        public VkDescriptorSet* DescriptorSetList;
        public VkPipeline Pipeline;
        public VkPipelineLayout PipelineLayout;
        public VkPipelineCache PipelineCache;
    };

    public static class GameSystem
    {
        public static OrthographicCamera2D OrthographicCamera { get; set; }
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

        public static unsafe void StartUp(IntPtr window, IntPtr renderAreaHandle)
        {
            RenderSystem.CreateVulkanRenderer(window, renderAreaHandle);
            LevelSystem.LoadLevel("C:/Users/dotha/Documents/GitHub/VulkanGameEngine/Levels/TestLevel.json");
        }

        public static void LoadLevel(String levelPath)
        {
            var res = new vec2((float)RenderSystem.SwapChainResolution.width, (float)RenderSystem.SwapChainResolution.height);
            var pos = new vec2(0.0f, 0.0f);
            OrthographicCamera = new OrthographicCamera2D(res, pos);

            string jsonContent = File.ReadAllText(levelPath);
            LevelLoader levelLoader = JsonConvert.DeserializeObject<LevelLoader>(levelPath);

        }
    }
}
