using GlmSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using System;
using VulkanGameEngineLevelEditor.GameEngine.GameObjectComponents;
using VulkanGameEngineLevelEditor.GameEngine.Structs;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.LevelEditor.Commands;
using VulkanGameEngineLevelEditor.LevelEditor.Dialog;
using System.IO;
using System.Drawing;
using System.Linq;

public class ObjectDataGridView : DataGridView
{
    private object selectedObject;
    private readonly Stack<ICommand> undoStack = new Stack<ICommand>();
    private readonly Stack<ICommand> redoStack = new Stack<ICommand>();
    private readonly ToolTip toolTip = new ToolTip();
    private string jsonFilePath;
    private DataTable dataTable;

    public ObjectDataGridView()
    {
        InitializeGrid();
    }

    public object SelectedObject
    {
        get => selectedObject;
        set
        {
            selectedObject = value;
            PopulateDataTable();
            DataSource = dataTable;
        }
    }

    public void LoadFromJson<T>(string filePath)
    {
        jsonFilePath = filePath;
        try
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                SelectedObject = JsonConvert.DeserializeObject<T>(json);
            }
            else
            {
                SelectedObject = Activator.CreateInstance<T>();
            }
        }
        catch (JsonException ex)
        {
            MessageBox.Show($"Failed to load JSON: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            SelectedObject = Activator.CreateInstance<T>();
        }
    }

    public void SaveToJson()
    {
        if (selectedObject == null || string.IsNullOrEmpty(jsonFilePath)) return;
        UpdateObjectFromDataTable();
        string json = JsonConvert.SerializeObject(selectedObject, Formatting.Indented);
        File.WriteAllText(jsonFilePath, json);
    }

    private void InitializeGrid()
    {
        Dock = DockStyle.Fill;
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        AllowUserToAddRows = false;
        RowHeadersVisible = false;
        BackgroundColor = Color.FromArgb(30, 30, 30);
        DefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
        DefaultCellStyle.ForeColor = Color.White;
        ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(50, 50, 50);

        dataTable = new DataTable();
        dataTable.Columns.Add("Property", typeof(string));
        dataTable.Columns.Add("Value", typeof(string));
        dataTable.Columns.Add("PropertyType", typeof(Type)); // Hidden
        dataTable.Columns.Add("Category", typeof(string));  // Hidden
        dataTable.Columns.Add("PropertyName", typeof(string)); // Hidden

        Columns.Add("Property", "Property");
        Columns.Add("Value", "Value");
        Columns[0].ReadOnly = true;
        Columns[0].DataPropertyName = "Property";
        Columns[1].DataPropertyName = "Value";

        CellBeginEdit += ObjectDataGridView_CellBeginEdit;
        CellEndEdit += ObjectDataGridView_CellEndEdit;
        CellMouseEnter += ObjectDataGridView_CellMouseEnter;
    }

    private void PopulateDataTable()
    {
        dataTable.Rows.Clear();
        if (selectedObject == null) return;

        var properties = GetCachedProperties(selectedObject.GetType());
        foreach (var prop in properties)
        {
            var category = prop.GetCustomAttribute<CategoryAttribute>()?.Category ?? "General";
            var displayName = $"{category}: {prop.Name}";
            object value = prop.GetValue(selectedObject);
            dataTable.Rows.Add(displayName, FormatValue(value, prop.PropertyType), prop.PropertyType, category, prop.Name);
        }
    }

    private string FormatValue(object value, Type propertyType)
    {
        if (value == null) return string.Empty;
        if (propertyType == typeof(Transform2DComponent)) return value.ToString();
        if (propertyType == typeof(vec2)) return $"({((vec2)value).x}, {((vec2)value).y})";
        if (propertyType == typeof(ivec2)) return $"({((ivec2)value).x}, {((ivec2)value).y})";
        if (propertyType == typeof(Sprite.SpriteAnimationEnum)) return value.ToString();
        if (propertyType == typeof(List<ComponentTypeEnum>)) return $"[{((List<ComponentTypeEnum>)value).Count} components]";
        if (propertyType.IsEnum) return value.ToString();
        return value?.ToString() ?? string.Empty;
    }

    private void UpdateObjectFromDataTable()
    {
        if (selectedObject == null) return;

        foreach (DataRow row in dataTable.Rows)
        {
            string propName = row["PropertyName"].ToString();
            var propInfo = selectedObject.GetType().GetProperty(propName);
            if (propInfo != null && propInfo.CanWrite)
            {
                try
                {
                    object newValue = ConvertValue(row["Value"].ToString(), propInfo.PropertyType);
                    if (newValue != null)
                    {
                        propInfo.SetValue(selectedObject, newValue);
                    }
                }
                catch
                {
                    // Skip invalid values
                }
            }
        }
    }

    private void ObjectDataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
        if (selectedObject == null) return;

        string propName = dataTable.Rows[e.RowIndex]["PropertyName"].ToString();
        var propInfo = selectedObject.GetType().GetProperty(propName);

        if (propInfo?.PropertyType.IsEnum == true)
        {
            var cell = new DataGridViewComboBoxCell { DataSource = Enum.GetNames(propInfo.PropertyType) };
            cell.Value = propInfo.GetValue(selectedObject)?.ToString();
            Rows[e.RowIndex].Cells[1] = cell;
        }
        else if (propInfo?.PropertyType == typeof(bool))
        {
            var cell = new DataGridViewCheckBoxCell();
            cell.Value = propInfo.GetValue(selectedObject);
            Rows[e.RowIndex].Cells[1] = cell;
        }
        else if (propInfo?.PropertyType == typeof(vec2) || propInfo?.PropertyType == typeof(ivec2))
        {
            var currentValue = (dynamic)propInfo.GetValue(selectedObject);
            var dialog = new Vec2EditDialog(currentValue.x, currentValue.y);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var newValue = propInfo.PropertyType == typeof(vec2)
                    ? new vec2(dialog.X, dialog.Y)
                    : new ivec2((int)dialog.X, (int)dialog.Y);
                var command = new PropertyChangeCommand(selectedObject, propInfo, currentValue, newValue);
                command.Execute();
                undoStack.Push(command);
                redoStack.Clear();
                dataTable.Rows[e.RowIndex]["Value"] = FormatValue(newValue, propInfo.PropertyType);
            }
            e.Cancel = true;
        }
    }

    private void ObjectDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
        if (selectedObject == null) return;

        string propName = dataTable.Rows[e.RowIndex]["PropertyName"].ToString();
        string newValue = dataTable.Rows[e.RowIndex]["Value"]?.ToString();
        var propInfo = selectedObject.GetType().GetProperty(propName);

        if (propInfo == null || !propInfo.CanWrite) return;

        try
        {
            object oldValue = propInfo.GetValue(selectedObject);
            object convertedValue = ConvertValue(newValue, propInfo.PropertyType);
            if (convertedValue != null && !Equals(oldValue, convertedValue))
            {
                var command = new PropertyChangeCommand(selectedObject, propInfo, oldValue, convertedValue);
                command.Execute();
                undoStack.Push(command);
                redoStack.Clear();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Invalid value: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            dataTable.Rows[e.RowIndex]["Value"] = FormatValue(propInfo.GetValue(selectedObject), propInfo.PropertyType);
        }
    }

    private object ConvertValue(string input, Type targetType)
    {
        if (string.IsNullOrEmpty(input)) return null;

        if (targetType == typeof(string)) return input;
        if (targetType == typeof(int)) return int.TryParse(input, out int result) ? result : 0;
        if (targetType == typeof(float)) return float.TryParse(input, out float result) ? result : 0f;
        if (targetType == typeof(bool)) return bool.TryParse(input, out bool result) ? result : false;
        if (targetType.IsEnum) return Enum.TryParse(targetType, input, out object result) ? result : Activator.CreateInstance(targetType);
        if (targetType == typeof(vec2))
        {
            var parts = input.Replace("(", "").Replace(")", "").Split(',');
            if (parts.Length == 2 && float.TryParse(parts[0].Trim(), out float x) && float.TryParse(parts[1].Trim(), out float y))
                return new vec2(x, y);
        }
        if (targetType == typeof(ivec2))
        {
            var parts = input.Replace("(", "").Replace(")", "").Split(',');
            if (parts.Length == 2 && int.TryParse(parts[0].Trim(), out int x) && int.TryParse(parts[1].Trim(), out int y))
                return new ivec2(x, y);
        }
        return null;
    }

    private void ObjectDataGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        string propName = dataTable.Rows[e.RowIndex]["PropertyName"].ToString();
        var prop = selectedObject?.GetType().GetProperty(propName);
        var description = prop?.GetCustomAttribute<DescriptionAttribute>()?.Description;
        if (!string.IsNullOrEmpty(description))
        {
            toolTip.Show(description, this, PointToClient(Cursor.Position), 2000);
        }
    }

    public void Undo()
    {
        if (undoStack.Count == 0) return;
        var command = undoStack.Pop();
        command.Undo();
        redoStack.Push(command);
        PopulateDataTable();
        DataSource = dataTable;
    }

    public void Redo()
    {
        if (redoStack.Count == 0) return;
        var command = redoStack.Pop();
        command.Execute();
        undoStack.Push(command);
        PopulateDataTable();
        DataSource = dataTable;
    }

    public void RefreshGrid()
    {
        PopulateDataTable();
        DataSource = dataTable;
    }

    private readonly Dictionary<Type, PropertyInfo[]> propertyCache = new Dictionary<Type, PropertyInfo[]>();
    private PropertyInfo[] GetCachedProperties(Type type)
    {
        if (!propertyCache.TryGetValue(type, out var properties))
        {
            properties = type.GetProperties()
                .Where(p => p.GetCustomAttribute<BrowsableAttribute>()?.Browsable != false)
                .OrderBy(p => p.GetCustomAttribute<CategoryAttribute>()?.Category ?? "General")
                .ToArray();
            propertyCache[type] = properties;
        }
        return properties;
    }
}