using AutoMapper;
using GlmSharp;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.Models
{
    [Serializable]
    public unsafe class RenderPassBuildInfoModel : RenderPassEditorBaseModel
    {
        public List<string> RenderPipelineList { get; set; } = new List<string>();

        public List<RenderedTextureInfoModel> RenderedTextureInfoModelList = new List<RenderedTextureInfoModel>();
        public List<VkSubpassDependencyModel> SubpassDependencyList { get; set; } = new List<VkSubpassDependencyModel>();
        public bool IsRenderedToSwapchain { get; set; }

        public RenderPassBuildInfoModel() 
        {
        }

        public RenderPassBuildInfoModel(string jsonFilePath) : base()
        {
            LoadJsonComponent(jsonFilePath);
        }

        public RenderPassBuildInfoModel(string name, string jsonFilePath) : base(name)
        {
           // LoadJsonComponent(@"C:\Users\dotha\Documents\GitHub\VulkanGameEngine\RenderPass\RenderPass\DefaultSubpassDependency.json");
        }

        public void ConvertToVulkan()
        {
            throw new NotImplementedException();
        }

        public RenderPassBuildInfoDLL ToDLL()
        {
            var renderPipelineArray = new IntPtr[RenderPipelineList.Count];
            for (int x = 0; x < RenderPipelineList.Count; x++)
            {
                renderPipelineArray[x] = Marshal.StringToHGlobalAnsi(RenderPipelineList[x]);
            }

            var renderedTextureInfoDLLList = new List<RenderedTextureInfoDLL>();
            foreach (var renderedTextureInfoModel in RenderedTextureInfoModelList)
            {
                renderedTextureInfoDLLList.Add(renderedTextureInfoModel.ToDLL());
            }
            var renderedTextureInfoArray = renderedTextureInfoDLLList.ToArray();

            var subpassDependencyDLLList = new List<VkSubpassDependency>();
            foreach (var subpassDependencyModel in SubpassDependencyList)
            {
                subpassDependencyDLLList.Add(subpassDependencyModel.Convert());
            }
            VkSubpassDependency[] subpassArray = subpassDependencyDLLList.ToArray();

            GCHandle renderPipelineHandle = GCHandle.Alloc(renderPipelineArray, GCHandleType.Pinned);
            GCHandle textureInfoHandle = GCHandle.Alloc(renderedTextureInfoArray, GCHandleType.Pinned);
            GCHandle subpassHandle = GCHandle.Alloc(subpassArray, GCHandleType.Pinned);
            fixed (byte* namePtr = System.Text.Encoding.UTF8.GetBytes(_name + "\0"))
            {
                return new RenderPassBuildInfoDLL
                {
                   // Name = (IntPtr)namePtr,
                    IsRenderedToSwapchain = IsRenderedToSwapchain,
                    RenderedTextureInfoModelList = (RenderedTextureInfoDLL*)textureInfoHandle.AddrOfPinnedObject(),
                    RenderedTextureInfoModeCount = RenderedTextureInfoModelList.UCount(),
                   // RenderPipelineList = (IntPtr*)renderPipelineHandle.AddrOfPinnedObject(),
                    SubpassDependencyCount = SubpassDependencyList.UCount(),
                    SubpassDependencyList = (VkSubpassDependency*)subpassHandle.AddrOfPinnedObject()
                };
            }
        }

        public void LoadJsonComponent(string jsonPath)
        {
            var obj = base.LoadJsonComponent<RenderPassBuildInfoModel>(jsonPath);
            RenderPipelineList = obj.RenderPipelineList;
            RenderedTextureInfoModelList = obj.RenderedTextureInfoModelList;
            SubpassDependencyList = obj.SubpassDependencyList;
            IsRenderedToSwapchain = obj.IsRenderedToSwapchain;
        }
    }
}
