using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VulkanGameEngineLevelEditor.LevelEditor.Dialog
{
    public class EnumEditDialog : Form
    {
        public object Result { get; private set; }
        private ComboBox comboBox;

        public EnumEditDialog(Type enumType, object currentValue)
        {
            Text = "Select Enum Value";
            Size = new Size(200, 150);
            FormBorderStyle = FormBorderStyle.FixedDialog;

            comboBox = new ComboBox
            {
                Location = new Point(20, 20),
                Width = 140,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            comboBox.Items.AddRange(Enum.GetNames(enumType));
            comboBox.SelectedItem = currentValue.ToString();

            var okButton = new Button { Text = "OK", Location = new Point(60, 60), DialogResult = DialogResult.OK };
            okButton.Click += (s, e) => Result = Enum.Parse(enumType, comboBox.SelectedItem.ToString());

            Controls.AddRange(new Control[] { comboBox, okButton });
        }
    }
}
