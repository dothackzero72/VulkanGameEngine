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
    public class VkSubpassDependencyEditDialog : EditorDialog
    {
        public VkSubpassDependency Result { get; private set; }
        private NumericUpDown srcSubpassNum, dstSubpassNum;
        private ComboBox srcStageMaskCombo, dstStageMaskCombo, srcAccessMaskCombo, dstAccessMaskCombo, dependencyFlagsCombo;

        public VkSubpassDependencyEditDialog(VkSubpassDependency initialValue)
        {
            Text = "Edit VkSubpassDependency";
            Size = new Size(300, 300);

            int y = 20;
            srcSubpassNum = CreateNumericUpDown("srcSubpass", initialValue.srcSubpass, new Point(20, y), 0, 100); y += 30;
            dstSubpassNum = CreateNumericUpDown("dstSubpass", initialValue.dstSubpass, new Point(20, y), 0, 100); y += 30;
            srcStageMaskCombo = CreateComboBox("srcStageMask", initialValue.srcStageMask, new Point(20, y)); y += 30;
            dstStageMaskCombo = CreateComboBox("dstStageMask", initialValue.dstStageMask, new Point(20, y)); y += 30;
            srcAccessMaskCombo = CreateComboBox("srcAccessMask", initialValue.srcAccessMask, new Point(20, y)); y += 30;
            dstAccessMaskCombo = CreateComboBox("dstAccessMask", initialValue.dstAccessMask, new Point(20, y)); y += 30;
            dependencyFlagsCombo = CreateComboBox("dependencyFlags", initialValue.dependencyFlags, new Point(20, y)); y += 30;

            var okButton = new Button { Text = "OK", Location = new Point(20, y), DialogResult = DialogResult.OK };
            okButton.Click += (s, e) =>
            {
                Result = new VkSubpassDependency
                {
                    srcSubpass = (uint)srcSubpassNum.Value,
                    dstSubpass = (uint)dstSubpassNum.Value,
                    srcStageMask = (VkPipelineStageFlagBits)Enum.Parse(typeof(VkPipelineStageFlagBits), srcStageMaskCombo.SelectedItem.ToString()),
                    dstStageMask = (VkPipelineStageFlagBits)Enum.Parse(typeof(VkPipelineStageFlagBits), dstStageMaskCombo.SelectedItem.ToString()),
                    srcAccessMask = (VkAccessFlagBits)Enum.Parse(typeof(VkAccessFlagBits), srcAccessMaskCombo.SelectedItem.ToString()),
                    dstAccessMask = (VkAccessFlagBits)Enum.Parse(typeof(VkAccessFlagBits), dstAccessMaskCombo.SelectedItem.ToString()),
                    dependencyFlags = (VkDependencyFlagBits)Enum.Parse(typeof(VkDependencyFlagBits), dependencyFlagsCombo.SelectedItem.ToString())
                };
                DialogResult = DialogResult.OK;
            };

            var cancelButton = new Button { Text = "Cancel", Location = new Point(110, y), DialogResult = DialogResult.Cancel };
            Controls.AddRange(new Control[] { srcSubpassNum, dstSubpassNum, srcStageMaskCombo, dstStageMaskCombo, srcAccessMaskCombo, dstAccessMaskCombo, dependencyFlagsCombo, okButton, cancelButton });
        }
    }
}
