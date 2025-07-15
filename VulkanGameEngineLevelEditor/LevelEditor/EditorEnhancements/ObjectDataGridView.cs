using GlmSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.GameEngine.GameObjectComponents;
using VulkanGameEngineLevelEditor.GameEngine.Structs;
using VulkanGameEngineLevelEditor.GameEngineAPI;
using VulkanGameEngineLevelEditor.LevelEditor.Commands;
using VulkanGameEngineLevelEditor.Models;

public class ObjectDataGridView : DataGridView
{
    private object selectedObject;
    private readonly Stack<ICommand> undoStack = new Stack<ICommand>();
    private readonly Stack<ICommand> redoStack = new Stack<ICommand>();
    private readonly ToolTip toolTip = new ToolTip();
    private string jsonFilePath;
    private DataTable dataTable;
    private Dictionary<string, bool> categoryExpanded = new Dictionary<string, bool>();

    public ObjectDataGridView()
    {
        InitializeGrid();
        DoubleBuffered = true; // Reduce flicker
    }

    public object SelectedObject
    {
        get => selectedObject;
        set
        {
            selectedObject = value;
            if (selectedObject is INotifyPropertyChanged inpc)
            {
                inpc.PropertyChanged += (s, e) => PopulateDataTable(); 
            }
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
      //  UpdateObjectFromDataTable();
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
        DefaultCellStyle.SelectionBackColor = Color.FromArgb(60, 60, 60);
        DefaultCellStyle.SelectionForeColor = Color.White;
        ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(50, 50, 50);
        ColumnHeadersDefaultCellStyle.Font = new Font(Font, FontStyle.Bold);

        dataTable = new DataTable();
        dataTable.Columns.Add("Property", typeof(string));
        dataTable.Columns.Add("Value", typeof(string));
        dataTable.Columns.Add("PropertyType", typeof(Type)); // Hidden
        dataTable.Columns.Add("Category", typeof(string));  // Hidden
        dataTable.Columns.Add("PropertyName", typeof(string)); // Hidden
        dataTable.Columns.Add("IsCategory", typeof(bool)); // Hidden, for category rows

        Columns.Add("Property", "Property");
        Columns.Add("Value", "Value");
        Columns[0].ReadOnly = true;
        Columns[0].Width = 200;
        Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        Columns[0].DataPropertyName = "Property";
        Columns[1].DataPropertyName = "Value";

       // CellClick += ObjectDataGridView_CellClick; // For category toggling
        CellBeginEdit += ObjectDataGridView_CellBeginEdit;
        CellEndEdit += ObjectDataGridView_CellEndEdit;
        CellMouseEnter += ObjectDataGridView_CellMouseEnter;
    }

    private void PopulateDataTable()
    {
        dataTable.Rows.Clear();
        if (selectedObject == null) return;

        var properties = GetCachedProperties(selectedObject.GetType());
        string currentCategory = null;
        foreach (var prop in properties)
        {
            var categoryAttr = prop.GetCustomAttribute<CategoryAttribute>();
            string category = categoryAttr?.Category ?? "General";
            var tooltipAttr = prop.GetCustomAttribute<TooltipAttribute>();
            var rangeAttr = prop.GetCustomAttribute<RangeAttribute>();

            if (currentCategory != category)
            {
                currentCategory = category;
                bool isExpanded = categoryExpanded.ContainsKey(category) ? categoryExpanded[category] : true;
                dataTable.Rows.Add($"[{category}]", isExpanded ? "-" : "+", null, category, null, true);
                if (!isExpanded) continue;
            }

            if (prop.GetCustomAttribute<BrowsableAttribute>()?.Browsable == false) continue;

            var displayName = prop.Name;
            object value = prop.GetValue(selectedObject);
            dataTable.Rows.Add($"  {displayName}", FormatValue(value, prop.PropertyType), prop.PropertyType, category, prop.Name, false);
        }
    }

    private string FormatValue(object value, Type propertyType)
    {
        if (value == null) return string.Empty;
        if (propertyType == typeof(Transform2DComponent)) return value.ToString();
        if (propertyType == typeof(vec2)) return $"({((vec2)value).x:F2}, {((vec2)value).y:F2})";
        if (propertyType == typeof(ivec2)) return $"({((ivec2)value).x}, {((ivec2)value).y})";
        if (propertyType == typeof(Sprite.SpriteAnimationEnum)) return value.ToString();
        if (propertyType == typeof(List<ComponentTypeEnum>)) return $"[{((List<ComponentTypeEnum>)value).Count} components]";
        if (propertyType.IsEnum) return value.ToString();
        return value?.ToString() ?? string.Empty;
    }

    private void ObjectDataGridView_CellClick(object sender, DataGridViewCellCancelEventArgs e)
    {
        if (e.RowIndex < 0 || !Convert.ToBoolean(dataTable.Rows[e.RowIndex]["IsCategory"])) return;

        string category = dataTable.Rows[e.RowIndex]["Category"].ToString();
        bool isExpanded = dataTable.Rows[e.RowIndex]["Value"].ToString() == "-";
        categoryExpanded[category] = !isExpanded;
        dataTable.Rows[e.RowIndex]["Value"] = isExpanded ? "+" : "-";
        PopulateDataTable(); // Rebuild to hide/show properties
    }

    private void ObjectDataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
        if (selectedObject == null) return;

        string propName = dataTable.Rows[e.RowIndex]["PropertyName"].ToString();
        var propInfo = selectedObject.GetType().GetProperty(propName);
        var rangeAttr = propInfo?.GetCustomAttribute<RangeAttribute>();

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
            var dialog = new Vec2EditDialog(currentValue.x, currentValue.y, rangeAttr?.Min ?? float.MinValue, rangeAttr?.Max ?? float.MaxValue);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var newValue = propInfo.PropertyType == typeof(vec2)
                    ? new vec2((float)dialog.X, (float)dialog.Y)
                    : new ivec2((int)dialog.X, (int)dialog.Y);
                var command = new PropertyChangeCommand(selectedObject, propInfo, currentValue, newValue);
                command.Execute();
                undoStack.Push(command);
                redoStack.Clear();
                dataTable.Rows[e.RowIndex]["Value"] = FormatValue(newValue, propInfo.PropertyType);
            }
            e.Cancel = true;
        }
        else if (propInfo?.PropertyType == typeof(float) && rangeAttr != null)
        {
            var currentValue = (float)propInfo.GetValue(selectedObject);
            var dialog = new RangeEditDialog(currentValue, rangeAttr.Min, rangeAttr.Max);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var newValue = (float)dialog.Value;
                var command = new PropertyChangeCommand(selectedObject, propInfo, currentValue, newValue);
                command.Execute();
                undoStack.Push(command);
                redoStack.Clear();
                dataTable.Rows[e.RowIndex]["Value"] = newValue.ToString("F2");
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
        if (e.RowIndex < 0 || Convert.ToBoolean(dataTable.Rows[e.RowIndex]["IsCategory"])) return;

        string propName = dataTable.Rows[e.RowIndex]["PropertyName"].ToString();
        var prop = selectedObject?.GetType().GetProperty(propName);
        var tooltipAttr = prop?.GetCustomAttribute<TooltipAttribute>();
        if (tooltipAttr != null)
        {
            toolTip.Show(tooltipAttr.Tooltip, this, PointToClient(Cursor.Position), 2000);
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

// Custom Attributes
[AttributeUsage(AttributeTargets.Property)]
public class TooltipAttribute : Attribute
{
    public string Tooltip { get; }

    public TooltipAttribute(string tooltip)
    {
        Tooltip = tooltip;
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class RangeAttribute : Attribute
{
    public float Min { get; }
    public float Max { get; }

    public RangeAttribute(float min, float max)
    {
        Min = min;
        Max = max;
    }
}

// Dialogs
public class Vec2EditDialog : Form
{
    private TextBox xTextBox;
    private TextBox yTextBox;
    public float X { get; private set; }
    public float Y { get; private set; }
    private float minValue, maxValue;

    public Vec2EditDialog(float x, float y, float min, float max)
    {
        X = x;
        Y = y;
        minValue = min;
        maxValue = max;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.Text = "Edit Vector";
        this.Size = new Size(250, 180);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.StartPosition = FormStartPosition.CenterParent;

        Label xLabel = new Label { Text = "X:", Location = new Point(10, 20), Size = new Size(30, 20) };
        xTextBox = new TextBox { Text = X.ToString("F2"), Location = new Point(50, 20), Size = new Size(100, 20) };

        Label yLabel = new Label { Text = "Y:", Location = new Point(10, 50), Size = new Size(30, 20) };
        yTextBox = new TextBox { Text = Y.ToString("F2"), Location = new Point(50, 50), Size = new Size(100, 20) };

        Button okButton = new Button { Text = "OK", Location = new Point(50, 100), Size = new Size(75, 30) };
        okButton.Click += (s, e) =>
        {
            if (float.TryParse(xTextBox.Text, out float x) && float.TryParse(yTextBox.Text, out float y))
            {
                if (x >= minValue && x <= maxValue && y >= minValue && y <= maxValue)
                {
                    X = x;
                    Y = y;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show($"Values must be between {minValue} and {maxValue}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter valid numbers.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        };

        Button cancelButton = new Button { Text = "Cancel", Location = new Point(130, 100), Size = new Size(75, 30) };
        cancelButton.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

        this.Controls.AddRange(new Control[] { xLabel, xTextBox, yLabel, yTextBox, okButton, cancelButton });
    }
}

public class RangeEditDialog : Form
{
    private TextBox valueTextBox;
    public float Value { get; private set; }
    private float minValue, maxValue;

    public RangeEditDialog(float value, float min, float max)
    {
        Value = value;
        minValue = min;
        maxValue = max;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.Text = "Edit Range";
        this.Size = new Size(200, 150);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.StartPosition = FormStartPosition.CenterParent;

        Label valueLabel = new Label { Text = "Value:", Location = new Point(10, 20), Size = new Size(40, 20) };
        valueTextBox = new TextBox { Text = Value.ToString("F2"), Location = new Point(60, 20), Size = new Size(100, 20) };

        Button okButton = new Button { Text = "OK", Location = new Point(60, 60), Size = new Size(75, 30) };
        okButton.Click += (s, e) =>
        {
            if (float.TryParse(valueTextBox.Text, out float value) && value >= minValue && value <= maxValue)
            {
                Value = value;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show($"Value must be between {minValue} and {maxValue}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        };

        Button cancelButton = new Button { Text = "Cancel", Location = new Point(140, 60), Size = new Size(75, 30) };
        cancelButton.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

        this.Controls.AddRange(new Control[] { valueLabel, valueTextBox, okButton, cancelButton });
    }
}