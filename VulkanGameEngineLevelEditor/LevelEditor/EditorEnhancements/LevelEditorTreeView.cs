using System.Windows.Forms;
using System.Xml.Linq;
using System;
using VulkanGameEngineLevelEditor.GameEngine.Structs;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using VulkanGameEngineLevelEditor.GameEngine.Systems;
using VulkanGameEngineLevelEditor.Models;
using VulkanGameEngineLevelEditor.LevelEditor.EditorEnhancements;

public class LevelEditorTreeView : TreeView
{
    private object rootObject;
    private Action<object> onSelectionChanged;
    private object lockObject = new object();
    public object selectedObject;
    public ObjectDataGridView dataGridView;

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
            TreeNode parentNode = Nodes.Count > 0 && Nodes[0].Text == "GameObjects" ? Nodes[0] : Nodes.Add("GameObjects");
            TreeNode gameObjectNode = parentNode.Nodes.Add(gameObject.Name);
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

    private void LevelEditorTreeView_AfterSelect(object sender, TreeViewEventArgs e)
    {
        if (rootObject is GameObject gameObject && e.Node != null)
        {
            object selectedObject = null;
            if (e.Node.Text == "GameObjects")
            {
                selectedObject = rootObject;
            }
            else if (e.Node.Parent?.Text == "GameObjects") 
            {
                if (e.Node.Tag is ComponentTypeEnum componentType && gameObject.Components.ContainsKey(componentType))
                {
                    selectedObject = gameObject.Components[componentType];
                }
            }
            else if (e.Node.Tag is int gameObjectId) 
            {
                selectedObject = GameObjectSystem.GameObjectMap[gameObjectId];
            }
            else if (e.Node.Tag is ComponentTypeEnum componentType && e.Node.Parent?.Tag is int parentId) 
            {
                var parentGameObject = GameObjectSystem.GameObjectMap[parentId];
                if (parentGameObject?.Components.ContainsKey(componentType) == true)
                {
                    selectedObject = parentGameObject.Components[componentType];
                }
            }

            onSelectionChanged?.Invoke(selectedObject);
        }
    }
}