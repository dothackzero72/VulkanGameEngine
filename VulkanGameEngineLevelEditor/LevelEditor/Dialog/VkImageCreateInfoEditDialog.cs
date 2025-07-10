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
    public class VkImageCreateInfoEditDialog : EditorDialog
    {
        public VkImageCreateInfo Result { get; private set; }
        private ComboBox sTypeCombo, flagsCombo, imageTypeCombo, formatCombo, samplesCombo, tilingCombo, usageCombo, sharingModeCombo, initialLayoutCombo;
        private NumericUpDown mipLevelsNum, arrayLayersNum, queueFamilyIndexCountNum;
        private TextBox queueFamilyIndicesText;
        private Button extentButton;

        public VkImageCreateInfoEditDialog(VkImageCreateInfo initialValue)
        {
            Text = "Edit VkImageCreateInfo";
            int y = 20;

            sTypeCombo = CreateComboBox("sType", initialValue.sType, new Point(20, y)); y += 30;
            flagsCombo = CreateComboBox("flags", initialValue.flags, new Point(20, y)); y += 30;
            imageTypeCombo = CreateComboBox("imageType", initialValue.imageType, new Point(20, y)); y += 30;
            formatCombo = CreateComboBox("format", initialValue.format, new Point(20, y)); y += 30;
            extentButton = new Button { Text = "Edit Extent", Location = new Point(20, y), Width = 150 };
            extentButton.Click += (s, e) =>
            {
                var dialog = new VkExtent3DEditDialog(initialValue.extent);
                if (dialog.ShowDialog() == DialogResult.OK)
                    initialValue.extent = dialog.Result;
            }; y += 30;
            mipLevelsNum = CreateNumericUpDown("mipLevels", initialValue.mipLevels, new Point(20, y), 1, 16); y += 30;
            arrayLayersNum = CreateNumericUpDown("arrayLayers", initialValue.arrayLayers, new Point(20, y), 1, 1024); y += 30;
            samplesCombo = CreateComboBox("samples", initialValue.samples, new Point(20, y)); y += 30;
            tilingCombo = CreateComboBox("tiling", initialValue.tiling, new Point(20, y)); y += 30;
            usageCombo = CreateComboBox("usage", initialValue.usage, new Point(20, y)); y += 30;
            sharingModeCombo = CreateComboBox("sharingMode", initialValue.sharingMode, new Point(20, y)); y += 30;
            queueFamilyIndexCountNum = CreateNumericUpDown("queueFamilyIndexCount", initialValue.queueFamilyIndexCount, new Point(20, y), 0, 16); y += 30;
            queueFamilyIndicesText = new TextBox { Location = new Point(20, y), Width = 150, Text = "0" }; y += 30;
            initialLayoutCombo = CreateComboBox("initialLayout", initialValue.initialLayout, new Point(20, y)); y += 30;

            var okButton = new Button { Text = "OK", Location = new Point(20, y), DialogResult = DialogResult.OK };
            okButton.Click += (s, e) =>
            {
                Result = new VkImageCreateInfo
                {
                    sType = (VkStructureType)Enum.Parse(typeof(VkStructureType), sTypeCombo.SelectedItem.ToString()),
                    pNext = null, // Not editable
                    flags = (VkImageCreateFlagBits)Enum.Parse(typeof(VkImageCreateFlagBits), flagsCombo.SelectedItem.ToString()),
                    imageType = (VkImageType)Enum.Parse(typeof(VkImageType), imageTypeCombo.SelectedItem.ToString()),
                    format = (VkFormat)Enum.Parse(typeof(VkFormat), formatCombo.SelectedItem.ToString()),
                    extent = initialValue.extent, // Updated via dialog
                    mipLevels = (uint)mipLevelsNum.Value,
                    arrayLayers = (uint)arrayLayersNum.Value,
                    samples = (VkSampleCountFlagBits)Enum.Parse(typeof(VkSampleCountFlagBits), samplesCombo.SelectedItem.ToString()),
                    tiling = (VkImageTiling)Enum.Parse(typeof(VkImageTiling), tilingCombo.SelectedItem.ToString()),
                    usage = (VkImageUsageFlagBits)Enum.Parse(typeof(VkImageUsageFlagBits), usageCombo.SelectedItem.ToString()),
                    sharingMode = (VkSharingMode)Enum.Parse(typeof(VkSharingMode), sharingModeCombo.SelectedItem.ToString()),
                    queueFamilyIndexCount = (uint)queueFamilyIndexCountNum.Value,
                    pQueueFamilyIndices = null, // Simplified for JSON
                    initialLayout = (VkImageLayout)Enum.Parse(typeof(VkImageLayout), initialLayoutCombo.SelectedItem.ToString())
                };
                DialogResult = DialogResult.OK;
            };

            var cancelButton = new Button { Text = "Cancel", Location = new Point(110, y), DialogResult = DialogResult.Cancel };
            Controls.AddRange(new Control[] { sTypeCombo, flagsCombo, imageTypeCombo, formatCombo, extentButton, mipLevelsNum, arrayLayersNum, samplesCombo, tilingCombo, usageCombo, sharingModeCombo, queueFamilyIndexCountNum, queueFamilyIndicesText, initialLayoutCombo, okButton, cancelButton });
        }
    }
}
