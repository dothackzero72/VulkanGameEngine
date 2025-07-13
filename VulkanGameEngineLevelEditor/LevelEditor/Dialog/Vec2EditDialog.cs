using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VulkanGameEngineLevelEditor.LevelEditor.Dialog
{
    public class Vec2EditDialog : Form
    {
        private TextBox xTextBox;
        private TextBox yTextBox;
        public float X { get; private set; }
        public float Y { get; private set; }

        public Vec2EditDialog(float x, float y)
        {
            X = x;
            Y = y;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Edit Vector";
            this.Size = new Size(200, 150);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            Label xLabel = new Label { Text = "X:", Location = new Point(10, 20), Size = new Size(30, 20) };
            xTextBox = new TextBox { Text = X.ToString(), Location = new Point(50, 20), Size = new Size(100, 20) };

            Label yLabel = new Label { Text = "Y:", Location = new Point(10, 50), Size = new Size(30, 20) };
            yTextBox = new TextBox { Text = Y.ToString(), Location = new Point(50, 50), Size = new Size(100, 20) };

            Button okButton = new Button { Text = "OK", Location = new Point(50, 80), Size = new Size(75, 30) };
            okButton.Click += (s, e) =>
            {
                if (float.TryParse(xTextBox.Text, out float x) && float.TryParse(yTextBox.Text, out float y))
                {
                    X = x;
                    Y = y;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Please enter valid numbers.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            Button cancelButton = new Button { Text = "Cancel", Location = new Point(130, 80), Size = new Size(75, 30) };
            cancelButton.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            this.Controls.AddRange(new Control[] { xLabel, xTextBox, yLabel, yTextBox, okButton, cancelButton });
        }
    }
}
