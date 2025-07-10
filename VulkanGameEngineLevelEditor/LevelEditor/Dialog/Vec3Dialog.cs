using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;
using GlmSharp;

namespace VulkanGameEngineLevelEditor.LevelEditor.Dialog
{
    public class Vec3EditDialog : Form
    {
        public vec3 Result { get; private set; }
        private NumericUpDown numX, numY, numZ;

        public Vec3EditDialog(vec3 initialValue)
        {
            Text = "Edit Vector3";
            Size = new Size(300, 150);
            FormBorderStyle = FormBorderStyle.FixedDialog;

            numX = new NumericUpDown { Location = new Point(20, 20), Width = 80, Value = (decimal)initialValue.x, DecimalPlaces = 2, Minimum = -1000, Maximum = 1000 };
            numY = new NumericUpDown { Location = new Point(110, 20), Width = 80, Value = (decimal)initialValue.y, DecimalPlaces = 2, Minimum = -1000, Maximum = 1000 };
            numZ = new NumericUpDown { Location = new Point(200, 20), Width = 80, Value = (decimal)initialValue.z, DecimalPlaces = 2, Minimum = -1000, Maximum = 1000 };

            var okButton = new Button { Text = "OK", Location = new Point(110, 60), DialogResult = DialogResult.OK };
            okButton.Click += (s, e) => Result = new vec3((float)numX.Value, (float)numY.Value, (float)numZ.Value);

            Controls.AddRange(new Control[] { numX, numY, numZ, okButton });
        }
    }
}
