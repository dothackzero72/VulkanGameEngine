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
    public class VkAttachmentDescriptionEditDialog : EditorDialog
    {
        public VkAttachmentDescription Result { get; private set; }
        private ComboBox flagsCombo, formatCombo, samplesCombo, loadOpCombo, storeOpCombo, stencilLoadOpCombo, stencilStoreOpCombo, initialLayoutCombo, finalLayoutCombo;

        public VkAttachmentDescriptionEditDialog(VkAttachmentDescription initialValue)
        {
            Text = "Edit VkAttachmentDescription";
            Size = new Size(300, 350);

            int y = 20;
            flagsCombo = CreateComboBox("flags", initialValue.flags, new Point(20, y)); y += 30;
            formatCombo = CreateComboBox("format", initialValue.format, new Point(20, y)); y += 30;
            samplesCombo = CreateComboBox("samples", initialValue.samples, new Point(20, y)); y += 30;
            loadOpCombo = CreateComboBox("loadOp", initialValue.loadOp, new Point(20, y)); y += 30;
            storeOpCombo = CreateComboBox("storeOp", initialValue.storeOp, new Point(20, y)); y += 30;
            stencilLoadOpCombo = CreateComboBox("stencilLoadOp", initialValue.stencilLoadOp, new Point(20, y)); y += 30;
            stencilStoreOpCombo = CreateComboBox("stencilStoreOp", initialValue.stencilStoreOp, new Point(20, y)); y += 30;
            initialLayoutCombo = CreateComboBox("initialLayout", initialValue.initialLayout, new Point(20, y)); y += 30;
            finalLayoutCombo = CreateComboBox("finalLayout", initialValue.finalLayout, new Point(20, y)); y += 30;

            var okButton = new Button { Text = "OK", Location = new Point(20, y), DialogResult = DialogResult.OK };
            okButton.Click += (s, e) =>
            {
                Result = new VkAttachmentDescription
                {
                    flags = (VkAttachmentDescriptionFlagBits)Enum.Parse(typeof(VkAttachmentDescriptionFlagBits), flagsCombo.SelectedItem.ToString()),
                    format = (VkFormat)Enum.Parse(typeof(VkFormat), formatCombo.SelectedItem.ToString()),
                    samples = (VkSampleCountFlagBits)Enum.Parse(typeof(VkSampleCountFlagBits), samplesCombo.SelectedItem.ToString()),
                    loadOp = (VkAttachmentLoadOp)Enum.Parse(typeof(VkAttachmentLoadOp), loadOpCombo.SelectedItem.ToString()),
                    storeOp = (VkAttachmentStoreOp)Enum.Parse(typeof(VkAttachmentStoreOp), storeOpCombo.SelectedItem.ToString()),
                    stencilLoadOp = (VkAttachmentLoadOp)Enum.Parse(typeof(VkAttachmentLoadOp), stencilLoadOpCombo.SelectedItem.ToString()),
                    stencilStoreOp = (VkAttachmentStoreOp)Enum.Parse(typeof(VkAttachmentStoreOp), stencilStoreOpCombo.SelectedItem.ToString()),
                    initialLayout = (VkImageLayout)Enum.Parse(typeof(VkImageLayout), initialLayoutCombo.SelectedItem.ToString()),
                    finalLayout = (VkImageLayout)Enum.Parse(typeof(VkImageLayout), finalLayoutCombo.SelectedItem.ToString())
                };
                DialogResult = DialogResult.OK;
            };

            var cancelButton = new Button { Text = "Cancel", Location = new Point(110, y), DialogResult = DialogResult.Cancel };
            Controls.AddRange(new Control[] { flagsCombo, formatCombo, samplesCombo, loadOpCombo, storeOpCombo, stencilLoadOpCombo, stencilStoreOpCombo, initialLayoutCombo, finalLayoutCombo, okButton, cancelButton });
        }
    }
}
