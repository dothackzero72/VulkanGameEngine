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
    public class VkSamplerCreateInfoEditDialog : EditorDialog
    {
        public VkSamplerCreateInfo Result { get; private set; }
        private ComboBox sTypeCombo, flagsCombo, magFilterCombo, minFilterCombo, mipmapModeCombo, addressModeUCombo, addressModeVCombo, addressModeWCombo, compareOpCombo, borderColorCombo;
        private NumericUpDown mipLodBiasNum, maxAnisotropyNum, minLodNum, maxLodNum;
        private CheckBox anisotropyEnableCheck, compareEnableCheck, unnormalizedCoordinatesCheck;

        public VkSamplerCreateInfoEditDialog(VkSamplerCreateInfo initialValue)
        {
            Text = "Edit VkSamplerCreateInfo";
            int y = 20;

            sTypeCombo = CreateComboBox("sType", initialValue.sType, new Point(20, y)); y += 30;
            flagsCombo = CreateComboBox("flags", initialValue.flags, new Point(20, y)); y += 30;
            magFilterCombo = CreateComboBox("magFilter", initialValue.magFilter, new Point(20, y)); y += 30;
            minFilterCombo = CreateComboBox("minFilter", initialValue.minFilter, new Point(20, y)); y += 30;
            mipmapModeCombo = CreateComboBox("mipmapMode", initialValue.mipmapMode, new Point(20, y)); y += 30;
            addressModeUCombo = CreateComboBox("addressModeU", initialValue.addressModeU, new Point(20, y)); y += 30;
            addressModeVCombo = CreateComboBox("addressModeV", initialValue.addressModeV, new Point(20, y)); y += 30;
            addressModeWCombo = CreateComboBox("addressModeW", initialValue.addressModeW, new Point(20, y)); y += 30;
            mipLodBiasNum = CreateNumericUpDown("mipLodBias", (decimal)initialValue.mipLodBias, new Point(20, y), -16, 16); y += 30;
            anisotropyEnableCheck = CreateCheckBox("anisotropyEnable", initialValue.anisotropyEnable, new Point(20, y)); y += 30;
            maxAnisotropyNum = CreateNumericUpDown("maxAnisotropy", (decimal)initialValue.maxAnisotropy, new Point(20, y), 0, 16); y += 30;
            compareEnableCheck = CreateCheckBox("compareEnable", initialValue.compareEnable, new Point(20, y)); y += 30;
            compareOpCombo = CreateComboBox("compareOp", initialValue.compareOp, new Point(20, y)); y += 30;
            minLodNum = CreateNumericUpDown("minLod", (decimal)initialValue.minLod, new Point(20, y), -1000, 1000); y += 30;
            maxLodNum = CreateNumericUpDown("maxLod", (decimal)initialValue.maxLod, new Point(20, y), -1000, 1000); y += 30;
            borderColorCombo = CreateComboBox("borderColor", initialValue.borderColor, new Point(20, y)); y += 30;
            unnormalizedCoordinatesCheck = CreateCheckBox("unnormalizedCoordinates", initialValue.unnormalizedCoordinates, new Point(20, y)); y += 30;

            var okButton = new Button { Text = "OK", Location = new Point(20, y), DialogResult = DialogResult.OK };
            okButton.Click += (s, e) =>
            {
                Result = new VkSamplerCreateInfo
                {
                    sType = (VkStructureType)Enum.Parse(typeof(VkStructureType), sTypeCombo.SelectedItem.ToString()),
                    pNext = null, // Not editable
                    flags = (VkSamplerCreateFlagBits)Enum.Parse(typeof(VkSamplerCreateFlagBits), flagsCombo.SelectedItem.ToString()),
                    magFilter = (VkFilter)Enum.Parse(typeof(VkFilter), magFilterCombo.SelectedItem.ToString()),
                    minFilter = (VkFilter)Enum.Parse(typeof(VkFilter), minFilterCombo.SelectedItem.ToString()),
                    mipmapMode = (VkSamplerMipmapMode)Enum.Parse(typeof(VkSamplerMipmapMode), mipmapModeCombo.SelectedItem.ToString()),
                    addressModeU = (VkSamplerAddressMode)Enum.Parse(typeof(VkSamplerAddressMode), addressModeUCombo.SelectedItem.ToString()),
                    addressModeV = (VkSamplerAddressMode)Enum.Parse(typeof(VkSamplerAddressMode), addressModeVCombo.SelectedItem.ToString()),
                    addressModeW = (VkSamplerAddressMode)Enum.Parse(typeof(VkSamplerAddressMode), addressModeWCombo.SelectedItem.ToString()),
                    mipLodBias = (float)mipLodBiasNum.Value,
                    anisotropyEnable = anisotropyEnableCheck.Checked,
                    maxAnisotropy = (float)maxAnisotropyNum.Value,
                    compareEnable = compareEnableCheck.Checked,
                    compareOp = (VkCompareOp)Enum.Parse(typeof(VkCompareOp), compareOpCombo.SelectedItem.ToString()),
                    minLod = (float)minLodNum.Value,
                    maxLod = (float)maxLodNum.Value,
                    borderColor = (VkBorderColor)Enum.Parse(typeof(VkBorderColor), borderColorCombo.SelectedItem.ToString()),
                    unnormalizedCoordinates = unnormalizedCoordinatesCheck.Checked
                };
                DialogResult = DialogResult.OK;
            };

            var cancelButton = new Button { Text = "Cancel", Location = new Point(110, y), DialogResult = DialogResult.Cancel };
            Controls.AddRange(new Control[] { sTypeCombo, flagsCombo, magFilterCombo, minFilterCombo, mipmapModeCombo, addressModeUCombo, addressModeVCombo, addressModeWCombo, mipLodBiasNum, anisotropyEnableCheck, maxAnisotropyNum, compareEnableCheck, compareOpCombo, minLodNum, maxLodNum, borderColorCombo, unnormalizedCoordinatesCheck, okButton, cancelButton });
        }
    }
}
