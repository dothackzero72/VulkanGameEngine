using GlmSharp;
using Newtonsoft.Json;
using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VulkanGameEngineLevelEditor.LevelEditor.Commands;
using VulkanGameEngineLevelEditor.LevelEditor.Dialog;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace VulkanGameEngineLevelEditor.LevelEditor.EditorEnhancements
{
    public class ObjectDataGridView : DataGridView
    {
        private object selectedObject;
        private readonly Stack<ICommand> undoStack = new Stack<ICommand>();
        private readonly Stack<ICommand> redoStack = new Stack<ICommand>();
        private readonly System.Windows.Forms.ToolTip toolTip = new System.Windows.Forms.ToolTip();
        private string jsonFilePath;


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
                PopulateGrid();
            }
        }

        public void LoadFromJson<T>(string filePath)
        {
            jsonFilePath = filePath;
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

        public void SaveToJson()
        {
            if (selectedObject == null || string.IsNullOrEmpty(jsonFilePath))
            {
                return;
            }
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

            Columns.Add("Property", "Property");
            Columns.Add("Value", "Value");
            Columns[0].ReadOnly = true;

            CellBeginEdit += ObjectDataGridView_CellBeginEdit;
            CellEndEdit += ObjectDataGridView_CellEndEdit;
            CellMouseEnter += ObjectDataGridView_CellMouseEnter;
        }

        private void PopulateGrid()
        {
            Rows.Clear();
            if (selectedObject == null) return;

            var properties = selectedObject.GetType().GetProperties()
                .Where(p => p.GetCustomAttribute<BrowsableAttribute>()?.Browsable != false)
                .OrderBy(p => p.GetCustomAttribute<CategoryAttribute>()?.Category ?? "General");

            foreach (var prop in properties)
            {
                var category = prop.GetCustomAttribute<CategoryAttribute>()?.Category ?? "General";
                var displayName = $"{category}: {prop.Name}";
                object value = prop.GetValue(selectedObject);
                Rows.Add(displayName, FormatValue(value, prop.PropertyType));
            }
        }
        private string FormatValue(object value, Type propertyType)
        {
            if (value == null) return string.Empty;
            if (propertyType == typeof(vec3)) return value.ToString();
            if (propertyType == typeof(Color)) return ((Color)value).Name;
            if (propertyType.IsEnum) return value.ToString();
            if (propertyType == typeof(List<RenderPass>)) return $"[{((List<RenderPass>)value).Count} passes]";
            return value.ToString();
        }

        private void ObjectDataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (selectedObject == null) return;

            string propName = Rows[e.RowIndex].Cells[0].Value.ToString().Split(':').Last().Trim();
            var propInfo = selectedObject.GetType().GetProperty(propName);

            if (propInfo.PropertyType == typeof(vec3))
            {
                e.Cancel = true;
                var currentValue = (vec3)propInfo.GetValue(selectedObject);
                var dialog = new Vec3EditDialog(currentValue);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var command = new PropertyChangeCommand(selectedObject, propInfo, currentValue, dialog.Result);
                    command.Execute();
                    undoStack.Push(command);
                    redoStack.Clear();
                    Rows[e.RowIndex].Cells[1].Value = FormatValue(dialog.Result, propInfo.PropertyType);
                }
            }
            else if (propInfo.PropertyType == typeof(Color))
            {
                e.Cancel = true;
                using (var colorDialog = new ColorDialog())
                {
                    colorDialog.Color = (Color)propInfo.GetValue(selectedObject);
                    if (colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        var command = new PropertyChangeCommand(selectedObject, propInfo, propInfo.GetValue(selectedObject), colorDialog.Color);
                        command.Execute();
                        undoStack.Push(command);
                        redoStack.Clear();
                        Rows[e.RowIndex].Cells[1].Value = FormatValue(colorDialog.Color, propInfo.PropertyType);
                    }
                }
            }
            else if (propInfo.PropertyType.IsEnum)
            {
                e.Cancel = true;
                var currentValue = propInfo.GetValue(selectedObject);
                var enumDialog = new EnumEditDialog(propInfo.PropertyType, currentValue);
                if (enumDialog.ShowDialog() == DialogResult.OK)
                {
                    var command = new PropertyChangeCommand(selectedObject, propInfo, currentValue, enumDialog.Result);
                    command.Execute();
                    undoStack.Push(command);
                    redoStack.Clear();
                    Rows[e.RowIndex].Cells[1].Value = FormatValue(enumDialog.Result, propInfo.PropertyType);
                }
            }
            //else if (propInfo.PropertyType == typeof(List<RenderPass>))
            //{
            //    e.Cancel = true;
            //    var currentValue = (List<RenderPass>)propInfo.GetValue(selectedObject);
            //    var dialog = new RenderPassListDialog(currentValue);
            //    if (dialog.ShowDialog() == DialogResult.OK)
            //    {
            //        var command = new PropertyChangeCommand(selectedObject, propInfo, currentValue, dialog.Result);
            //        command.Execute();
            //        undoStack.Push(command);
            //        redoStack.Clear();
            //        Rows[e.RowIndex].Cells[1].Value = FormatValue(dialog.Result, propInfo.PropertyType);
            //    }
            //}
        }

        private void ObjectDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (selectedObject == null) return;

            string propName = Rows[e.RowIndex].Cells[0].Value.ToString().Split(':').Last().Trim();
            string newValue = Rows[e.RowIndex].Cells[1].Value?.ToString();
            var propInfo = selectedObject.GetType().GetProperty(propName);

            if (propInfo == null || !propInfo.CanWrite) return;

            try
            {
                object oldValue = propInfo.GetValue(selectedObject);
                object convertedValue = ConvertValue(newValue, propInfo.PropertyType);
                var command = new PropertyChangeCommand(selectedObject, propInfo, oldValue, convertedValue);
                command.Execute();
                undoStack.Push(command);
                redoStack.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Invalid value: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Rows[e.RowIndex].Cells[1].Value = FormatValue(propInfo.GetValue(selectedObject), propInfo.PropertyType);
            }
        }

        private object ConvertValue(string input, Type targetType)
        {
            if (string.IsNullOrEmpty(input)) return null;

            if (targetType == typeof(string)) return input;
            if (targetType == typeof(int)) return int.Parse(input);
            if (targetType == typeof(float)) return float.Parse(input);
            if (targetType == typeof(bool)) return bool.Parse(input);
            if (targetType == typeof(Color)) return Color.FromName(input);
            if (targetType == typeof(Vector3))
            {
                var cleaned = input.Trim('(', ')').Split(',');
                if (cleaned.Length == 3)
                    return new Vector3(
                        float.Parse(cleaned[0].Trim()),
                        float.Parse(cleaned[1].Trim()),
                        float.Parse(cleaned[2].Trim())
                    );
                throw new FormatException("Vector3 format must be (x, y, z)");
            }

            throw new NotSupportedException($"Type {targetType.Name} is not supported.");
        }

        private void ObjectDataGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string propName = Rows[e.RowIndex].Cells[0].Value.ToString().Split(':').Last().Trim();
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
            PopulateGrid();
        }

        public void Redo()
        {
            if (redoStack.Count == 0) return;
            var command = redoStack.Pop();
            command.Execute();
            undoStack.Push(command);
            PopulateGrid();
        }

        public void RefreshGrid()
        {
            PopulateGrid();
        }
    }
}
