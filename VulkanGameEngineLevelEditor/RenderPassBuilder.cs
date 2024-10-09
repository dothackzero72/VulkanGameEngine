using Silk.NET.Vulkan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VulkanGameEngineLevelEditor
{
    public partial class RenderPassBuilder : Form
    {
        public RenderPassBuilder()
        {
            InitializeComponent();
            FillTableLayoutPanelWithImageTypes();
        }

        public void ShowRenderPassBuilder()
        {
            using (RenderPassBuilder popup = new RenderPassBuilder())
            {
                DialogResult result = popup.ShowDialog();
                if (result == DialogResult.OK)
                {
                    // Handle the close result if necessary
                    MessageBox.Show("Popup closed.");
                }
            }
        }
        private void FillTableLayoutPanelWithImageTypes()
        {
            // Clear existing controls, if any
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.ColumnStyles.Clear();

            // Set up table layout
            tableLayoutPanel1.ColumnCount = 3; // For Value, Deprecated Name, Current Name
            tableLayoutPanel1.RowCount = Enum.GetValues(typeof(ImageType)).Length + 1; // +1 for header row

            // Header Row
            tableLayoutPanel1.Controls.Add(new Label { Text = "Value", AutoSize = true }, 0, 0);
            tableLayoutPanel1.Controls.Add(new Label { Text = "Deprecated Name", AutoSize = true }, 1, 0);
            tableLayoutPanel1.Controls.Add(new Label { Text = "Current Name", AutoSize = true }, 2, 0);

            // Populate with enum data
            foreach (ImageType imageType in Enum.GetValues(typeof(ImageType)))
            {
                int rowIndex = (int)imageType + 1; // 1-based index for the row

                // Get current name and value
                string currentName = Enum.GetName(typeof(ImageType), imageType);
                string value = ((int)imageType).ToString();

                // Get deprecated name
                string deprecatedName = null;
                var fieldInfo = typeof(ImageType).GetField(currentName);
                var obsoleteAttr = fieldInfo.GetCustomAttributes(typeof(ObsoleteAttribute), false).FirstOrDefault() as ObsoleteAttribute;
                if (obsoleteAttr != null)
                {
                    deprecatedName = obsoleteAttr.Message.Substring(obsoleteAttr.Message.IndexOf("\"") + 1).Split('"')[0]; // Extract the deprecated name from the message
                }

                // Add to TableLayoutPanel
                tableLayoutPanel1.Controls.Add(new Label { Text = value, AutoSize = true }, 0, rowIndex);
                tableLayoutPanel1.Controls.Add(new Label { Text = deprecatedName ?? "N/A", AutoSize = true }, 1, rowIndex); // Displays "N/A" if there is no deprecated name
                tableLayoutPanel1.Controls.Add(new Label { Text = currentName, AutoSize = true }, 2, rowIndex);
            }
        }
        private void ImageTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var asdf = (ImageType)Enum.Parse(typeof(ImageType), ImageTypeBox.SelectedItem.ToString());
            var asd = ImageTypeBox.SelectedItem.ToString();
        }
    }
}
