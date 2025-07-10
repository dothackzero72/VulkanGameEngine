using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VulkanGameEngineLevelEditor.Models;

namespace VulkanGameEngineLevelEditor.LevelEditor.Commands
{
    public class AddRenderPipelineCommand : ICommand
    {
        private RenderPassBuildInfoModel target;
        private RenderPipelineModel pipeline;
        private int index;

        public AddRenderPipelineCommand(RenderPassBuildInfoModel target, RenderPipelineModel pipeline)
        {
            this.target = target ?? throw new ArgumentNullException(nameof(target));
            this.pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
            index = -1;
        }

        public void Execute()
        {
            target.renderPipelineModelList ??= new List<RenderPipelineModel>(); // Initialize if null
            index = target.renderPipelineModelList.Count;
            target.renderPipelineModelList.Add(pipeline);
            target.OnPropertyChanged(nameof(target.renderPipelineModelList)); // Notify property change
        }

        public void Undo()
        {
            if (index >= 0 && index < target.renderPipelineModelList.Count)
            {
                target.renderPipelineModelList.RemoveAt(index);
                target.OnPropertyChanged(nameof(target.renderPipelineModelList)); // Notify property change
            }
        }
    }
}
