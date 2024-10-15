using GlmSharp;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VulkanGameEngineLevelEditor.Models
{
    [Serializable]
    public class RenderPassModel : RenderPassEditorBaseModel
    {
        public ivec2 SwapChainResuloution { get; set; } = new ivec2();
        public List<RenderPipeline> RenderPipelineList { get; set; } = new List<RenderPipeline>();

        public List<RenderedTextureInfoModel> RenderedTextureInfoModelList = new List<RenderedTextureInfoModel>();
        public List<SubpassDependencyModel> SubpassDependencyList { get; set; } = new List<SubpassDependencyModel>();

        public RenderPassModel() 
        {
        }

        public RenderPassModel(string jsonFilePath) : base()
        {
            LoadJsonComponent(jsonFilePath);
        }

        public RenderPassModel(string name, string jsonFilePath) : base(name)
        {
           // LoadJsonComponent(@"C:\Users\dotha\Documents\GitHub\VulkanGameEngine\RenderPass\RenderPass\DefaultSubpassDependency.json");
        }

        public void ConvertToVulkan()
        {
            throw new NotImplementedException();
        }

        public void LoadJsonComponent(string jsonPath)
        {
            var obj = base.LoadJsonComponent<SubpassDependencyModel>(jsonPath);
            foreach (PropertyInfo property in typeof(SubpassDependencyModel).GetProperties())
            {
                if (property.CanWrite)
                {
                    property.SetValue(this, property.GetValue(obj));
                }
            }
        }
    }
}
