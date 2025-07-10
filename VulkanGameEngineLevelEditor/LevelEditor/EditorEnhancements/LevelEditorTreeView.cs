using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.LevelEditor.Commands;
using VulkanGameEngineLevelEditor.Models;

namespace VulkanGameEngineLevelEditor.LevelEditor.EditorEnhancements
{
    public class LevelEditorTreeView : TreeView
    {
        private object rootObject;
        private Action<object> onSelectionChanged;

        public LevelEditorTreeView()
        {
            BackColor = Color.FromArgb(30, 30, 30);
            Font = new Font("Microsoft Sans Serif", 12F);
            ForeColor = Color.White;
            Margin = new Padding(3, 4, 3, 4);
            TabIndex = 0;

            AfterSelect += LevelEditorTreeView_AfterSelect;
        }

        public object RootObject
        {
            get => rootObject;
            set
            {
                rootObject = value;
                PopulateTree();
            }
        }

        public event Action<object> SelectionChanged
        {
            add { onSelectionChanged += value; }
            remove { onSelectionChanged -= value; }
        }

        public void PopulateTree()
        {
            Nodes.Clear();
            if (rootObject is RenderPassBuildInfoModel renderPass)
            {
                renderPass.renderPipelineModelList ??= new List<RenderPipelineModel>();
                var rootNode = Nodes.Add("RenderPass");
                foreach (var renderPipeline in renderPass.renderPipelineModelList)
                {
                    if (renderPipeline != null && !string.IsNullOrEmpty(renderPipeline.Name))
                    {
                        rootNode.Nodes.Add(renderPipeline.Name);
                    }
                }
            }
        }

        public void AddRenderPipeline()
        {
            if (rootObject is RenderPassBuildInfoModel renderPass)
            {
                var newPipeline = new RenderPipelineModel($"Pipeline{renderPass.renderPipelineModelList.Count + 1}");
                var command = new AddRenderPipelineCommand(renderPass, newPipeline);
                try
                {
                    command.Execute();
                    var rootNode = Nodes.Count > 0 ? Nodes[0] : Nodes.Add("RenderPass"); // Ensure root node exists
                    rootNode.Nodes.Add(newPipeline.Name);
                    onSelectionChanged?.Invoke(newPipeline); // Select the new pipeline
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to add pipeline: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LevelEditorTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (rootObject is RenderPassBuildInfoModel renderPass && e.Node != null)
            {
                if (e.Node.Text == "RenderPass")
                {
                    onSelectionChanged?.Invoke(renderPass);
                }
                else
                {
                    var selectedPipeline = renderPass.renderPipelineModelList.FirstOrDefault(p => p?.Name == e.Node.Text);
                    if (selectedPipeline != null)
                    {
                        onSelectionChanged?.Invoke(selectedPipeline);
                    }
                    else
                    {
                        onSelectionChanged?.Invoke(renderPass);
                    }
                }
            }
        }
    }
}