using GlmSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.GameEngineAPI
{
    public unsafe class JsonRenderPass<T>
    {
        private VkDevice device => RenderSystem.Device;
        public List<RenderedTexture> RenderedColorTextureList { get; private set; } = new List<RenderedTexture>();
        public DepthTexture depthTexture { get; set; } = new DepthTexture();
        public ivec2 RenderPassResolution { get; set; }
        public VkRenderPass renderPass { get; protected set; }
        public ListPtr<VkCommandBuffer> commandBufferList { get; protected set; } = new ListPtr<VkCommandBuffer>();
        public ListPtr<VkFramebuffer> frameBufferList { get; protected set; } = new ListPtr<VkFramebuffer>();
        protected List<JsonPipeline<T>> jsonPipelineList { get; set; } = new List<JsonPipeline<T>>();
        public VkSampleCountFlagBits SampleCountFlags { get; protected set; }
        public ListPtr<VkClearValue> clearColorValueList { get; protected set; } = new ListPtr<VkClearValue>();
        public VkRenderPassBeginInfo RenderPassInfo { get; protected set; }
        public JsonRenderPass() : base()
        {
        }

        public void CreateJsonRenderPass(string jsonPath, ivec2 renderPassResolution, VkSampleCountFlagBits sampleCount = VkSampleCountFlagBits.VK_SAMPLE_COUNT_1_BIT)
        {
            RenderPassResolution = renderPassResolution;
            SampleCountFlags = sampleCount;

            string jsonContent = File.ReadAllText(jsonPath);
            RenderPassBuildInfoModel model = JsonConvert.DeserializeObject<RenderPassBuildInfoModel>(jsonContent);
            clearColorValueList = new ListPtr<VkClearValue>(model.ClearValueList);

            RenderPassBuildInfoDLL modelDLL = model.ToDLL();
            foreach (var item in model.RenderedTextureInfoModelList)
            {
                item.ImageCreateInfo.extent = new VkExtent3DModel
                {
                    width = (uint)renderPassResolution.x,
                    height = (uint)renderPassResolution.y,
                    depth = 1
                };
            }

            renderPass = GameEngineImport.DLL_RenderPass_BuildRenderPass(RenderSystem.Device, model.ToDLL());
            frameBufferList = CreateRenderPassImages(model);
            RenderSystem.CreateCommandBuffers(commandBufferList);

            VkExtent2D extent = new VkExtent2D
            {
                width = model.RenderArea.RenderArea.extent.width,
                height = model.RenderArea.RenderArea.extent.height,
            };
            if (model.RenderArea.UseDefaultRenderArea)
            {
                extent.width = (uint)RenderPassResolution.x;
                extent.height = (uint)RenderPassResolution.y;
            }
            RenderPassInfo = new VkRenderPassBeginInfo
            {
                renderPass = renderPass,
                framebuffer = frameBufferList[0],
                clearValueCount = clearColorValueList.UCount,
                pClearValues = clearColorValueList.Ptr,
                renderArea = new VkRect2D
                {
                    offset = new VkOffset2D
                    {
                        x = model.RenderArea.RenderArea.offset.x,
                        y = model.RenderArea.RenderArea.offset.y
                    },
                    extent = extent
                }
            };
        }

        private ListPtr<VkFramebuffer> CreateRenderPassImages(RenderPassBuildInfoModel model)
        {
            foreach (RenderedTextureInfoModel renderedTextureInfoModel in model.RenderedTextureInfoModelList)
            {
                switch (renderedTextureInfoModel.TextureType)
                {
                    case RenderedTextureType.ColorRenderedTexture: RenderedColorTextureList.Add(new RenderedTexture(VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, renderedTextureInfoModel.ImageCreateInfo.Convert(), renderedTextureInfoModel.SamplerCreateInfo.Convert())); break;
                    case RenderedTextureType.InputAttachmentTexture: RenderedColorTextureList.Add(new RenderedTexture(VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, renderedTextureInfoModel.ImageCreateInfo.Convert(), renderedTextureInfoModel.SamplerCreateInfo.Convert())); break;
                    case RenderedTextureType.ResolveAttachmentTexture: RenderedColorTextureList.Add(new RenderedTexture(VkImageAspectFlagBits.VK_IMAGE_ASPECT_COLOR_BIT, renderedTextureInfoModel.ImageCreateInfo.Convert(), renderedTextureInfoModel.SamplerCreateInfo.Convert())); break;
                    case RenderedTextureType.DepthRenderedTexture: depthTexture = new DepthTexture(VkImageAspectFlagBits.VK_IMAGE_ASPECT_DEPTH_BIT, renderedTextureInfoModel.ImageCreateInfo.Convert(), renderedTextureInfoModel.SamplerCreateInfo.Convert()); break;
                    default:
                        {
                            throw new Exception("Case doesn't exist: RenderedTextureType");
                        }
                };
            }

            ListPtr<VkImageView> imageViews = new ListPtr<VkImageView>(RenderedColorTextureList.Select(x => x.View).ToList());
            VkImageView depthView = depthTexture.View;
            return new ListPtr<VkFramebuffer>(GameEngineImport.DLL_RenderPass_BuildFrameBuffer(device, renderPass, model.ToDLL(), imageViews.Ptr, &depthView, RenderSystem.SwapChainImageViews.Ptr, RenderSystem.SwapChainImageViews.UCount, imageViews.UCount, new ivec2((int)RenderSystem.SwapChainResolution.width, (int)RenderSystem.SwapChainResolution.height)), RenderSystem.SwapChainImageViews.UCount);
        }

        public virtual void Update(float deltaTime)
        {

        }

        public virtual VkCommandBuffer Draw(List<GameObject> gameObjectList, SceneDataBuffer sceneDataBuffer)
        {
            var imageIndex = RenderSystem.ImageIndex;
            var commandBuffer = commandBufferList[(int)RenderSystem.CommandIndex];

            VkRenderPassBeginInfo renderPassInfo = new VkRenderPassBeginInfo
            {
                renderPass = renderPass,
                framebuffer = frameBufferList[(int)imageIndex],
                clearValueCount = clearColorValueList.UCount,
                pClearValues = clearColorValueList.Ptr,
                renderArea = new VkRect2D
                {
                    offset = new VkOffset2D
                    {
                        x = 0,
                        y = 0
                    },
                    extent = new VkExtent2D
                    {
                        width = (uint)RenderPassResolution.x,
                        height = (uint)RenderPassResolution.y
                    }
                }
            };

            var commandInfo = new VkCommandBufferBeginInfo
            {
                sType = VkStructureType.VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
                flags = VkCommandBufferUsageFlagBits.VK_COMMAND_BUFFER_USAGE_SIMULTANEOUS_USE_BIT
            };

            VkFunc.vkResetCommandBuffer(commandBuffer, 0);
            VkFunc.vkBeginCommandBuffer(commandBuffer, &commandInfo);
            VkFunc.vkCmdBeginRenderPass(commandBuffer, &renderPassInfo, VkSubpassContents.VK_SUBPASS_CONTENTS_INLINE);
            VkFunc.vkCmdBindPipeline(commandBuffer, VkPipelineBindPoint.VK_PIPELINE_BIND_POINT_GRAPHICS, jsonPipelineList[0].pipeline);
            foreach (var obj in gameObjectList)
            {
                //  obj.Draw(commandBuffer, jsonPipelineList[0].pipeline, jsonPipelineList[0].pipelineLayout, jsonPipelineList[0].descriptorSetList, sceneDataBuffer);
            }
            VkFunc.vkCmdEndRenderPass(commandBuffer);
            VkFunc.vkEndCommandBuffer(commandBuffer);

            return commandBuffer;
        }

        public virtual void Destroy()
        {
        }
    }
}