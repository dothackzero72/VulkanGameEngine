using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VulkanGameEngineLevelEditor.LevelEditor.Dialog
{
    public abstract class EditorDialog : Form
    {
        protected EditorDialog()
        {
            BackColor = Color.FromArgb(30, 30, 30);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(400, 500);
        }

        protected ComboBox CreateComboBox<TEnum>(string name, TEnum value, Point location)
        {
            var comboBox = new ComboBox
            {
                Name = name,
                Location = location,
                Width = 150,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            comboBox.Items.AddRange(Enum.GetNames(typeof(TEnum)));
            comboBox.SelectedItem = value.ToString();
            return comboBox;
        }

        protected NumericUpDown CreateNumericUpDown(string name, decimal value, Point location, int min = 0, int max = 1000)
        {
            return new NumericUpDown
            {
                Name = name,
                Location = location,
                Width = 100,
                Value = value,
                Minimum = min,
                Maximum = max,
                DecimalPlaces = name.Contains("Lod") ? 2 : 0
            };
        }

        protected CheckBox CreateCheckBox(string name, bool value, Point location)
        {
            return new CheckBox
            {
                Name = name,
                Location = location,
                Width = 150,
                Text = name,
                Checked = value,
                ForeColor = Color.White
            };
        }
    }
}
