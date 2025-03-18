using GlmSharp;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VulkanGameEngineLevelEditor.Vulkan;

namespace VulkanGameEngineLevelEditor.Models
{
    [Serializable]
    public class RenderPassBuildInfoModel : RenderPassEditorBaseModel
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
