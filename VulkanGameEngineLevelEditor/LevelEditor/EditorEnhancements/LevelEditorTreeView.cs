using System.Windows.Forms;
using System.Xml.Linq;
using System;
using VulkanGameEngineLevelEditor.GameEngine.Structs;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using VulkanGameEngineLevelEditor.GameEngine.Systems;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.LevelEditor.EditorEnhancements;
using VulkanGameEngineLevelEditor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

public class LevelEditorTreeView : System.Windows.Forms.TreeView
{
    private object rootObject;
    private Action<object> onSelectionChanged;
    private object lockObject = new object();
    public object selectedObject;
    public DynamicControlPanelView dynamicControlPanelView;

    public LevelEditorTreeView()
    {
        BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
        Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
        ForeColor = System.Drawing.Color.White;
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
        //if (this.InvokeRequired)
        //{
        //    this.Invoke(new Action(PopulateTree));
        //    return;
        //}

        //Nodes.Clear();
        //if (rootObject is GameObject gameObject)
        //{
        //    TreeNode rootNode;
        //    if (Nodes.Count > 0)
        //    {
        //        rootNode = Nodes[0];
        //    }
        //    else
        //    {
        //        rootNode = Nodes.Add(gameObject.Name);
        //    }

        //    rootNode.Nodes.Clear();
        //    foreach (var component in gameObject.GameObjectComponentTypeList)
        //    {
        //        switch (component)
        //        {
        //            case ComponentTypeEnum.kRenderMesh2DComponent:
        //                rootNode.Nodes.Add("RenderMesh2DComponent");
        //                break;
        //            case ComponentTypeEnum.kTransform2DComponent:
        //                rootNode.Nodes.Add("Transform2DComponent");
        //                break;
        //            case ComponentTypeEnum.kInputComponent:
        //                rootNode.Nodes.Add("InputComponent");
        //                break;
        //            case ComponentTypeEnum.kSpriteComponent:
        //                rootNode.Nodes.Add("SpriteComponent");
        //                break;
        //        }
        //    }
        //}
    }

    public void AddGameObject(GameObject gameObject)
    {
        if (this.InvokeRequired)
        {
            this.Invoke(new Action(() => AddGameObject(gameObject)));
            return;
        }

        try
        {
            //  this.Nodes.Clear();

            TreeNode gameObjectNode = Nodes.Add(gameObject.Name);
            gameObjectNode.Tag = gameObject.GameObjectId;

            foreach (var component in gameObject.GameObjectComponentTypeList)
            {
                switch (component)
                {
                    case ComponentTypeEnum.kRenderMesh2DComponent:
                        gameObjectNode.Nodes.Add("RenderMesh2DComponent");
                        break;
                    case ComponentTypeEnum.kTransform2DComponent:
                        gameObjectNode.Nodes.Add("Transform2DComponent");
                        break;
                    case ComponentTypeEnum.kInputComponent:
                        gameObjectNode.Nodes.Add("InputComponent");
                        break;
                    case ComponentTypeEnum.kSpriteComponent:
                        gameObjectNode.Nodes.Add("SpriteComponent");
                        break;
                }
            }

            // parentNode.Expand();

            onSelectionChanged?.Invoke(gameObject);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to add game object: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public void AddRenderPass(RenderPassLoaderModel renderPass)
    {
        if (this.InvokeRequired)
        {
            this.Invoke(new Action(() => AddRenderPass(renderPass)));
            return;
        }
        try
        {
            TreeNode renderPassNode = Nodes.Add("RenderPass");
            renderPassNode.Tag = renderPass.RenderPassId;

            for (int x = 0; x < renderPass.renderPipelineModelList.Count; x++)
            {
                renderPassNode.Nodes.Add($@"RenderPipelineModelList[{x}]").Tag = x;
            }
            for (int x = 0; x < renderPass.RenderedTextureInfoModelList.Count; x++)
            {
                renderPassNode.Nodes.Add($@"RenderedTextureInfoModelList[{x}]").Tag = x;
            }
            for (int x = 0; x < renderPass.SubpassDependencyList.Count; x++)
            {
                renderPassNode.Nodes.Add($@"SubpassDependencyList[{x}]").Tag = x;
            }
            for (int x = 0; x < renderPass.ClearValueList.Count; x++)
            {
                renderPassNode.Nodes.Add($@"ClearValue[{x}]").Tag = x;
            }

            onSelectionChanged?.Invoke(renderPass);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to add game object: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void LevelEditorTreeView_AfterSelect(object sender, TreeViewEventArgs e)
    {
        if (rootObject is GameObject gameObject)
        {
            if (e.Node?.Tag is int gameObjectId)
            {
                selectedObject = GameObjectSystem.GameObjectMap[gameObjectId];
                dynamicControlPanelView.SelectedObject = selectedObject;
            }
            else if (e.Node.Tag is ComponentTypeEnum componentType && e.Node.Parent?.Tag is int parentId) // Component under GameObject
            {
                var parentGameObject = GameObjectSystem.GameObjectMap[parentId];
                //if (parentGameObject?.Components.ContainsKey(componentType) == true)
                //{
                //    selectedObject = parentGameObject.Components[componentType];
                //}
            }
        }
        else if (rootObject is RenderPassLoaderModel)
        {
            var renderPassModel = (RenderPassLoaderModel)rootObject;
            if (e.Node?.Tag is Guid renderPassId)
            {
                selectedObject = RenderSystem.RenderPassEditor_RenderPass[renderPassId];
                dynamicControlPanelView.SelectedObject = selectedObject;
            }
        }
        else if (rootObject is RenderPipelineLoaderModel)
        {
            if (e.Node.Parent?.Tag is Guid renderPassId)
            {
                int tag = (int)e.Node.Tag;
                selectedObject = RenderSystem.RenderPassEditor_RenderPass[renderPassId].renderPipelineModelList[tag];
                dynamicControlPanelView.SelectedObject = selectedObject;
            }
        }
        else if (rootObject is RenderedTextureInfoModel)
        {
            if (e.Node.Parent?.Tag is Guid renderPassId)
            {
                int tag = (int)e.Node.Tag;
                selectedObject = RenderSystem.RenderPassEditor_RenderPass[renderPassId].RenderedTextureInfoModelList[tag];
                dynamicControlPanelView.SelectedObject = selectedObject;
            }
        }
        else if (rootObject is VkSubpassDependencyModel)
        {
            if (e.Node.Parent?.Tag is Guid renderPassId)
            {
                int tag = (int)e.Node.Tag;
                selectedObject = RenderSystem.RenderPassEditor_RenderPass[renderPassId].SubpassDependencyList[tag];
                dynamicControlPanelView.SelectedObject = selectedObject;
            }
        }
    }
}