﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Vulkan;
using VulkanGameEngineLevelEditor.GameEngineAPI;


namespace VulkanGameEngineLevelEditor.Models
{
    [Serializable]
    public unsafe class RenderPassLoaderModel : RenderPassEditorBaseModel
    {
        public Guid RenderPassId { get; set; } = Guid.NewGuid();
        public List<string> RenderPipelineList { get; set; } = new List<string>();
        public List<RenderPipelineLoaderModel> renderPipelineModelList { get; set; } = new List<RenderPipelineLoaderModel>();
        public List<RenderedTextureInfoModel> RenderedTextureInfoModelList = new List<RenderedTextureInfoModel>();
        public List<VkSubpassDependencyModel> SubpassDependencyList { get; set; } = new List<VkSubpassDependencyModel>();
        public List<VkClearValue> ClearValueList { get; set; } = new List<VkClearValue>();
        public RenderAreaModel RenderArea { get; set; } = new RenderAreaModel();
        public bool IsRenderedToSwapchain { get; set; } = false;

        public RenderPassLoaderModel()
        {
        }

        public RenderPassLoaderModel(string jsonFilePath) : base()
        {
            LoadJsonComponent(jsonFilePath);
        }

        public RenderPassLoaderModel(string name, string jsonFilePath) : base(name)
        {
            LoadJsonComponent(@"C:\Users\dotha\Documents\GitHub\VulkanGameEngine\RenderPass\RenderPass\DefaultSubpassDependency.json");
        }

        public void LoadJsonComponent(string jsonPath)
        {
            var obj = base.LoadJsonComponent<RenderPassLoaderModel>(jsonPath);
            RenderPipelineList = obj.RenderPipelineList;
            RenderedTextureInfoModelList = obj.RenderedTextureInfoModelList;
            SubpassDependencyList = obj.SubpassDependencyList;
            IsRenderedToSwapchain = obj.IsRenderedToSwapchain;
        }
    }
}
