using GlmSharp;
using System.Collections.Generic;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.Vulkan;
using Newtonsoft.Json;
using VulkanGameEngineLevelEditor.RenderPassEditor;
using System.IO;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class Level2DRenderer : JsonRenderPass<Vertex2D>
    {
        public List<SpriteBatchLayer> SpriteLayerList { get; private set; } = new List<SpriteBatchLayer>();
        public Level2DRenderer(List<GameObject> gameObjectList, GPUImport<Vertex2D> gpuImport, string json, ivec2 renderPassResolution)
        {
            RenderPassBuildInfoModel model = new RenderPassBuildInfoModel(json);
            foreach (var item in model.RenderedTextureInfoModelList)
            {
                item.ImageCreateInfo.extent = new VkExtent3DModel
                {
                    width = (uint)renderPassResolution.x,
                    height = (uint)renderPassResolution.y,
                    depth = 1
                };
            }

            RenderPassResolution = renderPassResolution;

            VulkanRenderer.CreateCommandBuffers(commandBufferList);
            base.CreateJsonRenderPass(json, renderPassResolution);
            StartLevelRenderer(gameObjectList, gpuImport);
        }

        public Level2DRenderer()
        {
        }

        public void StartLevelRenderer(List<GameObject> gameObjectList, GPUImport<Vertex2D> gpuImport)
        {
            string jsonContent = File.ReadAllText(ConstConfig.Default2DPipeline);
            RenderPipelineDLL model = JsonConvert.DeserializeObject<RenderPipelineModel>(jsonContent).ToDLL();

            CreatePipeline(model, gpuImport);
            SpriteLayerList.Add(new SpriteBatchLayer(gameObjectList, jsonPipelineList[0]));
        }

        private void CreatePipeline(RenderPipelineDLL model, GPUImport<Vertex2D> gpuImport)
        {
            var vertexBinding = NullVertex.GetBindingDescriptions();
            foreach (var instanceVar in SpriteInstanceVertex2D.GetBindingDescriptions())
            {
                vertexBinding.Add(instanceVar);
            }

            var vertexAttribute = NullVertex.GetAttributeDescriptions();
            foreach (var instanceVar in SpriteInstanceVertex2D.GetAttributeDescriptions())
            {
                vertexAttribute.Add(instanceVar);
            }

            jsonPipelineList.Add(new JsonPipeline<Vertex2D>("C:\\Users\\dotha\\Documents\\GitHub\\VulkanGameEngine\\Pipelines\\Default2DPipeline.json", renderPass, (uint)sizeof(SceneDataBuffer), vertexBinding, vertexAttribute, gpuImport, RenderPassResolution));
        }

        public void Update(VkCommandBuffer commandBuffer, float deltaTime)
        {
            foreach (var spriteLayer in SpriteLayerList)
            {
                spriteLayer.Update(commandBuffer, deltaTime);
            }
        }

        public override VkCommandBuffer Draw(List<GameObject> meshList, SceneDataBuffer sceneDataBuffer)
        {
            var commandIndex = VulkanRenderer.CommandIndex;
            var commandBuffer = commandBufferList[(int)commandIndex];

            using ListPtr<VkClearValue> clearValues = new ListPtr<VkClearValue>();
            clearValues.Add(new VkClearValue { Color = new VkClearColorValue(0, 0, 0, 1) });
            clearValues.Add(new VkClearValue { DepthStencil = new VkClearDepthStencilValue(1.0f, 0) });

            VkRenderPassBeginInfo renderPassInfo = new VkRenderPassBeginInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_RENDER_PASS_BEGIN_INFO,
                renderPass = renderPass,
                framebuffer = frameBufferList[0],
                clearValueCount = clearValues.UCount,
                pClearValues = clearValues.Ptr,
                renderArea = new(new VkOffset2D(0, 0), VulkanRenderer.SwapChain.SwapChainResolution)
            };

            VkCommandBufferBeginInfo commandInfo = new VkCommandBufferBeginInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
                flags = VkCommandBufferUsageFlagBits.VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT
            };

            VkFunc.vkResetCommandBuffer(commandBuffer, 0);
            VkFunc.vkBeginCommandBuffer(commandBuffer, &commandInfo);
            VkFunc.vkCmdBeginRenderPass(commandBuffer, &renderPassInfo, VkSubpassContents.VK_SUBPASS_CONTENTS_INLINE);
            foreach (var obj in SpriteLayerList)
            {
                obj.Draw(commandBuffer, sceneDataBuffer);
            }
            VkFunc.vkCmdEndRenderPass(commandBuffer);
            VkFunc.vkEndCommandBuffer(commandBuffer);

            return commandBuffer;
        }

        public void Destroy()
        {
            // vk.DestroyPipeline(device, pipeline, null);
            //vk.DestroyPipelineLayout(device, new PipelineLayout((ulong?)pipelineLayout), null);
            //foreach (var layout in descriptorSetLayoutList)
            //{
            //    // vk.DestroyDescriptorSetLayout(device, layout, null);
            //}
            //// vk.DestroyDescriptorPool(device, descriptorPool, null);
            //foreach (var fb in FrameBufferList)
            //{
            //    vk.DestroyFramebuffer(device, fb, null);
            //}
            //vk.DestroyRenderPass(device, new RenderPass((ulong?)renderPass), null);

            frameBufferList.Dispose();
            commandBufferList.Dispose();
            //descriptorSetLayoutList.Dispose();
            //  descriptorSetList.Dispose();
        }
    }
}