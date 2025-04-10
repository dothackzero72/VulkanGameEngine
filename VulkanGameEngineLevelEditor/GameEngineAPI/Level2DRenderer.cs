using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using VulkanGameEngineGameObjectScripts.Vulkan;
using VulkanGameEngineLevelEditor.Components;
using VulkanGameEngineLevelEditor.Models;
using Silk.NET.Vulkan;
using VulkanGameEngineGameObjectScripts;
using VulkanGameEngineLevelEditor.Vulkan;
using static VulkanGameEngineLevelEditor.GameEngineAPI.SpriteInstanceVertex2D;
using Newtonsoft.Json;
using VulkanGameEngineLevelEditor.RenderPassEditor;
using System.IO;
using System.Numerics;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class Level2DRenderer : JsonRenderPass<Vertex2D>
    {
        public Level2DRenderer()
        {
        }

        public Level2DRenderer(GPUImport<Vertex2D> gpuImport, ivec2 renderPassResolution)
        {
            RenderPassResolution = renderPassResolution;

            VulkanRenderer.CreateCommandBuffers(commandBufferList);
            base.CreateJsonRenderPass(@$"{ConstConfig.RenderPassBasePath}\\{ConstConfig.Default2DRenderPass}", renderPassResolution);
            CreatePipeline(gpuImport);
        }

        private void CreatePipeline(GPUImport<Vertex2D> gpuImport)
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

            jsonPipelineList.Add(new JsonPipeline<Vertex2D>(@$"{ConstConfig.PipelineBasePath}\\{ConstConfig.Default2DPipeline}", renderPass, (uint)sizeof(SceneDataBuffer), vertexBinding, vertexAttribute, gpuImport, RenderPassResolution));
        }

        public override void Update(float deltaTime)
        {

        }

        public virtual VkCommandBuffer Draw(List<GameObject> gameObjectList, SceneDataBuffer sceneDataBuffer)
        {
            throw new Exception("Can't call this version of draw.");
        }

        public VkCommandBuffer Draw(List<SpriteBatchLayer> spriteLayerList, SceneDataBuffer sceneDataBuffer)
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
            foreach (var obj in spriteLayerList)
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

        public SpriteBatchLayer AddSpriteLayer(List<GameObject> gameObjectList)
        {
            return new SpriteBatchLayer(gameObjectList, jsonPipelineList[0]);
        }
    }
}