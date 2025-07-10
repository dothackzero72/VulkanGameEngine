using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vulkan;

namespace VulkanGameEngineLevelEditor.LevelEditor.Dialog
{
    public class VkExtent3DEditDialog : EditorDialog
    {
        public VkExtent3D Result { get; private set; }
        private NumericUpDown numWidth, numHeight, numDepth;

        public VkExtent3DEditDialog(VkExtent3D initialValue)
        {
            Text = "Edit VkExtent3D";
            Size = new Size(300, 150);

            numWidth = CreateNumericUpDown("Width", initialValue.width, new Point(20, 20), 1, 4096);
            numHeight = CreateNumericUpDown("Height", initialValue.height, new Point(110, 20), 1, 4096);
            numDepth = CreateNumericUpDown("Depth", initialValue.depth, new Point(200, 20), 1, 4096);

            var okButton = new Button { Text = "OK", Location = new Point(110, 60), DialogResult = DialogResult.OK };
            okButton.Click += (s, e) =>
            {
                Result = new VkExtent3D
                {
                    width = (uint)numWidth.Value,
                    height = (uint)numHeight.Value,
                    depth = (uint)numDepth.Value
                };
                DialogResult = DialogResult.OK;
            };

            var cancelButton = new Button { Text = "Cancel", Location = new Point(200, 60), DialogResult = DialogResult.Cancel };
            Controls.AddRange(new Control[] { numWidth, numHeight, numDepth, okButton, cancelButton });
        }
    }
}
